[gicket-bot] PO-critic review contract

Summary
- Delivery contract is specific, bounded, and ready for developer handoff; repository evidence confirms the v0.27 documentation roll-forward targets are clear even though no implementation changes are on the branch yet.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted ticket `06F8KZHAB717MJJNAWWK7S0A5W` sets `PO Handoff` to `ready_for_po_critic`, lists `## Open Questions` as `none`, and names the exact target surfaces and acceptance criteria for `README.md`, `docs/production-adoption-checklist.md`, `docs/releases/v0.27.0.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/architecture/dvault-ef-compiled-compatibility.md`, and `dvault-ef-compiled-compatibility.md`.
- `README.md` still carries `0.26.0` install snippets and says `DVault v0.26.0` is the current coordinated release baseline, while already documenting `DMV1912` through `DMV1914` lifecycle guidance (`README.md` matches at lines reported by `rg`: 21, 25, 608, 610, 878, 1034).
- `docs/production-adoption-checklist.md` still says `v0.26.0` is the current public baseline and its analyzer-package bullet still names only `DMV1910`/`DMV1911` plus `DMV1960` through `DMV1969` (`docs/production-adoption-checklist.md:9` and `:11`).
- `src/DCoding.Data.DVault.Analyzers/README.md` already documents `DMV1912` through `DMV1914` as high-confidence, source-visible lifecycle diagnostics and still shows analyzer package version `0.26.0` in the installation snippet.
- `docs/architecture/dvault-ef-compiled-compatibility.md` already contains the authoritative lifecycle contract, including analyzer-only/no-runtime-change wording, fixed-shape `UseModel(...)` guidance, and fixed-shape `AddDbContextPool<TContext>(...)` guidance; `dvault-ef-compiled-compatibility.md` remains a lightweight entrypoint pointing back to that note.
- Validation evidence already exists in `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs` for supported diagnostics `DMV1910` through `DMV1914` and in `tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs` for runtime-model initialization, `UseModel(...)`, compiled-query reads over `HubOrder`, and explicit `IModelCacheKeyFactory` handling.
- Branch-history checks show no implementation has started yet: `git show --stat --oneline --no-patch ticket/06F8KZHAB717MJJNAWWK7S0A5W-task-update-v0-27-0-analyzer-and-ef-lifecycle-do` and `git rev-parse` both resolve to `854425f4cc26e0a895ff86571f7f48eb18bde2bf`, `git diff --name-status 854425f4cc26e0a895ff86571f7f48eb18bde2bf..ticket/06F8KZHAB717MJJNAWWK7S0A5W-task-update-v0-27-0-analyzer-and-ef-lifecycle-do -- README.md docs/production-adoption-checklist.md docs/releases src/DCoding.Data.DVault.Analyzers/README.md dvault-ef-compiled-compatibility.md docs/architecture/dvault-ef-compiled-compatibility.md tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs` returned no changed files, and `rg --files /mnt/c/Projects/DVault/docs/releases | rg 'v0.27.0.md$'` returned no match.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- This review assumes the contract-listed documentation surfaces are the intended handoff scope; no additional hidden 'current baseline' document surfaced in the bounded repository checks performed here.
- This review relies on the persisted contract statement that related tickets `06F8KZGC4NY41PRYB2RP00ZA1M`, `06F8KZGNRG5FY4WWCY3FAX2NS4`, and `06F8KZGZND5ZCH147PVBRWXYN4` are already done; no reopening evidence appeared in the retrieved comments.

AC / test suggestions
- When dev updates the docs, run a repo text check across the named surfaces for `v0.26.0`, `v0.27.0`, `DMV1912`, `DMV1913`, and `DMV1914` so the 'current baseline' wording is consistent and older release sections remain explicitly historical.
- In `docs/releases/v0.27.0.md`, cite `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs` and `tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs` directly in the validation evidence section, as the contract already expects.

Implementation watchouts
- The branch currently contains no documentation delivery changes, so dev must add `docs/releases/v0.27.0.md` and update the existing current-baseline references from `0.26.0` to `0.27.0` without rewriting older release notes as current guidance.
- `docs/production-adoption-checklist.md` currently lags the lifecycle story; its analyzer-package bullet needs to align with the README/analyzer README/architecture note on `DMV1912` through `DMV1914` while preserving the carried-forward `DMV1950` through `DMV1955` and `DMV1960` through `DMV1969` references where already in scope.
- Keep `dvault-ef-compiled-compatibility.md` as an entrypoint only; the authoritative lifecycle contract should remain in `docs/architecture/dvault-ef-compiled-compatibility.md` to avoid parallel prose drift.
- Preserve the existing no-publication disclaimer when updating versioned install snippets to `0.27.0` so the docs do not imply NuGet publication evidence that the repository does not provide.

Non-blocking notes
- Retrieved ticket comments were automation/refinement and handoff records; no newer human clarification or reopened PO question was present beyond the persisted delivery contract.
- The current repository already contains the underlying analyzer, architecture-note, and SQLite integration-test evidence, which supports treating this as a bounded documentation-alignment handoff rather than new product-definition discovery.

Split recommendations
- No split recommended; the persisted contract already keeps docs alignment separate from analyzer implementation, runtime behavior, and fixture work, and the repository evidence supports that bounded slice.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment