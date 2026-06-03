<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story to extend the existing SQLite-only optimized PIT/bridge dispatch baseline with diagnostics-gated PostgreSQL and SQL Server strategy candidates, while preserving safe provider-neutral fallback and making no ticket or planning writes.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already fixes the v1 baseline: PIT and bridge reads stay on the existing provider-neutral IDataVaultReadService boundary, consume already maintained read-model tables, and do not add automatic maintenance or SaveChanges-time refresh.
- Current published boundary says SQLite is the only repository-proven optimized PIT/bridge provider path today; this story adds PostgreSQL and SQL Server candidates inside that bounded model rather than introducing new PIT/bridge APIs.
- Supported shape scope is the existing maintained PIT/bridge boundary: supported PIT reads only for published hub-parent or bounded link-parent PIT shapes, and supported bridge reads only for published many-to-many and hierarchy bridge shapes.
- Repository structure already includes dedicated provider packages for PostgreSQL and SQL Server, so the default implementation location is the existing provider-support layer rather than new packages or generator surfaces.
- Recent ticket comments contain only bot claim/lease metadata, and no child tickets, relation changes, description updates, attachments, or planning documents were materialized in this PO pass.

### Scope In
- Diagnostics-gated PostgreSQL PIT optimized read strategy candidates for the already supported maintained PIT shapes.
- Diagnostics-gated SQL Server PIT optimized read strategy candidates for the already supported maintained PIT shapes.
- Diagnostics-gated PostgreSQL bridge optimized read strategy candidates for the already supported maintained bridge shapes.
- Diagnostics-gated SQL Server bridge optimized read strategy candidates for the already supported maintained bridge shapes.
- Safe fallback to the existing provider-neutral PIT and bridge read pipelines whenever provider, shape, maintenance evidence, or freshness evidence does not qualify.

### Scope Out
- New PIT or bridge metadata shapes, request semantics, or public IDataVaultReadService API changes.
- PIT or bridge maintenance orchestration, automatic refresh, SaveChanges integration, or background scheduling.
- Typed read-model helper generator changes beyond consuming the existing runtime read boundary.
- Provider support expansion for MySQL, Oracle, or other databases in this ticket.

## Acceptance Criteria
- When the active provider is PostgreSQL or SQL Server and the request matches a supported maintained PIT shape with the required diagnostics/read-shape evidence, the read pipeline can select a provider-specific PIT strategy candidate instead of the provider-neutral fallback.
- When the active provider is PostgreSQL or SQL Server and the request matches a supported maintained bridge shape with the required diagnostics/read-shape evidence, the read pipeline can select a provider-specific bridge strategy candidate instead of the provider-neutral fallback.
- Unsupported providers, unsupported shapes, stale-maintenance signals, or missing/incomplete evidence fail closed to the existing provider-neutral read path without changing caller-visible PIT or bridge semantics.
- Selected PostgreSQL and SQL Server candidates return the same functional PIT and bridge results as the existing provider-neutral implementation for the same supported inputs.
- Read telemetry and diagnostic output continues to report strategy selection versus fallback for PIT and bridge reads using the existing read-telemetry surface.
- Automated coverage exercises both candidate-selection and fallback behavior for PostgreSQL and SQL Server PIT and bridge reads.

## Definition of Done
- PostgreSQL and SQL Server PIT and bridge candidate paths are implemented within the existing provider-support architecture and stay bounded to the published PIT/bridge read contract.
- Tests prove supported-shape selection, unsupported-shape fallback, and result parity with the provider-neutral path for both providers.
- Telemetry or diagnostic assertions are updated so selected-strategy and fallback-cause reporting remain visible for PIT and bridge reads.
- Any provider-specific support-matrix change or limitation discovered during implementation is reflected in release or planning documentation.

## Implementation Notes
- Reuse the existing SQLite optimized PIT/bridge dispatch pattern as the architecture baseline; this ticket extends candidate selection to PostgreSQL and SQL Server rather than redesigning read orchestration.
- Keep maintenance explicit and caller-owned: optimized reads must consume already maintained PIT and bridge tables only and must not repair or refresh stale rows.
- Use the existing PIT and bridge boundary semantics from the published architecture docs as the v1 shape matrix, including bounded multi-active PIT behavior and published many-to-many and hierarchy bridge behavior.
- Preserve the current telemetry strategy plumbing already visible in DataVaultActivityTracing and related read-summary surfaces so PIT and bridge reads continue to expose provider, strategy, fallback, and failure details.
- No persistent planning artifact was written in this pass; the authoritative refinement contract is this response.

## Open Questions
- none

## Follow-Up Questions
- After PostgreSQL and SQL Server land, should the documented optimized-provider matrix be revisited for MySQL or Oracle, or remain SQLite/PostgreSQL/SQL Server only until new benchmark evidence exists?
- Should release-note and adoption-checklist language explicitly call out any provider-specific supported-shape exclusions discovered during implementation?

## Risks
- The ticket still has a live incoming blocks relation from 06F8KZHZ27SDTNCFNMFDQRVCKM, so delivery sequencing may still depend on upstream work even though PO refinement is complete.
- Provider-specific SQL paths can drift from provider-neutral semantics unless parity tests cover bounded multi-active PIT behavior and hierarchy bridge traversal depth rules.
- If stale-maintenance or read-shape evidence checks fail open instead of failing closed to fallback, optimized reads could return incorrect PIT or bridge results.

## Split Recommendations
- No split was materialized; keep the story whole if implementation stays limited to existing PIT and bridge shapes and fallback safety rules.
- If provider-specific SQL, tests, and benchmark evidence expand beyond that boundary, split next by provider rather than by public API surface.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add diagnostics-gated PostgreSQL and SQL Server optimized read strategy candidates for supported maintained PIT and bridge shapes. Unsupported shapes, stale maintenance, or missing evidence must fall back safely.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Implemented PostgreSQL and SQL Server optimized PIT/bridge read strategy candidates inside the existing provider-support architecture. The public `IDataVaultReadService` surface is unchanged. Unsupported providers and declined shapes still fall back to the provider-neutral read pipelines.

Verification:
- `dotnet build DVault.slnx --nologo` passed.
- `dotnet test DVault.slnx --nologo` passed.
- `bash tools/check-format.sh` passed.

External-provider live execution remains opt-in through existing connection-string environment variables; local full-solution test output skipped those external live tests when not configured.
<!-- gicket-bot:developer-delivery:v1:end -->

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Implemented rework for PostgreSQL and SQL Server optimized PIT/bridge read strategy candidates. The public `IDataVaultReadService` surface remains unchanged. Unsupported providers, unsupported shapes, and incomplete generated PIT/bridge read-model projection evidence fail closed to provider-neutral fallback.

Verification:
- `dotnet build DVault.slnx --nologo` passed.
- `dotnet test DVault.slnx --nologo` passed. External PostgreSQL, SQL Server, MySQL, and Oracle live tests skipped when their opt-in connection-string environment variables were not configured.
- `bash tools/check-format.sh` passed.

Notes:
- Added `IncompleteReadShapeEvidence` diagnostics and telemetry fallback cause coverage for PIT/bridge strategy candidates.
- SQLite remains the optimized latest-satellite read provider path; SQLite, PostgreSQL, and SQL Server are the diagnostics-gated PIT/bridge candidate provider paths.
- Stale PIT/bridge maintenance remains caller-owned under the existing v1 read request contract; this change does not add automatic maintenance, freshness watermark APIs, or read-time refresh.
<!-- gicket-bot:developer-delivery:v1:end -->

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Implemented tester rework for PostgreSQL and SQL Server optimized PIT/bridge read strategy candidates. The public `IDataVaultReadService` surface remains unchanged. Unsupported providers, unsupported shapes, incomplete generated PIT/bridge projection evidence, and observable stale-maintenance signals now fail closed to provider-neutral fallback.

Verification:
- `dotnet build DVault.slnx --nologo` passed.
- `dotnet test DVault.slnx --nologo` passed: integration total 212, failed 0, succeeded 191, skipped 21; unit total 429, failed 0, succeeded 429, skipped 0. External PostgreSQL, SQL Server, MySQL, and Oracle live tests skipped when their opt-in connection-string environment variables were not configured.
- `dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --no-restore --nologo` passed after the final parity-test refactor.
- `bash tools/check-format.sh` passed.

Notes:
- Added `StaleReadModelMaintenance` diagnostics and telemetry fallback cause coverage for PIT/bridge strategy candidates.
- Added PostgreSQL and SQL Server PIT/bridge candidate result-parity coverage against the provider-neutral `AddDVault()` fallback for raw rows and typed projections.
- Stale PIT/bridge maintenance remains caller-owned under the existing v1 read request contract; this change does not add automatic maintenance, freshness watermark APIs, or read-time refresh.
<!-- gicket-bot:developer-delivery:v1:end -->