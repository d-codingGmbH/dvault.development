[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NSXY2Y1JZ8SSCX177C770'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NSXY2Y1JZ8SSCX177C770`.
- Optimistic claim succeeded (`expectedRevision=06EZT6N7JYQ4D1DN426ZPHGE6C`, `currentRevision=06EZT6XQNC7NTRYFJYXR9QDX7W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06EZ0NSXY2Y1JZ8SSCX177C770': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NSXY2Y1JZ8SSCX177C770': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NSXY2Y1JZ8SSCX177C770-story-add-pit-table-modeling-and-generation' from source 'ed1605847501e64f190ed44b8c0687fdf40c7939'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EZ0NSXY2Y1JZ8SSCX177C770-story-add-pit-table-modeling-and-generation` as `e8207e2c08e5`.

Open questions / Risiken
- If docs or examples mix `LoadTimestamp` and `PitLoadTimestamp`, consumers may assume the two public PIT surfaces are interchangeable even though this story intentionally treats them as separate.
- Because this baseline is metadata-only, consumers may assume PIT rows are automatically maintained unless the docs explicitly say population and refresh are deferred.
- The no-relationship, no-secondary-index baseline may be functionally correct but still insufficient for real read workloads until later optimization tickets land.
- Users may over-assume PIT coverage unless the ticket and docs explicitly call out that link-based and multi-active scenarios are unsupported in this story.
- Split recommendation: Keep PIT metadata projection, canonical `DataVaultPitMetadata` examples, and documentation in this story, but reserve PIT row population or refresh orchestration for a separate follow-up ticket.
- Split recommendation: If public PIT API cleanup becomes material, split consolidation or deprecation of `DataVaultPointInTimeMetadata` / `PointInTime(...)` versus `DataVaultPitMetadata` / `DataVaultMetadataModel.Pits` into its own public-surface ticket rather than expanding th...
- Split recommendation: Handle provider-specific PIT indexing or physical optimization in provider-owned follow-up tickets once the shared metadata baseline is stable.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `53981`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0451`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `bfb1a25c76c143fa8966dd5da5c3bafd`
- completed-at-utc: `<redacted>-06T11:52:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NSXY2Y1JZ8SSCX177C770/runs/20260506T115205117Z-bfb1a25c76c143fa8966dd5da5c3bafd.json`