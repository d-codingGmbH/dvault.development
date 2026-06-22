[gicket-bot] PO refinement contract

Summary
- Refined the ticket by pinning the SQL Server prototype failure/cancellation rule to transaction-backed rollback, making post-failure PIT state deterministic, and tightening acceptance/test language. No child tickets, relation changes, attachments, or planning documents were materialized in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Added one explicit cleanup rule: when the SQL Server INSERT ... SELECT candidate path is selected, the rebuild attempt runs as one transaction-scoped operation. A fault or cancellation before commit must leave the previously committed PIT rows intact, and any transient SQL objects created for that attempt must be removed or transactionally discarded. If the gate declines, the call still falls back to the existing provider-neutral path.
- critic-item-2: `answered` - Updated the contract language so acceptance and test expectations are concrete: repository coverage must preload PIT rows, force a selected candidate-path fault or cancellation, assert the original PIT rows remain, and assert no transient SQL artifacts are left behind. The old generic failure or cancellation cleanup wording is no longer the verification target.
- critic-item-3: `answered` - The post-failure outcome is now deterministic for the SQL Server candidate path: no partial rebuild is committed. Tests should seed existing PIT rows, trigger a selected candidate-path fault or cancellation, and verify those pre-rebuild rows remain visible afterward. The ticket now explicitly separates that selected-path guarantee from the current provider-neutral fallback baseline, which this ticket is not re-scoping.

Clarifications
- The SQL Server candidate remains limited to AddDVaultSqlServer-selected RebuildAsync on a SQL Server DbContext with a clean context and an ordinary hub-parent PIT shape.
- When that SQL Server candidate path is selected, its delete plus INSERT ... SELECT rebuild must execute as one transactional attempt; a fault or cancellation before commit leaves the previously committed PIT rows intact.
- Any transient SQL objects used by the selected candidate path must be transactionally discarded or explicitly removed before the fault or cancellation returns to the caller.
- If the prototype gate declines, the invocation stays on the existing provider-neutral PIT maintenance path; this ticket does not retroactively change fallback semantics.

Scope In
- A SQL Server-specific candidate path for full PIT rebuilds that uses set-based INSERT ... SELECT for ordinary hub-parent PITs behind AddDVaultSqlServer selection.
- Gate logic that only considers the SQL Server candidate for RebuildAsync on a SQL Server DbContext with a clean context and otherwise stays on provider-neutral maintenance.
- Deterministic diagnostics or execution detail that make candidate selection versus fallback observable with bounded fallback causes.
- Transactional fault and cancellation cleanup behavior for the selected SQL Server candidate path, including preserved pre-rebuild PIT contents and no leftover transient SQL artifacts.
- SQL Server-specific parity and rollback or cleanup test coverage for representative supported PIT shapes.

Scope Out
- MaintainParentsAsync optimization.
- Multi-active PIT rebuild optimization in the SQL Server path.
- Link-parent PIT rebuild optimization in the SQL Server path.
- Non-SQL Server providers.
- Changing the public IDataVaultPitMaintenanceService API or retrofitting provider-neutral DefaultDataVaultPitMaintenanceService rollback semantics.
- Automatic PIT maintenance orchestration, scheduler behavior, SaveChanges interception, or read-path changes.
- Benchmark or promotion work beyond prototype-level parity, bounded diagnostics, and bounded rollback or cleanup verification.

Open questions
- none

Follow-up questions
- Should a later ticket extend SQL Server-specific handling to MaintainParentsAsync after rebuild parity and rollback or cleanup behavior are proven?
- Should multi-active and link-parent PIT rebuild optimization be evaluated only after the ordinary hub-parent prototype lands cleanly?
- Should provider-configured benchmark evidence and any public boundary promotion stay in a separate follow-up ticket after the prototype is implemented?
- Should a later ticket harden the provider-neutral PIT maintenance baseline to the same pre-rebuild-row preservation rule instead of keeping that guarantee SQL Server-candidate-only?

Risks
- Current provider-neutral fallback remains a delete-then-insert baseline; if the SQL Server gate declines, callers still rely on that older behavior until a separate ticket changes it.
- Deterministic rollback and cancellation verification on SQL Server may require a fault-injection seam or test interceptor because existing PIT maintenance tests currently cover tracing rather than persisted post-failure state.
- Widening the SQL Server candidate beyond ordinary hub-parent rebuilds before rollback and parity evidence are proven risks semantic drift and harder cleanup guarantees.

Split recommendations
- If provider-neutral PIT maintenance should also preserve pre-rebuild rows on failure or cancellation, split that baseline-hardening work into a separate ticket instead of broadening this SQL Server-only prototype.
- If the prototype starts to absorb multi-active, link-parent, benchmark, or documentation-promotion work, split those into follow-up tickets so this ticket stays focused on candidate selection, parity, and rollback or cleanup behavior.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment