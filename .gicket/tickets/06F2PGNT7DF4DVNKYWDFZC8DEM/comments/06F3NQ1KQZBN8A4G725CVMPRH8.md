## Dev Implementation Report

Implemented and repaired opt-in live bulk-provider coverage for Postgres, SQL Server, Oracle, and MySQL. The branch uses the existing external provider fixture/configuration lanes and exercises `IDataVaultSaveService.SaveAsync(DbContext, DataVaultBulkSaveRequest)` with an eligible ordered hub, link, and satellite batch.

Repair in this pass:
- Added the missing `DCoding.Data.DVault.Tests.Shared` import to `ExternalProviderBulkSaveAssertions`, resolving the failed `LiveSchemaReaderContractFixture` compile reference from the previous build snapshot.

Coverage included on the branch:
- Provider-strategy diagnostics assert `ProviderStrategySelected` for each provider-specific strategy.
- Bulk result assertions cover row counts, saved-record ordering, provider-visible hub/link/satellite rows, and latest-state satellite HashDiff suppression.
- MySQL and Oracle native strategies resolve produced table names to EF physical table names so the shared live fixture's temporary table-name overrides are honored.
- README external-provider wording describes the bulk lanes.

Verification run in this sandbox:
- `bash tools/check-format.sh` passed. It emitted the existing warning that solution workspace format verification failed, then folder whitespace verification passed.
- `git diff --check -- tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderBulkSaveAssertions.cs` passed.
- `dotnet build DVault.slnx --nologo` could not complete because network access to `https://api.nuget.org/v3/index.json` is blocked (`NU1301`, permission denied).
- `dotnet build DVault.slnx --nologo --ignore-failed-sources` and `dotnet test DVault.slnx --nologo --no-restore` also could not complete because the sandbox package cache lacks required EF packages (`NU1101`).

Live provider verification still requires the documented external connection strings, provider restore marker properties, and database privileges.