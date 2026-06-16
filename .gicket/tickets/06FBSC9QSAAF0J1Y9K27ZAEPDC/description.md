<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Narrowed the ticket to a ticket-level Oracle bulk recommendation: keep the current direct batching/array-binding baseline and 50-operation/10000-satellite gate, explicitly keep gap-matrix row P1.04 open as an evidence-gap backlog item, and replace the prior inaccurate closure-only wording about description updates.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- A previous PO run already refreshed the durable ticket description in commit 7c29bd76c, and comment 06FCWF63ZWP8E59NABEH1QS42G explicitly reported that update; this refinement replaces the inaccurate no-description-update wording rather than repeating it.
- The canonical planning surface remains docs/plans/provider-optimization-gap-matrix.md:59, where P1.04 is still an Oracle provider-native-bulk-ingestion evidence gap; this ticket no longer claims to close or reclassify that backlog row.
- The repository baseline remains: OracleDataVaultSaveStrategy keeps DirectOracleBatching with optional ArrayBindCount array binding, and staged Oracle bulk remains not-selected-no-measured-win.
- The Oracle optimized-save boundary remains provider name Oracle.EntityFrameworkCore, clean DbContext, no multi-active satellite operations, minimum 50 total operations, and maximum 10000 satellite operations; outside that gate the product falls back to the provider-neutral writer.
- The root benchmark triplet Oracle save rows are skipped placeholders when DVAULT_TEST_ORACLE_CONNECTION_STRING is unset, so they cannot be used to retire P1.04; measured Oracle timing discussion must stay anchored to the checked-in provider-configured v0.32 artifacts.
- The only durable planning change expected from accepting this refinement is refreshing the current ticket description again; no child tickets, relation edits, attachments, or planning documents are needed.
- Downstream blocked ticket 06FBSCAJ5HDJH6CR0HZQ4B7H30 already covers the two valid outcomes for later implementation work: implement only if an improvement is accepted, otherwise close no-work-required.

### Scope In
- Reframe this ticket as a bounded Oracle bulk evaluation and recommendation, not as closure of canonical backlog row P1.04.
- Document the current Oracle direct batching and array-binding baseline together with the existing 50-operation and 10000-satellite fallback gate.
- State the evidence boundary clearly: P1.04 stays open until provider-configured Oracle benchmark evidence justifies changing the canonical gap-matrix posture.

### Scope Out
- Editing docs/plans/provider-optimization-gap-matrix.md or otherwise retiring P1.04 in this ticket.
- Implementing staged Oracle bulk, raising the 10000-satellite cap, or tuning the current threshold gates.
- Re-running benchmarks or treating skipped root benchmark-summary Oracle rows as completed timing evidence.
- Creating child tickets, relation rewrites, attachments, or planning documents unless later evidence changes the bounded recommendation.

## Acceptance Criteria
- The ticket contract explicitly states that the deliverable is a ticket-level recommendation only: keep the current Oracle direct optimized batching and array-binding baseline and keep P1.04 open as an evidence-gap backlog item.
- The refinement cites docs/plans/provider-optimization-gap-matrix.md:59 as the authoritative P1.04 posture and does not claim that this ticket closes or reclassifies that canonical backlog row.
- The refinement cites repository-backed implementation evidence for the current Oracle save posture, including OracleDataVaultSaveStrategy and DataVaultProviderSaveStrategyGateEvaluator, with the clean-context, provider-name, 50-operation, and 10000-satellite gate preserved.
- The refinement cites repository-backed validation evidence showing that staged Oracle bulk remains not-selected-no-measured-win and that the 10000-satellite cap remains the checked-in boundary, including Oracle unit/integration coverage and the v0.32 Oracle high-volume artifact.

## Definition of Done
- Open questions are empty because the critic items are answered directly from repository, ticket, comment, relation, and benchmark evidence.
- The durable refinement contract is refreshed to supersede the earlier closure-only wording and to state that P1.04 remains an evidence-gap backlog item.
- The contract states the current Oracle save boundary clearly enough for PO-critic review: Oracle.EntityFrameworkCore, clean context, no multi-active satellites, minimum 50 total operations, maximum 10000 satellite operations, and provider-neutral fallback otherwise.
- No child-ticket split, relation change, attachment, or planning document is required for this refinement because the canonical planning surface already carries the correct backlog posture and the downstream implementation ticket already covers the accept-or-close outcome.

## Implementation Notes
- src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs returns DirectOracleBatching when the Oracle gate passes and returns ProviderNeutralFallback when it fails; staged Oracle bulk remains not-selected-no-measured-win.
- src/DCoding.Data.DVault/DataVaultProviderSaveStrategyGateEvaluator.cs enforces the Oracle gate with supported provider name, clean context, no multi-active satellites, a minimum of 50 total operations, and a maximum of 10000 satellite operations.
- tests/DCoding.Data.DVault.Tests/Unit/OracleProviderOptimizationTests.cs and tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs already cover retained direct batching, fallback cases, and rollback behavior for the current Oracle path.
- benchmark-summary.md:71-72 shows the root Oracle provider-native-bulk-ingestion rows as skipped because DVAULT_TEST_ORACLE_CONNECTION_STRING is unset, so those rows remain boundary markers rather than completed timing proof.
- artifacts/benchmarks/v0.32.0-06F9XD2TGEYEG6S0AK86YF295M-oracle-high-volume-threshold-20260607/benchmark-summary.md records the checked-in keep-10000 decision, direct Oracle selected at 10000 satellite operations, and fallback at 100000 satellite operations via OracleMaximumSatelliteOperationThreshold.
- The current relation set can remain unchanged for this refinement because downstream ticket 06FBSCAJ5HDJH6CR0HZQ4B7H30 already says to implement only if the evaluation accepts an improvement and otherwise close no-work-required.

## Open Questions
- none

## Follow-Up Questions
- If a later provider-configured Oracle benchmark triplet shows a measured win for staged bulk or for a higher satellite threshold over both provider-neutral fallback and the retained direct Oracle path, should that reopen P1.04 as implementation work rather than pure evidence collection?
- After this narrowed recommendation is accepted, should downstream ticket 06FBSCAJ5HDJH6CR0HZQ4B7H30 be closed no-work-required unless new Oracle evidence changes the decision?

## Risks
- P1.04 remains an open evidence-gap backlog item, so product messaging must not present this ticket as closure of Oracle save benchmarking work.
- The root benchmark-summary Oracle rows are skipped placeholders; treating them as completed timing evidence would recreate the same canonical-planning mismatch that triggered the critic block.
- Oracle workloads above 10000 satellite operations or with multi-active satellite shapes still rely on fallback behavior and remain unproven optimization territory.
- The checked-in v0.32 high-volume artifact still shows the 100000-satellite fallback case as a reason not to widen the current Oracle path without fresh provider-configured evidence.

## Split Recommendations
- No split is required for this ticket; keep it as a bounded PO recommendation and evidence-boundary clarification.
- Do not create a backlog-alignment child ticket now because the canonical gap matrix already reflects the correct evidence-gap posture for P1.04.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Use the v0.39 evidence matrix to evaluate Oracle array binding, staging, and threshold gaps. Acceptance: produce a small recommendation: implement, tune threshold, document no-op, or defer with reason.