﻿[gicket-bot] Manual repair after tester API-compatibility finding

Summary
- The tester correctly returned this ticket because the read diagnostics implementation had added two new abstract `Analyze(...)` overloads to the existing public `IDataVaultDiagnosticsService` interface.
- I moved those read-request diagnostics to a new additive `IDataVaultReadDiagnosticsService`, leaving `IDataVaultDiagnosticsService` source/binary compatibility for external implementers intact.
- `DefaultDataVaultDiagnosticsService` implements both interfaces, and `AddDVault()` now registers the read-diagnostics companion service.
- Integration tests that verify read-strategy diagnostics now resolve `IDataVaultReadDiagnosticsService` explicitly.

Verification
- `dotnet build DVault.slnx --nologo` passed.
- `dotnet test DVault.slnx --nologo --filter "FullyQualifiedName~DataVaultDiagnostics"` passed; Microsoft.Testing.Platform ignored the VSTest filter and executed the current test assemblies: Integration 121 total / 113 passed / 8 skipped, Unit 246 passed.
- `bash tools/check-format.sh` passed.
- SQLite benchmark smoke completed: `dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --provider sqlite --iterations 1 --warmup 0 --output artifacts/benchmarks/ticket-06F0MEJE-api-repair-smoke`.
- Benchmark artifact: `artifacts/benchmarks/ticket-06F0MEJE-api-repair-smoke/benchmark-summary.md`.

Handoff
- Ready for tester re-check. The product performance path is unchanged by this repair; the change is limited to API shape, DI registration, tests, and public API snapshot.