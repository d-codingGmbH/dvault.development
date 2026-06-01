[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F7Y0HJ1ZPY7ND9N8RVS92H4C'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0HJ1ZPY7ND9N8RVS92H4C`.
- Optimistic claim succeeded (`expectedRevision=06F7Y0YEJ9EXYJSMTKJB2CE5K8`, `currentRevision=06F88FGFXVXZTPKK6JGV5NRVG4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F7Y0HJ1ZPY7ND9N8RVS92H4C': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F7Y0HJ1ZPY7ND9N8RVS92H4C': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F7Y0HJ1ZPY7ND9N8RVS92H4C-story-generate-typed-bridge-read-helpers-from-su' from source 'd8c926189f1b7d02c5872fcdc04077e273a427a4'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Helper generation is gated by request-bound readShape.bridge support-bundle evidence; redaction or missing endpoint order, filter, or depth facts will intentionally suppress bridge helpers even when runtime bridge metadata exists.
- Hierarchy helpers must preserve the current inclusive maximumDepth boundary exactly; emitting an unbounded overload or widening depth semantics would silently change runtime behavior.
- Deterministic generated-name collisions across bridge types, methods, or constants must still fail with DMV1965 instead of partially emitting broken helper code.
- Split recommendation: No additional split is recommended. The current story is already bridge-only, the upstream contract story is complete, and downstream documentation work is already separated into ticket 06F7Y0HZKHBHMYX9EYDYFRYXZ0.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8204`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `2eab4e155c75440d9166be6bd9476d79`
- completed-at-utc: `<redacted>-01T17:49:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0HJ1ZPY7ND9N8RVS92H4C/runs/20260601T174931545Z-2eab4e155c75440d9166be6bd9476d79.json`