Plan and coordinate v0.31.0 as a documentation-and-example release for performance decision guidance and observability adoption.

Release intent
- Help adopters choose between existing DVault save/read profiles, diagnostics, tracing, metrics, and examples without adding dashboards, hosted observability, ingestion orchestration, background maintenance, or platform behavior.
- Keep the release focused on repository-visible documentation and examples. Runtime behavior changes belong in separate tickets and are out of scope for this epic.

Expected child flow
- First define the authoritative decision-tree contract in `docs/performance-profiles.md`.
- Then add practical decision-tree documentation and fallback examples based on that contract.
- Then add bounded observability and realistic EF Core examples that link back to the decision-tree guidance.
- Finally update release documentation and top-level navigation after the child outputs exist.

Done for the epic
- All v0.31.0 child tickets are completed or explicitly closed as not needed with evidence.
- `docs/releases/v0.31.0.md` exists and links to the final guidance and examples.
- The final documentation consistently states the non-goals: no dashboards, exporters, collectors, hosting, scheduling, automatic PIT/bridge maintenance, provider-specific SQL artifact workflow, or runtime routing beyond existing diagnostics-gated behavior.