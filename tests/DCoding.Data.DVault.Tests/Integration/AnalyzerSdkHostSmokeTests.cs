using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class AnalyzerSdkHostSmokeTests {
#if NET8_0
  [Fact]
  public void Net8ConsumerTargetCompilesGeneratedMapperOutputFromNet10AnalyzerAsset() {
    AssertGeneratedMapperOutput();
  }
#endif

#if NET10_0
  [Fact]
  public void Net10ConsumerTargetCompilesGeneratedMapperOutputFromNet10AnalyzerAsset() {
    AssertGeneratedMapperOutput();
  }
#endif

  private static void AssertGeneratedMapperOutput() {
    var mapper = GeneratedCustomerSourceDataVaultHubMapping.CreateMapper();
    var operation = mapper.Map(new GeneratedCustomerSource("C-SDK-HOST", "DE"));

    Assert.Equal("Customer", operation.HubName);
    Assert.Equal("C-SDK-HOST", operation.BusinessKeyValues["Customer Id"]);
    Assert.Equal("DE", operation.BusinessKeyValues["Region Code"]);
  }
}
