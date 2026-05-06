[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EZ0NV7KG94MTMNXMGVRYVW9C'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NV7KG94MTMNXMGVRYVW9C`.
- Optimistic claim succeeded (`expectedRevision=06EZT2PK8V3JYHDK98B5MJF94R`, `currentRevision=06EZTB9DX6BGGNPBTT99XG54C4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EZ0NV7KG94MTMNXMGVRYVW9C-task-generate-provider-neutral-bridge-ef-model-m' from source 'db959160d4f3d113f462d06acf15f182c672da13'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EZ0NV7KG94MTMNXMGVRYVW9C-task-generate-provider-neutral-bridge-ef-model-m` as `339ef32674ba`.

Open questions / Risiken
- Risky assumption: This ticket assumes the sibling implementation will land DataVaultTableKind.Bridge, a bridge collection on DataVaultMetadataModel, and a distinct hierarchy-depth semantic before work starts here, because current source does not contain them yet.
- Risky assumption: This approval assumes execution ordering continues to honor persisted relation 06EZ0NV0Y81AE1Z1Q3223TX2S4--06EZ0NV7KG94MTMNXMGVRYVW9C--blocks so the mapping ticket is not started before its prerequisite API exists.
- Split recommendation: No additional split is needed for this ticket now; keep richer bridge families such as effectivity windows, path payload columns, closure maintenance, query helpers, and navigation graph generation as separate follow-up tickets.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9115`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `0752966b918b4b249f7bb3053d9e8dd6`
- completed-at-utc: `<redacted>-06T12:12:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NV7KG94MTMNXMGVRYVW9C/runs/20260506T121211041Z-0752966b918b4b249f7bb3053d9e8dd6.json`