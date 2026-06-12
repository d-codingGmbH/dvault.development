<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket against the current hash-key storage and benchmark baselines: the existing repository-local benchmark harness and artifact contract should be extended to compare four bounded hash-key variants, with no child tickets, relation edits, description updates, attachments, or planning documents materialized in this run.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The benchmark baseline already exists in `benchmarks/DCoding.Data.DVault.Benchmarks`, the shared artifact triplet contract already exists in `docs/plans/performance-evidence-benchmark-artifact-contract.md`, and this ticket should extend those surfaces rather than introduce a second benchmark harness or artifact schema.
- The storage-profile baseline is already finite in repository code and docs: `DataVaultHashKeyStorageProfile` exposes only `HexString` and `Binary`, and `BuiltInStableHashService` exposes `sha256-v1`, `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1`.
- For this ticket, ratify `sha256-128-v1` as the single shorter opt-in comparison baseline instead of reopening the full non-default algorithm matrix; current provider-matrix and live-schema fixture tests already use `sha256-128-v1`, and it gives the clearest bounded footprint delta.
- Current benchmark code is still hard-wired to SHA-256-shaped hash assertions in the DVault save benchmarks and has no storage-profile or stable-hash option surface in `BenchmarkOptions`/`BenchmarkRunner`, so this ticket includes benchmark-harness generalization work, not only running an existing command.
- Live relation context was verified locally: parent epic `06F9GF5A8V7G3PAKGRXNYEBW5C` still owns this ticket, this ticket still blocks documentation task `06F9GF6CX7WE2JGBDW3QH1GX98`, and the incoming blocks relation from done task `06F9GF60BKEW0CC9FCZRPVX0SR` is historical and non-blocking because the related ticket is already `done`.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized during this refinement run.

### Scope In
- Extend the existing benchmark harness, options, and artifact output so it can compare `sha256-v1` hex, `sha256-v1` binary, `sha256-128-v1` hex, and `sha256-128-v1` binary within the current repository-local benchmark project.
- Measure repository-local insert/save cost on the existing DVault benchmark scenarios for the four bounded variants instead of inventing a separate workload family.
- Measure repository-local latest-satellite repeated-write lookup behavior and index-shape sensitivity by reusing the existing `--latest-indexes` path or an equivalent bounded extension of that same benchmark surface.
- Measure repository-local read and join-style workloads on the existing latest-satellite read, PIT as-of read, and bridge traversal read scenarios for the same four bounded variants where execution is feasible.
- Produce timing, allocation, and supporting footprint evidence through the existing benchmark artifact triplet, with same-label sidecar files when exact SQL or storage-footprint evidence needs supplemental capture.

### Scope Out
- A full comparison matrix across every non-default built-in stable-hash algorithm.
- Production telemetry, dashboards, hosted observability, or non-repository-local evidence collection.
- New public API shapes, caller-facing hash-key type changes, or changes to the canonical lowercase-hex boundary.
- Automatic rehash, migration, dual-write, backfill, or repair tooling.
- Mandatory completed external-provider execution across PostgreSQL, SQL Server, MySQL, and Oracle when those environments are not locally configured.
- A shared benchmark artifact contract rewrite unless a later separate contract ticket explicitly approves one.

## Acceptance Criteria
- The benchmark runner can execute one bounded four-variant comparison baseline covering `sha256-v1` hex, `sha256-v1` binary, `sha256-128-v1` hex, and `sha256-128-v1` binary without creating a parallel benchmark harness.
- Benchmark verification no longer hard-codes 64-character SHA-256-only assumptions; the active stable-hash algorithm and storage profile are validated deterministically so shorter-digest runs fail only on real drift.
- Repository-local evidence includes completed SQLite rows for save/insert workloads, latest-satellite lookup/replay behavior, latest-satellite read, PIT as-of read, and bridge traversal read for the bounded four-variant baseline, while optional external-provider rows continue to preserve skipped-row semantics when those providers are not configured.
- Footprint evidence for the compared hash-key/index shapes is preserved under the same benchmark label and artifact bundle as the timing rows, and any required SQL or storage sidecars stay attached to that same evidence set instead of inventing a new ad hoc reporting format.
- The resulting benchmark evidence is explicit enough for downstream documentation ticket `06F9GF6CX7WE2JGBDW3QH1GX98` to cite measured storage-footprint and lookup-performance tradeoffs without reopening PO refinement.

## Definition of Done
- All benchmark changes stay inside the existing benchmark project and shared benchmark-documentation surfaces rather than creating a second executable or one-off measurement script.
- Automated benchmark tests cover the new variant routing or option surface and preserve deterministic output expectations for the bounded four-variant baseline.
- The benchmark triplet can still be emitted locally under the shared artifact contract, and optional-provider discovery continues to report available versus skipped rows deterministically.
- The completed evidence set leaves the downstream documentation task with a bounded measured baseline instead of a placeholder performance claim.

## Implementation Notes
- `DataVaultHashKeyStorageProfile` and `BuiltInStableHashService` already define the bounded storage-profile and built-in algorithm inventory; the benchmark work should project those existing surfaces instead of adding new policy.
- `BenchmarkOptions` and `BenchmarkRunner` currently expose only iterations, warmup, output, `--scale`, `--latest-indexes`, provider filter, and load-timestamp storage. Add the hash-storage and stable-hash comparison dimension there instead of creating a separate command or benchmark project.
- `CustomerProfileDataVaultBenchmark` and `OrderProductDataVaultBenchmark` currently assert lowercase 64-character SHA-256 hash keys through `DataVaultBenchmarkHelpers.IsLowercaseSha256(...)`; those checks must be generalized before shorter-algorithm runs can succeed.
- Reuse `LatestSatelliteLookupIndexBenchmark` for repeated-write lookup and index-shape evidence, and reuse `LatestSatelliteReadBenchmark`, `PitAsOfReadBenchmark`, and `BridgeTraversalReadBenchmark` for the read/join workload baseline.
- Keep SQLite as the required executable baseline. Optional PostgreSQL, SQL Server, MySQL, and Oracle comparisons can continue to flow through the existing configured-versus-skipped provider model under the current artifact contract.
- If exact storage-footprint or index-footprint numbers need extra measurement files, keep them beside the required benchmark triplet under the same artifact label instead of changing the shared benchmark row schema in this ticket.

## Open Questions
- none

## Follow-Up Questions
- After the SQLite-local evidence lands, should a follow-up ticket execute the same four-variant benchmark matrix on one or more optional external providers to validate provider-specific storage savings beyond the required local baseline?
- Should later documentation expand beyond the bounded `sha256-128-v1` comparison and add advisory notes for `sha1-v1` or `sha256-160-v1`, or should those remain out of scope unless a later evidence ticket measures them directly?

## Risks
- The current benchmark suite assumes SHA-256 digest shape in multiple verification points; if that generalization is incomplete, shorter-algorithm rows may fail falsely or silently compare the wrong invariant.
- Storage-footprint claims can drift into one-off local notes if supplemental measurements are not kept under the same benchmark label and artifact-bundle discipline as the timing rows.
- Optional provider execution remains environment-gated; expecting completed cross-provider rows in this ticket without local provider infrastructure would expand the scope beyond the current bounded baseline.
- Because documentation ticket `06F9GF6CX7WE2JGBDW3QH1GX98` depends on this ticket, under-scoping the measured variant matrix would push unmeasured generalization pressure downstream into documentation work.

## Split Recommendations
- No split is recommended while the work stays inside the existing benchmark harness, artifact contract, and bounded four-variant comparison baseline.
- If stakeholders later require completed optional-provider matrices or a broader multi-algorithm comparison beyond `sha256-128-v1`, create follow-up tickets instead of broadening this task in place.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add benchmark evidence comparing SHA-256 hex, SHA-256 binary, shorter opt-in algorithm hex, and shorter opt-in algorithm binary storage for index footprint, insert cost, lookup, and join/read workloads where feasible. Keep benchmarks repository-local and avoid production telemetry or platform dashboards.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Implemented the bounded hash-key storage benchmark matrix in the existing repository-local benchmark harness. The runner now supports the four required variants: `sha256-v1` hex, `sha256-v1` binary, `sha256-128-v1` hex, and `sha256-128-v1` binary.

The implementation generalizes benchmark validation away from 64-character SHA-256-only assumptions, routes stable-hash and storage-profile selection through provider capabilities and DVault service registration, and keeps the existing benchmark-summary triplet as the primary evidence contract. Supplemental footprint evidence is emitted as same-label sidecars: `hash-key-footprint.md`, `hash-key-footprint.csv`, and `hash-key-footprint.json`.

Generated SQLite evidence is available under `artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/` and includes 106 completed benchmark rows plus 4 footprint rows for the bounded variant matrix. Optional external providers remain environment-gated by the existing provider discovery model.

Verification performed:

- `dotnet build DVault.slnx --nologo` completed with 0 errors and existing warning noise.
- `dotnet build benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --nologo --no-restore --no-dependencies -p:UseSharedCompilation=false` completed with 0 errors.
- `dotnet build tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj --nologo --no-restore --no-dependencies -p:UseSharedCompilation=false -v:minimal` completed with 0 errors.
- `dotnet test tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj --nologo --no-build --filter "FullyQualifiedName~BenchmarkOptionsCanSelectBoundedHashKeyStorageMatrix|FullyQualifiedName~CustomerProfileDataVaultBenchmarkSupportsShortBinaryHashKeyVariant|FullyQualifiedName~HashKeyFootprintSidecarRowsDescribeBoundedMatrixPayloads"` completed with 0 failures; Microsoft.Testing.Platform ignored the VSTest filter and ran the integration suite for net8.0 and net10.0.
- `bash tools/check-format.sh` passed.
- `dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Debug --no-build -- --provider sqlite --hash-key-storage-matrix --iterations 1 --warmup 0 --output artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612` completed and wrote the benchmark triplet plus footprint sidecars.

Note: an earlier Release configuration artifact-generation attempt timed out during local build/startup before benchmark execution. The persisted evidence run used the already-built Debug executable with `--no-build` so it could exercise the benchmark matrix within the bounded runtime.
<!-- gicket-bot:developer-delivery:v1:end -->