[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XPW1N9PATP3R6YG53ZNGV0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F1XPW1N9PATP3R6YG53ZNGV0`.
- Optimistic claim succeeded (`expectedRevision=06F20RVHX8SKJDVC9CDF08KVE8`, `currentRevision=06F20SGKE8K44RB6FKM3QVRSVG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XPW1N9PATP3R6YG53ZNGV0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XPW1N9PATP3R6YG53ZNGV0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XPW1N9PATP3R6YG53ZNGV0-task-wire-design-time-validation-into-a-sample-w' from source '3abcd92ec9bc3aa9ca121daf540f621b3fe00f00'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If the sample uses internal helpers or internal formatting shortcuts instead of public Diagnostics and Compare results, it will be less useful as consumer-facing workflow evidence.
- If the documented command targets brittle individual test names rather than a stable focused workflow test/class, the docs may drift unnecessarily during routine test refactors.
- If the workflow opens or initializes a database instead of staying design-time-only, it will expand beyond the ticket's non-invasive validation intent.
- Split recommendation: If stakeholders also want a consumer-facing quickstart or CLI/build-lane automation, keep that as a follow-up ticket separate from this focused test-harness-and-documentation refinement.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7000`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `c1a97972810d4f9f8c980d17cabdd447`
- completed-at-utc: `<redacted>-13T08:25:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F1XPW1N9PATP3R6YG53ZNGV0/runs/20260513T082537467Z-c1a97972810d4f9f8c980d17cabdd447.json`