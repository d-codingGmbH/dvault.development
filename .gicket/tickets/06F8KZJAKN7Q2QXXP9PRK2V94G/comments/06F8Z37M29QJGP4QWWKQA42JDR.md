## Dev rework

Addressed tester rework for PostgreSQL and SQL Server PIT/bridge read strategy candidates.

Changes:
- Added explicit `StaleReadModelMaintenance` read fallback cause and PIT/bridge gate requirement for observable stale-maintenance signals such as pending tracked changes.
- Updated the shared PIT/bridge gate evaluator so SQLite, PostgreSQL, and SQL Server candidate selection, diagnostics, and telemetry fallback causes fail closed on incomplete projection evidence or stale-maintenance signals.
- Added PostgreSQL and SQL Server PIT/bridge candidate parity coverage that executes the candidate read paths against maintained rows and compares row plus typed projection results with the provider-neutral `AddDVault()` fallback.
- Updated read-plan contract, PIT/bridge boundary evidence, and public API snapshot documentation for the new fallback cause.

Verification:
- `dotnet build DVault.slnx --nologo` passed.
- `dotnet test DVault.slnx --nologo` passed: integration total 212, failed 0, succeeded 191, skipped 21; unit total 429, failed 0, succeeded 429, skipped 0. External PostgreSQL, SQL Server, MySQL, and Oracle live tests skipped because opt-in connection-string environment variables were not configured.
- `dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --no-restore --nologo` passed after the final parity-test refactor.
- `bash tools/check-format.sh` passed.

Notes:
- Build/test emitted existing warning noise, including NU1900 warnings from the sandbox read-only NuGet vulnerability cache and existing analyzer/test warnings.
- Stale PIT/bridge maintenance remains caller-owned under the v1 read contract; this rework adds an observable fail-closed candidate gate and does not add automatic maintenance, freshness watermark APIs, or read-time refresh.