[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NADTKZP9J1YCVNMDH60WC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NADTKZP9J1YCVNMDH60WC`.
- Optimistic claim succeeded (`expectedRevision=06EZ0Y3XDRJCPMWNNRXQR0BN58`, `currentRevision=06EZAS91MMFSNEC8WQMYSDE9JW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EZ0NADTKZP9J1YCVNMDH60WC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NADTKZP9J1YCVNMDH60WC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NADTKZP9J1YCVNMDH60WC-story-optimize-sql-server-provider-save-strategy' from source 'b00fdcbf375ac794c8b9ba525c256a74833c5144'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EZ0NADTKZP9J1YCVNMDH60WC-story-optimize-sql-server-provider-save-strategy` as `e13cffa8e549`.

Open questions / Risiken
- Live SQL Server smoke coverage depends on a developer-managed database and conditional restore of the SQL Server EF Core provider package, so environment drift can block evidence collection even when the default smoke baseline stays green.
- Because the optimized path uses raw SQL batching rather than tracked EF inserts, regressions can hide in identifier quoting, schema resolution, or parameter-count chunking unless the default smoke and opt-in live coverage stay aligned.
- Ticket relations show existing incoming `blocks` edges from 06EZ0N8HW9PZAFKMM5WQD564VR and 06EZ0N9AM9AJ3AB8DQ6Y1JBS28; delivery sequencing should still respect those external dependencies.
- Split recommendation: No additional split is recommended in this PO pass; the story already has materialized `parentOf` children 06EZ0NAMGKJ63WCXAK1J7B08TR and 06EZ0NAWNDDEP32P497E39MQXR.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8502`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `1e841695fb594ac2b01fc7ce56d18cb6`
- completed-at-utc: `<redacted>-04T23:54:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NADTKZP9J1YCVNMDH60WC/runs/20260504T235423038Z-1e841695fb594ac2b01fc7ce56d18cb6.json`