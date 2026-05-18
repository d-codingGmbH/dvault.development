[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGNGVQ3TZZWSABAK5SNFK4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGNGVQ3TZZWSABAK5SNFK4`.
- Optimistic claim succeeded (`expectedRevision=06F3P4VT5GZVC1PM0YN01RFCQ4`, `currentRevision=06F3P4ZFNVTVQ0AXRS7RY8V2C8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGNGVQ3TZZWSABAK5SNFK4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGNGVQ3TZZWSABAK5SNFK4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGNGVQ3TZZWSABAK5SNFK4-story-add-provider-native-bulk-ingestion-strateg' from source '116dd999cc5a61b186d8f34e19c12f739d975dfe'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Audit/history confusion remains possible because the last visible develop integration for provider bulk proof is done child 06F2PGNT7DF4DVNKYWDFZC8DEM while this story currently claims implementation ownership.
- Documentation can continue to drift from actual shipped behavior until 06F2PGP2B2RZGGK3CVKK5WRRP8 reconciles older release-note wording, especially around Oracle bulk coverage.
- Split recommendation: No additional split is recommended; close or re-route this ticket as already-landed/no-work instead of creating a new child for code that is already on develop.
- Split recommendation: If future provider-native bulk changes arise, open a new follow-on story against the concrete missing delta instead of reusing this historical ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9130`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `4c29dea8019b43c8a9bdd487a8dba2d1`
- completed-at-utc: `<redacted>-18T12:45:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGNGVQ3TZZWSABAK5SNFK4/runs/20260518T124525047Z-4c29dea8019b43c8a9bdd487a8dba2d1.json`