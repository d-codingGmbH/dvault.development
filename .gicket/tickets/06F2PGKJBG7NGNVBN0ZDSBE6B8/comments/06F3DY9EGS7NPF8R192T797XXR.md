[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGKJBG7NGNVBN0ZDSBE6B8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGKJBG7NGNVBN0ZDSBE6B8`.
- Optimistic claim succeeded (`expectedRevision=06F2PNKCJTGFVQ10PN6CWW1NWG`, `currentRevision=06F3DWGFDCXEASVE4J96AQC2MW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGKJBG7NGNVBN0ZDSBE6B8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGKJBG7NGNVBN0ZDSBE6B8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGKJBG7NGNVBN0ZDSBE6B8-task-test-link-parent-satellite-metadata-project' from source '31e409f13540ae1f74b9e0f28198affdd6d459a8'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGKJBG7NGNVBN0ZDSBE6B8-task-test-link-parent-satellite-metadata-project` as `fe2b8ab9d3ab`.

Open questions / Risiken
- Covering only one projection layer could miss a parallel regression if both metadata-model translation and EF-model projection currently maintain their own assertions; add the minimum adjacent coverage needed to close that gap.
- Existing test helpers may encode hub-parent assumptions, so a small amount of test-only helper reshaping may be needed to express a link-parent satellite case without changing product code.
- Split recommendation: No split recommended; the repository already has bounded unit test surfaces for this contract, so the work remains a single focused testing ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `38365`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0634`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `d62eccf7b9fd4969a5260bc6cd6c61a5`
- completed-at-utc: `<redacted>-17T17:26:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGKJBG7NGNVBN0ZDSBE6B8/runs/20260517T172600203Z-d62eccf7b9fd4969a5260bc6cd6c61a5.json`