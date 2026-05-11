[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F0MEDJC732GDD77H60R259P0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F0MEDJC732GDD77H60R259P0`.
- Optimistic claim succeeded (`expectedRevision=06F1A7MWC9TQ3JBXSS0WZK56EG`, `currentRevision=06F1A7W14JDAV5JNME0QAHHTRG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F0MEDJC732GDD77H60R259P0-task-update-readme-and-release-docs-for-v0-6-0-u' from source '273fb9a2a02214931f6b33a88756d6744548ac26'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F0MEDJC732GDD77H60R259P0-task-update-readme-and-release-docs-for-v0-6-0-u` as `36dc098f3a36`.

Open questions / Risiken
- Blocking finding: No outgoing parentOf child tickets were found for the tracking-only parent ticket.
- Required PO action: Resolve the tracking-parent closure audit findings before this parent ticket can be closed.
- Risky assumption: Final audited 0.6.0 package filenames and publish approval remain release-operator work after the v0.6.0 tag exists; this review relies on the contract-approved pre-tag MinVer validation evidence.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9444`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `7e8e08d15c2b4f8daddae1aaf1874ef8`
- completed-at-utc: `<redacted>-11T03:48:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F0MEDJC732GDD77H60R259P0/runs/20260511T034854315Z-7e8e08d15c2b4f8daddae1aaf1874ef8.json`