using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class PitAsOfReadContractSnapshotTests {
  private const string ApprovedSnapshot = """
      # DVault PIT-backed as-of read API contract fixture
      # Ticket: 06F0MEGYHADPVN575H64D56W2G
      # Status: planning-level contract target

      Baseline:
      - The public service boundary is IDataVaultReadService.
      - The contract extends the existing latest/as-of satellite projector pattern.
      - The metadata declaration is DataVaultPitMetadata.
      - Legacy DataVaultPointInTimeMetadata and DataVaultModelBuilder.PointInTime(...) are out of scope.

      Request:
        type: DataVaultPitAsOfReadRequest
        constructor:
          DataVaultPitAsOfReadRequest(
            DataVaultPitMetadata pit,
            IEnumerable<string> parentHashKeys,
            DateTimeOffset asOf)
        properties:
          Pit: DataVaultPitMetadata
          ParentHashKeys: IReadOnlyList<string>
          AsOf: DateTimeOffset
        normalization:
          parent hash keys are distinct with StringComparer.Ordinal
          parent hash keys reject null, empty, or whitespace values
          AsOf is normalized to UTC with DateTimeOffset.ToUniversalTime()
        read rule:
          resolve the latest PIT row visible at or before AsOf for each requested parent hash key
          for supported multi-active PITs, resolve one latest row per requested parent hash key and driving-key tuple

      Read service:
        raw method:
          IDataVaultReadService.ReadPitRowsAsync(
            DbContext dbContext,
            DataVaultPitAsOfReadRequest request,
            CancellationToken cancellationToken = default)
            -> Task<IReadOnlyList<DataVaultPitReadRecord>>
        projector helper:
          IDataVaultReadService.ReadPitAsync<TProjection>(
            DbContext dbContext,
            DataVaultPitAsOfReadRequest request,
            Func<DataVaultPitProjectionRow, TProjection> projector,
            CancellationToken cancellationToken = default)
            -> Task<IReadOnlyList<TProjection>>

      Raw record:
        type: DataVaultPitReadRecord
        properties:
          ParentHashKey: string
          DrivingKeyValues: IReadOnlyDictionary<string, string>
          LoadTimestamp: DateTimeOffset
          SatelliteSnapshots: IReadOnlyList<DataVaultPitSatelliteSnapshot>
          SatelliteSnapshotsByName: IReadOnlyDictionary<string, DataVaultPitSatelliteSnapshot>
        satellite segment:
          type: DataVaultPitSatelliteSnapshot
          properties:
            SatelliteName: string
            Ordinal: int
            IsPresent: bool
            SnapshotLoadTimestamp: DateTimeOffset?
            HashDiff: string?
            RecordSource: string?
            PayloadValues: IReadOnlyDictionary<string, string?>
        ordering:
          SatelliteSnapshots order exactly matches DataVaultPitMetadata.Satellites declaration order.
          SatelliteSnapshotsByName uses declared satellite names and StringComparer.Ordinal.
          DrivingKeyValues uses canonical PIT driving-key names and StringComparer.Ordinal.
        absence:
          DrivingKeyValues is empty for ordinary PIT rows.
          IsPresent=false means no snapshot segment exists for that satellite in the matched PIT row.
          Absent satellite segments do not fall back to non-PIT latest/as-of satellite reads.

      Scenario: multi-satellite typed projection
        metadata:
          hub Customer business keys [Customer Id]
          satellite Profile parent Customer payload [Customer Name, Customer Tier]
          satellite Status parent Customer payload [Status Code]
          pit Customer/Profile/Status declared satellites [Profile, Status]
        request:
          parent hash keys [customer-hash-001, customer-hash-002]
          AsOf 2026-05-11T12:00:00+00:00
        matched row for customer-hash-001:
          PIT LoadTimestamp 2026-05-11T10:00:00+00:00
          Profile SnapshotLoadTimestamp 2026-05-11T09:58:00+00:00
          Status SnapshotLoadTimestamp 2026-05-11T09:59:00+00:00
        projection:
          row.RequiredString("ParentHashKey") -> customer-hash-001
          row.RequiredDateTimeOffset("LoadTimestamp") -> 2026-05-11T10:00:00+00:00
          row.RequiredSatellite("Profile").RequiredString("Customer Name") -> Alice Adams
          row.RequiredSatellite("Profile").RequiredString("Customer Tier") -> Gold
          row.OptionalSatellite("Status")?.NullableString("Status Code") -> Active

      Scenario: multi-active tuple PIT projection
        metadata:
          hub Customer business keys [Customer Id]
          multi-active satellite Contact parent Customer driving keys [Contact Type] payload [Email Address]
          satellite Profile parent Customer payload [Customer Tier]
          pit Customer/Contact/Profile declared satellites [Contact, Profile]
        request:
          parent hash keys [customer-hash-001]
          AsOf 2026-05-11T12:00:00+00:00
        matched rows:
          customer-hash-001 Contact Type billing LoadTimestamp 2026-05-11T10:00:00+00:00
          customer-hash-001 Contact Type shipping LoadTimestamp 2026-05-11T10:05:00+00:00
        projection:
          row.RequiredString("ParentHashKey") -> customer-hash-001
          row.RequiredString("Contact Type") -> billing or shipping
          row.RequiredSatellite("Contact").RequiredString("Email Address") -> tuple-specific contact value
          row.OptionalSatellite("Profile") reads the parent-wide profile snapshot

      Scenario: missing PIT row
        request:
          parent hash keys [customer-hash-001, customer-hash-missing]
          AsOf 2026-05-11T12:00:00+00:00
        table state:
          customer-hash-001 has a PIT row visible at 2026-05-11T10:00:00+00:00
          customer-hash-missing has no PIT row visible at or before AsOf
        expected result:
          exactly one result for customer-hash-001
          no result, placeholder, null record, or exception for customer-hash-missing

      Scenario: missing satellite snapshot inside matched PIT row
        request:
          parent hash keys [customer-hash-001]
          AsOf 2026-05-11T12:00:00+00:00
        matched PIT row:
          ParentHashKey customer-hash-001
          LoadTimestamp 2026-05-11T10:00:00+00:00
          Profile snapshot 2026-05-11T09:58:00+00:00
          Status snapshot absent
        expected raw record:
          ParentHashKey customer-hash-001
          LoadTimestamp 2026-05-11T10:00:00+00:00
          SatelliteSnapshots[0].SatelliteName Profile
          SatelliteSnapshots[0].IsPresent true
          SatelliteSnapshots[1].SatelliteName Status
          SatelliteSnapshots[1].IsPresent false
          SatelliteSnapshots[1].SnapshotLoadTimestamp null
        expected projection:
          Profile values are available.
          Status is absent.
          No non-PIT latest/as-of Status row is read as fallback.

      Timestamp behavior:
        caller API uses DateTimeOffset for AsOf, PIT row LoadTimestamp, and satellite SnapshotLoadTimestamp.
        provider storage modes such as ISO 8601 UTC text or UTC ticks are internal implementation details.

      Diagnostics:
        contradictory multi-active satellite reference metadata:
          fail deterministically before row materialization.
        incompatible multi-active driving-key family:
          fail deterministically before row materialization.
        unsupported bridge-driven read:
          fail deterministically before row materialization.
        unsupported link-based PIT parent:
          fail deterministically before row materialization.
        satellite attached to a different parent:
          fail deterministically before row materialization.
        legacy DataVaultPointInTimeMetadata or PointInTime request:
          fail as out of scope for this PIT contract.
        request outside one declared DataVaultPitMetadata:
          fail as outside the bounded v1 PIT baseline.
      """;

  [Fact]
  public void PitBackedAsOfReadContractMatchesApprovedFixture() {
    var snapshotPath = GetRepositoryPath(
        "tests",
        "DCoding.Data.DVault.Tests",
        "Unit",
        "Snapshots",
        "Contracts",
        "PitBackedAsOfReadContract.approved.txt");

    var actual = NormalizeLineEndings(File.ReadAllText(snapshotPath));

    Assert.Equal(NormalizeLineEndings(ApprovedSnapshot).TrimEnd('\n') + "\n", actual);
  }

  [Fact]
  public void PlanningDocumentCarriesPitReadContractMarkers() {
    var documentPath = GetRepositoryPath(
        "docs",
        "plans",
        "pit-backed-as-of-read-api-contract.md");
    var document = NormalizeLineEndings(File.ReadAllText(documentPath));

    Assert.Contains("IDataVaultReadService", document, StringComparison.Ordinal);
    Assert.Contains("DataVaultPitAsOfReadRequest", document, StringComparison.Ordinal);
    Assert.Contains("DataVaultPitReadRecord", document, StringComparison.Ordinal);
    Assert.Contains("DataVaultPitSatelliteSnapshot", document, StringComparison.Ordinal);
    Assert.Contains("Multi-Satellite Typed Projection Example", document, StringComparison.Ordinal);
    Assert.Contains("Missing PIT Row Example", document, StringComparison.Ordinal);
    Assert.Contains("Missing Satellite Snapshot Example", document, StringComparison.Ordinal);
    Assert.Contains("DataVaultPointInTimeMetadata", document, StringComparison.Ordinal);
    Assert.Contains("bridge traversal reads as outside the PIT baseline", document, StringComparison.Ordinal);
    Assert.Contains("Provider storage modes", document, StringComparison.OrdinalIgnoreCase);
  }

  private static string GetRepositoryPath(params string[] relativePath) {
    var pathSegments = new string[relativePath.Length + 1];
    pathSegments[0] = FindRepositoryRoot();
    Array.Copy(relativePath, 0, pathSegments, 1, relativePath.Length);

    return Path.Combine(pathSegments);
  }

  private static string FindRepositoryRoot() {
    var directory = new DirectoryInfo(AppContext.BaseDirectory);

    while (directory is not null) {
      if (File.Exists(Path.Combine(directory.FullName, "DVault.slnx"))) {
        return directory.FullName;
      }

      directory = directory.Parent;
    }

    throw new InvalidOperationException("Unable to locate the DVault repository root.");
  }

  private static string NormalizeLineEndings(string value) {
    return value.Replace("\r\n", "\n", StringComparison.Ordinal);
  }
}
