[gicket-bot] PO-critic review contract

Summary
- Ready for dev; the delivery contract is specific, evidence-backed, and has no unresolved open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB81QXE7XJPNM6NTPYCTP1M/description.md` contains the delivery contract and `## Open Questions` followed by `- none`.
- `git -C /mnt/c/Projects/DVault rev-parse HEAD` returned `4afffc204c874ea3a79034ab24d465e39266a39e`; `git diff --name-only 276d56aa07bd06f8b5841b817a8a133b66b129bd..HEAD -- src docs README.md tests tools Directory.Build.props Directory.Build.targets Directory.Solution.targets DVault.slnx .editorconfig .gitattributes` returned no paths.
- Project inspection found six packable package projects with `PackageId` values under `src/DCoding.Data.DVault{,.MySql,.Oracle,.Postgres,.Sqlite,.SqlServer}`; `src/DCoding.Data/DCoding.Data.csproj` contains `<IsPackable>false</IsPackable>`.
- Current baseline files already have multiple public/protected top-level declarations: `src/DCoding.Data.DVault/DataVaultAnnotationNames.cs` (2), `DataVaultProviderCapabilities.cs` (7), `DataVaultProviderSaveStrategy.cs` (2), `DataVaultSaveService.cs` (8), `Modeling/DataVaultMetadata.cs` (8), `Modeling/DataVaultModel.cs` (10), and `Modeling/IDataVaultNamingPolicy.cs` (10).
- `src/DCoding.Data.DVault/Modeling/DataVaultModel.cs` and `src/DCoding.Data.DVault/Modeling/DataVaultModelBuilder.cs` both declare `public sealed partial class DataVaultModelBuilder`, matching the documented partial-type case.
- `tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj:11` and `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:11` set `<RunAnalyzers>false</RunAnalyzers>`.
- `README.md:161-164` defines normal local validation as `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo`, `dotnet pack src/DCoding.Data.DVault/DCoding.Data.DVault.csproj --configuration Release --nologo`, and `bash tools/check-format.sh`; `tools/check-format.sh` and `Directory.Build.props` exclude `bin/**` and `obj/**`.
- Repository search `rg -n 'SA1402|one-member|one member|StyleCop|Roslynator|multiple types per file|single type per file' /mnt/c/Projects/DVault` returned `NO_MATCHES`, consistent with the contract note that no existing one-member-per-file rule/config is present.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not give a concrete example of a file that mixes one public declaration with additional internal declarations; `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs` is the current repo edge case to validate.
- The contract requires documented practical exceptions but does not name the eventual exception-list document/location.

Risky assumptions
- This assumes the enforcement path can plug into normal local validation without depending on analyzer execution in test projects, because the DVault test projects explicitly disable analyzers.
- This assumes the future provider-discovery choice can remain a follow-up decision rather than part of v1 scope.
- This assumes the public/protected-only scope will be implemented literally; broadening the rule to all top-level declarations would change the baseline immediately, especially in `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs`.

AC / test suggestions
- Verify one failing example from the core package, one from a provider package, and one documented exception path.
- Verify ignore behavior for `src/DCoding.Data`, test projects, benchmarks, `bin`, and `obj` with path-level assertions in the validation output.
- Verify the `DataVaultModelBuilder` partial case fails unless it is explicitly documented as an allowed exception.

Implementation watchouts
- Count only public/protected top-level declarations; do not accidentally fail files that also contain internal-only companion types.
- Do not rely on test projects for analyzer execution because both DVault test csproj files set `RunAnalyzers=false`.
- Keep project selection aligned to the six packable packages; `src/DCoding.Data/DCoding.Data.csproj` is the non-packable anchor that must stay out of scope.
- Do not leave the current core baseline as a silent pass-through; the listed multi-declaration files must be refactored or explicitly documented before the gate stays enabled.

Non-blocking notes
- The ticket is already separated from sibling XML-doc and API snapshot work; the upstream API snapshot ticket `06EXB81FSWAA6N1HMYQ0CM4S8G` is already done.
- The current branch contains ticket metadata refinement only, so developer work starts from repository baseline rather than a half-implemented change.
- The parent story `06EXB80ZNQTTGT6VN2DKEDGB0M` is still `todo`, but this ticket's own contract is internally coherent and ready for execution.

Split recommendations
- None; this ticket is already the focused downstream work item under story `06EXB80ZNQTTGT6VN2DKEDGB0M`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment