## Goal

Connect imported model-first artifacts to the registry and EF metadata projection pipeline used by v0.6 Code-First models.

## Scope In

- Conversion from imported model document to registry entries.
- EF metadata projection through existing provider capability profiles.
- Tests comparing imported model, Code-First model, and metadata-first model equivalence.

## Scope Out

- Export tooling.
- Read service implementation.

## Acceptance Criteria

- Imported models can drive schema projection without duplicate manual metadata declarations.
- Existing provider-aware timestamp/index behavior still applies.
- Projection failures identify the source model path that caused the issue.