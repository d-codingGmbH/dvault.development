[gicket-bot] PO refinement contract

Summary
- Refined this ticket to an implementation path: SQL Server latest-satellite remains a bounded single-ticket capability gap, and the visible repo evidence supports closing it with strategy, diagnostics, fallback, test, and evidence-surface updates rather than a no-work-required rejection.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository evidence still matches the documented gap: `AddDVaultSqlServer()` registers SQL Server optimized PIT/bridge reads only, `SqlServerDataVaultReadStrategy` does not currently implement `IDataVaultProviderReadStrategy`, and benchmark/docs still encode SQL Server latest-satellite as `providerSpecificReadStrategy=not registered for latest satellite reads`.
- No separate v0.41 criteria artifact was visible in the inspected repository surfaces, so this refinement treats the checked-in benchmark contract, evidence matrix, gap matrix, diagnostics tests, and performance guidance as the authoritative bounded criteria baseline.
- The visible evidence supports implementing the SQL Server latest-satellite improvement, not rejecting it as no-work-required; no checked-in repository evidence closes P0.02 already.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run. Live `gicket-read-ticket*` and relation reads were trust-policy blocked, so refinement relies on the supplied ticket snapshot plus bounded repository evidence.

Scope In
- Add SQL Server provider-specific latest/current and as-of satellite read support behind the existing `IDataVaultReadService` boundary.
- Register the SQL Server latest read strategy through `AddDVaultSqlServer()` and keep diagnostics selection/fallback behavior aligned with the existing provider strategy model.
- Update SQL Server latest-satellite gate, fallback, benchmark-contract, and evidence-surface expectations so repository guidance no longer says the strategy is unregistered.
- Add or update SQL Server latest-satellite unit/integration coverage for registration, selection, fallback, and result parity against the provider-neutral path.
- Align repository planning/evidence docs that currently classify SQL Server latest-satellite as a capability gap so they match the implemented strategy and any remaining evidence-only follow-up posture.

Scope Out
- PostgreSQL, MySQL, Oracle, and DB2 latest-satellite capability-gap work remains out of scope for this ticket.
- SQL Server PIT and bridge algorithm changes are out of scope except for keeping their existing behavior and shared strategy naming consistent.
- SQL Server save-strategy, bulk-threshold, staging-table, or provider-native chunking changes are out of scope.
- No new public API, query-planner surface, scheduler/maintenance runtime, benchmark schema, or deployable SQL artifact surface is introduced by this ticket.
- Do not widen provider claims into completed external-provider timing evidence when the SQL Server benchmark lane is still skipped or unconfigured.

Open questions
- none

Follow-up questions
- After this capability gap closes, should any remaining SQL Server external-provider latest-satellite timing collection be tracked as a separate evidence-gap ticket, or deferred until the next provider benchmark refresh?
- Should the same latest-satellite strategy pattern later be applied to PostgreSQL, MySQL, or Oracle, or should those providers stay explicitly parked in the gap matrix until separate tickets are opened?

Risks
- The SQL Server benchmark lane is opt-in and connection-string-gated; if local SQL Server execution is unavailable, reviewers must accept skipped-placeholder artifact evidence rather than completed external timing for this ticket.
- If code changes land without matching updates to the evidence matrix, gap matrix, benchmark expectations, and performance guidance, the repository will carry contradictory SQL Server latest-satellite claims.
- Live ticket comment/relation state could not be refreshed because the `gicket-read-ticket*` and relation tool calls were trust-policy blocked; no blocker is visible in the supplied snapshot, but relation housekeeping was not re-verified.

Split recommendations
- No split recommended; the visible repository evidence keeps this as one bounded SQL Server capability-gap task covering strategy registration, gating, diagnostics, tests, benchmark/evidence alignment, and fallback preservation.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment