[gicket-bot] PO-critic review contract

Summary
- Return to PO: the story's external-provider benchmark scope is not sufficiently bounded against current repository capabilities, so developers could implement incompatible provider sets or skip semantics.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted ticket 06EZ0NCAFFJSSRFFEG66AYG8XC has `## Open Questions` = `none` in `.gicket/tickets/06EZ0NCAFFJSSRFFEG66AYG8XC/description.md`, so approval is not blocked by unresolved contract questions.
- Branch history for `ticket/06EZ0NCAFFJSSRFFEG66AYG8XC-story-consolidate-provider-benchmark-reporting` contains only workflow commits (`acddce88`, `154409fb`, `128ac719`, `ba16e7d1`) after `develop`; there are no ticket implementation commits on this branch yet (`git --no-pager log --oneline --decorate --left-right --cherry-pick develop...HEAD --max-count 20`).
- `docs/architecture/dvault-v1-explicit-save-service.md` says SQLite benchmark coverage is required, PostgreSQL has an optimized strategy but benchmark coverage is `No`, and SQL Server/Oracle/MySQL are compatibility-only with benchmark coverage `No`.
- `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:15-19` registers `PostgresDataVaultSaveStrategy`, but `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:14-17`, `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs:14-17`, and `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:14-17` only call `services.AddDVault()` and register no provider-specific strategy.
- `benchmarks/DCoding.Data.DVault.Benchmarks/README.md:9-24` and `BenchmarkRunner.cs:36-39` describe a SQLite-only local runner; `BenchmarkOptions.cs:16-30` only supports `--iterations`, `--warmup`, and `--output`.
- `BenchmarkArtifacts.cs:50-51`, `133-156`, and `BenchmarkRunner.cs:114-125` show the current artifact schema has scenario/provider/baseline/strategy/datasetSize/changeRatio/iterations/timing/persistedOutcome, but no execution-status field or skipped-provider row model.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:12-191` asserts only the fixed provider `SQLite local temporary files` and 12 SQLite rows; no external-provider or skipped-provider expectations exist.
- Story 06EZ0NCAFFJSSRFFEG66AYG8XC is `parentOf` task 06EZ0NCGYCADKEYGR16J5PJFS0 and that child ticket is already `done`; the child contract explicitly bounded emitted artifacts to the existing SQLite harness and left external-provider expansion to follow-up work.

Blocking findings
- The story does not explicitly bound which external providers are in scope for fallback-versus-optimized comparison. Repository evidence shows only PostgreSQL currently has an external optimized strategy, while SQL Server/Oracle/MySQL are compatibility-only. As written, `any configured external providers` can be read to include providers that cannot satisfy the `fallback` versus `optimized` comparison acceptance criterion.
- The story does not ratify the benchmark-side discovery/configuration contract for external providers. Current benchmark surfaces are SQLite-only, while opt-in external configuration evidence exists only in tests (`DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `DVAULT_TEST_ORACLE_CONNECTION_STRING`). Without a defined benchmark discovery source, `configured`, `reachable`, and `skipped` remain implementer-defined.

Required PO actions
- State the exact v1 external-provider set for this story. Example boundary: `SQLite required`, `PostgreSQL optional opt-in external`, and `SQL Server/Oracle/MySQL out of scope` or `skip-only`.
- Define how the benchmark runner determines that an external provider is configured for this story: reuse named env vars, add explicit CLI/options input, or another concrete contract.
- Clarify expected artifact behavior when a provider package exists but no optimized strategy exists: fallback-only row, skipped optimized row with reason, or provider out of scope.

Open issues ledger
- critic-item-1 [required-po-action] State the exact v1 external-provider set for this story. Example boundary: `SQLite required`, `PostgreSQL optional opt-in external`, and `SQL Server/Oracle/MySQL out of scope` or `skip-only`.
- critic-item-2 [required-po-action] Define how the benchmark runner determines that an external provider is configured for this story: reuse named env vars, add explicit CLI/options input, or another concrete contract.
- critic-item-3 [required-po-action] Clarify expected artifact behavior when a provider package exists but no optimized strategy exists: fallback-only row, skipped optimized row with reason, or provider out of scope.
- critic-item-4 [blocking-finding] The story does not explicitly bound which external providers are in scope for fallback-versus-optimized comparison. Repository evidence shows only PostgreSQL currently has an external optimized strategy, while SQL Server/Oracle/MySQL are compatibility-only. As written, `any configured external providers` can be read to include providers that cannot satisfy the `fallback` versus `optimized` comparison acceptance criterion.
- critic-item-5 [blocking-finding] The story does not ratify the benchmark-side discovery/configuration contract for external providers. Current benchmark surfaces are SQLite-only, while opt-in external configuration evidence exists only in tests (`DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `DVAULT_TEST_ORACLE_CONNECTION_STRING`). Without a defined benchmark discovery source, `configured`, `reachable`, and `skipped` remain implementer-defined.

Missing examples / edge cases
- Configured PostgreSQL connection string present, but provider package is not restored or provider initialization fails before execution.
- Configured provider is reachable, but `CanSave` rejects the scenario/context so only fallback is executable.
- SQLite-only run with no external opt-in configured: which provider identities must still appear as skipped rows, if any.
- Compatibility-only providers (SQL Server/Oracle/MySQL) under the current package family: are they expected to appear in the artifact at all for this story.

Risky assumptions
- Assuming `any configured external providers` implicitly means only providers that already expose a provider-specific optimized strategy.
- Assuming benchmark discovery should reuse test-only env vars rather than a benchmark-specific configuration surface.
- Assuming skip reasons can vary freely even though the contract wants archive-stable release evidence.

AC / test suggestions
- Add an AC that names the in-scope external providers explicitly.
- Add an AC or note that normalizes execution-status/skip-reason values for archive-stable artifacts.
- Require at least one test that proves the SQLite-only run still emits deterministic external-provider skip behavior for the approved in-scope provider set.
- If env vars are the intended config surface, name them directly in the contract and docs.

Implementation watchouts
- Keep benchmark logic in the benchmark/documentation surface; do not introduce provider-name branching into `src/DCoding.Data.DVault` beyond the existing strategy boundary.
- PostgreSQL is opt-in and externally provisioned today, so benchmark execution cannot assume the provider package or database is always available locally.
- The existing artifact contract is already consumed across markdown/CSV/JSON; adding status/skip semantics needs a stable additive shape.

Non-blocking notes
- The persisted contract has no unresolved `## Open Questions`.
- Blocking story 06EZ0N8HW9PZAFKMM5WQD564VR is already `done`.
- Child task 06EZ0NCGYCADKEYGR16J5PJFS0 is already `done`, so this story should focus only on the remaining external-provider/skip-reporting scope.

Split recommendations
- If PO wants more than PostgreSQL beyond SQLite, split external-provider expansion by provider or by infra/discovery versus artifact-shape work.
- Do not reopen the completed SQLite artifact work from child ticket 06EZ0NCGYCADKEYGR16J5PJFS0 inside this story unless the parent contract is deliberately narrowed to remaining gap work.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment