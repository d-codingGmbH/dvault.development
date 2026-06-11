[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F9GF5FV54DGWY9GA8ZEZWM5R'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F9GF5FV54DGWY9GA8ZEZWM5R`.
- Optimistic claim succeeded (`expectedRevision=06F9GF724YKP6PMRMJ25FVWABG`, `currentRevision=06FBCMB1W5TM7RB8HWTM6XJG6G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F9GF5FV54DGWY9GA8ZEZWM5R': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F9GF5FV54DGWY9GA8ZEZWM5R': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract' from source '77dd4d760e43b664595376d188d620efd5cc9054'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract` as `dc8b54fc7669`.

Open questions / Risiken
- This ticket remains a dependency for blocked ticket 06F9GF5N4N3Q685XQPKTM5EC00, so delayed contract landing will continue to hold downstream work.
- If hash-key storage profile facts are not applied consistently across keys and hash-key references, joins, indexes, and cross-table comparisons can drift even when logical hash values still look valid at the API boundary.
- Changing algorithm id or converting persisted text columns to binary remains caller-owned compatibility work; the safe default is to reject unsupported transitions rather than infer or auto-migrate data.
- Provider-scope documentation drift is already visible because one older planning contract predates the current DB2 baseline; final implementation documentation needs to align on the six-profile repository baseline to avoid conflicting handoff guidance.
- Split recommendation: If delivery scope needs to shrink, split provider profile plus EF annotation/storage-profile work from migration/live-schema/explain guardrail work while keeping this ticket as the contract parent.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9306`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `015ece63e1624e0f890062d293cc5a25`
- completed-at-utc: `<redacted>-11T11:06:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/runs/20260611T110659250Z-015ece63e1624e0f890062d293cc5a25.json`