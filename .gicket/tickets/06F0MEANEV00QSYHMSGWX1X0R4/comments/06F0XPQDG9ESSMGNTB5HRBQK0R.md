[gicket-bot] PO-critic review contract

Summary
- Ticket contract is now aligned with the observed public registry, DI/EF integration, and code-first surfaces; the prior PO-critic blockers were addressed and `## Open Questions` is resolved to `none`.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F0MEANEV00QSYHMSGWX1X0R4/description.md` contains the updated delivery contract, explicitly narrows code-first compatibility to the existing EF model-building path, names both lookup families `DataVaultPointInTimeMetadata`/`TryGetPointInTimeTable` and `DataVaultPitMetadata`/`TryGetPit`, and sets `## Open Questions` to `none`.
- The earlier blocking review is recorded in `.gicket/tickets/06F0MEANEV00QSYHMSGWX1X0R4/comments/06F0XJNT9R83S2N0H9A3GSNN5M.md`, and the PO follow-up `.gicket/tickets/06F0MEANEV00QSYHMSGWX1X0R4/comments/06F0XMPSZTKXKB89KJRRC3X670.md` marks critic-item-1 through critic-item-5 as answered before the handoff commit `f7dc6e716b4e`.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadataRegistry.cs` publicly exposes immutable registry collections plus exact-name/CLR lookup APIs for hubs, links, bridges, satellites, `TryGetPointInTimeTable`, and `TryGetPit`, matching the refined contract.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault/DataVaultOptions.cs`, `src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs`, and `README.md` show the additive registration path: optionless `AddDVault()` remains the baseline, while `AddDVault(options => options.UseMetadataModel(...)|UseMetadataRegistry(...))` and `UseDataVaultMetadata(...)` provide registry-backed projection.
- `src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs` keeps the constructor and `BuildMetadataModel()` internal, while `src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilderExtensions.cs` and `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` expose only EF translation entrypoints, which matches the refined contract's narrowed code-first statement.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataRegistryTests.cs`, `tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs`, and `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs` directly cover duplicate-name/dependency/CLR conflict diagnostics, metadata-source conflict handling, registry-backed save/read flows, and context-scoped registry override behavior.
- Live child structure is unchanged and completed where expected: `.gicket/relations/R4/P0/06F0MEANEV00QSYHMSGWX1X0R4--06F0MEAXT99V0P115P0WEJD4P0--parentOf.json`, `.gicket/relations/R4/FG/06F0MEANEV00QSYHMSGWX1X0R4--06F0MEB634X6CTBZ00W108G3FG--parentOf.json`, and `.gicket/relations/R4/J4/06F0MEANEV00QSYHMSGWX1X0R4--06F0MEBFTW8FY5T7PY5HJ5JXJ4--parentOf.json` still point to the same three children, and each child ticket snapshot is `done` in its `ticket.json`.
- Read-only branch inspection shows the current refinement pass changed ticket artifacts only: `git diff --name-only c901e5520..d17858fa2d5daa185fa8ca36f6b4327e3c92f8b2` lists only `.gicket/tickets/06F0MEANEV00QSYHMSGWX1X0R4/*` files.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The ticket text does not include a worked example of an explicit `UseDataVaultMetadata(DataVaultMetadataRegistry)` context override; that behavior is evidenced by source and integration tests rather than a ticket example.
- The ticket now names both point-in-time lookup families, but it still does not show a side-by-side example that contrasts legacy `PointInTimeTables` lookup with `Pits` lookup.

Risky assumptions
- Approval assumes the broader code-first schema-parity regression matrix remains intentionally out of scope on ticket `06F0MEAD1BAA5QEVM3F9QJA38G`, which is still `todo` in `.gicket/tickets/06F0MEAD1BAA5QEVM3F9QJA38G/ticket.json`.
- Approval assumes downstream dev/test keep legacy `PointInTimeTables` and `Pits` as separate public lookup families, because the observed registry API exposes them separately rather than as one merged concept.

AC / test suggestions
- Keep acceptance and test wording tied to the exact public APIs already observed in source: `AddDVault()`, `AddDVault(options => options.UseMetadataModel(...)|UseMetadataRegistry(...))`, `UseDataVaultMetadata(...)`, `TryGetPointInTimeTable`, and `TryGetPit`.
- Retain explicit checks for metadata-source conflicts, missing registry entries before save/read orchestration, parent-scoped satellite lookup, and ambiguous or absent CLR mappings, consistent with the existing unit and integration tests.

Implementation watchouts
- Do not let downstream implementation or documentation re-expand scope into a new public code-first export or registry-registration API; the current public source only supports code-first through EF translation entrypoints.
- Keep app-default registry and context-scoped override conflict behavior immediate and deterministic; the contract depends on source-conflict failure rather than merge or registration-order behavior.
- Keep bridge/PIT deferred-capability wording explicit so representability in the registry is not mistaken for runtime refresh, maintenance, or provider-specific behavior scope.

Non-blocking notes
- The prompt seed said recent comments were `<none>`, but the live repository state contains prior po-critic and PO refinement comments under `.gicket/tickets/06F0MEANEV00QSYHMSGWX1X0R4/comments/`; this assessment is based on that current local state.

Split recommendations
- Keep the existing three-child split to `06F0MEAXT99V0P115P0WEJD4P0`, `06F0MEB634X6CTBZ00W108G3FG`, and `06F0MEBFTW8FY5T7PY5HJ5JXJ4`; live relation files and child statuses support it.
- Keep broader code-first parity/regression breadth on `06F0MEAD1BAA5QEVM3F9QJA38G` instead of pulling that matrix back into this parent.
- If app-startup code-first export or registration is desired later, keep it as a separate follow-up rather than widening this story again.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment