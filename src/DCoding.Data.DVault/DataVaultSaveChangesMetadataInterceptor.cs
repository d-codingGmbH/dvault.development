using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

internal sealed class DataVaultSaveChangesMetadataInterceptor : SaveChangesInterceptor {
  private readonly DataVaultSaveChangesMetadataInterceptorOptions _options;

  public DataVaultSaveChangesMetadataInterceptor(DataVaultSaveChangesMetadataInterceptorOptions options) {
    ArgumentNullException.ThrowIfNull(options);

    _options = options;
  }

  public override InterceptionResult<int> SavingChanges(
      DbContextEventData eventData,
      InterceptionResult<int> result) {
    PopulateMissingMetadata(eventData.Context);

    return result;
  }

  public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
      DbContextEventData eventData,
      InterceptionResult<int> result,
      CancellationToken cancellationToken = default) {
    PopulateMissingMetadata(eventData.Context);

    return new ValueTask<InterceptionResult<int>>(result);
  }

  private void PopulateMissingMetadata(DbContext? dbContext) {
    if (dbContext is null) {
      return;
    }

    DateTimeOffset? loadTimestamp = null;
    string? recordSource = null;

    foreach (var entry in dbContext.ChangeTracker.Entries()) {
      if (entry.State != EntityState.Added || !IsTargetEntity(entry.Metadata)) {
        continue;
      }

      foreach (var property in entry.Metadata.GetProperties()) {
        if (!TryGetTechnicalRole(property, out var technicalRole)) {
          continue;
        }

        if (technicalRole == TechnicalMetadataColumnRole.LoadTimestamp) {
          if (HasCurrentValue(entry, property)) {
            continue;
          }

          loadTimestamp ??= _options.ResolveLoadTimestamp();
          SetCurrentValue(
              entry,
              property,
              DataVaultLoadTimestampValueConverter.ToProviderValue(property, loadTimestamp.Value));
          continue;
        }

        if (technicalRole == TechnicalMetadataColumnRole.RecordSource) {
          if (HasCurrentValue(entry, property)) {
            continue;
          }

          recordSource ??= _options.ResolveRecordSource();
          SetCurrentValue(entry, property, recordSource);
        }
      }
    }
  }

  private static bool IsTargetEntity(IEntityType entityType) {
    var entityKind = entityType.FindAnnotation(DataVaultAnnotationNames.EntityKind)?.Value;

    return entityKind is DataVaultTableKind.Hub or DataVaultTableKind.Link or DataVaultTableKind.Satellite;
  }

  private static bool TryGetTechnicalRole(IProperty property, out TechnicalMetadataColumnRole technicalRole) {
    technicalRole = default;

    if (property.FindAnnotation(DataVaultAnnotationNames.PropertyRole)?.Value is not DataVaultPropertyRole.Technical) {
      return false;
    }

    if (property.FindAnnotation(DataVaultAnnotationNames.TechnicalColumnRole)?.Value is not TechnicalMetadataColumnRole value) {
      return false;
    }

    technicalRole = value;
    return true;
  }

  private static bool HasCurrentValue(EntityEntry entry, IProperty property) {
    if (entry.Entity is IDictionary<string, object> row) {
      return row.TryGetValue(property.Name, out var value) && value is not null;
    }

    return entry.Property(property.Name).CurrentValue is not null;
  }

  private static void SetCurrentValue(EntityEntry entry, IProperty property, object value) {
    entry.Property(property.Name).CurrentValue = value;

    if (entry.Entity is IDictionary<string, object> row) {
      row[property.Name] = value;
    }
  }
}
