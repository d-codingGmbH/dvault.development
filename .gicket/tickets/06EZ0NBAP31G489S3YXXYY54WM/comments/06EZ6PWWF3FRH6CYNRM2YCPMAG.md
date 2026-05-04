## Developer Rework Evidence

This rework pass found no repository source change to make. The ticket branch already contains the Oracle capability profile and optimized-writer boundary implementation; the unresolved tester return is limited to executable verification being blocked by the sandbox NuGet network policy.

### Branch State

- Branch: `ticket/06EZ0NBAP31G489S3YXXYY54WM-task-implement-oracle-provider-capability-profil`
- Verified HEAD: `fb6f1eed`
- `git diff --stat develop...HEAD -- README.md docs src tests benchmarks` reports 15 repository files changed with `714 insertions(+), 12 deletions(-)`.
- `git status --short -- README.md docs src tests benchmarks` and `git diff --name-only -- README.md docs src tests benchmarks` produced no output after this verification pass.

### Acceptance Criteria Evidence

- AC1: `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs` defines `DataVaultProviderCapabilityProfiles.Oracle` with profile name `oracle-v1`, `NoneInV1Unsupported` SQL-function and concurrency baselines, and mappings for `HashKey`, `HashDiff`, `LoadTimestamp`, `RecordSource`, `ParticipantReference`, `BusinessKey`, and `PayloadText` using `VARCHAR2(64 CHAR)`, `TIMESTAMP WITH TIME ZONE`, `VARCHAR2(255 CHAR)`, and `CLOB`.
- AC1 coverage: `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs` contains `OracleProfileDeclaresExplicitUnsupportedFunctionAndConcurrencyBaselines` and `OracleProfileDeclaresNativeStorageMappingsForAllLogicalPropertyKinds`.
- AC2: `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` keeps the no-argument `UseDataVault()` and two-argument `ApplyDataVaultMetadata(metadataModel)` on the SQLite default profile while adding provider-aware overloads that store `DataVaultAnnotationNames.ProviderProfile` and call the translator with the selected profile.
- AC2 coverage: `tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelBuilderExtensionsTests.cs` contains `UseDataVaultWithProviderProfileReturnsSameBuilderAndStoresSelectedProfileAnnotation`, and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs` contains `ApplyDataVaultMetadataWithOracleProfileProjectsOracleStorageAnnotations`.
- AC3: `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs` calls `services.AddDVault()` and registers `OracleDataVaultSaveStrategy` as `IDataVaultProviderSaveStrategy`.
- AC3 package boundary: `src/DCoding.Data.DVault.Oracle/DCoding.Data.DVault.Oracle.csproj` has only `Microsoft.Extensions.DependencyInjection.Abstractions` as a package reference and only `../DCoding.Data.DVault/DCoding.Data.DVault.csproj` as a project reference.
- AC3 coverage: `tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs` contains `OracleProjectDoesNotReferenceNonOracleProviderPackages`, which rejects Sqlite, Npgsql/PostgreSQL, SqlServer, and MySql references.
- AC4: `src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs` gates `CanSave` on provider name `Oracle.EntityFrameworkCore`, clean change tracker state, and a supported whole request batch; unsupported batches decline before dispatch.
- AC4 fallback: `src/DCoding.Data.DVault/DataVaultSaveService.cs` iterates provider strategies by selected order and then continues into the provider-neutral writer when no strategy accepts the request batch.
- AC4 coverage: `tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs` contains `AddDVaultOracleDeclinesSqliteContextAndFallsBackThroughCoreWriter`.
- AC5/API: `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt` includes the provider-aware `UseDataVault` and `ApplyDataVaultMetadata` overloads plus `IDataVaultProviderSaveStrategy` and `DataVaultProviderSaveStrategyContext`.

### Verification Commands

- `bash tools/check-format.sh`: passed. Output included `One-member-per-file check passed for 32 packable source files.` and `Formatting check passed.` The script also emitted its existing warning that solution workspace format verification failed while folder whitespace verification passed.
- `git diff --check develop...HEAD -- README.md docs src tests benchmarks`: passed with no output.
- `dotnet build DVault.slnx --nologo`: blocked before compilation by restore error `NU1301: Unable to load the service index for source https://api.nuget.org/v3/index.json` with `Permission denied (api.nuget.org:443)`.
- `dotnet test DVault.slnx --nologo`: blocked before test execution by the same `NU1301` NuGet restore denial.
- Diagnostic retry `dotnet build DVault.slnx --nologo -p:RestoreIgnoreFailedSources=true`: confirmed the sandbox does not have the required EF Core packages available locally; restore reports `NU1101` for packages including `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.Relational`, and `Microsoft.EntityFrameworkCore.Sqlite`.

Tester should rerun the policy build/test commands in an environment with NuGet restore access or a pre-populated package cache. The repository branch has no additional source edits from this rework pass.