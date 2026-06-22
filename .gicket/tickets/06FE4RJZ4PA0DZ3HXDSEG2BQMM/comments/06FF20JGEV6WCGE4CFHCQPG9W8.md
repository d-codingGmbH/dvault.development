[gicket-bot] Run report (outcome: test-workflow-returned)

Summary
- Tester workflow returned ticket '06FE4RJZ4PA0DZ3HXDSEG2BQMM' for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4RJZ4PA0DZ3HXDSEG2BQMM`.
- Optimistic claim succeeded (`expectedRevision=06FF1VXDH0DWB4ZMQ38B7TM6WW`, `currentRevision=06FF1YK80JDKDMN3KTNVSX3244`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel' and commit 'de8fd3d657dc' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel' from source 'de8fd3d657dc'.
- Interactive tester tool loop completed review for branch 'ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel'.
- Evidence: `git log --oneline --decorate -n 4` on the ticket branch shows `de8fd3d657` as the DEV implementation commit; later commits (`868344cc6a`, `c6be084547`, `c9af51535a`) are handoff/test writeback only, so the code review used `develop...de8fd3d657dc`.
- Evidence: `git diff --name-only develop...de8fd3d657dc -- src tests` shows 11 implementation files: the new SQL Server PIT maintenance service, service registration, activity tracing/default-service changes, and new unit/integration tests.
- Evidence: `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:19-22` adds `services.Replace(ServiceDescriptor.Singleton<IDataVaultPitMaintenanceService, SqlServerDataVaultPitMaintenanceService>())`, while `tests/DCoding.Data.DVault.Tests/Unit/DataV...
- Evidence: `src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs:28-67` gates rebuilds on provider/clean-context/PIT shape and delegates unsupported rebuilds plus all `MaintainParentsAsync` calls to `DefaultDataVaultPitMaintenanceService`.
- Evidence: `tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs:260-367` compares selected-path PIT rebuild output against the provider-neutral rebuild and verifies stale PIT rows survive an injected insert failure.
- Evidence: `src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs:143-194` only rolls back `localTransaction` or a savepoint; there is no rollback branch for `CurrentTransaction != null && !SupportsSavepoints`.
- 40 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- AC check failed: If the selected SQL Server candidate path faults or observes cancellation before commit, the attempt must not replace the previously committed PIT contents: the PIT table retains its pre-rebuild rows and any transient SQL artifacts created for the candidate at...
- AC check failed: Repository tests cover SQL Server path selection, provider-neutral fallback, parity for at least one representative ordinary PIT shape, and fault or cancellation verification that preloaded PIT rows survive the failed candidate attempt with no leftover transie...
- DoD check failed: When the SQL Server candidate is selected, it participates in the caller's current DbContext transaction when one is open or otherwise uses one local transaction for the candidate rebuild attempt so fault or cancellation does not commit a partial PIT replacem...
- DoD check failed: Existing PIT maintenance tests remain green and new SQL Server-specific unit or integration coverage proves gate behavior, parity, and rollback or cleanup behavior. (I could not run green-suite verification in this read-only session, and the repo's new SQL Se...
- `src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs:143-194`: selected PIT rebuilds inside an existing transaction are not rolled back when `CurrentTransaction.SupportsSavepoints` is false. The catch block only handles a local transaction or savepoint...
- `tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs:260-367` and `tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs:12-84`: the new test coverage never drives `SqlServerDataVaultPitMaintenanceService` through provider...

Next steps
- Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.
- Re-run tester verification after updating tests or implementation.
- Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.
- Re-run tester verification after completing the missing implementation, test, or documentation work.
- Handle the `CurrentTransaction && !SupportsSavepoints` branch explicitly, either by declining the SQL Server candidate or by adding a rollback mechanism that restores PIT state before the exception escapes.
- Add SQL Server PIT tests that invoke fallback through `RebuildAsync` or `MaintainParentsAsync` and add a cancellation-path persisted-state assertion.
- After rework, run legacy verification for `dotnet test DVault.slnx --nologo` and `bash tools/check-format.sh`.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9411`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `50ef24fee37247f6b64bfeb56311c853`
- completed-at-utc: `<redacted>-22T20:25:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4RJZ4PA0DZ3HXDSEG2BQMM/runs/20260622T202521266Z-50ef24fee37247f6b64bfeb56311c853.json`