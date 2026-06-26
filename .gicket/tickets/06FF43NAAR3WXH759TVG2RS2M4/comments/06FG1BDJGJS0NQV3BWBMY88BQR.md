[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF43NAAR3WXH759TVG2RS2M4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43NAAR3WXH759TVG2RS2M4`.
- Optimistic claim succeeded (`expectedRevision=06FF44PD3A7AEP3YABE2Q608Q8`, `currentRevision=06FG193SX1KP4T474T80683SX0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF43NAAR3WXH759TVG2RS2M4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF43NAAR3WXH759TVG2RS2M4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF43NAAR3WXH759TVG2RS2M4-task-extend-privacy-diagnostics-and-converter-te' from source '7d05b90aa461f24033e2644c676753ed28c5234e'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF43NAAR3WXH759TVG2RS2M4-task-extend-privacy-diagnostics-and-converter-te` as `adcad8885b10`.

Open questions / Risiken
- Most of the ticket's named behaviors are already present in the repository, so the implementation should avoid churn or broader behavior changes and focus on the remaining uncovered fail-closed branches.
- If new tests expose a defect, the fix must preserve redaction safety and the opt-in privacy boundary instead of silently relaxing failure behavior.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9564`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `ac8cc2551f054c668c28cdc27ab6a39b`
- completed-at-utc: `<redacted>-25T21:26:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43NAAR3WXH759TVG2RS2M4/runs/20260625T212651261Z-ac8cc2551f054c668c28cdc27ab6a39b.json`