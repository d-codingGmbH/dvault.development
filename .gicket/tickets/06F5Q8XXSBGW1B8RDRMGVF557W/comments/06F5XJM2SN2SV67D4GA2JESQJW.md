[gicket-bot] PO refinement contract

Summary
- Chunked-save API, tests, and telemetry already exist in-repo, but the current benchmark harness and checked-in `benchmark-summary.*` evidence contain no streaming/chunked rows; this ticket is refined as SQLite-first benchmark evidence against equivalent materialized explicit-save baselines within the existing artifact contract.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the v1 public baseline to `IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, ...)`, `DataVaultSaveChunk`, and the existing chunked telemetry summary fields, so this ticket is benchmark/evidence work rather than API design.
- The current benchmark harness and root `benchmark-summary.*` evidence set contain no `stream` or `chunked` rows; the checked-in summary reports 32 baselines covering classic EF, provider-neutral and SQLite-optimized saves, read scenarios, compiled/pooling evidence, and optional provider-native bulk ingestion.
- The visible ticket branch shows no benchmark/docs/src/tests delta versus scratch source `4d216276032f77476d70eddc7b92268980e1b5bc`, so no streaming benchmark evidence has been materialized yet.
- The existing benchmark artifact contract already defines the persisted row/file format; chunk-size and chunk-path details should be carried through existing scenario, dataset-size, and `executionDetail` metadata instead of a new artifact schema.
- The current ticket has no comments or attachments, and live relation evidence is unchanged: incoming `parentOf` from `06F5Q8WVYMV8KQPAENPEEE3YM4`, incoming `blocks` from `06F5Q8X8Q72TQ5B7F2JSAJWPR8` and `06F5Q8XF9DPKFW9VY0F3Y32BH4`, and outgoing `blocks` to `06F5Q8Y3WW9FFV7HA289VHCEAM`.
- No bounded child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement run.

Scope In
- Add benchmark scenarios or a documented focused benchmark mode that exercises `DataVaultChunkedSaveRequest` against equivalent ordered materialized explicit-save baselines built from the same logical input.
- Collect timing and allocation evidence for bounded chunk sizes on the required SQLite local temporary-file baseline using the existing benchmark artifact trio.
- Update benchmark documentation and any evidence-facing prose that must explain the streaming/chunked scenario scope and the artifact location for release-facing claims.

Scope Out
- No new public streaming/chunked save API, telemetry type, or retained-state contract design; those surfaces already exist in source, docs, and tests.
- No mandatory new external-provider streaming matrix; the existing optional-provider rows remain the visible completed/skipped boundary through the current benchmark contract.
- No new benchmark artifact file format or row-schema expansion for chunk metadata.
- No provider-specific chunk-optimization feature work beyond measuring the behavior selected by the current implementation.

Open questions
- none

Follow-up questions
- After this story lands, do we want a separate ticket for provider-specific chunk-optimization evidence once any provider advertises a stable optimized chunk path?
- Should a later release note call out streaming-save benchmark findings explicitly, or is a checked-in artifact bundle plus benchmark README sufficient for the first evidence drop?
- Do we want a dedicated benchmark CLI switch for streaming or chunk-size matrices, or is adding the scenarios to the existing SQLite matrix sufficient for v1?

Risks
- Benchmark timings and allocations are machine-sensitive; before/after comparisons must preserve the same run-context inputs already required by the artifact contract.
- Chunked-save results can vary by selected strategy or retained-state fallback behavior, so rows will be misleading if `executionDetail` does not expose the exercised path.
- The ticket currently has two incoming `blocks` relations, so delivery sequencing may still depend on upstream work even though PO scope is ready.

Split recommendations
- No split is recommended if the ticket stays bounded to SQLite evidence, existing artifact files, and documentation or evidence updates.
- If future work needs provider-specific chunk matrices or chunk-optimization feature changes, create a follow-up story instead of expanding this ticket.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment