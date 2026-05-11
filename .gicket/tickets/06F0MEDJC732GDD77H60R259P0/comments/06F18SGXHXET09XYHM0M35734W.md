[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff. The persisted contract is docs-only, has no unresolved Open Questions, names implemented public APIs backed by source, and gives enough release/manual-publication constraints for a developer to update README.md and v0.6.0 release notes without PO rework.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEDJC732GDD77H60R259P0/description.md persists a Delivery Contract with PO Handoff decision ready_for_po_critic and ## Open Questions containing only 'none'.
- git diff develop...HEAD shows only .gicket/tickets/06F0MEDJC732GDD77H60R259P0 ticket/comment/event files changed; no product code or docs implementation has been touched on the handoff branch.
- README.md:10,16,22-25 still show package install guidance at --version 0.5.0, and README.md:34,57-67 currently leads with metadata-first DataVaultMetadataModel configuration, matching the ticket's documented update need.
- docs/releases currently contains docs/releases/v0.5.0.md and the file existence check for docs/releases/v0.6.0.md returned v0.6.0_exists=1, so the v0.6.0 release-note document is not present yet.
- src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:95-97 exposes ApplyDataVaultMetadata(Action<DataVaultCodeFirstModelBuilder>), and src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs:23,40,50 plus DataVaultCodeFirstHubBuilder.cs:25,53, DataVaultCodeFirstSatelliteBuilder.cs:22,38, and DataVaultCodeFirstLinkBuilder.cs:18 expose Hub<TEntity>(), Link(...), BusinessKey(...), Satellite(...), DrivingKey(...), Payload(...), and Participant<TEntity>().
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16,39 exposes AddDVault() and AddDVault(configure); DataVaultOptions.cs:66 exposes UseMetadataModel(DataVaultMetadataModel); DataVaultDbContextOptionsBuilderExtensions.cs:16,34,48 exposes UseDataVaultMetadata() overloads.
- src/DCoding.Data.DVault/IDataVaultReadService.cs:8,16 exposes raw ReadLatestSatelliteRowsAsync; DataVaultReadServiceTypedProjectionExtensions.cs:48 and DataVaultReadServiceRegistryExtensions.cs:85 expose typed ReadLatestSatelliteAsync<TProjection> projector helpers.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs:281-347 exposes IDataVaultDiagnosticsService Analyze overloads for metadata/code-first/db-context and request-bound save diagnostics; integration tests include NotEvaluated behavior for no-request diagnostics at tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs:25.
- examples/README.md:6,8,28,34,36-39 documents AddDVaultPostgres(), DVAULT_TEST_POSTGRES_CONNECTION_STRING, registry-backed UseDataVaultMetadata(), explicit save, and typed latest/as-of reads; example Program.cs files use UseMetadataModel and UseDataVaultMetadata.
- docs/manual-nuget-publication.md:9-18 defines the six-package family, lines 44-51 define release-note evidence, and lines 57-65 require build, test, pack, tools/verify-packages.sh, and tools/check-format.sh.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The release date and final publish approval evidence are intentionally left to the release operator; the developer should keep placeholders clearly marked if final audited values are not available during the docs change.
- README examples must avoid implying a Code-First-to-registry conversion bridge because no such public API was observed and the runnable examples use registry-backed DataVaultMetadataModel instead.

AC / test suggestions
- Keep the existing AC requiring 0.6.0 install commands for all six package ids and normal EF Core provider-package guidance.
- Keep validation evidence aligned to docs/manual-nuget-publication.md: dotnet build, dotnet test, dotnet pack, bash tools/verify-packages.sh, and bash tools/check-format.sh for final release readiness.
- For docs-only implementation review, verify README snippets against the public source APIs cited above rather than relying on planning prose.

Implementation watchouts
- Lead README quickstart with Code-First declarations but keep DataVaultMetadataModel and registry-backed UseDataVaultMetadata() as compatible/advanced paths.
- Make the IDataVaultSaveService boundary explicit and do not describe SaveChanges interception or hidden persistence.
- Describe typed ReadLatestSatelliteAsync<TProjection> projector helpers as the common read path and keep ReadLatestSatelliteRowsAsync raw records as the escape hatch.
- Release notes should mark model-first import/export, PIT-backed reads, bridge traversal helpers, PIT/bridge row maintenance, and provider-specific read optimizations as future work, not v0.6.0 shipped behavior.

Non-blocking notes
- No split is needed; the observed gap is limited to README.md and docs/releases/v0.6.0.md documentation work.
- The target branch currently contains only ticket orchestration/refinement changes, which is appropriate for PO-critic handoff before development.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment