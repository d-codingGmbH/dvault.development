[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZM6KFZ3WC5MY5NC12B0TW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZM6KFZ3WC5MY5NC12B0TW`.
- Optimistic claim succeeded (`expectedRevision=06F98HC33ZA10ZNBJTZSS6MEN4`, `currentRevision=06F98HH2Y8D5010Z52PV93ZCQ8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZM6KFZ3WC5MY5NC12B0TW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZM6KFZ3WC5MY5NC12B0TW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZM6KFZ3WC5MY5NC12B0TW-epic-provider-naming-and-ddl-guardrails' from source '12dcf2de6852a271c75d97147541e537b6f5a7fc'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If future provider-expansion or physical-naming-override work is attached to this parent epic instead of new follow-up tickets, release traceability will blur and completed scope could be reopened unintentionally.
- Split recommendation: No new split is recommended; the existing four completed child tickets already cover the epic scope.
- Split recommendation: Any future provider-expansion or physical-naming-override work should be created as new follow-up tickets or epics rather than as children under this closure-only parent.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `50447`
- cached-tokens: `7552`
- effective-cache-ratio: `0.1497`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `85ed0aaacdbb4a01b6c6e829759a6554`
- completed-at-utc: `<redacted>-04T20:21:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/runs/20260604T202157059Z-85ed0aaacdbb4a01b6c6e829759a6554.json`