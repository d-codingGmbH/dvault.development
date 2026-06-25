[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FF43T2EK3CBYHTR287YWC5NR'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43T2EK3CBYHTR287YWC5NR`.
- Optimistic claim succeeded (`expectedRevision=06FFWF4GP2Q4ZYMGCZ55S86FMW`, `currentRevision=06FFWP559MB85V5D25PZ7KXN6M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FF43T2EK3CBYHTR287YWC5NR-task-add-postgresql-binary-first-provider-quicks' from source '0efbcbfdba9879c6646f91fe02bcc564d49012b7'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FF43T2EK3CBYHTR287YWC5NR-task-add-postgresql-binary-first-provider-quicks` as `7cd734405353`.

Open questions / Risiken
- Risky assumption: The contract assumes the developer will update at least one primary quickstart surface beyond the already PostgreSQL-aware `examples/README.md`; the natural candidates are `README.md` or `docs/getting-started.md`, which currently still read SQLite-first/provi...
- Risky assumption: If `examples/README.md` is touched for parity wording, its stale `8.45.0` / `10.45.0` install blocks need to be corrected in the same ticket so the docs do not keep visible version drift.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8414`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `d6374378d4334041bb3461a81bcb2841`
- completed-at-utc: `<redacted>-25T10:40:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43T2EK3CBYHTR287YWC5NR/runs/20260625T104042528Z-d6374378d4334041bb3461a81bcb2841.json`