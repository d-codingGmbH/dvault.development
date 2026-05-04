[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EZ0NB4965QZZYG0Z1PG5YY7C'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NB4965QZZYG0Z1PG5YY7C`.
- Optimistic claim succeeded (`expectedRevision=06EZ6Y9RN9W9AS3KAST1CCFWXG`, `currentRevision=06EZ6YRC0ZBG1YVXNAADJT249W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EZ0NB4965QZZYG0Z1PG5YY7C-story-optimize-oracle-provider-save-strategy' from source '1d1220c57a6399837694d93925fd4c72d5777f6b'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EZ0NB4965QZZYG0Z1PG5YY7C-story-optimize-oracle-provider-save-strategy` as `a7a1bcef8d76`.

Open questions / Risiken
- Risky assumption: The story assumes the only supported Oracle EF provider identity is the exact string `Oracle.EntityFrameworkCore`, matching `OracleDataVaultSaveStrategy.CanSave`.
- Risky assumption: The story assumes developers running opt-in validation have Oracle users with create/drop table privileges, as required by `README.md` and `OracleDataVaultSmokeTests`.
- Risky assumption: The story assumes future edits to the child tickets will stay aligned with the hub/link-only v1 Oracle boundary; the current child contracts are compatible, but that alignment is still a coordination risk.
- Split recommendation: No additional split recommended; the existing child tickets already separate Oracle capability/writer work from Oracle opt-in smoke validation.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9479`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `e8f5222db3d942d2877c20566dec09db`
- completed-at-utc: `<redacted>-04T15:00:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NB4965QZZYG0Z1PG5YY7C/runs/20260504T150009339Z-e8f5222db3d942d2877c20566dec09db.json`