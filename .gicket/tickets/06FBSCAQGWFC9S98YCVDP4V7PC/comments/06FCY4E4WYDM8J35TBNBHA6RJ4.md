[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSCAQGWFC9S98YCVDP4V7PC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCAQGWFC9S98YCVDP4V7PC`.
- Optimistic claim succeeded (`expectedRevision=06FCY28JSQCEM8P2RHHXRAV7DW`, `currentRevision=06FCY2EYADJPKMMF5NFDKSJV8C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSCAQGWFC9S98YCVDP4V7PC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSCAQGWFC9S98YCVDP4V7PC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSCAQGWFC9S98YCVDP4V7PC-task-implement-accepted-db2-bulk-improvement' from source 'b442992daa7ac1c6e5b6a40cad7080430d4f23bc'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSCAQGWFC9S98YCVDP4V7PC-task-implement-accepted-db2-bulk-improvement` as `0a1879b4f39b`.

Open questions / Risiken
- Until runtime closes or relabels the ticket, the historical open-work title can still imply unfinished DB2 bulk implementation.
- No live DB2 timing claim exists in the repository today because the checked-in benchmark rows are skipped placeholders when DVAULT_TEST_DB2_CONNECTION_STRING is unset.
- Relation/history surfaces were trust-blocked in this run, so the audit trail is contract-text-only rather than a persisted ticket relation.
- Split recommendation: Do not split this ticket; closure is the correct routing.
- Split recommendation: If more DB2 evidence is desired later, create one separate evidence/documentation ticket scoped to provider-configured benchmark capture or documentation updates only.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8903`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `760a86a7a485415bb16d8e1fc91b75a7`
- completed-at-utc: `<redacted>-16T06:15:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCAQGWFC9S98YCVDP4V7PC/runs/20260616T061508638Z-760a86a7a485415bb16d8e1fc91b75a7.json`