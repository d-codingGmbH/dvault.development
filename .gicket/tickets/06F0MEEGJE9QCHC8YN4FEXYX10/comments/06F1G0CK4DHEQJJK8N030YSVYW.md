[gicket-bot] PO refinement contract

Summary
- Refined the ticket against the existing dvault.model.v1 planning contract and repository naming/layout baseline. The ticket is ready for PO-critic review with no blocking product questions.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The parser target is strict JSON for schemaVersion exactly equal to dvault.model.v1; YAML, export, CLI/build integration, and provider-specific read optimization remain out of scope.
- The authoritative field names, tokens, defaults, ordinal comparison behavior, and validation categories are the referenced dvault.model.v1 schema contract.
- The v1 naming baseline is naming.policy = default, using the repository default naming policy and ordinal string semantics for declaration-name comparisons.
- Invalid artifacts must produce deterministic structured diagnostics and must not partially apply a model to the registry or metadata source.

Scope In
- Implement JSON deserialization/import for the dvault.model.v1 artifact envelope, including defaulting optional declaration arrays and supported top-level options.
- Map valid v1 hub, link, satellite, PIT, and bridge declarations into a registry-compatible model or the narrow model-first metadata adapters needed where current public metadata APIs do not yet expose the shape.
- Implement semantic validation beyond raw JSON shape, including version rejection, unknown fields, missing references, duplicate declaration names, duplicate child names where prohibited by the contract, naming conflicts after default naming normalization, unsupported tokens, unsupported capability combinations, and invalid role/participant/parent relationships.
- Return stable structured diagnostics with deterministic severity, category/code, path or declaration location, and message content suitable for future CLI/build integration.
- Add focused tests and fixtures covering valid artifacts and the invalid cases named in the ticket description.

Scope Out
- YAML import or any YAML dependency.
- Model export, round-trip formatting, or artifact generation.
- CLI commands, build integration, file watching, or MSBuild plumbing.
- Provider-specific read optimization or provider-specific DDL/SQL behavior.
- Runtime model mutation after a failed parse or validation result.
- Broad governance policy beyond enforcing the v1 schema contract.

Open questions
- none

Follow-up questions
- Should a later CLI/build ticket standardize exact diagnostic code names and command output formatting across parser, projection, and governance validators?
- Should later model-first work expose public APIs for PIT, bridge, and role-bearing recursive link metadata instead of keeping narrow internal adapters?
- Should future schema versions relax strict v1 compatibility with feature negotiation or minor-version compatibility rules?

Risks
- The v1 contract includes PIT, bridge, and role-bearing shapes that may exceed the current public metadata API, so implementation may need narrow internal model-first representations before projection tickets consume them.
- Diagnostic stability can drift if tests assert only message text loosely; tests should pin code/category/path ordering enough for future CLI/build integration.
- Naming-conflict validation depends on the repository default naming policy, so tests should include normalized-name collisions rather than only exact duplicate strings.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment