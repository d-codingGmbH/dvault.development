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
