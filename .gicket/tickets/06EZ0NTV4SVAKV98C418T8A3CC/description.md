Goal: add baseline bridge table modeling and EF generation for relationship traversal scenarios.

Scope:
- Define bridge metadata for many-to-many and hierarchy-style traversal over existing hubs and links.
- Generate provider-neutral EF metadata for bridge structures.
- Document supported traversal shapes and explicit limitations.

Acceptance Criteria:
- Bridge modeling is opt-in and does not change existing link behavior by default.
- Tests cover deterministic naming, keys, link references, and validation failures.
- Documentation includes a minimal bridge example and explains unsupported advanced traversal patterns.