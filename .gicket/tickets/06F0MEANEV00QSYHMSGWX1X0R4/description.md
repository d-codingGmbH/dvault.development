## Goal

Provide a reusable model registry so schema projection, save services, read services, diagnostics, and examples can use one authoritative Data Vault model rather than repeated metadata construction.

## Scope In

- Immutable registry API for hubs, links, satellites, PIT, bridges, and future model-first imports.
- Service registration through AddDVault and DbContext/model integration.
- Lookup by logical name and by CLR type where available.

## Scope Out

- Model-first file import/export.
- Runtime mutation of the registry after service-provider build.

## Acceptance Criteria

- Users can register or obtain a Data Vault model once and reuse it in save/read workflows.
- The registry can represent both existing metadata-first declarations and new Code-First declarations.
- Ambiguous or missing lookups fail with actionable diagnostics.