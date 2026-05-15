[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGG57K3S7CJQP5QX9AWW3G'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGG57K3S7CJQP5QX9AWW3G`.
- Optimistic claim succeeded (`expectedRevision=06F2PNGZAGV6QRSHE0G3KVYRK8`, `currentRevision=06F2PT7YPTG1C22CRR74ZZT1M0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGG57K3S7CJQP5QX9AWW3G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGG57K3S7CJQP5QX9AWW3G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGG57K3S7CJQP5QX9AWW3G-task-define-live-schema-reader-contract-and-fixt' from source 'd6371911a568a7ed0e8211575a8057e91b363ca0'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGG57K3S7CJQP5QX9AWW3G-task-define-live-schema-reader-contract-and-fixt` as `3b18f0595f99`.

Open questions / Risiken
- Provider catalogs vary in schema scoping, quoting, case folding, index metadata shape, and storage-type text; a fixture layer that is too generic may miss real reader bugs or become brittle.
- Oracle identifier-length limits and provider-specific physical naming rules can force fixture-specific naming overrides even when the logical metadata model is shared.
- External opt-in coverage depends on configured provider packages and create or drop permissions in developer-managed databases, so some reader defects may remain latent until those lanes are exercised.
- Split recommendation: No child split is needed for this contract-and-fixture ticket. If downstream implementation grows, split Task 06F2PGG8ZKSYGC8863118H56G8 by provider after this shared contract lands.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9090`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `39060bf6307e4437a16798ad518f8077`
- completed-at-utc: `<redacted>-15T11:40:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGG57K3S7CJQP5QX9AWW3G/runs/20260515T114023884Z-39060bf6307e4437a16798ad518f8077.json`