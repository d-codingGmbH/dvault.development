[gicket-bot] PO-critic review contract

Summary
- Ticket is ready for developer handoff: the persisted contract is bounded, `## Open Questions` is `none`, and repository evidence already proves existing MySQL PIT/bridge strategy registration plus completed provider-configured smoke-read rows; the remaining work is documentation/evidence alignment.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs` registers `MySqlDataVaultReadStrategy` for both `IDataVaultProviderPitReadStrategy` and `IDataVaultProviderBridgeReadStrategy`, matching the ticket’s claim that this is not a new-strategy invention task.
- `src/DCoding.Data.DVault.MySql/MySqlDataVaultReadStrategy.cs` implements MySQL PIT and bridge gate checks through `DataVaultProviderReadStrategyGateEvaluator.EvaluateMySql(...)`, confirming that MySQL PIT/bridge dispatch already exists in source.
- `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-<redacted>/benchmark-summary.md` records MySQL `latest-satellite-read` as completed with `selectedStrategy=<none>` while MySQL `pit-as-of-read` and `bridge-traversal-read` are completed with `selectedStrategy=MySqlDataVaultReadStrategy`.
- The repository-root `benchmark-summary.md` still shows MySQL `latest-satellite-read`, `pit-as-of-read`, and `bridge-traversal-read` as `skipped` because `DVAULT_TEST_MYSQL_CONNECTION_STRING` is unset, which matches the contract’s root-baseline-versus-provider-configured distinction.
- `docs/plans/provider-optimization-evidence-matrix.md`, `docs/plans/provider-optimization-gap-matrix.md`, and `docs/performance-profiles.md` still describe MySQL PIT/bridge as `skipped-placeholder` / evidence-gap follow-up driven by the root triplet, so the contradiction this ticket is meant to close is directly present in the repository today.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs` keeps MySQL guidance rows with `latest-satellite-read -> selectedStrategy=<none>` and `pit-as-of-read` / `bridge-traversal-read -> selectedStrategy=MySqlDataVaultReadStrategy`, which is consistent with the bounded scope described in the ticket.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs` contains `MySqlAndOraclePitReadGatesFailClosedForProviderShapeEvidenceAndMaintenanceFallbacks` and `MySqlAndOracleBridgeReadGatesFailClosedForProviderShapeEvidenceAndMaintenanceFallbacks`, covering provider mismatch, unsupported shape, incomplete read-shape evidence, and stale maintenance fallback causes named in the contract.
- `git diff --name-only 766e1ce48..6d2ca069d` lists only `.gicket/...` files, so the current branch after PO refinement has ticket-metadata changes only; the repository docs/code surfaces named by the ticket are still awaiting developer work.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The ticket assumes the 2026-06-07 smoke-read bundle is acceptable closure evidence for MySQL PIT/bridge even though the current evidence/gap matrices still encode root-triplet-only posture; if reviewers reject smoke-read bundles as closure evidence, the ticket would need policy clarification rather than implementation discovery.
- The developer handoff must preserve the explicit out-of-scope boundary for MySQL `latest-satellite-read`; the same smoke-read bundle contains a completed MySQL latest-satellite row with provider-neutral fallback and could be misread if the closure text is imprecise.

AC / test suggestions
- Keep acceptance text explicit that repository docs/plans must cite the provider-configured smoke-read artifact path, not reinterpret the skipped root quick baseline as the authoritative MySQL PIT/bridge evidence surface.
- Treat consistency with `BenchmarkScenarioExecutionTests.cs` as part of acceptance: root guidance rows may remain skipped, while the documentation surfaces should point to the v0.32 smoke-read bundle for completed MySQL PIT/bridge evidence.

Implementation watchouts
- Do not broaden the ticket into MySQL latest-satellite optimization, new strategy types, new APIs, or PIT/bridge maintenance behavior changes.
- Do not convert the root `benchmark-summary.*` MySQL skipped rows into a rerun requirement if the accepted closure path is the checked-in v0.32 smoke-read bundle.
- Documentation updates need to remove the current contradiction across `docs/plans/provider-optimization-evidence-matrix.md`, `docs/plans/provider-optimization-gap-matrix.md`, and `docs/performance-profiles.md` without turning smoke-read evidence into universal MySQL timing claims.

Non-blocking notes
- Ticket comments currently visible under `.gicket/tickets/06FBSCGVAZ5G8NP1TRXFNEP6DW/comments/` are automation, lease, and handoff records; I did not find substantive stakeholder comments that add new requirements beyond the persisted contract.
- The branch history at `6d2ca069d`, `6a0eb7906`, and `b66003156` is ticket-handoff metadata history, which is consistent with this being a pre-development PO gate rather than completed delivery.

Split recommendations
- No split recommended. The current contract already keeps the work bounded to one evidence-alignment task around existing MySQL PIT/bridge proof surfaces.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment