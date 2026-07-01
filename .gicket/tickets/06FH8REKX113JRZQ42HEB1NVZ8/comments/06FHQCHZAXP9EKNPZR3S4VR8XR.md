[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FH8REKX113JRZQ42HEB1NVZ8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8REKX113JRZQ42HEB1NVZ8`.
- Optimistic claim succeeded (`expectedRevision=06FHQAG1J4EVV7MKQ56DX646RM`, `currentRevision=06FHQAXAMPNDVG1D9R9QQV5K84`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FH8REKX113JRZQ42HEB1NVZ8-task-record-provider-parity-benchmark-evidence-a' from source 'baeefe25a726f4646beebed7ecd0da6231e1d715'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FH8REKX113JRZQ42HEB1NVZ8-task-record-provider-parity-benchmark-evidence-a` as `370e064c16c6`.

Open questions / Risiken
- Risky assumption: The handoff assumes developers will treat the delivery-contract block as authoritative over the legacy draft text at the bottom of the description, which still says 'Run or collect the benchmark evidence'.
- Risky assumption: The handoff also assumes this ticket can be completed as documentation/evidence ratification even though the current branch diff versus develop contains only ticket metadata updates and no repository doc edits yet.
- Split recommendation: No further split is needed for save/latest-satellite/PIT/bridge evidence publication in this ticket; keep the current save/read/documentation split intact.
- Split recommendation: If the team wants more work now, create at most one separate DB2 PIT maintenance child limited to the IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync lane through IDataVaultProviderPitMaintenanceStrategy.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9416`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `4fbc77f20bf8410ea556cc1a60cdba3a`
- completed-at-utc: `<redacted>-01T03:21:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8REKX113JRZQ42HEB1NVZ8/runs/20260701T032134291Z-4fbc77f20bf8410ea556cc1a60cdba3a.json`