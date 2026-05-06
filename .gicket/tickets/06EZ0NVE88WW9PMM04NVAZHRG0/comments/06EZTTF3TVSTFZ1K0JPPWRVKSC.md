[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NVE88WW9PMM04NVAZHRG0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NVE88WW9PMM04NVAZHRG0`.
- Optimistic claim succeeded (`expectedRevision=06EZTRS743NZ6ZEEBBP5EW73KC`, `currentRevision=06EZTS6JR4X3M5W8JEQB4HVT6G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06EZ0NVE88WW9PMM04NVAZHRG0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NVE88WW9PMM04NVAZHRG0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NVE88WW9PMM04NVAZHRG0-task-add-bridge-documentation-and-example-scenar' from source '4d32cde32372a28c7e75865b4aa0c2e7578b29db'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EZ0NVE88WW9PMM04NVAZHRG0-task-add-bridge-documentation-and-example-scenar` as `e8459f19a26b`.

Open questions / Risiken
- A later bridge implementation ticket may introduce concrete naming or runtime semantics that require a small terminology sync in the docs.
- The docs could drift into speculative API design unless they stay anchored to the current source-backed deferred baseline.
- Split recommendation: No split is required for the current bounded docs-only task.
- Split recommendation: If later work needs hierarchy-specific walkthroughs, runnable samples, or docs tied to implemented bridge APIs, create separate follow-up docs tickets.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `52777`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0461`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `ce96262908714176b41d3ffa1d79de98`
- completed-at-utc: `<redacted>-06T13:11:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NVE88WW9PMM04NVAZHRG0/runs/20260506T131107776Z-ce96262908714176b41d3ffa1d79de98.json`