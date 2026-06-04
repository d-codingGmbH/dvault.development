using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultProviderIdentifierPreflightTests {
  [Fact]
  public void BuiltInIdentifierRulesCoverFiniteSupportedProviderBaseline() {
    var profiles = new[]
    {
        DataVaultProviderCapabilityProfiles.Sqlite,
        DataVaultProviderCapabilityProfiles.Oracle,
        DataVaultProviderCapabilityProfiles.Postgres,
        DataVaultProviderCapabilityProfiles.SqlServer,
        DataVaultProviderCapabilityProfiles.MySql,
    };

    Assert.Equal(
        ["sqlite-v1", "oracle-v1", "postgres-v1", "sqlserver-v1", "mysql-pomelo-v1"],
        profiles.Select(profile => profile.ProfileName).ToArray());

    foreach (var profile in profiles) {
      var rules = DataVaultProviderIdentifierPreflight.GetRules(profile);

      Assert.Contains("select", rules.ReservedWords);
      Assert.Contains("table", rules.ReservedWords);
      Assert.True(rules.IsValidFirstCharacter('A'));
      Assert.True(rules.IsValidSubsequentCharacter('0'));
      Assert.False(rules.IsValidFirstCharacter('0'));
      Assert.False(rules.IsValidSubsequentCharacter('-'));
    }

    Assert.Null(DataVaultProviderIdentifierPreflight.GetRules(DataVaultProviderCapabilityProfiles.Sqlite).MaximumIdentifierLength);
    Assert.Null(DataVaultProviderIdentifierPreflight.GetRules(DataVaultProviderCapabilityProfiles.Oracle).MaximumIdentifierLength);
    Assert.Null(DataVaultProviderIdentifierPreflight.GetRules(DataVaultProviderCapabilityProfiles.Postgres).MaximumIdentifierLength);
    Assert.Null(DataVaultProviderIdentifierPreflight.GetRules(DataVaultProviderCapabilityProfiles.SqlServer).MaximumIdentifierLength);
    Assert.Equal(64, DataVaultProviderIdentifierPreflight.GetRules(DataVaultProviderCapabilityProfiles.MySql).MaximumIdentifierLength);
  }

  [Fact]
  public void ReservedWordsAreProjectedWithObjectClassSuffixes() {
    var result = DataVaultProviderIdentifierPreflight.Analyze(
        DataVaultProviderCapabilityProfiles.Sqlite,
        [
            Candidate(DataVaultProviderIdentifierKind.Table, "Select", "metadata/hubs/Select/table"),
            Candidate(DataVaultProviderIdentifierKind.Column, "Order", "metadata/hubs/Select/columns/Order"),
        ]);

    Assert.Empty(result.Issues);
    AssertProjection(result, "metadata/hubs/Select/table", "SelectTable", isDerived: true);
    AssertProjection(result, "metadata/hubs/Select/columns/Order", "OrderColumn", isDerived: true);
  }

  [Fact]
  public void LengthLimitedIdentifiersUseDeterministicHashProjection() {
    var logicalName = "IxSatCustomerContactExtremelyVerboseProviderIdentifierPreflightPayloadColumnNameLoadTimestamp";

    var result = DataVaultProviderIdentifierPreflight.Analyze(
        DataVaultProviderCapabilityProfiles.MySql,
        [Candidate(DataVaultProviderIdentifierKind.Index, logicalName, "metadata/satellite/Contact/indexes/" + logicalName)]);

    Assert.Empty(result.Issues);
    var projection = Assert.Single(result.ProjectionSet.Projections);

    Assert.True(projection.IsDerived);
    Assert.Equal(logicalName, projection.Candidate.LogicalName);
    Assert.Equal(64, projection.PhysicalName.Length);
    Assert.StartsWith(logicalName[..55], projection.PhysicalName, StringComparison.Ordinal);
    Assert.Equal(8, projection.PhysicalName.Split('_').Last().Length);
    Assert.All(projection.PhysicalName.Split('_').Last(), character => Assert.True(IsLowerHex(character)));
  }

  [Fact]
  public void HashProjectionExpandsSuffixWhenInitialProjectionCollides() {
    var first = "CollisionCandidateWithVeryLongSharedPrefix069916";
    var second = "CollisionCandidateWithVeryLongSharedPrefix088042";
    var profile = CreateMySqlProfileWithMaximumIdentifierLength(43);

    var result = DataVaultProviderIdentifierPreflight.Analyze(
        profile,
        [
            Candidate(DataVaultProviderIdentifierKind.Index, first, "metadata/hubs/Customer/indexes/first"),
            Candidate(DataVaultProviderIdentifierKind.Index, second, "metadata/hubs/Customer/indexes/second"),
        ]);

    Assert.Empty(result.Issues);
    var physicalNames = result.ProjectionSet.Projections
        .Select(projection => projection.PhysicalName)
        .Order(StringComparer.Ordinal)
        .ToArray();

    Assert.Equal(2, physicalNames.Length);
    Assert.NotEqual(physicalNames[0], physicalNames[1]);
    Assert.All(physicalNames, physicalName => {
      Assert.Equal(43, physicalName.Length);
      Assert.Equal(12, physicalName.Split('_').Last().Length);
      Assert.All(physicalName.Split('_').Last(), character => Assert.True(IsLowerHex(character)));
    });
  }

  [Fact]
  public void DuplicateProducedNamesInOneProviderVisibleScopeAreBlockingIssues() {
    var result = DataVaultProviderIdentifierPreflight.Analyze(
        DataVaultProviderCapabilityProfiles.Postgres,
        [
            Candidate(DataVaultProviderIdentifierKind.Column, "CustomerId", "metadata/hubs/Customer/columns/first"),
            Candidate(DataVaultProviderIdentifierKind.Column, "CustomerId", "metadata/hubs/Customer/columns/second"),
        ]);

    Assert.Equal(2, result.Issues.Count);
    Assert.All(result.Issues, issue => {
      Assert.Equal("DVM2009", DataVaultProviderIdentifierPreflight.CreateDiagnosticIssue(issue).Code);
      Assert.Equal("duplicate-produced-name", issue.FailureClass);
      Assert.Equal(DataVaultProviderIdentifierKind.Column, issue.Kind);
      Assert.Equal("CustomerId", issue.LogicalName);
      Assert.Equal("postgres-v1", issue.ProviderProfileName);
    });
  }

  private static DataVaultProviderIdentifierCandidate Candidate(
      DataVaultProviderIdentifierKind kind,
      string logicalName,
      string path) {
    return new DataVaultProviderIdentifierCandidate(kind, logicalName, logicalName, "scope", path);
  }

  private static void AssertProjection(
      DataVaultProviderIdentifierPreflightResult result,
      string path,
      string expectedPhysicalName,
      bool isDerived) {
    var projection = result.ProjectionSet.Projections.Single(projection =>
        string.Equals(projection.Candidate.Path, path, StringComparison.Ordinal));

    Assert.Equal(expectedPhysicalName, projection.PhysicalName);
    Assert.Equal(isDerived, projection.IsDerived);
  }

  private static DataVaultProviderCapabilityProfile CreateMySqlProfileWithMaximumIdentifierLength(
      int maximumIdentifierLength) {
    return new DataVaultProviderCapabilityProfile(
        "mysql-pomelo-v1-test",
        DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported,
        DataVaultProviderConcurrencySupport.NoneInV1Unsupported,
        DataVaultProviderCapabilityProfiles.MySql.TypeMappings,
        maximumIdentifierLength,
        unsupportedIncludedIndexColumnMode: DataVaultUnsupportedIncludedIndexColumnMode.Ignore);
  }

  private static bool IsLowerHex(char character) {
    return character is >= '0' and <= '9' or >= 'a' and <= 'f';
  }
}
