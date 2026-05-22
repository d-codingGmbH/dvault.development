[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F492BZPP5YT9SJSPDHQBGF3R'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492BZPP5YT9SJSPDHQBGF3R`.
- Optimistic claim succeeded (`expectedRevision=06F4NV0QNRDSRMSHR6MT2XCTH4`, `currentRevision=06F517BXWYVZVDPMA62VX7JJVG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F492BZPP5YT9SJSPDHQBGF3R': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F492BZPP5YT9SJSPDHQBGF3R': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F492BZPP5YT9SJSPDHQBGF3R-story-define-performance-evidence-and-benchmark' from source '17d9cd7d76d1b64153baf7612751129384d2f662'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F492BZPP5YT9SJSPDHQBGF3R-story-define-performance-evidence-and-benchmark` as `5337f23faadb`.

Open questions / Risiken
- The current visible harness already persists timing artifacts but not the full allocation and SQL evidence this story wants, so contract work that stops at prose without schema or test updates will let downstream tickets drift.
- External-provider availability is environment-dependent; if the contract is not explicit about `skipped` rows, teams may misread missing PostgreSQL, SQL Server, MySQL, or Oracle evidence as regressions or false failures.
- A contract that does not preserve comparable before/after artifact sets will make later release-note and tuning tickets argue over anecdotal console output instead of repeatable evidence.
- Split recommendation: No split recommended; keep this story as the single contract-definition blocker and push actual performance tuning or provider-specific expansions into the already-related downstream tickets.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `44394`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0548`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `9bb0870b22534a338a07719b08215410`
- completed-at-utc: `<redacted>-22T17:04:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492BZPP5YT9SJSPDHQBGF3R/runs/20260522T170453965Z-9bb0870b22534a338a07719b08215410.json`