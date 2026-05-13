<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story to a bounded v1 compatibility proof for EF Core compiled models and compiled queries: SQLite-backed integration evidence, documentation of supported patterns and limitations, and benchmark claims only when stable artifacts exist.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 compatibility baseline is EF-owned compiled-model usage through a runtime model supplied with UseModel; DVault does not add a compiled-model generator or EF design-time integration for this ticket.
- The required compiled-model proof should verify DVault metadata annotations survive runtime model initialization for the shared EF metadata projection path.
- Compiled query examples should target stable EF query shapes over generated shared-type Data Vault entities or direct read projections; arbitrary dynamic request/projector combinations are not required to be compilable.
- SQLite is the required local provider baseline for this proof. Broader provider matrices are follow-up work unless the developer finds an issue that affects the shared provider-neutral surface.

### Scope In
- Add or keep compatibility tests that initialize a compiled runtime model with UseModel and assert DVault model, entity, and property annotations remain available.
- Add or keep a representative EF.CompileQuery example/test that reads deterministic generated Data Vault values from a seeded SQLite database.
- Document supported compiled model and compiled query patterns, including any expected diagnostics or known limitations for unsupported query shapes.
- Verify normal non-compiled EF save/read usage continues to work through existing focused regression coverage.
- Benchmark only where the repository already has stable, attributable evidence; otherwise document that no performance claim is made.

### Scope Out
- No exhaustive support for every dynamic query shape or caller-owned projector delegate as a compiled query.
- No provider-specific compiled model generator or DVault-owned dotnet ef compiled-model tooling.
- No requirement to add provider-specific compiled query optimizations for PostgreSQL, SQL Server, MySQL, or Oracle in this story.
- No PIT, bridge, or full graph traversal compiled-query matrix unless used only as optional supporting evidence.
- No package publication or release-management work.

## Acceptance Criteria
- A compiled-model compatibility test passes and proves DVault annotations such as metadata source, entity kind, metadata name, produced name, property role, and technical column role are available after the model is supplied through UseModel.
- A representative compiled query using EF.CompileQuery reads expected generated Data Vault values from seeded SQLite data, or an unsupported shape fails with an explicitly documented diagnostic/limitation.
- Documentation explains the supported compiled-model pattern, supported compiled-query examples, and known limitations without promising exhaustive dynamic query compilation.
- Existing non-compiled EF usage remains covered by passing focused save/read tests or the relevant existing test suite.
- Any benchmark statement is tied to stable repository benchmark artifacts with provider and environment context; absent such evidence, docs avoid performance claims.

## Definition of Done
- Compatibility tests live in the established test layout and use existing provider traits/helpers for the SQLite local integration baseline.
- Docs or release notes are updated to describe the compiled model/query compatibility boundary in user-facing terms.
- The implementation does not introduce DVault-owned EF design-time services, custom dotnet ef commands, or provider-specific compiled model generation.
- Relevant tests are run and their command/results are recorded in the handoff or implementation notes.

## Implementation Notes
- Use the existing generated shared-type table access pattern, such as context.Set<Dictionary<string, object>>(producedName) with EF.Property<T>, for compiled query examples.
- A small metadata model with one hub and one hub-parent satellite is sufficient to exercise model-level annotations, entity annotations, business-key roles, and technical column roles.
- If using runtime initialization, obtain the design model through EF services and initialize the runtime model before passing it to UseModel, or use an equivalent EF-supported compiled-model mechanism.
- Keep documentation precise: compiled queries are recommended for stable direct EF query shapes, while IDataVaultReadService request-based reads and caller-owned projector delegates remain the normal flexible path.
- Avoid widening the ticket into provider-specific performance optimization unless test evidence reveals a compatibility defect in shared behavior.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket add a generated dotnet ef dbcontext optimize sample project or committed generated-model fixture for documentation parity with EF CLI workflows?
- Should future performance work add dedicated compiled-query benchmark rows for stable read shapes once benchmark evidence is available?
- Should external providers receive a compiled compatibility smoke matrix after the SQLite/shared-surface proof is accepted?

## Risks
- EF Core compiled-model internals can vary by EF version, so documentation should describe the tested UseModel/runtime-model boundary instead of making broader tooling claims.
- Compiled queries are only useful for stable query shapes; over-promising support for dynamic DVault read requests would create misleading performance expectations.
- Benchmark timing claims without archived artifacts and provider context would be fragile and should be omitted.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Make it clear and tested how DVault behaves with EF Core compiled models and compiled queries.

## Scope In

- Add compatibility tests for compiled models with DVault metadata annotations.
- Add compiled query examples for representative read APIs.
- Document supported patterns and known limitations.
- Benchmark where stable evidence is available.

## Scope Out

- No requirement to support every dynamic query shape as compiled.
- No provider-specific compiled model generator.

## Acceptance Criteria

- Compiled model tests pass for supported metadata paths.
- Compiled query examples work or fail with documented diagnostics.
- Normal non-compiled EF usage does not regress.

## Implementation Notes

- Let evidence guide whether additional optimization is needed.

## Open Questions

- none