[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted contract in .gicket/tickets/06F0MEF08AJ1K52STF42T74B04/description.md:33-68 defines 6 acceptance criteria, 5 DoD items, explicit risks/scope bounds, and `## Open Questions` = `- none` at lines 55-56.
- PO refinement comment .gicket/tickets/06F0MEF08AJ1K52STF42T74B04/comments/06F1NX95B4QTB9RN5J7NPS7BX8.md:6-8 explicitly hands off with decision `ready_for_po_critic`.
- Blocking relations are persisted in .gicket/relations/RW/04/06F0MEE8T9PKPKQH8EPWNQ2CRW--06F0MEF08AJ1K52STF42T74B04--blocks.json:3-5 and .gicket/relations/10/04/06F0MEEGJE9QCHC8YN4FEXYX10--06F0MEF08AJ1K52STF42T74B04--blocks.json:3-5; downstream blocks are persisted in .gicket/relations/04/04/06F0MEF08AJ1K52STF42T74B04--06F0MEFHKF04B746X7GJKRVT04--blocks.json:3-5 and .gicket/relations/04/4M/06F0MEF08AJ1K52STF42T74B04--06F0MEFX5M9V9SA25N76CPGT4M--blocks.json:3-5.
- Repository already has the parser baseline: src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs:96-159 parses `dvault.model.v1` and returns both `DataVaultMetadataModel` and `DataVaultMetadataRegistry`.
- Repository already has the registry-backed public integration points this ticket is supposed to reuse: src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:77-87 and 156-176 project a `DataVaultMetadataRegistry`; src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs:34-60 exposes `UseDataVaultMetadata(...)`; src/DCoding.Data.DVault/DataVaultOptions.cs:66-80 exposes `UseMetadataModel(...)` and `UseMetadataRegistry(...)`.
- Existing tests already prove registry-backed projection/fingerprint behavior: tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataRegistrationTests.cs:41-53 asserts registry projection parity and records `MetadataSourceKind`/`MetadataSourceFingerprint`.
- Public-surface and docs evidence match the ticket goal: tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:96-98,241,252 list registry APIs but no model-first import API; README.md:94,339 and docs/releases/v0.6.0.md:37,45,50 state that public Code-First-to-registry and model-first import/export are not yet delivered.
- Branch-history inspection is consistent with a PO handoff state rather than mixed implementation work: `git log --oneline --decorate -n 12` shows the latest task-specific commits are PO/po-critic workflow commits, and `git diff --name-only develop..HEAD` lists only .gicket/tickets/06F0MEF08AJ1K52STF42T74B04/* files.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- An explicit acceptance-test example for the public happy path `AddDVault(options => UseMetadataRegistry(importedRegistry))` plus `DbContextOptionsBuilder.UseDataVaultMetadata()` would make the supported import-to-runtime flow easier to verify.
- An explicit post-parse failure example where JSON parsing succeeds but registry-build or EF projection fails would make the required logical-declaration plus JSON Pointer/source-path remap fully concrete.
- One worked recursive self-link example that preserves distinct participant roles through hierarchy/bridge projection would reduce implementation guesswork for the narrow model-first-only adapter path.

Risky assumptions
- The contract assumes the optional logical source path is diagnostic-only and does not become part of authoritative-source identity or metadata fingerprint semantics.
- The contract assumes imported `loadTimestampStorage` should be carried by registry provider capability profiles rather than by a separate runtime override path.
- The contract assumes recursive-role handling can stay additive/internal even though public `DataVaultLinkParticipantMetadata` exposes only `HubReference` and no participant role (src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs:108-126,640-716).

AC / test suggestions
- Exercise all three `loadTimestampStorage` tokens across the built-in provider profiles already covered by the repo, not just the default SQLite path.
- Add parity tests that compare imported-model, metadata-first, and Code-First relational shape on the shared fluent subset, then separate imported-model vs metadata-first tests for link-parent satellites, PIT, bridges, and recursive-role cases.
- Add conflict-behavior tests for both identical-source idempotence and mismatched-source rejection on `ModelBuilder.ApplyDataVaultMetadata(...)` and `DbContextOptionsBuilder.UseDataVaultMetadata(...)` paths.
- Assert that post-parse failure diagnostics include the failing logical declaration plus artifact path or JSON Pointer, not only an exception message or DMV code.

Implementation watchouts
- Current parser code creates the registry with `DataVaultMetadataRegistry.Create(metadataModel)` only (src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs:152-155), so imported `loadTimestampStorage` is currently not carried into registry provider profiles by default.
- Current post-parse capability failure is collapsed to root-path `DMV1501` with `string.Empty` path in src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs:161-167; the ticket correctly requires richer source mapping than the current baseline.
- Registry-backed projection records authoritative-source fingerprints and rejects mixed sources in src/DCoding.Data.DVault/DataVaultMetadataSourceAnnotations.cs:16-43, so any import wrapper must preserve that behavior rather than bypass it.
- Recursive-role and hierarchy bridge cases remain sensitive because bridge endpoint bindings keep `Role` and `SourceEndpointName` internally (src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs:157-187) while public link participant metadata does not expose roles (src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs:108-126,640-716).

Non-blocking notes
- No blocking ticket-comment dispute was found; the local comments under .gicket/tickets/06F0MEF08AJ1K52STF42T74B04/comments/ are workflow/refinement records only.
- No split is currently justified by repository evidence because the contract already fences export/drift/governance to downstream tickets and narrows advanced imported-model-only parity work to metadata-first comparisons where Code-First has no public surface.

Split recommendations
- No split recommended. Keep this ticket as the additive public import-to-registry/import-to-EF handoff, and leave export, drift, and governance on 06F0MEFHKF04B746X7GJKRVT04, 06F0MEFX5M9V9SA25N76CPGT4M, and 06F0MEGAGJCEHQ8QRHGH8W7804 respectively.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment