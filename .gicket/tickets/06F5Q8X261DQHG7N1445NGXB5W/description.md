<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the contract ticket against the existing explicit save service, provider-strategy boundary, epic scope, and already-split implementation/diagnostic child stories; no additional materialized planning writes were needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The repository already fixes the v1 write boundary at `IDataVaultSaveService` with existing explicit request shapes `DataVaultSaveRequest` and ordered `DataVaultBulkSaveRequest`; this ticket should define the streaming/chunked contract as an additive boundary that stays compatible with those request types rather than replacing them.
- Existing architecture notes already fix key non-goals: no `SaveChanges` interception as the default write path, no background ingestion, and no scheduler or queue integration.
- Current child-story context already separates concerns: ticket `06F5Q8X8Q72TQ5B7F2JSAJWPR8` owns provider-neutral chunked execution, and ticket `06F5Q8XF9DPKFW9VY0F3Y32BH4` owns bounded hash-state and diagnostics. This contract ticket should stay focused on the public API/behavior definition and compatibility rules.
- Repository tests already establish baseline semantics that the streaming contract must preserve for v1: deterministic caller order for bulk requests, explicit cancellation propagation, participation in the caller's current transaction, record-source/load-timestamp resolution hooks, hub/link idempotent reuse, and satellite hash-diff replay behavior per parent.

### Scope In
- Define the public streaming or chunked explicit-save contract as an explicit `IDataVaultSaveService` boundary that remains compatible with current single-request and ordered bulk-save usage.
- Define caller-visible rules for chunk input shape, per-chunk ordering, cancellation, transaction ownership, and load-timestamp/record-source behavior.
- Define v1 compatibility expectations between streaming/chunked saves and existing hub/link/satellite save semantics, including deterministic saved-record ordering and idempotent reuse behavior.
- Define the contract-level rules for carrying enough hash-key/hash-diff continuity across chunk boundaries without requiring full logical-load materialization, while leaving concrete bounded-state implementation and diagnostics to the dedicated child ticket.

### Scope Out
- Implementing the provider-neutral chunked execution path.
- Implementing bounded state retention, memory diagnostics, or diagnostic event shapes beyond the contract-level requirement that such behavior remain bounded and deterministic.
- Background ingestion, schedulers, queues, file/CDC pipelines, or any automatic runtime orchestration.
- Changing the default write boundary to `SaveChanges` interception or making DVault persistence implicit.

## Acceptance Criteria
- The refined contract defines one additive streaming or chunked explicit-save boundary under `IDataVaultSaveService` and states that existing `SaveAsync(DbContext, DataVaultSaveRequest, ...)` and `SaveAsync(DbContext, DataVaultBulkSaveRequest, ...)` semantics remain valid and backward compatible.
- The contract defines the input model in bounded terms: requests are supplied as an ordered sequence of explicit save chunks, each chunk contains ordinary explicit save operations with the same validation rules as existing requests, and the service processes chunks in caller order without reordering within or across chunks.
- The contract states that the caller continues to own the `DbContext`, ambient/current transaction, and cancellation token, and that streaming/chunked execution must participate in the caller's current transaction and observe cancellation before partial continuation to later chunks.
- The contract states how load timestamp and record source are applied across chunks: they remain explicit caller-visible request metadata subject to the same configured resolver hooks already used by the existing save pipeline, and chunked execution must not invent hidden metadata lanes.
- The contract states compatibility rules for current save behavior: hub and link saves preserve idempotent reuse semantics, satellite saves preserve parent-scoped hash-diff replay semantics, and returned saved-record ordering remains deterministic relative to the caller-supplied chunk and operation order.
- The contract states that hash-key/hash-diff continuity across chunks must be achieved without requiring full source materialization of the complete logical load, and that unsupported shapes requiring unbounded retained state may be rejected or forced through a documented bounded fallback rather than silently consuming unbounded memory.
- The contract includes focused tests that prove compatibility with existing API behavior for ordering, cancellation, transaction participation, repeated hub/link reuse, and satellite hash-diff continuity across chunk boundaries.

## Definition of Done
- A repository planning or architecture note defines the streaming/chunked explicit-save contract in bounded, developer-actionable terms aligned with the existing explicit save-service architecture.
- The contract explicitly references or preserves the existing `IDataVaultSaveService`, `DataVaultSaveRequest`, `DataVaultBulkSaveRequest`, and provider-strategy boundaries already present in the repository.
- The refinement leaves implementation of execution mechanics and diagnostics to the existing child stories without duplicating or conflicting with their scope.
- No blocking PO-level ambiguity remains about transaction ownership, cancellation, ordering, compatibility with existing save requests, or the non-goals for this ticket.

## Implementation Notes
- Use the existing explicit service as the stable baseline: `IDataVaultSaveService` currently exposes single-request and ordered-bulk request overloads, and provider strategies already receive ordered `IReadOnlyList<DataVaultSaveRequest>` batches through `DataVaultProviderSaveStrategyContext`.
- The current provider strategy contract already proves important invariants that the new streaming contract should ratify instead of reopening: provider-specific optimized paths must use bound parameters, participate in the current transaction, propagate cancellation, and decline unsupported tracked-change shapes.
- Existing integration coverage already demonstrates the expected semantic baseline for reuse and hash-diff behavior. The contract ticket should reference those preserved semantics and let the implementation story extend them to chunk-boundary scenarios.
- No planning document, attachment, description update, or ticket relation change was materialized in this run because the current epic split already covers execution and diagnostics with dedicated child stories and the remaining work on this ticket is refinement of the parent contract itself.

## Open Questions
- none

## Follow-Up Questions
- Should the eventual public streaming surface be exposed as a new request type, a new overload, or an adapter over existing ordered bulk-save requests, as long as it preserves the explicit `IDataVaultSaveService` boundary?
- If some satellite or multi-active shapes cannot guarantee bounded retained state in v1, should the implementation prefer deterministic rejection or a documented provider-neutral fallback path?
- After the provider-neutral contract lands, do any provider packages need additional optimized strategy-specific chunk execution tickets beyond the already-split provider-neutral implementation story?

## Risks
- If the implementation ticket introduces a streaming surface that weakens the current deterministic ordering or current-transaction semantics, it will conflict with existing explicit-save and provider-strategy evidence already present in the repository.
- If the contract does not explicitly bound unsupported memory-sensitive shapes, later implementation may accidentally promise full logical-load streaming for cases that still require retained per-parent state across chunks.

## Split Recommendations
- No further split is recommended from this PO pass; the epic already has separate child stories for provider-neutral chunked execution (`06F5Q8X8Q72TQ5B7F2JSAJWPR8`) and bounded hash-state/diagnostics (`06F5Q8XF9DPKFW9VY0F3Y32BH4`).

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Define the API and behavior contract for streaming or chunked explicit saves.

Acceptance criteria:
- Describes input shape, ordering, cancellation, transaction ownership, and compatibility with current save requests.
- Documents hash-key and hash-diff state across chunks without full source materialization.
- Adds focused tests for validation and existing API compatibility.

Non-goals:
- No automatic background ingestion.
- No scheduler or queue framework integration.
- No implicit SaveChanges interception.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Added `docs/architecture/dvault-v1-streaming-explicit-save-contract.md` to define the additive streaming/chunked explicit-save contract under `IDataVaultSaveService`.
- Added a pinned contract fixture and unit tests under `tests/DCoding.Data.DVault.Tests/Unit` so the contract markers and existing compatibility evidence remain reviewable.
- Kept execution mechanics, bounded-state diagnostics, and provider-specific chunk optimizations out of this story and assigned them to the existing follow-on implementation scopes.

### Verification
- Passed: `dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --nologo --filter FullyQualifiedName~StreamingExplicitSaveContractSnapshotTests`; Microsoft Testing Platform ignored the VSTest filter and ran the full unit suite, `369` passed.
- Passed: `bash tools/check-format.sh`.
- Passed: `dotnet build DVault.slnx --nologo`; build completed with existing warning classes, including read-only NuGet vulnerability-cache warnings and pre-existing analyzer warnings.
- Attempted: `dotnet test DVault.slnx --nologo` under `timeout 600s`; the command exited `124` during MSBuild with `MSB4166` after the timeout, before a full solution test summary was available.

### Open Questions
- none
<!-- gicket-bot:developer-delivery:v1:end -->

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Added the additive `DataVaultChunkedSaveRequest`/`DataVaultSaveChunk` public boundary and `IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, ...)` implementation as a bounded adapter over the existing ordered save pipeline.
- Added focused executable integration coverage for multi-chunk ordering parity, cancellation before later chunks, caller transaction rollback, repeated hub/link reuse, and satellite hash-diff continuity across chunk boundaries.
- Updated the streaming contract note, pinned contract fixture, unit contract checks, and core public API snapshot so tester can verify the implemented boundary and the still-deferred optimized chunk execution/diagnostics scope.

### Verification
- Passed: `dotnet test tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj --nologo --filter FullyQualifiedName~ChunkedSave`; Microsoft Testing Platform ignored the VSTest filter and ran the full integration suite, `178` total, `162` passed, `16` skipped.
- Passed: `DVAULT_UPDATE_API_SNAPSHOTS=1 dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --nologo --filter FullyQualifiedName~ApiSurfaceSnapshotTests`; Microsoft Testing Platform ignored the VSTest filter and ran the full unit suite, `369` passed, and the core public API snapshot was updated.
- Passed: `bash tools/check-format.sh`.
- Passed: `dotnet build DVault.slnx --nologo`; build completed with existing warning classes, including read-only NuGet vulnerability-cache warnings and pre-existing analyzer warnings.
- Passed: `dotnet test DVault.slnx --nologo` under `timeout 600s`; the solution test command exited `0`.

### Rework Resolution
- Addressed tester AC7 by replacing string-only compatibility evidence with executable chunk-boundary behavior coverage for ordering, cancellation, transaction participation, repeated hub/link reuse, and satellite hash-diff continuity.

### Open Questions
- none
<!-- gicket-bot:developer-delivery:v1:end -->

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Reworked the previous implementation handoff to match the parent contract scope: production `DataVaultChunkedSaveRequest`, `DataVaultSaveChunk`, and `IDataVaultSaveService.SaveAsync(... DataVaultChunkedSaveRequest ...)` changes are not materialized in this ticket.
- Kept the architecture contract and made explicit that provider-neutral chunk execution, bounded retained-state diagnostics, and provider-specific optimizations remain owned by child stories.
- Preserved focused executable contract evidence through integration tests using a test-local chunk harness over the existing ordered `DataVaultBulkSaveRequest` boundary; this covers ordering, cancellation before later chunks, caller transaction participation, repeated hub/link reuse, and satellite hash-diff continuity across contract chunk boundaries.

### Verification
- Passed: `dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --nologo --filter FullyQualifiedName~StreamingExplicitSaveContractSnapshotTests`; Microsoft Testing Platform ignored the VSTest filter and ran the full unit suite, `369` passed.
- Passed: `dotnet test tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj --nologo --filter FullyQualifiedName~ChunkedSave`; Microsoft Testing Platform ignored the VSTest filter and ran the full integration suite, `178` total, `162` passed, `16` skipped.
- Passed: `bash tools/check-format.sh`.
- Passed: `dotnet build DVault.slnx --nologo`; completed with existing warning classes, including read-only NuGet vulnerability-cache warnings and pre-existing analyzer warnings.
- Passed: `timeout 600s dotnet test DVault.slnx --nologo`; completed with existing warning classes and external-provider skips for unconfigured SQL Server, MySQL, PostgreSQL, and Oracle lanes.

### Rework Resolution
- Addresses the tester finding that the parent contract duplicated child-story execution mechanics by removing the production chunked save overload/adapter and restoring the public API snapshot to omit `DataVaultChunkedSaveRequest` and `DataVaultSaveChunk`.
- Keeps executable contract coverage in the parent ticket without changing the production write boundary.

### Open Questions
- none
<!-- gicket-bot:developer-delivery:v1:end -->