<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket by pinning the SQL Server prototype failure/cancellation rule to transaction-backed rollback, making post-failure PIT state deterministic, and tightening acceptance/test language. No child tickets, relation changes, attachments, or planning documents were materialized in this run.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The SQL Server candidate remains limited to AddDVaultSqlServer-selected RebuildAsync on a SQL Server DbContext with a clean context and an ordinary hub-parent PIT shape.
- When that SQL Server candidate path is selected, its delete plus INSERT ... SELECT rebuild must execute as one transactional attempt; a fault or cancellation before commit leaves the previously committed PIT rows intact.
- Any transient SQL objects used by the selected candidate path must be transactionally discarded or explicitly removed before the fault or cancellation returns to the caller.
- If the prototype gate declines, the invocation stays on the existing provider-neutral PIT maintenance path; this ticket does not retroactively change fallback semantics.

### Scope In
- A SQL Server-specific candidate path for full PIT rebuilds that uses set-based INSERT ... SELECT for ordinary hub-parent PITs behind AddDVaultSqlServer selection.
- Gate logic that only considers the SQL Server candidate for RebuildAsync on a SQL Server DbContext with a clean context and otherwise stays on provider-neutral maintenance.
- Deterministic diagnostics or execution detail that make candidate selection versus fallback observable with bounded fallback causes.
- Transactional fault and cancellation cleanup behavior for the selected SQL Server candidate path, including preserved pre-rebuild PIT contents and no leftover transient SQL artifacts.
- SQL Server-specific parity and rollback or cleanup test coverage for representative supported PIT shapes.

### Scope Out
- MaintainParentsAsync optimization.
- Multi-active PIT rebuild optimization in the SQL Server path.
- Link-parent PIT rebuild optimization in the SQL Server path.
- Non-SQL Server providers.
- Changing the public IDataVaultPitMaintenanceService API or retrofitting provider-neutral DefaultDataVaultPitMaintenanceService rollback semantics.
- Automatic PIT maintenance orchestration, scheduler behavior, SaveChanges interception, or read-path changes.
- Benchmark or promotion work beyond prototype-level parity, bounded diagnostics, and bounded rollback or cleanup verification.

## Acceptance Criteria
- When AddDVaultSqlServer is used, the DbContext provider is SQL Server, the call is RebuildAsync, the context is clean, and the PIT is an ordinary hub-parent PIT, the maintenance service may execute a SQL Server INSERT ... SELECT rebuild path.
- When any prototype gate fails, including AddDVault-only registration, provider mismatch, dirty context, MaintainParentsAsync calls, multi-active PITs, or link-parent PITs, the invocation stays on the existing provider-neutral maintenance path.
- For supported prototype inputs, the SQL Server path produces the same PIT row contents and the same DataVaultPitMaintenanceResult semantics as the current provider-neutral rebuild for representative ordinary hub-parent PIT shapes.
- Selection and fallback are observable through deterministic diagnostics or execution detail with bounded fallback causes rather than silent provider-specific behavior.
- If the selected SQL Server candidate path faults or observes cancellation before commit, the attempt must not replace the previously committed PIT contents: the PIT table retains its pre-rebuild rows and any transient SQL artifacts created for the candidate attempt are removed or transactionally discarded.
- Repository tests cover SQL Server path selection, provider-neutral fallback, parity for at least one representative ordinary PIT shape, and fault or cancellation verification that preloaded PIT rows survive the failed candidate attempt with no leftover transient SQL artifacts.

## Definition of Done
- The prototype lands without changing the public IDataVaultPitMaintenanceService request or result contract.
- AddDVault behavior remains provider-neutral, and AddDVaultSqlServer-only projects can opt into the SQL Server candidate without changing caller code.
- When the SQL Server candidate is selected, it participates in the caller's current DbContext transaction when one is open or otherwise uses one local transaction for the candidate rebuild attempt so fault or cancellation does not commit a partial PIT replacement.
- Existing PIT maintenance tests remain green and new SQL Server-specific unit or integration coverage proves gate behavior, parity, and rollback or cleanup behavior.
- Any code comments or docs that mention the prototype describe it as SQL Server-only, rebuild-only, gated, fallback-backed, and rollback-cleanup bounded rather than as a general provider-specific PIT maintenance baseline.

## Implementation Notes
- Current provider-neutral behavior in DefaultDataVaultPitMaintenanceService reads satellite rows into memory, generates PIT rows in process, deletes PIT rows with ExecuteDeleteAsync, and inserts regenerated rows through tracked dictionary entities plus SaveChangesAsync; this ticket should preserve that public contract but not silently broaden provider-neutral fallback semantics.
- Reuse the repository's established SQL Server optimized-SQL execution pattern: participate in DbContext.Database.CurrentTransaction when present, otherwise open one local transaction for the candidate rebuild and roll it back on fault or cancellation before surfacing the error.
- Any transient helper SQL objects used by the candidate path are allowed only if the failed or canceled attempt leaves no leftover helper artifacts in the database afterward.
- Existing PIT maintenance integration coverage currently proves tracing outcomes for cancellation; this ticket needs additional SQL Server-specific persisted-state assertions for rollback and cleanup behavior.
- No child tickets, relation changes, attachments, or planning documents were applied during this refinement pass.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket extend SQL Server-specific handling to MaintainParentsAsync after rebuild parity and rollback or cleanup behavior are proven?
- Should multi-active and link-parent PIT rebuild optimization be evaluated only after the ordinary hub-parent prototype lands cleanly?
- Should provider-configured benchmark evidence and any public boundary promotion stay in a separate follow-up ticket after the prototype is implemented?
- Should a later ticket harden the provider-neutral PIT maintenance baseline to the same pre-rebuild-row preservation rule instead of keeping that guarantee SQL Server-candidate-only?

## Risks
- Current provider-neutral fallback remains a delete-then-insert baseline; if the SQL Server gate declines, callers still rely on that older behavior until a separate ticket changes it.
- Deterministic rollback and cancellation verification on SQL Server may require a fault-injection seam or test interceptor because existing PIT maintenance tests currently cover tracing rather than persisted post-failure state.
- Widening the SQL Server candidate beyond ordinary hub-parent rebuilds before rollback and parity evidence are proven risks semantic drift and harder cleanup guarantees.

## Split Recommendations
- If provider-neutral PIT maintenance should also preserve pre-rebuild rows on failure or cancellation, split that baseline-hardening work into a separate ticket instead of broadening this SQL Server-only prototype.
- If the prototype starts to absorb multi-active, link-parent, benchmark, or documentation-promotion work, split those into follow-up tickets so this ticket stays focused on candidate selection, parity, and rollback or cleanup behavior.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: prototype a bounded SQL Server PIT rebuild INSERT SELECT path behind explicit service/diagnostic gates. Acceptance: fallback remains default when criteria are not met.