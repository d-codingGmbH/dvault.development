[gicket-bot] PO refinement contract

Summary
- Refined the task to add async-source evidence inside the existing `customer-profile-streaming-save` benchmark family, reusing the landed async overload and the shared benchmark artifact contract.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Story `06F7Y0DCHTWCN3H25XQF18QE2G` is already `done` and supplies the additive `IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, CancellationToken)` boundary, so this ticket is evidence work, not API-shape work.
- The current benchmark harness and root `benchmark-summary.*` files only cover `customer-profile-streaming-save` as one materialized bulk row plus two synchronous `DataVaultChunkedSaveRequest` rows; this ticket is the bounded place to add async-source evidence.
- Repository docs already ratify the behavioral baseline: async streaming reuses `DataVaultSaveChunk`, the existing chunked telemetry and tracing family, and the same provider-neutral save semantics, so this task must not introduce provider-native async claims or a second public contract.

Scope In
- Extend the existing `customer-profile-streaming-save` benchmark family to measure the async `IAsyncEnumerable<DataVaultSaveChunk>` save path against the same logical workload already used for materialized bulk and synchronous chunked baselines.
- Capture throughput, allocated bytes, retained-state behavior, processed-chunk counts, and comparable stop-condition evidence for the async-source path without changing the shared benchmark artifact schema.
- Regenerate or otherwise validate the shared benchmark artifact triplet so async-source rows can be cited through the same `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` surfaces.
- Update benchmark-facing contract or README guidance needed to explain how the async row should be interpreted relative to materialized bulk and synchronous chunked saves.

Scope Out
- No provider-native async ingestion promise, provider-specific async benchmark rows, or new public API surface.
- No typed async mapper/helper APIs; that stays in `06F7Y0DZ3AJSG99YN00CAVX3JR`.
- No broad v0.24 docs, release-note, analyzer, or README adoption rewrite; downstream public-doc work stays in `06F7Y0F650KM61BQXMEQPZ86DR`.
- No new benchmark artifact filenames or schema columns beyond the existing triplet and row contract.

Open questions
- none

Follow-up questions
- After this evidence lands, should task `06F7Y0F650KM61BQXMEQPZ86DR` replace the current `docs/performance-profiles.md` wording that async streaming is not separate benchmark evidence with citations to the new async row while still preserving the no-provider-native-claim boundary?
- Should release-facing docs cite only the refreshed root `benchmark-summary.*` triplet, or also preserve a ticket-labelled `artifacts/benchmarks/...` bundle for before/after comparison history once the async benchmark is collected?

Risks
- Current benchmark tests enumerate exactly three `customer-profile-streaming-save` baselines and explicitly inspect only the two `chunked-save-bounded-*` rows, so harness changes without matching test updates will fail quickly.
- The shared artifact contract currently names materialized bulk and bounded chunked streaming evidence; if async rows are added without contract or README updates, downstream docs can drift or cite ambiguous evidence.
- Repository docs already constrain async streaming to the existing provider-neutral chunked boundary, so careless baseline naming or release wording could accidentally imply a provider-native async optimization that the repository does not prove.

Split recommendations
- No further split is recommended; keep benchmark/allocation evidence on this ticket and keep the downstream public-doc rewrite on `06F7Y0F650KM61BQXMEQPZ86DR`.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment