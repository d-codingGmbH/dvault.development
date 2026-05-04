[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EZ0NBH3YWJPF05AQWC0E6GV4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NBH3YWJPF05AQWC0E6GV4`.
- Optimistic claim succeeded (`expectedRevision=06EZ42TNFQ4JY9KG2WV3KV9C78`, `currentRevision=06EZ42XYR7C4H36YYTQES7DPAG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EZ0NBH3YWJPF05AQWC0E6GV4-task-add-oracle-opt-in-integration-configuration' from source 'e49ad83e953f8d843c582d9c1027499cf40cdff1'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EZ0NBH3YWJPF05AQWC0E6GV4-task-add-oracle-opt-in-integration-configuration` as `ab2a8609a5c5`.

Open questions / Risiken
- Risky assumption: The live Oracle smoke can obtain a compatible Oracle EF Core provider in the test project without forcing a dependency into `src/DCoding.Data.DVault.Oracle`.
- Risky assumption: The developer-managed Oracle account used for opt-in runs will have enough privileges to create and clean up the objects needed for the one-hub smoke scenario.
- Risky assumption: Oracle can mirror the existing Postgres and SQLite public-contract assertions without provider-specific behavior forcing a broader ticket scope.
- Split recommendation: No split needed. Oracle optimized-writer and provider-capability work is already separated into ticket `06EZ0NBAP31G489S3YXXYY54WM`, leaving this ticket cleanly scoped to opt-in configuration, one live smoke, and documentation.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9322`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `ead45d860ebf40e39794a7e49726e271`
- completed-at-utc: `<redacted>-04T08:18:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NBH3YWJPF05AQWC0E6GV4/runs/20260504T081808832Z-ead45d860ebf40e39794a7e49726e271.json`