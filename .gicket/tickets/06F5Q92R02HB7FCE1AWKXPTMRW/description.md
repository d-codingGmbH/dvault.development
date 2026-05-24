Goal: Generate typed projectors for stable PIT and bridge read shapes.

Acceptance criteria:
- Emits compile-time DTO/projector helpers for supported PIT and bridge metadata.
- Handles endpoint roles, traversal depth, PIT segment values, and generated column names deterministically.
- Adds analyzer diagnostics for unsupported PIT/bridge shapes.