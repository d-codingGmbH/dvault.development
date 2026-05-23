<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket to use the existing SQLite benchmark harness and artifact contract as the baseline for provider-neutral latest/as-of/PIT/bridge read allocation tuning; no child tickets or relation/document writes were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already provides the benchmark baseline: the harness emits provider-neutral read rows plus benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json for the required SQLite local baseline.
- Current benchmark evidence already shows material provider-neutral allocation gaps versus the SQLite optimized read rows for latest-satellite, PIT as-of, and bridge traversal reads, so tuning is justified without reopening scope discovery.
- Scope stays on provider-neutral read tuning only; provider-specific read strategies, save-path scenarios, compiled-model or compiled-query work, DbContext pooling, and PIT or bridge maintenance writes are outside this ticket unless used only as unchanged guardrails.
- Relation state was verified and left unchanged: this ticket remains a child of 06F492BTNHRPBC7D24E13ECFKM, has incoming blocks from 06F492BZPP5YT9SJSPDHQBGF3R and 06F492C50WM7V2NE0WZB3774XM, and outgoing blocks to 06F492CTREZEDXVKJ839YGCPWW and 06F492D05THPGQVT3B3K7853A0.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized in this refinement pass.

### Scope In
- Measure provider-neutral allocation and materialization overhead on the existing benchmarked read baselines for latest/current satellite reads, as-of/PIT-backed reads, and bridge traversal reads.
- Tune provider-neutral read-service, query, and materialization code paths only where before/after evidence on the same scenario shows a real improvement.
- Preserve the existing public provider-neutral read boundary, including IDataVaultReadService request shapes, raw record shapes, and caller-owned projector patterns.

### Scope Out
- Provider-specific optimized read-strategy work such as AddDVaultSqlite or non-SQLite provider-native optimization.
- Save-path or bulk-ingestion performance work, plus compiled-model startup, compiled-query, and DbContext-pooling benchmarks except as unchanged guardrails.
- New public read APIs, PIT or bridge maintenance semantics, metadata-model expansion, or release-note work beyond recording benchmark evidence for this ticket.

## Acceptance Criteria
- A comparable before/after benchmark evidence set is produced for the targeted provider-neutral read scenarios using the existing artifact trio under one explicit scenario or ticket label and the same run options and provider context.
- The evidence covers the provider-neutral baselines relevant to this ticket's latest/current, as-of/PIT, and bridge read shapes, and each claimed optimization is tied to the exact scenario row or rows it improved.
- Any accepted code change shows reduced allocation or materialization cost on the targeted scenario without regressing observable read correctness, API clarity, or provider-neutral compatibility.
- When a performance claim depends on SQL shape, index usage, or materialization behavior rather than pure allocation effects, representative SQL is captured beside the same before/after artifact set.

## Definition of Done
- Affected provider-neutral read benchmarks, tests, and any necessary supporting fixtures are updated and pass on the bounded branch baseline.
- Before/after benchmark artifacts preserve run context, including provider filter, load-timestamp storage, runtime or OS context, execution status, and skipped optional providers, so the comparison remains reproducible.
- The implementation leaves provider-specific optimized rows as comparison baselines rather than required code changes and does not widen the public API surface.
- The final handoff states which benchmark rows improved, which read paths were intentionally left unchanged, and why.

## Implementation Notes
- Reuse the existing benchmark scenario contracts rather than inventing new workloads; the current harness already seeds bounded customer latest/history data, PIT rows, and bridge traversal data for the required SQLite evidence.
- Start with the provider-neutral rows that show the clearest allocation headroom in the checked-in snapshot: latest-satellite read 2293472 versus 247312 allocated bytes, PIT as-of read 5767776 versus 2422320, then bridge traversal read 318336 versus 116288.
- Treat the current repository artifact contract as authoritative for evidence shape: markdown, CSV, and JSON must describe the same rows, and skipped optional providers stay visible rather than disappearing from archived artifacts.
- Keep tuning inside provider-neutral query and materialization internals; do not make the benchmark depend on provider-specific APIs or on changing consumer-facing request or record contracts.

## Open Questions
- none

## Follow-Up Questions
- If one read family needs a materially larger refactor than the others, should it become a dedicated follow-up ticket after this bounded evidence pass rather than expanding this story in place?
- After provider-neutral wins are measured, do we want a separate backlog pass to evaluate whether any of the same hotspots justify provider-specific read-strategy follow-up outside SQLite?

## Risks
- The checked-in benchmark summary is a single-iteration SQLite snapshot, so noisy or machine-specific deltas can mislead prioritization unless before and after comparisons reuse the same scenario and preserve full run context.
- Allocation reductions may shift cost into slower SQL or more complex query shapes; claims that depend on emitted SQL or index behavior need SQL capture, not only allocation numbers.
- Trying to close every gap to the SQLite optimized row in one pass could over-expand the story; prioritize the biggest provider-neutral wins first and document smaller residual gaps.

## Split Recommendations
- No immediate split is required from current evidence; keep one ticket unless profiling shows one read family needs an isolated architectural change that would dilute the bounded provider-neutral tuning scope.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Profile current/as-of/PIT/bridge reads for allocations and materialization overhead. Tune provider-neutral code paths where benchmarks show meaningful wins without reducing API clarity or provider compatibility.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Tuned provider-neutral read materialization for latest-satellite, PIT as-of, and bridge traversal rows without changing public read request, record, or projector contracts.
- Persisted matched before/after evidence under `artifacts/benchmarks/06F492CAB2293R7BGJWMWMRKT4-provider-neutral-read-allocations/` and updated the root `benchmark-summary.*` files to the after run.

### Benchmark Evidence
- Command: `dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -p:MinVerVersionOverride=0.0.0 -- --iterations 1 --warmup 0 --provider all --output artifacts/benchmarks/06F492CAB2293R7BGJWMWMRKT4-provider-neutral-read-allocations/after`
- Shared context: iterations `1`, warmup `0`, load timestamp storage `ProviderDefault`, provider filter `all`, required provider `SQLite local temporary files`; PostgreSQL, SQL Server, MySQL, and Oracle rows remained visible as skipped because connection-string environment variables were not configured.
- `latest-satellite-read` / `dvault-adddvault-fallback`: allocation improved from `2293472` to `1746656` bytes.
- `pit-as-of-read` / `dvault-adddvault-fallback`: allocation improved from `5767776` to `5684608` bytes.
- `bridge-traversal-read` / `dvault-adddvault-fallback`: allocation improved from `318336` to `292984` bytes.
- Representative bridge SQL shape for the new provider-neutral depth predicate is captured at `artifacts/benchmarks/06F492CAB2293R7BGJWMWMRKT4-provider-neutral-read-allocations/after/provider-neutral-bridge-depth-sql.md`.

### Read Paths Left Unchanged
- Provider-specific optimized read strategies stayed as comparison baselines; no SQLite-specific optimization work was added for this story.
- Public `IDataVaultReadService` request shapes, raw record shapes, registry adapters, and caller-owned typed projector APIs were not widened.
- Save paths, compiled model/query paths, DbContext pooling, PIT/bridge maintenance writes, and optional external-provider strategies remained out of scope.

### Verification
- `dotnet build DVault.slnx --nologo` passed with existing NuGet cache/analyzer warnings and `0` errors.
- `dotnet test DVault.slnx --nologo` passed: integration `173` total, `157` succeeded, `16` skipped; unit `364` total, `364` succeeded.
- `bash tools/check-format.sh` passed after normalizing the archived before artifact BOMs.

### Notes
- The benchmark evidence is a single-iteration local SQLite run, so non-target timing variance should not be overinterpreted. The tuning claim is allocation/materialization-focused.
<!-- gicket-bot:developer-delivery:v1:end -->

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Resolved tester rework by refreshing the comparable benchmark evidence so targeted provider-neutral read rows improve while required SQLite non-target allocation rows stay within the 5% regression budget.
- Added a small benchmark harness support guard that drains pending finalizers before each measured operation, avoiding cross-scenario allocation noise in the persisted evidence.
- Kept production read-surface changes scoped to the existing provider-neutral latest-satellite, PIT as-of, and bridge traversal internals; public read request, record, and projector contracts remain unchanged.

### Benchmark Evidence
- Command: `dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -p:MinVerVersionOverride=0.0.0 -- --iterations 3 --warmup 1 --provider all --output artifacts/benchmarks/06F492CAB2293R7BGJWMWMRKT4-provider-neutral-read-allocations/after`
- Shared context: iterations `3`, warmup `1`, load timestamp storage `ProviderDefault`, provider filter `all`, required provider `SQLite local temporary files`; PostgreSQL, SQL Server, MySQL, and Oracle rows remained visible as skipped because connection-string environment variables were not configured.
- `latest-satellite-read` / `dvault-adddvault-fallback`: allocation improved from `2201072` to `1752680` bytes (`-20.37%`).
- `pit-as-of-read` / `dvault-adddvault-fallback`: allocation improved from `2603512` to `2519512` bytes (`-3.23%`).
- `bridge-traversal-read` / `dvault-adddvault-fallback`: allocation improved from `181960` to `149656` bytes (`-17.75%`).
- Tester rework guardrails now pass in the same evidence set: `order-product-fulfillment-history` / `dvault-adddvault-fallback` held at `414560` bytes, `dbcontext-pooling-dvault-operation` / `adddbcontext` improved from `164269` to `164136` bytes, and `dbcontext-pooling-dvault-operation` / `adddbcontextpool` improved from `88768` to `86296` bytes.
- Representative bridge SQL shape for the provider-neutral maximum-depth predicate remains captured at `artifacts/benchmarks/06F492CAB2293R7BGJWMWMRKT4-provider-neutral-read-allocations/after/provider-neutral-bridge-depth-sql.md`.

### Read Paths Left Unchanged
- Provider-specific optimized read strategies stayed as comparison baselines; no SQLite-specific read optimization was added for this story.
- Public `IDataVaultReadService` request shapes, raw record shapes, registry adapters, and caller-owned typed projector APIs were not widened.
- Save paths, compiled model/query paths, DbContext pooling behavior, PIT/bridge maintenance writes, and optional external-provider strategies remained out of scope; the refreshed evidence only removes the benchmark-artifact non-target regression blocker.

### Verification
- `dotnet build DVault.slnx --nologo` passed with existing NuGet cache/analyzer warnings and `0` errors.
- `dotnet test DVault.slnx --nologo` passed: integration `173` total, `157` succeeded, `16` skipped; unit `364` total, `364` succeeded.
- `bash tools/check-format.sh` passed.
- Local CSV check confirmed all required SQLite non-target allocation deltas in the refreshed before/after artifact pair are within the 5% budget.

### Notes
- The refreshed evidence uses `3` measured iterations and `1` warmup iteration to reduce single-run startup variance that caused the tester return. The full run context is preserved in the markdown and JSON artifacts.
<!-- gicket-bot:developer-delivery:v1:end -->