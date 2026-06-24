[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF43AH9SK6J07GV5EKYV3AMM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43AH9SK6J07GV5EKYV3AMM`.
- Optimistic claim succeeded (`expectedRevision=06FF44HRNVET3Z2QMT5MXKZX6M`, `currentRevision=06FFJP3TNRPRC7KNEXD5WJTPRM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF43AH9SK6J07GV5EKYV3AMM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF43AH9SK6J07GV5EKYV3AMM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF43AH9SK6J07GV5EKYV3AMM-task-add-postgresql-pit-full-rebuild-benchmark-l' from source '77760f995590bbe7e1064abbc96ad22e74c5cd29'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF43AH9SK6J07GV5EKYV3AMM-task-add-postgresql-pit-full-rebuild-benchmark-l` as `a5d7613f2a52`.

Open questions / Risiken
- Completed timing evidence still depends on an opt-in PostgreSQL environment; absent or unreachable PostgreSQL must yield skipped-placeholder rows rather than omitted rows.
- If implementation blends caller-transaction fallback cases or unsupported PIT shapes into optimized timing claims, it will violate the current PIT maintenance evidence boundary.
- Because PostgreSQL supports three approved full-rebuild shapes, row metadata must stay deterministic enough for later evidence citations to distinguish the exercised shape from general provider availability.
- Split recommendation: Keep SQL Server PIT maintenance timing as a sibling ticket because it uses a different runtime seam (`SqlServerDataVaultPitMaintenanceService`) and fallback vocabulary from PostgreSQL.
- Split recommendation: Any future MySQL, Oracle, or DB2 PIT maintenance benchmarking should remain separate from this ticket until their runtime maintenance lanes are implemented or explicitly accepted beyond the current repository boundary.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8583`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `42de111f795a477bae291516794abd0f`
- completed-at-utc: `<redacted>-24T11:25:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43AH9SK6J07GV5EKYV3AMM/runs/20260624T112529712Z-42de111f795a477bae291516794abd0f.json`