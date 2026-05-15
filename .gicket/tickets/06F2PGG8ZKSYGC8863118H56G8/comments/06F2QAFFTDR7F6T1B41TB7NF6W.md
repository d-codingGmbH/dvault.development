[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGG8ZKSYGC8863118H56G8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGG8ZKSYGC8863118H56G8`.
- Optimistic claim succeeded (`expectedRevision=06F2PNH2RCRVAN7H2H01YV3F3G`, `currentRevision=06F2Q8082T6MCGBARRBJGYAJX8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGG8ZKSYGC8863118H56G8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGG8ZKSYGC8863118H56G8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGG8ZKSYGC8863118H56G8-task-implement-provider-catalog-readers' from source 'bf85e8973b980f15eef49845fb8b1ebd3df28114'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGG8ZKSYGC8863118H56G8-task-implement-provider-catalog-readers` as `feb681e2299b`.

Open questions / Risiken
- Provider catalogs differ in schema scoping, identifier casing, index metadata shape, and storage-type text, so normalization mistakes can create false drift or hide real mismatches.
- Oracle physical-name limits and MySQL dual provider-name support make identifier and dispatch handling more brittle than the existing SQLite path.
- External opt-in coverage depends on configured provider packages and create/drop permissions in developer-managed databases, so some defects may remain latent when those lanes are not exercised.
- Split recommendation: Keep this as one implementation ticket for now; the shared contract and fixture work is already split out, the provider set is finite, and the remaining work is one bounded dispatch-plus-reader slice. Re-split by provider only if catalog-specific helper c...

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9318`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `67b4787725d5421392e2ca4a19401df0`
- completed-at-utc: `<redacted>-15T12:43:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGG8ZKSYGC8863118H56G8/runs/20260515T124337644Z-67b4787725d5421392e2ca4a19401df0.json`