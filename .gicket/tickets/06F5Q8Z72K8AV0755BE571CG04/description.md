<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the SQL Server staged bulk save story around the existing SQL Server provider-strategy boundary, provider-neutral fallback semantics, and opt-in SQL Server validation/benchmark lanes; no child tickets, relation edits, description updates, attachments, or planning documents were materialized in this pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Visible repository evidence already fixes the implementation lane as the existing SQL Server provider package and strategy path, so this story should refine AddDVaultSqlServer()/SqlServerDataVaultSaveStrategy behavior rather than introduce a new public save entry point.
- The current explicit-save and streaming contracts keep provider-specific chunk execution outside scope, so this story targets eligible ordered bulk batches behind the existing IDataVaultSaveService contract rather than new chunk-only behavior.
- SQL Server live validation and benchmark evidence remain opt-in through DVAULT_TEST_SQLSERVER_CONNECTION_STRING; default local validation stays at non-live smoke or contract coverage.
- Current ticket relations were verified and left unchanged: this story remains a child of 06F5Q8YBVRS2EZVMJK5EATV9AR, is blocked by 06F5Q8YKR31DXGRXVPJ9031BQW, and blocks 06F5Q900FC0P3HBZP81CVK7264.
- No bounded planning writes were applied because the current evidence supports a single bounded refinement contract without an immediate split or authoritative description rewrite.

### Scope In
- Implement a staged SQL Server bulk save path inside src/DCoding.Data.DVault.SqlServer as the provider-specific optimization for eligible ordered DVault save batches.
- Use SQL Server staging plus SqlBulkCopy or an equivalent SQL Server-native transfer mechanism for eligible hub, link, and ordinary satellite persistence work accepted by the SQL Server strategy.
- Preserve deterministic caller order, idempotent hub and link reuse, latest-state satellite hash-diff behavior, caller-owned transaction participation, cancellation propagation, and staging cleanup within the optimized path.
- Keep SQL Server eligibility gating and fallback aligned with the documented provider-strategy boundary so declined or unsupported batches continue through the provider-neutral writer.
- Add SQL Server-gated tests and benchmark or evidence rows consistent with the existing optional external-provider lane.

### Scope Out
- New public save-service overloads, streaming or chunked API changes, or provider-specific chunk execution claims.
- Changes to non-SQL Server provider packages except for minimal shared contract work required to preserve existing dispatcher semantics.
- Making live SQL Server infrastructure a required local prerequisite for all contributors.
- Background ingestion, CDC or file ingestion, implicit SaveChanges interception, or release-governance work.

## Acceptance Criteria
- Eligible ordered bulk saves on SQL Server use a staged native path inside the existing SQL Server provider strategy, with SqlBulkCopy or an equivalent SQL Server-native transfer mechanism, instead of relying only on the provider-neutral row-by-row path.
- The optimized path preserves current public semantics for request ordering, caller-owned transactions, cancellation, hub and link idempotent reuse, and satellite latest-state or hash-diff checks, and it cleans up temporary or staging artifacts on success, failure, and cancellation.
- The SQL Server strategy continues to decline unsupported shapes and fall back through the provider-neutral writer without changing caller-visible IDataVaultSaveService behavior.
- Focused SQL Server coverage proves supported native staged execution and declined-shape fallback behavior, with live database execution gated by DVAULT_TEST_SQLSERVER_CONNECTION_STRING.
- Benchmark or evidence outputs include SQL Server provider rows when the opt-in lane is configured and preserve visible skipped optional-provider rows when it is not configured.

## Definition of Done
- Repository tests cover SQL Server staged native execution, fallback gates, caller-transaction participation, cancellation propagation, hub and link reuse, and satellite latest-state continuity for the supported lane.
- Any benchmark or evidence artifacts touched by the story keep provider, execution-status, and skip-reason context consistent with the existing benchmark artifact contract.
- The implementation remains behind the existing AddDVaultSqlServer()/IDataVaultProviderSaveStrategy boundary and does not expand the public save API surface.
- Supported success, failure, and cancellation paths do not leave unhandled staging artifacts or cleanup gaps.

## Implementation Notes
- Use the existing SQL Server provider package and SqlServerDataVaultSaveStrategy as the ownership boundary; do not add a parallel save pipeline.
- Keep the documented dispatcher model intact: the core save service selects provider strategies by CanSave, and declined batches continue through the provider-neutral writer.
- Align SQL Server eligibility with the visible architecture baseline: provider-name match, clean change tracker, no multi-active satellite operations, at least 50 total operations, and at most 500 satellite operations.
- Preserve the ordered-bulk semantics from the explicit-save contract; the optimized path must not reorder requests or operations for staging convenience.
- Treat the live SQL Server lane as external opt-in evidence rather than a required local prerequisite; non-live smoke or contract coverage should continue to protect default local development.

## Open Questions
- none

## Follow-Up Questions
- After the SQL Server ordered-bulk staged path lands, decide whether a separate story should add provider-specific chunk execution instead of continuing to route chunked saves through the existing ordered-bulk or provider-neutral behavior.
- After benchmark evidence is collected, decide whether the current documented SQL Server operation thresholds or satellite batch limit should be tuned in a follow-up performance story.

## Risks
- Because live SQL Server execution is opt-in, the staged path can be under-validated in unattended environments unless a configured SQL Server lane runs during CI or release validation.
- The staged path crosses temporary tables and bulk-transfer boundaries while still needing exact latest-state and hash-diff correctness; regressions here would be data-correctness issues, not only performance issues.
- Cleanup behavior under cancellation or mid-batch failure is a concentrated risk area because staging artifacts and caller-owned transaction behavior must remain consistent.

## Split Recommendations
- No split is recommended from the current evidence; provider-specific ordered-bulk staging, SQL Server-gated tests, and benchmark-lane evidence still fit one bounded story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Implement a SQL Server staged bulk path for eligible DVault save batches.

Acceptance criteria:
- Uses SQL Server-appropriate staging and SqlBulkCopy or equivalent native transfer.
- Preserves idempotency, hash-diff latest-state checks, transactions, cancellation, and cleanup.
- Adds SQL Server-gated integration tests and benchmark rows when configured.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Summary
- Implemented the SQL Server staged native bulk save path inside `src/DCoding.Data.DVault.SqlServer` behind `AddDVaultSqlServer()` / `SqlServerDataVaultSaveStrategy`.
- Eligible SQL Server hub, link, and ordinary satellite save groups now create local temporary staging tables, transfer rows through `SqlBulkCopy` from the loaded SqlClient provider, then insert from staging with ordinal-preserving unique deduplication and ordinary satellite ordering.
- Staging cleanup is attempted from `finally` blocks on success, failure, and cancellation, while caller-owned transactions remain attached to staging creation, bulk copy, target insert, and cleanup.

Validation
- Added default smoke unit coverage for SQL Server staging DDL and staged insert command text.
- Added opt-in live SQL Server coverage for staged hub/link idempotent replay, caller-owned transaction rollback plus staging cleanup, and cancellation-before-write behavior.
- `bash tools/check-format.sh` passed.
- `dotnet build DVault.slnx --nologo --no-restore` was attempted and failed before compilation because required cached NuGet packages were missing, including `Microsoft.EntityFrameworkCore.Analyzers` 10.0.8 and `xunit.analyzers` 1.27.0. No restore was run because this bot execution boundary disallowed network-dependent behavior.
<!-- gicket-bot:developer-delivery:v1:end -->

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Summary
- Implemented the SQL Server staged native bulk save path inside `src/DCoding.Data.DVault.SqlServer` behind `AddDVaultSqlServer()` / `SqlServerDataVaultSaveStrategy`.
- Reworked benchmark evidence after tester return: the root `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` SQL Server optimized provider row now identifies `DVault SQL Server staged native bulk save path; transfer=SqlBulkCopy; selectedStrategy=SqlServerDataVaultSaveStrategy` while preserving the skipped optional-provider status and `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` skip reason when SQL Server is not configured.
- Updated benchmark artifact generation so future benchmark runs emit the same staged native bulk execution detail for the SQL Server optimized lane.
- Strengthened `BenchmarkScenarioExecutionTests` to assert the SQL Server benchmark evidence row carries the staged native bulk / SqlBulkCopy marker and retains the not-configured skip reason.

Validation
- `bash tools/check-format.sh` passed.
- `git diff --check -- benchmark-summary.csv benchmark-summary.json benchmark-summary.md benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs` passed.
- `node -e "JSON.parse(require('fs').readFileSync('benchmark-summary.json', 'utf8'))"` passed.
- `dotnet test tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj --nologo --no-restore --filter FullyQualifiedName~BenchmarkScenarioExecutionTests` was attempted and failed before compilation because required cached NuGet package `Microsoft.EntityFrameworkCore.Analyzers` 10.0.8 was missing.
- `dotnet build DVault.slnx --nologo --no-restore` was attempted and failed before compilation because required cached NuGet packages were missing, including `Microsoft.EntityFrameworkCore.Analyzers` 10.0.8 and `xunit.analyzers` 1.27.0. No restore was run because this execution boundary disallows network-dependent behavior.
<!-- gicket-bot:developer-delivery:v1:end -->