[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706FE4RJZ4PA0DZ3HXDSEG2BQMM\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel\u0027 and commit \u0027de8fd3d657dc\u0027 (verification-source contract).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel\u0027 from source \u0027de8fd3d657dc\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel\u0027.",
    "Evidence: \u0060git log --oneline --decorate -n 4\u0060 on the ticket branch shows \u0060de8fd3d657\u0060 as the DEV implementation commit; later commits (\u0060868344cc6a\u0060, \u0060c6be084547\u0060, \u0060c9af51535a\u0060) are handoff/test writeback only, so the code review used \u0060develop...de8fd3d657dc\u0060.",
    "Evidence: \u0060git diff --name-only develop...de8fd3d657dc -- src tests\u0060 shows 11 implementation files: the new SQL Server PIT maintenance service, service registration, activity tracing/default-service changes, and new unit/integration tests.",
    "Evidence: \u0060src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:19-22\u0060 adds \u0060services.Replace(ServiceDescriptor.Singleton\u003CIDataVaultPitMaintenanceService, SqlServerDataVaultPitMaintenanceService\u003E())\u0060, while \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultPitMaintenanceServiceTests.cs:34-41\u0060 still expects \u0060AddDVault()\u0060 to resolve \u0060DefaultDataVaultPitMaintenanceService\u0060.",
    "Evidence: \u0060src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs:28-67\u0060 gates rebuilds on provider/clean-context/PIT shape and delegates unsupported rebuilds plus all \u0060MaintainParentsAsync\u0060 calls to \u0060DefaultDataVaultPitMaintenanceService\u0060.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs:260-367\u0060 compares selected-path PIT rebuild output against the provider-neutral rebuild and verifies stale PIT rows survive an injected insert failure.",
    "Evidence: \u0060src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs:143-194\u0060 only rolls back \u0060localTransaction\u0060 or a savepoint; there is no rollback branch for \u0060CurrentTransaction != null \u0026\u0026 !SupportsSavepoints\u0060.",
    "Evidence: \u0060rg -n \u0022OperationCanceledException|Cancel|cancellation\u0022 tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0060 matched only staged bulk-save cancellation coverage at \u0060SqlServerDataVaultSmokeTests.cs:421-431\u0060, not SQL Server PIT maintenance tests.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/performance, area/read-models, automation/bot-ready, needs-test, provider/sqlserver, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel\u0027.",
    "Evidence: Ticket history references implementation commit \u0027de8fd3d657dc\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: When AddDVaultSqlServer is used, the DbContext provider is SQL Server, the call is RebuildAsync, the context is clean, and the PIT is an ordinary hub-parent PIT, the maintenance service may execute a SQL Server INSERT ... SELECT rebuild path. (AddDVaultSqlServer now replaces IDataVaultPitMaintenanceService with SqlServerDataVaultPitMaintenanceService, and that service builds a SQL Server INSERT ... SELECT rebuild path for clean SQL Server hub-parent PIT rebuilds.).",
    "AC check passed: When any prototype gate fails, including AddDVault-only registration, provider mismatch, dirty context, MaintainParentsAsync calls, multi-active PITs, or link-parent PITs, the invocation stays on the existing provider-neutral maintenance path. (Unsupported cases are routed to provider-neutral maintenance: AddDVault() still resolves DefaultDataVaultPitMaintenanceService, RebuildAsync falls back when provider/dirty-context/parent/multi-active gates fail, and MaintainParentsAsync always delegates to MaintainParentsProviderNeutralCoreAsync.).",
    "AC check passed: For supported prototype inputs, the SQL Server path produces the same PIT row contents and the same DataVaultPitMaintenanceResult semantics as the current provider-neutral rebuild for representative ordinary hub-parent PIT shapes. (Integration test AddDVaultSqlServerRebuildsOrdinaryPitViaInsertSelectWhenConfigured compares the SQL Server result and PIT rows against the provider-neutral rebuild for a representative ordinary hub-parent PIT shape and they match.).",
    "AC check passed: Selection and fallback are observable through deterministic diagnostics or execution detail with bounded fallback causes rather than silent provider-specific behavior. (Maintenance activity tracing now records dvault.strategy.status/type plus selected and fallback events, and fallback causes are emitted as bounded enum names.).",
    "DoD check passed: The prototype lands without changing the public IDataVaultPitMaintenanceService request or result contract. (No public PIT maintenance interface or result files changed, and the public API snapshot file for DCoding.Data.DVault was untouched.).",
    "DoD check passed: AddDVault behavior remains provider-neutral, and AddDVaultSqlServer-only projects can opt into the SQL Server candidate without changing caller code. (AddDVault() remains provider-neutral while AddDVaultSqlServer() opt-in replaces only the PIT maintenance service without changing caller-facing usage.).",
    "DoD check passed: Any code comments or docs that mention the prototype describe it as SQL Server-only, rebuild-only, gated, fallback-backed, and rollback-cleanup bounded rather than as a general provider-specific PIT maintenance baseline. (No contradictory docs/comments were added; the new SQL Server PIT strings keep the scope bounded to SQL Server rebuild-only hub-parent ordinary PIT handling.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: If the selected SQL Server candidate path faults or observes cancellation before commit, the attempt must not replace the previously committed PIT contents: the PIT table retains its pre-rebuild rows and any transient SQL artifacts created for the candidate attempt are removed or transactionally discarded. (Injected-fault rollback is covered for the local-transaction path, but SqlServerDataVaultPitMaintenanceService only rolls back a local transaction or savepoint. When a caller transaction exists and savepoints are unavailable, a failed rebuild leaves PIT changes pending in the caller transaction, so pre-rebuild rows are not guaranteed to survive every selected-path fault/cancellation case.).",
    "AC check failed: Repository tests cover SQL Server path selection, provider-neutral fallback, parity for at least one representative ordinary PIT shape, and fault or cancellation verification that preloaded PIT rows survive the failed candidate attempt with no leftover transient SQL artifacts. (The new tests cover gate evaluation, SQL shape, selected-path parity, registration, and injected-fault rollback, but they do not exercise provider-neutral fallback through RebuildAsync or MaintainParentsAsync and they do not add a SQL Server PIT cancellation-state assertion.).",
    "DoD check failed: When the SQL Server candidate is selected, it participates in the caller\u0027s current DbContext transaction when one is open or otherwise uses one local transaction for the candidate rebuild attempt so fault or cancellation does not commit a partial PIT replacement. (The selected SQL Server path does not fully protect the caller-transaction case: if CurrentTransaction exists but savepoints are unavailable, the catch block performs no rollback before rethrowing.).",
    "DoD check failed: Existing PIT maintenance tests remain green and new SQL Server-specific unit or integration coverage proves gate behavior, parity, and rollback or cleanup behavior. (I could not run green-suite verification in this read-only session, and the repo\u0027s new SQL Server PIT tests still do not prove fallback and cancellation behavior for the PIT candidate path.).",
    "\u0060src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs:143-194\u0060: selected PIT rebuilds inside an existing transaction are not rolled back when \u0060CurrentTransaction.SupportsSavepoints\u0060 is false. The catch block only handles a local transaction or savepoint rollback, so a delete/insert failure can leave PIT changes pending in the caller transaction, which does not meet the ticket\u0027s rollback guarantee for the caller-transaction case.",
    "\u0060tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs:260-367\u0060 and \u0060tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs:12-84\u0060: the new test coverage never drives \u0060SqlServerDataVaultPitMaintenanceService\u0060 through provider-neutral fallback and never asserts SQL Server PIT state preservation on cancellation. That misses the explicit fallback/cancellation coverage required by acceptance criterion 6 and definition-of-done 4."
  ],
  "evidence": [
    "\u0060git log --oneline --decorate -n 4\u0060 on the ticket branch shows \u0060de8fd3d657\u0060 as the DEV implementation commit; later commits (\u0060868344cc6a\u0060, \u0060c6be084547\u0060, \u0060c9af51535a\u0060) are handoff/test writeback only, so the code review used \u0060develop...de8fd3d657dc\u0060.",
    "\u0060git diff --name-only develop...de8fd3d657dc -- src tests\u0060 shows 11 implementation files: the new SQL Server PIT maintenance service, service registration, activity tracing/default-service changes, and new unit/integration tests.",
    "\u0060src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:19-22\u0060 adds \u0060services.Replace(ServiceDescriptor.Singleton\u003CIDataVaultPitMaintenanceService, SqlServerDataVaultPitMaintenanceService\u003E())\u0060, while \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultPitMaintenanceServiceTests.cs:34-41\u0060 still expects \u0060AddDVault()\u0060 to resolve \u0060DefaultDataVaultPitMaintenanceService\u0060.",
    "\u0060src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs:28-67\u0060 gates rebuilds on provider/clean-context/PIT shape and delegates unsupported rebuilds plus all \u0060MaintainParentsAsync\u0060 calls to \u0060DefaultDataVaultPitMaintenanceService\u0060.",
    "\u0060tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs:260-367\u0060 compares selected-path PIT rebuild output against the provider-neutral rebuild and verifies stale PIT rows survive an injected insert failure.",
    "\u0060src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs:143-194\u0060 only rolls back \u0060localTransaction\u0060 or a savepoint; there is no rollback branch for \u0060CurrentTransaction != null \u0026\u0026 !SupportsSavepoints\u0060.",
    "\u0060rg -n \u0022OperationCanceledException|Cancel|cancellation\u0022 tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs\u0060 matched only staged bulk-save cancellation coverage at \u0060SqlServerDataVaultSmokeTests.cs:421-431\u0060, not SQL Server PIT maintenance tests.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/performance, area/read-models, automation/bot-ready, needs-test, provider/sqlserver, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel\u0027.",
    "Ticket history references implementation commit \u0027de8fd3d657dc\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Handle the \u0060CurrentTransaction \u0026\u0026 !SupportsSavepoints\u0060 branch explicitly, either by declining the SQL Server candidate or by adding a rollback mechanism that restores PIT state before the exception escapes.",
    "Add SQL Server PIT tests that invoke fallback through \u0060RebuildAsync\u0060 or \u0060MaintainParentsAsync\u0060 and add a cancellation-path persisted-state assertion.",
    "After rework, run legacy verification for \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060."
  ],
  "branchName": "ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel",
  "commitSha": "de8fd3d657dc"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06FE4RJZ4PA0DZ3HXDSEG2BQMM`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel`