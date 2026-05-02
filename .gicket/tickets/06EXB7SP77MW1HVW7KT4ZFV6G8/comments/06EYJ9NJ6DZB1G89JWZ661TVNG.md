[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EXB7SP77MW1HVW7KT4ZFV6G8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7SP77MW1HVW7KT4ZFV6G8`.
- Optimistic claim succeeded (`expectedRevision=06EYJ8QADYS19T6AYXHRV0FRAW`, `currentRevision=06EYJ8V15XSM1REF9XCQF41HKG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7SP77MW1HVW7KT4ZFV6G8-task-implement-normal-ef-baseline-for-order-prod' from source '0257c0f7f59190ca96bd7dc10ce123d494a9b9f1'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EXB7SP77MW1HVW7KT4ZFV6G8-task-implement-normal-ef-baseline-for-order-prod` as `c4122b4c58ca`.

Open questions / Risiken
- Risky assumption: Assumes the developer will choose a line payload and seed dataset that the blocked follow-up ticket 06EXB7SY3J6160R9Q35CFN6Q1W can mirror, because the ticket leaves the exact payload field open.
- Risky assumption: Assumes a test-only artifact is sufficient for stakeholders because `README.md` and `examples/.gitkeep` still position runnable examples as future work.
- Split recommendation: No split recommended; the current contract is already bounded to one conventional EF Core Sqlite baseline in the existing integration-test surface.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8122`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `90ad22a4f14449b6b74ed1d8152f85a3`
- completed-at-utc: `<redacted>-02T14:45:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7SP77MW1HVW7KT4ZFV6G8/runs/20260502T144519862Z-90ad22a4f14449b6b74ed1d8152f85a3.json`