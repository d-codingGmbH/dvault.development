using DCoding.Data.DVault.Privacy;
using DCoding.Data.DVault.Quickstarts.Shared;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault.SqliteQuickstart;

public sealed class SqliteQuickstartVaultContext(
    DbContextOptions<SqliteQuickstartVaultContext> options,
    IDataVaultPrivacyConfiguration privacyConfiguration) : QuickstartVaultContext(options) {
  public DbSet<CustomerProfilePrivacyProofRow> CustomerProfilePrivacyProofs =>
      Set<CustomerProfilePrivacyProofRow>();

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<CustomerProfilePrivacyProofRow>(entity => {
      entity.ToTable("CustomerProfilePrivacyProof");
      entity.HasKey(row => row.Id);
      entity.Property(row => row.CustomerBusinessKey).IsRequired();
      entity.Property(row => row.EmailAddress)
          .IsRequired()
          .HasConversion(new DataVaultEncryptedPayloadValueConverter(
              privacyConfiguration,
              SqlitePrivacyQuickstartFlow.CustomerProfileEmailEncryptedPayloadAlias));
    });
  }
}
