[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEJPGG7JBFEXD693BHY07W/description.md: PO Handoff is ready_for_po_critic; Scope Out excludes publishing/runtime/API/test/benchmark implementation; Open Questions is '- none'.
- git log shows HEAD f49829e46 on target branch ticket/06F0MEJPGG7JBFEXD693BHY07W-task-update-docs-and-release-notes-for-v0-7-0-mo; git diff f49829e468c1a9b20171c88bc5114a0edfe2ca30..HEAD for README.md/docs/src/tests/ticket files was empty.
- docs/releases currently contains v0.5.0.md and v0.6.0.md only; no docs/releases/v0.7.0.md exists yet, matching the requested release-note work.
- docs/model-first-governance.md is present, status v0.7.0 branch documentation, and states JSON-first exact schemaVersion dvault.model.v1, stable hubs/links/satellites/pits/bridges categories, strict unknown-field rejection, ordering preservation, YAML boundary, import, EF projection, export, and drift comparison workflow.
- Source files show model-first public surfaces named by the contract: DataVaultModelArtifactImporter.ImportJson, DataVaultModelArtifactExporter.ExportJson, DataVaultModelDriftReporter.Compare, DataVaultModelImportResult, and UseDataVaultMetadata(DataVaultModelImportResult).
- Source files show implemented read surfaces: IDataVaultReadService.ReadLatestSatelliteRowsAsync/ReadPitRowsAsync, DataVaultPitAsOfReadRequest, DataVaultReadServicePitExtensions.ReadPitAsync, DataVaultBridgeReadRequest, DataVaultReadServiceBridgeExtensions.ReadBridgeRowsAsync/ReadBridgeAsync, DataVaultBridgeReadRecord, DataVaultBridgeEndpointReadValue, and DataVaultBridgeProjectionRow.
- DataVaultBridgeReadRequest validates many-to-many endpoints From/To and hierarchy endpoints Ancestor/Descendant with required bounded maximumDepth; DataVaultBridgeReadRecord exposes TraversalDepth and DataVaultBridgeProjectionRow exposes exact-name RequiredString/RequiredInt32 access.
- README.md currently still contains stale text at README.md:208 and README.md:372 saying PIT-backed read APIs/bridge traversal/provider-specific read strategies remain future; the delivery contract explicitly scopes updating that documentation without overstating row maintenance or graph semantics.
- The six package directories exist under src, and docs/manual-nuget-publication.md plus README.md document tools/verify-packages.sh and the six-package family.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No PO-blocking missing examples. For implementation clarity, bridge docs should include only examples that match From/To many-to-many and Ancestor/Descendant hierarchy with maximumDepth/TraversalDepth if examples are added.
- A benchmark summary should either cite existing benchmark artifact/run context or avoid performance claims; this is already covered by the acceptance criteria.

Risky assumptions
- The docs developer must treat current source as more current than stale README wording when describing PIT, bridge, and SQLite read strategy behavior.
- v0.7.0 package wording must not imply NuGet publication, package hashes, or final publication links before release packaging happens.

AC / test suggestions
- After docs edits, run the repository formatting/documentation check available locally, especially bash tools/check-format.sh, or record why it could not run.
- Use a targeted stale-claim search across README.md and docs/releases for future extension points, not delivered, v0.6.0, PIT-backed read APIs, and bridge traversal helpers.
- If benchmark claims are included, cite artifacts/benchmarks/classic-vs-dvault-scale-5-with-10000/benchmark-summary.md or specific updated run evidence.

Implementation watchouts
- Keep README declaration paths distinct: Code-First, metadata-first, and model-first should be additive alternatives, not replacements.
- Do not document YAML as first-party ingestion, live database drift inspection, PIT/bridge row maintenance, bridge row population, complex/full graph traversal, or provider-specific read optimization beyond source-backed implemented behavior.
- Bridge examples should use exact generated column names with DataVaultBridgeProjectionRow rather than invented traversal APIs.
- Release notes should summarize v0.7.0 relative to v0.6.0 while preserving Code-First and metadata-first compatibility language.

Non-blocking notes
- Current README has partial model-first text already, but release-note coverage for v0.7.0 is absent; that is expected work for the developer.
- The target branch contains ticket/comment workflow commits and no implementation diff from the supplied scratch source ref.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment