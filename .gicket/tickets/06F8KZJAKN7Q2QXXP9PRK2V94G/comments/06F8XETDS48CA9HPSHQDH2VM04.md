## Dev implementation

Implemented diagnostics-gated PostgreSQL and SQL Server PIT/bridge read strategy candidates.

Repository changes:
- Added a shared relational PIT/bridge read strategy for provider-specific maintained PIT and bridge table reads.
- Registered PostgreSQL and SQL Server PIT/bridge strategy candidates from their provider startup extensions.
- Extended read diagnostics gates to recognize PostgreSQL and SQL Server candidates, supported provider names, and fallback causes.
- Added unit coverage for candidate selection gates, unsupported-shape fallback, and provider package registration.
- Updated support-matrix documentation in README, architecture, and production checklist guidance.

Verification:
- `dotnet build DVault.slnx --nologo` passed.
- `dotnet test DVault.slnx --nologo` passed; optional external-provider tests skipped where connection-string environment variables were not configured.
- `bash tools/check-format.sh` passed.

Notes:
- SQLite remains the only optimized latest-satellite read provider path.
- PostgreSQL and SQL Server live execution remains opt-in through the existing external provider test configuration.