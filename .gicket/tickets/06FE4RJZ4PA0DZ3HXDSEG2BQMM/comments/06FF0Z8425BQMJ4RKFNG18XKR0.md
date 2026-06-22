[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FE4RJZ4PA0DZ3HXDSEG2BQMM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4RJZ4PA0DZ3HXDSEG2BQMM`.
- Optimistic claim succeeded (`expectedRevision=06FF0THB73ETB0ABYMY2MG30W4`, `currentRevision=06FF0XH0G2RWMZWV5MH37QAM4C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel' from source '9ae8dcc3d9333b3921445ac4c585a8045da79db0'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel` as `ba2b7f9c4e9f`.

Open questions / Risiken
- Blocking finding: The ticket requires tests for `failure or cancellation cleanup behavior`, but it never states the expected post-failure outcome. From the observed baseline, `DefaultDataVaultPitMaintenanceService.RebuildAsync(...)` deletes PIT rows with `ExecuteDeleteAsync` a...
- Required PO action: Add one explicit cleanup rule for the SQL Server prototype failure/cancellation path. Example decision points: `rebuild is atomic and pre-existing PIT rows remain intact on failure/cancellation`, or `temporary/staging SQL artifacts must be cleaned up but PI...
- Required PO action: Update the acceptance criteria and test language to match that rule so the expected verification surface is concrete rather than inferred from current implementation details.
- Risky assumption: Assuming the SQL Server path should strengthen cleanup semantics beyond the provider-neutral baseline without PO approval.
- Risky assumption: Assuming any one of activity tracing, diagnostics, or execution detail is equally acceptable for fallback-cause observability without naming the preferred surface.
- Split recommendation: Keep benchmark evidence or public performance-promotion work in a follow-up ticket, as the current contract already recommends.
- Split recommendation: Keep multi-active and link-parent PIT rebuild optimization split from this ordinary hub-parent SQL Server prototype.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8905`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `c3917ce334f04f30be70fc707918aedc`
- completed-at-utc: `<redacted>-22T17:59:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4RJZ4PA0DZ3HXDSEG2BQMM/runs/20260622T175945422Z-c3917ce334f04f30be70fc707918aedc.json`