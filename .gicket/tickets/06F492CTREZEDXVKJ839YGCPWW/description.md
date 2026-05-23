<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Existing repository evidence already bounds this story: the shared performance artifact contract is in place, SQLite is the required completed local baseline, optional PostgreSQL/SQL Server/MySQL/Oracle provider rows stay visible with normalized skipped reasons when not configured, and no ticket-side write was needed during PO refinement.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Verified live relation state: 06F492CTREZEDXVKJ839YGCPWW is a child of 06F492BTNHRPBC7D24E13ECFKM, is currently blocked by 06F492CAB2293R7BGJWMWMRKT4, 06F492CFSJHN0RGXXRG3KT63FM, and 06F492CN76GS3CKM8EFD0C20XM, and currently blocks 06F492D05THPGQVT3B3K7853A0; no relation cleanup was materialized in this PO pass.
- Current ticket context has no recent human comments and no persisted attachments that add extra scope or constraints.
- docs/plans/performance-evidence-benchmark-artifact-contract.md already defines the authoritative benchmark artifact set, run-context fields, row fields, required SQLite baseline, and optional provider matrix for this story.
- benchmark-summary.csv already demonstrates the intended v1 evidence shape: completed SQLite optimized-versus-fallback rows for core scenarios plus visible provider-native-bulk-ingestion rows for PostgreSQL, SQL Server, MySQL, and Oracle with normalized skipped reasons when those providers are not configured.

### Scope In
- Extend or reuse the existing benchmark harness and shared artifact contract to persist provider optimization regression baselines instead of inventing a new benchmark format.
- Keep SQLite local temporary files as the required always-completed provider baseline for provider-neutral fallback versus provider-optimized DVault strategies across the provider-sensitive scenarios already covered by the harness.
- Preserve optional external-provider baseline rows for PostgreSQL, SQL Server, MySQL, and Oracle, producing completed optimized-versus-fallback evidence when configured and explicit skipped rows when not configured.
- Capture deterministic provider execution detail for each optimized scenario so reviewers can tell whether the optimized path or the provider-neutral fallback path ran, using generated SQL when stable and practical or equivalent provider-native execution detail otherwise.

### Scope Out
- Adding new providers beyond SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.
- Replacing the shared performance-evidence artifact contract or creating a parallel ticket-specific artifact schema.
- Guaranteeing external provider availability in every local developer environment.
- Broad non-provider optimization performance work such as expanding compiled-model, compiled-query, or DbContext-pooling benchmarks into a new cross-provider matrix.

## Acceptance Criteria
- Benchmark evidence for this story persists comparable before and after artifact sets under one explicit label using the existing benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json contract.
- Each provider-optimization comparison row preserves the required contract fields from docs/plans/performance-evidence-benchmark-artifact-contract.md, including provider, baseline, strategy family, dataset size, change ratio, execution status, skip reason, iterations, timing metrics, allocation metrics, and persisted outcome.
- SQLite local temporary files remains the required completed baseline and includes provider-neutral fallback versus provider-optimized DVault rows for the provider-sensitive scenarios already used by the harness.
- For PostgreSQL, SQL Server, MySQL, and Oracle, provider-native bulk-ingestion baseline rows remain present in the artifact set; when a provider is configured and reachable its optimized and fallback rows complete, and when it is not configured the rows are emitted as executionStatus=skipped with the normalized skip reason.
- Each optimized provider scenario records deterministic execution detail that makes the exercised optimization auditable, using generated SQL when stable and practical or a provider-native execution detail string when SQL capture is not the stable boundary.

## Definition of Done
- The compared provider rows run with matching scenario inputs so the evidence isolates provider-strategy differences rather than workload changes.
- Artifact validation or tests prevent required provider rows and required result fields from disappearing silently.
- Repository documentation or benchmark-facing notes point back to the existing shared artifact contract and preserve the required SQLite plus optional-provider matrix.
- Produced evidence shows no unexplained provider regression against the selected baseline, or the artifact clearly records any accepted regression with the same scenario label and persisted context.

## Implementation Notes
- Reuse docs/plans/performance-evidence-benchmark-artifact-contract.md as the authoritative output contract; this story should extend the current harness and artifact assertions instead of creating a second format.
- Treat the current benchmark-summary.csv labels as the v1 naming baseline for scenario ids, provider labels, baseline ids, strategy-family ids, and skipped-provider wording.
- Keep SQLite as the only required always-completed local proof lane. External provider lanes are optional execution targets, but their rows must stay visible even when the providers are not configured.
- Compare provider-optimized rows against provider-neutral fallback rows under identical dataset size, change ratio, iteration count, warmup count, load-timestamp storage, and provider configuration inputs.
- If raw SQL text is too unstable for regression storage, normalize it to a deterministic excerpt or replace it with a concise provider-native execution detail that still proves which optimized path executed.
- No description update, attachment, planning-document write, child-ticket creation, or relation change was materialized during this PO pass because the existing repository contract and current live relation state already bound the v1 scope.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket promote the optional external-provider evidence lanes into provisioned CI or nightly regression gates instead of release or manually collected evidence only?
- Should a later performance ticket expand the external-provider matrix beyond save-strategy baselines into compiled-model, compiled-query, or DbContext-pooling comparisons once provider environments are provisioned more reliably?

## Risks
- Completed external-provider baselines remain environment-dependent because PostgreSQL, SQL Server, MySQL, and Oracle rows only execute when their configured connection strings and backing services are available.
- Generated SQL and other low-level provider execution details can drift across EF or provider-version changes, so capture needs normalization to avoid noisy false regressions.
- Small cross-provider timing deltas can be masked by machine variance, so allocation metrics and strict run-context parity remain necessary to keep the evidence interpretable.

## Split Recommendations
- No split recommended; current repository evidence already bounds this story to extending the existing performance-evidence contract and provider matrix.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Capture provider-level performance baselines for supported providers where practical, including generated SQL or provider-native execution details. The goal is to prevent accidental regressions in DVault abstractions and provider strategies.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

- decision: `implement`
- Added `executionDetail` to the shared benchmark row artifact shape for markdown, CSV, and JSON output.
- Provider-optimized benchmark rows now preserve deterministic selected-strategy evidence, and completed provider-native bulk-ingestion rows record diagnostics-backed save-strategy status, selected strategy, candidate count, fallback causes, and operation counts.
- Regenerated root `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` from the benchmark harness with the required SQLite completed rows and skipped optional PostgreSQL, SQL Server, MySQL, and Oracle provider rows.

### Verification

- `bash tools/check-format.sh` passed.
- `dotnet build DVault.slnx --nologo` passed with warnings only.
- `dotnet test DVault.slnx --nologo` passed; optional external-provider tests were skipped because local provider connection-string environment variables are not configured.
- `dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 3 --warmup 1 --output .` completed and wrote the benchmark artifact trio.

### Notes

- External-provider completed timing remains environment-dependent. In this local run, PostgreSQL, SQL Server, MySQL, and Oracle rows are intentionally visible as `executionStatus=skipped` with normalized not-configured skip reasons and planned provider strategy details.
<!-- gicket-bot:developer-delivery:v1:end -->

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

- decision: `implement`
- Rework persisted the required before/after benchmark artifact trio under `artifacts/benchmarks/06F492CTREZEDXVKJ839YGCPWW-provider-optimization-regression-baselines/`.
- `.gitignore` now explicitly allowlists the six ticket-labeled benchmark evidence files so they can be committed with the branch.
- Both generated artifact sets contain 32 benchmark rows: 24 completed SQLite local rows and 8 visible skipped optional-provider rows for PostgreSQL, SQL Server, MySQL, and Oracle.
- The markdown, CSV, and JSON rows preserve `executionDetail`; optimized provider rows retain selected provider strategy detail, including the planned external-provider strategy names when local connection strings are not configured.

### Verification

- `dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release --no-restore -- --iterations 3 --warmup 1 --output artifacts/benchmarks/06F492CTREZEDXVKJ839YGCPWW-provider-optimization-regression-baselines/before` passed and wrote the before artifact trio.
- `dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release --no-restore -- --iterations 3 --warmup 1 --output artifacts/benchmarks/06F492CTREZEDXVKJ839YGCPWW-provider-optimization-regression-baselines/after` passed and wrote the after artifact trio.
- `bash tools/check-format.sh` passed after artifact generation.
- `dotnet build DVault.slnx --nologo` passed with warnings only.
- `dotnet test DVault.slnx --nologo` passed; optional external-provider integration tests were skipped because local provider connection-string environment variables are not configured.

### Notes

- External-provider completed timing remains environment-dependent. This local evidence records those lanes as skipped with normalized not-configured reasons and deterministic planned execution detail.
- The NuGet audit warning about the read-only HTTP cache appeared during restore/build/test, but the build and test commands completed successfully.
<!-- gicket-bot:developer-delivery:v1:end -->