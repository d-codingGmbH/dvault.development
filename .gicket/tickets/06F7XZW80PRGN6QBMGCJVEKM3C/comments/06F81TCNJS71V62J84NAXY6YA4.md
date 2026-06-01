[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F7XZW80PRGN6QBMGCJVEKM3C'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7XZW80PRGN6QBMGCJVEKM3C`.
- Optimistic claim succeeded (`expectedRevision=06F7Y0TX56R99KKPDDMN8SGS20`, `currentRevision=06F81R6RYAPC7376Q0K5ASK5YM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F7XZW80PRGN6QBMGCJVEKM3C': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F7XZW80PRGN6QBMGCJVEKM3C': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F7XZW80PRGN6QBMGCJVEKM3C-epic-async-streaming-save-and-ef-core-safety' from source '009f360339687bcde6f434ac4db182466c035dbf'.
- Interactive PO tool loop fell back to legacy planning after MODEL-TOOL-INVOCATION-RESULT-TOOL-CALL-LIMIT-EXCEEDED.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The epic title and older scope wording can be misread as promising new model-cache or pooling diagnostics even though the landed boundary intentionally settles on guidance-only EF safety.
- Optional external-provider benchmark rows are currently skipped when DVAULT_TEST_* connection strings are unset, so performance and provider wording must remain bounded to the checked-in SQLite/provider-neutral evidence.
- Consumers can still misuse compiled models or pooled contexts for variable realized model shapes; the current mitigation is documentation plus caller-owned cache-key design, not enforcement.
- Split recommendation: Keep any future provider-native async write or provider-specific async execution claims in a separate follow-on ticket rather than expanding this epic beyond provider-neutral async source saves.
- Split recommendation: Keep any future model-cache, compiled-model, or pooling analyzer/runtime guardrails in a separate follow-on ticket if guidance-only EF safety proves insufficient.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8251`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `7bcc6f57de77435bb25164eb1df8b7a2`
- completed-at-utc: `<redacted>-01T02:00:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7XZW80PRGN6QBMGCJVEKM3C/runs/20260601T020052368Z-7bcc6f57de77435bb25164eb1df8b7a2.json`