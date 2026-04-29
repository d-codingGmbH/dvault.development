[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB6PX7ZGYNR2SXF44C5VPJM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB6PX7ZGYNR2SXF44C5VPJM`.
- Optimistic claim succeeded (`expectedRevision=06EXBF7ZFH9N4P9XR23MGD3QPM`, `currentRevision=06EXCNY2Y0JY0M36G3DTSR4PB4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB6PX7ZGYNR2SXF44C5VPJM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB6PX7ZGYNR2SXF44C5VPJM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB6PX7ZGYNR2SXF44C5VPJM-task-document-mvp-data-vault-concepts' from source '45ce747ab3bc42c644a9a54d86a71a6a2b2b1197'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Because no source or test roots exist yet, examples may need minor adjustment when the first concrete test layout is introduced.
- If hash algorithm details are documented too specifically before implementation, future code may be constrained prematurely.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `18830`
- cached-tokens: `12160`
- effective-cache-ratio: `0.6458`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `7e35f038845440b69a372bfd9481088b`
- completed-at-utc: `<redacted>-28T23:08:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB6PX7ZGYNR2SXF44C5VPJM/runs/20260428T230846500Z-7e35f038845440b69a372bfd9481088b.json`