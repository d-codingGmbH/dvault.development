using DCoding.Data.DVault;
using DCoding.Data.DVault.Privacy;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultPrivacyServiceCollectionExtensionsTests {
  [Fact]
  public void AddDVaultPrivacyRegistersCoreDefaultsAndPrivacyConfiguration() {
    var keyProvider = new TestPrivacyKeyProvider();
    var services = new ServiceCollection();

    services.AddDVaultPrivacy(options => options
        .RegisterEncryptedPayloadAlias("CustomerProfileEmailEncrypted")
        .UseCallerOwnedKeyProvider(keyProvider));

    using var serviceProvider = services.BuildServiceProvider();
    var configuration = serviceProvider.GetRequiredService<IDataVaultPrivacyConfiguration>();

    Assert.NotNull(serviceProvider.GetRequiredService<IDataVaultSaveService>());
    Assert.Equal(["CustomerProfileEmailEncrypted"], configuration.EncryptedPayloadAliases);
    Assert.Same(keyProvider, configuration.KeyProvider);
    Assert.Same(keyProvider, serviceProvider.GetRequiredService<IDataVaultPrivacyKeyProvider>());
    Assert.NotNull(serviceProvider.GetRequiredService<IDataVaultPersonalDataCoverageProof>());
    Assert.Empty(serviceProvider.GetServices<IDataVaultProviderNativeCryptoSelectionProvider>());
    Assert.Null(serviceProvider.GetService<IDataVaultEncryptedPayloadKeyProvider>());
  }

  [Fact]
  public void AddDVaultPrivacyRejectsDuplicateEncryptedPayloadAlias() {
    var services = new ServiceCollection();

    var exception = Assert.Throws<InvalidOperationException>(() =>
        services.AddDVaultPrivacy(options => options
            .RegisterEncryptedPayloadAlias("CustomerProfileEmailEncrypted")
            .RegisterEncryptedPayloadAlias("CustomerProfileEmailEncrypted")));

    Assert.Contains("CustomerProfileEmailEncrypted", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void AddDVaultPrivacyRegistersEncryptedPayloadKeyProviderInterfaceWhenAvailable() {
    var keyProvider = new TestEncryptedPayloadKeyProvider();
    var services = new ServiceCollection();

    services.AddDVaultPrivacy(options => options.UseCallerOwnedKeyProvider(keyProvider));

    using var serviceProvider = services.BuildServiceProvider();

    Assert.Same(keyProvider, serviceProvider.GetRequiredService<IDataVaultPrivacyKeyProvider>());
    Assert.Same(keyProvider, serviceProvider.GetRequiredService<IDataVaultEncryptedPayloadKeyProvider>());
  }

  [Fact]
  public void AddDVaultSqlServerAlwaysEncryptedSelectionRegistersProviderOwnedSelectionForReviewedCapability() {
    var services = new ServiceCollection();

    services.AddDVaultPrivacy(options => options.RegisterEncryptedPayloadAlias("CustomerProfileEmailEncrypted"));
    services.AddDVaultSqlServerAlwaysEncryptedSelection(
        "CustomerProfileEmailEncrypted",
        "always-encrypted-column-key");

    using var serviceProvider = services.BuildServiceProvider();
    var configuration = serviceProvider.GetRequiredService<IDataVaultPrivacyConfiguration>();
    var selectionProvider = Assert.Single(serviceProvider.GetServices<IDataVaultProviderNativeCryptoSelectionProvider>());

    Assert.Equal(["CustomerProfileEmailEncrypted"], configuration.EncryptedPayloadAliases);
    Assert.Null(configuration.KeyProvider);

    var fact = Assert.Single(selectionProvider.Analyze(
        new DataVaultProviderNativeCryptoSelectionContext(
            "Microsoft.EntityFrameworkCore.SqlServer",
            "sqlserver-v1",
            false,
            [
                new DataVaultProviderCryptoCapabilityFact(
                    "Microsoft.EntityFrameworkCore.SqlServer",
                    "sqlserver-v1",
                    "always-encrypted",
                    "Always Encrypted",
                    "driver-mediated",
                    "conditional",
                    "redacted guidance"),
            ])));

    Assert.Equal("CustomerProfileEmailEncrypted", fact.EncryptedPayloadAlias);
    Assert.Equal("DCoding.Data.DVault.SqlServer", fact.ProviderPackageName);
    Assert.Equal("always-encrypted", fact.CapabilityFamily);
    Assert.Equal("driver-mediated", fact.CapabilityKind);
    Assert.Equal("conditional", fact.CapabilityStatus);
    Assert.Equal("provider-native-requested", fact.SelectionStatus);
  }

  [Fact]
  public void AddDVaultSqlServerAlwaysEncryptedSelectionRejectsDuplicateAlias() {
    var services = new ServiceCollection();

    services.AddDVaultSqlServerAlwaysEncryptedSelection("CustomerProfileEmailEncrypted", "column-key");
    var exception = Assert.Throws<InvalidOperationException>(() =>
        services.AddDVaultSqlServerAlwaysEncryptedSelection("CustomerProfileEmailEncrypted", "column-key"));

    Assert.Contains("CustomerProfileEmailEncrypted", exception.Message, StringComparison.Ordinal);
  }

  private sealed class TestPrivacyKeyProvider : IDataVaultPrivacyKeyProvider {
  }

  private sealed class TestEncryptedPayloadKeyProvider : IDataVaultEncryptedPayloadKeyProvider {
    public DataVaultEncryptedPayloadConversionResult ConvertEncryptedPayload(
        DataVaultEncryptedPayloadConversionRequest request) {
      return DataVaultEncryptedPayloadConversionResult.Approved("converted:" + request.Value);
    }
  }
}
