[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB75XTWD7FTRAFE5GNDCS5R'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB75XTWD7FTRAFE5GNDCS5R`.
- Optimistic claim succeeded (`expectedRevision=06EXBQKNN5JT29H39682KMAKXR`, `currentRevision=06EXCZ10V9NN9AMSHHDAJ5VSJ0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB75XTWD7FTRAFE5GNDCS5R': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB75XTWD7FTRAFE5GNDCS5R': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB75XTWD7FTRAFE5GNDCS5R-task-provide-override-points-for-naming-policies' from source '5418946a77a417a5511a0f2c55f635e0ccacb8f9'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB75XTWD7FTRAFE5GNDCS5R-task-provide-override-points-for-naming-policies` as `0fb7c02fe830`.

Open questions / Risiken
- Public API shape may be hard to change later, so the initial naming policy contract should stay minimal while still covering the six v1 name families.
- If this ticket and sibling 06EXB75NX7Z0DY7X0BD0YFZECM are developed independently, teams must coordinate to avoid conflicting default naming semantics.
- Split recommendation: No split recommended for this ticket; interface, options hook, default plumbing, and tests remain one cohesive implementation slice. The sibling default-policy ticket is the separate boundary for detailed default naming rules.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `36511`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0666`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `48989c43facd49d0bf7208acbd8660dd`
- completed-at-utc: `<redacted>-28T23:47:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB75XTWD7FTRAFE5GNDCS5R/runs/20260428T234725101Z-48989c43facd49d0bf7208acbd8660dd.json`