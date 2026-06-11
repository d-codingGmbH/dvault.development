using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

internal sealed class ExternalProviderLiveSchemaContext(
    DbContextOptions<ExternalProviderLiveSchemaContext> options,
    ExternalProviderLiveSchemaModelOptions modelOptions) : DbContext(options) {
  public ExternalProviderLiveSchemaModelOptions ModelOptions { get; } = modelOptions;

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    if (ModelOptions.DefaultSchema is not null) {
      modelBuilder.HasDefaultSchema(ModelOptions.DefaultSchema);
    }

    modelBuilder.ApplyDataVaultMetadata(
        LiveSchemaReaderContractFixture.CreateCanonicalMetadataModel(),
        ModelOptions.ProviderCapabilities);

    ConfigureProducedTable(
        modelBuilder,
        "HubCustomer",
        ["CustomerHashKey"],
        "PkHubCustomerCustomerHashKey",
        [
            new IndexOverride(
                ["CustomerId"],
                "IxHubCustomerBusinessKeyCustomerId"),
        ]);
    ConfigureProducedTable(
        modelBuilder,
        "HubOrder",
        ["OrderHashKey"],
        "PkHubOrderOrderHashKey",
        [
            new IndexOverride(
                ["OrderId"],
                "IxHubOrderBusinessKeyOrderId"),
        ]);
    ConfigureProducedTable(
        modelBuilder,
        "LinkCustomerOrder",
        ["CustomerOrderHashKey"],
        "PkLinkCustomerOrderCustomerOrderHashKey",
        [
            new IndexOverride(
                ["CustomerHashKey", "OrderHashKey"],
                "IxLinkCustomerOrderRelationshipCustomerHashKeyOrderHashKey"),
        ]);
    ConfigureProducedTable(
        modelBuilder,
        "SatCustomerContact",
        ["CustomerHashKey", "LoadTimestamp"],
        "PkSatCustomerContactCustomerHashKeyLoadTimestamp",
        [
            new IndexOverride(
                FindExpectedIndexColumns("SatCustomerContact"),
                "IxSatCustomerContactSatelliteParentCustomerHashKeyLoadTimestamp"),
        ]);
    ConfigureProducedTable(
        modelBuilder,
        "SatCustomerOrderState",
        ["CustomerOrderHashKey", "LoadTimestamp"],
        "PkSatCustomerOrderStateCustomerOrderHashKeyLoadTimestamp",
        [
            new IndexOverride(
                FindExpectedIndexColumns("SatCustomerOrderState"),
                "IxSatCustomerOrderStateSatelliteParentCustomerOrderHashKeyLoadTimestamp"),
        ]);
  }

  private void ConfigureProducedTable(
      ModelBuilder modelBuilder,
      string producedTableName,
      IReadOnlyList<string> primaryKeyColumnNames,
      string producedPrimaryKeyName,
      IReadOnlyList<IndexOverride> indexes) {
    var physicalTableName = ModelOptions.ResolveTableName(producedTableName);
    var shouldOverrideTableName = !string.Equals(physicalTableName, producedTableName, StringComparison.Ordinal);
    var shouldOverrideIdentifierNames =
        !string.Equals(
            ModelOptions.ResolveIdentifierName(producedPrimaryKeyName),
            producedPrimaryKeyName,
            StringComparison.Ordinal) ||
        indexes.Any(index => !string.Equals(
            ModelOptions.ResolveIdentifierName(index.ProducedIndexName),
            index.ProducedIndexName,
            StringComparison.Ordinal));

    if (!shouldOverrideTableName && !shouldOverrideIdentifierNames) {
      return;
    }

    modelBuilder.SharedTypeEntity<Dictionary<string, object>>(producedTableName, entity => {
      if (shouldOverrideTableName) {
        ConfigureTableName(entity, physicalTableName);
      }

      if (shouldOverrideIdentifierNames) {
        entity
            .HasKey(primaryKeyColumnNames.ToArray())
            .HasName(ModelOptions.ResolveIdentifierName(producedPrimaryKeyName));

        foreach (var index in indexes) {
          entity
              .HasIndex(index.ColumnNames.ToArray())
              .HasDatabaseName(ModelOptions.ResolveIdentifierName(index.ProducedIndexName));
        }
      }
    });
  }

  private void ConfigureTableName(
      EntityTypeBuilder<Dictionary<string, object>> entity,
      string physicalTableName) {
    if (ModelOptions.DefaultSchema is null) {
      entity.ToTable(physicalTableName);
    }
    else {
      entity.ToTable(physicalTableName, ModelOptions.DefaultSchema);
    }
  }

  private IReadOnlyList<string> FindExpectedIndexColumns(string tableName) {
    return ModelOptions.ExpectedSnapshot.Tables
        .Single(table => string.Equals(table.TableName, ModelOptions.ResolveTableName(tableName), StringComparison.Ordinal))
        .Indexes
        .Single()
        .ColumnNames;
  }

  private sealed record IndexOverride(IReadOnlyList<string> ColumnNames, string ProducedIndexName);
}
