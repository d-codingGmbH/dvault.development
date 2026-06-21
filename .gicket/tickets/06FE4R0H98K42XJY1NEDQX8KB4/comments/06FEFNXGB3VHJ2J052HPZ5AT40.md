[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FE4R0H98K42XJY1NEDQX8KB4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4R0H98K42XJY1NEDQX8KB4`.
- Optimistic claim succeeded (`expectedRevision=06FE4R3GHW7HCRDMYA3DZVNV7G`, `currentRevision=06FEFKN2G03K71R9ZTBX9X66AM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FE4R0H98K42XJY1NEDQX8KB4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FE4R0H98K42XJY1NEDQX8KB4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FE4R0H98K42XJY1NEDQX8KB4-task-write-binary-hash-storage-migration-and-val' from source '4aaf27248f08842b4a00091256526823d23bdedf'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FE4R0H98K42XJY1NEDQX8KB4-task-write-binary-hash-storage-migration-and-val` as `136d29a00a15`.

Open questions / Risiken
- Current quantified footprint evidence is SQLite-only, so overly broad provider performance or storage claims would create documentation drift.
- If downstream implementation tickets change the exact support-bundle or validation surface names, this guide will need a final terminology pass before release.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `29345`
- cached-tokens: `8064`
- effective-cache-ratio: `0.2748`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `b26c83a606bf4cfca4cf32deed94bd98`
- completed-at-utc: `<redacted>-21T01:42:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4R0H98K42XJY1NEDQX8KB4/runs/20260621T014212819Z-b26c83a606bf4cfca4cf32deed94bd98.json`