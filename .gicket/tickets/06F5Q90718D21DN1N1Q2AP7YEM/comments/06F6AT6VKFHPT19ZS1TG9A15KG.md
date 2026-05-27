[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q90718D21DN1N1Q2AP7YEM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q90718D21DN1N1Q2AP7YEM`.
- Optimistic claim succeeded (`expectedRevision=06F5Q9CG01ER49RG7APCBK8T1C`, `currentRevision=06F6AQNJC68DK6KZPWG8YJ0MY0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q90718D21DN1N1Q2AP7YEM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q90718D21DN1N1Q2AP7YEM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q90718D21DN1N1Q2AP7YEM-task-document-staged-bulk-guidance-and-stored-pr' from source '1fc9770b33e4feac929da4e02660838bba597e66'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The current ticket remains relation-blocked by `06F5Q8Z0Y0ADE5H37DAPA1ADQM` and `06F5Q900FC0P3HBZP81CVK7264`; if upstream implementation or evidence shifts, the documentation boundary may need wording changes before release.
- Because three downstream tickets are currently blocked by this documentation ticket, ambiguity in the write-path hierarchy or stored-procedure caveats will propagate quickly.
- If provider evidence or migration-synchronization rules are incomplete at doc-authoring time, the stored-procedure section can overclaim unsupported automation.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `76457`
- cached-tokens: `28800`
- effective-cache-ratio: `0.3767`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `7db4c2809c0d4f6ca0117d2625e9e5db`
- completed-at-utc: `<redacted>-26T17:50:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q90718D21DN1N1Q2AP7YEM/runs/20260526T175031315Z-7db4c2809c0d4f6ca0117d2625e9e5db.json`