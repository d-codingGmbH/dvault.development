Add one realistic but compact EF Core example scenario within the existing examples boundary.

Required repository output
- Update an existing quickstart/example under `examples/` or add one small example project if reuse would make the scenario unclear.
- Update `examples/README.md` so adopters can find the scenario and understand what it demonstrates.
- This ticket must produce example or documentation changes outside `.gicket`.

Scope in
- Use a fixed, compact domain such as customer/order activity or account activity; do not introduce multiple unrelated domains.
- Demonstrate ordinary EF Core usage with DVault metadata, explicit saves, typed or provider-neutral reads where already supported, diagnostics checks, and the guardrail/non-goal boundaries from v0.31.0 guidance.
- Prefer SQLite or an existing quickstart-friendly provider path unless the current examples already provide the provider setup needed without external infrastructure.
- Keep the example runnable with the repository's normal build/test expectations and avoid generated build artifacts in source control.

Scope out
- A sample platform, web app, hosted worker, dashboard, ingestion scheduler, external service dependency, container orchestration, or broad tutorial rewrite.
- Inventing new DVault APIs or changing library behavior to fit the example.