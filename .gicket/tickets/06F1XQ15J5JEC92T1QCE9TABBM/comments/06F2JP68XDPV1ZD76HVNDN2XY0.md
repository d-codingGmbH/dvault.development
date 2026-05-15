[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XQ15J5JEC92T1QCE9TABBM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F1XQ15J5JEC92T1QCE9TABBM`.
- Optimistic claim succeeded (`expectedRevision=06F2JNCWRJBVXP5GA4NSJK53T8`, `currentRevision=06F2JNGFTHE27SB2EHGKPVPCF0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XQ15J5JEC92T1QCE9TABBM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XQ15J5JEC92T1QCE9TABBM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XQ15J5JEC92T1QCE9TABBM-story-add-dvault-roslyn-analyzer-package-foundat' from source 'd29a934581e4f3ef56cb282ebaa10433128d20c7'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F1XQ15J5JEC92T1QCE9TABBM-story-add-dvault-roslyn-analyzer-package-foundat` as `89154cf570c5`.

Open questions / Risiken
- Analyzer packaging may still be incomplete if IsPackable remains false without a documented package-boundary rationale.
- Roslyn analyzer distribution details can be easy to under-specify; pack output should be inspected so analyzer assemblies land under analyzer assets rather than only as normal library references.
- Broader rules such as missing business keys can become noisy unless they are limited to high-confidence fluent scopes.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `41104`
- cached-tokens: `12160`
- effective-cache-ratio: `0.2958`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `af00c553f79d408bad11f1aff8fce1c4`
- completed-at-utc: `<redacted>-15T01:55:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F1XQ15J5JEC92T1QCE9TABBM/runs/20260515T015544480Z-af00c553f79d408bad11f1aff8fce1c4.json`