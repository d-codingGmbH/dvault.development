[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the persisted contract is clear, `## Open Questions` is `none`, and the cited repository evidence consistently separates Oracle latest-satellite capability from missing timing evidence.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FE4QQJCJH7J9AWQTPDR5DSSG/description.md:7-9` marks PO handoff `ready_for_po_critic`, and `:50-51` sets `## Open Questions` to `- none`.
- `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs:21-26` registers `OracleDataVaultReadStrategy` for `IDataVaultProviderReadStrategy`, `IDataVaultProviderPitReadStrategy`, and `IDataVaultProviderBridgeReadStrategy`.
- `src/DCoding.Data.DVault.Oracle/OracleDataVaultReadStrategy.cs:215-255` generates `ROW_NUMBER() OVER (PARTITION BY ParentHashKey ORDER BY LoadTimestamp DESC)`, batched `IN (...)` parent-hash-key filters, and an optional `<= asOf` predicate.
- `src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs:855-870` defines latest-satellite fallback causes for provider mismatch, non-hub parents, and multi-active driving keys; `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:200-245` and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs:54-96` cover Oracle gate acceptance/fail-closed behavior and parity against provider-neutral fallback.
- `benchmark-summary.csv:51` shows Oracle `latest-satellite-read` is currently `skipped` because `DVAULT_TEST_ORACLE_CONNECTION_STRING` is unset, while still carrying `selectedStrategy=OracleDataVaultReadStrategy` and `plannedReadStrategy=OracleDataVaultReadStrategy`.
- `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-<redacted>/benchmark-summary.csv:49-51` shows the 2026-06-07 Oracle smoke-read bundle completed `latest-satellite-read` through provider-neutral fallback with `selectedStrategy=<none>`, but completed PIT/bridge with `OracleDataVaultReadStrategy`.
- `docs/plans/provider-optimization-gap-matrix.md:87`, `docs/plans/provider-optimization-evidence-matrix.md:289-291`, `docs/releases/v0.41.0.md:60-62`, and `docs/releases/v0.32.0.md:70` preserve the same boundary: Oracle latest-satellite timing is still an evidence gap, while Oracle PIT/bridge completed timing is limited to the v0.32 smoke-read bundle.
- `git diff --name-only develop..HEAD` returns only `.gicket/tickets/06FE4QQJCJH7J9AWQTPDR5DSSG/*`, so the current branch is ticket-metadata-only and relies on existing repository evidence rather than new implementation changes.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Assuming Oracle PIT/bridge completed timing from the v0.32 smoke-read bundle also proves Oracle latest-satellite timing would overclaim the current evidence boundary.
- Assuming the root `benchmark-summary.csv` Oracle latest-satellite row is a timing result instead of a skipped guidance row would misread the benchmark contract.
- Assuming later PIT tuning work can widen latest-satellite support beyond Oracle provider, hub-parent satellites, and non-multi-active driving keys would conflict with the current gate logic.

AC / test suggestions
- Closure evidence should cite `benchmark-summary.csv:51` together with the smoke-read artifact `benchmark-summary.csv:49-51` so the deferral decision is explicitly tied to both the current skipped placeholder and the historical configured fallback run.
- If a downstream ticket cites 'diagnostics not selecting OracleDataVaultReadStrategy', include an actual diagnostics artifact or the existing gate/parity tests (`DataVaultProviderReadStrategyTests`, `DataVaultRelationalPitBridgeReadStrategyParityTests`) instead of prose-only reasoning.
- Keep release/doc evidence aligned with `docs/plans/provider-optimization-evidence-matrix.md:289-291` and `docs/releases/v0.41.0.md:60-62` so Oracle PIT/bridge timing is not reused as latest-satellite timing proof.

Implementation watchouts
- Do not broaden Oracle latest-satellite claims beyond supported hub-parent, non-multi-active satellites; gate evaluator fallback remains required for provider mismatch, link-parent satellites, and multi-active driving keys.
- Treat the root Oracle latest-satellite row as planned-strategy/row-identity guidance only until a provider-configured Oracle latest-satellite benchmark lane exists.
- Do not use the 2026-06-07 Oracle smoke-read latest-satellite row as optimized-strategy evidence; it completed via provider-neutral fallback with `selectedStrategy=<none>`.
- Oracle PIT and bridge timing can support PIT/bridge-specific work, but any PIT tuning claim that depends on Oracle latest-satellite performance still needs separate configured evidence.

Non-blocking notes
- The PO refinement comment `.gicket/tickets/06FE4QQJCJH7J9AWQTPDR5DSSG/comments/06FE64BJJJW5EMBKVQ0N4VD4BC.md:3-28` matches the authoritative description contract and already frames the ticket as an evidence-and-decision task rather than an implementation ticket.
- No additional split is indicated by branch history; the branch delta from `develop` is confined to `.gicket` ticket artifacts for this ticket.

Split recommendations
- No split recommended; keep Oracle latest-satellite follow-up bounded to the existing evidence-gap lane in `docs/plans/provider-optimization-gap-matrix.md:87` (`P0.04`).

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment