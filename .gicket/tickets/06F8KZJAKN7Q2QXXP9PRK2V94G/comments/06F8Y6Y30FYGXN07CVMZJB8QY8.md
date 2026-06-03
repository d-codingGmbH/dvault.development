## Dev rework

Addressed tester rework for PostgreSQL and SQL Server PIT/bridge read strategy candidates.

Changes:
- Added explicit `IncompleteReadShapeEvidence` read fallback cause and PIT/bridge gate requirements.
- Centralized generated PIT/bridge projection-evidence checks in the shared read strategy gate evaluator so diagnostics, telemetry fallback causes, and provider `CanRead` decisions agree.
- Updated PostgreSQL and SQL Server PIT/bridge strategies to delegate eligibility to that shared evaluator.
- Updated read-provider tuning text and PIT/bridge support-matrix docs to recognize PostgreSQL and SQL Server PIT/bridge candidates while keeping SQLite as the optimized latest-satellite path.
- Added unit coverage for incomplete read-shape evidence fail-closed behavior and updated the public API snapshot/read-plan contract.

Verification:
- `dotnet build DVault.slnx --nologo` passed.
- `dotnet test DVault.slnx --nologo` passed; external PostgreSQL, SQL Server, MySQL, and Oracle live tests skipped because their opt-in connection-string environment variables were not configured.
- `bash tools/check-format.sh` passed after the final documentation edit.

Notes:
- Build/test emitted existing warning noise, including NU1900 warnings from the sandbox read-only NuGet vulnerability cache and existing analyzer/test warnings.
- The v1 read request contract still has no freshness watermark; stale PIT/bridge maintenance remains caller-owned explicit maintenance, and this rework fails closed for missing or incomplete generated read-model projection evidence.