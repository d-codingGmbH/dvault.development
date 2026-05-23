[gicket-bot] Run report (outcome: dev-workflow-apply)

Summary
- Developer workflow prepared branch 'ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an' and commit 'c9a18dd0ebab' for ticket '06F492C50WM7V2NE0WZB3774XM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492C50WM7V2NE0WZB3774XM`.
- Optimistic claim succeeded (`expectedRevision=06F5AQNNHT5P2TZQSQ65174XWM`, `currentRevision=06F5AR33BZFW7MNNP7CFDR7PZM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive developer scratch worktree for target branch 'ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an' from source 'ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an'.
- Planned implementation step: Extended existing read-shape diagnostics records with init-only projected-column members and PIT lookup count without replacing IDataVaultReadDiagnosticsService or DataVaultDiagnosticsResult.ReadShape.
- Planned implementation step: Populated satellite technical, payload, and optional driving-key projection groups from the existing satellite projection pipeline.
- Planned implementation step: Populated PIT technical, snapshot-reference, satellite-payload projection groups and referenced satellite lookup count while preserving existing PIT index baseline behavior.
- Planned implementation step: Populated bridge endpoint projection groups and depth projection groups for bounded hierarchy requests while preserving traversal index baseline behavior.
- Planned implementation step: Expanded the existing read-shape diagnostics unit test and regenerated the core public API snapshot for the additive public members.
- Resolved branch route (fallback): base 'develop', work 'ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an'.
- Executed runtime-orchestration sync-first fetch/pull before workspace automation.
- Checked out existing branch 'ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an'.
- 12 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Risk: Verification still reports pre-existing NU1900 vulnerability-cache warnings in this sandbox because the NuGet HTTP cache is read-only; these did not fail build or test.
- Risk: External provider integration tests remain opt-in and were skipped without local provider connection strings.

Next steps
- Push branch 'ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an' manually if remote collaboration is required.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9756`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `dev`
- run-id: `33edabb91dea408bb4ff444f9f34e943`
- completed-at-utc: `<redacted>-23T15:56:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492C50WM7V2NE0WZB3774XM/runs/20260523T155610750Z-33edabb91dea408bb4ff444f9f34e943.json`