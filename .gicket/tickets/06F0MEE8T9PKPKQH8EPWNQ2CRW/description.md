## Goal

Design the model-first artifact contract before implementation so import/export and drift tooling share the same vocabulary.

## Scope In

- Schema version field and compatibility policy.
- Hub, link, satellite, multi-active, PIT, bridge, naming, and timestamp-storage representation.
- Validation rules and diagnostics taxonomy.

## Scope Out

- Parser implementation.
- Export or drift tooling implementation.

## Acceptance Criteria

- Representative valid and invalid model documents are captured as tests or fixtures.
- The schema avoids provider-specific leakage except where explicit provider capability choices are required.
- The contract maps cleanly to the existing registry/metadata model.