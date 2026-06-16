## Developer Delivery

Implemented the SQL Server latest-satellite read gap closure on the ticket branch.

Repository changes:
- `AddDVaultSqlServer()` now registers `SqlServerDataVaultReadStrategy` for `IDataVaultProviderReadStrategy`.
- `SqlServerDataVaultReadStrategy` now handles supported latest/current/as-of hub-parent, non-multi-active satellite reads and keeps unsupported shapes on provider-neutral fallback.
- Benchmark detail generation and the checked-in root benchmark triplet now name `SqlServerDataVaultReadStrategy` for the SQL Server `latest-satellite-read` skipped-placeholder row without claiming completed timing.
- Tests and docs were updated for registration, diagnostics selection, fallback, parity, smoke coverage, and evidence/gap posture.

Validation:
- `bash tools/check-format.sh` passed.
- Targeted `git diff --check` over touched source/test files passed.
- `benchmark-summary.json` parses as JSON.
- `dotnet build DVault.slnx --no-restore --nologo` and the focused unit test command were blocked by the local NuGet/asset cache state: missing `Microsoft.EntityFrameworkCore.Analyzers` 8.0.28/10.0.9, missing `xunit.analyzers` 1.27.0, and missing net8.0 targets in existing `project.assets.json` files. No restore was run because this bot run was constrained away from network-dependent behavior.

Remaining evidence boundary: SQL Server live execution and benchmark timing remain opt-in behind `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`; the checked-in root triplet intentionally remains a skipped placeholder when that connection string is unset.