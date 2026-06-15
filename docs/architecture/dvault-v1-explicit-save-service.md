# DVault V1 Explicit Save Service

Status: v1 implementation note
Ticket: 06EXB7H6KV753KM125XN3VDRTM

## Decision

DVault v1 uses an explicit DI-resolved save service as its default write entry point. Callers invoke `IDataVaultSaveService` with a focused single, ordered bulk, or bounded chunked request that carries the load timestamp, record source, and hub or link row intent.

The default `AddDVault()` path registers the save service without requiring an options object. Callers that need a different implementation can register their own `IDataVaultSaveService` through normal dependency injection override behavior.

## Rationale

The explicit service matches the existing convention-first public surface built around `AddDVault()`, `UseDataVault()`, and `ApplyDataVaultMetadata()`. Those APIs make Data Vault intent visible at startup or model configuration time, so the write path keeps the same visible boundary instead of hiding Data Vault persistence behind Entity Framework `SaveChanges` interception.

The v1 service owns the initial write orchestration for representative hub and link rows:

- Record source is supplied at the service request boundary.
- Load timestamp is supplied at the service request boundary and normalized to a UTC instant.
- Hub and link hash keys are computed through the registered `IStableHashNormalizer` and `IStableHashService`.
- The first concrete proof uses the existing SQLite EF Core test baseline.

Hub and link writes use the generated hash-key value as the reuse key. When a requested hub or link hash key is already present in the translated table, the default service leaves the existing row unchanged and still returns a deterministic `DataVaultSavedRecord` for the requested operation. `DataVaultSaveResult.RowsWritten` counts only rows inserted by that explicit invocation, so a fully reused request reports `0` while preserving the same hub-then-link saved-record ordering.

The current SQLite provider baseline is `DataVaultProviderCapabilityProfiles.Sqlite`, which declares `DataVaultProviderConcurrencySupport.NoneInV1Unsupported`. The default service therefore performs deterministic pre-insert reuse lookup for ordinary repeated saves, but it does not claim provider-neutral multi-writer conflict signals, retry behavior, merge semantics, or provider-specific upsert support.

SaveChanges interceptors remain outside the default v1 persistence path. v0.9.0 adds an optional `UseDataVaultSaveChangesMetadataInterceptor(...)` convenience lane for callers that already track generated DVault rows through EF and want missing `LoadTimestamp` and `RecordSource` values filled at `SaveChanges` time. That interceptor does not replace `IDataVaultSaveService`, does not compute hash keys or hash diffs, and does not make Data Vault persistence implicit by default.

## Chunked Save Boundary

The v0.19.0 public baseline added `IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, ...)` beside the existing single-request and ordered-bulk overloads. `DataVaultChunkedSaveRequest` contains caller-ordered `DataVaultSaveChunk` values, and each chunk contains ordinary `DataVaultSaveRequest` values. The detailed behavior contract is [DVault V1 Streaming Explicit Save Contract](dvault-v1-streaming-explicit-save-contract.md); the current public release summary is [DVault v0.26.0 Release Notes](../releases/v0.26.0.md).

The migration rule is intentionally narrow:

- Keep `DataVaultBulkSaveRequest` when the loader already has the complete ordered request set materialized.
- Use `DataVaultChunkedSaveRequest` when the loader needs bounded chunks without changing explicit load timestamps, record sources, request ordering, or caller-owned transaction behavior.

Chunked execution preserves caller-supplied chunk order, request order inside each chunk, and hub, link, then satellite operation ordering inside each request. Empty chunk sequences and empty chunks are no-ops. The service observes cancellation before continuing to later chunks, participates in the caller's current transaction, and does not create, commit, roll back, or suppress transactions on the caller's behalf. Callers that need all-or-nothing behavior across chunks should open the transaction before invoking the chunked save.

The provider-neutral retained-state implementation keeps satellite continuity state for one explicit chunked-save attempt, with the current default limit of `10000` satellite series. If that limit is reached, DVault records the finite retained-state fallback cause and unsupported-shape classification, clears retained state, and falls back to bounded per-chunk persisted latest-state lookup. `DataVaultSaveTelemetrySummary` reports chunk count, processed chunk count, retained-state current and high-water counts, finite retained-state fallback causes, unsupported-shape classifications, and chunked transaction guidance without raw hash keys, payload values, or per-parent retained-state entries.

Provider-native chunk execution, background ingestion, file ingestion, CDC ingestion, scheduler orchestration, and implicit `SaveChanges` streaming remain outside the v0.21.0 public claim set. Staged provider bulk ingestion is documented only for eligible materialized ordered PostgreSQL and MySQL batches. Provider packages can still optimize eligible ordinary ordered batches behind the same public save contract, and unsupported shapes continue to fall back without changing caller-visible semantics.

## Provider-Specific Save Strategy Dispatch

The shared provider optimization boundary is `IDataVaultProviderSaveStrategy`, `DataVaultProviderSaveStrategyContext`, and explicit provider capability profiles. `src/DCoding.Data.DVault` owns those contracts and the provider-neutral fallback dispatcher. Provider packages own provider-specific strategy implementations and any provider-specific SQL they require.

The core save service does not branch on provider names. It captures the registered `IDataVaultProviderSaveStrategy` implementations from dependency injection, sorts them by descending `Priority`, and preserves dependency-injection registration order when multiple strategies have the same priority. For every single-request save, ordered bulk save, or non-empty chunk in a chunked save, the dispatcher calls `CanSave` with the current `DbContext` and the ordered request batch. The first compatible strategy receives a `DataVaultProviderSaveStrategyContext` carrying the context, ordered requests, stable hash service, and normalizer.

When no provider-specific strategy is registered, or when every registered strategy rejects the current context and request batch, the dispatcher uses the built-in provider-neutral `IDataVaultSaveService` writer. Unsupported or unknown provider packages, and provider packages without a compatible strategy for the current batch, therefore keep the same public caller contract and fall back without requiring provider-name checks in the core package.

Provider-specific save-strategy registration is separate from provider-name capability-profile selection. The core package contains built-in capability profiles for the known SQLite, PostgreSQL, SQL Server, Oracle, Pomelo MySQL, and official MySQL EF provider names. Provider packages register optimized save strategies and can still register or override provider-name mappings when a future provider needs a custom profile.

## Provider-Specific SQL Artifact Boundary

Stored procedures, generated routines, and provider-specific SQL artifacts are not part of the default v1 save path. The default runtime boundary remains DI-resolved `IDataVaultSaveService` plus diagnostics-gated provider save strategies. Provider packages must not auto-generate stored procedures, register a procedure dispatcher, auto-run provider SQL artifacts, or silently route ordinary saves through a design-time artifact lane.

The v0.32 artifact lane is explicit opt-in, design-time-only, and review-only. The current command surface is `dvault sql-artifact --output <path> [--workload provider-native-bulk-ingestion]`, hosted by the consumer-owned design-time command entrypoint described in [DVault Dotnet EF Design-Time Workflow](dvault-dotnet-ef-design-time-workflow.md). The manifest schema version is `dvault.sql-artifact.v1`; the current exporter writes a deterministic dry-run manifest with `deployment=not-generated`, `runtimeDispatch=not-generated`, and `manifest-only-no-sidecar-sql` payload policy.

The current visible implementation is intentionally narrower than the repository-wide supported-provider baseline. SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 remain the supported provider set for DVault provider-profile and save/read strategy documentation, but the v0.32 SQL artifact exporter only covers the SQL Server `provider-native-bulk-ingestion` dry-run workload when request-bound save diagnostics select `SqlServerDataVaultSaveStrategy`. Documentation and release claims must not imply that every supported provider already has an implemented artifact exporter.

The consuming application owns artifact review, storage, deployment, invocation, versioning, rollback, cleanup, credentials, environment selection, observability, transaction policy, and migration compatibility. Migration synchronization is consumer-owned: DVault does not automatically align procedures or SQL artifacts with EF migrations, live schema reads, metadata changes, model-first import/export, or support-bundle refreshes.

Provider-specific artifact proposals and dry-run manifests must be compared against the existing save strategy dispatch and benchmark artifact contract before any deployable or runtime claim. At minimum, they need representative request-bound save diagnostics for the exact provider and workload, the root benchmark artifact triplet or a documented before/after triplet set, semantic parity with explicit DVault ordering, load timestamp, record source, hash key, hash diff, latest-state, cancellation, cleanup, and caller-owned transaction behavior, plus public non-goals for runtime dispatch, automatic execution, migration hooks, deployment automation, and default provider routing.

## Hashing Compatibility Boundary

The current save-service compatibility contract keeps canonical normalization and digest computation on the .NET side for hub and link hash-key generation. The provider-neutral writer uses the registered `IStableHashNormalizer` and `IStableHashService`, and provider-specific save strategies receive the same services through `DataVaultProviderSaveStrategyContext`. Provider packages may optimize batching, staging, existence checks, and insert shapes, but today's SQLite, PostgreSQL, SQL Server, Oracle, MySQL, and DB2 save strategies must preserve the same .NET-side stable-hash normalization and registered stable hash values instead of substituting provider SQL hash functions.

Database-side hashing is not part of the current runtime behavior and is not a default path. Any future provider-side hashing proposal must be introduced by a separate versioned provider contract that preserves the existing semantics and references the shared source-of-truth documents:

- `docs/plans/stable-hashing-contract.md` for canonical normalization rules, the `sha256-v1` stable-hash algorithm identifier, lowercase hexadecimal digest shape, and published compatibility vectors.
- `docs/plans/dvault-v1-default-persistence-convention-policy.md` for the logical `content_hash_algorithm`, `content_hash_canonicalization`, and `content_hash` tuple and the `sha-256` persistence content-hash meaning.
- `docs/plans/performance-evidence-benchmark-artifact-contract.md` for matched-input benchmark artifacts and optional-provider skipped-row visibility.

The minimum admission evidence for a provider-side path is deterministic provider-specific equivalence tests against the published stable-hash vectors and canonicalization rules, explicit opt-in or provider-gated selection, safe decline or fallback to the .NET-side path when parity cannot be proven, and benchmark evidence collected with the same scenario mode, provider filter, iteration counts, load-timestamp storage, and provider configuration as the comparison path. A provider-side path may only preserve DVault hash semantics; it must never silently replace the shared normalizer, change the algorithm identifiers, invent provider-local compatibility formats, or make release claims without the shared benchmark artifact set.

### Oracle Ordered Bulk Boundary

`AddDVaultOracle()` keeps Oracle bulk optimization behind the existing provider save-strategy dispatch. The Oracle strategy accepts only clean `Oracle.EntityFrameworkCore` contexts whose ordered batch has no multi-active satellite operations, at least `50` total hub/link/satellite operations, and no more than `10000` satellite operations. Batches outside that gate continue through the provider-neutral writer.

The retained Oracle implementation is direct Oracle batching: array binding when the provider command supports `ArrayBindCount`, and bounded direct insert batching otherwise. The staged Oracle path remains a reserved internal decision branch, not a selected execution path, until benchmark evidence shows a net win over direct Oracle batching and deterministic cleanup under the caller-owned transaction boundary.

The current diagnostics gate for native provider bulk execution is deliberately explicit. A provider strategy declines when the DbContext has pending tracked changes, the batch contains multi-active satellite operations, or the EF Core provider name does not match the strategy. PostgreSQL staged bulk dispatch requires at least `60` total operations and otherwise stays on the smaller PostgreSQL set-based direct or UNNEST path. SQL Server native dispatch also requires at least `50` total operations and at most `500` satellite operations. MySQL native dispatch requires at least `50` total operations and accepts both `Pomelo.EntityFrameworkCore.MySql` and `MySql.EntityFrameworkCore`. Oracle native dispatch requires at least `50` total operations and accepts at most `10000` satellite operations. Declined batches continue through the provider-neutral writer or a smaller provider-native path when one is available.

## Current Provider Optimization Capability Matrix

The current compatibility baseline is the core provider-neutral `AddDVault()`/`IDataVaultSaveService` path, including the chunked save overload. Provider packages add optimized save strategies around that same explicit contract; unsupported shapes and declined native gates fall back to the provider-neutral writer.

Validation vocabulary:

- `ProviderIntegration.RequiredLocal`: required local integration coverage.
- `ProviderIntegration.ExternalOptIn`: optional external database validation that runs only when explicitly configured.
- `ProviderSmoke.Default`: default smoke or contract coverage for provider registration and non-live behavior.

| Provider | Current release posture | Native save behavior required | Set-based existence checks required | Validation expectation | Benchmark evidence |
| --- | --- | --- | --- | --- | --- |
| SQLite | Provider-specific optimization baseline through `AddDVaultSqlite()` and `SqliteDataVaultSaveStrategy`. | Yes. | Yes. | `ProviderIntegration.RequiredLocal` integration coverage is required locally. | Required SQLite rows use local temporary files. |
| PostgreSQL | Provider-specific optimization through `AddDVaultPostgres()` and the Npgsql-compatible PostgreSQL save strategy, with staged COPY-backed bulk dispatch for larger eligible ordered batches and provider-neutral fallback when `CanSave` declines. | Yes, for clean `Npgsql.EntityFrameworkCore.PostgreSQL` contexts; larger eligible ordered batches use transient staging plus PostgreSQL COPY, while smaller batches retain the direct or UNNEST path. | Yes, using idempotent unique-row reuse, staged or set-based inserts, and latest-state satellite checks. | `ProviderSmoke.Default` covers registration locally. Live execution remains `ProviderIntegration.ExternalOptIn`, gated by `DVAULT_TEST_POSTGRES_CONNECTION_STRING`; it is not required local validation. | Optional provider-native bulk-ingestion rows are emitted when configured; skipped rows remain visible when unavailable and identify the staged PostgreSQL boundary. |
| SQL Server | Provider-specific optimization through `AddDVaultSqlServer()` and `SqlServerDataVaultSaveStrategy`, with provider-neutral fallback when `CanSave` declines. | Yes, for clean SQL Server contexts that meet the native bulk gate. | Yes, using SQL Server set-based unique-row inserts and latest-state satellite checks. | `ProviderSmoke.Default` covers non-live strategy behavior locally. Live execution remains `ProviderIntegration.ExternalOptIn`, gated by `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`; it is not required local validation. | Optional provider-native bulk-ingestion rows are emitted when configured; skipped rows remain visible when unavailable. |
| Oracle | Provider-specific optimization boundary through `AddDVaultOracle()` and an Oracle-gated insert strategy for eligible ordered bulk batches. Unsupported shapes, including dirty tracked contexts and multi-active satellite batches, still fall back through the provider-neutral writer. | Yes, for clean `Oracle.EntityFrameworkCore` hub, link, and ordinary satellite batches that meet the native bulk gate. | Yes, using Oracle insert/reuse behavior for unique hub and link rows plus latest-state satellite checks. | `ProviderSmoke.Default` covers registration locally. Live execution remains `ProviderIntegration.ExternalOptIn`, gated by `DVAULT_TEST_ORACLE_CONNECTION_STRING`; it is not required local validation. | Optional provider-native bulk-ingestion rows are emitted when configured; skipped rows remain visible when unavailable. |
| MySQL | Provider-specific optimization through `AddDVaultMySql()`, with staged bulk dispatch for larger eligible batches, multi-row dispatch for the existing native gate, and provider-neutral fallback when both candidates decline. | Yes, for clean `Pomelo.EntityFrameworkCore.MySql` or `MySql.EntityFrameworkCore` contexts that meet the relevant MySQL candidate gate. | Yes, using MySQL temporary staging for larger eligible batches, set-based unique-row inserts, and latest-state satellite checks. | `ProviderSmoke.Default` covers registration locally. Live execution remains `ProviderIntegration.ExternalOptIn`, gated by `DVAULT_TEST_MYSQL_CONNECTION_STRING`; it is not required local validation. | Optional provider-native bulk-ingestion rows are emitted when configured; skipped rows remain visible when unavailable. |
| DB2 | Provider-specific optimization through `AddDVaultDb2()` and `Db2DataVaultSaveStrategy`, with provider-neutral fallback when `CanSave` declines. | Yes, for clean `IBM.EntityFrameworkCore` hub, link, and ordinary satellite batches. No staged bulk lane or provider-native chunk execution is claimed in v0.34.0. | Yes, using DB2 set-based unique-row inserts and latest-state satellite checks. | `ProviderSmoke.Default` covers registration locally. Live execution remains `ProviderIntegration.ExternalOptIn`, gated by `DVAULT_TEST_DB2_CONNECTION_STRING`; it is not required local validation. | Optional DB2 provider-native bulk-ingestion rows are emitted when configured; skipped rows remain visible when unavailable and identify the clean-context save boundary. |

This matrix now reflects the v0.34.0 supported provider set while preserving the earlier explicit-save service boundary. PostgreSQL, SQL Server, Oracle, MySQL, and DB2 live execution evidence remains opt-in because it requires developer-managed databases through `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, `DVAULT_TEST_ORACLE_CONNECTION_STRING`, `DVAULT_TEST_MYSQL_CONNECTION_STRING`, and `DVAULT_TEST_DB2_CONNECTION_STRING`. Benchmark artifact scope is SQLite-required plus optional PostgreSQL, SQL Server, Oracle, MySQL, and DB2 provider-native bulk-ingestion rows. Skipped optional-provider rows remain part of the artifact contract through `executionStatus` and `skipReason`, and copied timings must keep provider and hardware/runtime context attached.

Use [Provider Optimization Evidence Matrix](../plans/provider-optimization-evidence-matrix.md) as the canonical save-row lookup for scenario, provider, baseline, evidence posture, authoritative artifact source, and finite stop/fallback conditions. This service boundary remains the behavior contract; the matrix is the citation surface for downstream provider optimization evidence.

Current optimization-hook ownership is:

- `src/DCoding.Data.DVault`: shared capability-profile contracts, provider-name keyed built-in profile selection, provider save strategy contracts, deterministic dispatch, and provider-neutral fallback.
- `src/DCoding.Data.DVault.Sqlite`: the optimized save strategy and SQLite set-based existence-check behavior exposed by `AddDVaultSqlite()`.
- `src/DCoding.Data.DVault.Postgres`: the Npgsql-compatible optimized save strategy exposed by `AddDVaultPostgres()` for clean PostgreSQL contexts, including staged COPY-backed bulk saves for larger eligible ordered batches and the retained direct or UNNEST path for smaller batches, with fallback for incompatible contexts.
- `src/DCoding.Data.DVault.SqlServer`: SQL Server optimized insert-only save strategy registration and SQL Server set-based existence-check behavior exposed by `AddDVaultSqlServer()`, with the opt-in live lane validating eligible ordered bulk hub, link, and satellite batches through `ProviderIntegration.ExternalOptIn`.
- `src/DCoding.Data.DVault.Oracle`: an Oracle-gated insert-only save strategy exposed by `AddDVaultOracle()` for clean `Oracle.EntityFrameworkCore` batches that meet the native bulk gate, with dirty tracked contexts, multi-active satellite batches, and very large satellite batches declined for provider-neutral fallback.
- `src/DCoding.Data.DVault.MySql`: the optimized MySQL save strategy exposed by `AddDVaultMySql()` for clean Pomelo or official MySQL contexts, with fallback for incompatible contexts.
- `src/DCoding.Data.DVault.Db2`: the optimized DB2 save strategy exposed by `AddDVaultDb2()` for clean `IBM.EntityFrameworkCore` hub, link, and ordinary satellite batches, plus DB2 PIT/bridge read strategy registration; DB2 latest-satellite reads, staged bulk, provider-native chunk execution, and live-schema reading remain out of scope for v0.34.0.
