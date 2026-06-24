[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF438KMPKSBT6KXZ5DBY85QC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF438KMPKSBT6KXZ5DBY85QC`.
- Optimistic claim succeeded (`expectedRevision=06FF44GKKZQ5VZQ14C7G7SFYW0`, `currentRevision=06FFD453CCQD7MACKE8G3CT4C4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF438KMPKSBT6KXZ5DBY85QC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF438KMPKSBT6KXZ5DBY85QC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi' from source 'df162adef01ffd36699c2223365d93d0ce756cb0'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi` as `b8384b3661f5`.

Open questions / Risiken
- If maintenance scenario naming stays ambiguous in implementation, downstream docs could continue misreading `pit-as-of-read` rows as maintenance evidence.
- Until `06FF43AH9SK6J07GV5EKYV3AMM` and `06FF43AYQYZKFF400CK5Q84WYR` land preserved provider-configured artifacts, this ticket can define the contract but cannot itself satisfy provider timing claims.
- If `06FF43BPP5NRJR3JTY48ZNEKHM` does not normalize the provider-neutral comparator row, fallback-cause comparisons across providers may remain inconsistent.
- Split recommendation: No additional split is justified; the existing decomposition already separates the contract task from PostgreSQL lane, SQL Server lane, and provider-neutral comparator-row work.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8586`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `3cf3b80b8bb645e59f8ee400cf2f0541`
- completed-at-utc: `<redacted>-23T22:29:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF438KMPKSBT6KXZ5DBY85QC/runs/20260623T222914399Z-3cf3b80b8bb645e59f8ee400cf2f0541.json`