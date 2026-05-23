[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F492C50WM7V2NE0WZB3774XM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492C50WM7V2NE0WZB3774XM`.
- Optimistic claim succeeded (`expectedRevision=06F54BQDXGV01NA6FAVY1J7QR8`, `currentRevision=06F54GK7JC052TWJGH0TPZGJYG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F492C50WM7V2NE0WZB3774XM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F492C50WM7V2NE0WZB3774XM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an' from source '3345664cf4e77f0f4fc0ff629cc798d3c00a79ae'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an` as `c4336707a7d4`.

Open questions / Risiken
- The persisted ticket description still contains stale closure-only language because no gicket-update-ticket-description write was materialized in this pass.
- If a later evidence pass reveals deeper in-branch read-shape implementation than the visible snapshot exposed, developers should reconcile that code with this explicit contract rather than reintroducing undocumented shipped-baseline assumptions.
- Split recommendation: No split is required from current visible evidence; keep this as one bounded diagnostics story unless future work expands into database-advisor behavior or provider-specific tuning recommendations.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `28310`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0859`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `30d650b12120490ca3e826f44a9f73ff`
- completed-at-utc: `<redacted>-23T00:41:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492C50WM7V2NE0WZB3774XM/runs/20260523T004122090Z-30d650b12120490ca3e826f44a9f73ff.json`