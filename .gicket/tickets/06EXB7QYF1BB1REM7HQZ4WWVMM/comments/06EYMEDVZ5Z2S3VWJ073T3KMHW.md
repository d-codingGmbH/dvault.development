[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EXB7QYF1BB1REM7HQZ4WWVMM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7QYF1BB1REM7HQZ4WWVMM`.
- Optimistic claim succeeded (`expectedRevision=06EYM6CJJKGG1YK95C41G16RWW`, `currentRevision=06EYMDBZTJ73HJ5F9R6W7RM87R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7QYF1BB1REM7HQZ4WWVMM-story-write-getting-started-documentation' from source 'f62c2d9b3b56e8a2b040b1166ad68605743b8a88'.
- Interactive PO-critic tool loop fell back to legacy critic review after MODEL-TOOL-INVOCATION-RESULT-TOOL-CALL-ARGUMENTS-JSON-INVALID.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EXB7QYF1BB1REM7HQZ4WWVMM-story-write-getting-started-documentation` as `e7a6a1504c91`.

Open questions / Risiken
- Risky assumption: Downstream runtime will treat this approval as aggregate close/advance handling for the umbrella story, not as a request to reopen parent-only development, even though the current parent ticket metadata is still stale.
- Split recommendation: No additional split recommended; the existing parentOf children and the separate blocked follow-up story 06EXB8202A88KJJP7WEGBESBYM already cover the remaining planning boundaries.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8631`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `1a0a2082412047c2afda2aba826dbe53`
- completed-at-utc: `<redacted>-02T19:45:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7QYF1BB1REM7HQZ4WWVMM/runs/20260502T194544807Z-1a0a2082412047c2afda2aba826dbe53.json`