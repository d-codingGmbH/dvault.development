[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB74XQJFKGSKVJ6THQWJY8W'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB74XQJFKGSKVJ6THQWJY8W`.
- Optimistic claim succeeded (`expectedRevision=06EXFRMDZFTYS99PMYHRW0H6DC`, `currentRevision=06EXG2V48CNG6TZ6N1CHCWCVCG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB74XQJFKGSKVJ6THQWJY8W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB74XQJFKGSKVJ6THQWJY8W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB74XQJFKGSKVJ6THQWJY8W-task-define-hub-link-and-satellite-metadata-abst' from source '18bcb3c21a57c3825ce5b884aa7abe15efebb1f4'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Without a direct persisted blocker relation, sequencing depends on the ticket contract and blocked routing labels rather than an enforceable task-level dependency.
- Sending this ticket back to PO-critic before foundation completion would repeat the same blocking finding because current repository evidence still lacks the required structure.
- The ticket intentionally defines only a minimal v1 metadata surface, so future Data Vault variants may require additive model changes.
- Split recommendation: No split is needed for the metadata abstraction scope; keep this task blocked until the existing foundation solution/library/test project work is complete or directly linked as an enforceable dependency.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `34837`
- cached-tokens: `12160`
- effective-cache-ratio: `0.3491`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `8768d218289f44b6aa4cfb16c4d13faa`
- completed-at-utc: `<redacted>-29T07:05:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB74XQJFKGSKVJ6THQWJY8W/runs/20260429T070530279Z-8768d218289f44b6aa4cfb16c4d13faa.json`