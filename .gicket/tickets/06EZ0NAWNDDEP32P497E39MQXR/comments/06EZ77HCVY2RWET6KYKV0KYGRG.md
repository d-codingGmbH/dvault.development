## Developer Rework Note

The current branch already contains the repository implementation expected by this ticket. I did not make a repository diff in this pass because the tester return is now an external verification-evidence gap rather than a source defect.

Repository state to verify:

- `README.md` section `Optional Local SQL Server Integration Tests` documents `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` and the repo-root command `dotnet test DVault.slnx --filter FullyQualifiedName~SqlServer`.
- `docs/architecture/dvault-v1-explicit-save-service.md` classifies SQL Server as `ProviderIntegration.ExternalOptIn`, gated by `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`.
- `tests/DCoding.Data.DVault.Tests/Integration/SqlServerIntegrationTestConfiguration.cs` defines deterministic missing-configuration skip behavior.
- `tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs` contains representative configured hub, link, and satellite smoke tests through `AddDVaultSqlServer()`.
- `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs` registers `SqlServerDataVaultSaveStrategy`.

Verification performed in this dev pass:

- `bash tools/check-format.sh` passed.
- `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is not set in this execution environment.
- `dotnet test DVault.slnx --filter FullyQualifiedName~SqlServer --nologo` could not produce live SQL Server evidence here because restore attempted to reach `https://api.nuget.org/v3/index.json` and network access is denied in the sandbox. Retrying with `-p:RestoreIgnoreFailedSources=true` still failed because required EF Core packages were not available from the local package cache.

Tester action needed for the remaining acceptance evidence: run the documented command from the repository root in an environment with restored packages and a developer-managed SQL Server database:

```sh
DVAULT_TEST_SQLSERVER_CONNECTION_STRING='Server=localhost;Database=dvault_tests;User Id=dvault;Password=local-secret;TrustServerCertificate=True' dotnet test DVault.slnx --filter FullyQualifiedName~SqlServer
```

Without that external connection string and database, this lane can only demonstrate deterministic skip behavior, which is the intended default for the external opt-in contract.