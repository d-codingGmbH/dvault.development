[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGNGVQ3TZZWSABAK5SNFK4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGNGVQ3TZZWSABAK5SNFK4`.
- Optimistic claim succeeded (`expectedRevision=06F2PNM25BTS0M9ERMSFJ23S9G`, `currentRevision=06F3P0WPHYA2755RWW1H0S46E0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGNGVQ3TZZWSABAK5SNFK4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGNGVQ3TZZWSABAK5SNFK4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGNGVQ3TZZWSABAK5SNFK4-story-add-provider-native-bulk-ingestion-strateg' from source 'e286da888a4b75061332a3cdf85dce5547db03a2'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGNGVQ3TZZWSABAK5SNFK4-story-add-provider-native-bulk-ingestion-strateg` as `bc6151f5cc7d`.

Open questions / Risiken
- Provider-specific behavior can drift from the documented gates if strategy CanSave logic, diagnostics fallback-cause reporting, and provider tests stop evolving together.
- SQL Server, MySQL, and Oracle are intentionally shape-gated; undersized or dirty batches will correctly fall back, but consumers may misread that as missing optimization unless the docs task explains the gates clearly.
- Oracle support is still bounded to eligible ordinary satellite batches and excludes multi-active satellite shapes; widening that boundary later will require dedicated proof rather than silent expansion.
- Split recommendation: No additional split is recommended; the current relation graph already separates SPI, fallback, native strategy implementation, live provider coverage, benchmarks, and docs.
- Split recommendation: If future work adds streaming ingestion, multi-active native support, or broader provider-decline observability beyond the current diagnostics model, create separate follow-on tickets instead of widening this story.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9158`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `c7ac0ed948bf4f3185527d59d316f0c5`
- completed-at-utc: `<redacted>-18T12:24:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGNGVQ3TZZWSABAK5SNFK4/runs/20260518T122417271Z-c7ac0ed948bf4f3185527d59d316f0c5.json`