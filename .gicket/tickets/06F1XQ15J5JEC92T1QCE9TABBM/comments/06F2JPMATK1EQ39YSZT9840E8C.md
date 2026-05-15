[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F1XQ15J5JEC92T1QCE9TABBM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F1XQ15J5JEC92T1QCE9TABBM`.
- Optimistic claim succeeded (`expectedRevision=06F2JP7QQZRGC2W04JRC2VDN74`, `currentRevision=06F2JPDMSRRJZ2WJJHHC3M7JY0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F1XQ15J5JEC92T1QCE9TABBM-story-add-dvault-roslyn-analyzer-package-foundat' from source '902951a5e5219d95888fefd1e03a347415707f7b'.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F1XQ15J5JEC92T1QCE9TABBM-story-add-dvault-roslyn-analyzer-package-foundat` as `0c11aabeb1f1`.

Open questions / Risiken
- Blocking finding: PO-critic cannot approve from prompt seed context alone; direct ticket/comment state and repository/branch evidence are required by the task instructions.
- Required PO action: Re-run the PO-critic review with access to the declared local tools so the reviewer can verify the latest persisted ticket, comments, branch history, analyzer project packaging metadata, solution inclusion, tests, and documentation guidance.
- Risky assumption: Assuming the prompt snapshot reflects current repository state would violate the instruction to treat seed branch snapshots as context rather than the source of truth.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `29389`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0828`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `13b6fa278ce3490c91fa6ee36fb713dd`
- completed-at-utc: `<redacted>-15T01:57:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F1XQ15J5JEC92T1QCE9TABBM/runs/20260515T015740041Z-13b6fa278ce3490c91fa6ee36fb713dd.json`