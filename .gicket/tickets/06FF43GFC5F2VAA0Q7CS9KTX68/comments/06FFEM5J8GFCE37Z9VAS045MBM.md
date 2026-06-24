[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF43GFC5F2VAA0Q7CS9KTX68'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43GFC5F2VAA0Q7CS9KTX68`.
- Optimistic claim succeeded (`expectedRevision=06FF44K7GBSJ5J3CNCFR435R4M`, `currentRevision=06FFEJRN0KJGG50YZQSCQ0E6KR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF43GFC5F2VAA0Q7CS9KTX68': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF43GFC5F2VAA0Q7CS9KTX68': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF43GFC5F2VAA0Q7CS9KTX68-task-review-postgresql-pit-maintenance-transacti' from source '0d04fe695f7f875adb667cabefdcaabe485e9ce1'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF43GFC5F2VAA0Q7CS9KTX68-task-review-postgresql-pit-maintenance-transacti` as `1814d9dcfb12`.

Open questions / Risiken
- If the implementation changes the PostgreSQL gate to decline some ambient-transaction scenarios, callers may see more provider-neutral fallback than before; diagnostics and docs must make that behavior intentional and visible.
- If savepoint-backed support is chosen, the repository needs live Npgsql integration evidence for fault and cancellation behavior; otherwise the docs could still overstate safety.
- Transaction-boundary changes must not regress the already-proven PostgreSQL supported-shape rebuild coverage for ordinary, shared-driving-key multi-active, or link-parent non-multi-active PIT full rebuilds.
- Split recommendation: No split recommended; the transaction-boundary decision, diagnostics, tests, and documentation updates are one bounded refinement and should stay in a single ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9085`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `b8f5e2c855174a4ebcc51a0ad51da562`
- completed-at-utc: `<redacted>-24T01:48:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43GFC5F2VAA0Q7CS9KTX68/runs/20260624T014841406Z-b8f5e2c855174a4ebcc51a0ad51da562.json`