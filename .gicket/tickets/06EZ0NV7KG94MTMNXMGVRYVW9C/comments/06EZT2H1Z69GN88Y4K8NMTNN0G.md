[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NV7KG94MTMNXMGVRYVW9C'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NV7KG94MTMNXMGVRYVW9C`.
- Optimistic claim succeeded (`expectedRevision=06EZSKDBZKR3CXVYW2MAW1ZWVM`, `currentRevision=06EZT0T7SW091X6GNK8KPYNAFR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06EZ0NV7KG94MTMNXMGVRYVW9C': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NV7KG94MTMNXMGVRYVW9C': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NV7KG94MTMNXMGVRYVW9C-task-generate-provider-neutral-bridge-ef-model-m' from source 'edddd33c3ae9e9c39665e3e28e186f6a45313aa0'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EZ0NV7KG94MTMNXMGVRYVW9C-task-generate-provider-neutral-bridge-ef-model-m` as `25867e83510b`.

Open questions / Risiken
- If sibling implementation or later ticket text diverges from docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md, the mapping and documentation siblings may target different bridge contracts.
- If the new hierarchy-depth property is modeled by reusing existing payload or technical semantics instead of a distinct logical kind or annotation, provider capability mappings and tests will become ambiguous.
- If endpoint declaration order is not preserved exactly, produced column order and deterministic key and index names will drift from the ratified examples.
- If this ticket introduces EF relationships or provider-specific physical behavior, the existing shared-type and SQLite baselines may regress.
- Split recommendation: No further split is recommended now; the existing bridge story plus metadata, mapping, and documentation siblings remain the right decomposition.
- Split recommendation: Treat richer bridge capability families such as effectivity windows, closure maintenance, query-model helpers, or navigation graph generation as separate follow-up tickets rather than broadening this provider-neutral mapping slice.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `49952`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0487`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `8aed49fc85854b01b2e0486d5c878047`
- completed-at-utc: `<redacted>-06T11:26:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NV7KG94MTMNXMGVRYVW9C/runs/20260506T112632179Z-8aed49fc85854b01b2e0486d5c878047.json`