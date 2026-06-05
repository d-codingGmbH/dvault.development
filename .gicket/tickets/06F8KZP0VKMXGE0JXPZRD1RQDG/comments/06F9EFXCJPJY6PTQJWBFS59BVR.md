[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZP0VKMXGE0JXPZRD1RQDG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZP0VKMXGE0JXPZRD1RQDG`.
- Optimistic claim succeeded (`expectedRevision=06F8KZYQKP9VVRG30FCH6KAKBC`, `currentRevision=06F9EEA7R69WMQJ251EZNRNGJ8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZP0VKMXGE0JXPZRD1RQDG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZP0VKMXGE0JXPZRD1RQDG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZP0VKMXGE0JXPZRD1RQDG-epic-support-bundle-freshness-and-generator-diag' from source '5739710aacd3c6512aab6075d6be3e5b67e41517'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The live relation set still includes an incoming `blocks` edge from child `06F8KZQAWZ7QRGB68KB21C9B0R`, so epic closure remains operationally dependent on that child until the relation is cleared or satisfied.
- Because PIT and bridge helper emission depends on representative request-bound `readShape` evidence, incomplete support-bundle diagnostics capture can still prevent intended helper generation even when metadata itself is valid.
- Split recommendation: No additional split is recommended in this refinement pass; the existing four-child epic structure should remain the delivery vehicle unless one child grows beyond a single bounded diagnostics theme.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `28363`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0857`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `38089cad43a24ea387baf501c06586d9`
- completed-at-utc: `<redacted>-05T10:06:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZP0VKMXGE0JXPZRD1RQDG/runs/20260605T100633092Z-38089cad43a24ea387baf501c06586d9.json`