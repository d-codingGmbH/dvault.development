[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB7J6HCA9QZ3DPP5Z03YGJ0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7J6HCA9QZ3DPP5Z03YGJ0`.
- Optimistic claim succeeded (`expectedRevision=06EXNNPT8BY36ZRYZ7EG9HTVFM`, `currentRevision=06EY0PKN014VPBD56NM659C67M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7J6HCA9QZ3DPP5Z03YGJ0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7J6HCA9QZ3DPP5Z03YGJ0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7J6HCA9QZ3DPP5Z03YGJ0-task-define-provider-capability-abstraction' from source '2fbf17e1aca9323a91fa0d2d0ab19f23fee0a648'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB7J6HCA9QZ3DPP5Z03YGJ0-task-define-provider-capability-abstraction` as `8fdfabffc427`.

Open questions / Risiken
- If the abstraction tries to model every possible provider feature now, it will become a speculative provider matrix and slow the downstream tickets this work is supposed to unblock.
- If provider-neutral logical contracts leak provider-native terms into the core API, later non-Sqlite adapters may inherit avoidable coupling.
- If no real consumer path is wired to the abstraction in this ticket, the result may remain a dormant contract that does not actually eliminate scattered provider checks.
- Split recommendation: No split recommended: current evidence keeps the work bounded to one abstraction plus one Sqlite profile, which is sufficient to support the downstream tickets already blocked by this item.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.5233`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `4dc0dedd89fb4d149f63f94c4a0eec4c`
- completed-at-utc: `<redacted>-30T21:49:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7J6HCA9QZ3DPP5Z03YGJ0/runs/20260430T214926240Z-4dc0dedd89fb4d149f63f94c4a0eec4c.json`