[gicket-bot] PO-critic review contract

Summary
- Contract is close, but two source-verified API/behavior gaps still need PO clarification before developer handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- src/DCoding.Data.DVault/DataVaultModelArtifactImporter.cs exposes `public static DataVaultModelImportResult ImportJson(...)`, and src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs exposes `ApplyDataVaultMetadata(DataVaultModelImportResult)` and `ApplyDataVaultMetadata(DataVaultMetadataRegistry)`, which confirms the import/projection boundary cited by the PO contract exists in source.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadataRegistry.cs exposes ordered `Hubs`, `Links`, `Satellites`, `Bridges`, `Pits`, `ProviderCapabilityProfiles`, and also legacy `PointInTimeTables` on the same public source object.
- docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md defines top-level `schemaVersion`, `naming.policy`, `loadTimestampStorage`, `hubs`, `links`, `satellites`, `pits`, and `bridges`; it does not define a top-level representation for legacy `PointInTimeTables`.
- docs/releases/v0.6.0.md says `There is no public Code-First-to-registry conversion API in v0.6.0`, and `A public Code-First-to-registry bridge is not delivered in v0.6.0`.
- src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs shows the type has an `internal` constructor and an `internal DataVaultMetadataModel BuildMetadataModel()` method, so public callers currently cannot obtain a `DataVaultMetadataModel` or `DataVaultMetadataRegistry` directly from Code-First declarations.
- `git show --stat --name-only --oneline 64c84ca24470` shows the PO handoff commit only changed `.gicket/tickets/06F0MEFHKF04B746X7GJKRVT04/...` metadata files, and `git log --oneline --decorate -n 6 HEAD` shows current HEAD `b7b93725e` is the PO-critic claim on top of that handoff.

Blocking findings
- The ticket requires Code-First-originated export through the registry/model path, but the repository explicitly documents that no public Code-First-to-registry bridge exists (`docs/releases/v0.6.0.md`), and the only direct model-building API is internal (`src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs`). The ticket does not say whether resolving that public caller gap is in scope, out of scope, or only test-only/internal coverage.
- The source types to be exported expose both `Pits` and legacy `PointInTimeTables` (`src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs`, `src/DCoding.Data.DVault/Modeling/DataVaultMetadataRegistry.cs`), while the target `dvault.model.v1` contract only defines `pits` (`docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md`). The ticket does not define whether point-in-time tables must be rejected, silently omitted, adapted, or documented as unsupported.

Required PO actions
- Clarify the intended public caller journey for `Code-First-originated` export: either keep this ticket limited to exporting already-materialized `DataVaultMetadataRegistry`/`DataVaultMetadataModel`, or explicitly add a public bridge/export entry point for Code-First declarations.
- Add an explicit contract decision for `PointInTimeTables` on export from `DataVaultMetadataModel`/`DataVaultMetadataRegistry`: reject with deterministic diagnostics, omit with public docs, or define an adapter to `pits`.
- Update acceptance criteria and definition-of-done text so the public API promise matches the clarified scope above.

Open issues ledger
- critic-item-1 [required-po-action] Clarify the intended public caller journey for `Code-First-originated` export: either keep this ticket limited to exporting already-materialized `DataVaultMetadataRegistry`/`DataVaultMetadataModel`, or explicitly add a public bridge/export entry point for Code-First declarations.
- critic-item-2 [required-po-action] Add an explicit contract decision for `PointInTimeTables` on export from `DataVaultMetadataModel`/`DataVaultMetadataRegistry`: reject with deterministic diagnostics, omit with public docs, or define an adapter to `pits`.
- critic-item-3 [required-po-action] Update acceptance criteria and definition-of-done text so the public API promise matches the clarified scope above.
- critic-item-4 [blocking-finding] The ticket requires Code-First-originated export through the registry/model path, but the repository explicitly documents that no public Code-First-to-registry bridge exists (`docs/releases/v0.6.0.md`), and the only direct model-building API is internal (`src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs`). The ticket does not say whether resolving that public caller gap is in scope, out of scope, or only test-only/internal coverage.
- critic-item-5 [blocking-finding] The source types to be exported expose both `Pits` and legacy `PointInTimeTables` (`src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs`, `src/DCoding.Data.DVault/Modeling/DataVaultMetadataRegistry.cs`), while the target `dvault.model.v1` contract only defines `pits` (`docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md`). The ticket does not define whether point-in-time tables must be rejected, silently omitted, adapted, or documented as unsupported.

Missing examples / edge cases
- A model/registry that contains legacy `PointInTimeTables` but no `Pits`.
- A registry built with `DataVaultMetadataRegistry.Create(metadataModel)` and therefore no provider profiles; confirm whether export must emit `loadTimestampStorage: provider-default`.
- A registry built via `DataVaultMetadataRegistryBuilder` with custom or mixed `DataVaultProviderCapabilityProfile` entries that do not map cleanly to one `loadTimestampStorage` token.

Risky assumptions
- Assuming `Code-First support` can be satisfied without any public API change even though the public repository contract currently says there is no public Code-First-to-registry bridge.
- Assuming every exportable registry carries enough provider-profile information to derive one canonical `loadTimestampStorage` token.
- Assuming callers will accept silent omission of legacy point-in-time table metadata even though it is present on the public source model/registry types.

AC / test suggestions
- Add one acceptance criterion and test that fixes the public Code-First story: either `export accepts only registry/model inputs` or `export can be invoked from Code-First declarations through a named public API`.
- Add a deterministic behavior test for exporting a source that contains `PointInTimeTables`.
- Add a test that covers registry export when provider profiles are absent, and another when custom profiles make `loadTimestampStorage` ambiguous.

Implementation watchouts
- Do not infer a public Code-First export path from internal/test-only access; source currently exposes only the `ApplyDataVaultMetadata(vault => ...)` callback publicly.
- Do not conflate legacy `PointInTimeTables` with `Pits`; both exist in source, but only `pits` exists in `dvault.model.v1`.
- If `loadTimestampStorage` is derived from provider profiles, the derivation must be deterministic and documented for empty, custom, or mixed profile sets.

Non-blocking notes
- The persisted delivery contract is otherwise in good shape: `.gicket/tickets/06F0MEFHKF04B746X7GJKRVT04/description.md` has `Open Questions: none` and clear scope/AC sections.
- Current comment history under `.gicket/tickets/06F0MEFHKF04B746X7GJKRVT04/comments/` is orchestration plus PO-refinement only; I did not find a later comment that resolves the two ambiguities above.

Split recommendations
- If product wants end-user export directly from Code-First declarations, split that public bridge into a separate ticket from the registry/model exporter so the export contract can stay narrow and deterministic.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment