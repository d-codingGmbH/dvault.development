[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F2PGGJQMKH2T5948VJH93M5R'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGGJQMKH2T5948VJH93M5R`.
- Optimistic claim succeeded (`expectedRevision=06F2S0H0TQCCWVV2PDHCEY8Q6R`, `currentRevision=06F2S0S563WQM141QWRBW3BJ5C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F2PGGJQMKH2T5948VJH93M5R-task-implement-validate-export-drift-guardrail-c' from source '6139170e93c8c4b754506e0eefaff6e2495de885'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F2PGGJQMKH2T5948VJH93M5R-task-implement-validate-export-drift-guardrail-c` as `b116ececfe88`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: No :: - No additional split is recommended once the ticket is bounded to reusable consumer-owned command runners over existing APIs; CI examples and v0.11.0 docs already remain separated in `06F2PGGR30XXCDKCZ8W2J2WX8C` and `06F...
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: No :: - No additional split is recommended once the ticket is bounded to reusable consumer-owned command runners over existing APIs; CI examples and v0.11.0 docs already remain separated in `06F2PGGR30XXCDKC...

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9072`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `1b65ea1d39254a52a8785f27c2cd4684`
- completed-at-utc: `<redacted>-15T16:46:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGGJQMKH2T5948VJH93M5R/runs/20260515T164624645Z-1b65ea1d39254a52a8785f27c2cd4684.json`