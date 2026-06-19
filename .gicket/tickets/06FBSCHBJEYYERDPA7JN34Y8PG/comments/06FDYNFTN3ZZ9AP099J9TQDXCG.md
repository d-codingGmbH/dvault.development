[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSCHBJEYYERDPA7JN34Y8PG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCHBJEYYERDPA7JN34Y8PG`.
- Optimistic claim succeeded (`expectedRevision=06FBSCHGRQ8DG00B3FRP392BV0`, `currentRevision=06FDYK89H7CJGFF3Z698KV7H3C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSCHBJEYYERDPA7JN34Y8PG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSCHBJEYYERDPA7JN34Y8PG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSCHBJEYYERDPA7JN34Y8PG-task-document-provider-read-parity-outcomes-and' from source '205905f3e265c023daf6eebb3ec8cb580fdaf6ea'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 4 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Docs can easily overclaim non-SQLite latest-satellite performance because the root benchmark rows already carry planned strategy names while remaining skipped placeholders.
- Docs drift remains likely unless the performance guide, PIT/bridge boundary note, and v0.40.0 release note are updated together against the evidence matrix and gap matrix.
- The stale incoming `blocks` relations from done tickets may confuse later workflow review if they are not cleaned up after documentation delivery.
- Split recommendation: No additional split is justified for this ticket; the current repository already provides a finite documentation baseline.
- Split recommendation: If future work is opened, keep it split between latest-satellite timing collection and DB2 PIT/bridge environment-backed evidence activation rather than reopening this documentation ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9090`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `2d62d94ccaf348a19d75ca752b4ee52a`
- completed-at-utc: `<redacted>-19T10:03:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCHBJEYYERDPA7JN34Y8PG/runs/20260619T100334416Z-2d62d94ccaf348a19d75ca752b4ee52a.json`