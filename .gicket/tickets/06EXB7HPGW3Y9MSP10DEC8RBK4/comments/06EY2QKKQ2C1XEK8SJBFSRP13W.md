[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EXB7HPGW3Y9MSP10DEC8RBK4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7HPGW3Y9MSP10DEC8RBK4`.
- Optimistic claim succeeded (`expectedRevision=06EY2P9G80T8B6Q77REFMCCHMW`, `currentRevision=06EY2PD43SAEYN75GAF0K1S06W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7HPGW3Y9MSP10DEC8RBK4-task-persist-satellite-rows-only-when-hash-diff' from source '701dfbb07bb65b4a4204587693417edfe0d35642'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EXB7HPGW3Y9MSP10DEC8RBK4-task-persist-satellite-rows-only-when-hash-diff` as `33b6c01171e9`.

Open questions / Risiken
- Risky assumption: Caller/domain code will supply stable HashDiff values consistently across producers; the ticket acknowledges this risk and docs/plans/stable-hashing-contract.md:47-74 keeps payload field selection outside the shared hash service.
- Risky assumption: Caller-provided LoadTimestamp values are usable for historization ordering per parent, because the existing satellite schema keys history by parent hash key plus load timestamp.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9238`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `54cdf184612b46db934eb786f4a98136`
- completed-at-utc: `<redacted>-01T02:29:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7HPGW3Y9MSP10DEC8RBK4/runs/20260501T022916236Z-54cdf184612b46db934eb786f4a98136.json`