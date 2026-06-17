[gicket-bot] PO refinement contract

Summary
- Refined the ticket as a bounded implement-or-document-no-work decision: current repository evidence keeps DB2 latest-satellite reads provider-neutral, DB2 only registers PIT/bridge read strategies, and the branch shows no delta from scratch ref 709ff4aebbfe7ef6c54bc616b1d53f741b75ae00.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current repository baseline is explicit: AddDVaultDb2() registers Db2DataVaultReadStrategy for PIT and bridge reads only and does not register a DB2 latest-satellite provider strategy.
- The checked-in root benchmark triplet preserves the DB2 latest-satellite row only as a skipped placeholder with selectedStrategy=<none>, plannedReadStrategy=<none>, and providerSpecificReadStrategy=not registered for latest satellite reads; that is row-identity guidance, not completed DB2 timing evidence.
- Current DB2 smoke coverage proves latest/current/as-of satellite reads through provider-neutral fallback and PIT/bridge reads through Db2DataVaultReadStrategy; PIT/bridge smoke evidence must not be treated as proof of DB2 latest-satellite optimization.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run. Live gicket ticket/comment/attachment/relation reads were trust-blocked earlier, so this refinement relies on the provided ticket snapshot plus repository evidence.

Scope In
- Close the DB2 latest-satellite capability gap by making an explicit bounded decision: either add a DB2 provider-specific latest/current/as-of satellite read strategy or authoritatively document no-work-required while keeping provider-neutral fallback.
- If implementation proceeds, keep the supported optimized shape aligned with the existing latest-satellite boundary already used by repository-proven providers: hub-parent satellites, current/as-of reads, and no multi-active driving-key support.
- Update the bounded evidence surfaces tied to the outcome: read-strategy registration or rejection posture, diagnostics and fallback expectations, tests, and benchmark/provider-evidence references for DB2 latest-satellite reads.
- Keep the existing DB2 PIT/bridge candidate path and provider-neutral fallback path internally consistent with the v0.34.0 and v0.39.0 repository baseline.

Scope Out
- Expanding DB2 into staged bulk, provider-native chunk execution, provider-specific PIT or bridge maintenance, or live-schema reading.
- Claiming completed DB2 latest-satellite timing without a configured DB2 benchmark run and updated artifact triplet.
- Widening optimized latest-satellite support to link-parent or multi-active satellites.
- Changing PostgreSQL, MySQL, Oracle, or unrelated SQL Server provider-gap tickets as part of this task.

Open questions
- none

Follow-up questions
- If this ticket closes as no-work-required, should the remaining PostgreSQL, MySQL, and Oracle latest-satellite capability-gap backlog be reviewed under the same closure rubric or kept open as future strategy-expansion work?
- If DB2 latest-satellite optimization is later implemented with completed timing evidence, which later release-note baseline should promote that row from skipped-placeholder to completed provider timing?

Risks
- A DB2 implementation may fail to produce a safe or worthwhile provider-specific latest-satellite path; in that case the ticket must close through the no-work-required branch rather than by widening unsupported claims.
- Without a configured DVAULT_TEST_DB2_CONNECTION_STRING benchmark run, any DB2 latest-satellite artifact row remains skipped-placeholder, so timing claims would still be unproven.
- Live ticket, comment, attachment, and relation reads were trust-blocked through gicket during this run, so ticket-state housekeeping beyond the provided snapshot could not be re-verified here.

Split recommendations
- No split is recommended; current evidence keeps DB2 latest-satellite closure as one bounded capability-decision ticket with an implementation branch and a no-work-required branch.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment