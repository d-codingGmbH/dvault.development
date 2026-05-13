[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F1XPV0YJ8Z9HQVT6BYR397Q8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F1XPV0YJ8Z9HQVT6BYR397Q8`.
- Optimistic claim succeeded (`expectedRevision=06F1YZ08K7PVPPGYDJ9KYV2W2W`, `currentRevision=06F1YZ9GN2Z22KES8R7FP0FEE8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F1XPV0YJ8Z9HQVT6BYR397Q8-task-validate-migration-operations-and-report-gu' from source '82221a2cb359263f17ff061abaea5ac1c31f2fe3'.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F1XPV0YJ8Z9HQVT6BYR397Q8-task-validate-migration-operations-and-report-gu` as `5335ed1d7201`.

Open questions / Risiken
- Blocking finding: The ticket does not define the invariant decision matrix that makes each of the six operation types safe vs. finding-producing. `.gicket/tickets/06F1XPV0YJ8Z9HQVT6BYR397Q8/description.md:44-45` says fixtures must prove invariant-specific cases and that operat...
- Blocking finding: The handoff does not resolve how migration findings enter the current diagnostics contract. The existing public diagnostics surface only exposes metadata/DbContext/save/read analysis and `DataVaultDiagnosticsIssue` only carries `Severity`, `Code`, `Message`, ...
- Required PO action: Add a concrete safe/unsafe example matrix for AddColumn, DropColumn, DropTable, RenameColumn, CreateIndex, and AlterColumn, tied to named DVault invariants and expected diagnostic codes.
- Required PO action: Clarify the diagnostics contract: which current `IDataVaultDiagnosticsService` entrypoint should own migration-operation analysis, whether a new public entrypoint is allowed, and whether public `DataVaultDiagnosticsIssue` / `DataVaultDiagnosticsResult` shap...
- Required PO action: Refresh the dependency text in the ticket contract so it no longer says this ticket is blocked by `06F1XPS7KGKBP5SVMQPJC49J2G` unless there is newer evidence reopening that dependency.
- Risky assumption: Assuming the internal model-artifact catalog pattern can be reused for migration diagnostics without explicit PO approval.
- Risky assumption: Assuming a public diagnostics API expansion is acceptable even though `ApiSurfaceSnapshotTests` currently lock the `DataVaultDiagnosticsIssue` and `DataVaultDiagnosticsResult` shape.
- Risky assumption: Assuming the current `blocks` language is stale because ticket `06F1XPS7KGKBP5SVMQPJC49J2G` is `done`, despite the persisted contract still describing this ticket as blocked.
- Split recommendation: No split is needed if PO only clarifies the rule matrix and diagnostics contract. If PO decides this work must redesign the public diagnostics API, split that API-contract change from the first migration-validator rule set.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9350`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `d0d1bb86b99c4724a75a8e6a691d73a6`
- completed-at-utc: `<redacted>-13T04:10:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F1XPV0YJ8Z9HQVT6BYR397Q8/runs/20260513T041032366Z-d0d1bb86b99c4724a75a8e6a691d73a6.json`