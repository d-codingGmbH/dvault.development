[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F8KZVCVRPS3NAGQA7J55EAA4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZVCVRPS3NAGQA7J55EAA4`.
- Optimistic claim succeeded (`expectedRevision=06FA2WHVM6QWT3XGKMGKHRFY2W`, `currentRevision=06FA2WSET46TD2BCSXE343V4TW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZVCVRPS3NAGQA7J55EAA4-task-define-artifact-benchmark-and-semantic-pari' from source '2544b6c03ba18cd4d8a14d29a0f24c72a2bf42d6'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F8KZVCVRPS3NAGQA7J55EAA4-task-define-artifact-benchmark-and-semantic-pari` as `7ab170eaceeb`.

Open questions / Risiken
- Risky assumption: Treating the current SQL Server dry-run example as general proof for PostgreSQL, MySQL, or Oracle would overstate the evidence; the checked-in external-provider benchmark rows are still skipped when connection strings are unset.
- Risky assumption: Treating skipped optional-provider rows or dry-run manifests as production-readiness proof would violate the ticket contract; exact-provider diagnostics and completed benchmark evidence for the same workload are still required.
- Split recommendation: No new split is justified; keep evidence requirements in 06F8KZVCVRPS3NAGQA7J55EAA4, dry-run manifest prototyping in 06F8KZV18BQ0GN3CE4G02ATVA0, documentation alignment in 06F8KZVRARQPG482YKCQ686PNM, and all-provider baseline capture in 06F9XD26D2MHVAKZ2G...

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7439`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `6d9d637d2b8c459e92c2eec4a8e31414`
- completed-at-utc: `<redacted>-07T09:47:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZVCVRPS3NAGQA7J55EAA4/runs/20260607T094721545Z-6d9d637d2b8c459e92c2eec4a8e31414.json`