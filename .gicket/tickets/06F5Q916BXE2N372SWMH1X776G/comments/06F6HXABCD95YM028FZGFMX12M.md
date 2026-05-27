[gicket-bot] PO refinement contract

Summary
- Refined the story into a delete-aware whole-bridge reconciliation contract based on repository evidence that current `MaintainBridgeAsync(...)` is append-only and topology shrink currently requires `RebuildBridgeAsync(...)`.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence shows `DefaultDataVaultBridgeMaintenanceService.MaintainBridgeAsync(...)` inserts missing rows and only lowers hierarchy `TraversalDepth`; it never deletes obsolete rows or raises depth after topology shrink.
- Current docs in `README.md`, `docs/releases/v0.7.0.md`, `docs/releases/v0.15.0.md`, and `docs/production-adoption-checklist.md` explicitly describe bridge maintenance as non-delete-aware, so this story is a real contract expansion rather than a naming cleanup.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement run.
- Live relation context remains unchanged: this ticket is a child of `06F5Q90CSKMGK3NZZ25XTW6W4C`, is blocked by `06F5Q90718D21DN1N1Q2AP7YEM`, and currently blocks `06F5Q91DR1555RSBQT7KDST684`.
- The current repo already ratifies the bounded baseline: bridge maintenance stays explicit, per-bridge, provider-neutral, and separate from automatic save or read flows.

Scope In
- Add one explicit delete-aware bridge maintenance operation that is distinct from append-only `MaintainBridgeAsync(...)` and full-table `RebuildBridgeAsync(...)`.
- Support delete-aware reconciliation for both many-to-many and hierarchy bridges over persisted source-link rows.
- Update registry-backed bridge maintenance extensions, public API approval snapshots, and bridge maintenance documentation for the new operation.
- Add automated coverage for row deletion and `TraversalDepth` correction under topology shrink.

Scope Out
- Automatic maintenance during `SaveChanges`, reads, startup, or background scheduling.
- New bridge metadata kinds, effectivity windows, path payload columns, closure-state columns, or broader graph-traversal APIs.
- Provider-specific optimization or bounded/key-scoped delete-aware maintenance beyond the existing whole-bridge request shape.
- Changing the compatibility behavior of `MaintainBridgeAsync(...)`; it stays append-only.

Open questions
- none

Follow-up questions
- If callers later need delete-aware maintenance scoped to a subset of endpoints instead of whole-bridge reconciliation, that should be handled in a separate ticket because the current request surface is bridge-wide only.
- After this story lands, confirm whether downstream documentation or dependent ticket `06F5Q91DR1555RSBQT7KDST684` needs wording adjusted to reference the new delete-aware path instead of rebuild-only shrink handling.

Risks
- The current implementation loads source-link rows and bridge rows into memory before reconciling; the new delete-aware path keeps that whole-bridge cost profile unless a later optimization ticket changes it.
- This story changes a public service contract and multiple published docs, so compatibility depends on keeping `MaintainBridgeAsync(...)` behavior stable while introducing the new delete-aware path additively.

Split recommendations
- No split recommended; repository evidence shows one cohesive service, test, and documentation change centered on the existing whole-bridge desired-row computation and public maintenance boundary.

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