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

  private sealed class TestPrivacyKeyProvider : IDataVaultPrivacyKeyProvider {
  }

  private sealed class TestEncryptedPayloadKeyProvider : IDataVaultEncryptedPayloadKeyProvider {
    public DataVaultEncryptedPayloadConversionResult ConvertEncryptedPayload(
        DataVaultEncryptedPayloadConversionRequest request) {
      return DataVaultEncryptedPayloadConversionResult.Approved("converted:" + request.Value);
    }
  }
}
