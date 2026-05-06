[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NTB26CCYQ7FCN2REEGDGW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NTB26CCYQ7FCN2REEGDGW`.
- Optimistic claim succeeded (`expectedRevision=06EZR8262AH7SCZC22Y37FPG2W`, `currentRevision=06EZRYJ5BTY3RRCE49JSAY7AGC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06EZ0NTB26CCYQ7FCN2REEGDGW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NTB26CCYQ7FCN2REEGDGW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NTB26CCYQ7FCN2REEGDGW-task-generate-provider-neutral-pit-ef-model-mapp' from source '92e9eb18d0b8336ec21690b6dae20df4b936fe95'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EZ0NTB26CCYQ7FCN2REEGDGW-task-generate-provider-neutral-pit-ef-model-mapp` as `0f9dfa62ff87`.

Open questions / Risiken
- Because the repository currently exposes zero PIT-facing public surface, implementation may require coordinated additions across enums, annotations, provider type mappings, translator tests, and approved API snapshots.
- If sibling 06EZ0NT4FDPC7XTQH40PQS942M lands a producer-side contract that diverges from the consumer-side minimum copied here, this ticket will need a PO re-check before dev handoff even though the live blocks relation already enforces sequencing.
- Split recommendation: No new functional split is needed; keep the existing PIT story split between producer-side modeling API ticket 06EZ0NT4FDPC7XTQH40PQS942M, EF mapping and projection ticket 06EZ0NTB26CCYQ7FCN2REEGDGW, and docs or examples ticket 06EZ0NTJZEMVA5RPR01V0KNVMR.
- Split recommendation: Retain the existing blocks relation for sequencing instead of creating another coordination ticket or planning document.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `40252`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0604`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `31c52888a43e4a11869ed4b8d5d33386`
- completed-at-utc: `<redacted>-06T08:59:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/runs/20260506T085923683Z-31c52888a43e4a11869ed4b8d5d33386.json`