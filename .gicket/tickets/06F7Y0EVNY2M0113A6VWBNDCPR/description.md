<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the task to add async-source evidence inside the existing `customer-profile-streaming-save` benchmark family, reusing the landed async overload and the shared benchmark artifact contract.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Story `06F7Y0DCHTWCN3H25XQF18QE2G` is already `done` and supplies the additive `IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, CancellationToken)` boundary, so this ticket is evidence work, not API-shape work.
- The current benchmark harness and root `benchmark-summary.*` files only cover `customer-profile-streaming-save` as one materialized bulk row plus two synchronous `DataVaultChunkedSaveRequest` rows; this ticket is the bounded place to add async-source evidence.
- Repository docs already ratify the behavioral baseline: async streaming reuses `DataVaultSaveChunk`, the existing chunked telemetry and tracing family, and the same provider-neutral save semantics, so this task must not introduce provider-native async claims or a second public contract.

### Scope In
- Extend the existing `customer-profile-streaming-save` benchmark family to measure the async `IAsyncEnumerable<DataVaultSaveChunk>` save path against the same logical workload already used for materialized bulk and synchronous chunked baselines.
- Capture throughput, allocated bytes, retained-state behavior, processed-chunk counts, and comparable stop-condition evidence for the async-source path without changing the shared benchmark artifact schema.
- Regenerate or otherwise validate the shared benchmark artifact triplet so async-source rows can be cited through the same `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` surfaces.
- Update benchmark-facing contract or README guidance needed to explain how the async row should be interpreted relative to materialized bulk and synchronous chunked saves.

### Scope Out
- No provider-native async ingestion promise, provider-specific async benchmark rows, or new public API surface.
- No typed async mapper/helper APIs; that stays in `06F7Y0DZ3AJSG99YN00CAVX3JR`.
- No broad v0.24 docs, release-note, analyzer, or README adoption rewrite; downstream public-doc work stays in `06F7Y0F650KM61BQXMEQPZ86DR`.
- No new benchmark artifact filenames or schema columns beyond the existing triplet and row contract.

## Acceptance Criteria
- The benchmark harness emits async-source evidence within the existing `customer-profile-streaming-save` scenario family, using `IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, CancellationToken)` over the same 20-customer, 60-request logical workload as the current bulk and chunked comparisons.
- Async rows remain inside the shared benchmark artifact contract: scenario, provider, baseline, strategy family, dataset size, change ratio, timing, allocation, execution detail, and persisted outcome stay available without adding new artifact files or row fields.
- Async execution detail makes the exercised source shape visible and preserves chunk boundary or size, chunk count, processed chunk count, retained-state high-water or fallback data, and the chunked telemetry family so docs can compare async streaming with materialized bulk and synchronous chunked saves.
- Benchmark-facing documentation explicitly states that the async row is provider-neutral bounded streaming evidence and does not overstate provider-native behavior or claim a different ordering contract from existing chunked saves.
- Automated benchmark tests and artifact assertions are updated for the added async baseline and remain compatible with existing benchmark conventions.

## Definition of Done
- The benchmark code contains a bounded async-source baseline for `customer-profile-streaming-save` and its scenario validation passes.
- The shared benchmark artifact triplet can represent the async row without schema drift, and baseline-count or streaming-row assertions are updated accordingly.
- Benchmark README or artifact-contract documentation is consistent with the implemented async row naming, execution-detail fields, and interpretation limits.
- The task lands without expanding into provider-specific async execution, typed helper APIs, or general release-documentation scope.

## Implementation Notes
- Start from `benchmarks/DCoding.Data.DVault.Benchmarks/ChunkedSaveBenchmarks.cs`: keep the existing scenario data factory and outcome validation, then add an async-source baseline inside the same `customer-profile-streaming-save` family rather than creating a separate benchmark suite.
- Feed the async path the same logical `DataVaultSaveChunk` sequence used by the synchronous chunked comparison so allocation and timing differences isolate source materialization or enumeration behavior instead of request-shape drift.
- Keep the async benchmark on the existing chunked telemetry boundary (`operationKind=ChunkedRequest` / `dvault.save.chunked_request`) and surface the async source shape through baseline naming or `executionDetail`, not through new artifact columns.
- Update `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs` and any other artifact assertions that currently hard-code the streaming baseline count or assume only `chunked-save-bounded-*` rows exist.
- Reconcile benchmark-facing docs in `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` and `docs/plans/performance-evidence-benchmark-artifact-contract.md`; broader adopter wording in `docs/performance-profiles.md` stays coordinated with blocked docs task `06F7Y0F650KM61BQXMEQPZ86DR`.

## Open Questions
- none

## Follow-Up Questions
- After this evidence lands, should task `06F7Y0F650KM61BQXMEQPZ86DR` replace the current `docs/performance-profiles.md` wording that async streaming is not separate benchmark evidence with citations to the new async row while still preserving the no-provider-native-claim boundary?
- Should release-facing docs cite only the refreshed root `benchmark-summary.*` triplet, or also preserve a ticket-labelled `artifacts/benchmarks/...` bundle for before/after comparison history once the async benchmark is collected?

## Risks
- Current benchmark tests enumerate exactly three `customer-profile-streaming-save` baselines and explicitly inspect only the two `chunked-save-bounded-*` rows, so harness changes without matching test updates will fail quickly.
- The shared artifact contract currently names materialized bulk and bounded chunked streaming evidence; if async rows are added without contract or README updates, downstream docs can drift or cite ambiguous evidence.
- Repository docs already constrain async streaming to the existing provider-neutral chunked boundary, so careless baseline naming or release wording could accidentally imply a provider-native async optimization that the repository does not prove.

## Split Recommendations
- No further split is recommended; keep benchmark/allocation evidence on this ticket and keep the downstream public-doc rewrite on `06F7Y0F650KM61BQXMEQPZ86DR`.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Show when async streaming saves improve memory behavior and when existing bulk or synchronous chunked saves remain preferable.

# Scope In
- Extend benchmark scenarios or artifact documentation for async streaming versus materialized bulk and synchronous chunked saves.
- Capture allocation, throughput, retained-state behavior, and stop conditions.

# Acceptance Criteria
- Evidence can be cited from docs without overstating provider-native behavior.
- Runs stay compatible with existing benchmark conventions.