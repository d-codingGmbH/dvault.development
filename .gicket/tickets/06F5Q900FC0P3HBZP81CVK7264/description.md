<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story around the existing provider-native bulk benchmark surface, the staged/direct provider thresholds already visible in the repo, and the shared artifact-contract budgets; no PO-blocking questions remain.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Done provider stories already landed the current staged or provider-specific save behavior for PostgreSQL, SQL Server, MySQL, and the retained direct Oracle path, so this ticket is benchmark-and-evidence work only.
- The visible benchmark baseline is the existing `provider-native-bulk-ingestion` scenario with external PostgreSQL, SQL Server, MySQL, and Oracle rows that remain present as completed rows when configured or skipped rows with normalized reasons when not configured.
- The shared artifact contract already fixes the row schema and regression-budget defaults: the targeted metric must improve or hold, required SQLite non-target regressions above 5% fail by default, and configured optional-provider regressions above 10% must be called out and justified.
- Repository evidence already fixes the current provider-path boundaries: PostgreSQL stays on direct or UNNEST below its 60-operation staged threshold, MySQL keeps a 50-operation native gate with staged bulk at 60+, SQL Server uses its current native bulk gate, and Oracle keeps the retained direct batching path with staged Oracle still `not-selected-no-measured-win`.
- No human comments or attachments added extra scope beyond the ticket description and referenced repository documents.

### Scope In
- Extend benchmark coverage for staged-bulk comparison rows on the existing provider-native bulk-ingestion surface, reusing the current artifact schema and benchmark triplet.
- Make the matrix distinguish provider-neutral fallback, retained provider-native direct or multi-row paths where they exist, and staged-provider paths where they exist.
- Preserve normalized skipped optional-provider rows and planned execution detail for all staged/direct comparison rows when external providers are not configured.
- Capture before/after evidence for this ticket under the shared artifact contract with explicit staged-bulk targeted rows and the existing regression budgets.
- Add or update benchmark contract tests and benchmark-facing docs only as needed to describe the new matrix and budget application.

### Scope Out
- Changing provider save semantics, thresholds, or strategy-selection behavior already owned by the landed provider stories.
- New public save APIs, chunked provider-specific execution work, or staged SPI and transaction-contract redesign.
- Benchmark artifact schema redesign or new artifact file types beyond the existing markdown, CSV, and JSON triplets plus optional SQL capture already defined in the shared contract.
- Broad release-note, README, production-checklist, or stored-procedure positioning work already owned by `06F5Q90718D21DN1N1Q2AP7YEM`.
- Rewriting historical release bundles as the public claim record for past releases; new evidence should stand on its own ticket or release label.

## Acceptance Criteria
- The benchmark harness adds a staged-bulk comparison matrix on top of the existing `provider-native-bulk-ingestion` evidence surface and keeps the current provider filter, run context, and artifact-triplet contract intact.
- For providers that already have both a retained provider-native path and a staged path in repository evidence, the matrix includes distinct comparison rows that make those paths separately visible; for SQL Server and the current Oracle baseline, the matrix keeps the currently visible native boundary explicit instead of inventing unsupported extra paths.
- Every staged/direct comparison row preserves timing, allocation, deterministic `executionDetail`, selected or planned strategy identity, and cleanup or boundary detail without adding new artifact columns; skipped optional-provider rows remain visible with normalized skip reasons.
- Before/after artifacts for this ticket reuse the existing regression-budget policy rather than introducing new numeric thresholds: targeted staged-bulk rows must improve or hold, and configured optional-provider regressions above 10% are explicitly called out and justified.
- Automated benchmark artifact tests cover row presence, row identity, skip-row behavior, and execution-detail expectations for the staged-bulk matrix, while default local runs remain valid without external databases.

## Definition of Done
- The repository has a stable staged-bulk matrix surface that developers can run and archive through the existing benchmark artifact pipeline without changing the shared artifact schema.
- Checked-in benchmark evidence for this ticket includes comparable before/after triplets under one explicit label and keeps optional-provider skipped rows visible when providers are unavailable.
- Benchmark-facing documentation and tests explain the staged/direct comparison boundary well enough that downstream docs or release-note work can cite the matrix without reopening benchmark-contract questions.
- The work lands without reopening provider implementation tickets or widening the public `IDataVaultSaveService` boundary.

## Implementation Notes
- Reuse the current `provider-native-bulk-ingestion` scenario family and its existing 63-operation staged-eligible batch as the staged comparison anchor instead of inventing a separate artifact format.
- Add at least one provider-native but staged-ineligible comparison shape where the current repo already exposes a meaningful retained path, especially PostgreSQL direct or UNNEST below the 60-operation staged threshold and MySQL multi-row execution between the 50-operation native gate and the 60-operation staged threshold.
- Keep SQL Server v1 bounded to its current native bulk row and keep Oracle v1 bounded to the retained direct Oracle row with explicit `stagedOracleBulk=not-selected-no-measured-win` evidence until a runnable staged Oracle path proves a net win.
- Preserve the current benchmark triplet fields and use `executionDetail` for path identity, staged/direct boundary text, strategy names, and cleanup or boundary hints rather than adding new artifact columns.
- Use the existing optional-provider discovery model and connection-string environment variables so the matrix emits completed rows when configured and deterministic skipped rows when not configured.
- Keep SQL capture optional for provider-native bulk claims unless the specific claim depends on emitted SQL or transfer shape beyond what the artifact contract already allows `executionDetail` to prove.

## Open Questions
- none

## Follow-Up Questions
- After this matrix lands, should `06F5Q90718D21DN1N1Q2AP7YEM` publish the staged/direct provider comparison as the v0.20.0 documentation baseline rather than relying on prose-only staged-bulk guidance?
- If Oracle later gains a runnable staged path with a measured win over the retained direct path, should a separate follow-up add Oracle direct-versus-staged timing rows instead of widening this ticket beyond the current benchmark baseline?

## Risks
- Because PostgreSQL, SQL Server, MySQL, and Oracle rows remain external opt-in, unattended runs may still archive skipped rows only; the contract must stay informative enough that missing live providers does not look like missing matrix coverage.
- If the new matrix does not separate direct, staged, and fallback row identities cleanly, regression budgets and downstream docs will compare the wrong execution paths.
- Updating or superseding historical provider-optimization bundles without a clearly labeled new evidence set could blur release provenance and make regressions harder to interpret.

## Split Recommendations
- No additional split is needed for PO refinement if the work stays on benchmark harness, artifact evidence, and benchmark-contract documentation for staged bulk comparisons.
- If future work wants cross-scenario budget policy changes beyond provider-native bulk ingestion, split that governance work into a separate artifact-contract ticket rather than widening this story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Add benchmark evidence for staged provider bulk ingestion.

Acceptance criteria:
- Compares provider-neutral, existing provider-native, and staged provider paths where configured.
- Keeps skipped optional provider rows visible with normalized skip reasons.
- Records timing, allocation, execution detail, staging strategy, and cleanup outcome.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Added the staged/direct provider-native bulk benchmark matrix on the existing `provider-native-bulk-ingestion` surface without changing artifact columns.
- PostgreSQL now has a distinct retained direct-or-UNNEST skipped/completed row below the 60-operation staged threshold plus the existing staged COPY row.
- MySQL now has a distinct retained multi-row skipped/completed row above the 50-operation native gate and below the 60-operation staged threshold plus the existing staged row.
- SQL Server remains bounded to the current native bulk row, and Oracle remains bounded to the retained direct row with `stagedOracleBulk=not-selected-no-measured-win`.

### Evidence
- Checked-in before/after triplets were added under `artifacts/benchmarks/06F5Q900FC0P3HBZP81CVK7264-staged-bulk-matrix/`.
- The after/root artifact matrix contains 37 result rows and keeps all optional-provider rows visible with normalized not-configured skip reasons in this unattended environment.
- Benchmark contract tests were updated to assert row identity, skipped-row behavior, and execution-detail text for the staged/direct boundaries.

### Verification
- `bash tools/check-format.sh` passed.
- `dotnet test DVault.slnx --nologo --no-restore --filter FullyQualifiedName~BenchmarkScenarioExecutionTests` could not compile because package `Microsoft.EntityFrameworkCore.Analyzers` version `10.0.8` was missing from local restore assets.
- `dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release --no-restore -- --provider sqlite --iterations 1 --warmup 0` hit the same missing package condition.
- Network restore was not attempted because this bot execution was instructed to avoid network-dependent behavior.

### Notes
- Live external-provider timing remains opt-in through the existing provider connection-string environment variables.
- The checked-in optional-provider rows are skipped evidence for this unattended run; configured provider lanes should rerun the benchmark command before making live provider timing claims.
<!-- gicket-bot:developer-delivery:v1:end -->

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Added the staged/direct provider-native bulk benchmark matrix on the existing `provider-native-bulk-ingestion` surface without changing artifact columns.
- PostgreSQL has a distinct retained direct-or-UNNEST row below the 60-operation staged threshold plus the staged COPY row.
- MySQL has a distinct retained multi-row row above the 50-operation native gate and below the 60-operation staged threshold plus the staged row.
- SQL Server remains bounded to the current native bulk row, and Oracle remains bounded to the retained direct row with `stagedOracleBulk=not-selected-no-measured-win`.
- Rework added the previously missing checked-in before/after benchmark triplets under `artifacts/benchmarks/06F5Q900FC0P3HBZP81CVK7264-staged-bulk-matrix/` and whitelisted that exact bundle in `.gitignore`.

### Evidence
- The before triplet under `artifacts/benchmarks/06F5Q900FC0P3HBZP81CVK7264-staged-bulk-matrix/before/` preserves the pre-matrix root benchmark summary with `Benchmark baselines: 35`.
- The after triplet under `artifacts/benchmarks/06F5Q900FC0P3HBZP81CVK7264-staged-bulk-matrix/after/` matches the current root benchmark summary and preserves `Benchmark baselines: 37`.
- The after/root artifact matrix keeps optional-provider rows visible with normalized `not configured: DVAULT_TEST_*` skip reasons in this unattended environment.
- The after triplet contains `dvault-adddvaultpostgres-direct-or-unnest` with `stagedBulkBoundary=below-60-operations` and `cleanupBoundary=no-staging-table`.
- The after triplet contains `dvault-adddvaultmysql-multi-row` with `selectedStrategy=MySqlDataVaultSaveStrategy`, `nativeBulkBoundary=50-plus-operations`, and `stagedBulkBoundary=below-60-operations`.
- Benchmark contract tests on the branch assert row identity, skipped-row behavior, unchanged artifact schema, and execution-detail text for the staged/direct boundaries.

### Verification
- `bash tools/check-format.sh` passed.
- `dotnet test DVault.slnx --nologo --no-restore --filter FullyQualifiedName~BenchmarkScenarioExecutionTests` did not compile because local restore assets are missing package `Microsoft.EntityFrameworkCore.Analyzers` version `10.0.8`.
- Network restore was not attempted because this bot execution was instructed to avoid network-dependent behavior.

### Notes
- Live external-provider timing remains opt-in through the existing provider connection-string environment variables.
- The checked-in optional-provider rows are skipped evidence for this unattended run; configured provider lanes should rerun the benchmark command before making live provider timing claims.
<!-- gicket-bot:developer-delivery:v1:end -->