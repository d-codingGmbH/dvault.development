[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FGX6CRPG02ZWGE62QWSG42EC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX6CRPG02ZWGE62QWSG42EC`.
- Optimistic claim succeeded (`expectedRevision=06FGX6SSPK0HHQWS4NMJR0MAZ4`, `currentRevision=06FH5GKF40VAKXD8ZWQZMFENGC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FGX6CRPG02ZWGE62QWSG42EC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FGX6CRPG02ZWGE62QWSG42EC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FGX6CRPG02ZWGE62QWSG42EC-task-document-binary-migration-manifest-validati' from source '0e48c6cc17fdbef7d3c8c0405506b455b87921c4'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FGX6CRPG02ZWGE62QWSG42EC-task-document-binary-migration-manifest-validati` as `78c499a3ada3`.

Open questions / Risiken
- If the docs update touches root `README.md`, packaged README verification can fail unless the existing package-verifier assertions are still satisfied or updated intentionally.
- If the update only amends the migration guide and skips current release notes, the public documentation baseline will remain inconsistent even though the underlying exporter and validator flow already exist.
- Split recommendation: No split recommended; the visible repository scope is bounded to documentation alignment around an already implemented export and validation flow.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7427`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `042812ab94274a498748f0d3bfcd2061`
- completed-at-utc: `<redacted>-29T09:50:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX6CRPG02ZWGE62QWSG42EC/runs/20260629T095055636Z-042812ab94274a498748f0d3bfcd2061.json`