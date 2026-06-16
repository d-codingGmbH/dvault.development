[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FBSCAQGWFC9S98YCVDP4V7PC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCAQGWFC9S98YCVDP4V7PC`.
- Optimistic claim succeeded (`expectedRevision=06FCY82015EQF8D124QQYARNV8`, `currentRevision=06FCY88CFC083FJ35H0GZ00KMR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FBSCAQGWFC9S98YCVDP4V7PC-task-implement-accepted-db2-bulk-improvement' from source '09ef7af6fdaec87f150b8f4537aad7df86bb22c3'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FBSCAQGWFC9S98YCVDP4V7PC-task-implement-accepted-db2-bulk-improvement` as `6c51a716f88b`.

Open questions / Risiken
- Blocking finding: `.gicket/tickets/06FBSCAQGWFC9S98YCVDP4V7PC/comments/06FCY6SZ8XFX59KFA143ZG0EM8.md` already recorded the same workflow-state risk, and the current `ticket.json` still shows that mismatch.
- Required PO action: If additional DB2 benchmark or documentation evidence is still desired, track it on a separate narrow follow-up ticket rather than reopening `06FBSCAQGWFC9S98YCVDP4V7PC`.
- Risky assumption: Assuming downstream automation or reviewers will infer `no-work-required closure` from `description.md` alone while `.gicket/tickets/06FBSCAQGWFC9S98YCVDP4V7PC/ticket.json` still says `todo` and carries implementation-routing labels.
- Risky assumption: Assuming skipped-placeholder DB2 benchmark rows or opt-in smoke coverage will not be overstated as completed DB2 timing evidence despite the contract and release note warning against that.
- Split recommendation: Do not split this implementation ticket into new developer work.
- Split recommendation: If the team still wants DB2 evidence or documentation expansion, use one separate narrow evidence or documentation follow-up ticket.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8848`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `662998732ed7452c95851aa1c81ac0e0`
- completed-at-utc: `<redacted>-16T06:40:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCAQGWFC9S98YCVDP4V7PC/runs/20260616T064023195Z-662998732ed7452c95851aa1c81ac0e0.json`