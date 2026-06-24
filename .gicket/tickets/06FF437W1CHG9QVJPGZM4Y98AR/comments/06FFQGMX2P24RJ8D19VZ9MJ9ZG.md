[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF437W1CHG9QVJPGZM4Y98AR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF437W1CHG9QVJPGZM4Y98AR`.
- Optimistic claim succeeded (`expectedRevision=06FF45MQ12F2YB9GR3RRBT9DT0`, `currentRevision=06FFQEJK3PS1H07FJJZEAY5JY8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF437W1CHG9QVJPGZM4Y98AR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF437W1CHG9QVJPGZM4Y98AR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF437W1CHG9QVJPGZM4Y98AR-story-define-provider-pit-maintenance-evidence-c' from source 'ad040600d3d6271dba237fea8cf74307422a4adf'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Downstream documentation or release summaries could still overstate PIT read rows or source/test-backed provider lanes as maintenance timing evidence if they stop citing the evidence matrix and artifact contract.
- The provider-maintenance baseline is intentionally asymmetric, so careless summaries can imply Oracle or DB2 parity that the repository does not currently implement or benchmark.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9232`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `cd24e4cb6af24816950e12a2de932e02`
- completed-at-utc: `<redacted>-24T22:31:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF437W1CHG9QVJPGZM4Y98AR/runs/20260624T223135951Z-cd24e4cb6af24816950e12a2de932e02.json`