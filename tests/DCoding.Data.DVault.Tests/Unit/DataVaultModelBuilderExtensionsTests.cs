using System.Reflection;
using System.Runtime.CompilerServices;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultModelBuilderExtensionsTests {
  private const string ConventionsAnnotationName = "DCoding.Data.DVault:Conventions";

  [Fact]
  public void UseDataVaultIsRootNamespaceOptionlessModelBuilderExtension() {
    var method = typeof(DCoding.Data.DVault.DataVaultModelBuilderExtensions)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(methodInfo =>
            methodInfo.Name == "UseDataVault" &&
            methodInfo.GetParameters().Length == 1 &&
            methodInfo.GetParameters()[0].ParameterType == typeof(ModelBuilder));
    var parameter = method.GetParameters()[0];

    Assert.Equal("DCoding.Data.DVault", method.DeclaringType?.Namespace);
    Assert.Equal(typeof(ModelBuilder), parameter.ParameterType);
    Assert.Equal(typeof(ModelBuilder), method.ReturnType);
    Assert.True(method.IsDefined(typeof(ExtensionAttribute), inherit: false));
  }

  [Fact]
  public void UseDataVaultRejectsNullModelBuilder() {
    ModelBuilder? modelBuilder = null;

    var exception = Assert.Throws<ArgumentNullException>(() => modelBuilder!.UseDataVault());

    Assert.Equal("modelBuilder", exception.ParamName);
  }

  [Fact]
  public void UseDataVaultReturnsSameBuilderAndStoresDefaultConventionsAnnotation() {
    var modelBuilder = CreateModelBuilder();

    var result = modelBuilder.UseDataVault();

    Assert.Same(modelBuilder, result);

    var annotation = modelBuilder.Model.FindAnnotation(ConventionsAnnotationName);

    Assert.NotNull(annotation);
    Assert.Same(DataVaultConventions.Default, annotation.Value);
    Assert.Equal("default", Assert.IsType<DataVaultConventions>(annotation.Value).ProfileName);
    Assert.Equal(
        "sqlite-v1",
        Assert.IsType<string>(modelBuilder.Model.FindAnnotation(DataVaultAnnotationNames.ProviderProfile)?.Value));
  }

  [Fact]
  public void UseDataVaultBinaryFirstProfileReturnsSameBuilderAndStoresNamedConventionsAnnotation() {
    var modelBuilder = CreateModelBuilder();

    var result = modelBuilder.UseDataVaultBinaryFirstProfile();

    Assert.Same(modelBuilder, result);

    var conventions = Assert.IsType<DataVaultConventions>(
        modelBuilder.Model.FindAnnotation(DataVaultAnnotationNames.Conventions)?.Value);

    Assert.Equal("sha256-v1", conventions.StableHashAlgorithmId);
    Assert.Equal(32, conventions.StableHashDigestByteLength);
    Assert.Equal(DataVaultHashKeyStorageProfile.Binary, conventions.HashKeyStorageProfile);
    Assert.Equal("binary-first", conventions.ProfileName);
    Assert.Equal(
        "sqlite-v1",
        Assert.IsType<string>(modelBuilder.Model.FindAnnotation(DataVaultAnnotationNames.ProviderProfile)?.Value));
  }

  [Fact]
  public void UseDataVaultWithProviderProfileReturnsSameBuilderAndStoresSelectedProfileAnnotation() {
    var modelBuilder = CreateModelBuilder();

    var result = modelBuilder.UseDataVault(DataVaultProviderCapabilityProfiles.Oracle);

    Assert.Same(modelBuilder, result);
    Assert.Same(DataVaultConventions.Default, modelBuilder.Model.FindAnnotation(DataVaultAnnotationNames.Conventions)?.Value);
    Assert.Equal(
        "oracle-v1",
        Assert.IsType<string>(modelBuilder.Model.FindAnnotation(DataVaultAnnotationNames.ProviderProfile)?.Value));
  }

  [Fact]
  public void UseDataVaultWithProviderProfileRejectsNullArguments() {
    ModelBuilder? modelBuilder = null;

    var modelBuilderException = Assert.Throws<ArgumentNullException>(() =>
        modelBuilder!.UseDataVault(DataVaultProviderCapabilityProfiles.Oracle));
    var profileException = Assert.Throws<ArgumentNullException>(() =>
        CreateModelBuilder().UseDataVault(null!));

    Assert.Equal("modelBuilder", modelBuilderException.ParamName);
    Assert.Equal("providerCapabilities", profileException.ParamName);
  }

  [Fact]
  public void UseDataVaultBinaryFirstProfileRejectsNullArguments() {
    ModelBuilder? modelBuilder = null;

    var modelBuilderException = Assert.Throws<ArgumentNullException>(() =>
        modelBuilder!.UseDataVaultBinaryFirstProfile(DataVaultProviderCapabilityProfiles.Oracle));
    var profileException = Assert.Throws<ArgumentNullException>(() =>
        CreateModelBuilder().UseDataVaultBinaryFirstProfile(null!));

    Assert.Equal("modelBuilder", modelBuilderException.ParamName);
    Assert.Equal("providerCapabilities", profileException.ParamName);
  }

  [Fact]
  public void BareUseDataVaultDoesNotCreateEntityPropertyKeyOrIndexMetadata() {
    var modelBuilder = CreateModelBuilder();

    modelBuilder.UseDataVault();

    Assert.Empty(modelBuilder.Model.GetEntityTypes());
  }

  private static ModelBuilder CreateModelBuilder() {
    return new ModelBuilder(new ConventionSet());
  }
}
