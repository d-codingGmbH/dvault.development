[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q93AVHRYJBAPJCJEB4N7KG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q93AVHRYJBAPJCJEB4N7KG`.
- Optimistic claim succeeded (`expectedRevision=06F5Q99KF1Z2RP97HSTPRGQB28`, `currentRevision=06F6SCTCG2149QMN8S07N4F5VW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q93AVHRYJBAPJCJEB4N7KG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q93AVHRYJBAPJCJEB4N7KG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q93AVHRYJBAPJCJEB4N7KG-task-document-database-side-hashing-boundary-and' from source '982d927f336f34571b6d9c288ad002b093a0f1ac'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If later documentation skips the shared manifest, vectors, or benchmark contract, teams could mistake provider-local hash behavior for proven compatibility.
- Downstream documentation rollup ticket 06F5Q93H60W6X8FJ88PWTR6NG4 remains blocked until this boundary text is landed.
- Split recommendation: If work ever moves beyond documentation, split it into one shared contract or governance ticket plus separate provider-specific evidence or implementation tickets; do not widen this ticket into multi-provider runtime hashing work.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9334`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `20f3a0ffa1934ea2aaf34f2da15314df`
- completed-at-utc: `<redacted>-28T03:58:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q93AVHRYJBAPJCJEB4N7KG/runs/20260528T035859733Z-20f3a0ffa1934ea2aaf34f2da15314df.json`