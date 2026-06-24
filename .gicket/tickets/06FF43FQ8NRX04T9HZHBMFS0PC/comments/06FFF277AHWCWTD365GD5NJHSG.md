## Developer Outcome

Implemented PostgreSQL PIT maintenance selected/fallback diagnostics on the existing maintenance Activity surface. Default PIT full rebuild selection now records `ProviderStrategySelected` with `PostgresDataVaultPitMaintenanceStrategy` when the registered provider strategy runs, and records `ProviderNeutralFallback` with finite `DataVaultPitMaintenanceStrategyFallbackCauseKind` values before provider-neutral fallback.

PostgreSQL parent maintenance remains provider-neutral; this change only adds observability for `IDataVaultPitMaintenanceService.RebuildAsync(...)` full rebuild strategy selection and fallback. Benchmark and documentation follow-up remains on the existing sibling tickets named in the delivery contract.

Verification completed:
- `bash tools/check-format.sh` passed.
- `dotnet build DVault.slnx --nologo` passed with 0 errors.
- `dotnet test DVault.slnx --nologo` passed with 0 failures. Local external-provider PostgreSQL tests were skipped because `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is not configured.

Notes:
- The solution commands emitted existing NU1900 warnings from a read-only NuGet vulnerability HTTP cache path.
- The PostgreSQL selected/fallback integration tests compile and are gated by the existing PostgreSQL connection-string opt-in.