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
    "Selected verification source branch \u0027ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile\u0027 and commit \u00270dfa713ca2aa\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile\u0027 from source \u00270dfa713ca2aa\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile\u0027.",
    "Evidence: \u0060git -C /mnt/c/Projects/DVault diff --name-status develop...0dfa713ca2aa -- src/DCoding.Data.DVault src/DCoding.Data.DVault.MySql tests/DCoding.Data.DVault.Tests README.md docs/architecture/dvault-v1-explicit-save-service.md\u0060 shows the claimed delivery touched core capability-selection files, the MySQL provider project, unit tests, and docs.",
    "Evidence: \u0060src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:18-20\u0060 calls \u0060DataVaultProviderCapabilityProfileSelection.Use(DataVaultProviderCapabilityProfiles.MySql)\u0060 and registers \u0060MySqlDataVaultSaveStrategy\u0060.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs:3-25\u0060 stores the active profile in a process-wide static field, and \u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:10-12\u0060 always applies \u0060DataVaultProviderCapabilityProfileSelection.Current\u0060.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs:61-80\u0060 creates \u0060new ModelBuilder(new ConventionSet())\u0060, calls \u0060ApplyDataVaultMetadata(...)\u0060, and asserts \u0060mysql-pomelo-v1\u0060 annotations without configuring any Pomelo provider.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs:252-269\u0060 defines the \u0060mysql-pomelo-v1\u0060 profile, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs\u0060 asserts that its mappings cover every current \u0060DataVaultLogicalPropertyKind\u0060.",
    "Evidence: \u0060git -C /mnt/c/Projects/DVault diff --name-only develop...0dfa713ca2aa -- tests/DCoding.Data.DVault.Tests/Integration tests/DCoding.Data.DVault.Tests/Shared tools\u0060 returned no changed files for integration/shared/tool verification coverage.",
    "Evidence: \u0060rg -n \u0022AddDVaultMySql|Pomelo.EntityFrameworkCore.MySql|mysql-pomelo-v1|MySqlDataVaultSaveStrategy\u0022 /mnt/c/Projects/DVault/tests/DCoding.Data.DVault.Tests/Integration /mnt/c/Projects/DVault/tests/DCoding.Data.DVault.Tests/Shared\u0060 returned no MySQL baseline matches, so the review found no added integration/shared dispatch or fallback coverage.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/mysql, area/performance, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile\u0027.",
    "Evidence: Ticket history references implementation commit \u00270dfa713ca2aa\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: The MySQL capability profile declares mappings for every current \u0060DataVaultLogicalPropertyKind\u0060 and preserves the existing annotation pattern for provider profile name, logical property kind, native store type, and value format. (\u0060src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs\u0060 adds \u0060mysql-pomelo-v1\u0060 mappings for every current \u0060DataVaultLogicalPropertyKind\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs\u0060 verifies completeness plus the expected annotation storage types and value formats.).",
    "AC check passed: \u0060AddDVaultMySql()\u0060 registers a MySQL \u0060IDataVaultProviderSaveStrategy\u0060 in \u0060src/DCoding.Data.DVault.MySql\u0060, and core dispatch selects it only when the current \u0060DbContext\u0060, ordered request batch, and active EF Core provider are compatible with the Pomelo baseline. (\u0060AddDVaultMySql()\u0060 now registers \u0060MySqlDataVaultSaveStrategy\u0060 in \u0060src/DCoding.Data.DVault.MySql\u0060, and \u0060MySqlDataVaultSaveStrategy.CanSave\u0060 gates the optimized path on the exact provider name \u0060Pomelo.EntityFrameworkCore.MySql\u0060 plus a clean EF change tracker.).",
    "AC check passed: All MySQL-specific SQL required by the optimized path lives in the MySQL provider project; the core package does not embed MySQL SQL text or execute MySQL-specific branches to perform the optimized write. (The optimized MySQL SQL builder and execution path live in \u0060src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs\u0060. The reviewed core-package changes add capability/profile selection wiring, but the MySQL SQL text itself is confined to the MySQL provider project.).",
    "DoD check passed: Core and MySQL implementation changes follow the existing repository layout, package boundaries, and one-member-per-file policy. (The implementation stays within the existing core/provider/test/docs layout, and the required provider output path \u0060src/DCoding.Data.DVault.MySql\u0060 contains the new MySQL strategy artifact.).",
    "DoD check passed: Documentation and comments that currently describe MySQL as compatibility-only are updated where the implemented behavior changes that statement, including the named Pomelo baseline and the preserved \u0060ApplyDataVaultMetadata(...)\u0060 caller experience. (\u0060README.md\u0060, \u0060docs/architecture/dvault-v1-explicit-save-service.md\u0060, and the \u0060AddDVaultMySql()\u0060 XML comment were updated so MySQL is no longer documented as compatibility-only and the preserved \u0060ApplyDataVaultMetadata(...)\u0060 caller path is named.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: With \u0060Pomelo.EntityFrameworkCore.MySql\u0060 configured and \u0060AddDVaultMySql()\u0060 registered, the existing \u0060ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel)\u0060 call path uses a MySQL capability profile instead of the current SQLite-only default without requiring callers to switch to a new public model-building hook. (\u0060src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs\u0060 switches the active profile to MySQL during DI registration, and \u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs\u0060 always reads that global current profile. \u0060tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs\u0060 then proves MySQL annotations on a bare \u0060ModelBuilder(new ConventionSet())\u0060 without any Pomelo provider configuration, so the delivered path is not actually keyed to a Pomelo-configured model.).",
    "AC check failed: When the active provider is not the supported Pomelo baseline or the request/context shape is otherwise unsafe, \u0060CanSave\u0060 declines and the existing provider-neutral fallback writer persists the request without changing the public save contract. (The code makes \u0060CanSave\u0060 decline non-Pomelo providers, but the delivered tests do not exercise \u0060DefaultDataVaultSaveService\u0060 with the MySQL strategy on a rejected context/request to prove the existing fallback writer still persists through the unchanged public save contract.).",
    "AC check failed: Ticket completion requires automated unit, snapshot, registration, capability-profile completeness, dispatch, and fallback coverage; live MySQL SQL contract tests are optional and not required for this ticket. (The added coverage is limited to unit/snapshot/registration/profile checks. No MySQL dispatch or fallback coverage was added under \u0060tests/DCoding.Data.DVault.Tests/Integration\u0060 or \u0060tests/DCoding.Data.DVault.Tests/Shared\u0060, so the required bounded Pomelo-baseline coverage is incomplete.).",
    "DoD check failed: Affected unit, snapshot, package-verification, and integration tests for the bounded Pomelo baseline are updated and passing; no required local MySQL database prerequisite is introduced. (Unit and snapshot files were updated, but no affected MySQL integration/package-verification coverage was added, and the repository verification commands were not executed in this read-only review. Passing state for the bounded Pomelo baseline is therefore unproven.).",
    "DoD check failed: No MySQL-specific SQL or provider-specific persistence behavior is introduced outside \u0060src/DCoding.Data.DVault.MySql\u0060; any optional live MySQL tests skip cleanly when their external opt-in configuration is absent. (Provider-specific activation behavior was introduced in core via \u0060src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs\u0060 and \u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs\u0060, so MySQL-specific persistence activation is not confined to \u0060src/DCoding.Data.DVault.MySql\u0060.).",
    "The metadata-profile activation is global state, not a Pomelo-aware selection mechanism. As delivered, \u0060AddDVaultMySql()\u0060 alone flips \u0060ApplyDataVaultMetadata(...)\u0060 to MySQL for later model builds, including the unit test path that never configures a Pomelo provider.",
    "The bounded Pomelo baseline is missing direct automated proof that rejected MySQL strategy cases fall back through \u0060DefaultDataVaultSaveService\u0060 without changing the public save contract.",
    "Read-only review did not run \u0060dotnet test DVault.slnx --nologo\u0060 or \u0060bash tools/check-format.sh\u0060; executable verification remains pending after the code/test blockers are fixed."
  ],
  "evidence": [
    "\u0060git -C /mnt/c/Projects/DVault diff --name-status develop...0dfa713ca2aa -- src/DCoding.Data.DVault src/DCoding.Data.DVault.MySql tests/DCoding.Data.DVault.Tests README.md docs/architecture/dvault-v1-explicit-save-service.md\u0060 shows the claimed delivery touched core capability-selection files, the MySQL provider project, unit tests, and docs.",
    "\u0060src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:18-20\u0060 calls \u0060DataVaultProviderCapabilityProfileSelection.Use(DataVaultProviderCapabilityProfiles.MySql)\u0060 and registers \u0060MySqlDataVaultSaveStrategy\u0060.",
    "\u0060src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs:3-25\u0060 stores the active profile in a process-wide static field, and \u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:10-12\u0060 always applies \u0060DataVaultProviderCapabilityProfileSelection.Current\u0060.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs:61-80\u0060 creates \u0060new ModelBuilder(new ConventionSet())\u0060, calls \u0060ApplyDataVaultMetadata(...)\u0060, and asserts \u0060mysql-pomelo-v1\u0060 annotations without configuring any Pomelo provider.",
    "\u0060src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs:252-269\u0060 defines the \u0060mysql-pomelo-v1\u0060 profile, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs\u0060 asserts that its mappings cover every current \u0060DataVaultLogicalPropertyKind\u0060.",
    "\u0060git -C /mnt/c/Projects/DVault diff --name-only develop...0dfa713ca2aa -- tests/DCoding.Data.DVault.Tests/Integration tests/DCoding.Data.DVault.Tests/Shared tools\u0060 returned no changed files for integration/shared/tool verification coverage.",
    "\u0060rg -n \u0022AddDVaultMySql|Pomelo.EntityFrameworkCore.MySql|mysql-pomelo-v1|MySqlDataVaultSaveStrategy\u0022 /mnt/c/Projects/DVault/tests/DCoding.Data.DVault.Tests/Integration /mnt/c/Projects/DVault/tests/DCoding.Data.DVault.Tests/Shared\u0060 returned no MySQL baseline matches, so the review found no added integration/shared dispatch or fallback coverage.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/mysql, area/performance, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile\u0027.",
    "Ticket history references implementation commit \u00270dfa713ca2aa\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Replace the process-wide profile switch with activation that is tied to the intended Pomelo-backed model path, so unsupported providers are not treated as MySQL-compatible by \u0060ApplyDataVaultMetadata(...)\u0060.",
    "Add deterministic MySQL baseline tests that cover optimized-path selection and fallback dispatch/rejection behavior, then rerun \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060 through the supported verification path."
  ],
  "branchName": "ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile",
  "commitSha": "0dfa713ca2aa"
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