[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FF43FQ8NRX04T9HZHBMFS0PC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43FQ8NRX04T9HZHBMFS0PC`.
- Optimistic claim succeeded (`expectedRevision=06FFECPYBT4WPTPBA9Q2DVK3PG`, `currentRevision=06FFEGHQ6YMCFVJAS3NKB9NQSW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FF43FQ8NRX04T9HZHBMFS0PC-task-add-postgresql-pit-maintenance-selected-fal' from source '726d8e376d8c7b269df3ffc7d3c61a6b979006c4'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FF43FQ8NRX04T9HZHBMFS0PC-task-add-postgresql-pit-maintenance-selected-fal` as `6e0dd8ffd0fa`.

Open questions / Risiken
- Risky assumption: The ticket assumes the existing maintenance Activity surface can carry the needed PostgreSQL selected/fallback facts without reopening scope into a new public diagnostics API; repository evidence supports that assumption today through `DataVaultMaintenanceAct...
- Risky assumption: The ticket assumes fallback capture will reuse `DataVaultProviderPitMaintenanceStrategyGateEvaluator` rather than ad hoc strings; if implementation diverges, the finite-vocabulary requirement could erode.
- Split recommendation: No further split is needed now; documentation, benchmark, comparator, and transaction follow-up scope is already parked on sibling tickets called out in the contract.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9241`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `ee38d617f5bd465e8d6619e59490ca70`
- completed-at-utc: `<redacted>-24T01:40:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43FQ8NRX04T9HZHBMFS0PC/runs/20260624T014048644Z-ee38d617f5bd465e8d6619e59490ca70.json`