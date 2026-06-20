[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FE4QPR8TF8R6PXNM3RMXN8JG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4QPR8TF8R6PXNM3RMXN8JG`.
- Optimistic claim succeeded (`expectedRevision=06FE6GS0P6FXE1D8XZC4YB9TXR`, `currentRevision=06FE6MB84Z5A590ZN8ACHJEB3R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FE4QPR8TF8R6PXNM3RMXN8JG-task-tune-postgresql-latest-satellite-strategy-w' from source '00c94a92f7475a7fdc349c8761fc9710c7b3294e'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FE4QPR8TF8R6PXNM3RMXN8JG-task-tune-postgresql-latest-satellite-strategy-w` as `6eae61d3ca26`.

Open questions / Risiken
- Blocking finding: The authoritative delivery contract contains a factual contradiction about this pass: the Implementation Notes say no description updates were materialized, but the persisted PO run report in .gicket/tickets/06FE4QPR8TF8R6PXNM3RMXN8JG/comments/06FE6GM93A8GZBY...
- Risky assumption: The contract assumes equivalent preserved benchmark comparison is sufficiently concrete for developer handoff even if the preserved evidence is not the standard provider-configured artifact triplet.
- Risky assumption: The contract assumes the historical incoming blocks link from done ticket 06FE4QP6FB892E7TJMB47A3MSR remains housekeeping only and will not be misread as an active prerequisite during execution.
- Split recommendation: No additional split is needed once the contract wording is corrected; the existing downstream documentation ticket 06FE4QRMXVGJVA65ZR5MZ817K8 already holds broader matrix and release-note promotion work.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7559`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `98ff9f49198342bda9925296e8d0adf3`
- completed-at-utc: `<redacted>-20T04:42:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4QPR8TF8R6PXNM3RMXN8JG/runs/20260620T044254462Z-98ff9f49198342bda9925296e8d0adf3.json`