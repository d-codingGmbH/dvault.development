[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEFHKF04B746X7GJKRVT04'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MEFHKF04B746X7GJKRVT04`.
- Optimistic claim succeeded (`expectedRevision=06F1SA4XQ4KECMZ4A3CFTV8TP4`, `currentRevision=06F1SACA5C1BK0Q5AS0Y2RX2GR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEFHKF04B746X7GJKRVT04': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEFHKF04B746X7GJKRVT04': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry' from source '1e40b686920065641ef7f4ded85db9b3aeb65db1'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry` as `a8fd6f896b63`.

Open questions / Risiken
- Existing callers that still populate legacy PointInTimeTables will receive export failures until they migrate to Pits, so diagnostics and docs must be explicit.
- Test-only/internal Code-First coverage must not be documented in a way that implies a new public raw Code-First export API.
- Serializer configuration must still be fixed by tests so rejection handling does not mask ordering or formatting regressions on successful exports.
- Split recommendation: No split recommended; the ticket is bounded once the public input surface is limited to existing model/registry objects and PointInTimeTables are defined as a deterministic rejection case.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `91390`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0266`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `c03d2a0a2e01450e9f2cb2a8ca30f92a`
- completed-at-utc: `<redacted>-12T15:27:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MEFHKF04B746X7GJKRVT04/runs/20260512T152731435Z-c03d2a0a2e01450e9f2cb2a8ca30f92a.json`