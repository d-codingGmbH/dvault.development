[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB74XQJFKGSKVJ6THQWJY8W'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB74XQJFKGSKVJ6THQWJY8W`.
- Optimistic claim succeeded (`expectedRevision=06EXC1W36N5JZ2ASSY76EZCN98`, `currentRevision=06EXD50FD5BM2GN0E87V52QWPR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB74XQJFKGSKVJ6THQWJY8W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB74XQJFKGSKVJ6THQWJY8W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB74XQJFKGSKVJ6THQWJY8W-task-define-hub-link-and-satellite-metadata-abst' from source '491a3e7cac1b21b3ad5af8f969c8f51b4677f496'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB74XQJFKGSKVJ6THQWJY8W-task-define-hub-link-and-satellite-metadata-abst` as `8af770983291`.

Open questions / Risiken
- Because the direct blocker relation write was denied by local trust policy, sequencing is expressed in the refined contract rather than enforced by a newly persisted relation from this run.
- The ticket intentionally defines only a minimal v1 metadata surface, so future Data Vault variants may require additive model changes.
- The referenced charter standards are not expanded in the provided ticket context; developers should apply any standards already present in the repository when implementing after the foundation projects exist.
- Split recommendation: No split is needed for the metadata abstraction scope; the required sequencing is handled by waiting for the existing foundation solution/library/test project work rather than creating another child ticket from this task.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `27961`
- cached-tokens: `12160`
- effective-cache-ratio: `0.4349`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `aad3e59a1a9e48f48be9ec537d074590`
- completed-at-utc: `<redacted>-29T00:15:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB74XQJFKGSKVJ6THQWJY8W/runs/20260429T001517207Z-aad3e59a1a9e48f48be9ec537d074590.json`