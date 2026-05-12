[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F0MEJPGG7JBFEXD693BHY07W'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F0MEJPGG7JBFEXD693BHY07W`.
- Optimistic claim succeeded (`expectedRevision=06F1VHBPCAPEMZQEG7G04XV5YW`, `currentRevision=06F1VHY08BH47S2A3P578EQ5FM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F0MEJPGG7JBFEXD693BHY07W-task-update-docs-and-release-notes-for-v0-7-0-mo' from source 'f49829e468c1a9b20171c88bc5114a0edfe2ca30'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F0MEJPGG7JBFEXD693BHY07W-task-update-docs-and-release-notes-for-v0-7-0-mo` as `4af80314120c`.

Open questions / Risiken
- Risky assumption: The docs developer must treat current source as more current than stale README wording when describing PIT, bridge, and SQLite read strategy behavior.
- Risky assumption: v0.7.0 package wording must not imply NuGet publication, package hashes, or final publication links before release packaging happens.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9023`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `03c0370f421c498d8c1b964eba9482d3`
- completed-at-utc: `<redacted>-12T20:07:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F0MEJPGG7JBFEXD693BHY07W/runs/20260512T200717079Z-03c0370f421c498d8c1b964eba9482d3.json`