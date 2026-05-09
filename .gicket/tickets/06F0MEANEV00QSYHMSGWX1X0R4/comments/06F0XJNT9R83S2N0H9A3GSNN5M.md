[gicket-bot] PO-critic review contract

Summary
- Return to PO: the contract is largely aligned with the repo, but it still overstates the code-first-to-registry public contract and leaves point-in-time lookup scope ambiguous.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- The persisted contract is present in `.gicket/tickets/06F0MEANEV00QSYHMSGWX1X0R4/description.md:1-67`, and `## Open Questions` is resolved to `none` at lines 51-52.
- Live `parentOf` relations exist from `06F0MEANEV00QSYHMSGWX1X0R4` to `06F0MEAXT99V0P115P0WEJD4P0`, `06F0MEB634X6CTBZ00W108G3FG`, and `06F0MEBFTW8FY5T7PY5HJ5JXJ4` in `.gicket/relations/R4/P0/...:1-11`, `.gicket/relations/R4/FG/...:1-11`, and `.gicket/relations/R4/J4/...:1-11`.
- Those three child tickets are all `done` in their persisted snapshots: `.gicket/tickets/06F0MEAXT99V0P115P0WEJD4P0/ticket.json:3-20`, `.gicket/tickets/06F0MEB634X6CTBZ00W108G3FG/ticket.json:3-20`, and `.gicket/tickets/06F0MEBFTW8FY5T7PY5HJ5JXJ4/ticket.json:3-20`.
- Direct source evidence backs the registry baseline: `src/DCoding.Data.DVault/Modeling/DataVaultMetadataRegistry.cs:7-60` defines immutable hub/link/satellite/point-in-time/bridge/PIT/provider-profile state, and `:98-252` exposes the public `Create`, `TryGet*`, and parent-scoped satellite lookup API.
- Direct source evidence backs the DI/EF registration path: `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-49`, `src/DCoding.Data.DVault/DataVaultOptions.cs:66-81`, and `src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs:16-60`; README examples also document this path in `README.md:70-101`.
- Registry-backed projection and conflict handling are directly evidenced by `src/DCoding.Data.DVault/DataVaultMetadataSourceAnnotations.cs:15-44,70-80`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataRegistrationTests.cs:13-54`, and `tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs:14-86`.
- Code-first normalization is only internally exposed in source: `src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs:14,58-72` shows an internal constructor and internal `BuildMetadataModel()`, while the public entrypoints are the EF translation overloads in `src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilderExtensions.cs:16-30` and `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:95-105`.
- `git log --oneline --decorate --no-abbrev-commit -n 6 ticket/06F0MEANEV00QSYHMSGWX1X0R4-story-introduce-data-vault-model-registry` shows the current branch contains PO claim/handover commits above `develop`, while the implementation work is already auto-integrated below via child-ticket commits `c5c2e3155`, `b2c3bf6c2`, and `c901e5520`.

Blocking findings
- The contract says code-first declarations normalize to `DataVaultMetadataModel` and frames that as already-established baseline behavior (`description.md:22,34,47`), but direct public source evidence does not expose a caller-usable code-first-to-`DataVaultMetadataModel` or code-first-to-`DataVaultMetadataRegistry` API. `DataVaultCodeFirstModelBuilder` has an internal constructor and internal `BuildMetadataModel()` in `src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs:14,58-72`, and the only public code-first entrypoints immediately translate to EF metadata in `src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilderExtensions.cs:16-30` and `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:95-105`. The ticket must clarify whether code-first compatibility is only internal EF normalization or whether new public export/registration behavior is intended.
- The contract uses `current point-in-time metadata` (`description.md:13,33`) without naming which public lookup family is in scope, but the registry publicly exposes two separate families: `PointInTimeTables`/`TryGetPointInTimeTable` and `Pits`/`TryGetPit` in `src/DCoding.Data.DVault/Modeling/DataVaultMetadataRegistry.cs:17-22,78-91,202-252`. Tests also assert both lookup domains in `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataRegistryTests.cs:67-73,121-126`. The scope needs explicit type/API wording before developer handoff.

Required PO actions
- Rewrite the code-first compatibility wording in the delivery contract so it matches the public source surface: either limit the claim to internal normalization during EF translation, or explicitly require a new public export/registration path if that is the intended scope.
- Replace `current point-in-time metadata` with the concrete in-scope public type/API names: `DataVaultPointInTimeMetadata`/`TryGetPointInTimeTable`, `DataVaultPitMetadata`/`TryGetPit`, or both.
- After the two scope statements above are corrected, recheck the acceptance criteria and Definition of Done so dev/test do not infer unplanned public API work from the parent story.

Open issues ledger
- critic-item-1 [required-po-action] Rewrite the code-first compatibility wording in the delivery contract so it matches the public source surface: either limit the claim to internal normalization during EF translation, or explicitly require a new public export/registration path if that is the intended scope.
- critic-item-2 [required-po-action] Replace `current point-in-time metadata` with the concrete in-scope public type/API names: `DataVaultPointInTimeMetadata`/`TryGetPointInTimeTable`, `DataVaultPitMetadata`/`TryGetPit`, or both.
- critic-item-3 [required-po-action] After the two scope statements above are corrected, recheck the acceptance criteria and Definition of Done so dev/test do not infer unplanned public API work from the parent story.
- critic-item-4 [blocking-finding] The contract says code-first declarations normalize to `DataVaultMetadataModel` and frames that as already-established baseline behavior (`description.md:22,34,47`), but direct public source evidence does not expose a caller-usable code-first-to-`DataVaultMetadataModel` or code-first-to-`DataVaultMetadataRegistry` API. `DataVaultCodeFirstModelBuilder` has an internal constructor and internal `BuildMetadataModel()` in `src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs:14,58-72`, and the only public code-first entrypoints immediately translate to EF metadata in `src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilderExtensions.cs:16-30` and `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:95-105`. The ticket must clarify whether code-first compatibility is only internal EF normalization or whether new public export/registration behavior is intended.
- critic-item-5 [blocking-finding] The contract uses `current point-in-time metadata` (`description.md:13,33`) without naming which public lookup family is in scope, but the registry publicly exposes two separate families: `PointInTimeTables`/`TryGetPointInTimeTable` and `Pits`/`TryGetPit` in `src/DCoding.Data.DVault/Modeling/DataVaultMetadataRegistry.cs:17-22,78-91,202-252`. Tests also assert both lookup domains in `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataRegistryTests.cs:67-73,121-126`. The scope needs explicit type/API wording before developer handoff.

Missing examples / edge cases
- If code-first participation in the `one authoritative registry` path is truly in scope, the contract is missing a caller-facing example showing how a code-first user obtains or registers that single registry during `AddDVault(...)` service setup.
- The contract does not include an explicit example for the metadata-source conflict edge case that repo tests already cover in `tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs:73-86`.
- The contract does not distinguish legacy point-in-time table lookup from PIT lookup with an explicit example or AC statement.

Risky assumptions
- Assuming internal code-first normalization implies an existing public code-first-to-registry contract; direct source evidence does not currently support that.
- Assuming `current point-in-time metadata` is self-explanatory even though the public registry surface splits point-in-time tables and PIT metadata into different lookup APIs.

AC / test suggestions
- Add one acceptance criterion that states the exact caller contract for code-first compatibility: internal EF-only normalization, or a public way to obtain/register a `DataVaultMetadataModel` or `DataVaultMetadataRegistry` from code-first declarations.
- Add explicit AC/test wording for whichever point-in-time family is intended instead of relying on `current point-in-time metadata`.
- Keep the explicit metadata-source conflict expectation, because direct source and integration evidence already support it in `src/DCoding.Data.DVault/DataVaultMetadataSourceAnnotations.cs:15-44` and `tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs:73-86`.

Implementation watchouts
- Registry-backed save/read adapters already depend on one authoritative DbContext registry in `src/DCoding.Data.DVault/DataVaultSaveService.cs:69-73,102-140` and `src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs:26-45`; the ticket wording should not imply a second metadata interpretation path.
- The optionless `AddDVault()` baseline and the registry-backed opt-in path are both real public APIs in `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-49`, `src/DCoding.Data.DVault/DataVaultOptions.cs:66-81`, and `src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs:16-60`.

Non-blocking notes
- The persisted contract has no unresolved `## Open Questions` in `.gicket/tickets/06F0MEANEV00QSYHMSGWX1X0R4/description.md:51-52`.
- The live parent-child structure matches the contract, and all three current children are already `done` in their persisted ticket snapshots.
- README and existing tests already back the registry-backed DI/EF path and the reuse of the existing translation pipeline.

Split recommendations
- Keep the current three-child split under `06F0MEANEV00QSYHMSGWX1X0R4`; repository relations and child statuses support it.
- Keep broader parity/regression breadth on `06F0MEAD1BAA5QEVM3F9QJA38G` and do not let parent-story wording pull that scope back in.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment