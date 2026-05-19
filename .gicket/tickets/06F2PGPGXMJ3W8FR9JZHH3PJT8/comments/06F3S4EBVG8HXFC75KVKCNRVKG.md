[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGPGXMJ3W8FR9JZHH3PJT8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGPGXMJ3W8FR9JZHH3PJT8`.
- Optimistic claim succeeded (`expectedRevision=06F2PNMN7SH96FDXTHHW2SA3MW`, `currentRevision=06F3S1TE117B4NTR72MDXK82M8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGPGXMJ3W8FR9JZHH3PJT8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGPGXMJ3W8FR9JZHH3PJT8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service' from source '334e93b5f83487eccbc11a03d9f41bea9fcca937'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service` as `ded9c98de666`.

Open questions / Risiken
- Hierarchy bridge maintenance can expand quickly on large recursive link sets; this story should stay correct and provider-neutral first, with any performance specialization treated as later work.
- Because current bridge tables do not carry effectivity, path payload, or closure-state columns, the implementation must avoid silently inventing advanced semantics that are not represented in the shipped metadata baseline.
- The story blocks downstream query-API and documentation tickets, so under-specifying idempotence or hierarchy-depth semantics would create avoidable rework in those follow-ons.
- Split recommendation: No split recommended; sibling tickets already isolate PIT maintenance, query API work, provider-aware optimization, and v0.15.0 documentation.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9512`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `7dee3eee1633486ba5de7746198f668b`
- completed-at-utc: `<redacted>-18T19:30:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGPGXMJ3W8FR9JZHH3PJT8/runs/20260518T193048247Z-7dee3eee1633486ba5de7746198f668b.json`