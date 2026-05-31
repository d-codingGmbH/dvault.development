[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q93R4633D41Z21WQW3SVGR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q93R4633D41Z21WQW3SVGR`.
- Optimistic claim succeeded (`expectedRevision=06F7W5B1XVANTKAW0ND0PJEQY4`, `currentRevision=06F7W5MG9BAJPZB95JQ91CVTD0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q93R4633D41Z21WQW3SVGR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q93R4633D41Z21WQW3SVGR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q93R4633D41Z21WQW3SVGR-epic-tracing-and-performance-guidance' from source '31481daaf25845ac2d9373a7a35d8e7ada01c27b'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Until queued replay lands, repository snapshots can still show `.gicket/relations/G4/GR/06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks.json`, which can confuse closure checks even though the PO cleanup action has been materialized.
- Two child-ticket reads previously hit the structured result bytes cap, so live child status should still be confirmed before final closure if runtime does not already enforce that.
- If a contract or tracing child reopens with scope beyond the fixed tracing vocabulary or evidence-bound performance posture, the epic may need re-scoping rather than simple closure.
- Split recommendation: No additional split recommended; the existing five-child decomposition remains bounded and matches the landed repository surfaces.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `33944`
- cached-tokens: `7552`
- effective-cache-ratio: `0.2225`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `fc5ba4c8bb9d4b7c9525436f40181362`
- completed-at-utc: `<redacted>-31T12:59:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q93R4633D41Z21WQW3SVGR/runs/20260531T125925847Z-fc5ba4c8bb9d4b7c9525436f40181362.json`