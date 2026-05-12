[gicket-bot] PO refinement contract

Summary
- Refined the ticket contract against the PO-critic ledger by replacing the v0.6.0-era deferred-tooling wording with a v0.7.0 branch-aware documentation scope. No child tickets, relation changes, attachments, or planning documents were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now distinguishes historical v0.6.0 limitations from current v0.7.0 branch capabilities: v0.6.0 release notes are only historical context, while the active branch must be documented as having public JSON import, JSON export, model-first projection, and drift comparison APIs.
- critic-item-2: `answered` - The docs must name the existing public APIs when describing executable v0.7.0 workflows: DataVaultModelArtifactImporter.ImportJson, DataVaultModelArtifactExporter.ExportJson, DataVaultModelDriftReporter.Compare, and UseDataVaultMetadata(DataVaultModelImportResult). They should not hide these behind generic future-tooling language.
- critic-item-3: `answered` - The blanket 'deferred import/export tooling' limitation is replaced with precise remaining limitations: no first-party CLI commands, no documented CI gate snippets, no direct YAML ingestion API or fixture contract, no live database drift introspection, and no public raw Code-First fluent/ModelBuilder-to-registry export bridge. Export starts from already-materialized DataVaultMetadataModel or DataVaultMetadataRegistry.
- critic-item-4: `answered` - The contradiction is removed. The docs should not say current import/export/projection/drift tooling is deferred for v0.7.0; they should document the implemented public API workflows and reserve limitation language only for absent CLI, CI, YAML, live database introspection, and raw Code-First export bridge surfaces.
- critic-item-5: `answered` - The contract no longer uses v0.6.0 release notes as the current limitation baseline. v0.6.0 is called out as historical release context only; the active v0.7.0 ticket scope is governed by current branch source and the dvault.model.v1 planning contract.

Clarifications
- v0.6.0 release notes are historical context and must not be presented as the current v0.7.0 capability baseline.
- The current v0.7.0 branch exposes public model-first JSON import, JSON export, metadata projection, and drift comparison surfaces.
- The documentation must name DataVaultModelArtifactImporter.ImportJson, DataVaultModelArtifactExporter.ExportJson, DataVaultModelDriftReporter.Compare, and UseDataVaultMetadata(DataVaultModelImportResult) when showing executable model-first workflows.
- The dvault.model.v1 planning contract remains the artifact baseline: exact schemaVersion dvault.model.v1, canonical JSON, default naming.policy, strict unknown-field validation, loadTimestampStorage tokens, and stable declaration categories.
- No child tickets, relation changes, attachments, or planning documents were created during this refinement pass.

Scope In
- Update README or add a linked docs guide for model-first governance usage on the current v0.7.0 branch.
- Document which profile should use Code-First, metadata-first registry-backed metadata, or model-first governed JSON artifacts.
- Describe the governance review workflow for dvault.model.v1 artifacts, JSON imports, JSON exports from existing metadata, model-first projection into EF metadata, and drift reports as review evidence alongside Code-First usage.
- Document the executable public API path using DataVaultModelArtifactImporter.ImportJson, UseDataVaultMetadata(DataVaultModelImportResult), DataVaultModelArtifactExporter.ExportJson, and DataVaultModelDriftReporter.Compare.
- Document artifact versioning rules using the dvault.model.v1 contract, including exact schemaVersion handling, canonical JSON, declaration ordering, unknown-field rejection, and safe handling of future schema versions.
- Make precise current limitations explicit: no first-party CLI commands, no CI gate snippets, no direct YAML ingestion, no live database drift introspection, and no public raw Code-First fluent/ModelBuilder-to-registry export bridge.

Scope Out
- Implementing parser, exporter, importer, projection, drift reporting, CLI commands, build integration, CI gates, or product APIs.
- Publishing packages or changing NuGet publication workflow.
- Adding direct YAML parser semantics, YAML fixture contracts, or package YAML dependencies.
- Adding live database drift introspection or database schema inspection behavior.
- Adding a public raw Code-First fluent declaration or EF ModelBuilder state export bridge.
- Changing product code, package metadata, provider behavior, or verification scripts.

Open questions
- none

Follow-up questions
- Decide later whether to add first-party CLI commands for import, export, drift reporting, or artifact validation.
- Decide later whether CI gate snippets should become part of release or governance documentation once command-line automation exists.
- If first-party YAML ingestion becomes a product goal, handle it as a separate additive contract instead of expanding this documentation ticket.
- If a public raw Code-First fluent or EF ModelBuilder-to-registry export bridge becomes a product goal, define it in a separate API contract before documenting it as supported.

Risks
- The main risk is stale wording copied from v0.6.0 release notes causing docs to understate the current v0.7.0 public API surface.
- README is packaged with NuGet, so long governance detail could obscure the quickstart; a concise README entry with a linked guide remains the safer documentation shape.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 6
- implementation-notes items: 8

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment