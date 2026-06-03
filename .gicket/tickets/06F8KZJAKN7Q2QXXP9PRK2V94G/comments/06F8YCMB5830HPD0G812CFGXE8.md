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
    "Selected verification source branch \u0027ticket/06F8KZJAKN7Q2QXXP9PRK2V94G-story-add-postgresql-and-sql-server-pit-bridge-r\u0027 and commit \u0027c24534aef008\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F8KZJAKN7Q2QXXP9PRK2V94G-story-add-postgresql-and-sql-server-pit-bridge-r\u0027 from source \u0027c24534aef008\u0027.",
    "Prompt-injection writeback protection rejected tester interactive assessment writeback on attempt 1/2; retrying tester output generation before creating a durable stop.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F8KZJAKN7Q2QXXP9PRK2V94G-story-add-postgresql-and-sql-server-pit-bridge-r\u0027.",
    "Evidence: \u0060git diff --name-only c24534aef008..HEAD\u0060 shows only \u0060.gicket/...\u0060 metadata changes after the claimed implementation commit, so the product review was anchored to \u0060c24534aef008\u0060.",
    "Evidence: \u0060src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:15-25\u0060 and \u0060src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:15-25\u0060 register \u0060PostgresDataVaultReadStrategy\u0060 and \u0060SqlServerDataVaultReadStrategy\u0060 for both PIT and bridge strategy interfaces.",
    "Evidence: \u0060src/DCoding.Data.DVault.Postgres/PostgresDataVaultReadStrategy.cs:10-25\u0060, \u0060src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultReadStrategy.cs:10-25\u0060, and \u0060src/DCoding.Data.DVault/DataVaultRelationalPitBridgeReadStrategy.cs:21-94\u0060 show provider-specific PIT/bridge selection delegating to shared gates and shared record-shaping pipelines.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:3238-3294\u0060, \u00603498-3643\u0060, and \u00603661-3682\u0060 add projection-evidence checks plus \u0060IncompleteReadShapeEvidence\u0060 fail-closed behavior for SQLite/PostgreSQL/SQL Server PIT and bridge strategies.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:156-245\u0060 adds PostgreSQL/SQL Server PIT and bridge gate-selection/fallback coverage, and \u0060tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:229-240\u0060 verifies PIT/bridge strategy registration.",
    "Evidence: \u0060rg -n \u0027ReadPitRowsAsync|ReadBridgeRowsAsync\u0027 tests/DCoding.Data.DVault.Tests/Integration\u0060 returned only SQLite PIT/bridge read-service integration tests; no PostgreSQL or SQL Server PIT/bridge result-parity tests were observed.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:190-203\u0060 and \u00603498-3643\u0060 show fallback causes/gates for provider mismatch, unsupported shape, and incomplete read-shape evidence, but no stale-maintenance or freshness gate.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/diagnostics, area/ef-core, area/performance, area/provider-support, area/read-models, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06F8KZJAKN7Q2QXXP9PRK2V94G-story-add-postgresql-and-sql-server-pit-bridge-r\u0027.",
    "Evidence: Ticket history references implementation commit \u0027c24534aef008\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Evidence: Ticket history contains 1 structured return-routing contract comment(s).",
    "Evidence: Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths.",
    "AC check passed: When the active provider is PostgreSQL or SQL Server and the request matches a supported maintained PIT shape with the required diagnostics/read-shape evidence, the read pipeline can select a provider-specific PIT strategy candidate instead of the provider-neutral fallback. (\u0060AddDVaultPostgres()\u0060/\u0060AddDVaultSqlServer()\u0060 register PIT strategies, \u0060CanReadPitRows\u0060 delegates to the shared evaluator, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:156-179\u0060 shows supported PostgreSQL/SQL Server PIT shapes selecting candidates when shape evidence is complete.).",
    "AC check passed: When the active provider is PostgreSQL or SQL Server and the request matches a supported maintained bridge shape with the required diagnostics/read-shape evidence, the read pipeline can select a provider-specific bridge strategy candidate instead of the provider-neutral fallback. (\u0060AddDVaultPostgres()\u0060/\u0060AddDVaultSqlServer()\u0060 register bridge strategies, \u0060CanReadBridgeRows\u0060 delegates to the shared evaluator, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:205-220\u0060 shows supported bridge shapes selecting candidates.).",
    "AC check passed: Selected PostgreSQL and SQL Server candidates return the same functional PIT and bridge results as the existing provider-neutral implementation for the same supported inputs. (\u0060src/DCoding.Data.DVault/DataVaultRelationalPitBridgeReadStrategy.cs:21-94\u0060 reads PIT/bridge rows through the same projection-to-record helpers used by the provider-neutral pipelines (\u0060DataVaultPitReadPipeline.CreatePitReadRecord\u0060 and \u0060DataVaultBridgeReadPipeline.CreateReadRecord/OrderBridgeRows\u0060), which supports functional parity for supported shapes.).",
    "AC check passed: Read telemetry and diagnostic output continues to report strategy selection versus fallback for PIT and bridge reads using the existing read-telemetry surface. (\u0060src/DCoding.Data.DVault/DataVaultTelemetryStrategy.cs:184-260\u0060 and \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:3498-3643\u0060 keep selected-strategy and fallback-cause reporting visible for PIT/bridge reads, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:337-368\u0060 now asserts the \u0060IncompleteReadShapeEvidence\u0060 diagnostics surface.).",
    "AC check passed: Automated coverage exercises both candidate-selection and fallback behavior for PostgreSQL and SQL Server PIT and bridge reads. (\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:156-245\u0060 covers candidate-selection and fallback behavior across PostgreSQL/SQL Server PIT and bridge gates, and \u0060tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:229-240\u0060 verifies provider package wiring.).",
    "DoD check passed: PostgreSQL and SQL Server PIT and bridge candidate paths are implemented within the existing provider-support architecture and stay bounded to the published PIT/bridge read contract. (The new PIT/bridge candidate paths live in the existing PostgreSQL and SQL Server provider packages and reuse the published PIT/bridge read contract through \u0060DataVaultRelationalPitBridgeReadStrategy\u0060 and the shared diagnostics gate evaluator.).",
    "DoD check passed: Telemetry or diagnostic assertions are updated so selected-strategy and fallback-cause reporting remain visible for PIT and bridge reads. (\u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:3498-3643\u0060, \u0060src/DCoding.Data.DVault/DataVaultTelemetryStrategy.cs:184-260\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:337-368\u0060 keep selected-strategy and fallback-cause reporting visible, including the new incomplete-evidence cause.).",
    "DoD check passed: Any provider-specific support-matrix change or limitation discovered during implementation is reflected in release or planning documentation. (Provider support-matrix and limitation documentation was updated in \u0060README.md:422\u0060, \u0060docs/architecture/dvault-v1-pit-bridge-boundary.md:12-13,57-61\u0060, \u0060docs/performance-profiles.md:56,226-232\u0060, and \u0060docs/production-adoption-checklist.md:60-63\u0060.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Unsupported providers, unsupported shapes, stale-maintenance signals, or missing/incomplete evidence fail closed to the existing provider-neutral read path without changing caller-visible PIT or bridge semantics. (Fail-closed handling exists for provider mismatch, unsupported shape, and incomplete projection evidence, but \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:190-203\u0060 and \u00603498-3643\u0060 expose no stale-maintenance/freshness fallback cause or gate, so the persisted stale-maintenance-signal clause is not fully met.).",
    "DoD check failed: Tests prove supported-shape selection, unsupported-shape fallback, and result parity with the provider-neutral path for both providers. (The observed automated tests do not execute PostgreSQL/SQL Server PIT or bridge reads and compare their results with provider-neutral reads; \u0060rg -n \u0027ReadPitRowsAsync|ReadBridgeRowsAsync\u0027 tests/DCoding.Data.DVault.Tests/Integration\u0060 only surfaced SQLite PIT/bridge read-service integration coverage, while the new provider tests stop at gate evaluation.).",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:156-245\u0060 only proves gate selection and fallback. The ticket still lacks automated PostgreSQL/SQL Server PIT/bridge read tests that compare candidate-path results against the provider-neutral path, so definition-of-done 2 is not met.",
    "\u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:190-203\u0060 and \u00603498-3643\u0060 still do not model a stale-maintenance/freshness fallback cause or gate for PIT/bridge candidates. Acceptance criterion 3 therefore remains partially unsatisfied."
  ],
  "evidence": [
    "\u0060git diff --name-only c24534aef008..HEAD\u0060 shows only \u0060.gicket/...\u0060 metadata changes after the claimed implementation commit, so the product review was anchored to \u0060c24534aef008\u0060.",
    "\u0060src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:15-25\u0060 and \u0060src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:15-25\u0060 register \u0060PostgresDataVaultReadStrategy\u0060 and \u0060SqlServerDataVaultReadStrategy\u0060 for both PIT and bridge strategy interfaces.",
    "\u0060src/DCoding.Data.DVault.Postgres/PostgresDataVaultReadStrategy.cs:10-25\u0060, \u0060src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultReadStrategy.cs:10-25\u0060, and \u0060src/DCoding.Data.DVault/DataVaultRelationalPitBridgeReadStrategy.cs:21-94\u0060 show provider-specific PIT/bridge selection delegating to shared gates and shared record-shaping pipelines.",
    "\u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:3238-3294\u0060, \u00603498-3643\u0060, and \u00603661-3682\u0060 add projection-evidence checks plus \u0060IncompleteReadShapeEvidence\u0060 fail-closed behavior for SQLite/PostgreSQL/SQL Server PIT and bridge strategies.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:156-245\u0060 adds PostgreSQL/SQL Server PIT and bridge gate-selection/fallback coverage, and \u0060tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:229-240\u0060 verifies PIT/bridge strategy registration.",
    "\u0060rg -n \u0027ReadPitRowsAsync|ReadBridgeRowsAsync\u0027 tests/DCoding.Data.DVault.Tests/Integration\u0060 returned only SQLite PIT/bridge read-service integration tests; no PostgreSQL or SQL Server PIT/bridge result-parity tests were observed.",
    "\u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:190-203\u0060 and \u00603498-3643\u0060 show fallback causes/gates for provider mismatch, unsupported shape, and incomplete read-shape evidence, but no stale-maintenance or freshness gate.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/diagnostics, area/ef-core, area/performance, area/provider-support, area/read-models, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F8KZJAKN7Q2QXXP9PRK2V94G-story-add-postgresql-and-sql-server-pit-bridge-r\u0027.",
    "Ticket history references implementation commit \u0027c24534aef008\u0027.",
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
    "Add PostgreSQL and SQL Server PIT/bridge read tests that execute the candidate paths and assert row/projected parity against \u0060AddDVault()\u0060 fallback for the same supported maintained inputs.",
    "Add an explicit stale-maintenance/freshness fail-closed gate and corresponding diagnostics/telemetry evidence for PIT/bridge candidate selection, or narrow the persisted contract language to remove that unsupported condition before handing back to test.",
    "After rework, rerun the solution verification commands in the supported verification environment."
  ],
  "branchName": "ticket/06F8KZJAKN7Q2QXXP9PRK2V94G-story-add-postgresql-and-sql-server-pit-bridge-r",
  "commitSha": "c24534aef008"
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