[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06F492ARW2N6SNYJH15RHMZEN8-story-add-ef-core-misuse-analyzers-for-dvault-in' for ticket '06F492ARW2N6SNYJH15RHMZEN8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492ARW2N6SNYJH15RHMZEN8`.
- Optimistic claim succeeded (`expectedRevision=06F4ZWWGGYS3XMRW47WQDNNH6M`, `currentRevision=06F4ZX49QZPXD7X3STGQBWVHBC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F492ARW2N6SNYJH15RHMZEN8-story-add-ef-core-misuse-analyzers-for-dvault-in' and commit '2521286203eb' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F492ARW2N6SNYJH15RHMZEN8-story-add-ef-core-misuse-analyzers-for-dvault-in' from source '2521286203eb'.
- Interactive tester tool loop completed review for branch 'ticket/06F492ARW2N6SNYJH15RHMZEN8-story-add-ef-core-misuse-analyzers-for-dvault-in'.
- Evidence: git rev-parse --verify 2521286203eb resolved the claimed commit to 2521286203eb07f4ba05ca7059d21039a74469c7.
- Evidence: git diff --name-status develop...2521286203eb shows the change set adds src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs, src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs, and tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCo...
- Evidence: src/DCoding.Data.DVault.Analyzers/DataVaultEfCoreMisuseAnalyzer.cs:22-42 and 91-258 implement DMV1910/DMV1911 with bounded mutating-method matching, constant generated-table-name checks, and visible metadata-interceptor opt-in suppression.
- Evidence: src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs:8-26 defines stable DMV1910/DMV1911 titles, messages, explanations, and remediation text.
- Evidence: tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs:12-160 asserts positive findings plus non-findings for arbitrary non-DVault dictionary sets, documented AsNoTracking()/compiled-query reads, IDataVaultSaveService usage, and UseDataVaultS...
- Evidence: DVault.slnx:22-23 includes tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj, and src/DCoding.Data.DVault.Analyzers/README.md:33-39 documents the bounded EF Core misuse analyzer scope and published DMV1910/DMV1911 behavior.
- 43 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Proceed to integrator.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8132`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `94c13258245d474d85d89e0673714b69`
- completed-at-utc: `<redacted>-22T13:58:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492ARW2N6SNYJH15RHMZEN8/runs/20260522T135855628Z-94c13258245d474d85d89e0673714b69.json`