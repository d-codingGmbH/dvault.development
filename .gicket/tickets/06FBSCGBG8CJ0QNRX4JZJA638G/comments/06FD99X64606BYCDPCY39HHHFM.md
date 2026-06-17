[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FBSCGBG8CJ0QNRX4JZJA638G'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCGBG8CJ0QNRX4JZJA638G`.
- Optimistic claim succeeded (`expectedRevision=06FD6D597R7NT20NT44DZ56V70`, `currentRevision=06FD97QB46SRMVAESM34C67QA4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FBSCGBG8CJ0QNRX4JZJA638G-task-audit-provider-pit-and-bridge-read-gaps' from source '74a911ae1edf02d0b15c9a1ddcbd0bd302442e55'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Cleared stale blocked label(s) during successful handoff: blocked/dev, blocked/test.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FBSCGBG8CJ0QNRX4JZJA638G-task-audit-provider-pit-and-bridge-read-gaps` as `032b21b24178`.

Open questions / Risiken
- Risky assumption: Approval assumes the child owner-branch refs inspected with `git show ticket/...` are the authoritative persisted state for the child tickets; the stale child descriptions visible on the current parent branch are not.
- Split recommendation: Use child `06FBSCGGN528A2NC6TTA5A99X0` for PostgreSQL provider-configured PIT and bridge timing evidence against `PostgresDataVaultReadStrategy`.
- Split recommendation: Use child `06FBSCGNY2R6PC7P4Y91RD0HVR` for SQL Server provider-configured PIT and bridge timing evidence against `SqlServerDataVaultReadStrategy`.
- Split recommendation: Use child `06FBSCGVAZ5G8NP1TRXFNEP6DW` for MySQL provider-configured PIT and bridge timing evidence against `MySqlDataVaultReadStrategy`.
- Split recommendation: Use child `06FBSCH0M358R5J3RGFB6GRDM4` for Oracle provider-configured PIT and bridge timing evidence against `OracleDataVaultReadStrategy`.
- Split recommendation: Use child `06FBSCH65R88BT6PS7XV32NQ1M` only as deferred DB2 planning until explicit DB2 evidence scope and environment-backed benchmark work are approved.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9195`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `69dcfd90fde6420cb666673acd968f95`
- completed-at-utc: `<redacted>-17T08:16:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCGBG8CJ0QNRX4JZJA638G/runs/20260617T081657350Z-69dcfd90fde6420cb666673acd968f95.json`