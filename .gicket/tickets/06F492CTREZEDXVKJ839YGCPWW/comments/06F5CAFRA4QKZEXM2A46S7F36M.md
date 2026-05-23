[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F492CTREZEDXVKJ839YGCPWW'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492CTREZEDXVKJ839YGCPWW`.
- Optimistic claim succeeded (`expectedRevision=06F5C7S5BG8N558SKY9N346KZC`, `currentRevision=06F5C8CM489NVE4ADWK1HHP0E0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F492CTREZEDXVKJ839YGCPWW-story-add-provider-optimization-regression-basel' from source '72b3eb3b1a02edf2eca5f372635f89b6363d7fbf'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F492CTREZEDXVKJ839YGCPWW-story-add-provider-optimization-regression-basel` as `1977ed518c24`.

Open questions / Risiken
- Risky assumption: Assumes the shared artifact contract can be extended for execution-detail evidence without breaking downstream consumers, even though current BenchmarkSummary and benchmark-summary outputs only carry the existing row fields.
- Risky assumption: Assumes a diagnostics-derived detail string is acceptable audit evidence when generated SQL is not practical to persist; the ticket leaves the minimum content of that detail implicit.
- Split recommendation: No split needed; the existing contract and repository evidence already bound this as one benchmark-contract extension story.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9285`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `e18f7f675d54413099d65862bc1f8d4e`
- completed-at-utc: `<redacted>-23T18:47:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492CTREZEDXVKJ839YGCPWW/runs/20260523T184731660Z-e18f7f675d54413099d65862bc1f8d4e.json`