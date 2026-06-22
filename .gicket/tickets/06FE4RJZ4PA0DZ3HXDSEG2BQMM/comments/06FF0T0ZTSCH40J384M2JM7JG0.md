[gicket-bot] PO refinement contract

Summary
- Refined this as a bounded SQL Server-only full PIT rebuild prototype: a provider-specific INSERT ... SELECT candidate behind AddDVaultSqlServer-style service selection and explicit fallback to the existing provider-neutral maintenance path. No ticket relations, child tickets, description updates, or planning documents were materialized during refinement.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The prototype is limited to IDataVaultPitMaintenanceService.RebuildAsync for SQL Server and does not redefine the public PIT maintenance API.
- AddDVault remains the provider-neutral baseline; AddDVaultSqlServer is the explicit service gate for any SQL Server-specific candidate behavior.
- Fallback to the existing provider-neutral PIT rebuild remains the default whenever the SQL Server candidate gate declines.

Scope In
- A SQL Server-specific candidate path for full PIT rebuilds that uses set-based INSERT ... SELECT instead of the current row-by-row provider-neutral write path.
- Gate logic that only considers the SQL Server candidate for RebuildAsync on a SQL Server DbContext registered through AddDVaultSqlServer and otherwise stays on provider-neutral maintenance.
- Parity verification against the current provider-neutral rebuild semantics for representative ordinary hub-parent PIT shapes.
- Deterministic diagnostics, tracing, or execution detail that makes candidate selection versus fallback observable with bounded fallback reasons.

Scope Out
- MaintainParentsAsync optimization.
- Multi-active PIT rebuild optimization in the SQL Server path.
- Link-parent PIT rebuild optimization in the SQL Server path.
- Non-SQL Server providers.
- Automatic PIT maintenance orchestration, scheduler behavior, SaveChanges interception, or read-path changes.
- Public performance-promotion claims beyond prototype-level parity and bounded diagnostics.

Open questions
- none

Follow-up questions
- Should a later ticket extend SQL Server-specific handling to MaintainParentsAsync after rebuild parity and gate behavior are proven?
- Should multi-active and link-parent PIT rebuild optimization be evaluated only after the ordinary hub-parent prototype lands cleanly?
- Should provider-configured benchmark evidence and any public boundary promotion stay in a separate follow-up ticket after the prototype is implemented?

Risks
- Multi-active and link-parent PIT semantics are materially more complex than ordinary hub-parent PITs; widening the SQL Server path too early risks semantic drift from the deterministic provider-neutral baseline.
- A raw SQL rebuild lane can interact poorly with pending tracked state; keeping a clean-context gate and explicit fallback is important to avoid surprising mixed persistence behavior.
- Without a separate benchmark artifact lane, this ticket must not be used to imply completed SQL Server PIT maintenance performance claims.

Split recommendations
- If the runtime prototype starts to absorb benchmark or documentation-promotion work, split provider-configured evidence collection into a separate ticket so this ticket stays on path selection, parity, and fallback behavior.
- If shape support expands beyond ordinary hub-parent PITs, split multi-active and link-parent rebuild optimization into later bounded tickets instead of reopening this prototype.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment