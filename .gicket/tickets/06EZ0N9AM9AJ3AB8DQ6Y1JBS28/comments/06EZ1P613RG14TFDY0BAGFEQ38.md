[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EZ0N9AM9AJ3AB8DQ6Y1JBS28'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0N9AM9AJ3AB8DQ6Y1JBS28`.
- Optimistic claim succeeded (`expectedRevision=06EZ1MKMESMDP4DNVR9W1JVV5G`, `currentRevision=06EZ1MW7KB7R7PMWVGCFNTC8WG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EZ0N9AM9AJ3AB8DQ6Y1JBS28-task-add-strategy-selection-tests-for-fallback-v' from source 'f3c314587e33efd57f18e1bde0c16e2f66145e96'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EZ0N9AM9AJ3AB8DQ6Y1JBS28-task-add-strategy-selection-tests-for-fallback-v` as `8034f49029c8`.

Open questions / Risiken
- Risky assumption: This assumes developers will model `missing capability registration` at the DI / `IDataVaultProviderSaveStrategy` boundary, not by expanding `DataVaultProviderCapabilityProfiles.Sqlite`, which serves the metadata-translator surface.
- Risky assumption: This assumes the selected path can be asserted through current `IDataVaultSaveService` behavior or a minimal non-production test seam without widening the production API.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8686`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `e61c7c0164ed42748399935f8be2bbf2`
- completed-at-utc: `<redacted>-04T02:37:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0N9AM9AJ3AB8DQ6Y1JBS28/runs/20260504T023709408Z-e61c7c0164ed42748399935f8be2bbf2.json`