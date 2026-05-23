[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F492C50WM7V2NE0WZB3774XM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492C50WM7V2NE0WZB3774XM`.
- Optimistic claim succeeded (`expectedRevision=06F53GWZPDF1M1EJSMBQJ306NW`, `currentRevision=06F53N5JD4QJMAVM6D9RYGCB30`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an' from source '5d215e7475770abb784b59d2c1b01522d8b9acca'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an` as `95b45bb2a3ad`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: DataVaultDiagnosticsResult, ReadShape, DataVaultReadShapeDiagnostics, IDataVaultReadDiagnosticsService :: - Current branch source proves existing public read diagnostics only for `DataVaultDiagnosticsResult.ReadShape`, `DataVau...
- Blocking finding: Unsupported inferred API claim: DataVaultDiagnosticsResult, ReadShape, DataVaultReadShapeDiagnostics :: - `DataVaultDiagnosticsResult.ReadShape` remains the single public request-bound read diagnostics member; any new performance-stage model is strictly addit...
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: DataVaultDiagnosticsResult, ReadShape, DataVaultReadShapeDiagnostics, IDataVaultReadDiagnosticsService :: - Current branch source proves existing public read diagnostics only for `DataVaultDiagnosticsResult....
- Risky assumption: Existing API/type assumption lacks source evidence: DataVaultDiagnosticsResult, ReadShape, DataVaultReadShapeDiagnostics :: - `DataVaultDiagnosticsResult.ReadShape` remains the single public request-bound read diagnostics member; any new performance-stage mod...

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9100`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `7ae83bfad7634582828d5c56c5241747`
- completed-at-utc: `<redacted>-22T22:42:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492C50WM7V2NE0WZB3774XM/runs/20260522T224241733Z-7ae83bfad7634582828d5c56c5241747.json`