using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault.Tests.Shared;

public static class ProviderSqlExecutionContract {
  public static DataVaultMetadataModel CreateMetadataModel() {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["Customer Id"])],
        [],
        []);
  }

  public static void ApplyModel(ModelBuilder modelBuilder) {
    ArgumentNullException.ThrowIfNull(modelBuilder);

    modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel());
    modelBuilder.Entity<ProviderSqlExecutionContractTrackedEntity>(entity => {
      entity.ToTable("ProviderSqlExecutionContractTrackedEntities");
      entity.HasKey(value => value.Id);
      entity.Property(value => value.Name).IsRequired();
    });
  }
}
