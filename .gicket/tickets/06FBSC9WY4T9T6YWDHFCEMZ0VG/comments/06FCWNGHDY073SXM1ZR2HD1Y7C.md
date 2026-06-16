[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff: the ticket is tightly scoped to a recommendation-only DB2 P1.05 evaluation, the persisted contract has no open questions, and the cited repository evidence matches the current DB2 boundary.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FBSC9WY4T9T6YWDHFCEMZ0VG/description.md and comment .gicket/tickets/06FBSC9WY4T9T6YWDHFCEMZ0VG/comments/06FCWKY7FA4H5B1M9AJSXDPWGR.md both record ready_for_po_critic, and the description's Open Questions section is none.
- docs/plans/provider-optimization-gap-matrix.md row P1.05 names the exact gap as DB2 provider-native-bulk-ingestion, scopes it to Db2DataVaultSaveStrategy clean-context set-based saves, and stops when work would require staged DB2 bulk or provider-native chunk execution.
- src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs registers AddDVaultDb2() with Db2DataVaultSaveStrategy and Db2DataVaultReadStrategy, and src/DCoding.Data.DVault.Db2/Db2DataVaultSaveStrategy.cs is the visible optimized DB2 save implementation.
- src/DCoding.Data.DVault/DataVaultProviderSaveStrategyGateEvaluator.cs evaluates DB2 with minimumOperationCount null and maximumSatelliteOperationCount null, and GetKnownStrategyGateRequirements maps Db2DataVaultSaveStrategy only to ProviderNameMismatch, DirtyDbContext, and MultiActiveSatelliteOperations.
- benchmark-summary.json and benchmark-summary.csv keep the DB2 provider-native-bulk-ingestion optimized row visible as executionStatus skipped with skipReason not configured: DVAULT_TEST_DB2_CONNECTION_STRING is not set or empty.; the executionDetail includes selectedStrategy=Db2DataVaultSaveStrategy, db2SaveBoundary=clean-context-set-based, stagedBulkBoundary=not-supported, and cleanupBoundary=direct-provider-transaction.
- tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs asserts the same DB2 guidance tokens, and tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs proves representative DB2 hub/link/satellite saves plus PIT/bridge diagnostics as smoke evidence only, not timing evidence.
- docs/releases/v0.34.0.md, docs/performance-profiles.md, and docs/plans/provider-optimization-evidence-matrix.md all restate the same DB2 boundary: clean-context saves exist, but staged DB2 bulk, provider-native chunk execution, latest-satellite optimization, and completed DB2 timing do not.
- git log --oneline on the ticket branch shows only workflow commits after develop (4f1fa767e, cd855d2f7, c91c286b5, 75f5f7ecf), and git diff --name-only develop..HEAD lists only .gicket/tickets/06FBSC9WY4T9T6YWDHFCEMZ0VG/** changes, so the branch remains a metadata-only pre-development handoff surface.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The separate follow-up task 06FBSCAQGWFC9S98YCVDP4V7PC is treated as provisional and blocked; its existence is not being read as a committed implement decision for this evaluation ticket.
- No newer checked-in DB2 benchmark artifact or staged-bulk capability lands before dev starts; the current repository baseline still reports skipped DB2 timing rows and stagedBulkBoundary=not-supported.

AC / test suggestions
- Have the eventual dev handoff comment state one allowed outcome explicitly: document no-op or defer with reason, unless new checked-in evidence contradicts the current baseline.
- Require the final recommendation to cite P1.05, the DB2 optimized provider-native-bulk-ingestion row from benchmark-summary.json or benchmark-summary.csv, and the DB2 gate evidence from DataVaultProviderSaveStrategyGateEvaluator.cs.

Implementation watchouts
- Do not turn skipped-placeholder, diagnostics-only, or smoke-only DB2 evidence into completed timing or expanded provider-capability claims.
- Do not expand this ticket into DB2 latest-satellite work (P0.05), PIT or bridge evidence work (P2.05/P3.05), connection-string provisioning, benchmark reruns, or live-schema reading.

Non-blocking notes
- Current branch history is consistent with a pre-development evaluation ticket: compared with develop, only .gicket/tickets/06FBSC9WY4T9T6YWDHFCEMZ0VG/** changed on this branch.

Split recommendations
- No additional split is needed for this ticket; keep it as the bounded recommendation-only P1.05 evaluation.
- If the dev evaluation later lands on implement, use the separate follow-up task 06FBSCAQGWFC9S98YCVDP4V7PC rather than widening this ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment