## Goal

Define the public fluent Code-First contract before implementation so the API is small, discoverable, and compatible with the current metadata model.

## Scope In

- API shape for hub business keys, ordinary satellites, multi-active opt-in, and links.
- Namespace and extension-method placement.
- Error messages for unsupported selector shapes.
- Compatibility notes for existing metadata APIs.

## Scope Out

- Implementation of the fluent API.
- Model-first files.
- Save helper generation.

## Acceptance Criteria

- Representative user code snippets for hub, satellite, and link configuration exist as tests or a design note.
- The contract keeps load timestamp and record source out of domain entities by default.
- The design avoids promising SaveChanges interception.
- Follow-up tasks can use the contract as an implementation boundary.