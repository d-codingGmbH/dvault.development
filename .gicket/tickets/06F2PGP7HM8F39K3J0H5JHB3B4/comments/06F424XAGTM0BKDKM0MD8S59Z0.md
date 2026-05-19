[gicket-bot] PO refinement contract

Summary
- Repository evidence already bounds this epic to the v0.15.0 read-model maintenance slice: explicit PIT maintenance, explicit bridge maintenance, and stronger current/as-of read helpers are documented, implemented, and covered without needing a new PO split or planning document in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Use the current repository baseline in `docs/releases/v0.15.0.md` dated 2026-05-19 as the public contract for this epic; `docs/releases/v0.10.0.md` is historical context, not the active release-note source for this scope.
- The v1 service boundary is already fixed by repository evidence: `IDataVaultReadService` owns latest/current/as-of/PIT/bridge read helpers, while PIT and bridge population remain separate explicit services through `IDataVaultPitMaintenanceService` and `IDataVaultBridgeMaintenanceService`.
- `docs/plans/pit-maintenance-service-v1-contract.md` already names existing child tickets `06F2PGPBRFT48JG57SV57N9TVW`, `06F2PGPKXWRFXNPFA1JR0X67XC`, `06F2PGPRGN0EVGD6RY5KY9M56W`, and `06F2PGPXVAYRBC94RQ7X5V4DVG`; no additional split was justified from current repository evidence.
- No bounded planning writes, child-ticket creation, relation updates, or attachment writes were materialized in this run because the existing repository contracts already provide the needed refinement baseline.

Scope In
- Explicit PIT maintenance for existing `DataVaultPitMetadata`, including full rebuild and parent-scoped maintenance over persisted hub-parent satellite history.
- Explicit bridge maintenance for existing `DataVaultBridgeMetadata`, including rebuild and incremental maintenance over persisted source-link rows for many-to-many and hierarchy bridges.
- Current/as-of satellite convenience overloads that stay additive over the existing latest-satellite request baseline.
- PIT-backed and bridge read behavior over explicitly maintained read-model tables, including SQLite optimized read dispatch for supported shapes and provider-neutral fallback otherwise.
- README, production-adoption guidance, tests, public API snapshots, and release-note updates required to document the maintenance and query contract.

Scope Out
- Automatic PIT or bridge maintenance during `SaveChanges`, ordinary reads, startup, interceptors, triggers, or background scheduling.
- Registry-backed PIT maintenance, link-parent PITs, multi-active PITs, or PIT/bridge orchestration batches.
- Delete-aware incremental hierarchy bridge maintenance, topology-shrink handling without rebuild, effectivity windows, path payload columns, closure-state columns, or broader graph traversal APIs.
- Non-SQLite provider-specific PIT or bridge read optimizations beyond provider-neutral fallback.
- Unrelated package shape changes, migrations workflow changes, or new declaration-model families outside the existing maintenance and read boundaries.

Open questions
- none

Follow-up questions
- Should a later release add registry-backed PIT maintenance by logical PIT name, matching the existing registry-backed bridge maintenance convenience path?
- Which non-SQLite provider, if any, should be the next owner for provider-specific PIT or bridge read optimization after the provider-neutral baseline?
- Is delete-aware hierarchy bridge maintenance or topology-shrink orchestration important enough to merit a dedicated follow-up ticket beyond the documented rebuild guidance?
- Should multi-read-model batch orchestration for PIT and bridge maintenance be planned as a separate operational follow-up once single-read-model correctness is fully adopted?

Risks
- Live `gicket-read-ticket`, relation, comment, and attachment verification remained blocked in this run by `BOT-LOCAL-TOOL-TRUST-BLOCKED`, so persisted Gicket relation state could not be re-confirmed beyond the prompt snapshot and repository planning documents.
- Incremental hierarchy bridge maintenance is intentionally not delete-aware; teams that use it after topology shrinkage without a rebuild can retain stale rows or stale shorter-depth assumptions.
- SQLite is the only repository-proven optimized PIT/bridge read path today; release or adoption messaging that implies broader provider optimization would overstate current evidence.

Split recommendations
- No new split is recommended from current evidence; keep the epic aligned to the already-documented child-ticket tree in `docs/plans/pit-maintenance-service-v1-contract.md` rather than reopening scope.
- If runtime relation cleanup later shows the epic is missing child links, restore links to the existing PIT maintenance, dependent PIT-read, PIT-read optimization, and documentation follow-through tickets already named in the repository planning contract instead of inventing broader new slices.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment