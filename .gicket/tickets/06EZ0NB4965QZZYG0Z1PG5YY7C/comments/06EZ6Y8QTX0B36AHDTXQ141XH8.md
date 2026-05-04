[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NB4965QZZYG0Z1PG5YY7C'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NB4965QZZYG0Z1PG5YY7C`.
- Optimistic claim succeeded (`expectedRevision=06EZ0Y3Z3AR36BWAE9FEBW7TFC`, `currentRevision=06EZ6WVF5G3RDNA677GHG5X7JW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EZ0NB4965QZZYG0Z1PG5YY7C': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NB4965QZZYG0Z1PG5YY7C': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NB4965QZZYG0Z1PG5YY7C-story-optimize-oracle-provider-save-strategy' from source '8985a7be7c5a33546497080a2963a99819340c10'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EZ0NB4965QZZYG0Z1PG5YY7C-story-optimize-oracle-provider-save-strategy` as `b46402fffe64`.

Open questions / Risiken
- Oracle is not part of the default local developer environment, so live validation depends on a manually managed Oracle database, credentials, and table-create or table-drop permissions.
- The optimized Oracle path is intentionally narrow; callers using satellites or dirty DbContexts will still rely on the slower provider-neutral fallback and may perceive uneven performance coverage.
- Provider selection is gated by exact provider identity, so unexpected Oracle provider naming or configuration changes could silently route requests to fallback instead of the optimized path.
- The story currently has two incoming `blocks` relations, so delivery timing still depends on upstream tickets even though the PO refinement scope is clear.
- Split recommendation: No additional split is recommended in PO refinement. The story already has two materialized child tickets linked through `parentOf`: `06EZ0NBAP31G489S3YXXYY54WM` and `06EZ0NBH3YWJPF05AQWC0E6GV4`.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `54984`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0442`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `85b7e196e3c34d8ba726bbea6c33218c`
- completed-at-utc: `<redacted>-04T14:51:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NB4965QZZYG0Z1PG5YY7C/runs/20260504T145132016Z-85b7e196e3c34d8ba726bbea6c33218c.json`