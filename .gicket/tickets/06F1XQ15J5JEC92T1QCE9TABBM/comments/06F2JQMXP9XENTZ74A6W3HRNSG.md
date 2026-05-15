[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XQ15J5JEC92T1QCE9TABBM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F1XQ15J5JEC92T1QCE9TABBM`.
- Optimistic claim succeeded (`expectedRevision=06F2JPNNPEEQZN77F8HZ7998BC`, `currentRevision=06F2JPRWZYX08QF7RY969WZ4N4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XQ15J5JEC92T1QCE9TABBM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XQ15J5JEC92T1QCE9TABBM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XQ15J5JEC92T1QCE9TABBM-story-add-dvault-roslyn-analyzer-package-foundat' from source '4f97b9372ff28602c98609c41edacdd6ce940c45'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Analyzer packaging may still be incomplete if IsPackable remains false without a documented package-boundary rationale.
- Roslyn analyzer distribution details can be under-specified unless pack output is inspected to confirm analyzer assemblies land under analyzer assets rather than only normal library references.
- Broader future rules such as missing business keys can become noisy unless limited to high-confidence fluent scopes.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `28955`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0840`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `6b04598b3976433788a1d94296b74efb`
- completed-at-utc: `<redacted>-15T02:02:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F1XQ15J5JEC92T1QCE9TABBM/runs/20260515T020206518Z-6b04598b3976433788a1d94296b74efb.json`