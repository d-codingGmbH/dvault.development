[gicket-bot] PO-critic review contract

Summary
- The ticket is grounded in real repo assets and has no open questions, but the developer handoff is still under-specified because the required selectable category/group mechanism is not evidenced for the current xUnit v3 Microsoft Testing Platform runner.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB80FPE3REH11RQ1YR6BW1G/description.md` contains `## Open Questions` with `- none`.
- `git -C /mnt/c/Projects/DVault log --oneline` on the ticket path shows `<redacted> [06EXB80FPE3REH11RQ1YR6BW1G] handoff po->po-critic`; `git show --stat <redacted>` changed only `.gicket/tickets/06EXB80FPE3REH11RQ1YR6BW1G/*` files, so the branch state is ticket-refinement-only.
- Relation files `.gicket/relations/MG/1G/06EXB807MN08HABHTHVPKKNFMG--06EXB80FPE3REH11RQ1YR6BW1G--parentOf.json`, `.gicket/relations/1G/S0/06EXB80FPE3REH11RQ1YR6BW1G--06EXB80QQHAYH61RY4X3T1E8S0--blocks.json`, and `.gicket/relations/MR/1G/06EXB76NNRDP7WH1F2R5VYYPMR--06EXB80FPE3REH11RQ1YR6BW1G--blocks.json` match the parent/downstream/upstream relations in the contract; `.gicket/tickets/06EXB76NNRDP7WH1F2R5VYYPMR/ticket.json` shows the upstream hashing ticket is `done`.
- `find tests/DCoding.Data.DVault.Tests -maxdepth 2 -type f` lists the cited scope files: `Unit/DataVaultMetadataTests.cs`, `Unit/DataVaultModelBuilderExtensionsTests.cs`, `Unit/DataVaultEfMetadataTranslationTests.cs`, `Unit/StableHashNormalizerTests.cs`, `Unit/StableHashServiceTests.cs`, `Unit/ExplicitDataVaultSaveServiceTests.cs`, `Unit/DataVaultProviderCapabilityProfileTests.cs`, plus `Modeling/DefaultNamingPolicyTests.cs` and `Modeling/NamingPolicyTests.cs`; `Unit/ConventionFirstEntryPointCoverageTests.cs` bridges the two modeling harnesses.
- `tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj` uses `xunit.v3.mtp-v1` with `UseMicrosoftTestingPlatformRunner=true` and only links `../Modeling/*.cs`; `tests/DCoding.Data.DVault.Tests/TechnicalMetadataColumnContractTests.cs` exists as a standalone harness, and a repository search for `TechnicalMetadataColumnContractTests` under `tests/DCoding.Data.DVault.Tests` returned only that file, so the metadata contract coverage is not yet bridged into the runnable unit project.
- A repository search for `Trait`, `TestCategory`, or `Category(` under `tests/DCoding.Data.DVault.Tests` returned no matches, and a search for `--filter`, `trait`, or `category` in `README.md`, `tests/DCoding.Data.DVault/README.md`, and `tests/DCoding.Data.DVault.Tests/TechnicalMetadataColumnContracts.md` also returned no matches.
- Provider baseline claims are directly evidenced: `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs` exposes `AddDVault()`, `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs` adds `IDataVaultProviderSaveStrategy`, the Postgres/SqlServer/Oracle/MySql service-collection extension files only call `AddDVault()`, and `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs` exposes only `DataVaultProviderCapabilityProfiles.Sqlite`.
- Hashing baseline claims are directly evidenced: `tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs` contains published SHA-256 vectors, and `tests/DCoding.Data.DVault.Tests/Unit/StableHashNormalizerTests.cs` covers null, culture, field-order, unsupported-type, and invalid-value cases.

Blocking findings
- The contract's core acceptance is still ambiguous: it requires 'deterministic local categories or equivalent selectable groups' under the current `xunit.v3.mtp-v1` / Microsoft Testing Platform runner, but local repo inspection found no existing trait/category usage, no documented filter syntax, and no repo-local source evidence that the grouping primitive will actually be selectable. The ticket records this as a risk instead of a required acceptance proof.

Required PO actions
- Refine the contract to name the accepted repo-local selection proof for the current runner: either specify the exact grouping mechanism developers must use, or relax the story away from runner-selectable categories if no locally evidenced filterable primitive is required.
- Add one acceptance expectation for discoverability/selectability, such as the expected verification surface for selecting only one unit grouping without pulling in `tests/DCoding.Data.DVault.Tests/Integration`.
- Clarify the required granularity for bridged harnesses: whether one xUnit bridge test is sufficient for the `Modeling/*.cs` and `TechnicalMetadataColumnContractTests.cs` coverage, or whether individual subcases must be independently selectable.

Open issues ledger
- critic-item-1 [required-po-action] Refine the contract to name the accepted repo-local selection proof for the current runner: either specify the exact grouping mechanism developers must use, or relax the story away from runner-selectable categories if no locally evidenced filterable primitive is required.
- critic-item-2 [required-po-action] Add one acceptance expectation for discoverability/selectability, such as the expected verification surface for selecting only one unit grouping without pulling in `tests/DCoding.Data.DVault.Tests/Integration`.
- critic-item-3 [required-po-action] Clarify the required granularity for bridged harnesses: whether one xUnit bridge test is sufficient for the `Modeling/*.cs` and `TechnicalMetadataColumnContractTests.cs` coverage, or whether individual subcases must be independently selectable.
- critic-item-4 [blocking-finding] The contract's core acceptance is still ambiguous: it requires 'deterministic local categories or equivalent selectable groups' under the current `xunit.v3.mtp-v1` / Microsoft Testing Platform runner, but local repo inspection found no existing trait/category usage, no documented filter syntax, and no repo-local source evidence that the grouping primitive will actually be selectable. The ticket records this as a risk instead of a required acceptance proof.

Missing examples / edge cases
- The contract does not show how a developer should prove selection of only the metadata group or only the hashing/provider group under the current runner.
- The ticket does not say whether `tests/DCoding.Data.DVault.Tests/Unit/TestDiscoverySmokeTests.cs` should remain outside the new grouping vocabulary or belong to a specific unit group.
- The ticket does not state whether discoverability/selectability is expected at solution level `dotnet test DVault.slnx`, only at unit-project level `dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj`, or both.

Risky assumptions
- That `xunit.v3.mtp-v1` with `UseMicrosoftTestingPlatformRunner=true` supports a filterable grouping primitive that satisfies the story without extra runner glue.
- That bridging `tests/DCoding.Data.DVault.Tests/TechnicalMetadataColumnContractTests.cs` into the unit surface can preserve the intended metadata-category discoverability without duplicating assertions or changing production behavior.
- That the existing single bridge test `tests/DCoding.Data.DVault.Tests/Unit/ConventionFirstEntryPointCoverageTests.cs` is granular enough for the requested naming/options grouping.

AC / test suggestions
- Add an acceptance criterion that the selected grouping mechanism can be demonstrated locally for at least one unit group under the current runner, not just declared in code.
- Add a definition-of-done note that the technical metadata contract coverage becomes visible from the runnable unit surface in the same way the modeling harness is currently surfaced through `ConventionFirstEntryPointCoverageTests`.
- If the intended proof is runner filtering, require a repo-local example or validation path for unit-only grouping so the story does not depend on undocumented external behavior.

Implementation watchouts
- Do not blur the unit/integration boundary: `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs`, `Integration/SqliteDataVaultSchemaTests.cs`, and `Integration/SqliteProviderCapabilityProfileTests.cs` are real integration surfaces with overlapping subject matter names.
- The unit project already links `tests/DCoding.Data.DVault.Tests/Modeling/*.cs`; grouping work should not orphan that bridge pattern or silently leave `TechnicalMetadataColumnContractTests.cs` outside the runnable unit surface again.
- Provider grouping should stay aligned with the currently visible source contracts only: `AddDVault()`, `AddDVaultSqlite()`, `IDataVaultProviderSaveStrategy`, and `DataVaultProviderCapabilityProfiles.Sqlite`.

Non-blocking notes
- Apart from the runner-selectability ambiguity, the refined contract is well-grounded in real repo paths, real tests, and real provider/type surfaces.
- The parent/blocking relations in `.gicket/relations` and the current status of `06EXB76NNRDP7WH1F2R5VYYPMR` and `06EXB80QQHAYH61RY4X3T1E8S0` are internally consistent with the written scope split.

Split recommendations
- No additional split is needed once the category-selection acceptance proof is clarified; the current unit-scope ticket and downstream integration-category ticket already form the right boundary.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment