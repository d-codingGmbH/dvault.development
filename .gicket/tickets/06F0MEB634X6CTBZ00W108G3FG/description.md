## Goal

Wire the registry into ordinary application startup and EF model configuration without forcing users into a service-location pattern.

## Scope In

- AddDVault options for registry/model registration.
- DbContext/model annotation integration for configured metadata.
- Clear precedence rules when explicit metadata and registry-backed metadata are both present.

## Scope Out

- Model-first import.
- Typed save/read helpers.
- Provider-specific SQL changes.

## Acceptance Criteria

- A typical app can register the Data Vault model once during service setup.
- EF model projection can consume the registered model without duplicating declarations.
- Conflicting model sources produce a validation error rather than silent divergence.