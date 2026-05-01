[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB7HEJY18HEB5A5MVTN5KZC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7HEJY18HEB5A5MVTN5KZC`.
- Optimistic claim succeeded (`expectedRevision=06EY20GJ01DDC5YGPFZCKCSQZC`, `currentRevision=06EY20KM20MP9VFNJG3HV5PQ3R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7HEJY18HEB5A5MVTN5KZC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7HEJY18HEB5A5MVTN5KZC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7HEJY18HEB5A5MVTN5KZC-task-persist-hubs-and-links-idempotently' from source '9a2375e7c0723721b8c5bbc0d2929deb46dde69d'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB7HEJY18HEB5A5MVTN5KZC-task-persist-hubs-and-links-idempotently` as `8360c0b3581e`.

Open questions / Risiken
- If the reuse lookup does not exactly match the current stable-hash normalization and field ordering, repeated writes may still miss existing rows and hit primary-key failures.
- Because the current implementation reports raw SaveChangesAsync row count, the developer must separate insert counting from requested operation counting to keep RowsWritten correct for reused rows.
- Because current link metadata only shows a non-unique participant relationship index, assuming a participant-combination uniqueness constraint would overstate what the branch actually provides.
- Tests limited to one DbContext could miss regressions in persisted lineage preservation or duplicate detection across separate invocations.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7900`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `75c66b1d172147d7af34982b087667c7`
- completed-at-utc: `<redacted>-01T00:53:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7HEJY18HEB5A5MVTN5KZC/runs/20260501T005327998Z-75c66b1d172147d7af34982b087667c7.json`