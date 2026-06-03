[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZJNZ999C8NKY0S92VBDN0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZJNZ999C8NKY0S92VBDN0`.
- Optimistic claim succeeded (`expectedRevision=06F8M0115MQMYWRF3PFT72Z3EM`, `currentRevision=06F8Z72P22M2ZGS262404EFSWG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZJNZ999C8NKY0S92VBDN0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZJNZ999C8NKY0S92VBDN0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZJNZ999C8NKY0S92VBDN0-story-add-mysql-and-oracle-pit-bridge-read-strat' from source 'de7a03a586b8599ec5cd9171af694b975af9ec8b'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZJNZ999C8NKY0S92VBDN0-story-add-mysql-and-oracle-pit-bridge-read-strat` as `4445072969e3`.

Open questions / Risiken
- MySQL and Oracle currently have no PIT or bridge read strategy registrations and no read-focused opt-in integration classes, so scope can sprawl if benchmark or documentation follow-through leaks into this story.
- MySQL dual-provider identity and Oracle-specific parameter or identifier behavior can drift from provider-neutral parity unless raw-row and typed-projection parity coverage stays first-class.
- A deliberate decline without explicit tests, diagnostics, and handoff notes would leave the public provider matrix easier to overstate than the visible source proves.
- This story currently blocks benchmark task 06F8KZK2MSFQP9G2DBM61ZVGD4, so unresolved provider outcome here will cascade into downstream evidence work.
- Split recommendation: Keep the story whole if implementation stays limited to candidate evaluation, provider-package registration, gate coverage, and explicit decline evidence inside the existing PIT and bridge architecture.
- Split recommendation: Split by provider only if MySQL and Oracle diverge enough that one ships a candidate path while the other needs a decline-only outcome or materially different live-provider validation work.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9006`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `a5f1934f07d349f6b4c6d61b497d8f60`
- completed-at-utc: `<redacted>-03T22:42:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZJNZ999C8NKY0S92VBDN0/runs/20260603T224225413Z-a5f1934f07d349f6b4c6d61b497d8f60.json`