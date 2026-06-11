[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F9GF46KZYRKR1EGEPR3TV824'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F9GF46KZYRKR1EGEPR3TV824`.
- Optimistic claim succeeded (`expectedRevision=06F9GF53B3H94S8288GGMR8X7R`, `currentRevision=06FBA9YSGBEM5NSWWG1Y4CBK6W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F9GF46KZYRKR1EGEPR3TV824': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F9GF46KZYRKR1EGEPR3TV824': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F9GF46KZYRKR1EGEPR3TV824-task-surface-hash-algorithm-choices-in-diagnosti' from source '1d9bd42b837c3a450297e45979a0d74d48ca1b3d'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F9GF46KZYRKR1EGEPR3TV824-task-surface-hash-algorithm-choices-in-diagnosti` as `373f9ee1af6b`.

Open questions / Risiken
- If the implementation reads conventions instead of the active hash service, diagnostics can drift from real runtime behavior for caller-supplied IStableHashService overrides.
- Adding new public diagnostics fields changes both public API snapshot and support-bundle JSON expectations, so additive compatibility needs explicit approval-test coverage.
- The human-readable diagnostics summary is a redaction-sensitive surface; weak negative tests could let digest text or hash-key-related values leak into display output.
- Documentation task 06F9GF4CRMXKEY2QT97W0S3GTR stays blocked until this metadata surface lands.
- Split recommendation: No further split is recommended. Documentation and storage-profile follow-up are already decomposed into existing sibling tickets, and the current task is a bounded diagnostics and support-bundle slice.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9682`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `4bcfe6971ec24d839cc57ca7136fff2b`
- completed-at-utc: `<redacted>-11T05:51:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F9GF46KZYRKR1EGEPR3TV824/runs/20260611T055123782Z-4bcfe6971ec24d839cc57ca7136fff2b.json`