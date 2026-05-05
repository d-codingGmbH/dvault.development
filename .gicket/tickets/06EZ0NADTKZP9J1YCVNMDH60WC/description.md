<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement ratifies the existing SQL Server optimized save-strategy baseline, keeps the existing child relations to 06EZ0NAMGKJ63WCXAK1J7B08TR and 06EZ0NAWNDDEP32P497E39MQXR, and requires no new planning artifact in this PO pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- For this story, "explicit optimized capability profile" means the SQL Server package wires `AddDVaultSqlServer()` to register a SQL Server-specific `IDataVaultProviderSaveStrategy` with a compatibility gate; it does not require a new `DataVaultProviderCapabilityProfiles.SqlServer` metadata-translation profile.
- The optimized path is bounded to clean `Microsoft.EntityFrameworkCore.SqlServer` contexts and the current insert-only save shapes: set-based unique-row hub/link writes plus latest-hash-diff satellite checks.
- When the SQL Server strategy rejects the context because of tracked EF changes, provider mismatch, or unsupported request shape, the provider-neutral `IDataVaultSaveService` fallback remains the required caller-visible safety net.

### Scope In
- Registration and dispatch of the SQL Server optimized save strategy through `DCoding.Data.DVault.SqlServer` and `AddDVaultSqlServer()`.
- SQL Server-specific raw SQL behavior for set-based hub/link existence detection, insert-only writes, satellite latest-state hash-diff lookup, and parameter-count chunking within the optimized path.
- Default smoke coverage for registration, strategy selection/gating, SQL command-shape behavior, satellite decision behavior, and saved-record ordering.
- Opt-in live SQL Server smoke coverage for at least one representative hub save, link save, and satellite save when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is supplied.
- Maintainer-facing documentation for enabling the opt-in SQL Server validation lane.

### Scope Out
- A new provider-neutral translator capability profile or storage-mapping surface in `DataVaultProviderCapabilityProfiles`.
- Mandatory local or CI SQL Server dependency for default `dotnet test` execution.
- New `SaveChanges` interception behavior or a non-explicit write entry point.
- Broader provider optimization work outside the SQL Server package and the shared save-strategy contracts it already consumes.

## Acceptance Criteria
- `AddDVaultSqlServer()` registers the SQL Server optimized provider strategy without removing or replacing the core explicit `IDataVaultSaveService` fallback.
- The SQL Server strategy accepts only clean SQL Server contexts and performs set-based unique-row hub/link inserts plus latest-hash-diff satellite filtering; rejected or unsupported cases fall back to the provider-neutral writer.
- Default smoke coverage proves SQL Server strategy registration, compatibility gating, representative SQL command shape, satellite decision logic, and deterministic saved-record ordering without requiring a live SQL Server instance.
- Opt-in external smoke coverage validates at least one representative hub save, one link save, and one satellite save against a developer-managed SQL Server when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is configured.
- README and architecture-level documentation explain the opt-in SQL Server validation command, the required environment variable, and that database provisioning is external to DVault.

## Definition of Done
- `src/DCoding.Data.DVault.SqlServer` contains the bounded SQL Server registration and optimized strategy behavior needed for this story.
- `tests/DCoding.Data.DVault.Tests` covers both default smoke expectations and deterministic skip-or-run behavior for the optional SQL Server integration lane.
- Default local validation remains SQL Server-free when the environment variable is absent, and the skip message remains explicit about the opt-in contract.
- Documentation and tests describe the same supported request shapes and fallback boundary.

## Implementation Notes
- The existing repository baseline already exposes the intended extension boundary in `docs/architecture/dvault-v1-explicit-save-service.md`: provider-specific optimization is dispatched through `IDataVaultProviderSaveStrategy`, not provider-name branching inside the core save service.
- `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs` and `src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultSaveStrategy.cs` already establish the concrete SQL Server registration point and optimized SQL execution path to preserve in this story.
- `tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs`, `tests/DCoding.Data.DVault.Tests/Integration/SqlServerIntegrationTestConfigurationTests.cs`, and `tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs` define the current unit/default-smoke/live-smoke evidence model that the ticket should explicitly own.
- `README.md` and `docs/architecture/dvault-v1-explicit-save-service.md` already document the opt-in SQL Server validation lane; refinement should treat that documentation surface as the baseline rather than opening a new doc location question.
- Existing `parentOf` child relations to 06EZ0NAMGKJ63WCXAK1J7B08TR and 06EZ0NAWNDDEP32P497E39MQXR are already materialized; no additional planning document or child ticket was created in this PO pass.

## Open Questions
- none

## Follow-Up Questions
- If SQL Server-specific metadata translation or storage mappings become necessary later, should that be a separate ticket that adds a true `DataVaultProviderCapabilityProfiles.SqlServer` profile rather than expanding this save-strategy story?
- After the current child tickets complete, does the parent story need a closeout pass that consolidates their evidence back into one release note or milestone summary?

## Risks
- Live SQL Server smoke coverage depends on a developer-managed database and conditional restore of the SQL Server EF Core provider package, so environment drift can block evidence collection even when the default smoke baseline stays green.
- Because the optimized path uses raw SQL batching rather than tracked EF inserts, regressions can hide in identifier quoting, schema resolution, or parameter-count chunking unless the default smoke and opt-in live coverage stay aligned.
- Ticket relations show existing incoming `blocks` edges from 06EZ0N8HW9PZAFKMM5WQD564VR and 06EZ0N9AM9AJ3AB8DQ6Y1JBS28; delivery sequencing should still respect those external dependencies.

## Split Recommendations
- No additional split is recommended in this PO pass; the story already has materialized `parentOf` children 06EZ0NAMGKJ63WCXAK1J7B08TR and 06EZ0NAWNDDEP32P497E39MQXR.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: implement and validate a SQL Server-specific optimized save path in the existing SQL Server provider project.

Scope:
- Use SQL Server-appropriate set-based existence checks and insert-only writes.
- Preserve fallback behavior for unsupported model shapes or unavailable capabilities.
- Add opt-in integration or smoke coverage that does not run by default without a configured SQL Server instance.

Acceptance Criteria:
- The SQL Server provider registers an explicit optimized capability profile.
- Provider tests cover strategy selection and at least one write scenario with SQL Server semantics.
- Documentation explains how maintainers can enable live SQL Server validation.