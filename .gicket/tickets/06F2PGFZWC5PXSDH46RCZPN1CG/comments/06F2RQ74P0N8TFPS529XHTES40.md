[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGFZWC5PXSDH46RCZPN1CG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGFZWC5PXSDH46RCZPN1CG`.
- Optimistic claim succeeded (`expectedRevision=06F2PNGRD4KKV1VH7E6KG4KGYG`, `currentRevision=06F2RMV5KK5QTC66R9DZT1BBA4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGFZWC5PXSDH46RCZPN1CG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGFZWC5PXSDH46RCZPN1CG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGFZWC5PXSDH46RCZPN1CG-story-add-provider-live-schema-drift-readers' from source '8acccb8846389c4661c208de0e94715c71969f92'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGFZWC5PXSDH46RCZPN1CG-story-add-provider-live-schema-drift-readers` as `fdadd787d7c6`.

Open questions / Risiken
- Public documentation currently understates repository behavior; if 06F2PGHA0EXJRGDHM4GQM7NPYR slips, users may treat supported providers as unsupported.
- External-provider verification depends on developer-managed databases, connection strings, and conditional package restore, so some regressions can remain latent until those lanes are explicitly configured.
- Provider catalog normalization remains sensitive to schema scoping, casing, identifier limits, index metadata shape, and storage-type text, especially for Oracle and the dual MySQL provider-name aliases.
- Split recommendation: No additional split is recommended; the story already has the right bounded child split via 06F2PGG57K3S7CJQP5QX9AWW3G and 06F2PGG8ZKSYGC8863118H56G8, both verified done in the local ticket store.
- Split recommendation: Keep documentation and downstream design-time command/CI work as separate blocked tickets instead of folding them back into this story.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9041`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `b1aea291c5a6404292b4c01f07a72011`
- completed-at-utc: `<redacted>-15T15:59:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGFZWC5PXSDH46RCZPN1CG/runs/20260515T155905747Z-b1aea291c5a6404292b4c01f07a72011.json`