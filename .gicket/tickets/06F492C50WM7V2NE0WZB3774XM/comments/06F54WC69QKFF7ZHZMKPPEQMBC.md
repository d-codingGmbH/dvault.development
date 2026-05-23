[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F492C50WM7V2NE0WZB3774XM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492C50WM7V2NE0WZB3774XM`.
- Optimistic claim succeeded (`expectedRevision=06F54TFH2B30NKXD0ZWA1M66CW`, `currentRevision=06F54TVPX5ZEWNH7WCPWTMSAG0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an' from source 'e27114862c68142b232a44374f02fb31455b8285'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an` as `c95d2c25dfc2`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: IDataVaultReadDiagnosticsService, DataVaultReadShapeDiagnostics, DataVaultDiagnosticsResult, ReadShape :: - The authoritative refinement and implementation preserve `IDataVaultReadDiagnosticsService`, `DataVaultReadShapeDiagnos...
- Blocking finding: Unsupported inferred API claim: Treat, DataVaultDiagnosticsResult, ReadShape, DataVaultReadShapeDiagnostics, IDataVaultReadDiagnosticsService, Extend :: - Treat `DataVaultDiagnosticsResult.ReadShape`, `DataVaultReadShapeDiagnostics`, and `IDataVaultReadDiagno...
- Blocking finding: Unsupported inferred API claim: Because :: - Because the additive delta changes existing public records, implementation must preserve compatibility by extending the current public types instead of replacing constructor parameters or resetting current member s...
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: IDataVaultReadDiagnosticsService, DataVaultReadShapeDiagnostics, DataVaultDiagnosticsResult, ReadShape :: - The authoritative refinement and implementation preserve `IDataVaultReadDiagnosticsService`, `DataV...
- Risky assumption: Existing API/type assumption lacks source evidence: Treat, DataVaultDiagnosticsResult, ReadShape, DataVaultReadShapeDiagnostics, IDataVaultReadDiagnosticsService, Extend :: - Treat `DataVaultDiagnosticsResult.ReadShape`, `DataVaultReadShapeDiagnostics`, and `...
- Risky assumption: Existing API/type assumption lacks source evidence: Because :: - Because the additive delta changes existing public records, implementation must preserve compatibility by extending the current public types instead of replacing constructor parameters or resett...

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9346`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `3be78cfc5403490f9d2b6733a173a344`
- completed-at-utc: `<redacted>-23T01:27:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492C50WM7V2NE0WZB3774XM/runs/20260523T012712200Z-3be78cfc5403490f9d2b6733a173a344.json`