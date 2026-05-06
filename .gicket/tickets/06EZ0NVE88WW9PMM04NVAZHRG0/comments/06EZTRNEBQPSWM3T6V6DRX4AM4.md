[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EZ0NVE88WW9PMM04NVAZHRG0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NVE88WW9PMM04NVAZHRG0`.
- Optimistic claim succeeded (`expectedRevision=06EZTPR0RDA14SRC8FZQ1X0HW4`, `currentRevision=06EZTQ8WWPDSKHKP44N0QSQ1D4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EZ0NVE88WW9PMM04NVAZHRG0-task-add-bridge-documentation-and-example-scenar' from source 'cda371b80d5670f2f38ea967e8168c4aefa07704'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EZ0NVE88WW9PMM04NVAZHRG0-task-add-bridge-documentation-and-example-scenar` as `30e6c9a0dcba`.

Open questions / Risiken
- Required PO action: Republish the handoff after the labels are corrected so the description, comments, and ticket.json all advertise the same routing decision.
- Split recommendation: No split is needed after metadata alignment; this remains a bounded docs-only task.
- Split recommendation: If later work needs hierarchy-specific walkthroughs or docs tied to a concrete bridge runtime surface, keep that as a separate follow-up ticket.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8582`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `d3afcf54df9a4018828d0bfc6dc1362d`
- completed-at-utc: `<redacted>-06T13:03:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NVE88WW9PMM04NVAZHRG0/runs/20260506T130315118Z-d3afcf54df9a4018828d0bfc6dc1362d.json`