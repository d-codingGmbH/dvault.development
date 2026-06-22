[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FE4RJZ4PA0DZ3HXDSEG2BQMM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4RJZ4PA0DZ3HXDSEG2BQMM`.
- Optimistic claim succeeded (`expectedRevision=06FF0ZDHV4TFFZ3Y63ZNZG1YYC`, `currentRevision=06FF167QH6C8ARRPSSPFN6E2R8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FE4RJZ4PA0DZ3HXDSEG2BQMM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FE4RJZ4PA0DZ3HXDSEG2BQMM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel' from source 'd94c6ecdd67a8011642c9699ffe8d07e757acff6'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel` as `6454812bd0c8`.

Open questions / Risiken
- Current provider-neutral fallback remains a delete-then-insert baseline; if the SQL Server gate declines, callers still rely on that older behavior until a separate ticket changes it.
- Deterministic rollback and cancellation verification on SQL Server may require a fault-injection seam or test interceptor because existing PIT maintenance tests currently cover tracing rather than persisted post-failure state.
- Widening the SQL Server candidate beyond ordinary hub-parent rebuilds before rollback and parity evidence are proven risks semantic drift and harder cleanup guarantees.
- Split recommendation: If provider-neutral PIT maintenance should also preserve pre-rebuild rows on failure or cancellation, split that baseline-hardening work into a separate ticket instead of broadening this SQL Server-only prototype.
- Split recommendation: If the prototype starts to absorb multi-active, link-parent, benchmark, or documentation-promotion work, split those into follow-up tickets so this ticket stays focused on candidate selection, parity, and rollback or cleanup behavior.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9235`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `5b9ed27477004f8ab9eca1e7a5213ba4`
- completed-at-utc: `<redacted>-22T18:38:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4RJZ4PA0DZ3HXDSEG2BQMM/runs/20260622T183810689Z-5b9ed27477004f8ab9eca1e7a5213ba4.json`