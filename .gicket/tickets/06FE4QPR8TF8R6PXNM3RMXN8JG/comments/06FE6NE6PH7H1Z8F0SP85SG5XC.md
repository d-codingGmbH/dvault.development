[gicket-bot] PO-critic review contract

Summary
- Ticket contract requires substantive product-owner changes before development.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4QPR8TF8R6PXNM3RMXN8JG/description.md has ## Open Questions set to - none and defines acceptance criteria around preserved PostgreSQL comparator evidence, fallback, diagnostics tokens, and downstream docs scope.
- git log --oneline -- .gicket/tickets/06FE4QPR8TF8R6PXNM3RMXN8JG shows only PO claim/handoff and PO-critic claim commits after 23d694202, so this branch remains in pre-development ticket-refinement state.
- benchmark-summary.csv:42 and benchmark-summary.md:75 record latest-satellite-read / PostgreSQL external provider as skipped with selectedStrategy=PostgresDataVaultReadStrategy, plannedReadStrategy=PostgresDataVaultReadStrategy, and readShape=LatestSatellite.
- artifacts/benchmarks/v0.31.0-all-providers-smoke-<redacted>/benchmark-summary.csv:33 and benchmark-summary.md:63 record the historical PostgreSQL latest-satellite comparator at mean 25.723 ms with selectedStrategy=<none> and fallbackCauses=NoProviderSpecificStrategyRegistered.
- src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs registers PostgresDataVaultReadStrategy, src/DCoding.Data.DVault.Postgres/PostgresDataVaultReadStrategy.cs gates it through DataVaultProviderReadStrategyGateEvaluator.EvaluatePostgres(...), and src/DCoding.Data.DVault/DataVaultRelationalPitBridgeReadStrategy.cs builds the shared ROW_NUMBER latest-row query.
- tests/DCoding.Data.DVault.Tests/Unit/PostgresProviderCapabilityTests.cs asserts the PostgreSQL latest-satellite SQL shape uses ROW_NUMBER(), and tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs asserts fallback on provider mismatch, unsupported link-parent satellites, and multi-active satellites.
- .gicket/tickets/06FE4QPR8TF8R6PXNM3RMXN8JG/comments/06FE6GM93A8GZBYQ0R94RV2Q54.md says the PO pass updated the durable refinement contract in the ticket description, but .gicket/tickets/06FE4QPR8TF8R6PXNM3RMXN8JG/description.md still says no description updates were materialized in this pass.

Blocking findings
- The authoritative delivery contract contains a factual contradiction about this pass: the Implementation Notes say no description updates were materialized, but the persisted PO run report in .gicket/tickets/06FE4QPR8TF8R6PXNM3RMXN8JG/comments/06FE6GM93A8GZBYQ0R94RV2Q54.md explicitly says the durable refinement contract in the ticket description was updated. That leaves the handoff contract internally inconsistent.

Required PO actions
- none

Open issues ledger
- critic-item-1 [blocking-finding] The authoritative delivery contract contains a factual contradiction about this pass: the Implementation Notes say no description updates were materialized, but the persisted PO run report in .gicket/tickets/06FE4QPR8TF8R6PXNM3RMXN8JG/comments/06FE6GM93A8GZBYQ0R94RV2Q54.md explicitly says the durable refinement contract in the ticket description was updated. That leaves the handoff contract internally inconsistent.

Missing examples / edge cases
- none

Risky assumptions
- The contract assumes equivalent preserved benchmark comparison is sufficiently concrete for developer handoff even if the preserved evidence is not the standard provider-configured artifact triplet.
- The contract assumes the historical incoming blocks link from done ticket 06FE4QP6FB892E7TJMB47A3MSR remains housekeeping only and will not be misread as an active prerequisite during execution.

AC / test suggestions
- When the PO fixes the contradiction, keep the acceptance text explicit that an evidence-backed retain-current decision must cite the exact preserved comparator artifact path or checked-in contract surface used.

Implementation watchouts
- Do not let the root skipped PostgreSQL latest-satellite row in benchmark-summary.* be cited as completed timing evidence; it is guidance only until a provider-configured run completes.
- Do not use the save-side latest-index-postgres-provider-default-container-3 artifact as proof of public latest-satellite read-strategy benefit.
- Keep PostgresDataVaultReadStrategy inside the current hub-parent, non-multi-active, provider-match gate so unsupported shapes continue to fall back through provider-neutral reads and diagnostics.

Non-blocking notes
- The contract's substantive scope is otherwise well bounded: Open Questions is none, downstream docs work is already split to 06FE4QRMXVGJVA65ZR5MZ817K8, and the repository already contains concrete strategy, diagnostics, artifact, and test evidence for the stated boundary.

Split recommendations
- No additional split is needed once the contract wording is corrected; the existing downstream documentation ticket 06FE4QRMXVGJVA65ZR5MZ817K8 already holds broader matrix and release-note promotion work.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment