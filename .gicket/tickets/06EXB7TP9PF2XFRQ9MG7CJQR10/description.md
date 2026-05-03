<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket around the existing SQLite benchmark runner: v1 should emit documentation-ready markdown, CSV, and JSON artifacts from the current four benchmark baselines and document the required hardware and provider context.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Use the existing benchmarks/DCoding.Data.DVault.Benchmarks runner as the sole v1 artifact source rather than introducing a separate benchmark pipeline.
- The v1 comparison surface is the four current baselines already wired in BenchmarkRunner: customer profile plain EF, customer profile DVault, order-product plain EF, and order-product DVault.
- The provider baseline is the current SQLite local temporary-file execution documented in the benchmark README; Postgres, Docker, secrets, and multi-provider coverage stay out of scope for this ticket.
- Artifacts should be generated from one benchmark execution and represent the same summary rows across markdown, CSV, and JSON outputs so docs and machines consume the same result set.
- The minimum v1 machine/runtime context is OS description, OS/process architecture, processor count, and .NET runtime version alongside the provider and benchmark-option values for the run.
- Scenario inputs remain the fixed contracts already codified in ScenarioContracts and the shared customer-profile comparison contract; this ticket is about artifact emission and documentation, not new benchmark scenarios.

### Scope In
- Extend the existing benchmark runner so one benchmark execution can emit documentation-ready artifact files alongside the current console output.
- Emit one markdown summary, one CSV summary, and one JSON summary for the current benchmark baselines.
- Capture run-level context needed for documentation, including provider identity, benchmark options, and machine/runtime context.
- Update benchmark-facing documentation to explain how to generate the artifacts and how downstream docs should preserve their context.

### Scope Out
- Adding new benchmark scenarios, changing the fixed scenario contracts, or expanding beyond the current four baselines.
- Provider matrices, Postgres benchmarking, Docker-based execution, or any external-service benchmark flow.
- Historical trend storage, CI publication, dashboards, or automatic documentation publishing.
- Release-governance or NuGet publication policy work owned by separate packaging/release documentation tickets.

## Acceptance Criteria
- The benchmark command can emit three files from one run into a selected output directory with deterministic filenames: benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json.
- All three artifact formats describe the same benchmark result set for the four current baselines and include scenario name, baseline name, iteration count, mean milliseconds, min milliseconds, max milliseconds, and persisted outcome.
- The emitted markdown and JSON artifacts capture documentation context for the run: the provider is identified as SQLite local temporary files, and the run records iterations, warmup count, OS description, OS/process architecture, processor count, and .NET runtime version.
- The markdown artifact is directly referenceable from docs by including a readable summary section and the benchmark table without requiring console copy/paste.
- Benchmark documentation explains the artifact-generation command and states that downstream docs must preserve the hardware/provider context when citing benchmark results.

## Definition of Done
- The acceptance criteria are satisfied using the existing benchmark project, the current four baselines, and the shared scenario contracts.
- The benchmark README or another benchmark-owned documentation page is updated with the artifact workflow and context expectations.
- The benchmark runner continues to support the existing --iterations and --warmup flow while adding artifact emission without introducing external service prerequisites.
- Repository formatting and shared implementation standards continue to apply.

## Implementation Notes
- Reuse the current BenchmarkSummary data and existing console markdown table shape as the canonical summary source so file artifacts and console output stay aligned.
- Treat CSV as the row-oriented export; keep richer run-level context in the markdown and JSON outputs instead of duplicating envelope metadata in every CSV row.
- Keep the v1 provider statement aligned with the current README and runner output: SQLite local temporary files only.
- The exact CLI switch name for the output directory is an implementation detail; the PO contract is deterministic multi-format emission from the existing runner, not a specific option spelling.
- Do not turn this ticket into benchmark result publication or baseline-number curation; it only needs to emit reusable artifacts and document how to reference them.

## Open Questions
- none

## Follow-Up Questions
- When additional provider profiles exist, should a later benchmark ticket define a multi-provider artifact comparison contract instead of reusing the SQLite-only v1 artifact shape unchanged?
- Should related ticket 06EXB82GR48V364RP5NHYE2T70 reference a checked-in benchmark artifact example once the manual release checklist is finalized?

## Risks
- Benchmark numbers vary by machine, so docs can mislead if artifact context is incomplete or stripped when results are copied.
- If generated benchmark outputs are later committed without a refresh policy, docs may cite stale measurements even when the artifact format is correct.

## Split Recommendations
- No split recommended; artifact emission and benchmark-documentation updates share one bounded surface in the existing benchmark runner and benchmark README.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Make benchmark output easy to include in docs.

## Scope
- Capture markdown, CSV, or JSON summaries from benchmark runs.

## Acceptance Criteria
- Artifacts can be referenced from documentation.
- Docs explain hardware and provider context.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.