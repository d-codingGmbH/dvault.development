[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MED4P7HMBDZVMPWQZ5A7PC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MED4P7HMBDZVMPWQZ5A7PC`.
- Optimistic claim succeeded (`expectedRevision=06F0Z9GSGNW92VB3XP07TK7SXM`, `currentRevision=06F0Z9R2NVVQESVFS8MR7K8WNW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MED4P7HMBDZVMPWQZ5A7PC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MED4P7HMBDZVMPWQZ5A7PC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MED4P7HMBDZVMPWQZ5A7PC-task-implement-data-vault-model-validation-and-e' from source '459c364e3c5f5aec8edf47fcec96996d088dc74f'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F0MED4P7HMBDZVMPWQZ5A7PC-task-implement-data-vault-model-validation-and-e` as `87c955b39d55`.

Open questions / Risiken
- If fallback-reason reporting duplicates provider `CanSave` gates instead of sharing extracted helpers, diagnostics can drift from actual runtime dispatch behavior.
- The SQL Server/MySQL/Oracle threshold numbers are part of the current v0.5 behavior baseline; future provider-optimization changes will need diagnostics tests and documentation updated in lockstep.
- Unknown-provider capability fallback to `sqlite-v1` remains a risky default and will mislead callers unless diagnostics keeps surfacing it as a warning state rather than supported configuration.
- Split recommendation: No new split is recommended; the current evidence supports a bounded contract refinement without materializing child tickets, and this ticket remains the diagnostics child under story 06F0MECWYMPQ4R0KWV1R637RT0 that blocks docs ticket 06F0MEDJC732GDD77H60...
- Split recommendation: Keep the completed registry ticket 06F0MEAXT99V0P115P0WEJD4P0 as upstream context only and do not reopen registry redesign work inside this diagnostics task.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9220`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `4d303cdecde94c4b81ed8795ff701125`
- completed-at-utc: `<redacted>-10T02:22:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MED4P7HMBDZVMPWQZ5A7PC/runs/20260510T022223117Z-4d303cdecde94c4b81ed8795ff701125.json`