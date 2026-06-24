[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF43BPP5NRJR3JTY48ZNEKHM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43BPP5NRJR3JTY48ZNEKHM`.
- Optimistic claim succeeded (`expectedRevision=06FFMCNKWE2EP9THKKJJH3PPYW`, `currentRevision=06FFPRWMBZ0FST2N1C0YEVXYP0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF43BPP5NRJR3JTY48ZNEKHM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF43BPP5NRJR3JTY48ZNEKHM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF43BPP5NRJR3JTY48ZNEKHM-task-normalize-provider-neutral-pit-maintenance' from source '5c3542454b74ed67f21734634fa10635a4e67af9'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF43BPP5NRJR3JTY48ZNEKHM-task-normalize-provider-neutral-pit-maintenance` as `183159ab6f4a`.

Open questions / Risiken
- If comparator rows drift into provider-specific prose or inconsistent tokens, downstream evidence consumers will need brittle special-case parsing.
- If PIT read or bridge read rows are cited as maintenance evidence, the repository will violate its documented maintenance-evidence boundary.
- PostgreSQL and SQL Server use different maintenance seams, so normalization must preserve a shared artifact contract without hiding bounded fallback-cause meaning.
- Split recommendation: No split recommended; the repository baseline supports one bounded implementation ticket for PostgreSQL and SQL Server PIT maintenance comparator-row normalization and coverage.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9013`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `fbebd82857ed4240bf0d8a7fb882c936`
- completed-at-utc: `<redacted>-24T20:54:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43BPP5NRJR3JTY48ZNEKHM/runs/20260624T205429599Z-fbebd82857ed4240bf0d8a7fb882c936.json`