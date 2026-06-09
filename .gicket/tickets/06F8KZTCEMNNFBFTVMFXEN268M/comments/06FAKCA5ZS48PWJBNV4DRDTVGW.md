[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZTCEMNNFBFTVMFXEN268M'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZTCEMNNFBFTVMFXEN268M`.
- Optimistic claim succeeded (`expectedRevision=06F9XD3BV5C46X4KHYY9GX8HJW`, `currentRevision=06FAKAJER58S3BFM27RDCYGSDM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZTCEMNNFBFTVMFXEN268M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZTCEMNNFBFTVMFXEN268M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZTCEMNNFBFTVMFXEN268M-epic-high-performance-artifact-evidence-lane' from source '8517ddbdf82cf063ba0c52e0e74b38825ef27771'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Future readers may overread the supported-provider baseline as implemented artifact-export coverage unless follow-up work keeps the SQL Server-only prototype boundary explicit.
- The live historical `blocks` relation from 06F8KZVRARQPG482YKCQ686PNM back to the epic can confuse relation-only readers even though the epic itself is not currently blocked.
- Later work could incorrectly widen this review-only design-time lane into runtime dispatch or automatic migration behavior unless it stays anchored to the existing non-goals and exact-provider evidence gate.
- Split recommendation: No new split is justified; the live child set already separates architecture contract, evidence rules, dry-run prototype, documentation alignment, and follow-on threshold evidence.
- Split recommendation: Create separate follow-up tickets instead of reopening this epic if the team wants additional provider exporters, deployable sidecar SQL payloads, runtime invocation helpers, repository storage conventions, or provider-specific validators.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9048`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `b7c6c83934884db38ca967698a01d704`
- completed-at-utc: `<redacted>-09T00:03:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZTCEMNNFBFTVMFXEN268M/runs/20260609T000347832Z-b7c6c83934884db38ca967698a01d704.json`