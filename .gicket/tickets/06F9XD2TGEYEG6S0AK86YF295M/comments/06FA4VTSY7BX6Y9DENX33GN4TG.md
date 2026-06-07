[gicket-bot] PO-critic review contract

Summary
- Contract is coherent and evidence-backed for pre-development handoff: the ticket has no open questions, the Oracle boundary is grounded in existing code/tests, and the referenced v0.32.0 all-provider artifact gives developers a concrete baseline to evaluate.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F9XD2TGEYEG6S0AK86YF295M/description.md contains the authoritative delivery contract, five Acceptance Criteria bullets, four Definition of Done bullets, and an Open Questions section with the single entry `none`.
- src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs defines `MinimumOptimizedBatchOperationCount = 50`, `MaximumOptimizedSatelliteOperationCount = 10000`, and returns `DirectOracleBatching` with `StagedOracleBulkNotSelectedReason = not-selected-no-measured-win`.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs asserts Oracle gate requirements `MinimumTotalOperationCount == 50` and `MaximumSatelliteOperationCount == 10000`; tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs covers direct-path selection and rollback on provider failure.
- tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs preserves the Oracle guidance row with `selectedStrategy=OracleDataVaultSaveStrategy` and `stagedOracleBulk=not-selected-no-measured-win`.
- artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-<redacted>/benchmark-summary.md shows Oracle `customer-profile-scale-1000x10` optimized mean `849.163 ms` vs fallback `<redacted> ms`, and Oracle `customer-profile-scale-10000x10` optimized-baseline mean `<redacted> ms` with `saveStrategyStatus=ProviderNeutralFallback` and `fallbackCauses=OracleMaximumSatelliteOperationThreshold`; conventional EF for that same `10000x10` scenario is `<redacted> ms`.
- .gicket/relations/ZW/5M/06F9XD1T3TJK7NEBYNVT2JEPZW--06F9XD2TGEYEG6S0AK86YF295M--parentOf.json keeps the ticket under the provider-threshold parent story, and .gicket/relations/5M/NM/06F9XD2TGEYEG6S0AK86YF295M--06F8KZVRARQPG482YKCQ686PNM--blocks.json shows the downstream v0.32.0 documentation task is intentionally waiting on this Oracle decision.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- If the Oracle boundary changes, the contract does not currently require exact at-threshold and just-over-threshold examples for the new cap; the implementation should add those cases to the final benchmark/report coverage.

Risky assumptions
- The follow-up questions in the delivery contract are treated as post-implementation routing questions, not prerequisites for starting the evaluation work.
- The team will treat the incoming blocks relation from done ticket 06F9XD26D2MHVAKZ2GCZ67BEFC as closed history because the source ticket is done and the current ticket is not blocked.

AC / test suggestions
- If the cap changes, add explicit coverage for exactly-at-threshold and just-over-threshold Oracle batches so the final report proves both selection and fallback behavior around the new boundary.
- Keep the final artifact label and file layout aligned with docs/plans/performance-evidence-benchmark-artifact-contract.md, and make the report state clearly whether the outcome is before/after evidence or a no-change decision.
- Require the landed diagnostics/report output to show OracleMinimumOperationThreshold and OracleMaximumSatelliteOperationThreshold whenever they drive fallback, not only inside raw benchmark executionDetail strings.

Implementation watchouts
- Do not broaden the task into default staged Oracle bulk selection unless new measurements beat the retained direct path and the change comes with a bounded transaction/cleanup contract.
- Preserve the existing IDataVaultSaveService and OracleDataVaultSaveStrategy boundary: caller-owned transaction, rollback, cancellation, ordering, hash-key/hash-diff, load-timestamp, record-source, and idempotency semantics are already explicit in the contract and existing tests.
- Do not raise the 10000-satellite cap based only on intuition; the current all-provider artifact shows the 10000x10 Oracle high-volume lane falling back and the conventional EF row still materially faster than the optimized-baseline row at that scale.

Non-blocking notes
- No human discussion thread was found under .gicket/tickets/06F9XD2TGEYEG6S0AK86YF295M/comments/*.md; the visible comments are orchestration, lease, and PO refinement records only.
- The branch is still ticket-metadata-only, so the absence of code or artifact edits on this branch is not a PO blocker for this pre-development gate.
- The clarification line claiming that no description updates were materialized conflicts with git diff --stat develop..HEAD, which shows .gicket/tickets/06F9XD2TGEYEG6S0AK86YF295M/description.md changed; this is bookkeeping noise, not a scope ambiguity.

Split recommendations
- Keep this ticket as the Oracle threshold-evaluation lane under story 06F9XD1T3TJK7NEBYNVT2JEPZW; split only if a staged Oracle win requires temporary-object cleanup or a wider transaction contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment