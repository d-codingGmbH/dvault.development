## Developer Handoff

Summary
- Added provider-native crypto guardrail unit tests for the SQL Server Always Encrypted selection seam.
- Covered incompatible provider profile, unsupported provider path, unavailable reviewed capability facts, unsupported reviewed capability facts, and redaction of caller-owned prerequisite details in support bundles.
- No live SQL Server coverage was added; the proof remains deterministic and local.

Verification
- `bash tools/check-format.sh` passed.
- `git diff --check -- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs` passed.
- `dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --nologo --no-restore --filter FullyQualifiedName~DataVaultDiagnosticsTests` was attempted but did not reach compilation because the local restore cache is missing `Microsoft.EntityFrameworkCore.Analyzers` versions `8.0.28` and `10.0.9` under the no-network execution boundary.

Live-provider note
- No `DVAULT_TEST_*` variables are required for the added tests.