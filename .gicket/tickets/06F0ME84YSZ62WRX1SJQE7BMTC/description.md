## Goal

Make ordinary DVault usage feel natural in a .NET/EF Core application by adding a fluent Code-First modeling surface, a reusable model registry, typed explicit save/read helpers, diagnostics, and examples.

## Scope In

- Fluent EF Code-First API for hubs, links, satellites, and ordinary satellite read/write workflows.
- Central registry for schema projection, save services, read services, examples, and diagnostics.
- Typed explicit save/read helpers that keep load timestamp and record source visible.
- Validation/explain output and starter examples.

## Scope Out

- SaveChanges interception or hidden writes.
- Model-first JSON/YAML specifications; those are planned for v0.7.0.
- Full PIT/bridge runtime read models beyond compatibility with current metadata.

## Acceptance Criteria

- A small domain model can be configured with a concise fluent API and projected to existing schema conventions.
- Users do not need to recreate equivalent metadata objects in schema, save, and read code for the happy path.
- Typed helpers preserve explicit Data Vault write boundaries.
- Existing v0.5 APIs remain source-compatible.