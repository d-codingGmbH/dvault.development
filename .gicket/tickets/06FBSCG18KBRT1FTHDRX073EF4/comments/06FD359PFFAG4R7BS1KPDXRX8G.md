[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the delivery contract is bounded, `## Open Questions` is `none`, and repository evidence matches the stated Oracle latest-satellite capability gap.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Ticket `06FBSCG18KBRT1FTHDRX073EF4` delivery contract marks `PO Handoff` as `ready_for_po_critic` and `## Open Questions` as `none`; recent comments and closure evidence amendments in the snapshot are both `<none>`.
- `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs` registers `OracleDataVaultReadStrategy` only as `IDataVaultProviderPitReadStrategy` and `IDataVaultProviderBridgeReadStrategy`; there is no `IDataVaultProviderReadStrategy` registration for Oracle latest-satellite reads.
- `src/DCoding.Data.DVault.Oracle/OracleDataVaultReadStrategy.cs` inherits `DataVaultRelationalPitBridgeReadStrategy`, and `src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs` only recognizes SQLite and SQL Server for latest-satellite strategy evaluation/gate requirements; Oracle appears only in the PIT/bridge paths.
- `tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs` `RelationalProviderPackagesRegisterOptimizedReadStrategies` expects `AddDVaultOracle()` to register PIT/bridge strategy types, but only SQL Server supplies `expectedLatestSatelliteStrategyName`.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs`, `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` all encode Oracle `latest-satellite-read` with `selectedStrategy=<none>` and `providerSpecificReadStrategy=not registered for latest satellite reads`, while Oracle PIT/bridge rows plan `OracleDataVaultReadStrategy`.
- `docs/plans/provider-optimization-gap-matrix.md`, `docs/plans/provider-optimization-evidence-matrix.md`, `docs/performance-profiles.md`, and `docs/architecture/dvault-v1-pit-bridge-boundary.md` consistently describe Oracle latest-satellite as the remaining capability gap / provider-neutral fallback path.
- `git show --no-patch --oneline HEAD` shows HEAD is `42273be58` (`[06FBSCG18KBRT1FTHDRX073EF4] lease claim po-critic`), `git diff --name-only 42273be586d1c8807a883418828e45a5c57e0214..HEAD` is empty, and `git rev-list --left-right --count 42273be586d1c8807a883418828e45a5c57e0214...HEAD` returned zero left/right commits.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Keep at least one explicit Oracle unsupported-shape example in the ticket-linked evidence set: a link-parent latest-satellite request and a multi-active driving-key satellite request should both remain provider-neutral fallback.

Risky assumptions
- The ticket assumes Oracle latest-satellite support can reuse the existing Oracle read-strategy boundary without widening beyond hub-parent, non-multi-active current/as-of semantics; current source only proves PIT/bridge registration today, so parity may still force the ticket into the contract's explicit no-work-required outcome.
- The ticket examples name the main benchmark and matrix surfaces, but repo-wide search also finds current 'not registered' Oracle latest-satellite statements in `benchmarks/DCoding.Data.DVault.Benchmarks/README.md`, `docs/production-adoption-checklist.md`, and `docs/releases/v0.28.0.md`; the developer sweep must treat the acceptance criterion's 'no checked-in document still claiming' language as the true boundary.

AC / test suggestions
- Keep one registration assertion that `AddDVaultOracle()` now exposes `OracleDataVaultReadStrategy` through `IDataVaultProviderReadStrategy` while PIT/bridge registrations remain intact.
- Keep diagnostics coverage for Oracle latest-satellite current and as-of requests that proves strategy selection on supported hub-parent shapes and finite fallback causes for provider mismatch, unsupported satellite parents, and multi-active satellites.
- Keep benchmark/verifier coverage that updates the Oracle `latest-satellite-read` guidance row from `selectedStrategy=<none>` to the planned/selected Oracle strategy while preserving skipped-placeholder behavior and normalized skip reasons when `DVAULT_TEST_ORACLE_CONNECTION_STRING` is unset.

Implementation watchouts
- Repo evidence is duplicated across code, tests, benchmark artifacts, benchmark README, performance guidance, architecture docs, release notes, and adoption docs; partial updates will leave stale Oracle latest-satellite gap claims behind.
- The current ticket branch contains no implementation beyond the PO-critic lease claim commit, so the developer will be starting from the baseline gap state described by the ticket.
- Benchmark execution-detail generation currently hardcodes the non-registered latest-satellite posture in `benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkExecutionDetails.cs`, so diagnostics/registration changes alone will not make the evidence surfaces consistent.

Non-blocking notes
- This is a clean pre-development handoff: the branch is unchanged from the supplied scratch ref, so there is no conflicting in-progress Oracle latest-satellite implementation to reconcile.
- The follow-up questions in the contract are backlog/release-routing questions rather than delivery blockers.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment