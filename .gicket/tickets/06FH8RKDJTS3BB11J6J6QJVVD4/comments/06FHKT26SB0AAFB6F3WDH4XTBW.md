[gicket-bot] manual dev rework

Summary
- Reworked the provider-native privacy selection surface after the tester return.
- Removed the public shared `DataVaultPrivacyOptions.RegisterProviderNativeCryptoSelection(...)` API, the shared privacy options selection property, and the public shared privacy selection record.
- Added the provider-owned SQL Server entrypoint `AddDVaultSqlServerAlwaysEncryptedSelection(...)` plus provider-owned diagnostics for the reviewed Always Encrypted capability.
- Added fail-closed diagnostics for missing caller-owned prerequisite proof names and kept native execution, encrypted DDL, SQL crypto calls, provider-name auto-dispatch, and live probing out of scope.

Verification
- `dotnet build DVault.slnx --nologo` passed with 0 errors.
- `DVAULT_UPDATE_API_SNAPSHOTS=1 dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --nologo --filter FullyQualifiedName~ApiSurfaceSnapshotTests` passed for the full unit suite because the Microsoft Testing Platform ignored the VSTest filter: net8.0 667 passed, net10.0 735 passed.
- `bash tools/check-format.sh` passed.

Handoff
- decision: `ready_for_test`
- next-role: `test`
- remaining notes: existing NuGet/auditing/analyzer warnings were observed during build; they did not fail verification.