<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket as a bounded implement-or-document-no-work decision: current repository evidence keeps DB2 latest-satellite reads provider-neutral, DB2 only registers PIT/bridge read strategies, and the branch shows no delta from scratch ref 709ff4aebbfe7ef6c54bc616b1d53f741b75ae00.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The current repository baseline is explicit: AddDVaultDb2() registers Db2DataVaultReadStrategy for PIT and bridge reads only and does not register a DB2 latest-satellite provider strategy.
- The checked-in root benchmark triplet preserves the DB2 latest-satellite row only as a skipped placeholder with selectedStrategy=<none>, plannedReadStrategy=<none>, and providerSpecificReadStrategy=not registered for latest satellite reads; that is row-identity guidance, not completed DB2 timing evidence.
- Current DB2 smoke coverage proves latest/current/as-of satellite reads through provider-neutral fallback and PIT/bridge reads through Db2DataVaultReadStrategy; PIT/bridge smoke evidence must not be treated as proof of DB2 latest-satellite optimization.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run. Live gicket ticket/comment/attachment/relation reads were trust-blocked earlier, so this refinement relies on the provided ticket snapshot plus repository evidence.

### Scope In
- Close the DB2 latest-satellite capability gap by making an explicit bounded decision: either add a DB2 provider-specific latest/current/as-of satellite read strategy or authoritatively document no-work-required while keeping provider-neutral fallback.
- If implementation proceeds, keep the supported optimized shape aligned with the existing latest-satellite boundary already used by repository-proven providers: hub-parent satellites, current/as-of reads, and no multi-active driving-key support.
- Update the bounded evidence surfaces tied to the outcome: read-strategy registration or rejection posture, diagnostics and fallback expectations, tests, and benchmark/provider-evidence references for DB2 latest-satellite reads.
- Keep the existing DB2 PIT/bridge candidate path and provider-neutral fallback path internally consistent with the v0.34.0 and v0.39.0 repository baseline.

### Scope Out
- Expanding DB2 into staged bulk, provider-native chunk execution, provider-specific PIT or bridge maintenance, or live-schema reading.
- Claiming completed DB2 latest-satellite timing without a configured DB2 benchmark run and updated artifact triplet.
- Widening optimized latest-satellite support to link-parent or multi-active satellites.
- Changing PostgreSQL, MySQL, Oracle, or unrelated SQL Server provider-gap tickets as part of this task.

## Acceptance Criteria
- The authoritative outcome is explicit: either AddDVaultDb2() gains a DB2 provider-specific latest/current/as-of satellite read strategy, or the ticket lands an authoritative no-work-required rejection that keeps DB2 latest-satellite reads on the provider-neutral path.
- If a DB2 latest-satellite strategy is added, it is diagnostics-gated and bounded to hub-parent, non-multi-active satellite reads, with provider-neutral fallback preserved for provider mismatch, unsupported parent shapes, multi-active satellites, incomplete evidence, and the existing finite fallback posture.
- Tests cover the chosen outcome, including strategy registration and gate behavior, DB2 latest/current/as-of read diagnostics or rejection posture, and representative DB2 read behavior without regressing PIT/bridge expectations.
- Benchmark and provider-evidence surfaces stay truthful to the chosen outcome: completed DB2 timing claims require a configured DB2 artifact triplet, while a rejection or no-work-required outcome preserves the skipped-placeholder latest-satellite evidence boundary and does not imply measured optimization.
- Any touched diagnostics or documentation surfaces remain aligned on the same DB2 read boundary and do not infer DB2 latest-satellite optimization from PIT/bridge candidate evidence.

## Definition of Done
- The chosen outcome is implemented or documented on the ticket branch with the relevant repository tests passing for the touched surfaces.
- DB2 latest-satellite diagnostics, fallback behavior, smoke expectations, and benchmark or evidence references are internally consistent after the change.
- No remaining changed surface claims DB2 has provider-specific latest-satellite optimization unless the repository also contains the supporting registration, diagnostics selection, and benchmark evidence.
- If the ticket resolves as no-work-required, the authoritative ticket or planning handoff surface states that DB2 latest-satellite remains provider-neutral by design in the current baseline.

## Implementation Notes
- Current branch inspection shows no implementation delta from scratch ref 709ff4aebbfe7ef6c54bc616b1d53f741b75ae00.
- DVaultDb2ServiceCollectionExtensions currently registers only IDataVaultProviderPitReadStrategy and IDataVaultProviderBridgeReadStrategy for Db2DataVaultReadStrategy; there is no IDataVaultProviderReadStrategy registration for DB2 latest-satellite reads.
- DataVaultProviderReadStrategyGateEvaluator currently evaluates latest-satellite provider strategies only for SQLite and SQL Server; DB2 has PIT and bridge gate evaluators only.
- Db2DataVaultSmokeTests currently assert provider-neutral fallback with NoProviderSpecificStrategyRegistered for DB2 latest-satellite diagnostics and Db2DataVaultReadStrategy selection for DB2 PIT and bridge diagnostics.
- The benchmark triplet, provider optimization evidence matrix, provider optimization gap matrix, performance profiles, PIT and bridge architecture note, and v0.34.0/v0.39.0 release notes all preserve the same current boundary: DB2 latest-satellite reads are not provider-specific today.
- If implementation is attempted, mirror the existing latest-satellite support envelope instead of inventing a broader DB2-only shape.
- If the chosen outcome is no-work-required, prefer updating the authoritative ticket or planning handoff surface instead of widening product-code or benchmark claims.

## Open Questions
- none

## Follow-Up Questions
- If this ticket closes as no-work-required, should the remaining PostgreSQL, MySQL, and Oracle latest-satellite capability-gap backlog be reviewed under the same closure rubric or kept open as future strategy-expansion work?
- If DB2 latest-satellite optimization is later implemented with completed timing evidence, which later release-note baseline should promote that row from skipped-placeholder to completed provider timing?

## Risks
- A DB2 implementation may fail to produce a safe or worthwhile provider-specific latest-satellite path; in that case the ticket must close through the no-work-required branch rather than by widening unsupported claims.
- Without a configured DVAULT_TEST_DB2_CONNECTION_STRING benchmark run, any DB2 latest-satellite artifact row remains skipped-placeholder, so timing claims would still be unproven.
- Live ticket, comment, attachment, and relation reads were trust-blocked through gicket during this run, so ticket-state housekeeping beyond the provided snapshot could not be re-verified here.

## Split Recommendations
- No split is recommended; current evidence keeps DB2 latest-satellite closure as one bounded capability-decision ticket with an implementation branch and a no-work-required branch.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Use v0.39 evidence and v0.41 criteria to implement or reject a DB2 latest-satellite read strategy improvement. Acceptance: tests, diagnostics, fallback, and benchmark evidence are updated, or no-work-required is documented.