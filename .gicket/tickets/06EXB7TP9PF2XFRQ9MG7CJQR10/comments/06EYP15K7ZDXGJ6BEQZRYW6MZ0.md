[gicket-bot] PO refinement contract

Summary
- Refined the ticket around the existing SQLite benchmark runner: v1 should emit documentation-ready markdown, CSV, and JSON artifacts from the current four benchmark baselines and document the required hardware and provider context.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Use the existing benchmarks/DCoding.Data.DVault.Benchmarks runner as the sole v1 artifact source rather than introducing a separate benchmark pipeline.
- The v1 comparison surface is the four current baselines already wired in BenchmarkRunner: customer profile plain EF, customer profile DVault, order-product plain EF, and order-product DVault.
- The provider baseline is the current SQLite local temporary-file execution documented in the benchmark README; Postgres, Docker, secrets, and multi-provider coverage stay out of scope for this ticket.
- Artifacts should be generated from one benchmark execution and represent the same summary rows across markdown, CSV, and JSON outputs so docs and machines consume the same result set.
- The minimum v1 machine/runtime context is OS description, OS/process architecture, processor count, and .NET runtime version alongside the provider and benchmark-option values for the run.
- Scenario inputs remain the fixed contracts already codified in ScenarioContracts and the shared customer-profile comparison contract; this ticket is about artifact emission and documentation, not new benchmark scenarios.

Scope In
- Extend the existing benchmark runner so one benchmark execution can emit documentation-ready artifact files alongside the current console output.
- Emit one markdown summary, one CSV summary, and one JSON summary for the current benchmark baselines.
- Capture run-level context needed for documentation, including provider identity, benchmark options, and machine/runtime context.
- Update benchmark-facing documentation to explain how to generate the artifacts and how downstream docs should preserve their context.

Scope Out
- Adding new benchmark scenarios, changing the fixed scenario contracts, or expanding beyond the current four baselines.
- Provider matrices, Postgres benchmarking, Docker-based execution, or any external-service benchmark flow.
- Historical trend storage, CI publication, dashboards, or automatic documentation publishing.
- Release-governance or NuGet publication policy work owned by separate packaging/release documentation tickets.

Open questions
- none

Follow-up questions
- When additional provider profiles exist, should a later benchmark ticket define a multi-provider artifact comparison contract instead of reusing the SQLite-only v1 artifact shape unchanged?
- Should related ticket 06EXB82GR48V364RP5NHYE2T70 reference a checked-in benchmark artifact example once the manual release checklist is finalized?

Risks
- Benchmark numbers vary by machine, so docs can mislead if artifact context is incomplete or stripped when results are copied.
- If generated benchmark outputs are later committed without a refresh policy, docs may cite stale measurements even when the artifact format is correct.

Split recommendations
- No split recommended; artifact emission and benchmark-documentation updates share one bounded surface in the existing benchmark runner and benchmark README.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment