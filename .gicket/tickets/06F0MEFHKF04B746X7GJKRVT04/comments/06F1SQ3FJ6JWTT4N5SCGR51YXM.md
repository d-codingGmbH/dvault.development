[gicket-bot] PO-critic review contract

Summary
- The refined ticket is ready for developer handoff. The persisted contract now resolves the prior Code-First public-surface and legacy PointInTimeTables ambiguities, has Open Questions set to none, and is grounded in existing registry/model/importer source surfaces.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEFHKF04B746X7GJKRVT04/description.md contains the durable Delivery Contract with PO Handoff decision ready_for_po_critic and ## Open Questions set to none.
- .gicket/tickets/06F0MEFHKF04B746X7GJKRVT04/comments/06F1S9QZSBMT29RC2WBJRFC6JG.md shows the prior PO-critic return was specifically for unresolved Code-First public caller scope and PointInTimeTables handling.
- .gicket/tickets/06F0MEFHKF04B746X7GJKRVT04/comments/06F1SJXX4JA7GNNP5EFFTC6EJW.md records PO answers: public export is limited to already-materialized DataVaultMetadataRegistry/DataVaultMetadataModel, raw Code-First export is out of scope, and non-empty PointInTimeTables must fail deterministically.
- git rev-parse observed the current branch as ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry at HEAD 3c9f0d257db09fd6a9b947816a5b3640a9b01cb6; git diff 3c9f0d257db09fd6a9b947816a5b3640a9b01cb6..HEAD returned no changed paths.
- git show --stat --name-only --oneline a8fd6f896b63 showed the latest PO handoff commit only changed .gicket ticket description, comments, events, and ticket.json files.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs exposes public DataVaultMetadataModel with Hubs, Links, Satellites, PointInTimeTables, Bridges, and Pits properties; lines 115-160 show the combined constructor and exported ordered collections.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadataRegistry.cs exposes public DataVaultMetadataRegistry with canonical-order Hubs, Links, Satellites, legacy PointInTimeTables, Bridges, Pits, and ProviderCapabilityProfiles; lines 63-101 also show Create(DataVaultMetadataModel).
- src/DCoding.Data.DVault/DataVaultModelArtifactImporter.cs exposes public ImportJson(string json, string? logicalSourcePath = null), giving a concrete round-trip/import boundary for exported dvault.model.v1 JSON.
- docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md defines the target top-level fields schemaVersion, naming, loadTimestampStorage, hubs, links, satellites, pits, and bridges, and does not define a legacy pointInTimeTables field.
- src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs shows DataVaultCodeFirstModelBuilder has an internal constructor and internal BuildMetadataModel(), while src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilderExtensions.cs shows the public Action<DataVaultCodeFirstModelBuilder> path immediately applies the internally built metadata to EF; docs/releases/v0.6.0.md says no public Code-First-to-registry conversion API exists.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Exporter behavior for a registry created with DataVaultMetadataRegistry.Create(metadataModel) and no provider profiles should be fixed by tests, most likely emitting loadTimestampStorage provider-default.
- Exporter behavior for custom or mixed ProviderCapabilityProfiles that cannot map to one dvault.model.v1 loadTimestampStorage token should be deterministic, preferably a caller-visible diagnostic.
- A repeated same-hub link or hierarchy bridge whose source metadata lacks role-aware participant names should be tested so the exporter does not emit invalid dvault.model.v1 JSON.

Risky assumptions
- Assuming loadTimestampStorage can always be inferred from registry provider profiles without an explicit model-level property is risky unless empty, imported, custom, and mixed profile sets are covered.
- Assuming all bridge metadata is exportable is risky; role-bearing hierarchy shapes are representable only when the source metadata carries enough participant role information.
- Assuming Code-First coverage implies a new public raw fluent export API would contradict the refined contract and release documentation.

AC / test suggestions
- Add deterministic registry export tests that compare repeated JSON output byte-for-byte or string-for-string for property order, declaration order, and formatting.
- Add tests for successful Pits export and deterministic legacy PointInTimeTables rejection from both DataVaultMetadataModel and DataVaultMetadataRegistry inputs.
- Add a successful round-trip test where exported JSON imports through DataVaultModelArtifactImporter.ImportJson without error diagnostics.
- Add a Code-First-originated coverage test using already-materialized DataVaultMetadataModel/DataVaultMetadataRegistry only, without exposing Action<DataVaultCodeFirstModelBuilder> as an exporter input.

Implementation watchouts
- Do not add exporter overloads for raw fluent declarations, Action<DataVaultCodeFirstModelBuilder>, or ModelBuilder.
- Check PointInTimeTables before writing JSON and return diagnostics that name PointInTimeTables and the missing dvault.model.v1 pointInTimeTables contract.
- Use the registry/model ordered collections rather than lookup dictionaries for hubs, links, satellites, pits, and bridges.
- Keep exporter JSON limited to the schema contract fields and route successful fixtures back through DataVaultModelArtifactImporter.ImportJson.

Non-blocking notes
- No split is needed for the refined registry/model exporter scope; direct raw Code-First export, CLI exposure, and PointInTimeTables migration remain follow-up topics.

Split recommendations
- No split recommended for this ticket as refined. Keep direct Code-First-to-registry/export APIs and legacy PointInTimeTables migration helpers as later tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment