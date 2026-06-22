[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FE4RJ4CC2YRVK0P98NBSXRKC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4RJ4CC2YRVK0P98NBSXRKC`.
- Optimistic claim succeeded (`expectedRevision=06FEZR7Y37B6012QYDCXYPTZ60`, `currentRevision=06FEZRGQE8YMXX9VQHQXWRYJHG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FE4RJ4CC2YRVK0P98NBSXRKC-story-define-server-side-pit-and-bridge-maintena' from source 'c35093debcd3229d145f673465e0fb51c0b81fd9'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FE4RJ4CC2YRVK0P98NBSXRKC-story-define-server-side-pit-and-bridge-maintena` as `9c56ac72860d`.

Open questions / Risiken
- Risky assumption: This approval assumes the parent remains a boundary/tracking story and that downstream child tickets inherit its provider baseline and automation non-goals instead of redefining them.
- Split recommendation: Keep 06FE4RJD5Z6MWC2E66YB3EZ5YW (PIT dry-run diagnostics), 06FE4RK80ZXGCZ62CMSAYP164W (bridge feasibility), 06FE4RJP5KG02DF7AEMCQYGNVW (PostgreSQL PIT prototype), 06FE4RJZ4PA0DZ3HXDSEG2BQMM (SQL Server PIT prototype), and 06FE4RKGASKV6F7DF0RD1WTAV4 (docum...
- Split recommendation: Do not open a bridge implementation ticket until 06FE4RK80ZXGCZ62CMSAYP164W closes the bounded feasibility question.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8786`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `ad15804109ea46aaa6f6f82ac0464cd7`
- completed-at-utc: `<redacted>-22T15:17:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4RJ4CC2YRVK0P98NBSXRKC/runs/20260622T151703328Z-ad15804109ea46aaa6f6f82ac0464cd7.json`