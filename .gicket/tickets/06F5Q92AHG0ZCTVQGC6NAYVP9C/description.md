Goal: Generate typed projectors for stable latest/current/as-of satellite read shapes.

Acceptance criteria:
- Emits compile-time DTO/projector helpers for supported satellite metadata.
- Uses existing read services or stable direct EF projections according to the generator contract.
- Adds analyzer diagnostics for unsupported shapes and stale generated metadata.