[gicket-bot] PO refinement contract

Summary
- Verified the v0.15.0 release context, live relations, and the current bridge-read-only repository baseline on an unmodified branch; this story is ready for PO critic as the provider-neutral bridge maintenance baseline that unblocks query API and documentation follow-ons.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The story sits under epic 06F2PGP7HM8F39K3J0H5JHB3B4 (Epic: Maintenance and query operations) and release v0.15.0 - Maintenance and Query Operations.
- Live relations show this story blocks 06F2PGPKXWRFXNPFA1JR0X67XC (Story: Improve current and as-of query APIs) and 06F2PGPXVAYRBC94RQ7X5V4DVG (Task: Update v0.15.0 documentation and release notes), so it must establish the stable bridge-maintenance baseline those follow-ons consume.
- A blocks relation still exists from done epic 06F2PGMFWSEC95ATBCGZ6HYT5W (Epic: Provider bulk ingestion); treat that as satisfied historical release ordering, not as an active PO blocker.
- Current repository behavior is read-only for bridges: README.md, docs/releases/v0.7.0.md, and docs/production-adoption-checklist.md all state that bridge reads operate over already materialized tables and do not maintain bridge rows.
- Current integration evidence seeds bridge rows manually in DataVaultBridgeReadServiceSqliteTests, which confirms this story must add maintenance behavior rather than only repackage existing read APIs.

Scope In
- Add an explicit provider-neutral bridge maintenance service in the core DVault package; keep it additive beside IDataVaultSaveService and IDataVaultReadService rather than hiding bridge upkeep behind ordinary EF tracking, bridge reads, or SaveChanges interception.
- Cover the shipped bridge metadata baseline only: many-to-many bridges and hierarchy bridges declared through DataVaultBridgeMetadata and projected through existing bridge tables/entities.
- Define both full rebuild behavior and incremental maintenance behavior for one bridge at a time so callers can either recompute a bridge from persisted source-link rows or maintain only newly affected bridge rows after source-link ingestion.
- Support both explicit metadata requests and registry-backed resolution so callers using UseDataVaultMetadata() can maintain a bridge by logical name without re-declaring metadata.
- Keep existing bridge read APIs compatible; the maintenance story must materialize rows those APIs can consume, not redesign query projection ergonomics.

Scope Out
- PIT maintenance behavior; that remains sibling story 06F2PGPBRFT48JG57SV57N9TVW.
- Provider-specific bridge/PIT read optimization; that remains sibling story 06F2PGPRGN0EVGD6RY5KY9M56W.
- Broader current/as-of query API redesign; that remains blocked story 06F2PGPKXWRFXNPFA1JR0X67XC.
- Advanced bridge projection features already marked deferred in repository evidence: effectivity windows, path payload columns, closure-maintenance state columns, generated relationship graphs, PIT interactions, and multi-active interactions.
- Automatic scheduler/trigger behavior, background orchestration, or implicit maintenance during ordinary reads/saves.
- Provider-specific bulk SQL, physical tuning, or benchmark claims beyond the provider-neutral baseline.
- Multi-bridge batch orchestration; keep the v1 maintenance contract bounded to one bridge per request.

Open questions
- none

Follow-up questions
- After the provider-neutral baseline ships, does the release need a separate follow-on for provider-specific bridge-maintenance performance paths, or is the existing read-optimization story sufficient for the first adopter wave?
- Should broader adopter guidance document a recommended loader orchestration pattern between IDataVaultSaveService and bridge maintenance for batch link ingestion, or is the minimal README and release-note delta enough for v0.15.0?

Risks
- Hierarchy bridge maintenance can expand quickly on large recursive link sets; this story should stay correct and provider-neutral first, with any performance specialization treated as later work.
- Because current bridge tables do not carry effectivity, path payload, or closure-state columns, the implementation must avoid silently inventing advanced semantics that are not represented in the shipped metadata baseline.
- The story blocks downstream query-API and documentation tickets, so under-specifying idempotence or hierarchy-depth semantics would create avoidable rework in those follow-ons.

Split recommendations
- No split recommended; sibling tickets already isolate PIT maintenance, query API work, provider-aware optimization, and v0.15.0 documentation.

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 4
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment