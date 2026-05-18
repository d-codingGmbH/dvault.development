[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGMFWSEC95ATBCGZ6HYT5W'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGMFWSEC95ATBCGZ6HYT5W`.
- Optimistic claim succeeded (`expectedRevision=06F2PNKSB1H0Z76PDCHNRF1G9C`, `currentRevision=06F3QCGB5H302MVGA1N4ZDTK8C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGMFWSEC95ATBCGZ6HYT5W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGMFWSEC95ATBCGZ6HYT5W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGMFWSEC95ATBCGZ6HYT5W-epic-provider-bulk-ingestion' from source '31eca3084183eafe2e2d93c143f5ec5408811fa3'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGMFWSEC95ATBCGZ6HYT5W-epic-provider-bulk-ingestion` as `2ecb4e1ed22b`.

Open questions / Risiken
- External-provider proof depends on opt-in environment-specific connection strings, so native provider evidence can be skipped in default local or CI runs unless those lanes are explicitly configured.
- Benchmark and throughput claims are environment-sensitive; release notes should continue to tie performance evidence to recorded machine, provider, and execution context rather than promise universal gains.
- The epic spans core contracts, provider packages, diagnostics, tests, and documentation, so downstream implementation work should stay bounded to this release contract to avoid scope creep.
- Split recommendation: No mandatory split is required for PO-critic handoff because the repository already presents one coherent v0.14.0 epic baseline.
- Split recommendation: If later execution tracking needs finer granularity, split along three bounded slices: core bulk-save contract and fallback behavior, provider-native strategy coverage and diagnostics, and documentation or benchmark evidence.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `55567`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0438`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `0da3526f44b247d980e5c09e84961203`
- completed-at-utc: `<redacted>-18T15:30:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGMFWSEC95ATBCGZ6HYT5W/runs/20260518T153020746Z-0da3526f44b247d980e5c09e84961203.json`