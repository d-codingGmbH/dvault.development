[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q8Z0Y0ADE5H37DAPA1ADQM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q8Z0Y0ADE5H37DAPA1ADQM`.
- Optimistic claim succeeded (`expectedRevision=06F5Q98EC46PTC9FPFWX1BRB94`, `currentRevision=06F624CXPKEK3YR3H7BKGEW4W4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q8Z0Y0ADE5H37DAPA1ADQM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q8Z0Y0ADE5H37DAPA1ADQM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q8Z0Y0ADE5H37DAPA1ADQM-story-add-actionable-staged-bulk-fallback-diagno' from source '55245bb0433e65c6c6ec6768a1635d0dc3943cd7'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If staged fallback causes are emitted as provider-specific free-form text instead of one shared finite catalog, provider packages will drift and downstream docs will not have a stable vocabulary.
- If the story broadens into new save APIs or stage-management surface area, it will reopen the already-closed staging SPI and transaction-contract decision.
- If staged diagnostics leak transient stage object details, SQL text, or row values, they will violate the existing bounded telemetry and support-bundle redaction posture.
- Split recommendation: No additional split is recommended; the epic already separates staging contract, provider implementations, benchmarks, documentation, and this bounded diagnostics story.
- Split recommendation: If later implementation evidence shows materially different caveat taxonomies per provider, create provider-specific follow-up tickets rather than widening this shared diagnostics contract.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9262`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `01e494b990db47e58c6368bb6bd841cb`
- completed-at-utc: `<redacted>-25T21:51:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q8Z0Y0ADE5H37DAPA1ADQM/runs/20260525T215101952Z-01e494b990db47e58c6368bb6bd841cb.json`