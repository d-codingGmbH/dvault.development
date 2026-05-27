[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q90SX5AQ07M4PQKDR4BZD8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q90SX5AQ07M4PQKDR4BZD8`.
- Optimistic claim succeeded (`expectedRevision=06F5Q98S6A122CW2N73QQ0R8K4`, `currentRevision=06F6J45RD491JCBYWE1085AJ5R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q90SX5AQ07M4PQKDR4BZD8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q90SX5AQ07M4PQKDR4BZD8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q90SX5AQ07M4PQKDR4BZD8-story-support-link-parent-pit-maintenance-and-re' from source '3a73524100fef27b35b1b8c86644d79cdbf25cd8'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q90SX5AQ07M4PQKDR4BZD8-story-support-link-parent-pit-maintenance-and-re` as `cdf25f64bd60`.

Open questions / Risiken
- README, production-adoption guidance, deferred-capabilities planning text, and existing release notes currently describe link-parent PITs as unsupported; partial doc updates would create public contract drift.
- The current codebase has separate hub-only guards in PIT translation, maintenance validation, read validation, and strategy diagnostics, so updating only one path would leave inconsistent behavior or regress hub-parent compatibility.
- Downstream diagnostics/benchmark work already depends on this story, so incomplete link-parent validation or missing regression coverage would delay later PIT evidence tickets.
- Split recommendation: No additional split is recommended. Multi-active PIT work and broader diagnostics/benchmark evidence are already separated, and the remaining link-parent PIT work is one coherent baseline across projection, maintenance, provider-neutral reads, tests, and ...

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.3533`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `f4afca5411c04203b89fe87efc0e533a`
- completed-at-utc: `<redacted>-27T11:13:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q90SX5AQ07M4PQKDR4BZD8/runs/20260527T111339412Z-f4afca5411c04203b89fe87efc0e533a.json`