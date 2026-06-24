[gicket-bot] PO refinement contract

Summary
- Refined the DB2 PIT full-rebuild feasibility ticket around the existing provider-strategy seam, the IBM-only validation lane, an explicit rollback-clean transaction gate, and an ordinary hub-parent first candidate slice; no persistent ticket or planning writes were applied.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository baseline: `AddDVaultDb2()` registers DB2 save plus latest-satellite/PIT/bridge read strategies, but no DB2 PIT maintenance strategy or `IDataVaultPitMaintenanceService` replacement exists today.
- This ticket is an evaluation task, not an implementation or performance-claim task; completed DB2 PIT read timings over maintained PIT rows do not count as maintenance push-down proof.
- The primary candidate architecture is a DB2 `IDataVaultProviderPitMaintenanceStrategy` for `IDataVaultPitMaintenanceService.RebuildAsync(...)` full rebuilds, not a SQL Server-style service replacement.
- Use the repository-proven IBM provider lane (`IBM.EntityFrameworkCore` with `DVAULT_TEST_DB2_CONNECTION_STRING`) and the existing opt-in DB2 smoke/benchmark surfaces, including the host-to-Podman validation run, as the live evidence lane.
- Ratify the current repository-proven compatible baseline as provider-default/hex-style DB2 execution; the separate DB2 binary-storage truncation evidence is a follow-up compatibility caveat, not part of the initial maintenance decision.

Scope In
- Assess whether DB2 can safely support `IDataVaultProviderPitMaintenanceStrategy` full-rebuild push-down for `RebuildAsync(...)` while preserving current provider-neutral PIT semantics.
- Record an explicit recommendation for the initial DB2 candidate slice: implement or defer ordinary hub-parent full rebuilds on `IBM.EntityFrameworkCore`.
- Classify candidate PIT shapes and fallback cases: ordinary hub-parent, shared-driving-key multi-active hub-parent, link-parent non-multi-active, dirty context, provider mismatch, incomplete maintenance-shape evidence, and caller-transaction rollback limits.
- Evaluate transaction and rollback requirements for delete-plus-insert full rebuilds when the strategy owns the transaction versus when a caller transaction is already active.
- Record DB2-specific SQL-shape risks and diagnostics/fallback vocabulary needed to keep unsupported or unsafe requests on provider-neutral maintenance.

Scope Out
- Implementing a DB2 PIT maintenance strategy, service replacement, or benchmark-backed maintenance timing claim in this ticket.
- `MaintainParentsAsync(...)` push-down, bridge maintenance push-down, automatic maintenance, read-time refresh, or EF `SaveChanges` orchestration.
- DB2 staged bulk, provider-native chunk execution, or unrelated DB2 save/read tuning.
- Non-IBM provider lanes, consumer database/container provisioning, or broad DB2 deployment guidance.
- Binary hash-key storage remediation beyond noting that current DB2 binary evidence shows separate provider-specific caveats.

Open questions
- none

Follow-up questions
- If the evaluation recommends implementation, should the follow-up implementation ticket and any later maintenance-timing evidence ticket stay split so architecture, behavior parity, and benchmark promotion remain independently reviewable?
- After the feasibility decision, should DB2 binary hash-key compatibility for save/latest/PIT paths be handled as a separate provider-compatibility ticket rather than widening this PIT-maintenance lane?

Risks
- IBM DB2 ambient-transaction and savepoint behavior may not support rollback-clean delete-plus-insert semantics, which could force strict fallback or a full defer decision.
- DB2 provider-specific SQL may be tractable for ordinary hub-parent PITs but materially riskier for shared-driving-key multi-active or link-parent PITs, making shape expansion a separate concern.
- Existing DB2 benchmark evidence proves reads over already maintained PIT rows, not write-side maintenance push-down, so the ticket can be over-read unless the artifact states that boundary clearly.
- Current DB2 binary-storage evidence includes provider truncation failures on save/latest/PIT paths, so the evaluation should not silently widen the supported baseline beyond the repository-proven compatible lane.

Split recommendations
- No PO split is required for the current refinement; this ticket remains a single bounded feasibility evaluation.
- If the evaluation approves an implementation path, create one follow-up ticket limited to `IBM.EntityFrameworkCore` ordinary hub-parent full-rebuild push-down through `IDataVaultProviderPitMaintenanceStrategy`.
- Keep multi-active hub-parent expansion, link-parent expansion, and any benchmark-backed DB2 PIT maintenance timing claim as separate later tickets.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment