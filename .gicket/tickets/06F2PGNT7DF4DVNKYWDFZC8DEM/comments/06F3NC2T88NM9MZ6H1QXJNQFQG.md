[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGNT7DF4DVNKYWDFZC8DEM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGNT7DF4DVNKYWDFZC8DEM`.
- Optimistic claim succeeded (`expectedRevision=06F2PNM5H9DJGPZMZ72RBSR2YC`, `currentRevision=06F3N9V0FC1137Y71H66PYFSGC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGNT7DF4DVNKYWDFZC8DEM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGNT7DF4DVNKYWDFZC8DEM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGNT7DF4DVNKYWDFZC8DEM-task-add-provider-bulk-integration-coverage' from source '6113275e17723832f8b2dcb6c64e9b115ffb4093'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGNT7DF4DVNKYWDFZC8DEM-task-add-provider-bulk-integration-coverage` as `b1d73079fd4f`.

Open questions / Risiken
- External-provider tests depend on developer-managed databases, privileges to create and drop temporary schemas or tables, and conditional provider restore markers; weak environment isolation can produce flaky evidence.
- If the live tests use undersized batches or dirty DbContexts, they can accidentally prove only fallback behavior and miss the intended provider-native bulk path.
- Oracle documentation in the repository still contains older hub and link only wording from the v0.5 architecture note, so the new live coverage could diverge from published claims unless the docs ticket reconciles them.
- MySQL coverage spans supported provider names through a reflection helper; bypassing that helper can make the live lane prove the wrong provider combination.
- Split recommendation: No additional split is recommended; the existing graph already separates provider-neutral fallback 06F2PGN4GPQCGC5WHZQBGP4SD0, provider-native strategy implementation story 06F2PGNGVQ3TZZWSABAK5SNFK4, this provider integration task, benchmark story 06F2PG...
- Split recommendation: If later work needs live-provider proof of provider-decline fallback behavior or multi-active satellite rejection, create a follow-on task instead of widening this ticket.
- Split recommendation: If documentation work grows beyond narrow execution-guidance updates, keep it on 06F2PGP2B2RZGGK3CVKK5WRRP8 instead of opening another planning split.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9395`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `c5732c2b6c174a969cbafc4bc9043b22`
- completed-at-utc: `<redacted>-18T10:44:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGNT7DF4DVNKYWDFZC8DEM/runs/20260518T104456294Z-c5732c2b6c174a969cbafc4bc9043b22.json`