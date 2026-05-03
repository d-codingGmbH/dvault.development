[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff; the ticket is bounded to six observed packable packages, the targeted public surfaces are backed by direct source evidence, existing snapshot conventions give a clear repository pattern to extend, and the persisted contract has no unresolved PO questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `git rev-parse --abbrev-ref HEAD` in `/mnt/c/Projects/DVault` returned `ticket/06EXB81FSWAA6N1HMYQ0CM4S8G-task-add-api-approval-or-compatibility-snapshot`, and `git rev-parse HEAD` returned `e6af709ebf84c55c443b7f9602dc5c505b8e8dec`.
- `git diff --stat develop..HEAD -- . ':(exclude).gicket/**'` returned no non-ticket file changes, so the branch is still the current source baseline rather than a partially implemented solution.
- `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` and the five provider `*.csproj` files declare `PackageId` values `DCoding.Data.DVault`, `.Sqlite`, `.Postgres`, `.SqlServer`, `.Oracle`, and `.MySql`; `src/DCoding.Data/DCoding.Data.csproj` declares `<IsPackable>false</IsPackable>`.
- `DVault.slnx` lines 7-12 include all six packable projects, and lines 17-23 include the integration and unit test projects that can host repository validation.
- Core public API source evidence is direct and current: `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs` exposes `AddDVault`, `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` exposes `UseDataVault` and `ApplyDataVaultMetadata`, `src/DCoding.Data.DVault/DataVaultSaveService.cs` exposes `IDataVaultSaveService`, and `src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs` plus `DataVaultProviderCapabilities.cs` expose provider strategy/profile contracts.
- `rg -n 'public (sealed class|class|record|static class|enum) DataVault(MetadataModel|HubMetadata|LinkMetadata|SatelliteMetadata|MetadataReference|Conventions)|public interface IDataVaultNamingPolicy|public sealed class DefaultNamingPolicy|public sealed class DefaultDataVaultNamingPolicy' src/DCoding.Data.DVault/Modeling` returned public modeling contracts in `Modeling/DataVaultMetadataModel.cs`, `Modeling/DataVaultMetadata.cs`, `Modeling/DefaultDataVaultNamingPolicy.cs`, and `Modeling/IDataVaultNamingPolicy.cs`.
- Provider extension packages expose separate public registration entry points: `AddDVaultSqlite`, `AddDVaultPostgres`, `AddDVaultSqlServer`, `AddDVaultOracle`, and `AddDVaultMySql` in the respective `src/DCoding.Data.DVault.*/*ServiceCollectionExtensions.cs` files.
- `rg -n 'namespace DCoding\.Data\.DVault;' src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs` shows all provider packages share the `DCoding.Data.DVault` namespace, confirming the contract risk that namespace-only snapshots would hide package boundaries.
- Existing committed snapshot practice is already present at `tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs` with baseline file `tests/DCoding.Data.DVault.Tests/Integration/Snapshots/SqliteDataVaultSchemaSnapshot.txt`, and `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` copies that snapshot to output.
- `rg -n 'PublicApiGenerator|PublicApi|ApiCompat|ApprovalTests|VerifyXunit|VerifyTests|ApiApprover' src tests docs Directory.Build.* DVault.slnx README.md` returned no hits, which matches the contract note that no API approval tooling exists yet.
- Latest persisted comments under `.gicket/tickets/06EXB81FSWAA6N1HMYQ0CM4S8G/comments/` are automation and handoff records only, for example `06EYWCS3A5QPP6M28F3HEJQ3JM.md` reports `po-refinement-ready` and `06EYWCXYJTQ7F0CX0K6W4GG7N8.md` records the `po-critic` lease claim.
- `git show --stat --oneline bf617038 -- src/DCoding.Data.DVault src/DCoding.Data.DVault.Sqlite src/DCoding.Data.DVault.Postgres src/DCoding.Data.DVault.SqlServer src/DCoding.Data.DVault.Oracle src/DCoding.Data.DVault.MySql` shows commit `bf617038 Split DVault provider extension packages`, which is the concrete history point that established the six-package baseline this ticket now needs to review.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not spell out a concrete core-only change example; the gate should still fail when only `src/DCoding.Data.DVault` public surface changes and no provider package surface changes.
- The contract does not spell out a concrete one-provider-only change example; changing only `AddDVaultSqlite()` should require only the SQLite package baseline update.
- Future addition of a new packable `src/DCoding.Data.DVault.*` provider package still needs the allowlist-vs-auto-discovery behavior resolved during implementation, as already noted in `## Follow-Up Questions`.

Risky assumptions
- The chosen approval mechanism can emit deterministic per-assembly or per-package baselines for all six `net10.0` packages despite the shared `DCoding.Data.DVault` namespace.
- Repository validation can run the API surface check from compiled output without machine-specific ordering or formatting noise.

AC / test suggestions
- Prove the core-package path by intentionally changing a public core contract such as `IDataVaultSaveService` and requiring only the core baseline update.
- Prove the provider-package path by intentionally changing a provider extension such as `AddDVaultSqlite()` and requiring only that provider baseline update.
- Keep baseline artifacts in a deterministic location beside the owning tests or contract checks, mirroring the existing committed snapshot pattern used by `tests/DCoding.Data.DVault.Tests/Integration/Snapshots/SqliteDataVaultSchemaSnapshot.txt`.

Implementation watchouts
- Group approval output by built package or assembly, not by namespace, because the provider extension files all declare `namespace DCoding.Data.DVault;`.
- Use compiled public API output rather than source-declaration scraping so package-level surface drift is not missed.
- Keep `src/DCoding.Data/DCoding.Data.csproj`, test projects, and benchmarks out of the approval target set; the repository already marks `src/DCoding.Data/DCoding.Data.csproj` as `<IsPackable>false</IsPackable>`.

Non-blocking notes
- `git diff --stat develop..HEAD -- . ':(exclude).gicket/**'` returned no non-ticket file changes, so the current branch is a clean baseline for the future developer implementation.
- The visible branch history is orchestration-only on top of existing repo work (`e6af709e`, `d8228af4`, `fef4f8b8`), which is consistent with a ticket awaiting developer handoff rather than a partially implemented change.

Split recommendations
- No split recommended; upstream XML-doc coverage is already done in `06EXB817Q8RAXCQH5QQR5RFY34`, and downstream one-member-per-file analyzer work remains tracked in `06EXB81QXE7XJPNM6NTPYCTP1M`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment