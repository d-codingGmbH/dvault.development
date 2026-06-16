[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSCA7QPNQ48K6G69K1Y8R4G'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCA7QPNQ48K6G69K1Y8R4G`.
- Optimistic claim succeeded (`expectedRevision=06FCX51RVV614WGYC7070YCR3G`, `currentRevision=06FCXV4AWJ363R2TB7YJZ7YT4M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSCA7QPNQ48K6G69K1Y8R4G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSCA7QPNQ48K6G69K1Y8R4G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSCA7QPNQ48K6G69K1Y8R4G-task-implement-accepted-postgresql-bulk-improvem' from source '8f87301ae382a4c403cb4f493ca484489bd501b2'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSCA7QPNQ48K6G69K1Y8R4G-task-implement-accepted-postgresql-bulk-improvem` as `0554c2b285b9`.

Open questions / Risiken
- gicket ticket/comment/relation reads were trust-blocked earlier in the session, so live relation metadata could not be revalidated or cleaned up in this unattended run.
- Until a later trusted ticket-write pass rewrites the ticket surface, the current implementation-style title may still mislead reviewers into expecting new developer work.
- Closure evidence must continue to cite the provider-configured v0.32 PostgreSQL bundle; the root benchmark triplet preserves PostgreSQL as skipped-placeholder when the connection string is unset.
- Split recommendation: No split for this ticket; treat the current ticket as closure-only.
- Split recommendation: If desired, open a separate housekeeping ticket for lineage or relation cleanup or a separate benchmark-evidence ticket for any fresh PostgreSQL rerun.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `52104`
- cached-tokens: `7552`
- effective-cache-ratio: `0.1449`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `96f8e486238e489e8fa7fd828a1ff55f`
- completed-at-utc: `<redacted>-16T05:42:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCA7QPNQ48K6G69K1Y8R4G/runs/20260616T054201535Z-96f8e486238e489e8fa7fd828a1ff55f.json`