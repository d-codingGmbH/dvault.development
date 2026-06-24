[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FF43AH9SK6J07GV5EKYV3AMM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43AH9SK6J07GV5EKYV3AMM`.
- Optimistic claim succeeded (`expectedRevision=06FFJRBSJ74H2J0NMYTJN0S8N8`, `currentRevision=06FFK97MPY677HBQWHVJQXT5FW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FF43AH9SK6J07GV5EKYV3AMM-task-add-postgresql-pit-full-rebuild-benchmark-l' from source '5045c4614d5e757392d8dc37b6fdce80fd7425a2'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FF43AH9SK6J07GV5EKYV3AMM-task-add-postgresql-pit-full-rebuild-benchmark-l` as `4888299bebe5`.

Open questions / Risiken
- Risky assumption: Treating `pit-as-of-read` or `bridge-traversal-read` rows as PIT maintenance evidence would violate the shared benchmark contract.
- Risky assumption: Assuming the PostgreSQL optimized path can run inside an ambient caller transaction would conflict with the current `CurrentTransactionSavepointUnavailable` fallback gate.
- Split recommendation: Keep SQL Server PIT full-rebuild benchmarking as a sibling ticket because its runtime seam and fallback vocabulary differ from PostgreSQL.
- Split recommendation: Keep any future MySQL, Oracle, or DB2 PIT maintenance benchmarking separate until those provider lanes are explicitly implemented or accepted.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8862`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `b53a0f2066eb4fdbaf2f9c3038c668fb`
- completed-at-utc: `<redacted>-24T12:46:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43AH9SK6J07GV5EKYV3AMM/runs/20260624T124605041Z-b53a0f2066eb4fdbaf2f9c3038c668fb.json`