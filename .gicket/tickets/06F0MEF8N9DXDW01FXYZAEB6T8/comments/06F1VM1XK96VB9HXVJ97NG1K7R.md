[gicket-bot] PO-critic review contract

Summary
- Ticket contract is ready for developer handoff at the story level. The persisted delivery contract has no unresolved Open Questions, the PO handoff/comment history supports PO-critic review, and repository evidence confirms the referenced v1 artifact, exporter, drift, documentation, annotation, and test surfaces exist for developers to work against.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEF8N9DXDW01FXYZAEB6T8/description.md contains PO Handoff decision ready_for_po_critic and ## Open Questions lists none.
- Comment .gicket/tickets/06F0MEF8N9DXDW01FXYZAEB6T8/comments/06F1VJ8JP4KWRAQ3DAR5BHYANC.md records runtime handover to po-critic; comment 06F1VJW0DY2TSWHH8DANHE4DYC.md records active po-critic lease.
- git branch --show-current returned ticket/06F0MEF8N9DXDW01FXYZAEB6T8-story-add-model-export-and-drift-tooling; git rev-parse HEAD returned 1681f0d96a74af1edb482609d79b6cd1c88a686f.
- git diff --name-status develop...HEAD lists only .gicket ticket/comment/event metadata and the ticket description/json for 06F0MEF8N9DXDW01FXYZAEB6T8; no product-code changes are part of the PO-critic branch delta.
- docs/model-first-governance.md documents ImportJson, ExportJson, DataVaultModelDriftReporter.Compare, manual review usage, and explicitly says drift comparison does not inspect a live database.
- src/DCoding.Data.DVault/DataVaultModelArtifactExporter.cs defines DataVaultModelArtifactExporter.ExportJson overloads for DataVaultMetadataRegistry and DataVaultMetadataModel and writes schemaVersion dvault.model.v1 with naming, loadTimestampStorage, hubs, links, satellites, pits, and bridges.
- src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs defines Compare overloads for DataVaultMetadataModel, DataVaultModelImportResult, IReadOnlyModel, and DbContext, returning deterministic structured/displayable drift reports.
- src/DCoding.Data.DVault/DataVaultAnnotationNames.cs defines the provider-neutral annotations named in the ticket, including ProducedName, EntityKind, MetadataName, ParentReferenceKind, ParentReferenceName, Ordinal, PropertyRole, TechnicalColumnRole, ProviderProfile, ProviderStorageType, ProviderValueFormat, MetadataSourceKind, and MetadataSourceFingerprint.
- tests/DCoding.Data.DVault.Tests/Unit contains DataVaultModelArtifactExporterTests.cs, DataVaultModelArtifactImporterTests.cs, DataVaultModelArtifactParserTests.cs, DataVaultModelBuilderExtensionsTests.cs, and DataVaultModelDriftReporterTests.cs; exporter and drift tests cover determinism, round-trip import, representative model shape, no-drift, missing entity/property, role mismatch, provider/timestamp drift, and key/index drift.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No PO-blocking missing examples found; the persisted AC already calls for deterministic export, empty/default artifact export, representative hub/link/satellite/PIT/bridge export, no-drift comparison, and representative drift categories.

Risky assumptions
- Code-First export should be interpreted as export from the provider-neutral metadata produced by Code-First declarations unless a direct raw Code-First/EF ModelBuilder export bridge is deliberately added and documented; docs/model-first-governance.md currently says no public raw Code-First fluent/EF ModelBuilder to registry export bridge exists.
- Rename reporting depends on stable metadata identity; otherwise added plus removed evidence is acceptable as the contract states.
- PIT and bridge comparison coverage remains bounded by the metadata surfaces present in the current branch and should report unsupported comparison gaps explicitly.

AC / test suggestions
- Keep explicit tests for JSON field ordering and repeated ExportJson determinism.
- Keep import round-trip validation for exported artifacts so exporter compatibility with the existing model-first import path is pinned.
- Include drift tests that assert stable report ordering and precise location fields for declaration, produced entity/table, produced property/column, role, and expected versus actual values.
- Include negative tests for unsupported legacy PointInTimeTables, ambiguous repeated-hub participants without roles, unsupported schemaVersion, and provider/timestamp storage mismatches.

Implementation watchouts
- Honor the strict dvault.model.v1 schemaVersion, default naming.policy, supported loadTimestampStorage tokens, default empty declaration arrays, stable declaration ordering, and unknown-field behavior from the schema contract.
- Use DVault-owned EF annotations and structured metadata for drift comparison instead of ad hoc produced-name parsing.
- Keep drift provider-neutral first; only report provider-specific differences when they are already represented through DVault provider metadata annotations.
- Do not add database migration execution, release publishing, CI gate wiring, direct YAML ingestion, provider-specific DDL diffing, or v2 compatibility under this story.

Non-blocking notes
- I did not run build or tests because this PO-critic role has read-only shell access and test/build commands may write obj/bin artifacts.

Split recommendations
- No additional PO split is required before dev handoff; if implementation is reopened or grows, reuse the existing split shape: exporter, drift report, and documentation/examples.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment