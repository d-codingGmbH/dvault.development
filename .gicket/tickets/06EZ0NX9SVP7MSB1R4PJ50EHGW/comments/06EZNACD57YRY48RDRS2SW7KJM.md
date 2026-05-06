[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EZ0NX9SVP7MSB1R4PJ50EHGW'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06EZ0NX9SVP7MSB1R4PJ50EHGW`.
- Optimistic claim succeeded (`expectedRevision=06EZN9EJ3C9T2V1PPPXKDPZQZR`, `currentRevision=06EZN9KAX5PYZMCQ5KT34X7QF4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EZ0NX9SVP7MSB1R4PJ50EHGW-task-document-advanced-hook-validation-and-failu' from source '0802392f93c3c200d7d5b26129d53943f1ca3b0e'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EZ0NX9SVP7MSB1R4PJ50EHGW-task-document-advanced-hook-validation-and-failu` as `cf05e4df0c87`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: Finalizing :: - Finalizing concrete names for future hook APIs beyond what already exists in the branch.
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: Finalizing :: - Finalizing concrete names for future hook APIs beyond what already exists in the branch.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8937`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `a6fb27b16be740699e386b759933624e`
- completed-at-utc: `<redacted>-06T00:21:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06EZ0NX9SVP7MSB1R4PJ50EHGW/runs/20260506T002148251Z-a6fb27b16be740699e386b759933624e.json`