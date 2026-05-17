[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F2PGHJAFMH80TZAMANQWH9PW/description.md` contains the persisted delivery contract, and its `## Open Questions` section is exactly `- none`.
- Local `rg -n` repository reads found the documented analyzer/generator baseline in source: `src/DCoding.Data.DVault.Analyzers/DataVaultMappingDiagnosticCatalog.cs:9,54` defines `DMV1950` and `DMV1955`; `src/DCoding.Data.DVault.Analyzers/DataVaultMappingSourceGenerator.cs:453-534` emits `IDataVaultHubMapper<TSource>`, `IDataVaultLinkMapper<TSource>`, and `IDataVaultSatelliteMapper<TSource>` helpers; and `src/DCoding.Data.DVault/DataVaultHubMappingAttribute.cs`, `DataVaultLinkMappingAttribute.cs`, and `DataVaultHubSatelliteMappingAttribute.cs` are present.
- Local `rg -n` reads found public guidance aligned with that baseline in `README.md:21,109,483-486,494`, `src/DCoding.Data.DVault.Analyzers/README.md:5-9,17,35-39`, and `docs/releases/v0.12.0.md:22-26,31,44-48,54-59,93-97`, including optional analyzer-package usage with `PrivateAssets="all"` and the preserved explicit `IDataVaultSaveService` boundary.
- Local `rg -n` reads found the named verification anchors in the repository: `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultCodeFirstAnalyzerTests.cs`, `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultMappingSourceGeneratorTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultTypedMapperContractTests.cs`, and `tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedMapperSaveServiceSqliteTests.cs`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A runnable end-to-end consumer example for compile-time mapping attributes and generated mappers is still only a deferred item under `## Follow-Up Questions` in `.gicket/tickets/06F2PGHJAFMH80TZAMANQWH9PW/description.md`; that is a follow-on example gap, not a handoff blocker.
- Later shapes called out in `### Scope Out` and downstream tickets (for example link-parent satellites, effectivity satellites, same-as links, and dependent child key modeling) are intentionally excluded from this closure epic rather than covered here.

Risky assumptions
- Assumes the extra live blocks edge to `.gicket/relations/PW/V0/06F2PGHJAFMH80TZAMANQWH9PW--06F2PGM9038RXVJH0RJFYEJEV0--blocks.json` is intentional even though the contract prose names only five downstream parity tickets in its clarification list.
- Assumes no further PO clarification is needed because the persisted contract has `## Open Questions` = `none` and the observed comment history contains no human-authored clarifications to reconcile.

AC / test suggestions
- For downstream parity tickets, keep the explicit-save boundary as an acceptance criterion: generated helpers must not hide caller-supplied `loadTimestamp`, `recordSource`, `DbContext`, or `IDataVaultSaveService` (`README.md:109,483-486`; `docs/releases/v0.12.0.md:26,48`).
- When follow-on analyzer/generator work starts, extend the existing verification anchors instead of reopening this epic: `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultCodeFirstAnalyzerTests.cs`, `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultMappingSourceGeneratorTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultTypedMapperContractTests.cs`, and `tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedMapperSaveServiceSqliteTests.cs`.

Implementation watchouts
- This branch is closure-only: `git diff --name-only develop..HEAD` showed ticket-store changes only, so any new implementation should land on child or downstream tickets rather than on epic `06F2PGHJAFMH80TZAMANQWH9PW`.
- Later Code-First parity work should preserve the current public boundary documented in `README.md:109,483-486,494` and `src/DCoding.Data.DVault.Analyzers/README.md:35-39`, especially the explicit `IDataVaultSaveService` orchestration boundary.

Non-blocking notes
- The live relation graph includes downstream documentation task `06F2PGM9038RXVJH0RJFYEJEV0` in addition to the five downstream parity tickets named in the contract prose; this is worth noting but does not make the current closure epic ambiguous.

Split recommendations
- No additional split is needed; the existing direct and nested child structure is already materialized and closed.
- Keep later analyzer/generator expansion in downstream epic `06F2PGK4QJ0YGXK5479W83Z2J0` and its child tickets instead of reopening this v0.12 closure epic.
- If a runnable example or broader capability-table docs are wanted later, create separate follow-on docs tickets rather than widening this ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment