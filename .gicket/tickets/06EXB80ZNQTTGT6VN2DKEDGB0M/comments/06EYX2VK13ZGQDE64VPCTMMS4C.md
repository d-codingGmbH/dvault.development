[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the story is bounded, child task coverage is complete, and the persisted contract has no unresolved open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The three child tickets named in the parent contract are all persisted as `done`: `06EXB817Q8RAXCQH5QQR5RFY34`, `06EXB81FSWAA6N1HMYQ0CM4S8G`, and `06EXB81QXE7XJPNM6NTPYCTP1M`.
- Repository source confirms the package boundary: `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` and the five provider `.csproj` files each contain `GenerateDocumentationFile=true` and `WarningsAsErrors ... CS1591`, while `src/DCoding.Data/DCoding.Data.csproj` contains `IsPackable=false`.
- Direct source evidence matches the contract's public API baseline: `AddDVault` is in `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs`, `UseDataVault` and `ApplyDataVaultMetadata` are in `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs`, `IDataVaultSaveService` is in `src/DCoding.Data.DVault/DataVaultSaveService.cs`, `IDataVaultProviderSaveStrategy` is in `src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs`, and each provider package exposes its own `AddDVault*` method in its extension file.
- `tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs` asserts separate public-API snapshots for `DCoding.Data.DVault`, `Sqlite`, `Postgres`, `SqlServer`, `Oracle`, and `MySql`, and `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/` contains six matching approved baseline files.
- `tools/check-format.sh` invokes `bash tools/check-one-member-per-file.sh` and `dotnet format DVault.slnx --verify-no-changes --no-restore`; `docs/quality/one-member-per-file.md` and `docs/quality/one-member-per-file-exceptions.txt` document the six in-scope roots and seven retained core-package exceptions.
- Branch history is consistent with a PO/critic handoff rather than unresolved source planning: `git rev-parse HEAD` returned `289b1f8fa52ef016ad5873acdfb56258772cadba`, and `git diff --stat 0151f1e3c54f..289b1f8fa52ef016ad5873acdfb56258772cadba` showed only `.gicket/**` ticket, comment, and event changes.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Published-package compatibility against released NuGet artifacts is explicitly deferred to a later follow-up decision.
- Future handling of newly added packable provider projects is intentionally left open between auto-discovery and explicit allowlist updates.

Risky assumptions
- The current six-project allowlist remains authoritative; adding another packable provider project will require coordinated updates in docs and shell checks.
- Shared MSBuild and shell gates remain packable-project-scoped; broadening them without conditions could pull non-packable `src/DCoding.Data`, tests, or benchmarks into enforcement.

AC / test suggestions
- During dev handoff, verify the XML-doc gate with build or pack output across all six packable projects, not only by checking project-file settings.
- Keep API approval evidence assembly-scoped per package, matching `ApiSurfaceSnapshotTests` and the six approved snapshot files.
- Keep one-member-per-file validation covering both violating source files and stale exception-list entries, since the current shell check enforces both behaviors.

Implementation watchouts
- Provider packages share the `DCoding.Data.DVault` namespace, so public-API review must stay package or assembly scoped rather than namespace scoped.
- `tools/check-one-member-per-file.sh` hardcodes the six roots and explicit glob depths for `.cs` discovery; deeper future folder layouts or new packable projects will need an intentional update.

Non-blocking notes
- Current branch HEAD `289b1f8fa52ef016ad5873acdfb56258772cadba` is a PO-critic lease-claim commit; no additional `src/`, `tests/`, `docs/`, or `tools/` changes were observed after the PO handoff commit range checked above.

Split recommendations
- No additional split recommended; the parent story is already decomposed into done child tickets `06EXB817Q8RAXCQH5QQR5RFY34`, `06EXB81FSWAA6N1HMYQ0CM4S8G`, and `06EXB81QXE7XJPNM6NTPYCTP1M`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment