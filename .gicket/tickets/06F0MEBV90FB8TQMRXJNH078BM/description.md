## Goal

Provide typed helper APIs that map domain objects or DTOs to explicit DVault save/read operations without hiding load timestamp, record source, or Data Vault write boundaries.

## Scope In

- Typed hub/link/satellite mapper contracts.
- Explicit save helpers over the existing save-service pipeline.
- Typed latest/as-of satellite read projections over the existing read service.

## Scope Out

- Automatic SaveChanges interception.
- Full LINQ provider or broad query abstraction.

## Acceptance Criteria

- Users can save a common hub plus satellite flow without assembling raw name/value lists by hand.
- Load timestamp and record source remain visible parameters or explicit policies.
- Typed read helpers project latest/as-of satellite rows into stable DTOs.