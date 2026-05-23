[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F492C50WM7V2NE0WZB3774XM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492C50WM7V2NE0WZB3774XM`.
- Optimistic claim succeeded (`expectedRevision=06F5ANESCDFNYH7JZTMWP1XZM8`, `currentRevision=06F5ANWVF3Q61NAFQQ5WZ3MYJ4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an' from source 'f9577aafd250060cf9072cccbcc31ba428ead6d9'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an` as `88b411a26b71`.

Open questions / Risiken
- Risky assumption: Approval assumes developers will follow the delivery contract rather than the stale `index hints` wording in the stored ticket title.
- Split recommendation: No delivery split is required for the implementation scope; if Product wants the stored title cleaned up, keep that as a separate ticket-admin follow-up rather than expanding this story.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7009`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `c3233479acf8438386ec2177b168bcb0`
- completed-at-utc: `<redacted>-23T15:04:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492C50WM7V2NE0WZB3774XM/runs/20260523T150450614Z-c3233479acf8438386ec2177b168bcb0.json`