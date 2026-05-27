[gicket-bot] PO refinement contract

Summary
- Verified that the repo still treats v0.20.0 as the current PIT/bridge public baseline, confirmed the PIT/bridge tests and benchmark evidence already exist, and refined this ticket as a bounded v0.21.0 documentation roll-forward; no child tickets, relation changes, description updates, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already confirms the shipped PIT/bridge boundary in `README.md` and `docs/production-adoption-checklist.md`: explicit `IDataVaultPitMaintenanceService` and `IDataVaultBridgeMaintenanceService`, SQLite-only optimized PIT/bridge reads, provider-neutral fallback elsewhere, bounded link-parent and shared-driving-key multi-active PIT support, and non-delete-aware bridge maintenance with `RebuildBridgeAsync(...)` as the shrink-safe path.
- `docs/releases/v0.21.0.md` is currently missing while `README.md`, `docs/production-adoption-checklist.md`, and `docs/model-first-governance.md` still point to `docs/releases/v0.20.0.md` as the current baseline, so this ticket is a documentation roll-forward rather than a capability-discovery ticket.
- The cited validation surfaces already exist under full repo paths in `tests/DCoding.Data.DVault.Tests/Integration/`, including `DataVaultPitReadServiceSqliteTests.cs`, `DataVaultPitMaintenanceServiceSqliteTests.cs`, `DataVaultBridgeReadServiceSqliteTests.cs`, `DataVaultBridgeMaintenanceServiceSqliteTests.cs`, and `DataVaultDiagnosticsIntegrationTests.cs`; the branch snapshot shorthand omitted some directory prefixes but the repository evidence is present.
- The benchmark evidence already exists both in the root `benchmark-summary.{md,csv,json}` triplet and in `artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/benchmark-summary.{md,csv,json}`, including PIT as-of and bridge traversal rows with explicit fallback-versus-SQLite-optimized execution detail.
- Because `docs/architecture/` currently has no PIT/bridge boundary note, the safe v0.21.0 default is to add one dedicated architecture entrypoint instead of overloading the existing design-time or compiled-compatibility notes.
- No child tickets, relation changes, description updates, attachments, or planning documents were applied in this refinement pass.

Scope In
- Add `docs/releases/v0.21.0.md` as the coordinated seven-package release note for the current PIT and bridge documentation boundary.
- Update current-baseline PIT/bridge references in `README.md`, `docs/production-adoption-checklist.md`, and `docs/model-first-governance.md` so v0.21.0 becomes the documented public baseline while older release notes remain historical.
- Add one dedicated PIT/bridge architecture note under `docs/architecture/` that centralizes maintenance boundaries, read surfaces, diagnostics and read-shape evidence, benchmark evidence pointers, and links to existing migration/drift guidance.
- Link the documentation set to existing repository evidence surfaces: the PIT/bridge/diagnostics integration tests, the root benchmark triplet, the `06F5Q91DR1555RSBQT7KDST684` benchmark artifact bundle, and `docs/plans/performance-evidence-benchmark-artifact-contract.md`.

Scope Out
- Runtime behavior changes, new public APIs, or code changes to PIT/bridge maintenance, diagnostics, or read dispatch.
- Delete-aware bridge maintenance claims, automatic PIT or bridge maintenance, provider-specific PIT maintenance, or non-SQLite optimized PIT/bridge read claims.
- New registry-backed PIT as-of read request claims, model-first link-parent PIT artifacts, incompatible driving-key-family PITs, or cross-product tuple semantics.
- Benchmark schema changes, diagnostic payload changes, or new ticket-specific artifact contracts outside the existing benchmark artifact contract.
- Typed read model generator or hash-canonicalization documentation work already tracked by `06F5Q91V0YGSA6SH9WDS02GH0M`, `06F5Q922T5B21GJN49FYN6DJH0`, and `06F5Q934MSKVCQAHPCWEM29CZW`.

Open questions
- none

Follow-up questions
- If a future capability ticket lands a real delete-aware bridge maintenance path or incremental shrink-safe maintenance flow, should a later release widen the bridge documentation boundary then?
- If a future capability ticket adds a registry-backed PIT as-of read request, should that move from a documented limitation into a supported release-note surface at that time?
- If non-SQLite providers later gain repository-proven optimized PIT or bridge read paths, should a later release add provider-specific evidence instead of broadening v0.21.0 retroactively?

Risks
- Documentation wording could drift past the implemented baseline by implying automatic maintenance, delete-aware bridge behavior, or provider optimization beyond SQLite.
- If the PIT/bridge boundary is not centralized in one dedicated architecture note, README, checklist, and release-note wording can diverge again and recreate the current baseline-reference split.
- Benchmark citations must preserve the explicit fallback-versus-SQLite-optimized execution detail already present in the root triplet and the `06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics` bundle, or the release note could overstate measured support.

Split recommendations
- No split is recommended if this ticket stays bounded to the v0.21.0 documentation roll-forward: release notes, baseline-reference updates, and one dedicated PIT/bridge architecture entrypoint.
- If stakeholders later want new PIT or bridge capabilities such as delete-aware bridge maintenance, non-SQLite optimized reads, or registry-backed PIT as-of requests, create additive follow-up tickets instead of widening this documentation ticket.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment