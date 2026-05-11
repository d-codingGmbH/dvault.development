[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0ME84YSZ62WRX1SJQE7BMTC/description.md lines 7-9: PO Handoff decision is ready_for_po_critic.
- .gicket/tickets/06F0ME84YSZ62WRX1SJQE7BMTC/description.md lines 19-25 and 27-34 bound Scope In/Out across Code-First metadata, registry usage, explicit save/read helpers, diagnostics/examples, and excluded SaveChanges/model-first/PIT/bridge/Code-First-to-registry/link-parent-satellite/hub-name override work.
- .gicket/tickets/06F0ME84YSZ62WRX1SJQE7BMTC/description.md lines 36-45 contain nine acceptance criteria covering fluent Code-First declarations, provider-aware projection, declaration ordering, validation errors, explicit saves, typed reads, diagnostics, docs/quickstarts, and v0.5 compatibility.
- .gicket/tickets/06F0ME84YSZ62WRX1SJQE7BMTC/description.md lines 62-63: ## Open Questions is '- none', so the explicit approval gate is satisfied.
- .gicket/relations/TC/1R, TC/R4, TC/BM, and TC/T0 parentOf relation files show this epic parents 06F0ME8NFJX6CD20MEA10J761R, 06F0MEANEV00QSYHMSGWX1X0R4, 06F0MEBV90FB8TQMRXJNH078BM, and 06F0MECWYMPQ4R0KWV1R637RT0.
- docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md lines 12-26 define the additive ApplyDataVaultMetadata(Action<DataVaultCodeFirstModelBuilder>) entry point and projection through DataVaultMetadataModel into the provider-aware path.
- docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md lines 118-141 define selector/link validation rules and compatibility guardrails, including no SaveChanges interception and no PIT/bridge/model-first/registry export/import/typed helper expansion in that contract.
- src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs lines 95-104 directly defines the public ApplyDataVaultMetadata overload accepting Action<DataVaultCodeFirstModelBuilder> and building a metadata model.
- src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs lines 8-71 directly defines DataVaultCodeFirstModelBuilder with Hub<TEntity>(), Link(...), and BuildMetadataModel projecting hubs, links, and satellites into DataVaultMetadataModel.
- src/DCoding.Data.DVault/DataVaultCodeFirstHubBuilder.cs lines 19-64, DataVaultCodeFirstSatelliteBuilder.cs lines 16-46, and DataVaultCodeFirstLinkBuilder.cs lines 13-23 directly define BusinessKey, Satellite, DrivingKey, Payload, and Participant<TEntity>() surfaces preserving declaration order via append operations.
- src/DCoding.Data.DVault/DataVaultSaveService.cs lines 9-35 directly defines IDataVaultSaveService as an explicit write boundary using DataVaultSaveRequest/DataVaultBulkSaveRequest, not SaveChanges interception.
- src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs lines 18-73 directly defines ReadLatestSatelliteAsync<TProjection> using a caller-supplied projector delegate over DataVaultSatelliteProjectionRow.
- README.md lines 24, 50-94, 98, and 165-206 document the recommended v0.6.0 Code-First, explicit-save, typed-read, raw-row escape hatch, and bounded limitation paths.
- docs/releases/v0.6.0.md lines 21-39 document Code-First highlights, explicit persistence, typed read helpers, registry-backed metadata compatibility, no public Code-First-to-registry API, and raw-row read availability; lines 41-49 list known limitations.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstSchemaParityTests.cs has tests named ApplyDataVaultMetadataCodeFirstMatchesMetadataFirstRelationalShapeForBuiltInProviderProfiles and ApplyDataVaultMetadataCodeFirstKeepsCoveredBaselineOrderingAndCollisionShapeExplicit, with covered Code-First setup using repeated BusinessKey/Payload/DrivingKey and ordered Participant calls at lines 120-136.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstMetadataTranslationTests.cs lines 41-144 cover driving-key parity and unsupported/duplicate selector validations for BusinessKey, Payload, and DrivingKey.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstLinkTests.cs lines 12-180 cover explicit/derived link projection and missing, late, ambiguous, too-few, repeated same-hub, and unsupported selector failures.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs lines 8-54 cover diagnostics/explain output for metadata model, registry, and Code-First result shapes.
- git diff --name-status develop...HEAD for the scoped ticket/docs paths shows this branch changes only the ticket record/comments/events/description for 06F0ME84YSZ62WRX1SJQE7BMTC; repository docs/source evidence is already present on the branch baseline.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The epic remains an umbrella coordination ticket; developer execution should continue through existing bounded child/implementation surfaces rather than expanding this epic into direct feature work.
- The documentation/source distinction between Code-First projection and registry-backed authoritative metadata must remain visible to avoid users inferring that Code-First declarations become a registry source in v0.6.0.

AC / test suggestions
- Keep AC coverage tied to the existing source/test anchors: Code-First hub/satellite/link parity, selector/link validation, registry-backed shared metadata, explicit save boundary, typed latest/as-of reads, diagnostics/explain output, docs/examples, and v0.5 API compatibility.
- For final release closure, replace the release-note placeholder validation language in docs/releases/v0.6.0.md lines 53-61 with audited pass/fail evidence once publication validation is actually run.

Implementation watchouts
- Do not add SaveChanges interception, model-first JSON/YAML, PIT/bridge runtime reads or maintenance, provider-specific read optimizations, a public Code-First-to-registry bridge, fluent link-parent satellite declarations, or hub logical-name overrides under this epic.
- Preserve declaration order for repeated BusinessKey, Payload, DrivingKey, and Participant<TEntity>() calls because both contract and tests treat order as observable metadata behavior.
- Keep typed reads delegate-based and preserve raw ReadLatestSatelliteRowsAsync access for advanced row-level projections.

Non-blocking notes
- Local git status showed only line-ending churn in .gicket-bot/.gitignore, .gicket/.gitignore, .gicket/project.json, and .gicket/types.json; scoped ticket/source/docs status was clean.

Split recommendations
- Keep 06F0ME84YSZ62WRX1SJQE7BMTC as the umbrella epic and continue using bounded child/product-surface splits for fluent API projection, registry integration, explicit save/read helpers, diagnostics/explain output, and examples/docs.
- Do not create new v0.6.0 subtickets for documented limitations unless release planning explicitly promotes one of the follow-up questions into current scope.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment