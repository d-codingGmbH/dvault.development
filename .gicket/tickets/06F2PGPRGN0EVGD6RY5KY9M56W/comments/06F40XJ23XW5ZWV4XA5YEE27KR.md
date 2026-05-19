[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGPRGN0EVGD6RY5KY9M56W'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGPRGN0EVGD6RY5KY9M56W`.
- Optimistic claim succeeded (`expectedRevision=06F40W5HZ59VPPGY6019V5J2VG`, `currentRevision=06F40W7JV59VEF9B4W196CHTPC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGPRGN0EVGD6RY5KY9M56W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGPRGN0EVGD6RY5KY9M56W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGPRGN0EVGD6RY5KY9M56W-story-add-provider-aware-pit-and-bridge-read-opt' from source '3c399380a68b9b942095b6957961187d3b2bb06f'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGPRGN0EVGD6RY5KY9M56W-story-add-provider-aware-pit-and-bridge-read-opt` as `0672955858b4`.

Open questions / Risiken
- Current visible source evidence does not confirm every helper API named in the prior contract, so additive public surface or snapshot churn may be needed while keeping request contracts compatible.
- PIT and bridge reads depend on correctness-sensitive ordering and snapshot semantics; provider-specific SQL that changes tie-breaking, filtering, or depth handling would create subtle regressions.
- Repository benchmark and documentation baselines currently distinguish only provider-neutral PIT and bridge reads; over-claiming optimization before artifact-backed evidence lands would regress release-note accuracy.
- Non-SQLite provider proof remains optional and consumer-managed, so expansion beyond SQLite may lag.
- Split recommendation: If implementation grows, split first into one common dispatch and diagnostics slice and two execution slices: PIT optimization and bridge optimization.
- Split recommendation: If non-SQLite provider-specific PIT or bridge SQL is needed in the same release, track each external provider package in its own child ticket so SQLite proof and fallback safety are not blocked.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `50720`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0479`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `28777a64ac074b698d87f1804b621a23`
- completed-at-utc: `<redacted>-19T13:39:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGPRGN0EVGD6RY5KY9M56W/runs/20260519T133912415Z-28777a64ac074b698d87f1804b621a23.json`