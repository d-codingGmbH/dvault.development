[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSCG6C40X9CV3FFEHHKS6G0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCG6C40X9CV3FFEHHKS6G0`.
- Optimistic claim succeeded (`expectedRevision=06FBSD0K0QE0Y67108H50Z0PK4`, `currentRevision=06FD35KB22XCJSPY7FYNGC82RR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSCG6C40X9CV3FFEHHKS6G0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSCG6C40X9CV3FFEHHKS6G0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSCG6C40X9CV3FFEHHKS6G0-task-close-db2-latest-satellite-read-gap' from source '709ff4aebbfe7ef6c54bc616b1d53f741b75ae00'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSCG6C40X9CV3FFEHHKS6G0-task-close-db2-latest-satellite-read-gap` as `1e18c06205a3`.

Open questions / Risiken
- A DB2 implementation may fail to produce a safe or worthwhile provider-specific latest-satellite path; in that case the ticket must close through the no-work-required branch rather than by widening unsupported claims.
- Without a configured DVAULT_TEST_DB2_CONNECTION_STRING benchmark run, any DB2 latest-satellite artifact row remains skipped-placeholder, so timing claims would still be unproven.
- Live ticket, comment, attachment, and relation reads were trust-blocked through gicket during this run, so ticket-state housekeeping beyond the provided snapshot could not be re-verified here.
- Split recommendation: No split is recommended; current evidence keeps DB2 latest-satellite closure as one bounded capability-decision ticket with an implementation branch and a no-work-required branch.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9127`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `761282333e5f4af6b9698fef2665bf51`
- completed-at-utc: `<redacted>-16T18:10:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCG6C40X9CV3FFEHHKS6G0/runs/20260616T181026167Z-761282333e5f4af6b9698fef2665bf51.json`