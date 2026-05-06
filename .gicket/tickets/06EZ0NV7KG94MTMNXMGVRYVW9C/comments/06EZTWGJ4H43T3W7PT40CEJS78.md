[gicket-bot] Run report (outcome: dev-workflow-apply)

Summary
- Developer workflow prepared branch 'ticket/06EZ0NV7KG94MTMNXMGVRYVW9C-task-generate-provider-neutral-bridge-ef-model-m' and commit '0ea088f14828' for ticket '06EZ0NV7KG94MTMNXMGVRYVW9C'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06EZ0NV7KG94MTMNXMGVRYVW9C`.
- Optimistic claim succeeded (`expectedRevision=06EZTS8P1W4K46H4QWYD5KTT10`, `currentRevision=06EZTSBY2JNWK1QETCMKSEPBVM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Prepared interactive developer scratch worktree for target branch 'ticket/06EZ0NV7KG94MTMNXMGVRYVW9C-task-generate-provider-neutral-bridge-ef-model-m' from source 'ticket/06EZ0NV7KG94MTMNXMGVRYVW9C-task-generate-provider-neutral-bridge-ef-model-m'.
- Planned implementation step: Updated the bridge unit translation test to build a combined model containing HubCustomer, HubOrder, LinkCustomerOrder, SatCustomerContact, BridgeCustomerOrder, and BridgeSalesRegionHierarchy.
- Planned implementation step: Added assertions that existing hub, link, and satellite projections still match their established shape in the same model that projects bridges.
- Planned implementation step: Added SQLite schema assertions that BridgeCustomerOrder and BridgeSalesRegionHierarchy have zero rows from pragma_foreign_key_list.
- Planned implementation step: Ran repository format and bounded verification commands available in the offline workspace.
- Resolved branch route (fallback): base 'develop', work 'ticket/06EZ0NV7KG94MTMNXMGVRYVW9C-task-generate-provider-neutral-bridge-ef-model-m'.
- Executed runtime-orchestration sync-first fetch/pull before workspace automation.
- Checked out existing branch 'ticket/06EZ0NV7KG94MTMNXMGVRYVW9C-task-generate-provider-neutral-bridge-ef-model-m'.
- Continuing with pre-existing repository changes on branch 'ticket/06EZ0NV7KG94MTMNXMGVRYVW9C-task-generate-provider-neutral-bridge-ef-model-m' because the active developer transport already materialized in-flight ticket edits: tests/DCoding.Data.DVault.Tests/Integration/Sqlite...
- 11 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Risk: Full source rebuild and full solution test execution were blocked locally because network access is restricted and the local NuGet cache lacks required EF Core packages; dotnet test --no-restore failed with NU1101 for Microsoft.EntityFrameworkCore packages. The no-build ...

Next steps
- Push branch 'ticket/06EZ0NV7KG94MTMNXMGVRYVW9C-task-generate-provider-neutral-bridge-ef-model-m' manually if remote collaboration is required.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9704`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `dev`
- run-id: `34f7b156762543109d89e6efe5cde24e`
- completed-at-utc: `<redacted>-06T13:20:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06EZ0NV7KG94MTMNXMGVRYVW9C/runs/20260506T132003930Z-34f7b156762543109d89e6efe5cde24e.json`