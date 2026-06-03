[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F8KZJAKN7Q2QXXP9PRK2V94G\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F8KZJAKN7Q2QXXP9PRK2V94G-story-add-postgresql-and-sql-server-pit-bridge-r\u0027 and commit \u0027f419ece1d1c6\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F8KZJAKN7Q2QXXP9PRK2V94G-story-add-postgresql-and-sql-server-pit-bridge-r\u0027 from source \u0027f419ece1d1c6\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F8KZJAKN7Q2QXXP9PRK2V94G-story-add-postgresql-and-sql-server-pit-bridge-r\u0027.",
    "Evidence: \u0060git show --name-only --format=oneline f419ece1d1c6\u0060 shows the claimed implementation commit touches 11 product/doc/test files; current branch HEAD is later than that commit, so the review was anchored to \u0060f419ece1d1c6\u0060 rather than the newer ticket-metadata commits.",
    "Evidence: \u0060src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:21-25\u0060 and \u0060src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:21-25\u0060 register \u0060PostgresDataVaultReadStrategy\u0060 and \u0060SqlServerDataVaultReadStrategy\u0060 as both PIT and bridge strategy candidates.",
    "Evidence: \u0060src/DCoding.Data.DVault.Postgres/PostgresDataVaultReadStrategy.cs:10-27\u0060 and \u0060src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultReadStrategy.cs:10-27\u0060 select eligibility through \u0060EvaluatePostgres\u0060/\u0060EvaluateSqlServer\u0060 plus projection creation.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:3468-3541\u0060 shows PIT/bridge gate evaluation only checks provider and supported request shape conditions; no stale-maintenance or freshness gate appears in the observed evaluator.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:1878-1892\u0060 still returns read provider tuning text that says SQLite is the only repository-proven optimized read provider path.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:156-220\u0060 adds PostgreSQL/SQL Server PIT and bridge gate tests, and \u0060tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:229-240\u0060 adds provider registration tests.",
    "Evidence: \u0060git diff --name-only develop...f419ece1d1c6 -- tests/DCoding.Data.DVault.Tests/Integration\u0060 returned no changed integration test files.",
    "Evidence: \u0060rg -n \u0022PostgresDataVaultReadStrategy|SqlServerDataVaultReadStrategy|EvaluatePostgres\\(|EvaluateSqlServer\\(\u0022 tests/DCoding.Data.DVault.Tests\u0060 only finds the new read-strategy coverage in \u0060Unit/DataVaultProviderReadStrategyTests.cs\u0060 and \u0060Unit/ExplicitDataVaultSaveServiceTests.cs\u0060 for this work.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/diagnostics, area/ef-core, area/performance, area/provider-support, area/read-models, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 3 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F8KZJAKN7Q2QXXP9PRK2V94G-story-add-postgresql-and-sql-server-pit-bridge-r\u0027.",
    "Evidence: Ticket history references implementation commit \u0027f419ece1d1c6\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: When the active provider is PostgreSQL or SQL Server and the request matches a supported maintained PIT shape with the required diagnostics/read-shape evidence, the read pipeline can select a provider-specific PIT strategy candidate instead of the provider-neutral fallback. (\u0060src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:21-25\u0060 and \u0060src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:21-25\u0060 register provider-specific PIT read strategies, and \u0060DataVaultProviderReadStrategyGateEvaluator\u0060 accepts supported PostgreSQL/SQL Server PIT shapes.).",
    "AC check passed: When the active provider is PostgreSQL or SQL Server and the request matches a supported maintained bridge shape with the required diagnostics/read-shape evidence, the read pipeline can select a provider-specific bridge strategy candidate instead of the provider-neutral fallback. (The same startup extensions register provider-specific bridge read strategies, and \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:3521-3541\u0060 accepts many-to-many and hierarchy bridge shapes for PostgreSQL and SQL Server.).",
    "DoD check passed: PostgreSQL and SQL Server PIT and bridge candidate paths are implemented within the existing provider-support architecture and stay bounded to the published PIT/bridge read contract. (The implementation is placed in the existing provider-support architecture through \u0060PostgresDataVaultReadStrategy\u0060, \u0060SqlServerDataVaultReadStrategy\u0060, provider startup registration, and the shared \u0060DataVaultRelationalPitBridgeReadStrategy\u0060, with no claimed \u0060IDataVaultReadService\u0060 API change in the diff.).",
    "DoD check passed: Any provider-specific support-matrix change or limitation discovered during implementation is reflected in release or planning documentation. (Support-matrix documentation was updated in \u0060README.md\u0060, \u0060docs/architecture/dvault-v1-pit-bridge-boundary.md\u0060, and \u0060docs/production-adoption-checklist.md\u0060 to describe PostgreSQL/SQL Server PIT/bridge optimized-provider support and the remaining SQLite latest-satellite limitation.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Unsupported providers, unsupported shapes, stale-maintenance signals, or missing/incomplete evidence fail closed to the existing provider-neutral read path without changing caller-visible PIT or bridge semantics. (\u0060src/DCoding.Data.DVault.Postgres/PostgresDataVaultReadStrategy.cs:10-27\u0060, \u0060src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultReadStrategy.cs:10-27\u0060, and \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:3468-3541\u0060 gate only on provider/shape/projection validity; no stale-maintenance or missing-evidence fail-closed gate is present in the observed implementation.).",
    "AC check failed: Selected PostgreSQL and SQL Server candidates return the same functional PIT and bridge results as the existing provider-neutral implementation for the same supported inputs. (The claimed diff adds no PostgreSQL/SQL Server PIT/bridge execution or parity tests. \u0060git diff --name-only develop...f419ece1d1c6 -- tests/DCoding.Data.DVault.Tests/Integration\u0060 returned no changed integration read tests, so same-result parity with the provider-neutral path is not proven by direct repo evidence.).",
    "AC check failed: Read telemetry and diagnostic output continues to report strategy selection versus fallback for PIT and bridge reads using the existing read-telemetry surface. (\u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:1878-1892\u0060 still hardcodes SQLite-only optimized read guidance, so selected PostgreSQL/SQL Server PIT/bridge reads would emit incorrect diagnostics text instead of accurately reporting the new strategy selection surface.).",
    "AC check failed: Automated coverage exercises both candidate-selection and fallback behavior for PostgreSQL and SQL Server PIT and bridge reads. (The new tests in \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:156-220\u0060 and \u0060tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:229-240\u0060 cover gate acceptance and DI registration only; they do not exercise PostgreSQL/SQL Server PIT and bridge runtime selection, fallback, and parity behavior.).",
    "DoD check failed: Tests prove supported-shape selection, unsupported-shape fallback, and result parity with the provider-neutral path for both providers. (Observed coverage is limited to gate evaluator and DI registration tests, so the repo does not prove supported-shape selection, unsupported-shape fallback, and provider-neutral parity for both PostgreSQL and SQL Server.).",
    "DoD check failed: Telemetry or diagnostic assertions are updated so selected-strategy and fallback-cause reporting remain visible for PIT and bridge reads. (Telemetry/diagnostic assertions were not expanded to cover the new providers, and the current read provider tuning recommendation still reports SQLite-only optimized guidance in \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:1878-1892\u0060.).",
    "\u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:1878-1892\u0060 still hardcodes SQLite-only read tuning guidance. If PostgreSQL or SQL Server PIT/bridge strategies are selected, diagnostics will report the wrong optimized-provider story, which blocks acceptance criterion 5 and definition-of-done item 3.",
    "\u0060src/DCoding.Data.DVault.Postgres/PostgresDataVaultReadStrategy.cs:10-27\u0060, \u0060src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultReadStrategy.cs:10-27\u0060, and \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:3468-3541\u0060 do not implement a stale-maintenance or missing-evidence fail-closed gate; the observed selection logic only evaluates provider and shape constraints.",
    "The claimed tests do not cover PostgreSQL/SQL Server PIT and bridge runtime execution against the provider-neutral path. With no changed integration read tests and only gate/registration unit tests added, parity and fallback behavior remain unproven for acceptance criteria 4 and 6 and definition-of-done item 2."
  ],
  "evidence": [
    "\u0060git show --name-only --format=oneline f419ece1d1c6\u0060 shows the claimed implementation commit touches 11 product/doc/test files; current branch HEAD is later than that commit, so the review was anchored to \u0060f419ece1d1c6\u0060 rather than the newer ticket-metadata commits.",
    "\u0060src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:21-25\u0060 and \u0060src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:21-25\u0060 register \u0060PostgresDataVaultReadStrategy\u0060 and \u0060SqlServerDataVaultReadStrategy\u0060 as both PIT and bridge strategy candidates.",
    "\u0060src/DCoding.Data.DVault.Postgres/PostgresDataVaultReadStrategy.cs:10-27\u0060 and \u0060src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultReadStrategy.cs:10-27\u0060 select eligibility through \u0060EvaluatePostgres\u0060/\u0060EvaluateSqlServer\u0060 plus projection creation.",
    "\u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:3468-3541\u0060 shows PIT/bridge gate evaluation only checks provider and supported request shape conditions; no stale-maintenance or freshness gate appears in the observed evaluator.",
    "\u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:1878-1892\u0060 still returns read provider tuning text that says SQLite is the only repository-proven optimized read provider path.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:156-220\u0060 adds PostgreSQL/SQL Server PIT and bridge gate tests, and \u0060tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:229-240\u0060 adds provider registration tests.",
    "\u0060git diff --name-only develop...f419ece1d1c6 -- tests/DCoding.Data.DVault.Tests/Integration\u0060 returned no changed integration test files.",
    "\u0060rg -n \u0022PostgresDataVaultReadStrategy|SqlServerDataVaultReadStrategy|EvaluatePostgres\\(|EvaluateSqlServer\\(\u0022 tests/DCoding.Data.DVault.Tests\u0060 only finds the new read-strategy coverage in \u0060Unit/DataVaultProviderReadStrategyTests.cs\u0060 and \u0060Unit/ExplicitDataVaultSaveServiceTests.cs\u0060 for this work.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/diagnostics, area/ef-core, area/performance, area/provider-support, area/read-models, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 3 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F8KZJAKN7Q2QXXP9PRK2V94G-story-add-postgresql-and-sql-server-pit-bridge-r\u0027.",
    "Ticket history references implementation commit \u0027f419ece1d1c6\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Update \u0060CreateReadProviderTuningRecommendation\u0060 so PostgreSQL and SQL Server PIT/bridge strategy selection and fallback guidance are reported accurately in read diagnostics.",
    "Add PostgreSQL and SQL Server PIT/bridge automated coverage that exercises selected-strategy execution, unsupported-shape/provider fallback, and parity against the provider-neutral read path for supported inputs.",
    "After the fixes, rerun \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060 in the supported verification environment before returning to test."
  ],
  "branchName": "ticket/06F8KZJAKN7Q2QXXP9PRK2V94G-story-add-postgresql-and-sql-server-pit-bridge-r",
  "commitSha": "f419ece1d1c6"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F8KZJAKN7Q2QXXP9PRK2V94G`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F8KZJAKN7Q2QXXP9PRK2V94G-story-add-postgresql-and-sql-server-pit-bridge-r`