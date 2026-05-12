<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket contract against the PO-critic ledger by replacing the v0.6.0-era deferred-tooling wording with a v0.7.0 branch-aware documentation scope. No child tickets, relation changes, attachments, or planning documents were needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- v0.6.0 release notes are historical context and must not be presented as the current v0.7.0 capability baseline.
- The current v0.7.0 branch exposes public model-first JSON import, JSON export, metadata projection, and drift comparison surfaces.
- The documentation must name DataVaultModelArtifactImporter.ImportJson, DataVaultModelArtifactExporter.ExportJson, DataVaultModelDriftReporter.Compare, and UseDataVaultMetadata(DataVaultModelImportResult) when showing executable model-first workflows.
- The dvault.model.v1 planning contract remains the artifact baseline: exact schemaVersion dvault.model.v1, canonical JSON, default naming.policy, strict unknown-field validation, loadTimestampStorage tokens, and stable declaration categories.
- No child tickets, relation changes, attachments, or planning documents were created during this refinement pass.

### Scope In
- Update README or add a linked docs guide for model-first governance usage on the current v0.7.0 branch.
- Document which profile should use Code-First, metadata-first registry-backed metadata, or model-first governed JSON artifacts.
- Describe the governance review workflow for dvault.model.v1 artifacts, JSON imports, JSON exports from existing metadata, model-first projection into EF metadata, and drift reports as review evidence alongside Code-First usage.
- Document the executable public API path using DataVaultModelArtifactImporter.ImportJson, UseDataVaultMetadata(DataVaultModelImportResult), DataVaultModelArtifactExporter.ExportJson, and DataVaultModelDriftReporter.Compare.
- Document artifact versioning rules using the dvault.model.v1 contract, including exact schemaVersion handling, canonical JSON, declaration ordering, unknown-field rejection, and safe handling of future schema versions.
- Make precise current limitations explicit: no first-party CLI commands, no CI gate snippets, no direct YAML ingestion, no live database drift introspection, and no public raw Code-First fluent/ModelBuilder-to-registry export bridge.

### Scope Out
- Implementing parser, exporter, importer, projection, drift reporting, CLI commands, build integration, CI gates, or product APIs.
- Publishing packages or changing NuGet publication workflow.
- Adding direct YAML parser semantics, YAML fixture contracts, or package YAML dependencies.
- Adding live database drift introspection or database schema inspection behavior.
- Adding a public raw Code-First fluent declaration or EF ModelBuilder state export bridge.
- Changing product code, package metadata, provider behavior, or verification scripts.

## Acceptance Criteria
- Docs clearly recommend Code-First for app-local EF declarations that fit the implemented surface, metadata-first registry-backed metadata for one shared authoritative model used by projection/save/read paths, and model-first for governed dvault.model.v1 JSON artifacts that need review, versioning, import/export, projection, or drift-report workflows.
- Docs distinguish historical v0.6.0 limitations from current v0.7.0 branch capabilities and do not state that model-first import/export/projection/drift APIs are currently deferred.
- Docs name DataVaultModelArtifactImporter.ImportJson, DataVaultModelArtifactExporter.ExportJson, DataVaultModelDriftReporter.Compare, and UseDataVaultMetadata(DataVaultModelImportResult) when presenting executable model-first workflows.
- Docs define a model-first review workflow where artifact changes are reviewed in source control, JSON artifacts can be imported and projected, metadata models or registries can be exported to canonical JSON, and drift reports are treated as review evidence.
- Docs include versioning guidance for dvault.model.v1 artifacts, including exact schemaVersion, strict v1 compatibility, canonical declaration ordering, unknown-field rejection, and separation of future schema expansion from the v1 contract.
- Docs list the remaining limitations precisely: no CLI commands, no CI gates, no direct YAML ingestion, no live database drift introspection, and no public raw Code-First-to-registry export bridge.
- README remains valid for NuGet package verification and keeps installation, quickstart, package scope, and limitation guidance intact.

## Definition of Done
- README contains a concise model-first governance entry point, or links to a new docs guide that contains the full workflow.
- Any new guide lives under docs/ and is linked from README so package consumers can find it.
- Executable examples use implemented public APIs and avoid invented command names or APIs.
- Artifact examples use canonical dvault.model.v1 JSON and clearly separate external YAML authoring from first-party JSON ingestion.
- Remaining limitations are explicit in the same section or guide that introduces model-first governance.
- The final documentation change is docs-only and does not alter product code, package publication mechanics, provider behavior, or verification scripts.

## Implementation Notes
- Use the exact contract terms dvault.model.v1, schemaVersion, naming.policy default, loadTimestampStorage provider-default/iso-8601-utc-text/utc-ticks, hubs, links, satellites, pits, and bridges when referring to the artifact shape.
- Use v0.6.0 release notes only as historical context; the current branch source and public API snapshot define the v0.7.0 model-first documentation baseline.
- Show DataVaultModelArtifactImporter.ImportJson as strict JSON artifact ingestion into DataVaultModelImportResult, with diagnostics and metadata outputs.
- Show UseDataVaultMetadata(DataVaultModelImportResult) as the implemented way to project a successful model-first import into DbContext configuration.
- Show DataVaultModelArtifactExporter.ExportJson as export from already-materialized DataVaultMetadataModel or DataVaultMetadataRegistry, not from raw Code-First fluent declarations or EF ModelBuilder state.
- Show DataVaultModelDriftReporter.Compare as design-time EF metadata comparison against expected metadata or successful import results; do not imply live database introspection.
- Keep YAML positioned as an external authoring convenience whose converted JSON must match the canonical artifact; do not add first-party YAML semantics.
- Prefer a short README summary plus linked guide if the workflow text would make the package README too large for quick NuGet consumption.

## Open Questions
- none

## Follow-Up Questions
- Decide later whether to add first-party CLI commands for import, export, drift reporting, or artifact validation.
- Decide later whether CI gate snippets should become part of release or governance documentation once command-line automation exists.
- If first-party YAML ingestion becomes a product goal, handle it as a separate additive contract instead of expanding this documentation ticket.
- If a public raw Code-First fluent or EF ModelBuilder-to-registry export bridge becomes a product goal, define it in a separate API contract before documenting it as supported.

## Risks
- The main risk is stale wording copied from v0.6.0 release notes causing docs to understate the current v0.7.0 public API surface.
- README is packaged with NuGet, so long governance detail could obscure the quickstart; a concise README entry with a linked guide remains the safer documentation shape.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Document how teams should use model-first artifacts, export, import, and drift reports alongside Code-First usage.

## Scope In

- README or guide updates for model-first usage.
- Suggested review workflow and versioning rules.
- Boundaries between Code-First, metadata-first, and model-first paths.

## Scope Out

- Publishing packages.
- Documenting unimplemented graph semantics.

## Acceptance Criteria

- Docs make it clear which path is recommended for which user profile.
- Remaining limitations are explicit and not hidden in examples.
- Package README remains valid for NuGet verification.