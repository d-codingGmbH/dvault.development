[gicket-bot] PO-critic review contract

Summary
- Repository and ticket evidence now align: P1.04 stays open, the Oracle boundary is repository-backed, and the persisted contract has no unresolved `## Open Questions`, so the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FBSC9QSAAF0J1Y9K27ZAEPDC/description.md:7-18,31-52` sets PO handoff to `ready_for_po_critic`, keeps P1.04 open, and shows `## Open Questions` as `- none`.
- `.gicket/tickets/06FBSC9QSAAF0J1Y9K27ZAEPDC/comments/06FCXPB6YT3NP9GRRFXC7HK6SW.md` answers all four PO-critic checklist items and states the deliverable is ticket-level recommendation only.
- `git log --oneline -- .gicket/tickets/06FBSC9QSAAF0J1Y9K27ZAEPDC | head` includes prior handoff commit `7c29bd76c`, and `.gicket/tickets/06FBSC9QSAAF0J1Y9K27ZAEPDC/comments/06FCWF63ZWP8E59NABEH1QS42G.md` reports that commit refreshed the durable ticket description.
- `docs/plans/provider-optimization-gap-matrix.md:59` still defines P1.04 as an Oracle `provider-native-bulk-ingestion` evidence gap and keeps staged Oracle bulk at `not-selected-no-measured-win` with the 50-plus and 10000-satellite boundary.
- `src/DCoding.Data.DVault/DataVaultProviderSaveStrategyGateEvaluator.cs:143-154,217-263` evaluates Oracle only for `KnownProviderNames.Oracle` and requires provider-name match, clean context, no multi-active satellites, at least 50 total operations, and at most 10000 satellite operations.
- `src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs:84-102` returns `DirectOracleBatching` when the Oracle gate passes and `ProviderNeutralFallback` when it fails; `src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs:560-603,951-959` shows the retained optional `ArrayBindCount` array-binding path.
- `tests/DCoding.Data.DVault.Tests/Unit/OracleProviderOptimizationTests.cs:14-42,64-83,122-165` covers retained direct batching, fallback below 50 / above 10000 / multi-active shapes, and array-binding SQL behavior; `tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs:23-37,102-115` covers configured direct-path execution and rollback.
- `benchmark-summary.md:71-72` shows the root Oracle save rows are skipped because `DVAULT_TEST_ORACLE_CONNECTION_STRING` is unset, while `artifacts/benchmarks/v0.32.0-06F9XD2TGEYEG6S0AK86YF295M-oracle-high-volume-threshold-<redacted>/benchmark-summary.md:32-50` preserves the keep-10000 decision, selects direct Oracle at 10000 satellite operations, and falls back at <redacted> with `OracleMaximumSatelliteOperationThreshold`.
- `git diff --name-only b6df28384db89a88058b4fd32458e6f120347b22..HEAD` shows only `.gicket/tickets/06FBSC9QSAAF0J1Y9K27ZAEPDC/...` artifacts changed on this branch, which is consistent with a pre-development refinement-only ticket.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The ticket assumes any later attempt to widen the Oracle path or select staged bulk will be reopened through P1.04 or downstream ticket `06FBSCAJ5HDJH6CR0HZQ4B7H30`, not inferred from this keep-as-is recommendation.
- The contract depends on readers continuing to distinguish the skipped root Oracle placeholders in `benchmark-summary.md:71-72` from the completed provider-configured Oracle evidence in the checked-in v0.32 artifact.

AC / test suggestions
- When the developer handoff or closure note is written, cite both `benchmark-summary.md:71-72` and the v0.32 Oracle artifact lines 32-50 so later readers can see why P1.04 remains open despite the retained baseline.
- If downstream ticket `06FBSCAJ5HDJH6CR0HZQ4B7H30` is activated later, require a new provider-configured Oracle benchmark triplet that beats both provider-neutral fallback and the retained direct Oracle path at the claimed workload before accepting staged bulk or threshold changes.

Implementation watchouts
- Do not treat the root Oracle rows as measured timing evidence while `DVAULT_TEST_ORACLE_CONNECTION_STRING` is unset; they are skipped placeholders only.
- Keep the current Oracle boundary unchanged unless new evidence exists: `Oracle.EntityFrameworkCore`, clean context, no multi-active satellites, minimum 50 total operations, maximum 10000 satellite operations, provider-neutral fallback otherwise.
- The checked-in v0.32 Oracle artifact still shows conventional EF ahead of the retained direct Oracle path on the recorded large-batch comparisons, so an `accepted improvement` must mean evidence-backed improvement over the retained boundary, not just over fallback.

Non-blocking notes
- Related ticket `06FBSCAJ5HDJH6CR0HZQ4B7H30` is still `todo`, and `.gicket/tickets/06FBSCAJ5HDJH6CR0HZQ4B7H30/description.md:1` already says to implement only if the spike recommends an improvement and otherwise close no-work-required, so downstream cleanup is follow-up workflow rather than a blocker for this ticket.

Split recommendations
- No split recommended; this ticket is already bounded to evaluation-only scope, and related ticket `06FBSCAJ5HDJH6CR0HZQ4B7H30` already isolates any later implementation work.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment