## Goal

Create small runnable examples that demonstrate the v0.6 Code-First, registry, typed save, and typed read flow.

## Scope In

- SQLite example that runs without external infrastructure.
- PostgreSQL example with environment-variable configuration and clear skip/setup guidance.
- Minimal domain model, schema creation, save, latest read, and as-of read.

## Scope Out

- Provisioning every provider in examples.
- Embedding credentials or machine-specific paths.

## Acceptance Criteria

- Examples are buildable from the solution or documented command lines.
- No credentials or machine-specific paths are committed.
- Examples avoid future APIs and reflect the implemented v0.6 surface.