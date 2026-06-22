[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FE4RJZ4PA0DZ3HXDSEG2BQMM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4RJZ4PA0DZ3HXDSEG2BQMM`.
- Optimistic claim succeeded (`expectedRevision=06FE4RME0GBEY8NCZ8808G8B3R`, `currentRevision=06FF0RET7C7YZMGTBPSWBR3G7R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FE4RJZ4PA0DZ3HXDSEG2BQMM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FE4RJZ4PA0DZ3HXDSEG2BQMM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel' from source '6e5ca58879025b4632fba365c5c8cead8b0376c0'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel` as `ec5ec5ff55a6`.

Open questions / Risiken
- Multi-active and link-parent PIT semantics are materially more complex than ordinary hub-parent PITs; widening the SQL Server path too early risks semantic drift from the deterministic provider-neutral baseline.
- A raw SQL rebuild lane can interact poorly with pending tracked state; keeping a clean-context gate and explicit fallback is important to avoid surprising mixed persistence behavior.
- Without a separate benchmark artifact lane, this ticket must not be used to imply completed SQL Server PIT maintenance performance claims.
- Split recommendation: If the runtime prototype starts to absorb benchmark or documentation-promotion work, split provider-configured evidence collection into a separate ticket so this ticket stays on path selection, parity, and fallback behavior.
- Split recommendation: If shape support expands beyond ordinary hub-parent PITs, split multi-active and link-parent rebuild optimization into later bounded tickets instead of reopening this prototype.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9034`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `0bee84e1ee4f49c7b4bdbd002a0c2489`
- completed-at-utc: `<redacted>-22T17:38:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4RJZ4PA0DZ3HXDSEG2BQMM/runs/20260622T173825235Z-0bee84e1ee4f49c7b4bdbd002a0c2489.json`