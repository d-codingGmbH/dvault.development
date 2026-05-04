Goal: add baseline point-in-time table modeling and EF model generation for Data Vault read optimization scenarios.

Scope:
- Define PIT metadata that references a hub and one or more satellites.
- Generate provider-neutral EF model metadata for the PIT structure.
- Document supported PIT semantics, limitations, and example usage.

Acceptance Criteria:
- PIT modeling is explicit and does not alter existing hub/satellite persistence unless configured.
- Tests verify generated names, keys, relationships, and basic queryability for the provider-neutral baseline.
- Documentation shows a minimal PIT example and states which automation is deferred to future tickets.