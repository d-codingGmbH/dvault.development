Finalize v0.31.0 release documentation after the decision guidance and examples have landed.

Required repository output
- Add `docs/releases/v0.31.0.md` with the final release summary, evidence links, and non-goals.
- Update top-level navigation only where needed, such as `README.md`, `docs/README.md`, `docs/performance-profiles.md`, `docs/production-adoption-checklist.md`, or `examples/README.md`, to point at the already-landed v0.31.0 guidance and examples.
- This ticket must produce release-documentation changes outside `.gicket`.

Scope in
- Summarize the final decision-tree contract, practical guidance, observability examples, and realistic EF Core example outputs from completed v0.31.0 tickets.
- Ensure release wording is consistent about application-owned observability, no dashboards/exporters/hosting, no automatic PIT/bridge maintenance, no ingestion orchestration, and no provider-specific SQL artifact workflow in v0.31.0.
- Keep the existing cross-release dependency to the v0.32 artifact-lane contract as a forward boundary: v0.31.0 may mention what is out of scope, but it must not implement or specify the v0.32 artifact workflow.

Scope out
- Rewriting the core decision-tree or examples from scratch; this is the final alignment and release-note pass.
- New runtime behavior, benchmark reruns, new provider-specific artifacts, or broad README restructuring.