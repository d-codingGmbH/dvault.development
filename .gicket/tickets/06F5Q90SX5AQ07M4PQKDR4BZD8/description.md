Goal: Add PIT support where the PIT parent is a link rather than a hub.

Acceptance criteria:
- Projects deterministic link-parent PIT columns and validates referenced link-parent satellites.
- Supports rebuild and targeted parent maintenance using link hash keys.
- Extends read diagnostics and read APIs without breaking hub-parent PIT behavior.