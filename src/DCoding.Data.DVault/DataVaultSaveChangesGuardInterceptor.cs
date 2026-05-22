using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

internal sealed class DataVaultSaveChangesGuardInterceptor : SaveChangesInterceptor {
  private readonly DataVaultSaveChangesGuardOptions _options;

  public DataVaultSaveChangesGuardInterceptor(DataVaultSaveChangesGuardOptions options) {
    ArgumentNullException.ThrowIfNull(options);

    _options = options;
  }

  public override InterceptionResult<int> SavingChanges(
      DbContextEventData eventData,
      InterceptionResult<int> result) {
    Evaluate(eventData.Context);

    return result;
  }

  public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
      DbContextEventData eventData,
      InterceptionResult<int> result,
      CancellationToken cancellationToken = default) {
    Evaluate(eventData.Context);

    return new ValueTask<InterceptionResult<int>>(result);
  }

  internal void Evaluate(DbContext? dbContext) {
    if (dbContext is null) {
      return;
    }

    var findings = dbContext.ChangeTracker
        .Entries()
        .Select(CreateFinding)
        .Where(finding => finding is not null)
        .Cast<DataVaultSaveChangesGuardFinding>()
        .OrderBy(finding => finding.TableName, StringComparer.Ordinal)
        .ThenBy(finding => finding.MetadataName, StringComparer.Ordinal)
        .ThenBy(finding => finding.EntityKind)
        .ThenBy(finding => finding.State)
        .ThenBy(finding => string.Join("|", finding.Reasons), StringComparer.Ordinal)
        .ToArray();

    _options.HandleReport(new DataVaultSaveChangesGuardReport(findings));
  }

  private static DataVaultSaveChangesGuardFinding? CreateFinding(EntityEntry entry) {
    if (!TryGetTargetEntityKind(entry.Metadata, out var entityKind) ||
        entry.State is not (EntityState.Added or EntityState.Modified or EntityState.Deleted)) {
      return null;
    }

    var reasons = entry.State switch {
      EntityState.Modified or EntityState.Deleted => [CreateUnsafeStateReason(entityKind, entry.State)],
      EntityState.Added => GetMissingStructuralValueReasons(entry).ToArray(),
      _ => [],
    };

    return reasons.Length == 0
        ? null
        : new DataVaultSaveChangesGuardFinding(
            GetTableName(entry.Metadata),
            entityKind,
            GetMetadataName(entry.Metadata),
            entry.State,
            reasons);
  }

  private static bool TryGetTargetEntityKind(IEntityType entityType, out DataVaultTableKind entityKind) {
    entityKind = default;

    if (entityType.FindAnnotation(DataVaultAnnotationNames.EntityKind)?.Value is not DataVaultTableKind value ||
        value is not (DataVaultTableKind.Hub or DataVaultTableKind.Link or DataVaultTableKind.Satellite)) {
      return false;
    }

    entityKind = value;
    return true;
  }

  private static string CreateUnsafeStateReason(DataVaultTableKind entityKind, EntityState state) {
    return "Tracked state '" +
        state +
        "' is unsafe for generated Data Vault " +
        entityKind +
        " rows; use IDataVaultSaveService or an explicit caller-owned append-only workflow.";
  }

  private static IEnumerable<string> GetMissingStructuralValueReasons(EntityEntry entry) {
    return entry.Metadata
        .GetProperties()
        .Where(IsRequiredAddedStructuralProperty)
        .Where(property => !HasCurrentValue(entry, property))
        .OrderBy(GetPropertyOrdinal)
        .ThenBy(GetPropertyName, StringComparer.Ordinal)
        .Select(property => "Required structural property '" + GetPropertyName(property) + "' is missing.");
  }

  private static bool IsRequiredAddedStructuralProperty(IProperty property) {
    var propertyRole = property.FindAnnotation(DataVaultAnnotationNames.PropertyRole)?.Value;
    if (propertyRole is DataVaultPropertyRole.ParticipantReference or DataVaultPropertyRole.DrivingKey) {
      return true;
    }

    return propertyRole is DataVaultPropertyRole.Technical &&
        property.FindAnnotation(DataVaultAnnotationNames.TechnicalColumnRole)?.Value
            is TechnicalMetadataColumnRole.HashKey or TechnicalMetadataColumnRole.HashDiff;
  }

  private static bool HasCurrentValue(EntityEntry entry, IProperty property) {
    var value = GetCurrentValue(entry, property);

    return value switch {
      null => false,
      string stringValue => !string.IsNullOrWhiteSpace(stringValue),
      _ => true,
    };
  }

  private static object? GetCurrentValue(EntityEntry entry, IProperty property) {
    if (entry.Entity is IDictionary<string, object> row) {
      return row.TryGetValue(property.Name, out var value) ? value : null;
    }

    return entry.Property(property.Name).CurrentValue;
  }

  private static string GetTableName(IEntityType entityType) {
    return entityType.GetTableName() ??
        (entityType.FindAnnotation(DataVaultAnnotationNames.ProducedName)?.Value as string) ??
        entityType.Name;
  }

  private static string GetMetadataName(IEntityType entityType) {
    return (entityType.FindAnnotation(DataVaultAnnotationNames.MetadataName)?.Value as string) ??
        entityType.ShortName();
  }

  private static string GetPropertyName(IProperty property) {
    return (property.FindAnnotation(DataVaultAnnotationNames.ProducedName)?.Value as string) ??
        property.GetColumnName() ??
        property.Name;
  }

  private static int GetPropertyOrdinal(IProperty property) {
    return property.FindAnnotation(DataVaultAnnotationNames.Ordinal)?.Value is int ordinal
        ? ordinal
        : int.MaxValue;
  }
}
