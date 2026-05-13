[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XPW1N9PATP3R6YG53ZNGV0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F1XPW1N9PATP3R6YG53ZNGV0`.
- Optimistic claim succeeded (`expectedRevision=06F20ZYFTRG193DS2NS6ZKS41R`, `currentRevision=06F2106GYQ87DE09X6397KT7CM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XPW1N9PATP3R6YG53ZNGV0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XPW1N9PATP3R6YG53ZNGV0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XPW1N9PATP3R6YG53ZNGV0-task-wire-design-time-validation-into-a-sample-w' from source '1ff95ec7c1ccef147729b9d8dff0ba123bb4ff49'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F1XPW1N9PATP3R6YG53ZNGV0-task-wire-design-time-validation-into-a-sample-w` as `5e82807180cc`.

Open questions / Risiken
- If the added sample reaches into internal translation or formatting helpers instead of the public import, opt-in, drift, and diagnostic surfaces, it will stop being durable consumer-facing evidence.
- Documentation can drift if it targets brittle individual test names instead of a stable focused test class or filter.
- If the workflow opens or initializes a real database, it expands beyond the bounded design-time intent.
- Split recommendation: No split is required for PO-critic readiness; the current ticket is bounded once it is limited to unit-test coverage plus governance documentation.
- Split recommendation: If stakeholders later want a runnable quickstart, CLI or build-lane automation, or a broader invalid-model matrix, create separate follow-up tickets instead of expanding this task.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `48681`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0500`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `6ef3ea55683d484e8565ed577704d707`
- completed-at-utc: `<redacted>-13T08:52:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F1XPW1N9PATP3R6YG53ZNGV0/runs/20260513T085243958Z-6ef3ea55683d484e8565ed577704d707.json`