## Goal

Define mapper interfaces and conventions that convert domain objects or DTOs into Data Vault hub, link, and satellite operation inputs.

## Scope In

- Mapper contract shape and default convention behavior.
- Explicit handling for business keys, parent hash keys, payload values, driving keys, hash diff, load timestamp, and record source.
- Validation for unsupported or ambiguous mappings.

## Scope Out

- Helper implementation.
- Source generation.
- SaveChanges interception.

## Acceptance Criteria

- Contracts are small enough for users to implement manually.
- Future source-generation or model-first generation can reuse the same contract.
- Tests cover nulls, missing keys, duplicate payload names, and multi-active boundaries.