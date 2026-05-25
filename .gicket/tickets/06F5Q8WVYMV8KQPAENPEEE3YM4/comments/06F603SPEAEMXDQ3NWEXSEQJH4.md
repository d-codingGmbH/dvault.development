[gicket-bot] PO refinement contract

Summary
- Refined the epic as a six-child roll-up for the v0.19.0 streaming explicit-save baseline; repository code, tests, docs, and release notes already verify the additive IDataVaultSaveService chunked-save contract, bounded fallback diagnostics, benchmark evidence, and release/documentation work, so no further split or PO blocker remains.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The existing persisted split is sufficient: child tickets 06F5Q8X261DQHG7N1445NGXB5W, 06F5Q8X8Q72TQ5B7F2JSAJWPR8, 06F5Q8XF9DPKFW9VY0F3Y32BH4, 06F5Q8XPXEQPJTKGJ7BQGCY438, 06F5Q8XXSBGW1B8RDRMGVF557W, and 06F5Q8Y3WW9FFV7HA289VHCEAM are already linked by parentOf and each is done.
- Repository evidence already lands the public baseline: src/DCoding.Data.DVault/DataVaultSaveService.cs exposes IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, ...), DataVaultSaveChunk, and the bounded fallback shape/fallback enums, while src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs keeps AddDVault() as the explicit registration path.
- docs/architecture/dvault-v1-streaming-explicit-save-contract.md, docs/architecture/dvault-v1-explicit-save-service.md, tests/DCoding.Data.DVault.Tests/Unit/StreamingExplicitSaveContractSnapshotTests.cs, and docs/releases/v0.19.0.md dated 2026-05-25 together establish the epic's current contract and evidence baseline.
- No new child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run because the existing graph and repository baseline already bound the epic cleanly.

Scope In
- Epic coordination of the additive chunked explicit-save boundary on IDataVaultSaveService.
- Provider-neutral chunk execution that preserves caller-supplied chunk order, request order, and existing hub/link/satellite operation ordering.
- Explicit DbContext, transaction, cancellation, load-timestamp, and record-source semantics across chunked saves.
- Bounded retained-state fallback and diagnostics for satellite continuity across chunk boundaries.
- Public documentation, remediation guidance, benchmark evidence, and v0.19.0 release-note baseline for the streaming save pipeline.

Scope Out
- Background workers, schedulers, file ingestion, CDC ingestion, and platform orchestration.
- Replacing IDataVaultSaveService with implicit SaveChanges interception as the default write path.
- Provider-specific chunk optimization or staged provider-native ingestion beyond the provider-neutral fallback baseline.
- NuGet publication approval or package push work beyond the documented release-note baseline.
- Additional split or relation cleanup inside this epic; the current child graph already captures the bounded v0.19.0 scope.

Open questions
- none

Follow-up questions
- When provider-specific chunk optimization becomes active scope, should it open as a separate epic or story set instead of reopening this v0.19.0 baseline?
- If loaders later need file, CDC, queue, or scheduler-driven ingestion, which separate planning lane should own that orchestration above the explicit save boundary?
- Do future release-management tickets need a dedicated publication-verification artifact beyond the v0.19.0 release-note baseline?

Risks
- If later work reopens this epic for provider staging or ingestion orchestration, the bounded v0.19.0 baseline will blur into a broader roadmap umbrella.
- Future optimizations must preserve the documented retained-state limit and fallback semantics; otherwise the public memory-bounded claim will regress.
- The release notes are evidence of scope and documentation baseline, not by themselves proof of final package publication approval or push.

Split recommendations
- No additional split recommended; the current parentOf graph already separates contract, execution, memory diagnostics, fallback/remediation, benchmark evidence, and release documentation.
- Create a separate follow-up epic or story set for provider-specific chunk optimization or staged provider ingestion rather than widening this ticket.
- Create separate ingestion/orchestration planning tickets if file, CDC, queue, or scheduler-driven loaders are later required.

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