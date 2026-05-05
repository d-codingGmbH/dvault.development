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
    "Selected verification source branch \u0027ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile\u0027 and commit \u0027123605cc017a\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile\u0027 from source \u0027123605cc017a\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile\u0027.",
    "Evidence: \u0060git diff --name-status develop...123605cc017a -- src/DCoding.Data.DVault src/DCoding.Data.DVault.MySql tests/DCoding.Data.DVault.Tests README.md docs/architecture/dvault-v1-explicit-save-service.md\u0060 shows changes in the core translator/profile-selection code, the MySQL provider project, tests, and docs.",
    "Evidence: \u0060src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:15-22\u0060 registers \u0060DataVaultProviderCapabilityProfileSelection.Register(MySqlDataVaultSaveStrategy.PomeloProviderName, DataVaultProviderCapabilityProfiles.MySql)\u0060 and the \u0060MySqlDataVaultSaveStrategy\u0060 service.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:10-12\u0060 now routes \u0060ApplyDataVaultMetadata(...)\u0060 through \u0060DataVaultProviderCapabilityProfileSelection.Select(modelBuilder)\u0060.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs:46-74\u0060 relies on reflective \u0060DatabaseProviders\u0060 discovery and falls back to \u0060DataVaultProviderCapabilityProfiles.Sqlite\u0060 when no registered active provider name is found.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs:67-113\u0060 keeps bare \u0060ModelBuilder\u0060 instances on \u0060sqlite-v1\u0060 and proves MySQL annotations only by manual provider-profile selection plus the internal translator overload.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:15-18\u0060 references \u0060Microsoft.EntityFrameworkCore.Sqlite\u0060 and conditional \u0060Npgsql.EntityFrameworkCore.PostgreSQL\u0060, but no Pomelo provider package.",
    "Evidence: \u0060rg -n \u0022Pomelo\u0022 /mnt/c/Projects/DVault/tests/DCoding.Data.DVault.Tests/Integration -g \u0027*.cs\u0027\u0060 only returns the non-Pomelo fallback diagnostics and test in \u0060DataVaultSaveStrategySelectionTests.cs\u0060.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs:130-156\u0060 verifies that \u0060AddDVaultMySql()\u0060 falls back on a SQLite context when the active provider is not Pomelo.",
    "Evidence: \u0060README.md:28-29,135\u0060 and \u0060docs/architecture/dvault-v1-explicit-save-service.md:53-62\u0060 were updated to document the Pomelo baseline.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/mysql, area/performance, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 7 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile\u0027.",
    "Evidence: Ticket history references implementation commit \u0027123605cc017a\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Evidence: Ticket history contains 1 structured return-routing contract comment(s).",
    "Evidence: Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths.",
    "AC check passed: The MySQL capability profile declares mappings for every current \u0060DataVaultLogicalPropertyKind\u0060 and preserves the existing annotation pattern for provider profile name, logical property kind, native store type, and value format. (\u0060src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs:254-270\u0060 defines \u0060mysql-pomelo-v1\u0060 with mappings for every current \u0060DataVaultLogicalPropertyKind\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs\u0060 asserts the complete mapping set plus native type and value-format expectations.).",
    "AC check passed: \u0060AddDVaultMySql()\u0060 registers a MySQL \u0060IDataVaultProviderSaveStrategy\u0060 in \u0060src/DCoding.Data.DVault.MySql\u0060, and core dispatch selects it only when the current \u0060DbContext\u0060, ordered request batch, and active EF Core provider are compatible with the Pomelo baseline. (\u0060src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:15-22\u0060 registers \u0060MySqlDataVaultSaveStrategy\u0060, \u0060src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs:19-27\u0060 gates \u0060CanSave\u0060 on the Pomelo provider name and a clean change tracker, and \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs:130-156\u0060 proves the strategy declines on a non-Pomelo context so core fallback remains reachable.).",
    "AC check passed: All MySQL-specific SQL required by the optimized path lives in the MySQL provider project; the core package does not embed MySQL SQL text or execute MySQL-specific branches to perform the optimized write. (MySQL insert SQL generation and execution are confined to \u0060src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs\u0060; repository search over the core and other provider projects found MySQL capability-profile metadata in core but no MySQL SQL text or optimized-write branch outside the MySQL provider project.).",
    "AC check passed: When the active provider is not the supported Pomelo baseline or the request/context shape is otherwise unsafe, \u0060CanSave\u0060 declines and the existing provider-neutral fallback writer persists the request without changing the public save contract. (\u0060MySqlDataVaultSaveStrategy.CanSave\u0060 rejects non-Pomelo providers and dirty tracked contexts in \u0060src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs:19-27\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs:130-156\u0060 verifies fallback persistence on a non-Pomelo context without changing the public save-service contract.).",
    "DoD check passed: Core and MySQL implementation changes follow the existing repository layout, package boundaries, and one-member-per-file policy. (The required output path \u0060src/DCoding.Data.DVault.MySql\u0060 exists, provider SQL stays in that project, and the repository\u0027s one-member-per-file rule only enforces public or protected top-level declarations (\u0060docs/quality/one-member-per-file.md:3-28\u0060), which these additions respect.).",
    "DoD check passed: Documentation and comments that currently describe MySQL as compatibility-only are updated where the implemented behavior changes that statement, including the named Pomelo baseline and the preserved \u0060ApplyDataVaultMetadata(...)\u0060 caller experience. (\u0060README.md:28-29,123-135\u0060 and \u0060docs/architecture/dvault-v1-explicit-save-service.md:47-62\u0060 now describe the Pomelo baseline and preserved \u0060ApplyDataVaultMetadata(...)\u0060 caller experience.).",
    "DoD check passed: No MySQL-specific SQL or provider-specific persistence behavior is introduced outside \u0060src/DCoding.Data.DVault.MySql\u0060; any optional live MySQL tests skip cleanly when their external opt-in configuration is absent. (The optimized MySQL write logic lives in \u0060src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs\u0060, and no optional live MySQL test suite was added that would require extra opt-in skip handling.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: With \u0060Pomelo.EntityFrameworkCore.MySql\u0060 configured and \u0060AddDVaultMySql()\u0060 registered, the existing \u0060ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel)\u0060 call path uses a MySQL capability profile instead of the current SQLite-only default without requiring callers to switch to a new public model-building hook. (The public \u0060ApplyDataVaultMetadata(...)\u0060 entry point now routes through provider selection, but the only positive MySQL annotation proof is \u0060tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs:90-113\u0060, which manually selects \u0060MySqlDataVaultSaveStrategy.PomeloProviderName\u0060 and calls the internal translator overload instead of configuring Pomelo and exercising the public call path.).",
    "AC check failed: Ticket completion requires automated unit, snapshot, registration, capability-profile completeness, dispatch, and fallback coverage; live MySQL SQL contract tests are optional and not required for this ticket. (Registration, capability-profile completeness, non-Pomelo fallback, and API snapshot tests were added, but \u0060tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:15-18\u0060 contains no Pomelo provider reference and \u0060rg -n \u0022Pomelo\u0022 tests/DCoding.Data.DVault.Tests/Integration\u0060 only returns the non-Pomelo fallback test and diagnostics, so the bounded Pomelo baseline and positive activation path are not automatically covered.).",
    "DoD check failed: Affected unit, snapshot, package-verification, and integration tests for the bounded Pomelo baseline are updated and passing; no required local MySQL database prerequisite is introduced. (Tests and snapshots were updated, but the repository still lacks automated Pomelo-configured baseline coverage, so the affected bounded-baseline test set is not fully demonstrated; this read-only review also did not execute \u0060dotnet test DVault.slnx --nologo\u0060 or \u0060bash tools/check-format.sh\u0060.).",
    "Blocking: the public Pomelo activation path remains unproven. \u0060DataVaultProviderCapabilityProfileSelection.Select(modelBuilder)\u0060 depends on reflective provider-name discovery with silent SQLite fallback, but no automated test configures Pomelo and calls the public \u0060ApplyDataVaultMetadata(...)\u0060 entry point.",
    "Blocking: bounded Pomelo baseline coverage is incomplete. The integration test project has no Pomelo provider reference, and Pomelo mentions in integration tests are limited to non-Pomelo fallback diagnostics, so positive activation/optimized-path selection for the supported MySQL baseline is not directly covered."
  ],
  "evidence": [
    "\u0060git diff --name-status develop...123605cc017a -- src/DCoding.Data.DVault src/DCoding.Data.DVault.MySql tests/DCoding.Data.DVault.Tests README.md docs/architecture/dvault-v1-explicit-save-service.md\u0060 shows changes in the core translator/profile-selection code, the MySQL provider project, tests, and docs.",
    "\u0060src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:15-22\u0060 registers \u0060DataVaultProviderCapabilityProfileSelection.Register(MySqlDataVaultSaveStrategy.PomeloProviderName, DataVaultProviderCapabilityProfiles.MySql)\u0060 and the \u0060MySqlDataVaultSaveStrategy\u0060 service.",
    "\u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:10-12\u0060 now routes \u0060ApplyDataVaultMetadata(...)\u0060 through \u0060DataVaultProviderCapabilityProfileSelection.Select(modelBuilder)\u0060.",
    "\u0060src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs:46-74\u0060 relies on reflective \u0060DatabaseProviders\u0060 discovery and falls back to \u0060DataVaultProviderCapabilityProfiles.Sqlite\u0060 when no registered active provider name is found.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs:67-113\u0060 keeps bare \u0060ModelBuilder\u0060 instances on \u0060sqlite-v1\u0060 and proves MySQL annotations only by manual provider-profile selection plus the internal translator overload.",
    "\u0060tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:15-18\u0060 references \u0060Microsoft.EntityFrameworkCore.Sqlite\u0060 and conditional \u0060Npgsql.EntityFrameworkCore.PostgreSQL\u0060, but no Pomelo provider package.",
    "\u0060rg -n \u0022Pomelo\u0022 /mnt/c/Projects/DVault/tests/DCoding.Data.DVault.Tests/Integration -g \u0027*.cs\u0027\u0060 only returns the non-Pomelo fallback diagnostics and test in \u0060DataVaultSaveStrategySelectionTests.cs\u0060.",
    "\u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs:130-156\u0060 verifies that \u0060AddDVaultMySql()\u0060 falls back on a SQLite context when the active provider is not Pomelo.",
    "\u0060README.md:28-29,135\u0060 and \u0060docs/architecture/dvault-v1-explicit-save-service.md:53-62\u0060 were updated to document the Pomelo baseline.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/mysql, area/performance, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 7 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile\u0027.",
    "Ticket history references implementation commit \u0027123605cc017a\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 1 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Add a non-live Pomelo-configured test path (for example a conditional Pomelo test reference or reflection helper) that can exercise the public \u0060ApplyDataVaultMetadata(...)\u0060 entry point and assert \u0060mysql-pomelo-v1\u0060 annotations.",
    "Add positive compatible-provider coverage for \u0060AddDVaultMySql()\u0060 so the supported Pomelo baseline is proven, not just the non-Pomelo fallback path.",
    "After that rework, run deterministic legacy verification for \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060."
  ],
  "branchName": "ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile",
  "commitSha": "123605cc017a"
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