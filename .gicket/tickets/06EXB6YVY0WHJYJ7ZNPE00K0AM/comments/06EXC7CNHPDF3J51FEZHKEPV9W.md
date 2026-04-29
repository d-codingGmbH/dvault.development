[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EXB6YVY0WHJYJ7ZNPE00K0AM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB6YVY0WHJYJ7ZNPE00K0AM`.
- Optimistic claim succeeded (`expectedRevision=06EXC6W1A4YG3ZAY5SJNM1GWTW`, `currentRevision=06EXC6YYWZ6RBD54EGGPTVYBQG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB6YVY0WHJYJ7ZNPE00K0AM-task-configure-xml-documentation-and-determinist' from source 'be79de46897388ff0848853b19f18b80704daf7d'.
- Interactive PO-critic tool loop fell back to legacy critic review after MODEL-TOOL-INVOCATION-RESULT-TOOL-CALL-ARGUMENTS-JSON-INVALID.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EXB6YVY0WHJYJ7ZNPE00K0AM-task-configure-xml-documentation-and-determinist` as `d5e84cf6c6c6`.

Open questions / Risiken
- Risky assumption: The contract intentionally leaves SourceLink verification conditional on available repository host or remote metadata, which is acceptable but should be documented by implementation if unavailable.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `20291`
- cached-tokens: `2432`
- effective-cache-ratio: `0.1199`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `7ec1dd1785e44fa0a8e0f223506473e5`
- completed-at-utc: `<redacted>-28T22:02:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB6YVY0WHJYJ7ZNPE00K0AM/runs/20260428T220235700Z-7ec1dd1785e44fa0a8e0f223506473e5.json`