[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NTB26CCYQ7FCN2REEGDGW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NTB26CCYQ7FCN2REEGDGW`.
- Optimistic claim succeeded (`expectedRevision=06EZQ1925JYYQ6458ZJY7X73G0`, `currentRevision=06EZR1K360Q39KM62KXP35B1KM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06EZ0NTB26CCYQ7FCN2REEGDGW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NTB26CCYQ7FCN2REEGDGW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NTB26CCYQ7FCN2REEGDGW-task-generate-provider-neutral-pit-ef-model-mapp' from source 'ea26b137560844d3bd7d0a1e955c27895058d7a5'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- PIT output projection may require public enum, annotation, logical-property-kind, or provider type-mapping additions that ripple through multiple existing translator and snapshot tests.
- PIT scope can sprawl into refresh/materialization behavior or provider-specific optimization unless the one-hub plus attached-satellite projection boundary stays enforced.
- Split recommendation: No new functional split is recommended; keep the current PIT story split of metadata API, EF mapping, and docs/example work.
- Split recommendation: If workflow clarity needs stronger live dependency signaling, add a bounded blocks relation from 06EZ0NT4FDPC7XTQH40PQS942M to 06EZ0NTB26CCYQ7FCN2REEGDGW in a later ticket-operation pass.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8409`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `9c7253c8a25b4571bd17974a3e9d31c5`
- completed-at-utc: `<redacted>-06T06:53:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NTB26CCYQ7FCN2REEGDGW/runs/20260506T065301310Z-9c7253c8a25b4571bd17974a3e9d31c5.json`