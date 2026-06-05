[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZR38EDSVZBCTC0XYR4R80'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZR38EDSVZBCTC0XYR4R80`.
- Optimistic claim succeeded (`expectedRevision=06F9JF85NPAYE22V0K0T5ESNN0`, `currentRevision=06F9KCRZP4E6XDFDQJC5RV773M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZR38EDSVZBCTC0XYR4R80': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZR38EDSVZBCTC0XYR4R80': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZR38EDSVZBCTC0XYR4R80-story-define-performance-decision-tree-contract' from source 'd10aa920476c17fee71608754ad7f836e6503e92'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If the new contract over-explains benchmark values instead of choice order, it will duplicate the existing profile tables and compete with the downstream practical-doc task instead of unblocking it.
- Optional PostgreSQL, SQL Server, MySQL, and Oracle provider rows are still evidence-visible but can be skipped when connection strings are unset; the contract must present those lanes as diagnostics-gated starting points, not as repository-proven measured wins.
- Typed-helper wording can regress into a false runtime-profile claim unless the doc keeps helper generation explicitly bound to one authoritative support bundle and reviewed `ReadShape` evidence.
- Read guidance can overpromise if it forgets the maintained PIT or bridge prerequisite or omits fallback handling such as unsupported shape or incomplete evidence from the decision tree.
- Split recommendation: No further split is needed. Keep this story as the contract-defining child under epic `06F8KZQNH8CCMTJW9P95W1N388` and leave practical examples, checklist polish, and release-note or navigation updates to the existing downstream tickets.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9264`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `48b40470498c46798f8b1b8ee758993b`
- completed-at-utc: `<redacted>-05T21:41:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZR38EDSVZBCTC0XYR4R80/runs/20260605T214119557Z-48b40470498c46798f8b1b8ee758993b.json`