<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story as a closure-ready provider-aware read optimization follow-up. The existing child tickets cover the benchmark matrix, read-strategy hook, first SQLite optimization, and docs/release note updates; repository evidence confirms the hook, dispatcher, SQLite registration, and read benchmark scenarios are present. No planning writes or relation changes were needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Parent epic 06F0MEDTB8496GYVM9K42F9VPG remains the broader model-first and advanced read-model container; this story stays bounded to provider-aware read optimization follow-up closure.
- Repository evidence shows the provider read strategy contract in src/DCoding.Data.DVault/DataVaultProviderReadStrategy.cs, strategy dispatch in src/DCoding.Data.DVault/DefaultDataVaultReadService.cs, SQLite read-strategy registration in src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs, and read benchmark scenarios in benchmarks/DCoding.Data.DVault.Benchmarks/ReadModelBenchmarks.cs.
- The v1 optimized provider path is SQLite latest/as-of satellite reads for supported ordinary hub-parent satellite shapes. Other providers, PIT reads, bridge reads, and broader read shapes remain fallback or future optimization scope.
- No new child tickets, planning documents, attachments, or relation changes were materialized during this PO refinement run.

### Scope In
- Use the completed benchmark child ticket as the baseline evidence owner for latest satellite, PIT as-of, and bridge traversal read measurements across the visible provider matrix.
- Use the completed read-strategy hook child ticket and current source as the architecture baseline for provider-specific read strategy selection with provider-neutral fallback.
- Use the completed implementation child ticket as the first provider-specific optimization slice: SQLite latest/as-of satellite reads through the shared read-service entry point.
- Keep reproducibility requirements for benchmark commands, provider filters, timestamp storage, run context, measured rows, and deterministic skip reasons.
- Keep documentation/release-note updates aligned to implemented read behavior without claiming PIT refresh, bridge maintenance, or unimplemented provider optimizations.

### Scope Out
- Optimizing every provider or every read shape in this story.
- Provider-specific PIT or bridge read optimization.
- PIT row refresh orchestration, bridge traversal maintenance, or automatic graph maintenance.
- Changing write strategy behavior, save dispatch semantics, schema generation semantics, or caller-facing provider selection.
- Provisioning secrets, containers, cloud databases, or persistent benchmark infrastructure.

## Acceptance Criteria
- The completed child-ticket set demonstrates the story split: benchmark matrix, read strategy hook, first provider optimization, and docs/release notes each have a done owner.
- Benchmark coverage includes latest-satellite-read, pit-as-of-read, and bridge-traversal-read scenarios, and the provider filter matrix includes SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.
- Existing public read-service calls remain provider-neutral for callers while registered read strategies are evaluated before provider-neutral fallback for latest/as-of satellite reads.
- SQLite registration includes the provider read strategy and AddDVault alone remains provider-neutral fallback only.
- For the supported SQLite latest/as-of satellite shape, optimized output matches fallback semantics for row count, metadata/table names, parent hash keys, hash diff, load timestamp, record source, payload values, null handling, and deterministic ordering.
- Unsupported providers, unsupported shapes, PIT reads, and bridge reads retain correct fallback behavior or bounded provider-neutral behavior.
- Benchmark and documentation evidence are reproducible enough for a reviewer to identify the command, provider, configuration or skip reason, timestamp storage mode, and measured baseline/optimized rows.

## Definition of Done
- Linked child tickets for the benchmark matrix, read-strategy hook, first optimization, and docs/release updates are completed and remain consistent with this story's refined scope.
- Core read-strategy contracts, dispatcher behavior, fallback behavior, diagnostics, SQLite registration, optimized correctness, typed projection parity, and provider registration are covered by the implementation child evidence.
- Read benchmark documentation or output explains provider configuration discovery and deterministic skip reporting.
- Build, test, and SQLite benchmark smoke evidence from implementation remains attached in ticket comments, artifacts, or repository documentation as applicable.
- No new product-code work is required by this parent story beyond verifying the completed child outcomes.

## Implementation Notes
- Current source evidence ratifies the contract names IDataVaultProviderReadStrategy and DataVaultProviderReadStrategyContext for the additive provider read hook.
- DefaultDataVaultReadService dispatches registered provider read strategies by descending priority before calling DataVaultSatelliteReadPipeline fallback for latest/as-of satellite reads.
- AddDVaultSqlite registers SqliteDataVaultReadStrategy through the established provider service registration surface.
- ReadModelBenchmarks contains latest satellite, PIT as-of, and bridge traversal read scenarios; BenchmarkOptions exposes provider filters for all, sqlite, postgres, sqlserver, mysql, and oracle.
- Treat the done hook ticket 06F0MEJ7NANHCP64VR1SH3S3G8 as completed historical context; the done implementation ticket 06F0MEJE5WC51MFQ3CWDRATCWC is the concrete combined hook-plus-SQLite optimization delivery record.

## Open Questions
- none

## Follow-Up Questions
- After this story closes, use the benchmark matrix to decide whether another provider or read shape deserves a separate optimization ticket.
- Decide later whether benchmark artifacts should be archived in release notes or CI build artifacts for trend comparison.
- Decide later whether non-SQLite providers should gain standardized local container profiles for manual benchmarking.
- Decide later whether SQLite driving-key satellites, PIT reads, or bridge reads warrant separate provider-specific optimization work once their public surfaces and baseline data justify it.

## Risks
- Benchmark timings are machine-specific, so review should focus on reproducible commands and comparable before/after context rather than absolute numbers alone.
- Non-SQLite providers may have deterministic skip rows on machines without local configuration, leaving fewer measured optimization candidates.
- Docs can overstate advanced read behavior if they imply PIT refresh, bridge maintenance, or provider-specific PIT/bridge optimization; keep wording tied to implemented provider-neutral behavior.
- Public additive hook and diagnostics surfaces require compatibility review through existing API approval practices.

## Split Recommendations
- No additional split is recommended. The existing split is already materialized and done through 06F0MEJ0NE80R7CNS982S3PKVR, 06F0MEJ7NANHCP64VR1SH3S3G8, 06F0MEJE5WC51MFQ3CWDRATCWC, and 06F0MEJPGG7JBFEXD693BHY07W.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Benchmark and optimize the highest-impact read paths after provider-neutral latest/as-of/PIT/bridge correctness is in place.

## Scope In

- Benchmark matrix for latest satellite, PIT, and bridge reads across available providers.
- Provider-specific read strategy hook surface where beneficial.
- First provider-specific read optimization selected by measured impact.

## Scope Out

- Optimizing every provider/read shape in one story.
- Changing write strategy behavior unless required by read correctness.

## Acceptance Criteria

- Baseline and optimized measurements are documented reproducibly.
- Provider-specific read optimization is selected by evidence, not assumption.
- Fallback read behavior remains correct and available.