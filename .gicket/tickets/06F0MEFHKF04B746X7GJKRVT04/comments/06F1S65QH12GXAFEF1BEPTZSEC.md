[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEFHKF04B746X7GJKRVT04'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.3` was applied to ticket `06F0MEFHKF04B746X7GJKRVT04`.
- Optimistic claim succeeded (`expectedRevision=06F0QH3FCA1KX7YSG8ZW06F1H4`, `currentRevision=06F1S4VWEXMTWJQV2X83393RQM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.3`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEFHKF04B746X7GJKRVT04': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEFHKF04B746X7GJKRVT04': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry' from source '43b12d5eeefeb5ae57aba3795c27836651429a47'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The model-first contract may include shapes that are broader than today's Code-First fluent surface, especially PIT and bridge metadata; implementation should export what the registry can represent and test the supported subset explicitly.
- Round-trip equality should compare supported metadata semantics rather than incidental object identity or runtime-only fields.
- Serializer defaults can accidentally destabilize output if property ordering or null/default omission behavior is not fixed in tests.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `43973`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0553`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `117103b6190841b3b1ac4eb9e682d7f5`
- completed-at-utc: `<redacted>-12T14:30:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/77ab9562dde05301902c1ce959c54a9e729a6376a305f47811212a4df17a5a96/tickets/06F0MEFHKF04B746X7GJKRVT04/runs/20260512T143030383Z-117103b6190841b3b1ac4eb9e682d7f5.json`