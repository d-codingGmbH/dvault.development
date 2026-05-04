[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EZ0NC3VNZ5FP9XDYVX9DHW1G'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NC3VNZ5FP9XDYVX9DHW1G`.
- Optimistic claim succeeded (`expectedRevision=06EZ4A7N86ZCE6PKE8H3P0TT38`, `currentRevision=06EZ4AB24A5C8W9H7T128ZJQ1G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EZ0NC3VNZ5FP9XDYVX9DHW1G-task-add-mysql-opt-in-integration-configuration' from source 'b6be589aaad77dec6772427d50fa07f3901a9cbf'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EZ0NC3VNZ5FP9XDYVX9DHW1G-task-add-mysql-opt-in-integration-configuration` as `2d58049fb9f3`.

Open questions / Risiken
- Blocking finding: The contract requires a live MySQL-backed `DbContext`, but it never names the EF Core MySQL provider package or its setup contract. That is implementation-critical in this repo because the only proven external opt-in pattern is Postgres-specific (`Npgsql.Enti...
- Blocking finding: The ticket does not state whether MySQL should mirror the Postgres conditional-restore behavior when the env var is set but the provider package is unavailable. The current Postgres path has explicit skip behavior in `NpgsqlProviderReflection.cs`, but the MyS...
- Required PO action: Name the exact EF Core MySQL provider/package this ticket must use and make that choice part of the durable contract.
- Required PO action: Specify the expected MySQL `DbContext` setup contract for this repo, including any required server-version handling or equivalent provider-specific bootstrap.
- Required PO action: Clarify whether the MySQL path must mirror the Postgres conditional package-restore and missing-provider skip behavior when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is set.
- Risky assumption: A developer can choose any MySQL EF Core provider without affecting test wiring, restore behavior, or README guidance.
- Risky assumption: The chosen provider can exercise the bounded insert-only explicit-save scenario through the provider-neutral fallback writer without extra MySQL-specific prerequisites beyond a connection string.
- Split recommendation: Keep MySQL-specific optimized save behavior or capability-profile work in a separate follow-up ticket if scope grows beyond one compatibility-path smoke test.
- Split recommendation: Keep repository-managed MySQL provisioning or always-on CI automation separate from this ticket's documentation and opt-in test-contract scope.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9357`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `f6b2d71e2cc64e8f9b01bfc2aa24d44b`
- completed-at-utc: `<redacted>-04T08:49:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NC3VNZ5FP9XDYVX9DHW1G/runs/20260504T084959194Z-f6b2d71e2cc64e8f9b01bfc2aa24d44b.json`