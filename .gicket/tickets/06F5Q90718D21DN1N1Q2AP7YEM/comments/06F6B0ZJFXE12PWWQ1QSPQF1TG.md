[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q90718D21DN1N1Q2AP7YEM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q90718D21DN1N1Q2AP7YEM`.
- Optimistic claim succeeded (`expectedRevision=06F6AWTVKCG2PNZ0A3Z5BCBGY4`, `currentRevision=06F6AX3MY0N8CGWCTV3VXA2XKG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q90718D21DN1N1Q2AP7YEM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q90718D21DN1N1Q2AP7YEM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q90718D21DN1N1Q2AP7YEM-task-document-staged-bulk-guidance-and-stored-pr' from source 'a2eeac7fcf6c95dad3c33e66de265dffb8dc1733'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q90718D21DN1N1Q2AP7YEM-task-document-staged-bulk-guidance-and-stored-pr` as `3eaeb44177b4`.

Open questions / Risiken
- The persisted relation graph still carries incoming `blocks` links from done stories `06F5Q8Z0Y0ADE5H37DAPA1ADQM` and `06F5Q900FC0P3HBZP81CVK7264`; treat them as historical rather than active blockers, but reopened implementation or evidence changes would still require documen...
- Because three downstream tickets are currently blocked by this documentation ticket, ambiguity in the provider-specific write-path hierarchy or stored-procedure caveats will propagate quickly.
- If provider evidence or migration-synchronization rules are incomplete at doc-authoring time, the stored-procedure section can overclaim unsupported automation.
- If v0.20.0 release prose generalizes staged bulk beyond measured or supported provider lanes, adopter guidance can overstate SQL Server or Oracle behavior relative to the current repository evidence.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `43264`
- effective-cache-ratio: `0.4310`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `90a5c1d721cb49a6b29db7d39c996b10`
- completed-at-utc: `<redacted>-26T18:20:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q90718D21DN1N1Q2AP7YEM/runs/20260526T182006650Z-90a5c1d721cb49a6b29db7d39c996b10.json`