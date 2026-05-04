[gicket-bot] integrator-decision-v1

Decision: ACCEPT.

Manual integration after the auto-integrator conflict resolver produced a valid resolution but the bot rejected the BOM-prefixed resolver snapshot. I resolved the remaining SQL Server/provider documentation and test conflicts, kept the optimized SQL Server save strategy with schema-aware table resolution, and verified both the default local integration path and the configured SQL Server opt-in lane.

Validation:
- `dotnet test tests\DCoding.Data.DVault.Tests\Unit\DCoding.Data.DVault.Tests.Unit.csproj --nologo`
- `dotnet test tests\DCoding.Data.DVault.Tests\Integration\DCoding.Data.DVault.Tests.Integration.csproj --nologo --filter "Category!=ProviderIntegration.ExternalOptIn"`
- `DVAULT_TEST_SQLSERVER_CONNECTION_STRING=... dotnet test tests\DCoding.Data.DVault.Tests\Integration\DCoding.Data.DVault.Tests.Integration.csproj --nologo`