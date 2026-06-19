[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the persisted contract is clear, has no open questions, and matches current repository and branch evidence for a defer/no-work-required DB2 PIT/bridge evidence lane.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FBSCH65R88BT6PS7XV32NQ1M/description.md contains the authoritative delivery contract, with ## Open Questions set to none and acceptance criteria that explicitly keep DB2 PIT/bridge timing behind future environment-backed approval.
- git show --name-only 19651c9f8, 0c951823d18f, and 51d413e9b touches only .gicket/tickets/06FBSCH65R88BT6PS7XV32NQ1M/... files, confirming the refinement pass changed ticket metadata only and introduced no repository implementation work.
- benchmark-summary.md:87-89 records DB2 latest-satellite-read as skipped with providerSpecificReadStrategy=not registered for latest satellite reads, and DB2 pit-as-of-read / bridge-traversal-read as skipped placeholder rows naming planned Db2DataVaultReadStrategy.
- docs/plans/provider-optimization-evidence-matrix.md rows 268-271 classify DB2 latest-satellite as no optimization claim, DB2 PIT/bridge as skipped-placeholder, and DB2 PIT/bridge smoke as diagnostics-only and smoke-only, not completed timing evidence.
- docs/plans/provider-optimization-gap-matrix.md rows P2.05 and P3.05 keep DB2 PIT and bridge as evidence gaps gated by configured DB2 environment, maintained read models, complete read-shape evidence, and diagnostics selection.
- src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs registers Db2DataVaultReadStrategy only for PIT and bridge, while docs/architecture/dvault-v1-pit-bridge-boundary.md and docs/releases/v0.34.0.md state DB2 latest-satellite stays provider-neutral.
- tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs and tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs directly cover opt-in DB2 smoke behavior plus PIT/bridge gate selection and fail-closed fallback boundaries.
- find .gicket/tickets/06FBSCH65R88BT6PS7XV32NQ1M -maxdepth 1 -type d shows only the ticket root, comments, and events; sampled comment files comments/06FD6EE67KPXHGARS243J1DT9M.md, comments/06FDTPDG42GRT0ADGTDTMNDC7R.md, and comments/06FDTQ3QTQDNSX79253VK8WBFM.md are all bot-authored, matching the contract's automation-only comment claim.
- .gicket/relations/8G/1M/06FBSCGBG8CJ0QNRX4JZJA638G--06FBSCH65R88BT6PS7XV32NQ1M--blocks.json and .gicket/relations/1M/PG/06FBSCH65R88BT6PS7XV32NQ1M--06FBSCHBJEYYERDPA7JN34Y8PG--blocks.json preserve the upstream audit and downstream documentation links described in the contract.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The contract assumes any future DB2 timing activation will arrive as a separate, explicitly approved environment-backed follow-up instead of widening this ticket in place.

AC / test suggestions
- If DB2 timing work is later reopened, copy the exact artifact triplet names benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json into the follow-up ticket so the evidence lane stays explicit.

Implementation watchouts
- Developer handoff should treat this ticket as a defer/no-work-required evidence gate, not as permission to add DB2 PIT/bridge timing claims or latest-satellite optimization on this branch.
- Any future DB2 activation must keep latest-satellite out of scope for this ticket and require maintained PIT/bridge rows plus a configured DB2 environment before timing claims are accepted.

Non-blocking notes
- The related documentation ticket 06FBSCHBJEYYERDPA7JN34Y8PG and upstream audit ticket 06FBSCGBG8CJ0QNRX4JZJA638G both remain todo; that is workflow context, not a ticket-quality blocker for this refined defer-lane contract.

Split recommendations
- No split now; if DB2 environment-backed evidence is later approved, open a new follow-up ticket for that benchmark/smoke lane instead of widening 06FBSCH65R88BT6PS7XV32NQ1M.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment