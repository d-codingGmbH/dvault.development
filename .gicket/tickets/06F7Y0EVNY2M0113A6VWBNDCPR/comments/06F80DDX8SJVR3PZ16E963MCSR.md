[gicket-bot] PO-critic review contract

Summary
- Ready for dev: the contract is specific, the async save boundary is already landed, the current benchmark/docs/tests baseline is directly evidenced, and `## Open Questions` is resolved to `none`.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F7Y0EVNY2M0113A6VWBNDCPR/description.md:28-49` defines the async benchmark AC/DoD and shows `## Open Questions` -> `- none`.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs:50-60` exposes `IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, CancellationToken)`.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs:<redacted>` and `src/DCoding.Data.DVault/DataVaultActivityTracing.cs:17` show the async overload runs through the shared chunked pipeline and `dvault.save.chunked_request` tracing/telemetry boundary.
- `benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs:230-232` currently registers only three `customer-profile-streaming-save` baselines: materialized explicit bulk, chunked size 10, and chunked size 5.
- `benchmark-summary.md:5` and `benchmark-summary.md:42-44` show the checked-in artifact has `Benchmark baselines: 37` and only those three streaming rows; no async row exists yet.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:76-92`, `:300-302`, `:348`, `:399`, and `:438-450` hard-code the current streaming baselines, `37/27/10` row totals, CSV line count `38`, and exactly two `/chunked-save-bounded-*` artifact rows.
- `benchmarks/DCoding.Data.DVault.Benchmarks/README.md:20`, `docs/plans/performance-evidence-benchmark-artifact-contract.md:79-85`, and `docs/performance-profiles.md:83-89` document the present repo baseline as materialized bulk plus bounded chunked evidence only; `.gicket/tickets/06F7Y0EVNY2M0113A6VWBNDCPR/description.md:46` keeps the broader adopter-doc rewrite on downstream docs task `06F7Y0F650KM61BQXMEQPZ86DR`.
- `git rev-parse HEAD` returned `33f2c57ecc6346c98686f7f127228af0f68bd2d4`, matching the supplied `scratch-source-ref`; `git diff --stat 33f2c57ecc6346c98686f7f127228af0f68bd2d4..HEAD` returned empty, so this is a clean pre-development review snapshot.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not pin one exact async baseline token or chunk-size label; implementation should keep the async row self-describing in `baseline` and/or `executionDetail` without schema drift.

Risky assumptions
- This ticket assumes benchmark-facing docs (`benchmarks/.../README.md` and `docs/plans/performance-evidence-benchmark-artifact-contract.md`) are sufficient for handoff even though `docs/performance-profiles.md` still says async streaming is not separate benchmark evidence until downstream task `06F7Y0F650KM61BQXMEQPZ86DR` lands.
- The async row is expected to remain on the existing `ChunkedRequest`/`dvault.save.chunked_request` boundary; any implementation that invents provider-native async semantics or a second public contract would violate the verified source baseline.

AC / test suggestions
- Add one explicit artifact assertion for the async row's source-shape marker plus `savePath`, `chunkBoundary`, `processedChunkCount`, and retained-state fields in `executionDetail`.
- Update the total artifact-count assertions (`Recorded 37`, `Executed 27`, `Skipped 10`, `Assert.Equal(38, csvLines.Length)`, `Assert.Equal(37, results.Length)`) to exact post-change values.
- Keep one assertion that the async row reuses the same dataset size and change ratio as the existing `customer-profile-streaming-save` baselines.

Implementation watchouts
- Do not add artifact files or row-schema columns; the contract is the existing `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` triplet.
- Use the existing async overload and shared chunked pipeline so telemetry stays on `operationKind=ChunkedRequest` / `dvault.save.chunked_request`.
- Avoid touching broader adopter-doc scope in `docs/performance-profiles.md` beyond whatever coordination is strictly necessary; that rewrite is already split to ticket `06F7Y0F650KM61BQXMEQPZ86DR`.

Non-blocking notes
- Current branch HEAD still equals the supplied scratch source ref, so there is no implementation delta yet; that is acceptable for this pre-development PO gate.
- The upstream API story `06F7Y0DCHTWCN3H25XQF18QE2G` is already done and the current ticket only needs benchmark/artifact/doc alignment work.

Split recommendations
- No further split recommended; keep benchmark/allocation evidence on this ticket and leave the broader v0.24 adopter-doc rewrite on `06F7Y0F650KM61BQXMEQPZ86DR`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment