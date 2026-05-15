[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06F1XQ2MB5Y9JW25W2CWVZZ9G4-story-refresh-adoption-examples-and-production-c' for ticket '06F1XQ2MB5Y9JW25W2CWVZZ9G4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F1XQ2MB5Y9JW25W2CWVZZ9G4`.
- Optimistic claim succeeded (`expectedRevision=06F2J8GNTB7AV93X9KBXNHVHS0`, `currentRevision=06F2J8X32Y23T3833SBCPZ413M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Selected verification source branch 'ticket/06F1XQ2MB5Y9JW25W2CWVZZ9G4-story-refresh-adoption-examples-and-production-c' and commit 'e3147fecf137' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F1XQ2MB5Y9JW25W2CWVZZ9G4-story-refresh-adoption-examples-and-production-c' from source 'e3147fecf137'.
- Interactive tester tool loop completed review for branch 'ticket/06F1XQ2MB5Y9JW25W2CWVZZ9G4-story-refresh-adoption-examples-and-production-c'.
- Evidence: git show --name-status --oneline --no-renames e3147fecf137 reported commit e3147fecf [06F1XQ2MB5Y9JW25W2CWVZZ9G4] with M examples/README.md.
- Evidence: git diff --name-status develop...e3147fecf137 showed M examples/README.md plus .gicket ticket metadata files; no src/ product-code paths changed.
- Evidence: git cat-file -e succeeded for all required output paths: README.md, examples/README.md, docs/production-adoption-checklist.md, docs/model-first-governance.md, docs/architecture/dvault-dotnet-ef-design-time-workflow.md, docs/releases/v0.8.0.md, and docs/manual-nuget-p...
- Evidence: git diff --check develop...e3147fecf137 -- examples/README.md exited 0.
- Evidence: git grep at e3147fecf137 found README.md and examples/README.md dotnet add package commands for DCoding.Data.DVault, Sqlite, Postgres, MySql, Oracle, and SqlServer all using --version 0.9.0; docs/production-adoption-checklist.md lists the same six IDs.
- Evidence: git grep at e3147fecf137 confirmed source PackageId values for the six DVault packages and IsPackable=false for src/DCoding.Data/DCoding.Data.csproj.
- 48 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Proceed to the integrator gate.
- Run the full policy verification commands in the normal writable verification environment if the integrator requires executable evidence for dotnet test DVault.slnx --nologo and bash tools/check-format.sh.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8831`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `09458f5812a84389b836e8d1fd357de3`
- completed-at-utc: `<redacted>-15T01:04:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F1XQ2MB5Y9JW25W2CWVZZ9G4/runs/20260515T010405608Z-09458f5812a84389b836e8d1fd357de3.json`