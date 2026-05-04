[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0N90QDR6X6XDMSK88X5NBR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0N90QDR6X6XDMSK88X5NBR`.
- Optimistic claim succeeded (`expectedRevision=06EZ0Y46BGG6CNVKB6902MQKXG`, `currentRevision=06EZ0YHYW9NKMRTZVXFD9FSNRC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EZ0N90QDR6X6XDMSK88X5NBR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0N90QDR6X6XDMSK88X5NBR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0N90QDR6X6XDMSK88X5NBR-task-document-provider-optimization-capability-m' from source '648b30138bc2a9d1f743c4e8949879f3a0d9cb65'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EZ0N90QDR6X6XDMSK88X5NBR-task-document-provider-optimization-capability-m` as `ab4d4a116b70`.

Open questions / Risiken
- If a non-SQLite provider strategy or new integration harness lands before this document ships, the matrix will need a quick recheck against the updated registrations and test categories.
- The current repository proves external database validation only for PostgreSQL; documenting broader external-validation expectations for SQL Server, Oracle, or MySQL would overpromise.
- Benchmark evidence is intentionally SQLite-only and machine-context-specific, so the matrix must not generalize those timings or coverage claims to other providers.
- Split recommendation: No split recommended; current evidence supports one bounded documentation ticket because the provider matrix can be derived from the existing provider packages, tests, and benchmark surfaces.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9240`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `c1c2d2dfc3f14a738e5cc0fdb80ff72f`
- completed-at-utc: `<redacted>-04T01:00:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0N90QDR6X6XDMSK88X5NBR/runs/20260504T010005493Z-c1c2d2dfc3f14a738e5cc0fdb80ff72f.json`