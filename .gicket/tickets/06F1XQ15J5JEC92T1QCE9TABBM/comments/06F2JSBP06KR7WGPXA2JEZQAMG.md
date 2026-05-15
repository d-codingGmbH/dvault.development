[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F1XQ15J5JEC92T1QCE9TABBM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F1XQ15J5JEC92T1QCE9TABBM`.
- Optimistic claim succeeded (`expectedRevision=06F2JQPBQ61QDSSVMTVBPHX0TM`, `currentRevision=06F2JQWHF8FAK55G37C4SKRYE8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F1XQ15J5JEC92T1QCE9TABBM-story-add-dvault-roslyn-analyzer-package-foundat' from source '819ca62dfa328b0aaf81f716454d675c5e56b001'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F1XQ15J5JEC92T1QCE9TABBM-story-add-dvault-roslyn-analyzer-package-foundat` as `0f40e7eb18e2`.

Open questions / Risiken
- Risky assumption: Analyzer packaging remains implementation work: current analyzer csproj has IsPackable=false and no package id/description/analyzer asset packing metadata.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8508`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `e70d0d7e505e4aaaa97aa225955fe1b1`
- completed-at-utc: `<redacted>-15T02:09:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F1XQ15J5JEC92T1QCE9TABBM/runs/20260515T020935568Z-e70d0d7e505e4aaaa97aa225955fe1b1.json`