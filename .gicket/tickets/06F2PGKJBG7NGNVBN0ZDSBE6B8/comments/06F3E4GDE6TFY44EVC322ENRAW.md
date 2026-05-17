[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGKJBG7NGNVBN0ZDSBE6B8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGKJBG7NGNVBN0ZDSBE6B8`.
- Optimistic claim succeeded (`expectedRevision=06F3E2ZGRJ3DV7B1MG7CB589E4`, `currentRevision=06F3E36AXB642X27XH65D88500`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGKJBG7NGNVBN0ZDSBE6B8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGKJBG7NGNVBN0ZDSBE6B8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGKJBG7NGNVBN0ZDSBE6B8-task-test-link-parent-satellite-metadata-project' from source '8255bab49e9d45f27be9bff30118741d3e117b3d'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If the original human intent was a new fluent code-first capability instead of confirming existing projection coverage, closing this ticket will not deliver that future feature and a separate feature ticket will be needed.
- Split recommendation: No split on this closure ticket. Keep it no-work-required/already covered. If needed later, open a separate feature ticket for fluent code-first link-parent satellite support and a separate hardening ticket for any broader coverage expansion.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `53228`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0457`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `b732f27cb3e9452ab89e35d44526d345`
- completed-at-utc: `<redacted>-17T17:53:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGKJBG7NGNVBN0ZDSBE6B8/runs/20260517T175310323Z-b732f27cb3e9452ab89e35d44526d345.json`