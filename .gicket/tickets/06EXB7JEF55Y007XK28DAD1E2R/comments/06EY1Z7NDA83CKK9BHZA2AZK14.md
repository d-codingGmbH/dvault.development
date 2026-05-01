[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706EXB7JEF55Y007XK28DAD1E2R\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06EXB7JEF55Y007XK28DAD1E2R-task-add-optional-postgres-integration-test-swit\u0027 and commit \u0027ecdd312c2851\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06EXB7JEF55Y007XK28DAD1E2R-task-add-optional-postgres-integration-test-swit\u0027 from source \u0027ecdd312c2851\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06EXB7JEF55Y007XK28DAD1E2R-task-add-optional-postgres-integration-test-swit\u0027.",
    "Evidence: git rev-parse ecdd312c2851^{commit} resolved the claimed revision to ecdd312c2851874af0211d308e727abe716b177a.",
    "Evidence: git ls-files README.md DVault.slnx tests/DCoding.Data.DVault.Tests/Integration listed README.md, DVault.slnx, the Postgres integration files, and tests/DCoding.Data.DVault.Tests/Integration/Snapshots/SqliteDataVaultSchemaSnapshot.txt.",
    "Evidence: git diff --name-status develop..ecdd312c2851 -- . \u0027:(exclude).gicket/**\u0027 reported M README.md, M tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj, added Postgres integration files, and unrelated src/docs/test deletions including src/DCoding.Data.DVault/DataVaultSaveService.cs and docs/architecture/dvault-v1-explicit-save-service.md.",
    "Evidence: git diff --unified=40 develop..ecdd312c2851 -- README.md added an \u0027Optional Local Postgres Integration Tests\u0027 section documenting DVAULT_TEST_POSTGRES_CONNECTION_STRING, default skip behavior, and the external Docker/database provisioning boundary.",
    "Evidence: git show ecdd312c2851:tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs defines DVAULT_TEST_POSTGRES_CONNECTION_STRING and the missing-configuration skip message; git show ecdd312c2851:tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfigurationTests.cs covers absent, whitespace, configured, and message-content cases.",
    "Evidence: git show ecdd312c2851:tests/DCoding.Data.DVault.Tests/Integration/PostgresDataVaultSchemaTests.cs skips when unconfigured and otherwise creates a temporary schema, applies the model, checks expected Postgres table names, and drops the schema.",
    "Evidence: git diff --unified=80 develop..ecdd312c2851 -- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs src/DCoding.Data.DVault/DataVaultSaveService.cs removed the IDataVaultSaveService registration from AddDVault and deleted DataVaultSaveService.cs entirely.",
    "Evidence: git diff develop..ecdd312c2851 -- tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs produced no diff, which indicates the earlier SQLite schema snapshot regression is no longer present in this claimed revision.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/testing, automation/bot-ready, backlog/initial-dvault, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06EXB7JEF55Y007XK28DAD1E2R-task-add-optional-postgres-integration-test-swit\u0027.",
    "Evidence: Ticket history references implementation commit \u0027ecdd312c2851\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Evidence: Ticket history contains 1 structured return-routing contract comment(s).",
    "Evidence: Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths.",
    "AC check passed: When required Postgres configuration is absent, Postgres-specific integration tests are skipped instead of failing, and the skip message clearly explains that local Postgres configuration is missing. (tests/DCoding.Data.DVault.Tests/Integration/PostgresDataVaultSchemaTests.cs calls Assert.Skip(PostgresIntegrationTestConfiguration.MissingConfigurationSkipMessage) when PostgresIntegrationTestConfiguration.FromEnvironment() is unconfigured, and tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfigurationTests.cs verifies the skip message explicitly says local Postgres configuration is missing.).",
    "AC check passed: When the documented configuration is present, a developer can opt into the Postgres integration tests without editing product code or repository-tracked secrets. (README.md documents DVAULT_TEST_POSTGRES_CONNECTION_STRING, PostgresIntegrationTestConfiguration.cs reads that environment variable, and tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj adds the Npgsql package only when the variable is set, so a developer can opt in without editing product code or tracked secrets.).",
    "AC check passed: Documentation names the local opt-in contract and states that Docker or database provisioning is external to DVault. (README.md adds an \u0027Optional Local Postgres Integration Tests\u0027 section that names DVAULT_TEST_POSTGRES_CONNECTION_STRING and states that DVault does not provision Docker containers or databases for these tests.).",
    "AC check passed: Normal dotnet test execution on an unconfigured machine does not require Postgres. (The integration project keeps Npgsql.EntityFrameworkCore.PostgreSQL behind Condition=\u0022\u0027$(DVAULT_TEST_POSTGRES_CONNECTION_STRING)\u0027 != \u0027\u0027\u0022, and the Postgres test path loads the provider via reflection, so the unconfigured default dotnet test path does not take a hard Postgres dependency.).",
    "DoD check passed: Relevant tests are added or updated inside the existing test roots to cover both configured and unconfigured behavior. (Relevant coverage was added under tests/DCoding.Data.DVault.Tests/Integration/: PostgresIntegrationTestConfigurationTests.cs covers absent, whitespace, configured, and skip-message behavior, and PostgresDataVaultSchemaTests.cs covers the configured Postgres schema path inside the existing integration test root.).",
    "DoD check passed: Documentation is added or updated in the repository and follows the shared implementation standards and formatting gate. (README.md was updated in the repository with the required local Postgres contract, and the inspected markdown diff is structurally consistent with the repository\u0027s existing documentation style.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "DoD check failed: The repository\u0027s default provider behavior remains unchanged outside the explicit Postgres test opt-in path. (git diff develop..ecdd312c2851 shows the delivered commit is not confined to the explicit Postgres opt-in path: it also modifies src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs, deletes src/DCoding.Data.DVault/DataVaultSaveService.cs, deletes docs/architecture/dvault-v1-explicit-save-service.md, and removes explicit-save-service tests. That changes default runtime behavior outside the Postgres test switch.).",
    "Blocking: the claimed commit still carries unrelated explicit-save-service runtime, documentation, and test removals, including removal of the IDataVaultSaveService registration from AddDVault and deletion of DataVaultSaveService.cs; that violates the contract boundary and Definition of Done 3.",
    "The Postgres-specific opt-in work itself appears wired correctly; the rework is to isolate or remove the unrelated branch changes from the delivered ticket commit."
  ],
  "evidence": [
    "git rev-parse ecdd312c2851^{commit} resolved the claimed revision to ecdd312c2851874af0211d308e727abe716b177a.",
    "git ls-files README.md DVault.slnx tests/DCoding.Data.DVault.Tests/Integration listed README.md, DVault.slnx, the Postgres integration files, and tests/DCoding.Data.DVault.Tests/Integration/Snapshots/SqliteDataVaultSchemaSnapshot.txt.",
    "git diff --name-status develop..ecdd312c2851 -- . \u0027:(exclude).gicket/**\u0027 reported M README.md, M tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj, added Postgres integration files, and unrelated src/docs/test deletions including src/DCoding.Data.DVault/DataVaultSaveService.cs and docs/architecture/dvault-v1-explicit-save-service.md.",
    "git diff --unified=40 develop..ecdd312c2851 -- README.md added an \u0027Optional Local Postgres Integration Tests\u0027 section documenting DVAULT_TEST_POSTGRES_CONNECTION_STRING, default skip behavior, and the external Docker/database provisioning boundary.",
    "git show ecdd312c2851:tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs defines DVAULT_TEST_POSTGRES_CONNECTION_STRING and the missing-configuration skip message; git show ecdd312c2851:tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfigurationTests.cs covers absent, whitespace, configured, and message-content cases.",
    "git show ecdd312c2851:tests/DCoding.Data.DVault.Tests/Integration/PostgresDataVaultSchemaTests.cs skips when unconfigured and otherwise creates a temporary schema, applies the model, checks expected Postgres table names, and drops the schema.",
    "git diff --unified=80 develop..ecdd312c2851 -- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs src/DCoding.Data.DVault/DataVaultSaveService.cs removed the IDataVaultSaveService registration from AddDVault and deleted DataVaultSaveService.cs entirely.",
    "git diff develop..ecdd312c2851 -- tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs produced no diff, which indicates the earlier SQLite schema snapshot regression is no longer present in this claimed revision.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/testing, automation/bot-ready, backlog/initial-dvault, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06EXB7JEF55Y007XK28DAD1E2R-task-add-optional-postgres-integration-test-swit\u0027.",
    "Ticket history references implementation commit \u0027ecdd312c2851\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 1 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "nextSteps": [
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Rebase or otherwise remove the unrelated explicit-save-service changes from the delivered ticket branch so the change set is limited to Postgres test opt-in and documentation work.",
    "Keep the Postgres additions that already satisfy the acceptance criteria: the README section, PostgresIntegrationTestConfiguration.cs, PostgresIntegrationTestConfigurationTests.cs, PostgresDataVaultSchemaTests.cs, NpgsqlProviderReflection.cs, and the conditional Npgsql package reference.",
    "After the branch is narrowed back to the ticket scope, rerun dotnet test DVault.slnx --nologo and bash tools/check-format.sh in the supported environment before handing the ticket back to test."
  ],
  "branchName": "ticket/06EXB7JEF55Y007XK28DAD1E2R-task-add-optional-postgres-integration-test-swit",
  "commitSha": "ecdd312c2851"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06EXB7JEF55Y007XK28DAD1E2R`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06EXB7JEF55Y007XK28DAD1E2R-task-add-optional-postgres-integration-test-swit`