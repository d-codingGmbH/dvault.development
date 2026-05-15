[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F2PGG57K3S7CJQP5QX9AWW3G'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGG57K3S7CJQP5QX9AWW3G`.
- Optimistic claim succeeded (`expectedRevision=06F2PW1WYBDR449SQA0QDEE5TW`, `currentRevision=06F2PW7SVYVGKDGRJW3KJYPYQW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F2PGG57K3S7CJQP5QX9AWW3G-task-define-live-schema-reader-contract-and-fixt' from source 'c87b5bb0593039b0fea5c32b9e21651875899b9c'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F2PGG57K3S7CJQP5QX9AWW3G-task-define-live-schema-reader-contract-and-fixt` as `471a4395e2fe`.

Open questions / Risiken
- Risky assumption: Assumes PostgreSQL, SQL Server, Oracle, and MySQL catalogs can all map into the existing DataVaultLiveSchemaPrimaryKey and DataVaultLiveSchemaIndex shapes without reopening the public API.
- Risky assumption: Assumes developer-managed external databases grant create and drop permissions for isolated objects; README external-provider guidance already makes those lanes opt-in.
- Split recommendation: No split is needed for this contract-and-fixture ticket. If provider catalog quirks make implementation too large, split Task 06F2PGG8ZKSYGC8863118H56G8 by provider after this contract ticket lands.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8429`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `7bbb629c723343088206e5054258da62`
- completed-at-utc: `<redacted>-15T11:47:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGG57K3S7CJQP5QX9AWW3G/runs/20260515T114744397Z-7bbb629c723343088206e5054258da62.json`