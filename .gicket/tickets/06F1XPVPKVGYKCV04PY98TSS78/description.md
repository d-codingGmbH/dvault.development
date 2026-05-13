## Goal

Make DVault validation visible during EF Core design-time workflows instead of surprising users only at runtime.

## Scope In

- Provide design-time service registration guidance or implementation for dotnet ef.
- Surface DVault metadata validation and migration guardrail summaries where EF Core allows it.
- Document supported project layouts and limitations.
- Keep provider-neutral behavior in the core package.

## Scope Out

- No custom dotnet-ef fork.
- No IDE extension.
- No provider-specific online migration runner.

## Acceptance Criteria

- A sample project can run the documented design-time path.
- Validation failures show stable diagnostic ids.
- Docs explain how this complements normal EF migrations.

## Implementation Notes

- Favor EF Core design-time extension points over command wrapping.

## Open Questions

- none