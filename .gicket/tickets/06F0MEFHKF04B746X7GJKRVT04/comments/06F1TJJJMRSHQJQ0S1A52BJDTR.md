[gicket-bot] Run report (outcome: dev-workflow-apply)

Summary
- Developer workflow prepared branch 'ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry' and commit 'ade2e9fbd2e7' for ticket '06F0MEFHKF04B746X7GJKRVT04'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.4` was applied to ticket `06F0MEFHKF04B746X7GJKRVT04`.
- Optimistic claim succeeded (`expectedRevision=06F1SQ65MGSTRQ0KR9ZDDD32G4`, `currentRevision=06F1SQBZ0X3RGS035GNQ22DJ5C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.4`.
- Prepared interactive developer scratch worktree for target branch 'ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry' from source 'ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry'.
- Planned implementation step: Added DataVaultModelArtifactExporter with public ExportJson overloads for DataVaultMetadataRegistry and DataVaultMetadataModel.
- Planned implementation step: Serialized schemaVersion, naming.policy, loadTimestampStorage, hubs, links, satellites, pits, and bridges in stable contract order with deterministic formatting.
- Planned implementation step: Rejected non-empty legacy PointInTimeTables before serialization with caller-visible NotSupportedException diagnostics naming the unsupported surface.
- Planned implementation step: Added exporter tests for deterministic registry export, importer round-trip, model overload behavior, Code-First-produced metadata after materialization, PIT export, and legacy PointInTimeTables rejection.
- Planned implementation step: Updated the core public API approved snapshot for the new exporter surface.
- Resolved branch route (fallback): base 'develop', work 'ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry'.
- Executed runtime-orchestration sync-first fetch/pull before workspace automation.
- Checked out existing branch 'ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry'.
- 12 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Risk: Build and test execution could not reach compilation in this sandbox because EF Core and related packages were not cached and network restore is blocked.
- Risk: The public API snapshot was updated manually because the snapshot update test could not run without package restore.

Next steps
- Push branch 'ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry' manually if remote collaboration is required.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9449`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `dev`
- run-id: `e9017562b39540f38d5010d646884775`
- completed-at-utc: `<redacted>-12T17:44:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/d189cefa058d781d6e64d979814d17ab804061edc525b3e1e95f172607e8edb3/tickets/06F0MEFHKF04B746X7GJKRVT04/runs/20260512T174429976Z-e9017562b39540f38d5010d646884775.json`