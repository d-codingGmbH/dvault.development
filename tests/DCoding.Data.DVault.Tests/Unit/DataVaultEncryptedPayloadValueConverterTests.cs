using System.Text;
using DCoding.Data.DVault.Privacy;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultEncryptedPayloadValueConverterTests {
  private const string CustomerEmailAlias = "CustomerProfileEmailEncrypted";
  private const string CustomerBusinessKey = "C-100";
  private const string CustomerEmailAddress = "alice@example.test";

  [Fact]
  public async Task ExplicitConverterPersistsEncryptedProviderValueThroughSqliteAndRoundTrips() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var configuration = CreateConfiguration(new TestPrivacyKeyProvider(), CustomerEmailAlias);
    var options = new DbContextOptionsBuilder<EncryptedPayloadProofContext>()
        .UseSqlite(CreateConnectionString(database))
        .Options;

    await using (var context = new EncryptedPayloadProofContext(options, configuration)) {
      await context.Database.EnsureCreatedAsync();

      context.CustomerProfiles.Add(new CustomerProfilePrivacyProofRow {
        CustomerBusinessKey = CustomerBusinessKey,
        EmailAddress = CustomerEmailAddress,
      });

      await context.SaveChangesAsync();
    }

    using (var connection = database.CreateOpenConnection()) {
      var providerValue = Assert.IsType<string>(
          connection.ExecuteScalarString("SELECT EmailAddress FROM CustomerProfilePrivacyProof LIMIT 1;"));

      Assert.NotEqual(CustomerEmailAddress, providerValue);
      Assert.StartsWith("test-encrypted:" + CustomerEmailAlias + ":", providerValue, StringComparison.Ordinal);
    }

    await using (var context = new EncryptedPayloadProofContext(options, configuration)) {
      var row = await context.CustomerProfiles.AsNoTracking().SingleAsync();

      Assert.Equal(CustomerBusinessKey, row.CustomerBusinessKey);
      Assert.Equal(CustomerEmailAddress, row.EmailAddress);
    }
  }

  [Fact]
  public void ExplicitConverterRejectsUnregisteredAliasBeforePlaintextCanBeStored() {
    var configuration = CreateConfiguration(new TestPrivacyKeyProvider());

    var exception = Assert.Throws<InvalidOperationException>(() =>
        new DataVaultEncryptedPayloadValueConverter(configuration, CustomerEmailAlias));

    Assert.Contains(CustomerEmailAlias, exception.Message, StringComparison.Ordinal);
    Assert.Contains("not registered", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void ExplicitConverterRejectsMissingKeyProviderBeforePlaintextCanBeStored() {
    var configuration = CreateConfiguration(keyProvider: null, CustomerEmailAlias);

    var exception = Assert.Throws<InvalidOperationException>(() =>
        new DataVaultEncryptedPayloadValueConverter(configuration, CustomerEmailAlias));

    Assert.Contains(CustomerEmailAlias, exception.Message, StringComparison.Ordinal);
    Assert.Contains("key provider", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void ExplicitConverterFailsClosedWhenCallerDeclinesConversion() {
    var configuration = CreateConfiguration(new DecliningPrivacyKeyProvider(), CustomerEmailAlias);
    var converter = new DataVaultEncryptedPayloadValueConverter(configuration, CustomerEmailAlias);
    var convertToProvider = converter.ConvertToProviderExpression.Compile();

    var exception = Assert.Throws<InvalidOperationException>(() => convertToProvider(CustomerEmailAddress));

    Assert.Contains(CustomerEmailAlias, exception.Message, StringComparison.Ordinal);
    Assert.Contains("key-unavailable", exception.Message, StringComparison.Ordinal);
    Assert.DoesNotContain(CustomerEmailAddress, exception.Message, StringComparison.Ordinal);
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

  private static string CreateConnectionString(SqliteTestDatabase database) {
    return "Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False";
  }

  private sealed class EncryptedPayloadProofContext(
      DbContextOptions<EncryptedPayloadProofContext> options,
      IDataVaultPrivacyConfiguration privacyConfiguration) : DbContext(options) {
    public DbSet<CustomerProfilePrivacyProofRow> CustomerProfiles => Set<CustomerProfilePrivacyProofRow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<CustomerProfilePrivacyProofRow>(entity => {
        entity.ToTable("CustomerProfilePrivacyProof");
        entity.HasKey(row => row.Id);
        entity.Property(row => row.CustomerBusinessKey).IsRequired();
        entity.Property(row => row.EmailAddress)
            .IsRequired()
            .HasConversion(new DataVaultEncryptedPayloadValueConverter(
                privacyConfiguration,
                CustomerEmailAlias));
      });
    }
  }

  private sealed class CustomerProfilePrivacyProofRow {
    public long Id { get; set; }

    public string CustomerBusinessKey { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;
  }

  private sealed class TestPrivacyKeyProvider : IDataVaultEncryptedPayloadKeyProvider {
    public DataVaultEncryptedPayloadConversionResult ConvertEncryptedPayload(
        DataVaultEncryptedPayloadConversionRequest request) {
      return request.Direction switch {
        DataVaultEncryptedPayloadConversionDirection.Encrypt => DataVaultEncryptedPayloadConversionResult.Approved(
            "test-encrypted:" +
            request.EncryptedPayloadAlias +
            ":" +
            Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Value))),
        DataVaultEncryptedPayloadConversionDirection.Decrypt => Decrypt(request),
        _ => DataVaultEncryptedPayloadConversionResult.Declined("unsupported-conversion-direction"),
      };
    }

    private static DataVaultEncryptedPayloadConversionResult Decrypt(
        DataVaultEncryptedPayloadConversionRequest request) {
      var prefix = "test-encrypted:" + request.EncryptedPayloadAlias + ":";
      if (!request.Value.StartsWith(prefix, StringComparison.Ordinal)) {
        return DataVaultEncryptedPayloadConversionResult.Declined("alias-mismatch");
      }

      var providerPayload = request.Value[prefix.Length..];
      return DataVaultEncryptedPayloadConversionResult.Approved(
          Encoding.UTF8.GetString(Convert.FromBase64String(providerPayload)));
    }
  }

  private sealed class DecliningPrivacyKeyProvider : IDataVaultEncryptedPayloadKeyProvider {
    public DataVaultEncryptedPayloadConversionResult ConvertEncryptedPayload(
        DataVaultEncryptedPayloadConversionRequest request) {
      return DataVaultEncryptedPayloadConversionResult.Declined("key-unavailable");
    }
  }
}
