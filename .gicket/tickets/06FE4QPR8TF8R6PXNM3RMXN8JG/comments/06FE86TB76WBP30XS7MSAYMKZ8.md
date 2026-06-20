[gicket-bot] PO-critic review contract

Summary
- Prior PO-critic contradiction is resolved, Open Questions is none, and the ticket is now bounded by concrete repository evidence for PostgreSQL latest-satellite developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4QPR8TF8R6PXNM3RMXN8JG/description.md:7-9 sets PO Handoff to ready_for_po_critic; :49-53 sets Open Questions to none and leaves only a Follow-Up Question about downstream docs attachment.
- .gicket/tickets/06FE4QPR8TF8R6PXNM3RMXN8JG/comments/06FE6NE6PH7H1Z8F0SP85SG5XC.md:5-19 is the prior PO-critic return_to_po for a description-update contradiction; .gicket/tickets/06FE4QPR8TF8R6PXNM3RMXN8JG/comments/06FE82NDSP4HNJ0PGD54GHZGN4.md:7-11 records critic-item-1 answered and the contradiction resolved.
- git diff --name-only 2ef1dae8a..HEAD lists only .gicket/tickets/06FE4QPR8TF8R6PXNM3RMXN8JG/**, and git log --max-count=6 shows only PO/PO-critic claim and handoff commits through 52bfcb073, so the branch is still in pre-development ticket-refinement state.
- benchmark-summary.csv:42 records the current PostgreSQL latest-satellite root row as executionStatus=skipped with selectedStrategy=PostgresDataVaultReadStrategy, plannedReadStrategy=PostgresDataVaultReadStrategy, readShape=LatestSatellite, and persistedOutcome=not executed.
- artifacts/benchmarks/v0.31.0-all-providers-smoke-<redacted>/benchmark-summary.csv:33 records the historical PostgreSQL latest-satellite comparator as completed at mean 25.723 ms with selectedStrategy=<none> and fallbackCauses=NoProviderSpecificStrategyRegistered.
- src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:15-26 registers PostgresDataVaultReadStrategy; src/DCoding.Data.DVault.Postgres/PostgresDataVaultReadStrategy.cs:10-35 gates latest-satellite, PIT, and bridge reads through EvaluatePostgres(...); src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs:728-732 fixes latest-satellite fallback requirements to provider-name mismatch, unsupported satellite parent, and multi-active satellite unsupported.
- src/DCoding.Data.DVault/IDataVaultReadDiagnosticsService.cs:14-49 confirms the IDataVaultReadDiagnosticsService type named in the contract exists in source; tests/DCoding.Data.DVault.Tests/Unit/PostgresProviderCapabilityTests.cs:102-130 asserts the current ROW_NUMBER latest-satellite SQL shape.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:155-197 asserts the supported hub-parent PostgreSQL path plus fallback on provider mismatch, link-parent satellites, and multi-active satellites; tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:452-456 and :<redacted> preserve plannedReadStrategy and readShape tokens for provider read rows.
- docs/plans/provider-optimization-evidence-matrix.md:10,16-17,27-30 and docs/plans/provider-read-parity-outcomes-benchmarks-refinement.md:15-23 align with the ticket contract: non-SQLite latest-satellite rows are skipped-placeholder or guidance only, and completed timing claims require preserved provider-configured artifact evidence.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The contract allows an equivalent preserved comparator instead of the standard provider-configured artifact triplet; developers still need to keep that comparator explicit enough to satisfy description.md:19, :30, and :40.
- The historical relation .gicket/relations/SR/JG/06FE4QP6FB892E7TJMB47A3MSR--06FE4QPR8TF8R6PXNM3RMXN8JG--blocks.json remains present even though ticket 06FE4QP6FB892E7TJMB47A3MSR is done; the contract assumes that relation remains housekeeping-only.

AC / test suggestions
- If the implementation retains the current ROW_NUMBER query, require final delivery evidence to cite the exact preserved comparator path used for the retain-current decision, not just a prose statement.
- Keep benchmark and diagnostics verification on selectedStrategy, plannedReadStrategy, readShape=LatestSatellite, executionStatus, and fallback causes aligned with tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:452-456 and :<redacted>.
- Keep gate coverage for provider mismatch, link-parent satellites, and multi-active satellites aligned with tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:155-197.

Implementation watchouts
- Do not promote benchmark-summary.csv:42 from skipped-placeholder guidance into completed PostgreSQL timing evidence.
- Do not use save-side latest-index PostgreSQL artifacts as proof of public latest-satellite read improvement.
- Keep PostgresDataVaultReadStrategy inside the current provider-match, hub-parent, and non-multi-active gate so unsupported shapes continue to fall back through the provider-neutral path.

Non-blocking notes
- git diff --name-only 2ef1dae8a..HEAD lists only ticket metadata files, which is consistent with a pre-development handoff branch rather than an implementation branch.

Split recommendations
- No additional split is needed; provider-specific latest-satellite tuning stays isolated in this ticket and broader documentation promotion remains in 06FE4QRMXVGJVA65ZR5MZ817K8.
- If the historical done-ticket blocks relation causes routing noise later, clean it up as separate relation housekeeping instead of widening this ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment