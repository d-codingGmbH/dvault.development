[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FH8QAVJFXANVQFXGPYVAFXSR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8QAVJFXANVQFXGPYVAFXSR`.
- Optimistic claim succeeded (`expectedRevision=06FHC8N08HBX9ZAD76S30CZSVR`, `currentRevision=06FHC90NDXM3J5Q8BW63RBW7H4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FH8QAVJFXANVQFXGPYVAFXSR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FH8QAVJFXANVQFXGPYVAFXSR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FH8QAVJFXANVQFXGPYVAFXSR-story-deliver-net-8-sdk-compatible-analyzer-supp' from source 'd3dbb617e0835d15a42bb3a9ad3250d26ffa5ec8'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Until queued replay finishes on ticket 06FH8RP1SBVZ7K3K48ERGZSMQC, the follow-up's persisted description may temporarily lag the intended 8.51.0 / 10.51.0 delivery contract.
- A later package-line roll-forward can drift if changelog, release notes, install guidance, pack script, and package verification are not updated together on the follow-up ticket.
- Split recommendation: No additional split is needed; this parent is now bounded to the landed 8.50.0 / 10.50.0 baseline, and ticket 06FH8RP1SBVZ7K3K48ERGZSMQC is the single remaining carrier for the future 8.51.0 / 10.51.0 release-surface work.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `46630`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0522`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `e5f73f0937934c1d8296ea21265e88a1`
- completed-at-utc: `<redacted>-30T01:45:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8QAVJFXANVQFXGPYVAFXSR/runs/20260630T014530855Z-e5f73f0937934c1d8296ea21265e88a1.json`