[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F492C50WM7V2NE0WZB3774XM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492C50WM7V2NE0WZB3774XM`.
- Optimistic claim succeeded (`expectedRevision=06F5AGM17F9TZ8EN5QGC286H34`, `currentRevision=06F5AH41G1RXSSHFGZF6ZM8AY8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an' from source '9a95f904507251f3918886cd733082cd31e47077'.
- Interactive PO-critic tool loop fell back to legacy critic review after MODEL-TOOL-INVOCATION-RESULT-TOOL-CALL-ARGUMENTS-JSON-INVALID.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an` as `391337749f90`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: Verified :: - Verified the existing API split in source and tests: IDataVaultReadDiagnosticsService owns request-bound read Analyze calls and populates DataVaultDiagnosticsResult.ReadShape and DataVaultReadShapeDiagnostics; IDa...
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: Verified :: - Verified the existing API split in source and tests: IDataVaultReadDiagnosticsService owns request-bound read Analyze calls and populates DataVaultDiagnosticsResult.ReadShape and DataVaultReadS...

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `53888`
- effective-cache-ratio: `0.3058`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `9476bd6f5a454d5d9485b77e6258f29a`
- completed-at-utc: `<redacted>-23T14:42:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492C50WM7V2NE0WZB3774XM/runs/20260523T144221249Z-9476bd6f5a454d5d9485b77e6258f29a.json`