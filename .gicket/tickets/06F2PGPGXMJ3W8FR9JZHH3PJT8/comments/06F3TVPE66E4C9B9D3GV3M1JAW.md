[gicket-bot] Run report (outcome: test-workflow-returned)

Summary
- Tester workflow returned ticket '06F2PGPGXMJ3W8FR9JZHH3PJT8' for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGPGXMJ3W8FR9JZHH3PJT8`.
- Optimistic claim succeeded (`expectedRevision=06F3TQG4M2VJSTVWDSXNAVFXT8`, `currentRevision=06F3TT8MJDGKZZGF70FVN5S9BW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service' and commit '915efc9db1ce' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service' from source '915efc9db1ce'.
- Interactive tester tool loop completed review for branch 'ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service'.
- Evidence: git -C /mnt/c/Projects/DVault diff --name-only develop...915efc9db1ce -- README.md docs src tests lists README.md, docs/production-adoption-checklist.md, docs/releases/v0.7.0.md, the new bridge-maintenance source files, and the new bridge-maintenance tests/public API...
- Evidence: src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:28-30 registers IDataVaultBridgeMaintenanceService in AddDVault() beside IDataVaultSaveService and IDataVaultReadService.
- Evidence: src/DCoding.Data.DVault/DefaultDataVaultBridgeMaintenanceService.cs:8-41 rebuilds by removing existing bridge rows and inserting desired rows, and lines 44-92 implement incremental insert/update maintenance without deletes.
- Evidence: src/DCoding.Data.DVault/DefaultDataVaultBridgeMaintenanceService.cs:404-477 builds hierarchy closure by BFS and records every reached descendant, but there is no guard that skips ancestor==descendant rows.
- Evidence: tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs:13-64 verifies many-to-many rebuild/incremental maintenance and read-back, lines 68-130 verify hierarchy shortest-depth behavior and shorter-path updates, and lines 134-192 ve...
- Evidence: tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:34-49,727-729,948-950 contains the new public maintenance request/result types, registry request, registry extensions, and IDataVaultBridgeMaintenanceService methods.
- 43 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- AC check failed: Full rebuild over a hierarchy bridge recomputes ancestor/descendant closure rows from persisted recursive link rows, persists exactly one row per distinct ancestor/descendant pair, stores positive integer TraversalDepth values equal to the minimum hop count ac...
- AC check failed: Incremental bridge maintenance can add missing bridge rows for newly relevant source-link data without requiring a full rebuild. For hierarchy bridges, when later source-link ingestion creates a shorter alternate path for an existing pair, maintenance updates ...
- AC check failed: README and the v0.15.0 release-note delta are updated to replace the current read-only bridge limitation with the new explicit caller-invoked maintenance baseline while documenting the minimum-hop TraversalDepth rule for hierarchy bridges. (README.md was updat...
- DoD check failed: Unit and SQLite integration tests pass for both bridge kinds and both maintenance modes, including duplicate-path shortest-depth coverage and shorter-path incremental update coverage for hierarchy bridges. (Relevant unit and SQLite integration tests were adde...
- Hierarchy closure can materialize forbidden self rows on cyclic source-link data because DefaultDataVaultBridgeMaintenanceService never filters out ancestor==descendant during closure generation (src/DCoding.Data.DVault/DefaultDataVaultBridgeMaintenanceService.cs:404-477).
- Acceptance criterion 9 remains unmet: the branch updates docs/releases/v0.7.0.md, but there is no v0.15.0 release-note delta in docs/releases.
- Required executable verification was not directly observed in this read-only review; dotnet test DVault.slnx --nologo and bash tools/check-format.sh were not run here.

Next steps
- Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.
- Re-run tester verification after updating tests or implementation.
- Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.
- Re-run tester verification after completing the missing implementation, test, or documentation work.
- Fix hierarchy closure maintenance so cyclic recursive-link graphs do not materialize implicit self rows, and add a regression test that proves A->A rows are not emitted from cycles unless explicitly supported by contract.
- Add the required v0.15.0 release-note delta instead of rewriting the historical v0.7.0 notes.
- After rework, run deterministic verification for dotnet test DVault.slnx --nologo and bash tools/check-format.sh in the supported environment.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8876`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `38b996456df440b5ad455efc98a62984`
- completed-at-utc: `<redacted>-18T23:32:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGPGXMJ3W8FR9JZHH3PJT8/runs/20260518T233212277Z-38b996456df440b5ad455efc98a62984.json`