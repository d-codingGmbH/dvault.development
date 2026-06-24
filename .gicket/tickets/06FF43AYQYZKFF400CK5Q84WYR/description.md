<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the SQL Server PIT full-rebuild benchmark ticket around the existing SQL Server maintenance-service boundary, the shared benchmark artifact contract, and the missing `pit-full-rebuild-maintenance` row family. No child split or relation change was needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already fixes the implementation boundary: `AddDVaultSqlServer()` replaces `IDataVaultPitMaintenanceService` with `SqlServerDataVaultPitMaintenanceService`; this ticket benchmarks that existing service path rather than introducing a new PIT-maintenance architecture.
- The benchmark evidence family for this work is `pit-full-rebuild-maintenance`; existing `pit-as-of-read` and `bridge-traversal-read` rows are explicitly non-substitutes for PIT rebuild timing.
- The bounded SQL Server shape is the existing clean ordinary hub-parent full rebuild only; `MaintainParentsAsync(...)`, multi-active PITs, link-parent PITs, dirty contexts, provider mismatch, and no-savepoint caller transactions remain fallback or non-goal cases.
- The SQL Server external-provider comparison baselines are already ratified by the evidence matrix: `dvault-adddvault-fallback` versus `dvault-adddvaultsqlserver-optimized`.
- No child tickets, relation writes, attachments, or planning-document writes were materialized because the ticket is already bounded to one benchmark-lane addition.

### Scope In
- Add a benchmark lane for `IDataVaultPitMaintenanceService.RebuildAsync(...)` full rebuild timing under scenario `pit-full-rebuild-maintenance`.
- Capture a SQL Server external-provider comparator pair for provider-neutral fallback and SQL Server optimized execution.
- Use the existing SQL Server ordinary hub-parent PIT maintenance path and its current service-selection boundary as the v1 benchmark scope.
- Preserve skipped placeholder behavior in the root benchmark artifact triplet when the SQL Server connection string is not configured.

### Scope Out
- Using `pit-as-of-read` or `bridge-traversal-read` rows as PIT full-rebuild evidence.
- Benchmarking `MaintainParentsAsync(...)`, bridge maintenance, automatic maintenance, or read-time refresh.
- Widening the SQL Server maintenance lane to multi-active hub-parent PITs, link-parent PITs, or other unsupported shapes.
- Adding MySQL, Oracle, DB2, or PostgreSQL PIT maintenance expansion work to this ticket.
- Changing the existing SQL Server PIT maintenance architecture away from the current service-replacement model.

## Acceptance Criteria
- The benchmark harness emits scenario `pit-full-rebuild-maintenance` for PIT full-rebuild timing instead of reusing `pit-as-of-read` or `bridge-traversal-read`.
- A configured SQL Server run can produce comparable `SQL Server external provider` rows for baseline `dvault-adddvault-fallback` and `dvault-adddvaultsqlserver-optimized` within the same benchmark artifact contract.
- The optimized completed row identifies `SqlServerDataVaultPitMaintenanceService` as the selected maintenance path, while the fallback comparator row identifies provider-neutral full-rebuild execution with `selectedStrategy=<none>`.
- Completed rows preserve the required benchmark row contract for `pit-full-rebuild-maintenance`, including `maintenanceScope=FullRebuild`, provider, baseline, strategy family, dataset/change context, timing/allocation metrics, deterministic execution detail, and persisted outcome across `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json`.
- The ticket only claims clean ordinary hub-parent full rebuild timing. `MaintainParentsAsync(...)`, multi-active PITs, link-parent PITs, dirty contexts, provider mismatch, and no-savepoint caller transactions are not promoted as completed optimized SQL Server timing claims in this scope.
- When `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is absent, the SQL Server maintenance lane yields skipped placeholder evidence with `iterations=0`, blank or null metrics, deterministic planned execution detail, and `persistedOutcome=not executed`, and the default validation/benchmark run does not fail solely because the optional provider is unconfigured.

## Definition of Done
- The benchmark project includes the new PIT full-rebuild maintenance lane and the repository still builds/tests within the normal benchmark and SQL Server PIT maintenance surfaces.
- The default repository benchmark artifacts reflect the new lane without requiring a live SQL Server instance, using skipped SQL Server placeholder rows when the optional connection string is absent.
- A provider-configured execution of the lane, when run, can emit the benchmark artifact triplet with the SQL Server optimized row and provider-neutral comparator row in contract-compliant form.
- The implementation does not widen the proven SQL Server maintenance boundary beyond clean ordinary hub-parent full rebuilds.

## Implementation Notes
- Reuse the existing benchmark provider-availability pipeline so SQL Server skip handling continues to flow from `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` rather than ad hoc ticket-specific logic.
- Reuse the existing ordinary hub-parent SQL Server PIT maintenance model already exercised in smoke and unit tests as the bounded benchmark workload, rather than inventing a wider PIT shape for v1.
- Because SQL Server PIT maintenance is implemented as a service replacement, the optimized lane should surface `SqlServerDataVaultPitMaintenanceService` in planned/selected execution detail rather than a provider-strategy type name.
- Execution detail for this lane should follow the shared maintenance contract: `maintenanceScope=FullRebuild`, selected strategy or `selectedStrategy=<none>`, and bounded fallback-cause tokens only when a provider path is declined.
- Keep read evidence and maintenance evidence separate in code and artifacts; the new lane adds a maintenance scenario, not a reinterpretation of existing PIT read rows.

## Open Questions
- none

## Follow-Up Questions
- After the lane lands, should a separate evidence-capture ticket check in provider-configured SQL Server `pit-full-rebuild-maintenance` artifacts so the evidence matrix can move from contract-only to completed-timing for this row?
- Should the bounded PostgreSQL PIT full-rebuild maintenance lane be implemented next so the v1 provider-maintenance timing slice covers both currently accepted provider-maintenance families?

## Risks
- Without a configured `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, repository validation will only prove skipped-row behavior and not measured SQL Server PIT rebuild timing.
- If the benchmark workload or row labels drift away from the existing ordinary hub-parent maintenance boundary, later readers may overclaim support for `MaintainParentsAsync(...)`, multi-active PITs, or link-parent PITs.
- Because SQL Server uses a service-replacement model instead of strategy registration, it is easy to mislabel execution detail if the new lane copies read/save benchmark conventions without adjustment.

## Split Recommendations
- If delivery starts pulling in live SQL Server provisioning or checked-in provider-configured artifact collection, split that operational evidence capture into a separate follow-up ticket and keep this ticket limited to adding the lane and default skipped-placeholder behavior.
- Do not bundle PostgreSQL PIT maintenance timing, bridge maintenance timing, or broader provider-maintenance expansion into this ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add a benchmark lane for SQL Server ordinary hub-parent PIT full rebuilds, comparing provider-specific strategy selection with provider-neutral fallback. Acceptance: connection-string absence produces skipped evidence, not a failed default validation run.