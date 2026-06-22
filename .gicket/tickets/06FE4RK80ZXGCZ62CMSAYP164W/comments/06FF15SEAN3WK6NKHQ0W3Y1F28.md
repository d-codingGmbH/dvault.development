[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FE4RK80ZXGCZ62CMSAYP164W'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4RK80ZXGCZ62CMSAYP164W`.
- Optimistic claim succeeded (`expectedRevision=06FF133GXD75PPBY84B515W0CG`, `currentRevision=06FF13C5D5B93Y70XG11WCMCMC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FE4RK80ZXGCZ62CMSAYP164W-task-evaluate-bridge-rebuild-push-down-feasibili' from source '6a5ffc586faa59993829679925d02c6f6b3d25c3'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FE4RK80ZXGCZ62CMSAYP164W-task-evaluate-bridge-rebuild-push-down-feasibili` as `890cdbd01996`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: Reopen :: - Reopen PIT prototype tickets or bridge read optimization evidence that already exists for maintained bridge rows.
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: Reopen :: - Reopen PIT prototype tickets or bridge read optimization evidence that already exists for maintained bridge rows.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7889`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `9bafbac162944f4f8c337620a8e1be12`
- completed-at-utc: `<redacted>-22T18:28:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4RK80ZXGCZ62CMSAYP164W/runs/20260622T182820178Z-9bafbac162944f4f8c337620a8e1be12.json`