[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEFX5M9V9SA25N76CPGT4M'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F0MEFX5M9V9SA25N76CPGT4M`.
- Optimistic claim succeeded (`expectedRevision=06F0QH3JC4SGFQQWW4P8TQ88RM`, `currentRevision=06F1S6RV9BMGVFB6KJ4YXJ5070`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEFX5M9V9SA25N76CPGT4M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEFX5M9V9SA25N76CPGT4M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEFX5M9V9SA25N76CPGT4M-task-add-model-diff-and-drift-report-for-generat' from source 'f3752058743f9792f2e2ddc8f2a439de3fd39b6c'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If some generated metadata paths do not yet annotate keys or indexes with enough DVault-owned information, implementation may need narrowly scoped metadata extraction from EF Core key/index APIs while keeping output deterministic.
- Provider capability comparison must avoid overclaiming provider-specific DDL guarantees because the v1 contract is provider-neutral and the visible baseline is annotation/profile metadata rather than live schema inspection.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `25496`
- cached-tokens: `12160`
- effective-cache-ratio: `0.4769`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `af327d37867e41ed95cbb8b137463adb`
- completed-at-utc: `<redacted>-12T14:38:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F0MEFX5M9V9SA25N76CPGT4M/runs/20260512T143804590Z-af327d37867e41ed95cbb8b137463adb.json`