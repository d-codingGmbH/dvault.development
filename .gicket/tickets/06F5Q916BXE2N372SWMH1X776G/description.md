<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story into a delete-aware whole-bridge reconciliation contract based on repository evidence that current `MaintainBridgeAsync(...)` is append-only and topology shrink currently requires `RebuildBridgeAsync(...)`.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence shows `DefaultDataVaultBridgeMaintenanceService.MaintainBridgeAsync(...)` inserts missing rows and only lowers hierarchy `TraversalDepth`; it never deletes obsolete rows or raises depth after topology shrink.
- Current docs in `README.md`, `docs/releases/v0.7.0.md`, `docs/releases/v0.15.0.md`, and `docs/production-adoption-checklist.md` explicitly describe bridge maintenance as non-delete-aware, so this story is a real contract expansion rather than a naming cleanup.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement run.
- Live relation context remains unchanged: this ticket is a child of `06F5Q90CSKMGK3NZZ25XTW6W4C`, is blocked by `06F5Q90718D21DN1N1Q2AP7YEM`, and currently blocks `06F5Q91DR1555RSBQT7KDST684`.
- The current repo already ratifies the bounded baseline: bridge maintenance stays explicit, per-bridge, provider-neutral, and separate from automatic save or read flows.

### Scope In
- Add one explicit delete-aware bridge maintenance operation that is distinct from append-only `MaintainBridgeAsync(...)` and full-table `RebuildBridgeAsync(...)`.
- Support delete-aware reconciliation for both many-to-many and hierarchy bridges over persisted source-link rows.
- Update registry-backed bridge maintenance extensions, public API approval snapshots, and bridge maintenance documentation for the new operation.
- Add automated coverage for row deletion and `TraversalDepth` correction under topology shrink.

### Scope Out
- Automatic maintenance during `SaveChanges`, reads, startup, or background scheduling.
- New bridge metadata kinds, effectivity windows, path payload columns, closure-state columns, or broader graph-traversal APIs.
- Provider-specific optimization or bounded/key-scoped delete-aware maintenance beyond the existing whole-bridge request shape.
- Changing the compatibility behavior of `MaintainBridgeAsync(...)`; it stays append-only.

## Acceptance Criteria
- The public bridge maintenance surface defines a new explicit delete-aware operation for one `DataVaultBridgeMetadata` bridge while preserving the existing append-only `MaintainBridgeAsync(...)` behavior.
- For many-to-many bridges, the delete-aware operation reconciles the bridge table to the distinct endpoint pairs implied by persisted source-link rows by inserting missing pairs and deleting obsolete pairs.
- For hierarchy bridges, the delete-aware operation reconciles ancestor/descendant rows to the currently reachable positive paths, deletes rows whose path disappeared, keeps self rows absent, and stores the shortest currently valid `TraversalDepth` even when the correct depth becomes larger after topology shrink.
- The delete-aware operation reports inserts, updates, deletes, and unchanged rows through `DataVaultBridgeMaintenanceResult` and is available through the registry-backed bridge-name extension surface as well as explicit metadata.
- Automated coverage proves many-to-many removal, hierarchy edge removal, shorter-path replacement, and longer-path correction against the maintained bridge rows.

## Definition of Done
- Implementation lands in the public bridge maintenance service, registry extension surface, and supporting tests without regressing existing rebuild or append-only maintenance semantics.
- Public API approval artifacts and any service-replacement test doubles that implement `IDataVaultBridgeMaintenanceService` are updated for the new method.
- Repository docs that currently state bridge maintenance is not delete-aware are updated so shipped guidance matches the new contract.
- SQLite integration coverage demonstrates the new delete-aware behavior against persisted source-link data for both many-to-many and hierarchy bridges.

## Implementation Notes
- The current service already computes complete desired bridge rows through `CreateDesiredRowsAsync(...)`; the new operation should reuse that desired-state calculation and diff it against existing bridge rows rather than inventing a new traversal contract.
- The existing `DataVaultBridgeMaintenanceRequest` already identifies one whole bridge and `DataVaultBridgeMaintenanceResult` already carries `RowsDeleted`; that visible baseline supports a full-bridge reconciliation API without adding a new metadata shape.
- Hierarchy reconciliation must update `TraversalDepth` whenever the desired shortest path differs from the stored value, not only when the desired path is shorter.
- Expected touch points are `src/DCoding.Data.DVault/IDataVaultBridgeMaintenanceService.cs`, `src/DCoding.Data.DVault/DefaultDataVaultBridgeMaintenanceService.cs`, `src/DCoding.Data.DVault/DataVaultBridgeMaintenanceServiceRegistryExtensions.cs`, `tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs`, and the public API snapshot.
- Current README and release guidance say callers should rebuild for topology shrink; this ticket should replace that shrink-specific guidance with the new explicit delete-aware path while keeping rebuild available as the full replacement operation.

## Open Questions
- none

## Follow-Up Questions
- If callers later need delete-aware maintenance scoped to a subset of endpoints instead of whole-bridge reconciliation, that should be handled in a separate ticket because the current request surface is bridge-wide only.
- After this story lands, confirm whether downstream documentation or dependent ticket `06F5Q91DR1555RSBQT7KDST684` needs wording adjusted to reference the new delete-aware path instead of rebuild-only shrink handling.

## Risks
- The current implementation loads source-link rows and bridge rows into memory before reconciling; the new delete-aware path keeps that whole-bridge cost profile unless a later optimization ticket changes it.
- This story changes a public service contract and multiple published docs, so compatibility depends on keeping `MaintainBridgeAsync(...)` behavior stable while introducing the new delete-aware path additively.

## Split Recommendations
- No split recommended; repository evidence shows one cohesive service, test, and documentation change centered on the existing whole-bridge desired-row computation and public maintenance boundary.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Make bridge maintenance safe for destructive source-link topology changes.

Acceptance criteria:
- Defines explicit delete-aware maintenance separate from append-only MaintainBridgeAsync behavior.
- Handles hierarchy topology shrink, path removal, and increased TraversalDepth deterministically.
- Adds tests for many-to-many removal, hierarchy edge removal, shorter-path replacement, and longer-path correction.