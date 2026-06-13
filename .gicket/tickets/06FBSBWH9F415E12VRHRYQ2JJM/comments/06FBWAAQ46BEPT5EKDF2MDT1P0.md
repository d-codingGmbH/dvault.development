[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSBWH9F415E12VRHRYQ2JJM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSBWH9F415E12VRHRYQ2JJM`.
- Optimistic claim succeeded (`expectedRevision=06FBSCWZ0WP7MHSMYNTY3HWYTG`, `currentRevision=06FBW8VM2FC307K9T39FR8SZV8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSBWH9F415E12VRHRYQ2JJM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSBWH9F415E12VRHRYQ2JJM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSBWH9F415E12VRHRYQ2JJM-task-update-analyzer-packaging-docs-and-verifica' from source '5e0d6707d184528692d0656a836a60585f088a0b'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSBWH9F415E12VRHRYQ2JJM-task-update-analyzer-packaging-docs-and-verifica` as `d0cff3563c1f`.

Open questions / Risiken
- As long as the analyzer remains a single net10.0 asset, any future documentation or package-metadata change that implies pure .NET 8 SDK support will overstate what the repository currently verifies.
- Live ticket relations still show this ticket blocked by 06FBSBWBT33K7Y1Z6NM71GAQ68 and blocking 06FBSBWPN112S4CGP0239K0ZT8, so delivery sequencing can still depend on external ticket flow even though PO refinement is complete.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `56187`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0433`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `10cc18b4f68f4ce197931aae102df290`
- completed-at-utc: `<redacted>-12T23:27:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSBWH9F415E12VRHRYQ2JJM/runs/20260612T232720858Z-10cc18b4f68f4ce197931aae102df290.json`