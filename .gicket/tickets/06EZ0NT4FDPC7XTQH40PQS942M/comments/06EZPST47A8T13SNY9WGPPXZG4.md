[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NT4FDPC7XTQH40PQS942M'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NT4FDPC7XTQH40PQS942M`.
- Optimistic claim succeeded (`expectedRevision=06EZ0Y4A07HWMD2X0AWTC704EM`, `currentRevision=06EZPQR6MRHX3RCJNRHRBWEE0R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06EZ0NT4FDPC7XTQH40PQS942M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NT4FDPC7XTQH40PQS942M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NT4FDPC7XTQH40PQS942M-task-define-pit-metadata-model-and-builder-api' from source '7998587d8b622cf96c62951ee9117ab975970e64'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EZ0NT4FDPC7XTQH40PQS942M-task-define-pit-metadata-model-and-builder-api` as `b7d52e541a28`.

Open questions / Risiken
- If the implementation reuses existing satellite technical metadata or key-role abstractions too aggressively, PIT-specific fields may leak into the closed v1 ingest contract and create unnecessary public-API churn.
- If this ticket does not pin one provider-neutral PIT key and reference baseline, sibling mapping work may invent a different field shape and create model-builder versus EF-mapping drift.
- Current branch evidence already contains satellite index-shape differences between pure model generation and EF translation, so PIT tests need explicit cross-surface assertions to prevent the same divergence.
- Split recommendation: No additional split recommended. The parent story is already bounded across 06EZ0NT4FDPC7XTQH40PQS942M for metadata and builder work, 06EZ0NTB26CCYQ7FCN2REEGDGW for EF mapping, and 06EZ0NTJZEMVA5RPR01V0KNVMR for docs/example work.
- Split recommendation: No child tickets, relation changes, attachments, or planning documents were materialized in this refinement pass.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9355`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `39a8e3a3b41e4a629efae2e74cc9987a`
- completed-at-utc: `<redacted>-06T03:49:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NT4FDPC7XTQH40PQS942M/runs/20260506T034901361Z-39a8e3a3b41e4a629efae2e74cc9987a.json`