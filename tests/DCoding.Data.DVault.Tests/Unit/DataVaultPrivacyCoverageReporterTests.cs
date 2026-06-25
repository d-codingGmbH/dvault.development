using DCoding.Data.DVault.Privacy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultPrivacyCoverageReporterTests {
  private const string CustomerEmailAlias = "CustomerProfileEmailEncrypted";
  private const string CustomerPhoneAlias = "CustomerProfilePhoneEncrypted";
  private const string PostureAlias = "PostureEncrypted";

  [Fact]
  public void AnalyzeReportsCoveredAndRegisteredButUnmappedAliasesWithStableDisplay() {
    var keyProvider = new CountingEncryptedPayloadKeyProvider();
    var configuration = CreateConfiguration(keyProvider, CustomerPhoneAlias, CustomerEmailAlias);
    var options = new DbContextOptionsBuilder<CoverageContext>()
        .UseSqlite("Data Source=:memory:")
        .Options;

    using var context = new CoverageContext(options, configuration);
    var report = DataVaultPrivacyCoverageReporter.Analyze(configuration, context);
    var modelReport = DataVaultPrivacyCoverageReporter.Analyze(configuration, context.Model);
    var entityTypeName = typeof(CustomerProfileCoverageRow).FullName!;

    Assert.Equal(DataVaultPrivacyKeyProviderPosture.EncryptedPayloadCapable, report.KeyProviderPosture);
    Assert.True(report.HasUnmappedAliases);
    Assert.Equal(report.ToDisplayString(), modelReport.ToDisplayString());
    Assert.Equal(0, keyProvider.ConversionCallCount);

    Assert.Collection(
        report.AliasCoverages,
        emailCoverage => {
          Assert.Equal(CustomerEmailAlias, emailCoverage.EncryptedPayloadAlias);
          Assert.Equal(DataVaultPrivacyAliasCoverageStatus.Covered, emailCoverage.Status);
          Assert.Equal(
              ["BackupEmailAddress", "EmailAddress"],
              emailCoverage.CoveredProperties.Select(property => property.PropertyName));
          Assert.All(
              emailCoverage.CoveredProperties,
              property => Assert.Equal(entityTypeName, property.EntityTypeName));
        },
        phoneCoverage => {
          Assert.Equal(CustomerPhoneAlias, phoneCoverage.EncryptedPayloadAlias);
          Assert.Equal(DataVaultPrivacyAliasCoverageStatus.RegisteredButUnmapped, phoneCoverage.Status);
          Assert.Empty(phoneCoverage.CoveredProperties);
        });

    Assert.Equal(
        string.Join(
            Environment.NewLine,
            "DVault privacy coverage: aliases 2, covered 1, registered-but-unmapped 1, key provider encrypted-payload-capable.",
            "- covered " + CustomerEmailAlias + ": " + entityTypeName + ".BackupEmailAddress, " + entityTypeName + ".EmailAddress",
            "- registered-but-unmapped " + CustomerPhoneAlias + ": no mapped properties use DataVaultEncryptedPayloadValueConverter for this alias."),
        report.ToDisplayString());
  }

  [Fact]
  public void AnalyzeClassifiesKeyProviderPosturesWithoutConversionCalls() {
    var keyProvider = new CountingEncryptedPayloadKeyProvider();
    var options = new DbContextOptionsBuilder<EmptyCoverageContext>()
        .UseSqlite("Data Source=:memory:")
        .Options;

    using var context = new EmptyCoverageContext(options);

    AssertPosture(
        CreateConfiguration(keyProvider: null, PostureAlias),
        context.Model,
        DataVaultPrivacyKeyProviderPosture.None,
        "key provider none");
    AssertPosture(
        CreateConfiguration(new MarkerOnlyPrivacyKeyProvider(), PostureAlias),
        context.Model,
        DataVaultPrivacyKeyProviderPosture.MarkerOnly,
        "key provider marker-only");
    AssertPosture(
        CreateConfiguration(keyProvider, PostureAlias),
        context.Model,
        DataVaultPrivacyKeyProviderPosture.EncryptedPayloadCapable,
        "key provider encrypted-payload-capable");

    Assert.Equal(0, keyProvider.ConversionCallCount);
  }

  [Fact]
  public void EncryptedPayloadValueConverterExposesAliasForCoverageReporting() {
    var keyProvider = new CountingEncryptedPayloadKeyProvider();
    var configuration = CreateConfiguration(keyProvider, CustomerEmailAlias);

    var converter = new DataVaultEncryptedPayloadValueConverter(configuration, CustomerEmailAlias);

    Assert.Equal(CustomerEmailAlias, converter.EncryptedPayloadAlias);
    Assert.Equal(0, keyProvider.ConversionCallCount);
  }

  private static void AssertPosture(
      IDataVaultPrivacyConfiguration configuration,
      IModel model,
      DataVaultPrivacyKeyProviderPosture expectedPosture,
      string expectedDisplayText) {
    var report = DataVaultPrivacyCoverageReporter.Analyze(configuration, model);

    Assert.Equal(expectedPosture, report.KeyProviderPosture);
    Assert.Single(report.AliasCoverages);
    Assert.Equal(DataVaultPrivacyAliasCoverageStatus.RegisteredButUnmapped, report.AliasCoverages[0].Status);
    Assert.Contains(expectedDisplayText, report.ToDisplayString(), StringComparison.Ordinal);
  }

  private static IDataVaultPrivacyConfiguration CreateConfiguration(
      IDataVaultPrivacyKeyProvider? keyProvider,
      params string[] encryptedPayloadAliases) {
    var services = new ServiceCollection();
    services.AddDVaultPrivacy(options => {
      foreach (var encryptedPayloadAlias in encryptedPayloadAliases) {
        options.RegisterEncryptedPayloadAlias(encryptedPayloadAlias);
      }

      if (keyProvider is not null) {
        options.UseCallerOwnedKeyProvider(keyProvider);
      }
    });

    using var serviceProvider = services.BuildServiceProvider();
    return serviceProvider.GetRequiredService<IDataVaultPrivacyConfiguration>();
  }

  private sealed class CoverageContext(
      DbContextOptions<CoverageContext> options,
      IDataVaultPrivacyConfiguration privacyConfiguration) : DbContext(options) {
    public DbSet<CustomerProfileCoverageRow> CustomerProfiles => Set<CustomerProfileCoverageRow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<CustomerProfileCoverageRow>(entity => {
        entity.HasKey(row => row.Id);
        entity.Property(row => row.CustomerBusinessKey).IsRequired();
        entity.Property(row => row.EmailAddress)
            .IsRequired()
            .HasConversion(new DataVaultEncryptedPayloadValueConverter(
                privacyConfiguration,
                CustomerEmailAlias));
        entity.Property(row => row.BackupEmailAddress)
            .IsRequired()
            .HasConversion(new DataVaultEncryptedPayloadValueConverter(
                privacyConfiguration,
                CustomerEmailAlias));
        entity.Property(row => row.PhoneNumber).IsRequired();
      });
    }
  }

  private sealed class EmptyCoverageContext(DbContextOptions<EmptyCoverageContext> options) : DbContext(options) {
    public DbSet<UncoveredCoverageRow> Rows => Set<UncoveredCoverageRow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<UncoveredCoverageRow>(entity => {
        entity.HasKey(row => row.Id);
        entity.Property(row => row.Note).IsRequired();
      });
    }
  }

  private sealed class CustomerProfileCoverageRow {
    public long Id { get; set; }

    public string CustomerBusinessKey { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string BackupEmailAddress { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;
  }

  private sealed class UncoveredCoverageRow {
    public long Id { get; set; }

    public string Note { get; set; } = string.Empty;
  }

  private sealed class MarkerOnlyPrivacyKeyProvider : IDataVaultPrivacyKeyProvider {
  }

  private sealed class CountingEncryptedPayloadKeyProvider : IDataVaultEncryptedPayloadKeyProvider {
    public int ConversionCallCount { get; private set; }

    public DataVaultEncryptedPayloadConversionResult ConvertEncryptedPayload(
        DataVaultEncryptedPayloadConversionRequest request) {
      ConversionCallCount++;
      return DataVaultEncryptedPayloadConversionResult.Approved("converted:" + request.Value);
    }
  }
}
