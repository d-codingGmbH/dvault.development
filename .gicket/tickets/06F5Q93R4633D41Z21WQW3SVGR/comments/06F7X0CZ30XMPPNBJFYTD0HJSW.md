[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q93R4633D41Z21WQW3SVGR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q93R4633D41Z21WQW3SVGR`.
- Optimistic claim succeeded (`expectedRevision=06F7WTE78TQKGYPWG8XHNM75S0`, `currentRevision=06F7WYZWVRMYB6T8FW944C3A70`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q93R4633D41Z21WQW3SVGR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q93R4633D41Z21WQW3SVGR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q93R4633D41Z21WQW3SVGR-epic-tracing-and-performance-guidance' from source 'b1d16f7cd4398edb99a8e44f3c78fb318ab7edcc'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q93R4633D41Z21WQW3SVGR-epic-tracing-and-performance-guidance` as `75d0d77f877b`.

Open questions / Risiken
- Low: if a child ticket is reopened after this cleanup, the epic should wait for that child again.
- Low: if another branch reintroduces the removed stale relation during integration, rerun relation eligibility before final closure.
- Split recommendation: No additional split recommended; the existing five-child decomposition remains bounded and matches the visible repository and ticket evidence.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `50816`
- effective-cache-ratio: `0.3578`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `3947e5afb7ad492fa6a4765097dd3768`
- completed-at-utc: `<redacted>-31T14:48:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q93R4633D41Z21WQW3SVGR/runs/20260531T144804620Z-3947e5afb7ad492fa6a4765097dd3768.json`