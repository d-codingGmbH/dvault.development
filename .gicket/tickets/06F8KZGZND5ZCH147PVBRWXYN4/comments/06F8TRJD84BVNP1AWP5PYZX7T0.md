[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff. The delivery contract is concrete, `## Open Questions` is `none`, repository evidence matches the intended fixture gap, and the remaining inconsistencies are non-blocking metadata/history details.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F8KZGZND5ZCH147PVBRWXYN4/description.md` contains concrete scope and acceptance criteria for DMV1912-DMV1914 and has `## Open Questions` -> `none`.
- `git log --oneline --decorate -n 8 HEAD` on `ticket/06F8KZGZND5ZCH147PVBRWXYN4-story-add-ef-lifecycle-analyzer-fixtures-and-reg` shows HEAD `ea20e5615`, earlier PO handoff commit `f9e0791ff`, and the earlier PO-claim commit `762b610ef`.
- `git diff --name-only 762b610ef6a278348cf9238e6227a455abb26650..HEAD` listed only `.gicket/tickets/06F8KZGZND5ZCH147PVBRWXYN4/*`, so there is still no `src/`, `tests/`, or `docs/` implementation delta on this story branch.
- `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs` already contains DMV1912/DMV1913/DMV1914 assertions plus direct `ApplyDataVaultMetadata(...)`, `UseModel(...)`, and `AddDbContextPool<TContext>(...)` cases; `rg -n "UseDataVaultMetadata|DataVaultModelImportResult|DataVaultMetadataModel|DataVaultMetadataRegistry" tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs` returned no hits.
- `tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs:21,56,60,81,85,107,125,158,177,191` directly exercises `UseDataVaultMetadata()`, explicit registry selection, and successful `DataVaultModelImportResult` import/projection baselines.
- `src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs:34-53` shows `UseDataVaultMetadata(DataVaultMetadataModel)` delegates to `DataVaultMetadataRegistry.Create(metadataModel)` and `UseDataVaultMetadata(DataVaultModelImportResult)` delegates to `RequireMetadataRegistry()`, giving direct source evidence that those overloads are registry-backed paths.
- `README.md` in the `Isolate EF model cache entries` section and `docs/architecture/dvault-ef-compiled-compatibility.md` both state that `UseDataVaultMetadata()`, `UseDataVaultMetadata(DataVaultMetadataRegistry)`, `UseDataVaultMetadata(DataVaultMetadataModel)`, and `UseDataVaultMetadata(DataVaultModelImportResult)` are the built-in non-diagnostic model-cache-isolated baselines.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The exact `UseDataVaultMetadata(DataVaultMetadataModel)` non-diagnostic lane is not explicitly present in `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs` today and is worth keeping explicit in the new fixture matrix, even though `src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs` shows it delegates to a registry-backed path.
- No other obvious fixture gap beyond the ones already listed in the delivery contract was found.

Risky assumptions
- The description still cites scratch-source ref `762b610ef6a278348cf9238e6227a455abb26650`, while current branch HEAD is `ea20e5615d42d416207d075c241549ce7cf46701`; approval assumes developers rely on current branch history and the observed file diff rather than that older hash literal.

AC / test suggestions
- Keep one positive and one non-diagnostic case per rule plus dedicated safe-lane fixtures for the documented design-model-to-runtime-model `UseModel(...)` path and fixed options-only `AddDbContextPool<TContext>(...)` path.
- Keep read-only generated-table query/compiled-query and metadata-interceptor opt-in cases in the non-diagnostic set, because those are already documented baselines and partially represented in the current analyzer test file.

Implementation watchouts
- Do not let the story drift into helper expansion, cross-assembly inference, pooled-factory analysis, or raw `dvault.model.v1` parsing inside the analyzer; the contract and `docs/architecture/dvault-ef-compiled-compatibility.md` keep the rule boundary at direct source-visible facts.
- Preserve the distinction between caller-owned variable-shape `ApplyDataVaultMetadata(...)` / `UseModel(...)` / `AddDbContextPool<TContext>(...)` lanes and registry-backed `UseDataVaultMetadata(...)` lanes, or the new fixtures will blur the documented non-diagnostic baseline.
- The branch currently carries ticket-metadata commits only, so developer work should start from the existing analyzer/test baseline rather than assuming partial story implementation is already present.

Non-blocking notes
- The follow-up documentation concern appears to be intentionally split already: `.gicket/tickets/06F8KZHAB717MJJNAWWK7S0A5W/ticket.json` is `Task: Update v0.27.0 analyzer and EF lifecycle documentation`, and `.gicket/relations/N4/5W/06F8KZGZND5ZCH147PVBRWXYN4--06F8KZHAB717MJJNAWWK7S0A5W--blocks.json` shows this story blocks that doc task.

Split recommendations
- No further split recommended. Visible ticket structure already separates lifecycle implementation (`06F8KZGNRG5FY4WWCY3FAX2NS4`, done), broader fixture expansion (`06F8KZGZND5ZCH147PVBRWXYN4`, this story), and documentation follow-up (`06F8KZHAB717MJJNAWWK7S0A5W`, todo).

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment