[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706EZ0NBX79YQ0J5A9ECJG955TC\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile\u0027 and commit \u002786bf61cd5a71\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile\u0027 from source \u002786bf61cd5a71\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile\u0027.",
    "Evidence: \u0060git diff --name-status develop...86bf61cd5a71\u0060 shows changes in \u0060src/DCoding.Data.DVault\u0060, \u0060src/DCoding.Data.DVault.MySql\u0060, \u0060tests/DCoding.Data.DVault.Tests\u0060, \u0060README.md\u0060, and \u0060docs/architecture/dvault-v1-explicit-save-service.md\u0060.",
    "Evidence: \u0060src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:15-22\u0060 registers \u0060Pomelo.EntityFrameworkCore.MySql\u0060 to \u0060DataVaultProviderCapabilityProfiles.MySql\u0060 and adds \u0060MySqlDataVaultSaveStrategy\u0060 to DI.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:10-12\u0060 now routes \u0060ApplyDataVaultMetadata(...)\u0060 through \u0060DataVaultProviderCapabilityProfileSelection.Select(modelBuilder)\u0060, and \u0060src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs:46-119\u0060 resolves provider names from EF model internals.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs:254-270\u0060 declares the MySQL capability profile, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs:49-95\u0060 verifies all logical-property mappings.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs:12-118\u0060 adds registration, provider-gate, SQL-text, bare-model fallback, and manual profile-annotation tests.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs:130-182\u0060 adds non-Pomelo fallback coverage and a configured-provider profile-selection test that uses SQLite options plus manual \u0060Register(SqliteProviderName, DataVaultProviderCapabilityProfiles.MySql)\u0060.",
    "Evidence: Repository-wide Pomelo search found only docs/code/test string mentions, and \u0060tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:15-18\u0060 references SQLite and conditional Npgsql packages only; I did not find a Pomelo package reference in the repo.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/mysql, area/performance, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 9 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile\u0027.",
    "Evidence: Ticket history references implementation commit \u002786bf61cd5a71\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Evidence: Ticket history contains 2 structured return-routing contract comment(s).",
    "Evidence: Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths.",
    "AC check passed: The MySQL capability profile declares mappings for every current \u0060DataVaultLogicalPropertyKind\u0060 and preserves the existing annotation pattern for provider profile name, logical property kind, native store type, and value format. (\u0060src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs:254-270\u0060 declares MySQL mappings for every current \u0060DataVaultLogicalPropertyKind\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs:49-95\u0060 plus \u0060tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs:90-113\u0060 verify completeness and the preserved provider annotation metadata.).",
    "AC check passed: All MySQL-specific SQL required by the optimized path lives in the MySQL provider project; the core package does not embed MySQL SQL text or execute MySQL-specific branches to perform the optimized write. (Repository search showed MySQL SQL text only in \u0060src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs\u0060 (for example \u0060CreateMySqlInsertCommandText\u0060 at lines 63-107 and the execution helpers later in the file); the core package changes are generic capability-profile selection and annotation wiring, not embedded MySQL SQL branches.).",
    "AC check passed: When the active provider is not the supported Pomelo baseline or the request/context shape is otherwise unsafe, \u0060CanSave\u0060 declines and the existing provider-neutral fallback writer persists the request without changing the public save contract. (\u0060MySqlDataVaultSaveStrategy.CanSave\u0060 declines non-Pomelo providers and dirty tracked contexts in \u0060src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs:19-26\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs:130-156\u0060 proves that a non-Pomelo context falls back through the existing provider-neutral save path.).",
    "DoD check passed: Core and MySQL implementation changes follow the existing repository layout, package boundaries, and one-member-per-file policy. (The changes stay within the expected core/provider/test/docs layout: the required output path \u0060src/DCoding.Data.DVault.MySql\u0060 now contains the MySQL service extension, strategy, and assembly-info wiring, and the structure mirrors the existing provider-extension pattern already used in the repository.).",
    "DoD check passed: Documentation and comments that currently describe MySQL as compatibility-only are updated where the implemented behavior changes that statement, including the named Pomelo baseline and the preserved \u0060ApplyDataVaultMetadata(...)\u0060 caller experience. (Documentation was updated where behavior changed: \u0060README.md:28,135\u0060 names the Pomelo baseline and preserved \u0060ApplyDataVaultMetadata(...)\u0060 path, and \u0060docs/architecture/dvault-v1-explicit-save-service.md:53-61\u0060 updates the provider matrix and ownership notes accordingly.).",
    "DoD check passed: No MySQL-specific SQL or provider-specific persistence behavior is introduced outside \u0060src/DCoding.Data.DVault.MySql\u0060; any optional live MySQL tests skip cleanly when their external opt-in configuration is absent. (I did not find MySQL SQL text outside \u0060src/DCoding.Data.DVault.MySql\u0060, and no optional live MySQL test suite or mandatory local MySQL prerequisite was introduced in the changed test project files.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: With \u0060Pomelo.EntityFrameworkCore.MySql\u0060 configured and \u0060AddDVaultMySql()\u0060 registered, the existing \u0060ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel)\u0060 call path uses a MySQL capability profile instead of the current SQLite-only default without requiring callers to switch to a new public model-building hook. (\u0060src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:15-22\u0060 and \u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:10-12\u0060 wire the existing caller path to provider-profile selection, but the only configured-provider automation I found is \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs:160-182\u0060, which remaps SQLite to the MySQL profile instead of exercising the exact \u0060AddDVaultMySql()\u0060 plus Pomelo-configured path.).",
    "AC check failed: \u0060AddDVaultMySql()\u0060 registers a MySQL \u0060IDataVaultProviderSaveStrategy\u0060 in \u0060src/DCoding.Data.DVault.MySql\u0060, and core dispatch selects it only when the current \u0060DbContext\u0060, ordered request batch, and active EF Core provider are compatible with the Pomelo baseline. (\u0060AddDVaultMySql()\u0060 registers the strategy and profile (\u0060src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:18-22\u0060), and \u0060MySqlDataVaultSaveStrategy.CanSave\u0060 gates on the Pomelo provider name plus a clean change tracker (\u0060src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs:19-26\u0060), but I did not find direct automated positive dispatch selection for a Pomelo-configured context.).",
    "AC check failed: Ticket completion requires automated unit, snapshot, registration, capability-profile completeness, dispatch, and fallback coverage; live MySQL SQL contract tests are optional and not required for this ticket. (The branch adds unit and integration coverage for registration, profile completeness, and non-Pomelo fallback, but I did not find direct automated optimized-path selection coverage for the bounded Pomelo baseline, and I did not directly observe \u0060dotnet test DVault.slnx --nologo\u0060 or \u0060bash tools/check-format.sh\u0060 succeeding in this read-only review.).",
    "DoD check failed: Affected unit, snapshot, package-verification, and integration tests for the bounded Pomelo baseline are updated and passing; no required local MySQL database prerequisite is introduced. (Affected tests were updated, but I did not directly observe the required verification commands passing in this read-only session, and the automated suite still stops short of a direct Pomelo-positive proof.).",
    "Missing direct automated proof of the exact \u0060AddDVaultMySql()\u0060 plus Pomelo-configured positive path. The new configured-provider test remaps SQLite to the MySQL profile, and the repo contains no Pomelo package reference, so AC1, AC3, and AC6 remain unconfirmed from directly observed evidence.",
    "Required verification commands were not directly observed green in this read-only review, so Definition of Done index 2 still needs deterministic legacy verification."
  ],
  "evidence": [
    "\u0060git diff --name-status develop...86bf61cd5a71\u0060 shows changes in \u0060src/DCoding.Data.DVault\u0060, \u0060src/DCoding.Data.DVault.MySql\u0060, \u0060tests/DCoding.Data.DVault.Tests\u0060, \u0060README.md\u0060, and \u0060docs/architecture/dvault-v1-explicit-save-service.md\u0060.",
    "\u0060src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:15-22\u0060 registers \u0060Pomelo.EntityFrameworkCore.MySql\u0060 to \u0060DataVaultProviderCapabilityProfiles.MySql\u0060 and adds \u0060MySqlDataVaultSaveStrategy\u0060 to DI.",
    "\u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:10-12\u0060 now routes \u0060ApplyDataVaultMetadata(...)\u0060 through \u0060DataVaultProviderCapabilityProfileSelection.Select(modelBuilder)\u0060, and \u0060src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs:46-119\u0060 resolves provider names from EF model internals.",
    "\u0060src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs:254-270\u0060 declares the MySQL capability profile, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs:49-95\u0060 verifies all logical-property mappings.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs:12-118\u0060 adds registration, provider-gate, SQL-text, bare-model fallback, and manual profile-annotation tests.",
    "\u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs:130-182\u0060 adds non-Pomelo fallback coverage and a configured-provider profile-selection test that uses SQLite options plus manual \u0060Register(SqliteProviderName, DataVaultProviderCapabilityProfiles.MySql)\u0060.",
    "Repository-wide Pomelo search found only docs/code/test string mentions, and \u0060tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:15-18\u0060 references SQLite and conditional Npgsql packages only; I did not find a Pomelo package reference in the repo.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/mysql, area/performance, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 9 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile\u0027.",
    "Ticket history references implementation commit \u002786bf61cd5a71\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 2 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Add a direct automated positive-path test for the bounded Pomelo baseline that exercises the existing \u0060ApplyDataVaultMetadata(...)\u0060 caller path and MySQL strategy selection without introducing a mandatory live MySQL prerequisite.",
    "After that coverage gap is closed, run legacy verification for \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060, then resubmit to test."
  ],
  "branchName": "ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile",
  "commitSha": "86bf61cd5a71"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06EZ0NBX79YQ0J5A9ECJG955TC`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile`