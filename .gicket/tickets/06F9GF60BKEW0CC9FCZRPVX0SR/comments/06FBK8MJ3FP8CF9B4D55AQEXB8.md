[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F9GF60BKEW0CC9FCZRPVX0SR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F9GF60BKEW0CC9FCZRPVX0SR`.
- Optimistic claim succeeded (`expectedRevision=06F9GF77EKNG8WBY6B1ZPGKEE4`, `currentRevision=06FBK5F2DB9ASS689QNS7EH04G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F9GF60BKEW0CC9FCZRPVX0SR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F9GF60BKEW0CC9FCZRPVX0SR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F9GF60BKEW0CC9FCZRPVX0SR-task-add-schema-save-and-read-tests-for-hash-sto' from source '226e58a92946cbde791c33e4b661d0e64c85d4fd'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If implementation stops at provider-mapping unit assertions and skips an executable Binary save/read round-trip, EF conversion or query-translation regressions can still slip through.
- PIT and bridge coverage depend on explicitly seeded maintained tables in the test harness; partial read-path coverage could leave a false impression that Binary support is complete.
- Requiring every external provider to execute Binary round-trips in this ticket would couple a test-only task to optional database infrastructure and likely expand it beyond the current bounded scope.
- Scheduling still depends on the existing incoming blocks relation from 06F9GF5TNAXBCKN5BD9CKD7WVG; refinement did not change dependency state.
- Split recommendation: No split recommended while the work stays within existing test and shared-fixture surfaces.
- Split recommendation: If later stakeholders want live Binary execution across optional external providers, spin that into a follow-up ticket instead of broadening this test-coverage ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9436`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `6eaa999f0b3f4be0a460afabe68c74ab`
- completed-at-utc: `<redacted>-12T02:21:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F9GF60BKEW0CC9FCZRPVX0SR/runs/20260612T022139730Z-6eaa999f0b3f4be0a460afabe68c74ab.json`