[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XPYA9MD0T9C4651ND8KX0W'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F1XPYA9MD0T9C4651ND8KX0W`.
- Optimistic claim succeeded (`expectedRevision=06F1XTPRCMER74T88MBT51Y2NM`, `currentRevision=06F2695GC2ETHS45WT1QQRQ5YR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XPYA9MD0T9C4651ND8KX0W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XPYA9MD0T9C4651ND8KX0W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XPYA9MD0T9C4651ND8KX0W-story-prove-compiled-model-and-compiled-query-co' from source '486095b6aab2291666b8f1846e28190cb460997e'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- EF Core compiled-model internals can vary by EF version, so documentation should describe the tested UseModel/runtime-model boundary instead of making broader tooling claims.
- Compiled queries are only useful for stable query shapes; over-promising support for dynamic DVault read requests would create misleading performance expectations.
- Benchmark timing claims without archived artifacts and provider context would be fragile and should be omitted.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7039`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `8851364c165c4809927376c79188429f`
- completed-at-utc: `<redacted>-13T21:06:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F1XPYA9MD0T9C4651ND8KX0W/runs/20260513T210605520Z-8851364c165c4809927376c79188429f.json`