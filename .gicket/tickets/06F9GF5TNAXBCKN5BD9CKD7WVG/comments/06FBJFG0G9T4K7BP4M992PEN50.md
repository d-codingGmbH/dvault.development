[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F9GF5TNAXBCKN5BD9CKD7WVG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F9GF5TNAXBCKN5BD9CKD7WVG`.
- Optimistic claim succeeded (`expectedRevision=06F9GF75RRYTF9X09W6X3TJXT8`, `currentRevision=06FBJCB87T47PJN4W9FES8HVH0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F9GF5TNAXBCKN5BD9CKD7WVG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F9GF5TNAXBCKN5BD9CKD7WVG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F9GF5TNAXBCKN5BD9CKD7WVG-story-add-provider-specific-binary-hash-column-m' from source 'b39171c73886d42dcad3446b91f6ee89f7013f30'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If callers rely on implicit provider fallback instead of a resolved built-in or registered provider profile, SQLite-default capability selection could be mistaken for a provider-specific guarantee unless the existing defaulted diagnostics warnings remain visible.
- DB2 parity is only partially proven at the repository level for this story because the live-schema reader intentionally remains unsupported, so full drift verification for DB2 stays outside the current scope.
- Split recommendation: No further split recommended; the current story is already bounded between done provider-neutral conversion work in 06F9GF5N4N3Q685XQPKTM5EC00 and downstream integration coverage in 06F9GF60BKEW0CC9FCZRPVX0SR.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9361`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `d6a7ee2b38714d5c9be616d805ac0253`
- completed-at-utc: `<redacted>-12T00:31:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F9GF5TNAXBCKN5BD9CKD7WVG/runs/20260612T003148859Z-d6a7ee2b38714d5c9be616d805ac0253.json`