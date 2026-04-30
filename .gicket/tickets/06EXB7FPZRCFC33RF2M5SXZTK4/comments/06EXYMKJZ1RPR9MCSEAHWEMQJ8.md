[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EXB7FPZRCFC33RF2M5SXZTK4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7FPZRCFC33RF2M5SXZTK4`.
- Optimistic claim succeeded (`expectedRevision=06EXYKCJ4N4XNG2ZGC7MMW2FB8`, `currentRevision=06EXYKG4D8X7VA76NEZK0YSPEW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7FPZRCFC33RF2M5SXZTK4-task-add-modelbuilder-extension-for-dvault-conve' from source '40a8d013225bd3cf6cae6f2685e01c7bffca8f66'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EXB7FPZRCFC33RF2M5SXZTK4-task-add-modelbuilder-extension-for-dvault-conve` as `02efdd3c0ca2`.

Open questions / Risiken
- Risky assumption: Implementation will need to choose a net10-compatible EF Core package/version even though the contract intentionally leaves the exact package id/version to repository-aligned developer judgment.
- Risky assumption: Consumers may call `UseDataVault()` from multiple places in `OnModelCreating`; the contract assumes overwrite/idempotent behavior will be handled consistently even though repeated-call semantics are not spelled out.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7526`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `101345ddcc13436c93f415e370e92a33`
- completed-at-utc: `<redacted>-30T16:56:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7FPZRCFC33RF2M5SXZTK4/runs/20260430T165655181Z-101345ddcc13436c93f415e370e92a33.json`