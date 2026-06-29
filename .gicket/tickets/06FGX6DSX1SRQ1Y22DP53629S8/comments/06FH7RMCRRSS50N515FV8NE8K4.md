[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FGX6DSX1SRQ1Y22DP53629S8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX6DSX1SRQ1Y22DP53629S8`.
- Optimistic claim succeeded (`expectedRevision=06FH77VBGF5P23Z6MSVSFD061W`, `currentRevision=06FH7Q7AH3XQSQE88JWAQWVC8M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FGX6DSX1SRQ1Y22DP53629S8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FGX6DSX1SRQ1Y22DP53629S8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FGX6DSX1SRQ1Y22DP53629S8-task-update-v0-50-0-release-notes-and-package-va' from source '58bcf314eaf5d4310c7ce1cbc99ece01bdad01d3'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FGX6DSX1SRQ1Y22DP53629S8-task-update-v0-50-0-release-notes-and-package-va` as `d1340ef12d34`.

Open questions / Risiken
- Consumers may still see v0.49.0 presented as the current baseline in ancillary documentation until follow-up cleanup lands.
- Package verification focuses on packaged README/install guidance, so non-packaged docs can drift without automated coverage unless they are reviewed explicitly or covered by a separate follow-up.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.6634`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `bb394a8706ad4100889a587a4c4ed1a0`
- completed-at-utc: `<redacted>-29T14:57:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX6DSX1SRQ1Y22DP53629S8/runs/20260629T145722113Z-bb394a8706ad4100889a587a4c4ed1a0.json`