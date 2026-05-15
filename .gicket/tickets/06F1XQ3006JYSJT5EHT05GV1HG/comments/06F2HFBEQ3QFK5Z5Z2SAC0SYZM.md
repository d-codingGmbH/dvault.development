[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XQ3006JYSJT5EHT05GV1HG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F1XQ3006JYSJT5EHT05GV1HG`.
- Optimistic claim succeeded (`expectedRevision=06F1XTQHHRZJCKTTF0B8QZEZ1M`, `currentRevision=06F2HEQEQ6Y6MDRNT5HA2Y7SPM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XQ3006JYSJT5EHT05GV1HG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XQ3006JYSJT5EHT05GV1HG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XQ3006JYSJT5EHT05GV1HG-task-add-production-adoption-checklist-draft' from source 'c1741d0d38bc29c392fb2ff7d92c40864f721e3e'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F1XQ3006JYSJT5EHT05GV1HG-task-add-production-adoption-checklist-draft` as `f41215b9b3d3`.

Open questions / Risiken
- The checklist could become duplicative if it restates existing README, architecture, governance, and publication docs instead of linking to them.
- The incoming blocks relation from 06F1XPX99KQRB09GRQG50Z75FM remains live in relation state; no cleanup was performed because this run had no evidence that it is stale.
- NuGet/package wording needs care: current docs show released-package installation guidance and manual publication policy, but this ticket must not claim availability for unpublished package versions.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `44253`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0550`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `72f6cb32f4a54da796358a2aaa4349cc`
- completed-at-utc: `<redacted>-14T23:06:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F1XQ3006JYSJT5EHT05GV1HG/runs/20260514T230603692Z-72f6cb32f4a54da796358a2aaa4349cc.json`