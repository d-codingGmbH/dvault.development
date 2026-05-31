[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q93R4633D41Z21WQW3SVGR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q93R4633D41Z21WQW3SVGR`.
- Optimistic claim succeeded (`expectedRevision=06F7X2EW2TJJH14EGFG5KAYM2W`, `currentRevision=06F7X2R9F1HF1DNPP8K9DKA914`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q93R4633D41Z21WQW3SVGR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q93R4633D41Z21WQW3SVGR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q93R4633D41Z21WQW3SVGR-epic-tracing-and-performance-guidance' from source '6f75d9e90c305500f71569a21378a16473aa3913'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If any child ticket is reopened, the parent epic should stop at closure tracking until that child is done again.
- If another branch reintroduces a blocks relation or other parent-owned work signal during integration, rerun closure eligibility before final closure.
- Split recommendation: No additional split recommended; the existing five-child decomposition remains complete and the parent epic now carries only closure tracking.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `42869`
- cached-tokens: `7552`
- effective-cache-ratio: `0.1762`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `4378a60bd0ba45fe9010e7f3f0f62ed5`
- completed-at-utc: `<redacted>-31T15:12:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q93R4633D41Z21WQW3SVGR/runs/20260531T151206185Z-4378a60bd0ba45fe9010e7f3f0f62ed5.json`