[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NVE88WW9PMM04NVAZHRG0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NVE88WW9PMM04NVAZHRG0`.
- Optimistic claim succeeded (`expectedRevision=06EZQJ13F70TQB1FNPPC7H6JZG`, `currentRevision=06EZREKVR07H0A0HVVJ7VN8A50`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06EZ0NVE88WW9PMM04NVAZHRG0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NVE88WW9PMM04NVAZHRG0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NVE88WW9PMM04NVAZHRG0-task-add-bridge-documentation-and-example-scenar' from source '279064a91609dfc6f988c73dd1d3f0847b8c91ff'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- When parent story 06EZ0NTV4SVAKV98C418T8A3CC is refined, its final bridge naming or metadata shape may require one last sync pass on this child before dev handoff.
- If this child reaches dev before the parent contract becomes authoritative, the docs example would again rely on guesses rather than a ratified bridge surface.
- Split recommendation: No split recommended for the current child while it remains a bounded docs task blocked on the parent bridge contract.
- Split recommendation: If future scope needs a concrete hierarchy-style walkthrough in addition to the many-to-many example, create a separate follow-up docs ticket after the parent bridge surface is defined.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `41790`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0582`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `155224a8e7e64c5a971f33ec763e6102`
- completed-at-utc: `<redacted>-06T07:47:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NVE88WW9PMM04NVAZHRG0/runs/20260506T074725580Z-155224a8e7e64c5a971f33ec763e6102.json`