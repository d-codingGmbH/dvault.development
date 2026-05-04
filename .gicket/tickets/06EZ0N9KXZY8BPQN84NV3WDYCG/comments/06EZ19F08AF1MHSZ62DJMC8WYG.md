[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0N9KXZY8BPQN84NV3WDYCG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0N9KXZY8BPQN84NV3WDYCG`.
- Optimistic claim succeeded (`expectedRevision=06EZ0Y34XRH00D4DVDAE8ED5MG`, `currentRevision=06EZ17HNP4VE8GVSJEFGD41X0W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EZ0N9KXZY8BPQN84NV3WDYCG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0N9KXZY8BPQN84NV3WDYCG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0N9KXZY8BPQN84NV3WDYCG-task-add-shared-provider-sql-execution-contract' from source '78670a1b304e152a9a7c1cd84385e6bdf2b3a6ee'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If implementation drifts back to asserting SQLite SQL strings instead of provider-neutral command behavior, the shared harness will not be reusable across providers.
- Because SQLite is the only current optimized executor, some shared-contract gaps may stay hidden until a second provider adopts the harness.
- Future providers may require execution capabilities beyond the current insert-only non-query path, which could force a later contract expansion and test refactor.
- Split recommendation: No split recommended; the parent story already separates documentation matrix work, service-level strategy-selection tests, and this lower-level SQL execution contract coverage into bounded tickets.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9549`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `6d91b307ec6248359fbd587e916a0bb7`
- completed-at-utc: `<redacted>-04T01:41:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0N9KXZY8BPQN84NV3WDYCG/runs/20260504T014135236Z-6d91b307ec6248359fbd587e916a0bb7.json`