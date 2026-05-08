[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EZ0NWCA6NEZH8VBJNGW4FVHG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NWCA6NEZH8VBJNGW4FVHG`.
- Optimistic claim succeeded (`expectedRevision=06F037D0KM1XZ74CAJ1R47NZJR`, `currentRevision=06F039M629KN6P2Z9YXGFKSK5W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EZ0NWCA6NEZH8VBJNGW4FVHG-task-add-multi-active-satellite-docs-and-tests' from source 'ba31c1d7dfefd28ec7995b401f613431d79d03ea'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EZ0NWCA6NEZH8VBJNGW4FVHG-task-add-multi-active-satellite-docs-and-tests` as `d665e198f3b7`.

Open questions / Risiken
- Blocking finding: No outgoing parentOf child tickets were found for the tracking-only parent ticket.
- Required PO action: Resolve the tracking-parent closure audit findings before this parent ticket can be closed.
- Risky assumption: The contract leaves the durable-doc destination open; implementation needs a non-planning repo surface so the explanation does not remain only under docs/plans.
- Split recommendation: No split recommended; the persisted contract is already bounded to durable docs plus ratification or extension of existing suites, and the prerequisite contract and persistence slices are done.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9310`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `704b5da67f31467ebbd1d008084c4028`
- completed-at-utc: `<redacted>-07T09:02:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NWCA6NEZH8VBJNGW4FVHG/runs/20260507T090245839Z-704b5da67f31467ebbd1d008084c4028.json`