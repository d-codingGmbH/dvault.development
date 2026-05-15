[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGG8ZKSYGC8863118H56G8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGG8ZKSYGC8863118H56G8`.
- Optimistic claim succeeded (`expectedRevision=06F2QEVM84EN08BY03FXWCZAJ8`, `currentRevision=06F2QEZM3WABWMBXTW8X78BE8G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGG8ZKSYGC8863118H56G8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGG8ZKSYGC8863118H56G8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGG8ZKSYGC8863118H56G8-task-implement-provider-catalog-readers' from source 'd5618b1fd10a5a0ac6ce7c5ac83ead875ba1e054'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGG8ZKSYGC8863118H56G8-task-implement-provider-catalog-readers` as `146cc8c0aabf`.

Open questions / Risiken
- Provider catalogs differ in schema scoping, identifier casing, index metadata shape, and storage-type text, so normalization mistakes can create false drift or hide real mismatches.
- Oracle physical-name limits and MySQL dual provider-name support make identifier mapping and dispatch handling more brittle than the existing SQLite path.
- External opt-in coverage depends on configured provider packages and developer-managed databases, so some defects can remain latent when those lanes are not exercised.
- If the ticket is resent to PO-critic without non-ticket src/ and tests/ evidence, it will fail on the same repository-gap findings.
- Split recommendation: Keep this as one bounded implementation ticket for PostgreSQL, SQL Server, Oracle, and MySQL live-schema readers.
- Split recommendation: If product later narrows the release back to SQLite-only, create a separate implementation ticket for first-class PostgreSQL, SQL Server, Oracle, and MySQL readers and update relations at that time.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.5037`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `dbad9fb30c844d0ab19a6700032120bd`
- completed-at-utc: `<redacted>-15T13:08:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGG8ZKSYGC8863118H56G8/runs/20260515T130830434Z-dbad9fb30c844d0ab19a6700032120bd.json`