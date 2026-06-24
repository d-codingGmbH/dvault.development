[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF43FQ8NRX04T9HZHBMFS0PC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43FQ8NRX04T9HZHBMFS0PC`.
- Optimistic claim succeeded (`expectedRevision=06FF44K140AVCEG0AB901KP138`, `currentRevision=06FFEAB3SZX66KVTNHHNBFG4W0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF43FQ8NRX04T9HZHBMFS0PC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF43FQ8NRX04T9HZHBMFS0PC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF43FQ8NRX04T9HZHBMFS0PC-task-add-postgresql-pit-maintenance-selected-fal' from source 'a3f822b484b767d5d50dc52e869b763ab88c64d5'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF43FQ8NRX04T9HZHBMFS0PC-task-add-postgresql-pit-maintenance-selected-fal` as `f1249973cfb8`.

Open questions / Risiken
- DefaultDataVaultPitMaintenanceService has no existing save/read-style selector object, so ad hoc fallback capture could drift from the repository's established finite diagnostics pattern unless it explicitly reuses the gate evaluator.
- The current activity-tracing documentation still treats maintenance fallback causes as effectively undocumented, so code landing before docs could create a temporary source-versus-doc mismatch.
- Sibling transaction-review or benchmark tickets may later narrow or expand PostgreSQL maintenance eligibility; this ticket should keep the observability vocabulary stable across those later changes.
- Split recommendation: Do not split further now; transaction review, benchmark lane, comparator/evidence-matrix work, and documentation already exist as bounded sibling tickets.
- Split recommendation: Only create a new follow-up if implementation proves the existing maintenance Activity surface cannot carry the required bounded facts cleanly; keep any such follow-up limited to a dedicated maintenance diagnostics surface.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9411`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `bc3a81412be94c5888d0403bba1f044e`
- completed-at-utc: `<redacted>-24T01:15:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43FQ8NRX04T9HZHBMFS0PC/runs/20260624T011526022Z-bc3a81412be94c5888d0403bba1f044e.json`