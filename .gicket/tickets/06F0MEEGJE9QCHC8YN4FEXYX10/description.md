<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket against the existing dvault.model.v1 planning contract and repository naming/layout baseline. The ticket is ready for PO-critic review with no blocking product questions.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The parser target is strict JSON for schemaVersion exactly equal to dvault.model.v1; YAML, export, CLI/build integration, and provider-specific read optimization remain out of scope.
- The authoritative field names, tokens, defaults, ordinal comparison behavior, and validation categories are the referenced dvault.model.v1 schema contract.
- The v1 naming baseline is naming.policy = default, using the repository default naming policy and ordinal string semantics for declaration-name comparisons.
- Invalid artifacts must produce deterministic structured diagnostics and must not partially apply a model to the registry or metadata source.

### Scope In
- Implement JSON deserialization/import for the dvault.model.v1 artifact envelope, including defaulting optional declaration arrays and supported top-level options.
- Map valid v1 hub, link, satellite, PIT, and bridge declarations into a registry-compatible model or the narrow model-first metadata adapters needed where current public metadata APIs do not yet expose the shape.
- Implement semantic validation beyond raw JSON shape, including version rejection, unknown fields, missing references, duplicate declaration names, duplicate child names where prohibited by the contract, naming conflicts after default naming normalization, unsupported tokens, unsupported capability combinations, and invalid role/participant/parent relationships.
- Return stable structured diagnostics with deterministic severity, category/code, path or declaration location, and message content suitable for future CLI/build integration.
- Add focused tests and fixtures covering valid artifacts and the invalid cases named in the ticket description.

### Scope Out
- YAML import or any YAML dependency.
- Model export, round-trip formatting, or artifact generation.
- CLI commands, build integration, file watching, or MSBuild plumbing.
- Provider-specific read optimization or provider-specific DDL/SQL behavior.
- Runtime model mutation after a failed parse or validation result.
- Broad governance policy beyond enforcing the v1 schema contract.

## Acceptance Criteria
- A valid dvault.model.v1 JSON artifact with omitted optional arrays/options is accepted using documented defaults and produces a registry-compatible model equivalent to the declared hubs, links, satellites, PITs, and bridges that are representable in the current metadata layer.
- Artifacts with missing schemaVersion, non-string schemaVersion, or any schemaVersion other than dvault.model.v1 are rejected with deterministic structured diagnostics.
- Unknown fields at any object level are rejected with deterministic diagnostics that identify the offending path or declaration location.
- References from links, satellites, PITs, and bridges are validated against declared model names and invalid or missing references are rejected without applying a partial model.
- Duplicate names and naming conflicts are validated using ordinal string semantics and the repository default naming policy baseline from the v1 contract.
- Unsupported token values and unsupported capability combinations, including invalid loadTimestampStorage, naming.policy, satellite parent kind, bridge kind, repeated same-hub participants without distinct roles, invalid multi-active driving-key shapes, and invalid PIT/bridge parent/member combinations, are rejected with stable diagnostics.
- Parser and validation tests cover at least unknown version, missing references, duplicate names, unsupported capability combinations, and naming conflicts, plus at least one representative valid full artifact.

## Definition of Done
- The implementation is covered by deterministic unit tests for valid and invalid JSON artifacts, including assertion of diagnostic structure rather than only free-form exception text.
- Invalid input returns diagnostics through the intended parser result surface and does not mutate or register a partial metadata model.
- The parser behavior follows the referenced dvault.model.v1 contract for required fields, defaults, supported tokens, unknown-field rejection, and strict version compatibility.
- The solution builds and the relevant DVault test project passes in the ticket branch.
- Any model-first adapter added for contract shapes not exposed by the current public metadata API is narrow, internal where possible, and documented in code/tests through behavior-focused names.

## Implementation Notes
- Use the existing source layout under src/DCoding.Data.DVault and tests under tests/DCoding.Data.DVault.Tests for the parser, diagnostics, and fixtures unless the repository already has a more specific validation/modeling folder.
- Prefer System.Text.Json with explicit options and extension-data handling or equivalent structured parsing so unknown fields can be reported deterministically instead of ignored.
- Keep diagnostics as data-first results rather than thrown exceptions for validation failures; reserve exceptions for programmer misuse or unrecoverable runtime faults.
- Use ordinal string comparison for model declaration names and token validation, matching the schema contract.
- Preserve declaration order in the produced model and in diagnostic ordering so future CLI/build consumers receive stable output.
- Use existing DefaultNamingPolicy/DataVault metadata types where they fit; add only narrow model-first metadata/adapters for PIT, bridge, or role-bearing recursive link shapes that current public APIs cannot represent.

## Open Questions
- none

## Follow-Up Questions
- Should a later CLI/build ticket standardize exact diagnostic code names and command output formatting across parser, projection, and governance validators?
- Should later model-first work expose public APIs for PIT, bridge, and role-bearing recursive link metadata instead of keeping narrow internal adapters?
- Should future schema versions relax strict v1 compatibility with feature negotiation or minor-version compatibility rules?

## Risks
- The v1 contract includes PIT, bridge, and role-bearing shapes that may exceed the current public metadata API, so implementation may need narrow internal model-first representations before projection tickets consume them.
- Diagnostic stability can drift if tests assert only message text loosely; tests should pin code/category/path ordering enough for future CLI/build integration.
- Naming-conflict validation depends on the repository default naming policy, so tests should include normalized-name collisions rather than only exact duplicate strings.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Implement the JSON import path for the versioned model-first artifact and produce deterministic diagnostics for invalid documents.

## Scope In

- Parser for the v1 model-first JSON schema.
- Semantic validation beyond raw JSON shape.
- Tests for unknown version, missing references, duplicate names, unsupported capability combinations, and naming conflicts.

## Scope Out

- YAML support.
- Export tooling.
- Provider-specific read optimization.

## Acceptance Criteria

- Valid JSON artifacts produce a registry-compatible model.
- Invalid artifacts return structured diagnostics without partial model application.
- Parser errors are stable enough for CLI/build integration later.