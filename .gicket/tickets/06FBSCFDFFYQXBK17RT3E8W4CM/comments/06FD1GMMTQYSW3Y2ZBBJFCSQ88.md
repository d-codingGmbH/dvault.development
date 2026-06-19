[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSCFDFFYQXBK17RT3E8W4CM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCFDFFYQXBK17RT3E8W4CM`.
- Optimistic claim succeeded (`expectedRevision=06FBSD0CHNP5E0GWH6MNW839ZW`, `currentRevision=06FD1D989BEY0T0J2TZ4SAMBYR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSCFDFFYQXBK17RT3E8W4CM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSCFDFFYQXBK17RT3E8W4CM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSCFDFFYQXBK17RT3E8W4CM-task-close-postgresql-latest-satellite-read-gap' from source '965645e58de38a95ab927d858d211817d8b2512f'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The current repository baseline strongly supports no-work-required; attempting an implementation without provider-configured benchmark evidence risks overclaiming PostgreSQL latest-satellite performance.
- Mixing this ticket with PostgreSQL PIT/bridge work would violate the existing ticket split and blur a capability-gap decision into a separate evidence-gap lane.
- If optional PostgreSQL benchmark configuration is unavailable, an implemented strategy may still fail the timing-claim closure gate even if diagnostics and functional tests pass.
- Split recommendation: No new split recommended; the live graph already separates this PostgreSQL latest-satellite task from sibling provider latest-satellite tasks and the downstream read docs/benchmark ticket.
- Split recommendation: Do not pre-split PIT/bridge or cross-provider work out of this ticket; only create a later follow-on if a concrete PostgreSQL latest-satellite implementation proves functional but still needs separately scheduled benchmark or documentation execution.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8791`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `85aa0daafaf74104a48f7984459baab6`
- completed-at-utc: `<redacted>-16T14:07:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCFDFFYQXBK17RT3E8W4CM/runs/20260616T140753421Z-85aa0daafaf74104a48f7984459baab6.json`