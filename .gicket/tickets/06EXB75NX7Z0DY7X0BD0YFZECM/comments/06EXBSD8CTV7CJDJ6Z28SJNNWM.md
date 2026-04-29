[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EXB75NX7Z0DY7X0BD0YFZECM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB75NX7Z0DY7X0BD0YFZECM`.
- Optimistic claim succeeded (`expectedRevision=06EXBRRFVVY42XHYYNQB231PDW`, `currentRevision=06EXBRVRQSA18S7FBTVWPNMXAM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB75NX7Z0DY7X0BD0YFZECM-task-define-default-table-and-column-naming-poli' from source 'c2f5ff20b03d3a60ee94a4c8106f208d93f5fa72'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EXB75NX7Z0DY7X0BD0YFZECM-task-define-default-table-and-column-naming-poli` as `f07fc7400f43`.

Open questions / Risiken
- Risky assumption: The naming policy depends on the sibling technical metadata contract remaining aligned; the ticket already calls this out as a risk and implementation note rather than leaving it ambiguous.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `23870`
- cached-tokens: `2432`
- effective-cache-ratio: `0.1019`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `055ee5633dbc41bbb3eea1b5aec5247a`
- completed-at-utc: `<redacted>-28T21:01:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB75NX7Z0DY7X0BD0YFZECM/runs/20260428T210130481Z-055ee5633dbc41bbb3eea1b5aec5247a.json`