[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FGX69QJYHGNKBV8MJ1HG7MMG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX69QJYHGNKBV8MJ1HG7MMG`.
- Optimistic claim succeeded (`expectedRevision=06FGZJFJEVPTSQVV2R4C1Q86NC`, `currentRevision=06FGZJV7QPYC1TGRTMS3629N54`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FGX69QJYHGNKBV8MJ1HG7MMG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FGX69QJYHGNKBV8MJ1HG7MMG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FGX69QJYHGNKBV8MJ1HG7MMG-task-implement-hash-key-storage-migration-manife' from source 'd6654952aecc026feb73b28a14b659719998180c'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FGX69QJYHGNKBV8MJ1HG7MMG-task-implement-hash-key-storage-migration-manife` as `9b473e2e0d80`.

Open questions / Risiken
- Repository docs currently describe conceptual field names that do not match the checked-in serialized v1 producer shape; if no later doc-alignment follow-up is taken, contributors may reintroduce the same ambiguity.
- Because invalid manifests are hand-built current-shape fixtures rather than producer-emitted files, exporter schema changes in a future ticket must update validator fixtures in lockstep.
- This ticket still sits directly upstream of 06FGX6B9KQME0NJ8B810239DG0, so validator result-shape or finding-code drift can ripple into downstream preflight wiring even though the original contract-definition ticket is already done.
- Split recommendation: No split is needed while this ticket stays validator-only and consumes the existing producer output.
- Split recommendation: If the team later wants to change the producer JSON shape, embed validation into the artifact, or publish a successor schema version, create a separate follow-up ticket rather than widening this task.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9005`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `b5483c89876849c38510ae76a8e18d29`
- completed-at-utc: `<redacted>-28T20:13:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX69QJYHGNKBV8MJ1HG7MMG/runs/20260628T201325388Z-b5483c89876849c38510ae76a8e18d29.json`