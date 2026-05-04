using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
public sealed class SqlServerBatchScriptTests {
  [Fact]
  public void SplitBatchesReturnsSingleBatchWhenScriptHasNoBatchTerminator() {
    var batches = SqlServerBatchScript.SplitBatches(
        "CREATE TABLE [dbo].[Example] ([Id] int NOT NULL);\n");

    Assert.Equal(
        ["CREATE TABLE [dbo].[Example] ([Id] int NOT NULL);"],
        batches);
  }

  [Fact]
  public void SplitBatchesSplitsSqlServerGoLinesWithoutKeepingTerminators() {
    var batches = SqlServerBatchScript.SplitBatches(
        "CREATE SCHEMA [dvault_test];\n" +
        "GO\n" +
        "CREATE TABLE [dvault_test].[HubCustomer] ([CustomerHashKey] nvarchar(64) NOT NULL);\n" +
        "  go  \n" +
        "\n" +
        "GO\n");

    Assert.Equal(
        [
            "CREATE SCHEMA [dvault_test];",
            "CREATE TABLE [dvault_test].[HubCustomer] ([CustomerHashKey] nvarchar(64) NOT NULL);",
        ],
        batches);
  }

  [Fact]
  public void SplitBatchesDoesNotSplitGoInsideSqlStatements() {
    var batches = SqlServerBatchScript.SplitBatches(
        "INSERT INTO [dbo].[Example] ([Value]) VALUES (N'GO');\n");

    Assert.Equal(
        ["INSERT INTO [dbo].[Example] ([Value]) VALUES (N'GO');"],
        batches);
  }
}
