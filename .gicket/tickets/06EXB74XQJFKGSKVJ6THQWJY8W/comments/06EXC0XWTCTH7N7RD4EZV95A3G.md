[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB74XQJFKGSKVJ6THQWJY8W'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB74XQJFKGSKVJ6THQWJY8W`.
- Optimistic claim succeeded (`expectedRevision=06EXBF7FYE0F7RBEB17AKP4KSR`, `currentRevision=06EXC0E18GFH5XSHVB023AQYFM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB74XQJFKGSKVJ6THQWJY8W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB74XQJFKGSKVJ6THQWJY8W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB74XQJFKGSKVJ6THQWJY8W-task-define-hub-link-and-satellite-metadata-abst' from source '56779e28c638c4e74a4c29dc8b429f4a72283c98'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- 2 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The ticket intentionally defines only a minimal v1 metadata surface, so future Data Vault variants may require additive model changes.
- The referenced charter standards are not expanded in the provided ticket context; developers should apply any standards already present in the repository when implementing.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `18826`
- cached-tokens: `2432`
- effective-cache-ratio: `0.1292`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `6a289e458f2f438cb47f16e4539ab2e6`
- completed-at-utc: `<redacted>-28T21:34:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB74XQJFKGSKVJ6THQWJY8W/runs/20260428T213421816Z-6a289e458f2f438cb47f16e4539ab2e6.json`