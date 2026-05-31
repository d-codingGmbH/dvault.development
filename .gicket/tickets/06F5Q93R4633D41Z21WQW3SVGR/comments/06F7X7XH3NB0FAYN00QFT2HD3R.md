[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F5Q93R4633D41Z21WQW3SVGR'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q93R4633D41Z21WQW3SVGR`.
- Optimistic claim succeeded (`expectedRevision=06F7X62D07EJWWXR3SEC61EJFR`, `currentRevision=06F7X6BKZK877EW7N7QHCWG2W4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F5Q93R4633D41Z21WQW3SVGR-epic-tracing-and-performance-guidance' from source 'b51c67a053272b099cc0440bf46c5db23a31f984'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F5Q93R4633D41Z21WQW3SVGR-epic-tracing-and-performance-guidance` as `9206205cd699`.

Open questions / Risiken
- Blocking finding: Because this review is limited to ticket-level readiness, that metadata contradiction leaves the automated handoff state ambiguous and is not clean enough for unattended progression.
- Required PO action: Rerun PO handoff after the persisted ticket metadata matches the already-verified relation graph and child-completion evidence.
- Risky assumption: Closure still assumes no new incoming relation or child-ticket reopen occurs after this review; the contract already calls for a final eligibility check before close.
- Split recommendation: No additional split recommended once the persisted metadata is aligned; the existing five-child decomposition is sufficient.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9039`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `89e9bc740e89476a9fdcb8ebcd9ddb7e`
- completed-at-utc: `<redacted>-31T15:20:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q93R4633D41Z21WQW3SVGR/runs/20260531T152055322Z-89e9bc740e89476a9fdcb8ebcd9ddb7e.json`