[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEHSH6S31ZE4K0Q3EKR784/description.md contains PO Handoff decision ready_for_po_critic and ## Open Questions with the single item none.
- Parent relations exist as parentOf files from 06F0MEHSH6S31ZE4K0Q3EKR784 to 06F0MEJ0NE80R7CNS982S3PKVR, 06F0MEJ7NANHCP64VR1SH3S3G8, 06F0MEJE5WC51MFQ3CWDRATCWC, and 06F0MEJPGG7JBFEXD693BHY07W.
- Child integration evidence exists: git log --grep found AUTO-INTEGRATION squash commits 4c3f6f6b4, fbbec26b1, 2d630dce9, and 048204be8 for the four child tickets.
- Current branch history shows HEAD 638b3d021 on ticket/06F0MEHSH6S31ZE4K0Q3EKR784-story-add-provider-aware-read-optimization-follo after PO handoff commits, based on develop at 048204be8.
- git diff --name-status develop..HEAD lists only this parent ticket's .gicket comments/events/description/ticket files; no product, test, benchmark, or docs code changes are pending on the parent branch.
- src/DCoding.Data.DVault/DataVaultProviderReadStrategy.cs and DataVaultProviderReadStrategyContext.cs define the public provider read strategy contract and context used by latest/as-of satellite reads.
- src/DCoding.Data.DVault/DefaultDataVaultReadService.cs orders registered IDataVaultProviderReadStrategy instances by descending Priority, calls CanReadLatestSatelliteRows before strategy execution, and falls back to DataVaultSatelliteReadPipeline for latest/as-of reads; PIT reads go directly to DataVaultPitReadPipeline.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs registers IDataVaultReadService as DefaultDataVaultReadService for AddDVault, while tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs verifies AddDVault registers no provider read strategy and AddDVaultSqlite registers one.
- src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs registers SqliteDataVaultReadStrategy via TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderReadStrategy, SqliteDataVaultReadStrategy>()).
- src/DCoding.Data.DVault.Sqlite/SqliteDataVaultReadStrategy.cs implements SQLite latest/as-of satellite reads using parent hash-key batching, optional AsOf filtering, ROW_NUMBER partitioning by parent hash key, and deterministic ordering by parent hash key.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs contains DataVaultProviderReadStrategyGateEvaluator for SQLite, with fallback causes for provider mismatch, non-hub satellite parent, and multi-active driving keys.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs covers descending priority selection, same-priority registration order, typed projection strategy usage, and fallback with no provider strategy.
- tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs covers latest and as-of SQLite read semantics for metadata/table name, parent hash key, hash diff, load timestamp, record source, payload values, and missing parent rows.
- benchmarks/DCoding.Data.DVault.Benchmarks/ReadModelBenchmarks.cs defines latest-satellite-read, pit-as-of-read, and bridge-traversal-read scenarios; BenchmarkOptions.cs accepts provider filters all, sqlite, postgres, sqlserver, mysql, and oracle.
- benchmarks/DCoding.Data.DVault.Benchmarks/README.md documents the benchmark command, optional provider environment variables, deterministic skipped rows with skipReason, load timestamp storage options, output artifacts, and that SQLite latest-satellite reads compare AddDVault fallback with AddDVaultSqlite optimized strategy while PIT/bridge remain provider-neutral.
- docs/releases/v0.7.0.md documents implemented latest/as-of, PIT, and bridge read flows without claiming PIT refresh, bridge maintenance, full graph traversal, or provider-specific PIT/bridge optimization.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Benchmark timings remain machine-specific; downstream review should preserve command, provider filter, timestamp storage, run context, and measured rows rather than relying on absolute times alone.
- Non-SQLite provider benchmark rows may be skipped on machines without configured provider dependencies or connection strings; this is acceptable only if skipReason remains deterministic and visible.

AC / test suggestions
- If downstream roles choose to revalidate rather than rely on child evidence, rerun dotnet build DVault.slnx, dotnet test DVault.slnx, and a SQLite benchmark smoke with --provider sqlite --iterations 1 --warmup 0.

Implementation watchouts
- Keep this parent as closure-only; do not add product-code scope under the parent story.
- Do not broaden the v1 optimization claim beyond SQLite latest/as-of ordinary hub-parent, non-multi-active satellite reads through IDataVaultReadService.
- Keep unsupported providers, link-parent satellites, multi-active satellites, PIT reads, and bridge reads on provider-neutral fallback or their already documented provider-neutral behavior.

Non-blocking notes
- git status --short --branch showed uncommitted local modifications in .gicket-bot/.gitignore, .gicket/.gitignore, .gicket/project.json, and .gicket/types.json; git diff --stat showed balanced churn outside the claimed ticket diff, so I did not treat it as a PO readiness blocker.
- I did not run build, tests, or benchmarks in this read-only PO-critic pass; the assessment relies on persisted ticket state, branch history, source, tests, docs, and child integration evidence.

Split recommendations
- No additional split is recommended. The existing split is already materialized and done through 06F0MEJ0NE80R7CNS982S3PKVR, 06F0MEJ7NANHCP64VR1SH3S3G8, 06F0MEJE5WC51MFQ3CWDRATCWC, and 06F0MEJPGG7JBFEXD693BHY07W.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment