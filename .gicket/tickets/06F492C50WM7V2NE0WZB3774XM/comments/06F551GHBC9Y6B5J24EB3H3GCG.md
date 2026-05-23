[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F492C50WM7V2NE0WZB3774XM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492C50WM7V2NE0WZB3774XM`.
- Optimistic claim succeeded (`expectedRevision=06F54ZHS24TATYFTAH904GS23M`, `currentRevision=06F54ZYQ016X4X9KT33EVS69MR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an' from source 'd8f4f1121b46c44d88d1e3645f7a43d43299b92a'.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an` as `4f077c8c11bf`.

Open questions / Risiken
- Blocking finding: The persisted delivery contract names the wrong existing compatibility surface for request-bound read diagnostics. It tells developers to preserve `IDataVaultDiagnosticsService` ownership and not require `IDataVaultReadDiagnosticsService`, but current source ...
- Blocking finding: The ticket is internally inconsistent: the latest PO refinement comment says the baseline already includes `DataVaultDiagnosticsResult.ReadShape`, `DataVaultReadShapeDiagnostics`, and `IDataVaultReadDiagnosticsService`, while the current durable description r...
- Required PO action: Restore the durable contract to the actual source-backed baseline: `DataVaultDiagnosticsResult.ReadShape`, `DataVaultReadShapeDiagnostics`, and `IDataVaultReadDiagnosticsService` are existing public request-bound read diagnostics surfaces.
- Required PO action: Rewrite Acceptance Criteria, Definition of Done, and Implementation Notes so request-bound work references the correct service and proof points, including `IDataVaultReadDiagnosticsService` and `ReadDiagnosticsPopulateReadShapeForExplicitRegistryPitAndBridg...
- Required PO action: Align the ticket title with the narrowed additive scope if new index-hint work is no longer intended; current source already exposes `ExpectedIndexBaseline` and `ExpectedTraversalIndexBaseline`.
- Risky assumption: Assuming developers can safely treat `IDataVaultDiagnosticsService` as the request-bound compatibility anchor despite current source and tests proving `IDataVaultReadDiagnosticsService` already owns that surface.
- Risky assumption: Assuming the title phrase `index hints` will not pull implementation back toward new index/provider advice, even though the current narrowed contract treats existing index baselines as already shipped behavior.
- Split recommendation: No split is required once PO restores the correct existing read-diagnostics baseline; the narrowed `ProjectedColumns` plus PIT lookup-count addition is still one bounded story.
- Split recommendation: If Product still wants broader index/provider guidance beyond the existing index baseline fields, keep that as a separate follow-up story.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8573`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `38d075b7f5954136b5bc7d2d5783a73f`
- completed-at-utc: `<redacted>-23T01:49:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492C50WM7V2NE0WZB3774XM/runs/20260523T014938519Z-38d075b7f5954136b5bc7d2d5783a73f.json`