[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F0MEFX5M9V9SA25N76CPGT4M'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F0MEFX5M9V9SA25N76CPGT4M`.
- Optimistic claim succeeded (`expectedRevision=06F1S7Y4R29PK8EGR6M61A1D44`, `currentRevision=06F1S83Z3K947T7N748GH8CQGR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F0MEFX5M9V9SA25N76CPGT4M-task-add-model-diff-and-drift-report-for-generat' from source '9f49d2d5a47e2d62d01698d0b1a193339c5a8b5b'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F0MEFX5M9V9SA25N76CPGT4M-task-add-model-diff-and-drift-report-for-generat` as `7e594ad1772b`.

Open questions / Risiken
- Risky assumption: The exact difference code strings are left to implementation; the contract constrains them to be stable and deterministic but does not enumerate the literal code taxonomy.
- Risky assumption: Key and index comparison will need to combine DVault annotations with EF Core key/index APIs because observed source only annotates keys/indexes with ProducedName and index Ordinal.
- Risky assumption: The repository contains both DataVaultTableKind.PointInTime and DataVaultTableKind.Pit; implementation should align v1 artifact PIT handling with the model-first Pits contract and avoid accidentally drifting into legacy point-in-time behavior.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9351`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `68b2d3b9840047b3abc43b4c97765594`
- completed-at-utc: `<redacted>-12T14:52:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F0MEFX5M9V9SA25N76CPGT4M/runs/20260512T145240897Z-68b2d3b9840047b3abc43b4c97765594.json`