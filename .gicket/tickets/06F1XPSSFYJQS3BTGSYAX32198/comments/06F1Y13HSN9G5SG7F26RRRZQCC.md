[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XPSSFYJQS3BTGSYAX32198'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F1XPSSFYJQS3BTGSYAX32198`.
- Optimistic claim succeeded (`expectedRevision=06F1XZA4503PMV9DE2TGZXEGSC`, `currentRevision=06F1XZJYPE9GKC30R7EMKDJK8R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XPSSFYJQS3BTGSYAX32198': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XPSSFYJQS3BTGSYAX32198': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XPSSFYJQS3BTGSYAX32198-task-implement-diagnostic-catalog-infrastructure' from source '9774789d59f41451f18c7212c3e56d87adab152f'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F1XPSSFYJQS3BTGSYAX32198-task-implement-diagnostic-catalog-infrastructure` as `a56a66269860`.

Open questions / Risiken
- Centralizing 18 existing emitters may touch multiple call sites, so regression coverage must protect stable observed behavior.
- If later tickets extend the catalog without preserving the same per-entry documentation tests, diagnostic documentation quality could drift.
- Split recommendation: No split is recommended; the 18-code importer/projection seed set is already the smallest coherent first slice visible in the repository.
- Split recommendation: If future migration expands beyond this seed set, create separate follow-up tickets by diagnostic family instead of enlarging this ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `49184`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0494`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `af0c35a4a37446aa97c01f5e834ad02b`
- completed-at-utc: `<redacted>-13T01:47:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F1XPSSFYJQS3BTGSYAX32198/runs/20260513T014724760Z-af0c35a4a37446aa97c01f5e834ad02b.json`