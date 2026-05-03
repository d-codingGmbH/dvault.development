[gicket-bot] PO-critic review contract

Summary
- Ticket is sufficiently refined for developer handoff; the current branch, persisted contract, repo-local benchmark surface, and related child ticket evidence all align on the SQLite-only benchmark harness scope.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB7T62EMCD7CSHS9PE501SC/description.md` and `.gicket/tickets/06EXB7T62EMCD7CSHS9PE501SC/comments/06EYPC2GSKQDZJQCEAH3D75HDC.md` both record PO handoff `ready_for_po_critic` and `Open questions - none`.
- Current branch inspection returned `ticket/06EXB7T62EMCD7CSHS9PE501SC-story-build-benchmark-harness-for-normal-ef-vers` at commit `aa61fd83bc96ebde69ad75c48fe677d5ad4e3396` (`git rev-parse --abbrev-ref HEAD`, `git rev-parse HEAD`).
- `DVault.slnx` includes `benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj`, and that project file targets `net10.0` with only `Microsoft.EntityFrameworkCore.Sqlite` and `Microsoft.Extensions.DependencyInjection` package references, matching the SQLite-only local-harness contract.
- `benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs` directly enumerates `CustomerProfilePlainEfBenchmark`, `CustomerProfileDataVaultBenchmark`, `OrderProductPlainEfBenchmark`, and `OrderProductDataVaultBenchmark`; `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs` asserts all four rows plus `Executed 4 benchmark baselines.`
- `benchmarks/DCoding.Data.DVault.Benchmarks/ScenarioContracts.cs` fixes `C-100`, `O-1000`, and `SKU-COFFEE`; `CustomerProfileBenchmarks.cs` and `OrderProductBenchmarks.cs` verify the exact persisted-outcome row counts described in the parent story.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs`, `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs`, and `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` directly define `IDataVaultSaveService`, `AddDVault()`, and `ApplyDataVaultMetadata(...)`, so the implementation notes reference existing public APIs instead of inferred ones.
- `git log --oneline -5 -- benchmarks/DCoding.Data.DVault.Benchmarks tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md docs/architecture/dvault-v1-explicit-save-service.md` includes `a5dc34db [06EXB7TP9PF2XFRQ9MG7CJQR10]` and `af970a00 [06EXB7TE0806E7EY5ZBATHQNK8]`, so benchmark-related history lines up with the two child tickets named in the contract.
- Related child PO-critic evidence in `.gicket/tickets/06EXB7TP9PF2XFRQ9MG7CJQR10/comments/06EYP1ZY8SGY5TAP6GBRR3XN1C.md` explicitly treats Markdown/JSON as the rich context carriers and says implementation should not assume CSV must carry the full run-level context envelope.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The parent story does not restate whether `--output` may create a missing directory, although `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` already documents directory creation; this is non-blocking.

Risky assumptions
- Downstream roles may misread the parent AC as requiring standalone context fields in CSV; related child ticket comment evidence resolves the intended behavior, but the parent story itself does not restate that nuance.
- The story assumes the benchmark surface remains the existing repo-local harness under `benchmarks/DCoding.Data.DVault.Benchmarks` and not a later BenchmarkDotNet migration.
- The ticket remains SQLite-only even though the broader repo contains optional Postgres test surfaces elsewhere.

AC / test suggestions
- Acceptance verification should run `dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0` and assert the four named baseline rows plus `Executed 4 benchmark baselines.`
- Artifact verification should assert deterministic filenames and confirm Markdown/JSON carry provider, iteration/warmup, OS/process architecture, processor count, and .NET runtime context while CSV carries the same four benchmark result rows from the same run.
- Keep acceptance text and test evidence tied to the fixed `C-100` and `O-1000`/`SKU-COFFEE` scenario contracts so later benchmark tickets do not blur v1 scope.

Implementation watchouts
- Keep the DVault comparison on the explicit `IDataVaultSaveService` path; the direct source and architecture note do not support switching this story to `SaveChanges` interception.
- Do not widen this story into Postgres, Docker, external services, secrets, or performance-threshold gating.
- Preserve the fixed scenario contracts and persisted-outcome wording so conventional EF versus DVault storage-shape differences stay directly comparable.

Non-blocking notes
- `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` already provides a benchmark-owned location for usage and artifact-behavior documentation.
- Benchmark-related branch history already contains the integrated child task work for project creation and artifact emission.

Split recommendations
- Keep the existing split to 06EXB7TE0806E7EY5ZBATHQNK8 and 06EXB7TP9PF2XFRQ9MG7CJQR10; repository relations and branch history already bound the work cleanly.
- If scope expands later, split by new provider/environment coverage or long-lived artifact publication rather than reopening the fixed SQLite v1 harness contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment