[gicket-bot] PO-critic review contract

Summary
- Return to PO refinement: the ticket is docs-only and has Open Questions set to none, but the persisted contract is stale against the current branch's v0.7 model-first APIs and related done tickets.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- HEAD is 2157ec4745dbe334c24bbdcc92289563c7a019c1 on ticket/06F0MEGAGJCEHQ8QRHGH8W7804-task-document-model-first-governance-workflow; git diff 2157ec4745dbe334c24bbdcc92289563c7a019c1..HEAD produced no file changes.
- .gicket/tickets/06F0MEGAGJCEHQ8QRHGH8W7804/description.md lines 18-23 scope docs for Code-First, metadata-first, model-first, review workflow, versioning, and current limitations; lines 52-53 set Open Questions to none.
- .gicket/tickets/06F0MEGAGJCEHQ8QRHGH8W7804/description.md lines 16, 23, and 48 say model-first import/export tooling is deferred or must be qualified as not shipped in v0.6.0.
- .gicket/releases/06F0ME7ZTS0AF9TVFVJWFNY2Q8.json names the active release v0.7.0 - Model-First and Advanced Read Models, with import/export/drift tooling in scope.
- src/DCoding.Data.DVault/DataVaultModelArtifactImporter.cs lines 3-15 exposes public DataVaultModelArtifactImporter.ImportJson for strict JSON dvault.model.v1 artifacts.
- src/DCoding.Data.DVault/DataVaultModelArtifactExporter.cs lines 7-15 and 46-72 exposes public DataVaultModelArtifactExporter.ExportJson for DataVaultMetadataRegistry and DataVaultMetadataModel.
- src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs lines 10-23 and 45-68 exposes public deterministic drift comparison APIs, including overloads for DataVaultModelImportResult.
- src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs lines 42-54 exposes UseDataVaultMetadata(DataVaultModelImportResult).
- tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt lists DataVaultModelArtifactExporter, DataVaultModelArtifactImporter, DataVaultModelDriftReporter, DataVaultModelImportResult, and model-first ApplyDataVaultMetadata overloads as public API.
- README.md lines 326-339 and docs/releases/v0.6.0.md lines 43-50 still describe v0.6.0 limitations where model-first import/export specs were not delivered.
- .gicket/tickets/06F0MEGAGJCEHQ8QRHGH8W7804/comments/06F1TPM5V56F49T6TYHDHATFW0.md lines 4-12 record the PO handoff and state there were no human clarification comments.

Blocking findings
- The delivery contract tells developers to make deferred model-first import/export tooling a current limitation, but current branch source and public API snapshots show implemented public import, export, projection, and drift surfaces. That contradiction can lead to docs that are false for v0.7.0.
- The contract relies on v0.6.0 release notes as a limitation baseline without clearly separating historical v0.6.0 guidance from the active v0.7.0 release scope and already-done related tickets.

Required PO actions
- Revise the contract to distinguish historical v0.6.0 limitations from current v0.7.0 branch capabilities.
- State whether the docs must name the existing public APIs DataVaultModelArtifactImporter.ImportJson, DataVaultModelArtifactExporter.ExportJson, DataVaultModelDriftReporter.Compare, and UseDataVaultMetadata(DataVaultModelImportResult), or deliberately keep them out with a concrete reason.
- Replace the blanket deferred import/export tooling limitation with precise remaining limitations such as no CLI commands, no CI gates, no direct YAML ingestion, no live database drift introspection, and no public raw Code-First-to-registry export bridge.

Open issues ledger
- critic-item-1 [required-po-action] Revise the contract to distinguish historical v0.6.0 limitations from current v0.7.0 branch capabilities.
- critic-item-2 [required-po-action] State whether the docs must name the existing public APIs DataVaultModelArtifactImporter.ImportJson, DataVaultModelArtifactExporter.ExportJson, DataVaultModelDriftReporter.Compare, and UseDataVaultMetadata(DataVaultModelImportResult), or deliberately keep them out with a concrete reason.
- critic-item-3 [required-po-action] Replace the blanket deferred import/export tooling limitation with precise remaining limitations such as no CLI commands, no CI gates, no direct YAML ingestion, no live database drift introspection, and no public raw Code-First-to-registry export bridge.
- critic-item-4 [blocking-finding] The delivery contract tells developers to make deferred model-first import/export tooling a current limitation, but current branch source and public API snapshots show implemented public import, export, projection, and drift surfaces. That contradiction can lead to docs that are false for v0.7.0.
- critic-item-5 [blocking-finding] The contract relies on v0.6.0 release notes as a limitation baseline without clearly separating historical v0.6.0 guidance from the active v0.7.0 release scope and already-done related tickets.

Missing examples / edge cases
- No clarified example boundary for artifact-only JSON examples versus executable public API examples now that model-first APIs exist.
- No explicit edge case for documenting export from existing DataVaultMetadataModel/DataVaultMetadataRegistry while not implying raw Code-First declarations can be exported directly.
- No clarified wording for drift reports as governance evidence without implying CI gate or live database comparison support.

Risky assumptions
- Assuming README.md and docs/releases/v0.6.0.md are the current capability source is risky because the active ticket release is v0.7.0 and source now contains public model-first APIs.

AC / test suggestions
- Add acceptance criteria requiring the docs to reconcile README v0.6.0 limitations with v0.7.0 model-first APIs observed on the current branch.
- Add acceptance criteria that any executable code examples use only current public APIs and any artifact examples are clearly JSON artifacts, not nonexistent CLI/build commands.
- Add a doc review check that the new guide does not claim direct YAML ingestion, CLI commands, CI gates, live database drift comparison, or raw Code-First-to-registry export support.

Implementation watchouts
- Keep the final change docs-only and avoid product code, package metadata, publication mechanics, or verification script changes.
- Prefer a short README entry linking to a docs/ guide so the NuGet README quickstart stays readable.
- Use exact dvault.model.v1 terms and token names from the schema contract, but update capability wording to match current public APIs.

Non-blocking notes
- The persisted Open Questions section is none, so the return is based on repository-contract mismatch, not unresolved open questions.
- The incoming block relations from the schema, exporter, and drift tickets point to tickets that are now done.
- The ticket comment history appears bot-only; the PO handoff comment states no human clarification comments.

Split recommendations
- No split recommended; the issue is contract refinement, not scope size.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment