## Dev implementation
- Reworked the default optional external-provider benchmark rows so PostgreSQL, SQL Server, MySQL, and Oracle now emit only a provider-native bulk-ingestion fallback row plus the matching provider-optimized row.
- Added a provider-native bulk benchmark using a clean DbContext, 63 total mixed hub/link/satellite operations, and three satellite operations; optimized rows assert through DVault diagnostics that the named provider save strategy was selected before timing.
- Preserved the required local SQLite classic EF, AddDVault fallback, and AddDVaultSqlite matrix, and updated benchmark artifact expectations and README guidance for the new optional-provider evidence boundary.

## Verification
- `bash tools/check-format.sh` passed. It reported the existing solution-format fallback warning, then confirmed folder whitespace formatting.
- Targeted benchmark integration tests could not complete in this sandbox: the first `dotnet test` attempt hit MSBuild named-pipe sandbox restrictions, and the single-node/no-reuse retry reached restore but failed because network access to `api.nuget.org` is blocked and the local package cache is missing required EF/Microsoft.Extensions packages.

## Follow-up for test role
- In an environment with packages restored or network-enabled restore, run `dotnet test tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj --nologo --filter FullyQualifiedName~BenchmarkScenarioExecutionTests -m:1 /nr:false /p:UseSharedCompilation=false`.
- Run the policy build and test commands before acceptance: `dotnet build DVault.slnx --nologo` and `dotnet test DVault.slnx --nologo`.