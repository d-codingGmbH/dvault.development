[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F8KZK2MSFQP9G2DBM61ZVGD4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZK2MSFQP9G2DBM61ZVGD4`.
- Optimistic claim succeeded (`expectedRevision=06F8ZVNJCJQCGD25PEGMMNPCKC`, `currentRevision=06F8ZVW94K7G2CEV1QSBDT1NN4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZK2MSFQP9G2DBM61ZVGD4-task-add-provider-read-benchmark-rows-and-verifi' from source 'c9bbfa5020e1849260decf55ef5ede0f8edb2fae'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F8KZK2MSFQP9G2DBM61ZVGD4-task-add-provider-read-benchmark-rows-and-verifi` as `df19e163e961`.

Open questions / Risiken
- Risky assumption: Assumes optional-provider read rows are required only where a provider-specific read surface already exists; the direct source boundary today is SQLite latest/PIT/bridge plus non-SQLite PIT/bridge, not non-SQLite latest-satellite.
- Risky assumption: Assumes skipped optional-provider rows can be surfaced without changing the shared artifact schema, because the existing contract already covers executionStatus, skipReason, iterations=0, executionDetail, and persistedOutcome=not executed.
- Risky assumption: Assumes the stale blocked-by wording in the ticket description is informational only, since the persisted ticket state has isBlocked=false and the upstream blocker is already done.
- Split recommendation: No split recommended; the repository already isolates this as one harness/verifier/evidence task separate from provider implementation and broader documentation work.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8778`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `8dc0d1ca703540b78045c7f98667dabb`
- completed-at-utc: `<redacted>-04T00:12:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZK2MSFQP9G2DBM61ZVGD4/runs/20260604T001216960Z-8dc0d1ca703540b78045c7f98667dabb.json`