<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified that the repo still treats v0.20.0 as the current PIT/bridge public baseline, confirmed the PIT/bridge tests and benchmark evidence already exist, and refined this ticket as a bounded v0.21.0 documentation roll-forward; no child tickets, relation changes, description updates, attachments, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already confirms the shipped PIT/bridge boundary in `README.md` and `docs/production-adoption-checklist.md`: explicit `IDataVaultPitMaintenanceService` and `IDataVaultBridgeMaintenanceService`, SQLite-only optimized PIT/bridge reads, provider-neutral fallback elsewhere, bounded link-parent and shared-driving-key multi-active PIT support, and non-delete-aware bridge maintenance with `RebuildBridgeAsync(...)` as the shrink-safe path.
- `docs/releases/v0.21.0.md` is currently missing while `README.md`, `docs/production-adoption-checklist.md`, and `docs/model-first-governance.md` still point to `docs/releases/v0.20.0.md` as the current baseline, so this ticket is a documentation roll-forward rather than a capability-discovery ticket.
- The cited validation surfaces already exist under full repo paths in `tests/DCoding.Data.DVault.Tests/Integration/`, including `DataVaultPitReadServiceSqliteTests.cs`, `DataVaultPitMaintenanceServiceSqliteTests.cs`, `DataVaultBridgeReadServiceSqliteTests.cs`, `DataVaultBridgeMaintenanceServiceSqliteTests.cs`, and `DataVaultDiagnosticsIntegrationTests.cs`; the branch snapshot shorthand omitted some directory prefixes but the repository evidence is present.
- The benchmark evidence already exists both in the root `benchmark-summary.{md,csv,json}` triplet and in `artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/benchmark-summary.{md,csv,json}`, including PIT as-of and bridge traversal rows with explicit fallback-versus-SQLite-optimized execution detail.
- Because `docs/architecture/` currently has no PIT/bridge boundary note, the safe v0.21.0 default is to add one dedicated architecture entrypoint instead of overloading the existing design-time or compiled-compatibility notes.
- No child tickets, relation changes, description updates, attachments, or planning documents were applied in this refinement pass.

### Scope In
- Add `docs/releases/v0.21.0.md` as the coordinated seven-package release note for the current PIT and bridge documentation boundary.
- Update current-baseline PIT/bridge references in `README.md`, `docs/production-adoption-checklist.md`, and `docs/model-first-governance.md` so v0.21.0 becomes the documented public baseline while older release notes remain historical.
- Add one dedicated PIT/bridge architecture note under `docs/architecture/` that centralizes maintenance boundaries, read surfaces, diagnostics and read-shape evidence, benchmark evidence pointers, and links to existing migration/drift guidance.
- Link the documentation set to existing repository evidence surfaces: the PIT/bridge/diagnostics integration tests, the root benchmark triplet, the `06F5Q91DR1555RSBQT7KDST684` benchmark artifact bundle, and `docs/plans/performance-evidence-benchmark-artifact-contract.md`.

### Scope Out
- Runtime behavior changes, new public APIs, or code changes to PIT/bridge maintenance, diagnostics, or read dispatch.
- Delete-aware bridge maintenance claims, automatic PIT or bridge maintenance, provider-specific PIT maintenance, or non-SQLite optimized PIT/bridge read claims.
- New registry-backed PIT as-of read request claims, model-first link-parent PIT artifacts, incompatible driving-key-family PITs, or cross-product tuple semantics.
- Benchmark schema changes, diagnostic payload changes, or new ticket-specific artifact contracts outside the existing benchmark artifact contract.
- Typed read model generator or hash-canonicalization documentation work already tracked by `06F5Q91V0YGSA6SH9WDS02GH0M`, `06F5Q922T5B21GJN49FYN6DJH0`, and `06F5Q934MSKVCQAHPCWEM29CZW`.

## Acceptance Criteria
- `docs/releases/v0.21.0.md` exists and records v0.21.0 as the current coordinated PIT/bridge documentation boundary, stating shipped support and explicit limitations without widening the behavior claim set.
- `README.md` replaces PIT/bridge current-baseline references that still point at `docs/releases/v0.20.0.md` while keeping earlier release notes as historical records.
- `docs/production-adoption-checklist.md` and the new PIT/bridge architecture note agree with `README.md` on explicit maintenance, bounded link-parent PIT scope, bounded shared-driving-key multi-active PIT scope, SQLite-only optimized read dispatch, diagnostics/read-shape evidence, benchmark evidence, and the non-delete-aware bridge baseline.
- Updated docs link to concrete existing repo evidence surfaces, including the full integration-test paths and benchmark artifacts, and route migration/drift guidance through the existing design-time and model-first governance docs instead of inventing PIT/bridge-specific automation.
- No updated doc implies automatic maintenance, delete-aware bridge maintenance, new registry-backed PIT read APIs, or provider optimization claims beyond the visible SQLite and provider-neutral fallback evidence already present in the repo.

## Definition of Done
- The release note, README, production checklist, model-first governance reference, and PIT/bridge architecture entrypoint consistently describe the same v0.21.0 PIT/bridge baseline.
- All evidence links resolve to existing repo assets or documented repo command surfaces already present in the repository.
- The published documentation preserves the current implementation limits proven in code and tests, including explicit maintenance boundaries and the bounded link-parent and shared-driving-key multi-active PIT scope.
- Historical done ticket `06F5Q91DR1555RSBQT7KDST684` remains evidence context only, and unrelated typed-read or hash-work tickets are not pulled back in as blockers for this documentation ticket.

## Implementation Notes
- Reuse the existing PIT and bridge sections in `README.md` as the public wording source of truth; they already document explicit maintenance, bounded link-parent and multi-active PIT behavior, SQLite-only optimized reads, and append-only/non-delete-aware `MaintainBridgeAsync(...)` with `RebuildBridgeAsync(...)` for shrink-safe recomputation.
- Treat `tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitReadServiceSqliteTests.cs`, `DataVaultPitMaintenanceServiceSqliteTests.cs`, `DataVaultBridgeReadServiceSqliteTests.cs`, `DataVaultBridgeMaintenanceServiceSqliteTests.cs`, and `DataVaultDiagnosticsIntegrationTests.cs` as the primary repository validation surfaces for the documentation claims.
- Use the root `benchmark-summary.{md,csv,json}` triplet and `artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/benchmark-summary.{md,csv,json}` as the benchmark evidence pointers; those artifacts already carry PIT as-of and bridge traversal rows with fallback versus SQLite-optimized execution detail.
- Since `docs/architecture/` currently lacks a PIT/bridge note, create a dedicated architecture document rather than scattering the boundary across `dvault-dotnet-ef-design-time-workflow.md` or `dvault-ef-compiled-compatibility.md`; link those existing notes only for migration/drift and adjacent compatibility context.
- Keep registry-backed PIT wording limited to maintenance-request resolution through `DataVaultRegistryPitRebuildRequest` and `DataVaultRegistryPitParentMaintenanceRequest`; the visible repository baseline still does not include a `DataVaultRegistryPitAsOfReadRequest`.
- No persistent ticket, relation, attachment, or planning-document writes were applied during this refinement run.

## Open Questions
- none

## Follow-Up Questions
- If a future capability ticket lands a real delete-aware bridge maintenance path or incremental shrink-safe maintenance flow, should a later release widen the bridge documentation boundary then?
- If a future capability ticket adds a registry-backed PIT as-of read request, should that move from a documented limitation into a supported release-note surface at that time?
- If non-SQLite providers later gain repository-proven optimized PIT or bridge read paths, should a later release add provider-specific evidence instead of broadening v0.21.0 retroactively?

## Risks
- Documentation wording could drift past the implemented baseline by implying automatic maintenance, delete-aware bridge behavior, or provider optimization beyond SQLite.
- If the PIT/bridge boundary is not centralized in one dedicated architecture note, README, checklist, and release-note wording can diverge again and recreate the current baseline-reference split.
- Benchmark citations must preserve the explicit fallback-versus-SQLite-optimized execution detail already present in the root triplet and the `06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics` bundle, or the release note could overstate measured support.

## Split Recommendations
- No split is recommended if this ticket stays bounded to the v0.21.0 documentation roll-forward: release notes, baseline-reference updates, and one dedicated PIT/bridge architecture entrypoint.
- If stakeholders later want new PIT or bridge capabilities such as delete-aware bridge maintenance, non-SQLite optimized reads, or registry-backed PIT as-of requests, create additive follow-up tickets instead of widening this documentation ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Publish the v0.21.0 documentation boundary for PIT and bridge completeness using the repository-proven baseline already present in README, tests, and benchmark artifacts.

Clarifications:
- Repository evidence already shows explicit PIT maintenance through `IDataVaultPitMaintenanceService`, explicit bridge maintenance through `IDataVaultBridgeMaintenanceService`, link-parent PIT reads and maintenance for unique non-multi-active link-parent satellites, bounded multi-active hub-parent PITs with one shared canonical driving-key family, provider-neutral PIT and bridge reads, SQLite-only optimized PIT and bridge read dispatch, read-shape diagnostics through `IDataVaultReadDiagnosticsService`, and benchmark evidence in the root `benchmark-summary.{md,csv,json}` triplet plus `artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/`.
- Bridge maintenance remains explicit and non-delete-aware. `MaintainBridgeAsync(...)` is append-only except for lowering an existing hierarchy `TraversalDepth`; `RebuildBridgeAsync(...)` is the shrink-safe path when row removal or increased depth must be handled.
- Registry-backed PIT coverage is limited to maintenance-request resolution through `DataVaultRegistryPitRebuildRequest` and `DataVaultRegistryPitParentMaintenanceRequest`. The visible baseline does not include a `DataVaultRegistryPitAsOfReadRequest`, so docs must not imply one.
- Current docs already explain most runtime behavior in `README.md` and `docs/production-adoption-checklist.md`, but those documents and `docs/model-first-governance.md` still point to `docs/releases/v0.20.0.md` as the current public baseline and the repository currently has no `docs/releases/v0.21.0.md`.

Scope in:
- Add `docs/releases/v0.21.0.md` as the coordinated seven-package PIT and bridge completeness release note.
- Update `README.md`, `docs/production-adoption-checklist.md`, and the relevant baseline references in `docs/model-first-governance.md` so v0.21.0 becomes the current documented PIT and bridge baseline.
- Add or update one architecture doc under `docs/architecture/` so PIT and bridge maintenance boundaries, read surfaces, diagnostics and read-shape evidence, benchmark evidence pointers, and migration and drift guidance have one stable documentation entrypoint.
- Link the documentation set to the existing validation and evidence surfaces: `tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitReadServiceSqliteTests.cs`, `DataVaultBridgeReadServiceSqliteTests.cs`, `DataVaultDiagnosticsIntegrationTests.cs`, `BenchmarkScenarioExecutionTests.cs`, the root `benchmark-summary.{md,csv,json}`, and `docs/plans/performance-evidence-benchmark-artifact-contract.md`.

Scope out:
- Runtime behavior changes, new public APIs, benchmark schema changes, or diagnostic payload changes.
- Delete-aware bridge maintenance claims, automatic PIT or bridge maintenance, provider-specific PIT maintenance, non-SQLite PIT or bridge read optimization, model-first link-parent PIT artifacts, incompatible driving-key-family PITs, cross-product tuple semantics, or a registry-backed PIT as-of read request.
- Typed read model generator or hash canonicalization documentation work owned by `06F5Q91V0YGSA6SH9WDS02GH0M`, `06F5Q922T5B21GJN49FYN6DJH0`, and `06F5Q934MSKVCQAHPCWEM29CZW`.

Acceptance criteria:
- `docs/releases/v0.21.0.md` records v0.21.0 as the current coordinated release boundary for PIT and bridge completeness and states the shipped support plus limitations without overclaiming future behavior.
- `README.md` updates every PIT and bridge current-baseline reference that still points at v0.20.0 and keeps earlier release notes as historical records.
- `docs/production-adoption-checklist.md` and the chosen architecture doc agree with README on explicit maintenance, link-parent PIT scope, bounded multi-active PIT scope, SQLite-only PIT and bridge read optimization, diagnostics and benchmark evidence, and the non-delete-aware bridge baseline.
- The docs link to concrete validation and evidence surfaces already present in the repo and point migration guidance at the existing design-time workflow and guardrail docs rather than inventing PIT or bridge-specific migration automation.
- No updated doc implies automatic maintenance, new registry-backed PIT read APIs, delete-aware bridge maintenance, or provider optimization beyond the visible repository evidence.

Definition of done:
- Release notes, README, architecture docs, and production checklist consistently describe the same v0.21.0 PIT and bridge baseline.
- Evidence links resolve to existing repo assets and commands.
- Historical done ticket `06F5Q91DR1555RSBQT7KDST684` is treated as evidence context, not as a live blocker, and unrelated typed-read or hash-work tickets are not kept as blocking work for this ticket.

Implementation notes:
- Use the existing README PIT and bridge sections and the current benchmark summary rows as the source of truth for public wording.
- Use `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` and `docs/model-first-governance.md` for migration and drift guidance links.
- Preserve the current bridge boundary from repository code and docs: `MaintainBridgeAsync(...)` is not delete-aware; `RebuildBridgeAsync(...)` is the shrink-safe path.
- Preserve the current PIT boundary from repository code and docs: link-parent PITs are runtime-only for unique non-multi-active link-parent satellites, and model-first PIT artifacts remain hub-parent-only.

Follow-up questions:
- If a future ticket introduces a real delete-aware bridge maintenance path or a registry-backed PIT as-of read request, document that in a later release rather than broadening v0.21.0.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Summary:
- Added the v0.21.0 PIT/bridge release note and a dedicated PIT/bridge architecture boundary note.
- Updated README, production adoption checklist, and model-first governance references so v0.21.0 is the current PIT/bridge documentation baseline and v0.20.0 remains historical write-boundary context.
- Kept the claim set bounded to explicit maintenance, SQLite-only optimized PIT/bridge reads, provider-neutral fallback, read diagnostics, existing tests, and existing benchmark artifacts.

Repository artifacts:
- README.md
- docs/production-adoption-checklist.md
- docs/model-first-governance.md
- docs/releases/v0.21.0.md
- docs/architecture/dvault-v1-pit-bridge-boundary.md

Verification:
- `bash tools/check-format.sh` passed.
- `git diff --check -- README.md docs/production-adoption-checklist.md docs/model-first-governance.md docs/releases/v0.21.0.md docs/architecture/dvault-v1-pit-bridge-boundary.md` passed.
- Verified that every cited release note, architecture note, integration-test path, root benchmark triplet, and PIT/bridge benchmark bundle file exists.

Notes:
- No runtime behavior, public API, benchmark schema, diagnostic payload, or test source changes were made.
- `dotnet build DVault.slnx --nologo` and `dotnet test DVault.slnx --nologo` were not run for this docs-only handoff; tester can run the standard baseline commands from the updated docs.
<!-- gicket-bot:developer-delivery:v1:end -->

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Rework Summary
- Added root-level documentation entrypoints `dvault-dotnet-ef-design-time-workflow.md` and `dvault-ef-compiled-compatibility.md` so the tester-declared expected repository paths resolve.
- Kept the authoritative notes under `docs/architecture/`; the new root files route readers there and explicitly keep v0.21.0 PIT/bridge scope bounded to the documented maintenance, read, SQLite optimization, and provider-neutral fallback limits.

### Verification
- `bash tools/check-format.sh` passed.
- `dotnet test DVault.slnx --nologo` passed. The run emitted existing warning output, including NuGet vulnerability-cache warnings from the read-only local cache and existing analyzer warnings, but exited successfully.

### Tester Notes
- Inspect `dvault-dotnet-ef-design-time-workflow.md` for the root heading, `Status: relocated documentation entrypoint`, and the link to `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`.
- Inspect `dvault-ef-compiled-compatibility.md` for the root heading, `Status: relocated documentation entrypoint`, and the link to `docs/architecture/dvault-ef-compiled-compatibility.md`.
<!-- gicket-bot:developer-delivery:v1:end -->