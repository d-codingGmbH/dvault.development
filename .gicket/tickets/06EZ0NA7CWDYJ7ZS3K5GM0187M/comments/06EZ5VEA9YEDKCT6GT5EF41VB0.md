[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706EZ0NA7CWDYJ7ZS3K5GM0187M\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06EZ0NA7CWDYJ7ZS3K5GM0187M-task-add-opt-in-postgresql-integration-coverage\u0027 and commit \u00277523b55964b2\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06EZ0NA7CWDYJ7ZS3K5GM0187M-task-add-opt-in-postgresql-integration-coverage\u0027 from source \u00277523b55964b2\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06EZ0NA7CWDYJ7ZS3K5GM0187M-task-add-opt-in-postgresql-integration-coverage\u0027.",
    "Evidence: git rev-parse HEAD returned 7523b55964b23b28c559880972c78407bb79bcfe.",
    "Evidence: git diff --name-status develop...7523b55964b2 shows the implementation-side diff only changed tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj, added tests/DCoding.Data.DVault.Tests/Integration/PostgresOptimizedDataVaultSaveServiceTests.cs, and modified tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs; no src/DCoding.Data.DVault.Postgres implementation file changed in the claimed delivery diff.",
    "Evidence: src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs at commit 7523b55964b2 still contains only services.AddDVault(); return services; with no IDataVaultProviderSaveStrategy registration.",
    "Evidence: rg over src/ and tests/ finds a SqliteDataVaultSaveStrategy registration in src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs but no PostgreSQL strategy implementation or registration, while tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs still asserts services.AddDVaultPostgres() uses expectProviderStrategy: false.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/PostgresOptimizedDataVaultSaveServiceTests.cs adds the intended opt-in coverage shape: skip-on-missing-config, AddDVaultPostgres() service resolution, AssertCompatiblePostgresStrategy(...), AssertOptimizedPathObserved(...), and persisted hub/link/satellite assertions including unchanged and changed satellite history cases.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj keeps PackageReference Include=\u0022Npgsql.EntityFrameworkCore.PostgreSQL\u0022 behind Condition=\u0022\u0027$(DVAULT_TEST_POSTGRES_CONNECTION_STRING)\u0027 != \u0027\u0027\u0022 and adds a ProjectReference to src/DCoding.Data.DVault.Postgres/DCoding.Data.DVault.Postgres.csproj; that provider project itself only references Microsoft.Extensions.DependencyInjection.Abstractions and the core DVault project.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/postgres, area/tests, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 5 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06EZ0NA7CWDYJ7ZS3K5GM0187M-task-add-opt-in-postgresql-integration-coverage\u0027.",
    "Evidence: Ticket history references implementation commit \u00277523b55964b2\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: When DVAULT_TEST_POSTGRES_CONNECTION_STRING is absent or blank, the PostgreSQL optimized-path suite skips with the existing configuration/provider messages instead of failing or requiring PostgreSQL on default machines. (tests/DCoding.Data.DVault.Tests/Integration/PostgresOptimizedDataVaultSaveServiceTests.cs reads PostgresIntegrationTestConfiguration.FromEnvironment() and calls Assert.Skip(PostgresIntegrationTestConfiguration.MissingConfigurationSkipMessage) when the connection string is absent or blank; PostgresIntegrationTestConfiguration.Normalize() trims whitespace to null.).",
    "AC check passed: The default local test run remains green without PostgreSQL installed and without restoring Npgsql unless the opt-in configuration is supplied. (tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj still restores Npgsql.EntityFrameworkCore.PostgreSQL only when $(DVAULT_TEST_POSTGRES_CONNECTION_STRING) is non-empty, the new test skips when configuration is absent, and src/DCoding.Data.DVault.Postgres/DCoding.Data.DVault.Postgres.csproj adds no unconditional Npgsql dependency baseline.).",
    "DoD check passed: Integration coverage lives under tests/DCoding.Data.DVault.Tests/Integration and is categorized as ProviderIntegration.ExternalOptIn plus Postgres. (The added coverage lives at tests/DCoding.Data.DVault.Tests/Integration/PostgresOptimizedDataVaultSaveServiceTests.cs and ProviderIntegrationCategoryDiscoveryTests.cs now requires it to carry ProviderIntegration.ExternalOptIn plus Postgres traits.).",
    "DoD check passed: Tests reuse PostgresIntegrationTestConfiguration, NpgsqlProviderReflection, and the existing live-schema cleanup patterns instead of introducing a second PostgreSQL harness. (The new test reuses PostgresIntegrationTestConfiguration, NpgsqlProviderReflection.UseNpgsql(...), and the existing create-schema / drop-schema cleanup pattern already used by PostgresDataVaultSchemaTests instead of introducing a second PostgreSQL harness.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: When PostgreSQL is configured and task 06EZ0NA180RA0FQ64KXQTHEVZW has supplied the optimized provider strategy, the opt-in suite resolves services through AddDVaultPostgres(), confirms a compatible IDataVaultProviderSaveStrategy accepts the clean Npgsql-backed context for representative hub, link, and satellite save requests, and then executes those requests against live PostgreSQL. (The new integration test asserts a compatible IDataVaultProviderSaveStrategy before each hub/link/satellite save, but direct repository evidence shows src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs still only calls services.AddDVault() and returns, and tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs still expects services.AddDVaultPostgres() to register no provider strategy (expectProviderStrategy: false). The optimized strategy surface this ticket depends on is not present in the delivered work.).",
    "AC check failed: For each optimized-path scenario, the same DbContext proves fallback was not used by showing no leftover tracked hub, link, or satellite entries after the save; persisted tables still show the expected insert-only hub/link/satellite outcomes. (tests/DCoding.Data.DVault.Tests/Integration/PostgresOptimizedDataVaultSaveServiceTests.cs contains zero-tracked-entry and persisted-row assertions, but those checks are not wired to a working PostgreSQL optimized path because AddDVaultPostgres() still exposes no provider save strategy in this branch.).",
    "AC check failed: Satellite history coverage proves unchanged hash-diff replays do not append a row and changed hash diffs append exactly one new satellite history row while preserving earlier history. (The new test encodes unchanged-hash and changed-hash satellite history expectations, but the repository still lacks the PostgreSQL optimized strategy registration required for that live optimized-path suite to be a satisfiable delivered artifact.).",
    "DoD check failed: The completed suite proves optimized-path selection with strategy-acceptance and no-fallback-tracking assertions in addition to persisted-behavior checks; RowsWritten or persisted rows alone are not the sole proof. (The suite text includes strategy-acceptance and no-fallback-tracking assertions, but the delivered branch still does not register any PostgreSQL IDataVaultProviderSaveStrategy behind AddDVaultPostgres(), so the completed repository state does not actually prove an optimized path.).",
    "DoD check failed: No public API broadening or mandatory local dependency baseline is introduced, and the ticket is only considered done once sibling task 06EZ0NA180RA0FQ64KXQTHEVZW provides an optimized strategy surface that makes these tests pass. (No public API broadening is apparent in the diff, but this definition of done explicitly requires the sibling optimized strategy surface to exist and make the tests pass. Current repository evidence still shows that surface is missing.).",
    "The claimed delivery adds a PostgreSQL optimized-path test, but the branch still lacks the PostgreSQL optimized provider strategy registration that the test requires, so the new coverage is structurally unwired.",
    "Existing unit coverage still codifies the opposite contract for AddDVaultPostgres() (no provider strategy registered), which directly conflicts with the new integration test\u0027s prerequisite and blocks tester acceptance."
  ],
  "evidence": [
    "git rev-parse HEAD returned 7523b55964b23b28c559880972c78407bb79bcfe.",
    "git diff --name-status develop...7523b55964b2 shows the implementation-side diff only changed tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj, added tests/DCoding.Data.DVault.Tests/Integration/PostgresOptimizedDataVaultSaveServiceTests.cs, and modified tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs; no src/DCoding.Data.DVault.Postgres implementation file changed in the claimed delivery diff.",
    "src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs at commit 7523b55964b2 still contains only services.AddDVault(); return services; with no IDataVaultProviderSaveStrategy registration.",
    "rg over src/ and tests/ finds a SqliteDataVaultSaveStrategy registration in src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs but no PostgreSQL strategy implementation or registration, while tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs still asserts services.AddDVaultPostgres() uses expectProviderStrategy: false.",
    "tests/DCoding.Data.DVault.Tests/Integration/PostgresOptimizedDataVaultSaveServiceTests.cs adds the intended opt-in coverage shape: skip-on-missing-config, AddDVaultPostgres() service resolution, AssertCompatiblePostgresStrategy(...), AssertOptimizedPathObserved(...), and persisted hub/link/satellite assertions including unchanged and changed satellite history cases.",
    "tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj keeps PackageReference Include=\u0022Npgsql.EntityFrameworkCore.PostgreSQL\u0022 behind Condition=\u0022\u0027$(DVAULT_TEST_POSTGRES_CONNECTION_STRING)\u0027 != \u0027\u0027\u0022 and adds a ProjectReference to src/DCoding.Data.DVault.Postgres/DCoding.Data.DVault.Postgres.csproj; that provider project itself only references Microsoft.Extensions.DependencyInjection.Abstractions and the core DVault project.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/postgres, area/tests, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 5 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06EZ0NA7CWDYJ7ZS3K5GM0187M-task-add-opt-in-postgresql-integration-coverage\u0027.",
    "Ticket history references implementation commit \u00277523b55964b2\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Return to dev to land or merge the sibling PostgreSQL optimized strategy surface behind AddDVaultPostgres() and update provider-registration expectations accordingly.",
    "Keep the new integration coverage only once direct repository evidence shows AddDVaultPostgres() registers a compatible IDataVaultProviderSaveStrategy for Npgsql-backed contexts.",
    "After the wiring exists, run deterministic verification for dotnet test DVault.slnx --nologo and bash tools/check-format.sh in the supported environment before re-handing to test."
  ],
  "branchName": "ticket/06EZ0NA7CWDYJ7ZS3K5GM0187M-task-add-opt-in-postgresql-integration-coverage",
  "commitSha": "7523b55964b2"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06EZ0NA7CWDYJ7ZS3K5GM0187M`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06EZ0NA7CWDYJ7ZS3K5GM0187M-task-add-opt-in-postgresql-integration-coverage`