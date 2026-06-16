[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSC9DCB0S58DYFY3TAEZ848'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC9DCB0S58DYFY3TAEZ848`.
- Optimistic claim succeeded (`expectedRevision=06FBSCYV2Z29STNYDRTQSF1700`, `currentRevision=06FCW0TGWZ8BDXH50ED0QME51R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSC9DCB0S58DYFY3TAEZ848': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSC9DCB0S58DYFY3TAEZ848': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSC9DCB0S58DYFY3TAEZ848-task-evaluate-postgresql-bulk-strategy-gaps' from source 'ccfba2329b156e488bb5505ea05c10cacb3963cd'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSC9DCB0S58DYFY3TAEZ848-task-evaluate-postgresql-bulk-strategy-gaps` as `12dc84b9edcd`.

Open questions / Risiken
- Changing the threshold now without a new provider-configured benchmark triplet would replace a repository-backed boundary with guesswork.
- The v0.39 root benchmark triplet still shows PostgreSQL provider rows as skipped placeholders, so a future developer could misread the root summary unless this ticket explicitly points them to the v0.32 completed evidence bundle.
- Relation state could not be re-verified through the blocked local gicket transport during this run; if downstream workflow depends on exact ticket links, a later tool-enabled pass should confirm them.
- Split recommendation: No split recommended. If follow-up work is opened later, make it a dedicated provider-configured PostgreSQL evidence-collection ticket rather than mixing new benchmarks into this evaluation ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8883`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `1dd2d5c4e15f4e8aaff851e40c3bac94`
- completed-at-utc: `<redacted>-16T01:33:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC9DCB0S58DYFY3TAEZ848/runs/20260616T013319567Z-1dd2d5c4e15f4e8aaff851e40c3bac94.json`