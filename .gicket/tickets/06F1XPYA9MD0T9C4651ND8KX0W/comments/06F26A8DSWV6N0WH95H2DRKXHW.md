[gicket-bot] PO refinement contract

Summary
- Refined the story to a bounded v1 compatibility proof for EF Core compiled models and compiled queries: SQLite-backed integration evidence, documentation of supported patterns and limitations, and benchmark claims only when stable artifacts exist.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 compatibility baseline is EF-owned compiled-model usage through a runtime model supplied with UseModel; DVault does not add a compiled-model generator or EF design-time integration for this ticket.
- The required compiled-model proof should verify DVault metadata annotations survive runtime model initialization for the shared EF metadata projection path.
- Compiled query examples should target stable EF query shapes over generated shared-type Data Vault entities or direct read projections; arbitrary dynamic request/projector combinations are not required to be compilable.
- SQLite is the required local provider baseline for this proof. Broader provider matrices are follow-up work unless the developer finds an issue that affects the shared provider-neutral surface.

Scope In
- Add or keep compatibility tests that initialize a compiled runtime model with UseModel and assert DVault model, entity, and property annotations remain available.
- Add or keep a representative EF.CompileQuery example/test that reads deterministic generated Data Vault values from a seeded SQLite database.
- Document supported compiled model and compiled query patterns, including any expected diagnostics or known limitations for unsupported query shapes.
- Verify normal non-compiled EF save/read usage continues to work through existing focused regression coverage.
- Benchmark only where the repository already has stable, attributable evidence; otherwise document that no performance claim is made.

Scope Out
- No exhaustive support for every dynamic query shape or caller-owned projector delegate as a compiled query.
- No provider-specific compiled model generator or DVault-owned dotnet ef compiled-model tooling.
- No requirement to add provider-specific compiled query optimizations for PostgreSQL, SQL Server, MySQL, or Oracle in this story.
- No PIT, bridge, or full graph traversal compiled-query matrix unless used only as optional supporting evidence.
- No package publication or release-management work.

Open questions
- none

Follow-up questions
- Should a later ticket add a generated dotnet ef dbcontext optimize sample project or committed generated-model fixture for documentation parity with EF CLI workflows?
- Should future performance work add dedicated compiled-query benchmark rows for stable read shapes once benchmark evidence is available?
- Should external providers receive a compiled compatibility smoke matrix after the SQLite/shared-surface proof is accepted?

Risks
- EF Core compiled-model internals can vary by EF version, so documentation should describe the tested UseModel/runtime-model boundary instead of making broader tooling claims.
- Compiled queries are only useful for stable query shapes; over-promising support for dynamic DVault read requests would create misleading performance expectations.
- Benchmark timing claims without archived artifacts and provider context would be fragile and should be omitted.

Split recommendations
- none

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