[gicket-bot] PO refinement contract

Summary
- Refined the ticket for deterministic export from the existing DVault metadata registry / Code-First-built metadata path into the strict JSON dvault.model.v1 artifact contract. No child tickets, relation changes, attachments, or planning documents were created in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Live relations were verified: parent 06F0MEF8N9DXDW01FXYZAEB6T8 owns this ticket; parser/import ticket 06F0MEF08AJ1K52STF42T74B04 blocks this ticket; this ticket blocks downstream consumer 06F0MEGAGJCEHQ8QRHGH8W7804. No stale relation cleanup was identified.
- The repository already contains the model-first import boundary through DataVaultModelArtifactImporter.ImportJson and DataVaultModelImportResult, and it applies valid artifacts back to EF metadata through ApplyDataVaultMetadata(DataVaultModelImportResult). Export should use the same dvault.model.v1 contract rather than introducing a second artifact shape.
- The authoritative export source for v1 is DataVaultMetadataRegistry in canonical declaration order: hubs, links, satellites, PIT metadata, bridges, and provider capability profiles are already exposed as ordered read-only collections.
- Code-First export is in scope only after Code-First declarations have been projected into DataVaultMetadataModel / DataVaultMetadataRegistry. There is no need to export raw fluent builder call syntax.
- The v1 default output format is strict JSON with schemaVersion set to dvault.model.v1, naming.policy default when applicable, and loadTimestampStorage emitted using the existing model-first tokens.
- No split is required for this ticket based on current evidence; the scope is bounded to registry-to-artifact export plus focused deterministic and round-trip tests.

Scope In
- Add a public exporter API in DCoding.Data.DVault that serializes a DataVaultMetadataRegistry, and where practical a DataVaultMetadataModel, to the canonical dvault.model.v1 JSON artifact.
- Support deterministic export of hubs, links, satellites, PIT declarations, bridge declarations, naming policy, and load timestamp storage choices that are representable in the existing dvault.model.v1 contract.
- Preserve exact logical metadata names and declaration order from the registry/model while producing stable property order, stable array ordering, and stable formatting across runs.
- Support Code-First-originated metadata by exporting the DataVaultMetadataModel / DataVaultMetadataRegistry produced by DataVaultCodeFirstModelBuilder and ApplyDataVaultMetadata paths.
- Add focused tests proving deterministic output, import/export compatibility through DataVaultModelArtifactImporter.ImportJson, and coverage for supported keys, payloads, driving keys, PIT, bridge, naming, and timestamp storage choices.

Scope Out
- Drift reporting, diff output, or comparison against database schema.
- Database schema extraction or reverse engineering from EF/database metadata into dvault.model.v1.
- YAML export or a YAML dependency in the core package.
- Code generation or reconstruction of fluent Code-First builder source code.
- Provider-specific DDL, SQL, migrations, or physical table/column override export beyond the provider-neutral dvault.model.v1 contract.
- Export of runtime-only values such as save requests, load timestamp instances, record source values, generated hash values, provider dispatch decisions, or read projection state.

Open questions
- none

Follow-up questions
- A later tooling ticket can decide whether the exporter should also be exposed through a CLI command or build task.
- A later governance ticket can decide whether exported JSON should be normalized by repository policy before commit, such as final newline or indentation width beyond the API default.
- Future model-first expansion can revisit provider-specific physical naming or override export if those become part of a later artifact schema version.

Risks
- The model-first contract may include shapes that are broader than today's Code-First fluent surface, especially PIT and bridge metadata; implementation should export what the registry can represent and test the supported subset explicitly.
- Round-trip equality should compare supported metadata semantics rather than incidental object identity or runtime-only fields.
- Serializer defaults can accidentally destabilize output if property ordering or null/default omission behavior is not fixed in tests.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment