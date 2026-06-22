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

  private sealed class TestPrivacyKeyProvider : IDataVaultPrivacyKeyProvider {
  }
}
