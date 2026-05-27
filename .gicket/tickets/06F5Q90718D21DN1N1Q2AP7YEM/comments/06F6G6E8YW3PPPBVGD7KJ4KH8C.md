[gicket-bot] Run report (outcome: dev-workflow-failed)

Summary
- Developer workflow for ticket '06F5Q90718D21DN1N1Q2AP7YEM' cannot continue because the implementation plan does not persist the ticket-side decision artifact required by the contract.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q90718D21DN1N1Q2AP7YEM`.
- Optimistic claim succeeded (`expectedRevision=06F6G5E4CJ0ZR17TGRZ0B9WEBG`, `currentRevision=06F6G5PWM7SMS418G5PMSXJDM0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.

Open questions / Risiken
- Expected persisted ticket artifact(s): [description].
- The current developer plan does not include a matching ticket_artifacts entry with concrete markdown content.

Next steps
- Update the developer plan so it writes the required ticket comment or description content instead of only describing the intended documentation outcome.
- Retry the developer workflow after aligning the implementation plan with the persisted ticket artifact expectations.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8948`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `dev`
- run-id: `40a17152283a47c5ad3d1a0845fb8759`
- completed-at-utc: `<redacted>-27T06:23:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q90718D21DN1N1Q2AP7YEM/runs/20260527T062300852Z-40a17152283a47c5ad3d1a0845fb8759.json`