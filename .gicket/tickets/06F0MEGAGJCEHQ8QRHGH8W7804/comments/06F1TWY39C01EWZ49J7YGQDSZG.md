[gicket-bot] PO-critic review contract

Summary
- Ticket contract is ready for developer handoff. PO refinement resolved the prior stale v0.6.0/deferred-tooling concerns, leaves Open Questions as none, and direct source evidence confirms the named v0.7.0 model-first public APIs exist on the ticket branch.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEGAGJCEHQ8QRHGH8W7804/description.md: PO Handoff says ready_for_po_critic and ## Open Questions contains only '- none'.
- .gicket/tickets/06F0MEGAGJCEHQ8QRHGH8W7804/description.md: Scope In/AC require README or docs guide coverage for Code-First vs metadata-first vs model-first, exact APIs, dvault.model.v1 versioning, and precise remaining limitations; Scope Out/DoD keep the change docs-only.
- .gicket/tickets/06F0MEGAGJCEHQ8QRHGH8W7804/comments/06F1TTJSFRTPZX2W2RDEXXC2Z0.md: PO refinement marked critic items answered, replaced v0.6.0-era deferred-tooling wording with current v0.7.0 branch-aware scope, and recorded open questions as none.
- git status --short --branch reported only the ticket branch tracking origin, with no modified file entries.
- src/DCoding.Data.DVault/DataVaultModelArtifactImporter.cs defines public static DataVaultModelArtifactImporter.ImportJson(string json, string? logicalSourcePath = null).
- src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs defines public UseDataVaultMetadata(this DbContextOptionsBuilder, DataVaultModelImportResult) and projects via importResult.RequireMetadataRegistry().
- src/DCoding.Data.DVault/DataVaultModelArtifactExporter.cs defines public ExportJson overloads for DataVaultMetadataRegistry and DataVaultMetadataModel, and remarks state it does not accept raw Code-First fluent declarations or Entity Framework ModelBuilder state.
- src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs defines public Compare overloads for DataVaultMetadataModel, DataVaultModelImportResult, IReadOnlyModel, and DbContext, and the summary says comparison is against generated/current EF metadata without database access.
- docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md defines exact schemaVersion dvault.model.v1, default naming.policy, loadTimestampStorage tokens provider-default/iso-8601-utc-text/utc-ticks, hubs/links/satellites/pits/bridges arrays, strict unknown-field rejection, and JSON-first YAML boundary.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- README currently still contains v0.6.0 package guidance, so implementation must clearly distinguish current branch v0.7.0 API capability from already-published v0.6.0 release notes/package wording.

AC / test suggestions
- During dev/test, verify the docs contain the exact API names DataVaultModelArtifactImporter.ImportJson, DataVaultModelArtifactExporter.ExportJson, DataVaultModelDriftReporter.Compare, and UseDataVaultMetadata(DataVaultModelImportResult).
- Check that no current v0.7.0 docs wording says model-first import/export/projection/drift APIs are deferred; reserve limitation wording for no CLI, no CI gate snippets, no direct YAML ingestion, no live DB drift introspection, and no raw Code-First/ModelBuilder-to-registry export bridge.
- Verify README still preserves installation, quickstart, package scope, and limitation guidance after linking or summarizing the governance workflow.

Implementation watchouts
- Keep executable examples on public source-proven APIs only; do not invent CLI commands or CI snippets.
- Show export only from already-materialized DataVaultMetadataModel or DataVaultMetadataRegistry, not from raw Code-First fluent declarations or EF ModelBuilder state.
- Describe DataVaultModelDriftReporter.Compare as design-time/generated EF metadata comparison, not live database introspection.
- Keep YAML framed as external authoring converted to canonical dvault.model.v1 JSON before DVault ingestion.

Non-blocking notes
- none

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment