[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FGX6DSX1SRQ1Y22DP53629S8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX6DSX1SRQ1Y22DP53629S8`.
- Optimistic claim succeeded (`expectedRevision=06FGX6EZJ4CX5YGXHERHJCKTCR`, `currentRevision=06FH73S49PQN2GS70G4PYW1T84`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FGX6DSX1SRQ1Y22DP53629S8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FGX6DSX1SRQ1Y22DP53629S8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FGX6DSX1SRQ1Y22DP53629S8-task-update-v0-50-0-release-notes-and-package-va' from source 'e5520341f6239069f5c378efc0f16b415e3d8403'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FGX6DSX1SRQ1Y22DP53629S8-task-update-v0-50-0-release-notes-and-package-va` as `9eaf2c57cd1a`.

Open questions / Risiken
- If ancillary docs outside the explicit acceptance surface keep v0.49.0 as the current baseline, consumers may still see inconsistent current-release guidance after the main v0.50.0 note/changelog work is done.
- Because the visible verifier evidence targets packaged README/install fragments, drift in non-packaged planning or adoption documents can escape automated package verification unless those docs are manually reviewed or separately covered.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7938`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `7b066e201dc944419ea6ccd502e26ae9`
- completed-at-utc: `<redacted>-29T13:33:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX6DSX1SRQ1Y22DP53629S8/runs/20260629T133322685Z-7b066e201dc944419ea6ccd502e26ae9.json`