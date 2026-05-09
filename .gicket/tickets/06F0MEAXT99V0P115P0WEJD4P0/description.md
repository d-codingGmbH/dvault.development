## Goal

Define the registry types and lookup contract that become the shared source of truth for configured Data Vault metadata.

## Scope In

- Registry shape for hubs, links, satellites, PIT, bridges, and provider capability metadata.
- Lookup methods by logical name and optional CLR type.
- Validation for duplicate names, ambiguous CLR mappings, and missing dependencies.

## Scope Out

- Service registration wiring.
- Save/read service refactoring.
- Model-first import.

## Acceptance Criteria

- Registry construction is deterministic and immutable after build.
- Existing DataVaultMetadataModel can be adapted into the registry without losing information.
- Validation output identifies the exact conflicting metadata element.