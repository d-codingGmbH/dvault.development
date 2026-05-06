[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NWKC9ZME5BSCJFSQEQ02R'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NWKC9ZME5BSCJFSQEQ02R`.
- Optimistic claim succeeded (`expectedRevision=06EZPDFHG55PB9WSKA0RZG5FEW`, `currentRevision=06EZPDPQ5AVNDX7JTJFVEPQR00`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06EZ0NWKC9ZME5BSCJFSQEQ02R': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NWKC9ZME5BSCJFSQEQ02R': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NWKC9ZME5BSCJFSQEQ02R-story-expose-advanced-configuration-hooks-needed' from source 'bda1a6ca77ca6cb32b49c6ac14c6bcc9063aceb3'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If eventual closure does not clean up live outgoing blocks relations, downstream tracking can continue to show stale dependency edges even though the hook work is already delivered through done child tickets.
- Reviewers can still misread docs/plans/optional-advanced-configuration-hooks.md as current API truth unless the parent contract keeps the architecture/background limitation explicit.
- Any future attempt to record an exact persisted-comment count in the parent contract will drift again because automation continues appending claim, lease, orchestration, and run-report comments.
- Split recommendation: Existing split already materialized and remains sufficient: 06EZ0NWTM3EPBJS0SWVHXGDGTM for timestamp and record-source hooks, 06EZ0NX282R80VF5VBKS6ARFZC for provider behavior hooks, and 06EZ0NX9SVP7MSB1R4PJ50EHGW for validation and failure-mode documentat...
- Split recommendation: No further split is recommended for this parent umbrella unless future naming or hashing customization becomes new implementation scope.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `52013`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0468`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `dd31756865af44649ba88a9e6f4a28a9`
- completed-at-utc: `<redacted>-06T03:05:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NWKC9ZME5BSCJFSQEQ02R/runs/20260506T030508238Z-dd31756865af44649ba88a9e6f4a28a9.json`