[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q8Z72K8AV0755BE571CG04'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q8Z72K8AV0755BE571CG04`.
- Optimistic claim succeeded (`expectedRevision=06F5Q980ZM6SYVWMWPNNTCQNAR`, `currentRevision=06F61EPQ984SJ7JYHGXGN0NHNG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q8Z72K8AV0755BE571CG04': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q8Z72K8AV0755BE571CG04': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q8Z72K8AV0755BE571CG04-story-implement-sql-server-staged-bulk-save-stra' from source '555f1ce4e471aea887f320883ebfe460def22ac4'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q8Z72K8AV0755BE571CG04-story-implement-sql-server-staged-bulk-save-stra` as `5a292203c3fc`.

Open questions / Risiken
- Because live SQL Server execution is opt-in, the staged path can be under-validated in unattended environments unless a configured SQL Server lane runs during CI or release validation.
- The staged path crosses temporary tables and bulk-transfer boundaries while still needing exact latest-state and hash-diff correctness; regressions here would be data-correctness issues, not only performance issues.
- Cleanup behavior under cancellation or mid-batch failure is a concentrated risk area because staging artifacts and caller-owned transaction behavior must remain consistent.
- Split recommendation: No split is recommended from the current evidence; provider-specific ordered-bulk staging, SQL Server-gated tests, and benchmark-lane evidence still fit one bounded story.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `31177`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0780`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `04d306c5ba0448dd9576acf24de637a4`
- completed-at-utc: `<redacted>-25T20:10:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q8Z72K8AV0755BE571CG04/runs/20260525T201036162Z-04d306c5ba0448dd9576acf24de637a4.json`