[gicket-bot] Run report (outcome: po-refinement-failed)

Summary
- PO refinement for ticket '06EZ0NSQFCD3W4CDCJ44GFSKA0' failed because the model response was not parseable.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NSQFCD3W4CDCJ44GFSKA0`.
- Optimistic claim succeeded (`expectedRevision=06EZM5VV454KD6AE3HP6MP9DP4`, `currentRevision=06EZM5Y7KFAN6E3JTEKSJQFQAC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EZ0NSQFCD3W4CDCJ44GFSKA0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NSQFCD3W4CDCJ44GFSKA0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NSQFCD3W4CDCJ44GFSKA0-task-add-api-snapshot-guardrails-for-deferred-ca' from source 'f53bd179104e59344e9082d1102391011991364e'.
- Interactive PO tool loop hit bounded stop reason 'legacy_verification_requested' and fell back to legacy planning.

Open questions / Risiken
- Model response must provide 'handoff_decision' as 'ready_for_po_critic' or 'needs_po_clarification'. Captured raw model response: C:/Projects/DVault/.gicket-bot/logs/model-response-diagnostics/20260505T215211637Z-po-po-refinement-06EZ0NSQFCD3W4CDCJ44GFSKA0.json.

Next steps
- Review ticket comments and bot logs.
- Retry PO refinement after resolving the reported issue.

Prompt cache usage
- prompt-tokens: `32790`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0742`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `39130af7b36a4c21843b66301eb4502d`
- completed-at-utc: `<redacted>-05T21:52:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NSQFCD3W4CDCJ44GFSKA0/runs/20260505T215213751Z-39130af7b36a4c21843b66301eb4502d.json`