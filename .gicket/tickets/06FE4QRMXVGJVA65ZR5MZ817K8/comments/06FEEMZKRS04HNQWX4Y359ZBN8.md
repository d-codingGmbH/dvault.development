[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FE4QRMXVGJVA65ZR5MZ817K8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4QRMXVGJVA65ZR5MZ817K8`.
- Optimistic claim succeeded (`expectedRevision=06FE4R484D7CD5VZFA9FGX4XAR`, `currentRevision=06FEEK8MJGABPKX2Q0NYKP2RRW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FE4QRMXVGJVA65ZR5MZ817K8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FE4QRMXVGJVA65ZR5MZ817K8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FE4QRMXVGJVA65ZR5MZ817K8-task-update-provider-performance-matrices-and-v0' from source 'a323e5750c0081e617657c89956db4dc30477210'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Future provider work can drift the docs if thresholds, accepted bundles, or fallback boundaries change without updating the release note, performance guide, local validation, and matrix surfaces together.
- Downstream tickets could overstate performance if they cite skipped-placeholder, diagnostics-only, smoke-only, or gap-matrix rows as completed timing instead of using the accepted artifact bundles.
- Split recommendation: No split recommended; the ticket is already bounded to a finite documentation baseline and the remaining unmeasured provider lanes are explicitly tracked in the gap matrix instead of needing new child tickets from this PO refinement.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9280`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `2758f39bd6ef473aa0bb0e1faf75d7cd`
- completed-at-utc: `<redacted>-20T23:18:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4QRMXVGJVA65ZR5MZ817K8/runs/20260620T231819319Z-2758f39bd6ef473aa0bb0e1faf75d7cd.json`