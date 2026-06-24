[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FF43HQ8E0435ZZSRZQQJW1HC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43HQ8E0435ZZSRZQQJW1HC`.
- Optimistic claim succeeded (`expectedRevision=06FFF7TQJ17HFZD46X8X2JDS2C`, `currentRevision=06FFFJ74PEHZ7FTTPZN5W2AW2R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FF43HQ8E0435ZZSRZQQJW1HC-task-harden-pit-maintenance-unsupported-shape-fa' from source '82f0b28441ea6b2aa6c68399d1a3753e40848396'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FF43HQ8E0435ZZSRZQQJW1HC-task-harden-pit-maintenance-unsupported-shape-fa` as `5e9a0243b2c3`.

Open questions / Risiken
- Risky assumption: The SQL Server missing-registration case will be exercised intentionally as a provider-neutral `AddDVault()` / no-service-replacement scenario, not accidentally re-proved as mere provider mismatch; the contract's Risks section calls this out and the service-r...
- Risky assumption: PostgreSQL fallback proof will stay mostly deterministic in unit or SQLite-backed provider-neutral tests and will not rely only on opt-in live Npgsql integration, consistent with the Definition of Done.
- Split recommendation: No split recommended; the repository evidence and contract already bound the work to existing PIT maintenance test surfaces.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9058`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `d4715aa7be7343689959fc27b257b36c`
- completed-at-utc: `<redacted>-24T04:05:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43HQ8E0435ZZSRZQQJW1HC/runs/20260624T040534470Z-d4715aa7be7343689959fc27b257b36c.json`