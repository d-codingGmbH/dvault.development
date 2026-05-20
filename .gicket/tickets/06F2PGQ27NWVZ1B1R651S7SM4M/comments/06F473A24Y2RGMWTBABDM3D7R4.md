[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGQ27NWVZ1B1R651S7SM4M'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGQ27NWVZ1B1R651S7SM4M`.
- Optimistic claim succeeded (`expectedRevision=06F2PNN0MCC4W11T1X3M17MFQW`, `currentRevision=06F47205TDC1A77SSQCXJCZ9A8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGQ27NWVZ1B1R651S7SM4M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGQ27NWVZ1B1R651S7SM4M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGQ27NWVZ1B1R651S7SM4M-epic-observability-and-operations' from source 'bf95cf363104b121a149ad8657e94a06ed5a7ccd'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If later docs or follow-up work overstate the current contract, consumers may assume automatic instrumentation, support-bundle transport, or broader runtime coverage than the repository actually ships.
- If future observability work stops reusing the existing diagnostics status/fallback vocabulary, telemetry, support-bundle output, and documentation can drift from one another.
- The lingering historical blocks relation from done epic 06F2PGP7HM8F39K3J0H5JHB3B4 could confuse later readers even though it is non-blocking today.
- Split recommendation: No additional split is recommended. The epic already has a finite four-ticket decomposition, and all four child tickets are present and done.
- Split recommendation: If future work is needed for maintenance-service telemetry, backend-specific observability guidance, support-bundle transport, or relation-hygiene cleanup, track it as separate follow-up tickets instead of reopening this epic.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7825`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `740b4c94bac8444cab2e219cb19019cd`
- completed-at-utc: `<redacted>-20T04:03:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGQ27NWVZ1B1R651S7SM4M/runs/20260520T040311395Z-740b4c94bac8444cab2e219cb19019cd.json`