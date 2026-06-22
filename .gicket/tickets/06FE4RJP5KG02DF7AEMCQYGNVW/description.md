<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the PostgreSQL PIT rebuild prototype as a full-rebuild-only, AddDVaultPostgres/Npgsql-gated provider-library path that preserves IDataVaultPitMaintenanceService semantics, requires bounded gate-and-fallback behavior, and leaves documentation follow-through to 06FE4RKGASKV6F7DF0RD1WTAV4. No description, relation, attachment, child-ticket, or planning-document write was applied in this pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The caller-visible API stays IDataVaultPitMaintenanceService; this ticket is about a provider-owned internal PostgreSQL prototype path, not a new application entrypoint.
- The prototype is full-rebuild-only and must not broaden to MaintainParentsAsync just because targeted delete-and-insert logic already exists in the provider-neutral service.
- The supported PIT baseline is the repo-backed v1 PIT shape set already accepted by maintenance and PIT-read tests: hub-parent ordinary PITs, hub-parent shared-driving-key multi-active PITs, and link-parent non-multi-active PITs.
- Runtime selection must never be broader than the approved dry-run contract: if the PostgreSQL candidate cannot be selected or proven for a request, provider-neutral rebuild remains the runtime default.
- The rollout remains bounded to the existing PostgreSQL and SQL Server PIT rebuild prototype tickets; other providers stay provider-neutral fallback unless later tickets add explicit proof.

### Scope In
- Prototype a PostgreSQL-owned full PIT rebuild INSERT SELECT candidate path for IDataVaultPitMaintenanceService.RebuildAsync.
- Add the bounded PostgreSQL gate-and-diagnostic evaluation needed to select or decline that provider path consistently with the approved dry-run contract.
- Keep execution scoped to the repo-backed PIT baseline already proven elsewhere: hub-parent ordinary PITs, hub-parent shared-driving-key multi-active PITs, and link-parent non-multi-active PITs.
- Preserve explicit caller-owned maintenance semantics, provider-neutral fallback as the default, result-count parity, and redacted tracing and diagnostics behavior.
- Add repo evidence through unit coverage plus opt-in external PostgreSQL integration coverage using the existing Npgsql test harness.

### Scope Out
- No MaintainParentsAsync push-down or parent-targeted incremental maintenance path.
- No bridge maintenance push-down, bridge feasibility work, or bridge runtime commitment.
- No new public maintenance entrypoint, no standalone maintenance diagnostics or export artifact lane, and no cross-provider deployment or runtime platform.
- No automatic maintenance on reads, saves, SaveChanges, startup, migrations, or background scheduling.
- No non-PostgreSQL provider implementation in this ticket; SQL Server remains in 06FE4RJZ4PA0DZ3HXDSEG2BQMM and other providers remain provider-neutral fallback.

## Acceptance Criteria
- With AddDVaultPostgres on an Npgsql-backed DbContext, IDataVaultPitMaintenanceService.RebuildAsync may select a PostgreSQL provider path only when the rebuild request passes explicit candidate gates consistent with the approved dry-run contract; otherwise the current provider-neutral rebuild path remains the runtime default.
- This prototype's execution scope is full PIT rebuild only, using the repo-backed supported PIT baseline already visible in repository tests: hub-parent ordinary PITs, hub-parent shared-driving-key multi-active PITs, and link-parent non-multi-active PITs. MaintainParentsAsync, bridge maintenance, unsupported PIT shapes, and provider mismatches must fall back rather than partially execute a provider path.
- For every gated PostgreSQL execution, the produced PIT contents and DataVaultPitMaintenanceResult values are parity-equivalent to the current provider-neutral rebuild baseline for the same persisted satellite history.
- The PostgreSQL path keeps caller-visible boundaries unchanged: application code still invokes IDataVaultPitMaintenanceService, maintenance stays explicit, and no automatic refresh is added to reads, saves, SaveChanges, startup, migrations, or background jobs.
- Repo proof includes bounded unit and integration coverage for gate selection and fallback plus provider-path rebuild behavior under the existing PostgreSQL test harness, while non-gated conditions still prove provider-neutral fallback.
- The prototype keeps diagnostics and tracing redacted: no raw SQL, query plans, connection details, request values, or ad hoc fallback prose become part of the caller-facing contract for this ticket.

## Definition of Done
- The refined ticket fixes a full-rebuild-only PostgreSQL prototype boundary with no blocking PO questions left open.
- Implementation can stay inside the existing DCoding.Data.DVault and DCoding.Data.DVault.Postgres architecture without inventing a new caller API, deployment runtime, or cross-provider maintenance platform.
- Tests demonstrate provider-path parity for gated PostgreSQL rebuilds and deterministic fallback for unsupported or non-gated cases.
- This ticket remains implementation-focused and leaves architecture-doc updates to 06FE4RKGASKV6F7DF0RD1WTAV4 instead of expanding into separate documentation scope.

## Implementation Notes
- AddDVault currently registers DefaultDataVaultPitMaintenanceService, while AddDVaultPostgres registers save and read strategies only. A PIT maintenance selection seam is new work, but it should remain internal and provider-owned so the public caller boundary stays IDataVaultPitMaintenanceService.
- Existing provider patterns to mirror are DefaultDataVaultSaveService, DefaultDataVaultReadService, DataVaultTelemetryStrategySelector, DataVaultProviderReadStrategyGateEvaluator, and provider package registration via TryAddEnumerable rather than ad hoc provider checks in application code.
- The earlier diagnostics ticket 06FE4RJD5Z6MWC2E66YB3EZ5YW fixed the contract, not repo code. This prototype must supply the concrete PostgreSQL gate behavior it needs without turning that work into a separate runtime platform or manifest or exporter lane.
- Keep provider-specific SQL text, quoting, connection handling, and transaction behavior inside DCoding.Data.DVault.Postgres, following the repository precedent set by PostgresDataVaultSaveStrategy and the exact Npgsql provider-name match.
- Preserve current PIT maintenance semantics from DefaultDataVaultPitMaintenanceService: delete-and-rebuild for RebuildAsync, explicit caller invocation, DataVaultPitMaintenanceResult count semantics, late-arriving correction behavior, and redacted activity tracing.
- Existing tests already cover deterministic PIT row generation, tuple-aware multi-active behavior, link-parent support, and PostgreSQL PIT read gating for the supported PIT baseline. Add prototype coverage on top of that baseline rather than redefining it.
- Relation context is already sufficient: done ticket 06FE4RJD5Z6MWC2E66YB3EZ5YW is historical prerequisite context, and this ticket currently blocks docs follow-up 06FE4RKGASKV6F7DF0RD1WTAV4. No description update, relation change, attachment, child-ticket, or planning-document write was applied in this refinement pass.

## Open Questions
- none

## Follow-Up Questions
- After the PostgreSQL and SQL Server prototypes land, should provider-specific PIT rebuild selection stay prototype-local in provider packages, or should the repo promote a shared maintenance-strategy seam in core?
- If one repo-backed PIT baseline shape materially diverges in SQL complexity or proof burden, should it get a follow-up ticket instead of widening this prototype mid-stream?
- Should 06FE4RKGASKV6F7DF0RD1WTAV4 publish any provider-specific performance or profile language only after both PIT prototype tickets have local evidence?

## Risks
- Because provider packages currently have no PIT maintenance strategy registration seam, this ticket can sprawl into shared-core API design unless the work stays internal and provider-owned.
- If execution gating drifts from the approved dry-run contract, PostgreSQL behavior may silently diverge from fallback expectations or from the forthcoming SQL Server sibling.
- If the prototype grows beyond full rebuild into parent maintenance or bridge maintenance, it will reopen scope the parent story deliberately split out.
- External PostgreSQL integration coverage is opt-in through the existing environment-based harness; without that proof, provider-path regressions may survive unit-only validation.

## Split Recommendations
- Keep the existing decomposition unchanged: 06FE4RJD5Z6MWC2E66YB3EZ5YW for dry-run contract context, this ticket for the PostgreSQL prototype, 06FE4RJZ4PA0DZ3HXDSEG2BQMM for SQL Server, 06FE4RK80ZXGCZ62CMSAYP164W for bridge feasibility, and 06FE4RKGASKV6F7DF0RD1WTAV4 for documentation follow-through.
- If implementation evidence shows one supported PIT baseline shape cannot safely share the same PostgreSQL INSERT SELECT prototype, create a shape-specific follow-up ticket instead of reopening PO scope here.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: prototype a bounded PostgreSQL PIT rebuild INSERT SELECT path behind explicit service/diagnostic gates. Acceptance: fallback remains default when criteria are not met.