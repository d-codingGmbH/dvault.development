[gicket-bot] PO refinement contract

Summary
- Refinement ratifies the existing SQL Server optimized save-strategy baseline, keeps the existing child relations to 06EZ0NAMGKJ63WCXAK1J7B08TR and 06EZ0NAWNDDEP32P497E39MQXR, and requires no new planning artifact in this PO pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- For this story, "explicit optimized capability profile" means the SQL Server package wires `AddDVaultSqlServer()` to register a SQL Server-specific `IDataVaultProviderSaveStrategy` with a compatibility gate; it does not require a new `DataVaultProviderCapabilityProfiles.SqlServer` metadata-translation profile.
- The optimized path is bounded to clean `Microsoft.EntityFrameworkCore.SqlServer` contexts and the current insert-only save shapes: set-based unique-row hub/link writes plus latest-hash-diff satellite checks.
- When the SQL Server strategy rejects the context because of tracked EF changes, provider mismatch, or unsupported request shape, the provider-neutral `IDataVaultSaveService` fallback remains the required caller-visible safety net.

Scope In
- Registration and dispatch of the SQL Server optimized save strategy through `DCoding.Data.DVault.SqlServer` and `AddDVaultSqlServer()`.
- SQL Server-specific raw SQL behavior for set-based hub/link existence detection, insert-only writes, satellite latest-state hash-diff lookup, and parameter-count chunking within the optimized path.
- Default smoke coverage for registration, strategy selection/gating, SQL command-shape behavior, satellite decision behavior, and saved-record ordering.
- Opt-in live SQL Server smoke coverage for at least one representative hub save, link save, and satellite save when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is supplied.
- Maintainer-facing documentation for enabling the opt-in SQL Server validation lane.

Scope Out
- A new provider-neutral translator capability profile or storage-mapping surface in `DataVaultProviderCapabilityProfiles`.
- Mandatory local or CI SQL Server dependency for default `dotnet test` execution.
- New `SaveChanges` interception behavior or a non-explicit write entry point.
- Broader provider optimization work outside the SQL Server package and the shared save-strategy contracts it already consumes.

Open questions
- none

Follow-up questions
- If SQL Server-specific metadata translation or storage mappings become necessary later, should that be a separate ticket that adds a true `DataVaultProviderCapabilityProfiles.SqlServer` profile rather than expanding this save-strategy story?
- After the current child tickets complete, does the parent story need a closeout pass that consolidates their evidence back into one release note or milestone summary?

Risks
- Live SQL Server smoke coverage depends on a developer-managed database and conditional restore of the SQL Server EF Core provider package, so environment drift can block evidence collection even when the default smoke baseline stays green.
- Because the optimized path uses raw SQL batching rather than tracked EF inserts, regressions can hide in identifier quoting, schema resolution, or parameter-count chunking unless the default smoke and opt-in live coverage stay aligned.
- Ticket relations show existing incoming `blocks` edges from 06EZ0N8HW9PZAFKMM5WQD564VR and 06EZ0N9AM9AJ3AB8DQ6Y1JBS28; delivery sequencing should still respect those external dependencies.

Split recommendations
- No additional split is recommended in this PO pass; the story already has materialized `parentOf` children 06EZ0NAMGKJ63WCXAK1J7B08TR and 06EZ0NAWNDDEP32P497E39MQXR.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment