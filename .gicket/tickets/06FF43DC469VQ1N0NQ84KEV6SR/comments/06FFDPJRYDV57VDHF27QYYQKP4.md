[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF43DC469VQ1N0NQ84KEV6SR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43DC469VQ1N0NQ84KEV6SR`.
- Optimistic claim succeeded (`expectedRevision=06FF44H5RTK07SDN6N95B0442M`, `currentRevision=06FFDMYTBGFKNJMY007M532500`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF43DC469VQ1N0NQ84KEV6SR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF43DC469VQ1N0NQ84KEV6SR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF43DC469VQ1N0NQ84KEV6SR-task-evaluate-oracle-pit-full-rebuild-push-down' from source '4a71f93a33e03271deb83fac3d3e61b2a502c486'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF43DC469VQ1N0NQ84KEV6SR-task-evaluate-oracle-pit-full-rebuild-push-down` as `8f6e3640178d`.

Open questions / Risiken
- Oracle may not offer rollback-clean full-rebuild behavior through the same EF Core transaction/savepoint surfaces relied on by the current SQL Server safeguard, which raises partial PIT refresh risk.
- The PostgreSQL rebuild path depends on SQL patterns such as `WITH`, `UNION`, and lateral snapshot selection; Oracle may require materially different SQL that expands the proof surface.
- Existing Oracle PIT read evidence can be misread as maintenance evidence, creating scope pressure to ship a provider push-down path without equivalent rebuild-specific proof.
- Split recommendation: No split is needed during refinement; only create a follow-up implementation ticket if the evaluation produces a clearly bounded Oracle full-rebuild candidate.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8151`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `359e3736668846bca4d5218a6ee9a92d`
- completed-at-utc: `<redacted>-23T23:39:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43DC469VQ1N0NQ84KEV6SR/runs/20260623T233925290Z-359e3736668846bca4d5218a6ee9a92d.json`