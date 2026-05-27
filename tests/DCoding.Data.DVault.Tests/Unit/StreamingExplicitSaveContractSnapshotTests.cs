using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class StreamingExplicitSaveContractSnapshotTests {
  private const string ApprovedSnapshot = """
      # DVault streaming explicit-save API contract fixture
      # Ticket: 06F5Q8X261DQHG7N1445NGXB5W
      # Status: contract target

      Baseline:
      - The public service boundary is IDataVaultSaveService.
      - Existing SaveAsync(DbContext, DataVaultSaveRequest, ...) semantics remain valid.
      - Existing SaveAsync(DbContext, DataVaultBulkSaveRequest, ...) semantics remain valid.
      - The chunked contract is additive and does not replace the existing single-request or ordered-bulk requests.
      - DataVaultChunkedSaveRequest is the named contract target for the follow-on implementation story.
      - Provider strategies keep the ordered DataVaultProviderSaveStrategyContext.Requests and ResolvedRequests batch contract.

      Request:
        type: DataVaultChunkedSaveRequest
        service method:
          IDataVaultSaveService.SaveAsync(
            DbContext dbContext,
            DataVaultChunkedSaveRequest request,
            CancellationToken cancellationToken = default)
            -> Task<DataVaultSaveResult>
        chunk type: DataVaultSaveChunk
        chunk shape:
          DataVaultChunkedSaveRequest.Chunks: IEnumerable<DataVaultSaveChunk>
          DataVaultSaveChunk.Requests: IReadOnlyList<DataVaultSaveRequest>
        input rule:
          chunks are supplied as an ordered sequence
          each chunk is bounded and finite
          each contained DataVaultSaveRequest uses existing validation rules
          empty chunk sequence and empty chunk are no-ops

      Ordering:
        chunks are processed in caller order
        requests inside a chunk are processed in caller order
        operations inside each DataVaultSaveRequest keep existing hub, link, then satellite ordering
        the service must not reorder by load timestamp, record source, table, provider strategy, or hash key
        saved records remain deterministic relative to caller-supplied chunk, request, and operation order

      Metadata:
        load timestamp remains DataVaultSaveRequest.LoadTimestamp subject to IDataVaultLoadTimestampResolver
        record source remains DataVaultSaveRequest.RecordSource subject to IDataVaultRecordSourceResolver
        chunked execution must not introduce hidden metadata lanes
        resolver failure stops continuation to later chunks

      Cancellation and transaction ownership:
        caller owns DbContext
        caller owns current or ambient transaction
        caller owns cancellation token
        chunked execution participates in the caller's current transaction
        chunked execution must not create, commit, roll back, or suppress caller transactions
        cancellation is observed before continuation to later chunks
        canceled execution propagates cancellation instead of silently completing later chunks

      Compatibility:
        default v1 implementation should process each bounded chunk through the existing ordered request pipeline
        hub saves preserve idempotent reuse by generated hash key
        link saves preserve idempotent reuse by generated hash key
        satellite saves preserve parent-scoped and driving-key-scoped hash-diff replay behavior
        provider-specific optimized paths keep bound-parameter, current-transaction, cancellation, and decline-unsupported-shape rules
        provider-specific strategy decline must not change the public caller contract

      Hash-state continuity:
        chunked saves carry enough hash-key and hash-diff continuity across chunk boundaries to match equivalent ordered bulk input
        satellite continuity is scoped by satellite table, parent hash key, and canonical multi-active driving-key values
        the contract does not require full logical source-load materialization
        unsupported shapes requiring unbounded retained state can fail deterministically or use a documented bounded fallback
        implementations must not silently consume unbounded memory

      Non-goals:
        no provider-neutral optimized chunk execution implementation in this contract story
        no bounded retained-state diagnostics in this contract story
        no provider-specific chunk optimization in this contract story
        no background ingestion
        no scheduler or queue integration
        no file or CDC ingestion
        no implicit SaveChanges interception

      Existing compatibility evidence:
        ChunkedSaveMatchesEquivalentBulkOrderingForHubAndLinkRequests
        ChunkedSaveObservesCancellationBeforeLaterChunks
        ChunkedSaveParticipatesInCallerTransactionAcrossChunks
        ChunkedSaveReusesRepeatedHubAndLinkRowsAcrossChunks
        ChunkedSaveCarriesSatelliteHashDiffContinuityAcrossChunks
        DefaultSaveServiceCarriesSatelliteHashDiffsAcrossBulkRequests
        DefaultSaveServiceKeepsBulkSatelliteLatestHashDiffChronological
        DefaultSaveServiceReusesExistingHubAndLinkRowsAcrossSqliteContexts
        ProviderSqlExecutionContract.ParticipatesInCurrentTransactionAsync
        ProviderSqlExecutionContract.PropagatesCancellationTokenAsync
      """;

  [Fact]
  public void StreamingExplicitSaveContractMatchesApprovedFixture() {
    var snapshotPath = GetRepositoryPath(
        "tests",
        "DCoding.Data.DVault.Tests",
        "Unit",
        "Snapshots",
        "Contracts",
        "StreamingExplicitSaveContract.approved.txt");

    var actual = NormalizeLineEndings(File.ReadAllText(snapshotPath));

    Assert.Equal(NormalizeLineEndings(ApprovedSnapshot).TrimEnd('\n') + "\n", actual);
  }

  [Fact]
  public void ArchitectureDocumentCarriesStreamingExplicitSaveContractMarkers() {
    var documentPath = GetRepositoryPath(
        "docs",
        "architecture",
        "dvault-v1-streaming-explicit-save-contract.md");
    var document = NormalizeLineEndings(File.ReadAllText(documentPath));

    Assert.Contains("IDataVaultSaveService", document, StringComparison.Ordinal);
    Assert.Contains("DataVaultChunkedSaveRequest", document, StringComparison.Ordinal);
    Assert.Contains("DataVaultSaveChunk", document, StringComparison.Ordinal);
    Assert.Contains("DataVaultSaveRequest", document, StringComparison.Ordinal);
    Assert.Contains("DataVaultBulkSaveRequest", document, StringComparison.Ordinal);
    Assert.Contains("The current v0.20.0 baseline keeps chunked saves provider-neutral while documenting staged PostgreSQL/MySQL optimized paths only for eligible materialized ordered bulk batches.", document, StringComparison.Ordinal);
    Assert.Contains("DataVaultProviderSaveStrategyContext", document, StringComparison.Ordinal);
    Assert.Contains("IDataVaultLoadTimestampResolver", document, StringComparison.Ordinal);
    Assert.Contains("IDataVaultRecordSourceResolver", document, StringComparison.Ordinal);
    Assert.Contains("caller's current transaction", document, StringComparison.Ordinal);
    Assert.Contains("explanation/remediation records", document, StringComparison.Ordinal);
    Assert.Contains("all-or-nothing behavior across chunks", document, StringComparison.Ordinal);
    Assert.Contains("cancellation", document, StringComparison.OrdinalIgnoreCase);
    Assert.Contains("hub and link saves preserve idempotent reuse", document, StringComparison.Ordinal);
    Assert.Contains("satellite saves preserve parent-scoped", document, StringComparison.Ordinal);
    Assert.Contains("must not silently consume unbounded memory", document, StringComparison.Ordinal);
    Assert.Contains("SaveChanges interceptors remain outside the default v1 persistence path", document, StringComparison.Ordinal);
  }

  [Fact]
  public void ExistingSaveCompatibilityEvidenceRemainsAvailable() {
    var integrationTests = NormalizeLineEndings(File.ReadAllText(GetRepositoryPath(
        "tests",
        "DCoding.Data.DVault.Tests",
        "Integration",
        "ExplicitDataVaultSaveServiceSqliteTests.cs")));
    var providerContract = NormalizeLineEndings(File.ReadAllText(GetRepositoryPath(
        "tests",
        "DCoding.Data.DVault.Tests",
        "Shared",
        "ProviderSqlExecutionContract.cs")));
    var providerStrategy = NormalizeLineEndings(File.ReadAllText(GetRepositoryPath(
        "src",
        "DCoding.Data.DVault",
        "DataVaultProviderSaveStrategy.cs")));

    Assert.Contains("DefaultSaveServiceCarriesSatelliteHashDiffsAcrossBulkRequests", integrationTests, StringComparison.Ordinal);
    Assert.Contains("DefaultSaveServiceKeepsBulkSatelliteLatestHashDiffChronological", integrationTests, StringComparison.Ordinal);
    Assert.Contains("DefaultSaveServiceReusesExistingHubAndLinkRowsAcrossSqliteContexts", integrationTests, StringComparison.Ordinal);
    Assert.Contains("ChunkedSaveMatchesEquivalentBulkOrderingForHubAndLinkRequests", integrationTests, StringComparison.Ordinal);
    Assert.Contains("ChunkedSaveObservesCancellationBeforeLaterChunks", integrationTests, StringComparison.Ordinal);
    Assert.Contains("ChunkedSaveParticipatesInCallerTransactionAcrossChunks", integrationTests, StringComparison.Ordinal);
    Assert.Contains("ChunkedSaveReusesRepeatedHubAndLinkRowsAcrossChunks", integrationTests, StringComparison.Ordinal);
    Assert.Contains("ChunkedSaveCarriesSatelliteHashDiffContinuityAcrossChunks", integrationTests, StringComparison.Ordinal);
    Assert.Contains("ParticipatesInCurrentTransactionAsync", providerContract, StringComparison.Ordinal);
    Assert.Contains("PropagatesCancellationTokenAsync", providerContract, StringComparison.Ordinal);
    Assert.Contains("public IReadOnlyList<DataVaultSaveRequest> Requests", providerStrategy, StringComparison.Ordinal);
    Assert.Contains("public IReadOnlyList<DataVaultResolvedSaveRequest> ResolvedRequests", providerStrategy, StringComparison.Ordinal);
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
