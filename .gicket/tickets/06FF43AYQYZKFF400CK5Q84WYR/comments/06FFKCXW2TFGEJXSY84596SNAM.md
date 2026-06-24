[gicket-bot] PO refinement contract

Summary
- Refined the SQL Server PIT full-rebuild benchmark ticket around the existing SQL Server maintenance-service boundary, the shared benchmark artifact contract, and the missing `pit-full-rebuild-maintenance` row family. No child split or relation change was needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the implementation boundary: `AddDVaultSqlServer()` replaces `IDataVaultPitMaintenanceService` with `SqlServerDataVaultPitMaintenanceService`; this ticket benchmarks that existing service path rather than introducing a new PIT-maintenance architecture.
- The benchmark evidence family for this work is `pit-full-rebuild-maintenance`; existing `pit-as-of-read` and `bridge-traversal-read` rows are explicitly non-substitutes for PIT rebuild timing.
- The bounded SQL Server shape is the existing clean ordinary hub-parent full rebuild only; `MaintainParentsAsync(...)`, multi-active PITs, link-parent PITs, dirty contexts, provider mismatch, and no-savepoint caller transactions remain fallback or non-goal cases.
- The SQL Server external-provider comparison baselines are already ratified by the evidence matrix: `dvault-adddvault-fallback` versus `dvault-adddvaultsqlserver-optimized`.
- No child tickets, relation writes, attachments, or planning-document writes were materialized because the ticket is already bounded to one benchmark-lane addition.

Scope In
- Add a benchmark lane for `IDataVaultPitMaintenanceService.RebuildAsync(...)` full rebuild timing under scenario `pit-full-rebuild-maintenance`.
- Capture a SQL Server external-provider comparator pair for provider-neutral fallback and SQL Server optimized execution.
- Use the existing SQL Server ordinary hub-parent PIT maintenance path and its current service-selection boundary as the v1 benchmark scope.
- Preserve skipped placeholder behavior in the root benchmark artifact triplet when the SQL Server connection string is not configured.

Scope Out
- Using `pit-as-of-read` or `bridge-traversal-read` rows as PIT full-rebuild evidence.
- Benchmarking `MaintainParentsAsync(...)`, bridge maintenance, automatic maintenance, or read-time refresh.
- Widening the SQL Server maintenance lane to multi-active hub-parent PITs, link-parent PITs, or other unsupported shapes.
- Adding MySQL, Oracle, DB2, or PostgreSQL PIT maintenance expansion work to this ticket.
- Changing the existing SQL Server PIT maintenance architecture away from the current service-replacement model.

Open questions
- none

Follow-up questions
- After the lane lands, should a separate evidence-capture ticket check in provider-configured SQL Server `pit-full-rebuild-maintenance` artifacts so the evidence matrix can move from contract-only to completed-timing for this row?
- Should the bounded PostgreSQL PIT full-rebuild maintenance lane be implemented next so the v1 provider-maintenance timing slice covers both currently accepted provider-maintenance families?

Risks
- Without a configured `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, repository validation will only prove skipped-row behavior and not measured SQL Server PIT rebuild timing.
- If the benchmark workload or row labels drift away from the existing ordinary hub-parent maintenance boundary, later readers may overclaim support for `MaintainParentsAsync(...)`, multi-active PITs, or link-parent PITs.
- Because SQL Server uses a service-replacement model instead of strategy registration, it is easy to mislabel execution detail if the new lane copies read/save benchmark conventions without adjustment.

Split recommendations
- If delivery starts pulling in live SQL Server provisioning or checked-in provider-configured artifact collection, split that operational evidence capture into a separate follow-up ticket and keep this ticket limited to adding the lane and default skipped-placeholder behavior.
- Do not bundle PostgreSQL PIT maintenance timing, bridge maintenance timing, or broader provider-maintenance expansion into this ticket.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment