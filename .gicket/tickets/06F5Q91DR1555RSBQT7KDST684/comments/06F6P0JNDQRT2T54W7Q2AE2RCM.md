[gicket-bot] PO-critic review contract

Summary
- Refined contract now matches the observed PIT/bridge repository baseline and is clear enough for an evidence-only developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F5Q91DR1555RSBQT7KDST684/description.md:11-16 and 18-56 now bound the story to the actual non-delete-aware bridge baseline, bound registry-backed PIT evidence to maintenance-name resolution plus explicit `DataVaultPitAsOfReadRequest` diagnostics, and set `## Open Questions` to `- none`.
- README.md:367 and 788, src/DCoding.Data.DVault/IDataVaultBridgeMaintenanceService.cs:20-28, and src/DCoding.Data.DVault/DefaultDataVaultBridgeMaintenanceService.cs:39-85 all define incremental bridge maintenance as append-only/non-delete-aware; `MaintainBridgeAsync(...)` returns `rowsDeleted: 0`, while docs/releases/v0.15.0.md:61 says shrink/topology removal should use `RebuildBridgeAsync(...)`.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs:870-904 exposes read diagnostics for explicit latest/PIT/bridge requests plus registry-backed latest-satellite and bridge requests; src/DCoding.Data.DVault/DataVaultPitMaintenanceServiceRegistryExtensions.cs:7-68 exposes registry-backed PIT rebuild/parent-maintenance adapters; an `rg` search for `DataVaultRegistryPitAsOfReadRequest` across `src/DCoding.Data.DVault`, `tests`, `docs`, and `.gicket` returned only current ticket/comment text, not a source/public API type.
- Existing baseline evidence already exists in tests: ordinary PIT and registry-backed latest/bridge read shapes in tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:71-209 and tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs:173-227; tuple-aware multi-active PIT read-shape diagnostics in tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:213-265; link-parent/multi-active/registry PIT maintenance coverage in tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs:82,330,410,449; and bridge maintenance/read coverage in tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs:13-56,68-129,170-201 and tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs:13-84,97-133,160-199.
- benchmarks/DCoding.Data.DVault.Benchmarks/README.md:13-18 says the default SQLite matrix currently includes ordinary `pit-as-of-read` and `bridge-traversal-read`; benchmark-summary.csv:19-22 contains only those PIT/bridge rows, which matches the ticket's requirement to add new rows or artifact bundles only when this story introduces new measured claims.
- PO refinement comment .gicket/tickets/06F5Q91DR1555RSBQT7KDST684/comments/06F6NY3MJRFF3SARQKY82069SG.md explicitly marks prior critic-items 1-4 as answered, and `git diff --name-only 1ef4cdc6d..20a721956 -- .gicket/tickets/06F5Q91DR1555RSBQT7KDST684` shows the handoff change set was limited to ticket metadata/comments/description.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Optional clarity example: show one shrink/removal scenario that uses `RebuildBridgeAsync(...)` after a many-to-many pair removal or hierarchy-path shrink so readers do not infer delete-aware incremental maintenance.
- Optional clarity example: show one link-parent PIT case that names the ordered snapshot columns and explicitly states that `ParentHashKey` carries the link hash key.

Risky assumptions
- The legacy draft at .gicket/tickets/06F5Q91DR1555RSBQT7KDST684/description.md:76-85 still mentions `delete-aware bridge operations`; the contract block above it is authoritative, but implementers must ignore the legacy text.
- If docs/release wording turns this evidence story into a performance claim, correctness tests alone will not be enough; benchmark rows or an artifacts bundle will still be required by docs/plans/performance-evidence-benchmark-artifact-contract.md.

AC / test suggestions
- Add explicit link-parent PIT read-diagnostics coverage; an `rg` search for `LinkParent|link-parent` across DataVaultDiagnosticsTests.cs, DataVaultDiagnosticsIntegrationTests.cs, and DataVaultPitReadServiceSqliteTests.cs found no current link-parent diagnostics test even though link-parent maintenance/read fallback exists in tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs:330.
- Keep multi-active PIT evidence focused on tuple-aware row identity, read-shape, and index-baseline behavior, matching tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:213-265 and tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs:82-249.
- If new measured claims are added, persist either new root `benchmark-summary.*` rows or an `artifacts/benchmarks/<label>/before|after` bundle, and keep fallback/selected-strategy detail visible.

Implementation watchouts
- Do not add or imply a public `DataVaultRegistryPitAsOfReadRequest`; current source exposes registry-backed PIT maintenance adapters plus explicit PIT read diagnostics/read requests, not a registry PIT as-of read request type.
- Do not imply provider-specific PIT optimization for link-parent or multi-active shapes; current diagnostics/strategy baseline keeps unsupported shapes visible via provider-neutral fallback.
- Do not imply incremental delete-aware bridge reconciliation; current public/docs baseline is append-only `MaintainBridgeAsync(...)` plus `RebuildBridgeAsync(...)` for shrink/removal.

Non-blocking notes
- Current relations still show 06F5Q91DR1555RSBQT7KDST684 -> 06F5Q91M0PM17RP43ZQRPBDXP0 (`blocks`) and 06F5Q90CSKMGK3NZZ25XTW6W4C -> 06F5Q91DR1555RSBQT7KDST684 (`parentOf`), consistent with the refined scope.

Split recommendations
- If stakeholders later want a real delete-aware bridge maintenance capability or incremental shrink-safe reconciliation, keep it as a separate additive capability ticket.
- If stakeholders later want a public registry-backed PIT read request, keep it as a separate additive API ticket instead of broadening this evidence-only story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment