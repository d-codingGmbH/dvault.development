<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository evidence already bounds this epic to the v0.15.0 read-model maintenance slice: explicit PIT maintenance, explicit bridge maintenance, and stronger current/as-of read helpers are documented, implemented, and covered without needing a new PO split or planning document in this run.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Use the current repository baseline in `docs/releases/v0.15.0.md` dated 2026-05-19 as the public contract for this epic; `docs/releases/v0.10.0.md` is historical context, not the active release-note source for this scope.
- The v1 service boundary is already fixed by repository evidence: `IDataVaultReadService` owns latest/current/as-of/PIT/bridge read helpers, while PIT and bridge population remain separate explicit services through `IDataVaultPitMaintenanceService` and `IDataVaultBridgeMaintenanceService`.
- `docs/plans/pit-maintenance-service-v1-contract.md` already names existing child tickets `06F2PGPBRFT48JG57SV57N9TVW`, `06F2PGPKXWRFXNPFA1JR0X67XC`, `06F2PGPRGN0EVGD6RY5KY9M56W`, and `06F2PGPXVAYRBC94RQ7X5V4DVG`; no additional split was justified from current repository evidence.
- No bounded planning writes, child-ticket creation, relation updates, or attachment writes were materialized in this run because the existing repository contracts already provide the needed refinement baseline.

### Scope In
- Explicit PIT maintenance for existing `DataVaultPitMetadata`, including full rebuild and parent-scoped maintenance over persisted hub-parent satellite history.
- Explicit bridge maintenance for existing `DataVaultBridgeMetadata`, including rebuild and incremental maintenance over persisted source-link rows for many-to-many and hierarchy bridges.
- Current/as-of satellite convenience overloads that stay additive over the existing latest-satellite request baseline.
- PIT-backed and bridge read behavior over explicitly maintained read-model tables, including SQLite optimized read dispatch for supported shapes and provider-neutral fallback otherwise.
- README, production-adoption guidance, tests, public API snapshots, and release-note updates required to document the maintenance and query contract.

### Scope Out
- Automatic PIT or bridge maintenance during `SaveChanges`, ordinary reads, startup, interceptors, triggers, or background scheduling.
- Registry-backed PIT maintenance, link-parent PITs, multi-active PITs, or PIT/bridge orchestration batches.
- Delete-aware incremental hierarchy bridge maintenance, topology-shrink handling without rebuild, effectivity windows, path payload columns, closure-state columns, or broader graph traversal APIs.
- Non-SQLite provider-specific PIT or bridge read optimizations beyond provider-neutral fallback.
- Unrelated package shape changes, migrations workflow changes, or new declaration-model families outside the existing maintenance and read boundaries.

## Acceptance Criteria
- `AddDVault()` registers `IDataVaultPitMaintenanceService` and `IDataVaultBridgeMaintenanceService` beside the existing explicit save and read services.
- PIT maintenance supports full rebuild for one PIT and bounded maintenance for explicit parent hash keys, generates deterministic PIT rows from persisted satellite history, preserves missing-snapshot semantics, and corrects targeted parent history for late-arriving satellite rows.
- Bridge maintenance supports full rebuild and incremental maintenance for one bridge, materializes one row per distinct many-to-many endpoint pair, materializes one row per distinct hierarchy ancestor/descendant pair, stores minimum positive `TraversalDepth`, and does not create implicit self rows.
- Current/as-of satellite convenience overloads remain compatibility wrappers over the existing latest-satellite pipeline rather than a separate read semantics family.
- PIT and bridge reads continue to operate only over explicitly maintained tables, use SQLite optimized dispatch when supported, and fall back to provider-neutral reads when a provider or shape is unsupported.
- README, `docs/production-adoption-checklist.md`, and `docs/releases/v0.15.0.md` document the explicit service boundaries, supported behavior, and intentional limitations of the maintenance and query scope.

## Definition of Done
- Public API surface for PIT maintenance, bridge maintenance, and read convenience helpers is present and captured in the approved API snapshot.
- Unit coverage proves validation, DI registration, no-op or unsupported-shape behavior, and contract-level failure cases for the new maintenance surfaces.
- SQLite integration coverage proves PIT rebuild and parent maintenance, bridge rebuild and incremental maintenance, current/as-of read helpers, and PIT/bridge optimized read dispatch behavior.
- Release-note and adopter documentation reflect the maintained-read-model contract and do not overclaim automatic maintenance or non-SQLite optimization breadth.
- Any intentionally deferred behavior remains explicitly documented as out of scope rather than left implicit.

## Implementation Notes
- Repository source already contains the public surfaces and DI registration in `src/DCoding.Data.DVault`, including `IDataVaultPitMaintenanceService`, `IDataVaultBridgeMaintenanceService`, `IDataVaultReadService.ReadPitRowsAsync(...)`, current/as-of read extension helpers, and registry-backed bridge maintenance adapters.
- `tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs`, `DataVaultBridgeMaintenanceServiceSqliteTests.cs`, and `DataVaultPitReadServiceSqliteTests.cs` provide concrete repository evidence for deterministic PIT maintenance, bridge maintenance semantics, and SQLite/provider-neutral read behavior.
- `README.md` and `docs/releases/v0.15.0.md` already ratify the bounded defaults that PO should keep closed: explicit caller-owned maintenance, SQLite as the only repository-proven optimized PIT/bridge read provider, and rebuild-as-the-fix for delete-aware hierarchy shrink cases.

## Open Questions
- none

## Follow-Up Questions
- Should a later release add registry-backed PIT maintenance by logical PIT name, matching the existing registry-backed bridge maintenance convenience path?
- Which non-SQLite provider, if any, should be the next owner for provider-specific PIT or bridge read optimization after the provider-neutral baseline?
- Is delete-aware hierarchy bridge maintenance or topology-shrink orchestration important enough to merit a dedicated follow-up ticket beyond the documented rebuild guidance?
- Should multi-read-model batch orchestration for PIT and bridge maintenance be planned as a separate operational follow-up once single-read-model correctness is fully adopted?

## Risks
- Live `gicket-read-ticket`, relation, comment, and attachment verification remained blocked in this run by `BOT-LOCAL-TOOL-TRUST-BLOCKED`, so persisted Gicket relation state could not be re-confirmed beyond the prompt snapshot and repository planning documents.
- Incremental hierarchy bridge maintenance is intentionally not delete-aware; teams that use it after topology shrinkage without a rebuild can retain stale rows or stale shorter-depth assumptions.
- SQLite is the only repository-proven optimized PIT/bridge read path today; release or adoption messaging that implies broader provider optimization would overstate current evidence.

## Split Recommendations
- No new split is recommended from current evidence; keep the epic aligned to the already-documented child-ticket tree in `docs/plans/pit-maintenance-service-v1-contract.md` rather than reopening scope.
- If runtime relation cleanup later shows the epic is missing child links, restore links to the existing PIT maintenance, dependent PIT-read, PIT-read optimization, and documentation follow-through tickets already named in the repository planning contract instead of inventing broader new slices.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Provide explicit PIT/bridge maintenance and stronger current/as-of reads.

## Scope
- Refine and complete the work for "Maintenance and query operations" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.