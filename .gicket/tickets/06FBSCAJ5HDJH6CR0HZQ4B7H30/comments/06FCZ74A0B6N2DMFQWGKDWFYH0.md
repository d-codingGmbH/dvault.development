[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSCAJ5HDJH6CR0HZQ4B7H30'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCAJ5HDJH6CR0HZQ4B7H30`.
- Optimistic claim succeeded (`expectedRevision=06FBSCZ7FG6GEH8QENMPXW1T9C`, `currentRevision=06FCZ5EA245Z20QG40Z0M8NG84`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSCAJ5HDJH6CR0HZQ4B7H30': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSCAJ5HDJH6CR0HZQ4B7H30': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSCAJ5HDJH6CR0HZQ4B7H30-task-implement-accepted-oracle-bulk-improvement' from source '2446d19fc08f5818b436b69e6166ebc3a4a0a22f'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSCAJ5HDJH6CR0HZQ4B7H30-task-implement-accepted-oracle-bulk-improvement` as `ebf59e11be9f`.

Open questions / Risiken
- The root quick benchmark baseline still carries Oracle as a skipped-placeholder row when DVAULT_TEST_ORACLE_CONNECTION_STRING is unset, so downstream documentation can overstate Oracle timing evidence if it ignores evidence posture.
- Reopening staged Oracle bulk inside this ticket would conflict with the current source, docs, and artifact contract that keep Oracle on the retained direct batching path.
- Any stale live relation cleanup could not be re-verified in-session because gicket relation reads were trust-blocked.
- Split recommendation: No split is justified from current repository evidence; the remaining Oracle work is already bounded as an evidence-gap follow-up in the provider optimization gap matrix rather than a child implementation ticket from this task.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8865`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `d222ce0a78244415b3a641a4d7f7bcf5`
- completed-at-utc: `<redacted>-16T08:46:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCAJ5HDJH6CR0HZQ4B7H30/runs/20260616T084643002Z-d222ce0a78244415b3a641a4d7f7bcf5.json`