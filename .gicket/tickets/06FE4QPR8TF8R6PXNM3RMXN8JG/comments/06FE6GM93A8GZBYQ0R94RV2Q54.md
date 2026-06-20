[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FE4QPR8TF8R6PXNM3RMXN8JG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4QPR8TF8R6PXNM3RMXN8JG`.
- Optimistic claim succeeded (`expectedRevision=06FE4QT7E75MAQQ7K0BQBSNKVR`, `currentRevision=06FE6CTKT2271XJ2QE0BKXBXY4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FE4QPR8TF8R6PXNM3RMXN8JG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FE4QPR8TF8R6PXNM3RMXN8JG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FE4QPR8TF8R6PXNM3RMXN8JG-task-tune-postgresql-latest-satellite-strategy-w' from source '5ffff3ea7e8c512e6edcb2795e10cf594a4c8eb9'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FE4QPR8TF8R6PXNM3RMXN8JG-task-tune-postgresql-latest-satellite-strategy-w` as `e60d244b7a30`.

Open questions / Risiken
- Because the root PostgreSQL latest-satellite row is currently a skipped placeholder, the team could overstate strategy registration as measured timing unless a provider-configured completed artifact is preserved.
- Changing the SQL shape without preserving parity and fallback tests could drift latest-row semantics or diagnostics behavior.
- The strongest checked-in PostgreSQL latest-index numbers currently describe save-side lookup and index experiments, so mixing them into read-strategy claims would blur the evidence contract.
- Split recommendation: No additional PO split is needed; provider-specific latest-satellite tuning is already isolated to this ticket and broader documentation follow-through already exists in 06FE4QRMXVGJVA65ZR5MZ817K8.
- Split recommendation: If relation normalization is wanted later, handle it as separate housekeeping rather than widening this tuning ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9733`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `3cb98bef54e54b678b2fc51978dbed5b`
- completed-at-utc: `<redacted>-20T04:20:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4QPR8TF8R6PXNM3RMXN8JG/runs/20260620T042049042Z-3cb98bef54e54b678b2fc51978dbed5b.json`