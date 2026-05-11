[gicket-bot] PO refinement contract

Summary
- Refined the ticket contract to remove unsupported assumptions about existing PIT/bridge/model-first public APIs and to make missing import/projection metadata surfaces explicit implementation allowance rather than inferred current-branch fact. The contract now treats current source evidence as supporting hub/link/satellite, ordered link participants, multi-active driving keys, naming, diagnostics annotations, provider-logical timestamp storage, and bridge-related annotation roles, while PIT/bridge/model-first schema types may be created by downstream implementation tickets as needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract is restated so it no longer requires all referenced public metadata API/types to already exist. Current-branch evidence supports existing code-first hub/link/satellite and multi-active driving-key declarations plus provider-neutral annotation roles, but the model-first schema, parser/validator/importer, and any missing PIT/bridge projection metadata adapters may be created by downstream implementation work when not already present in visible source. The schema contract must define names and validation behavior without claiming those missing APIs already exist.
- critic-item-2: `answered` - The persisted delivery contract is corrected to avoid inferring an existing model-first or bridge/PIT public API/type from the branch snapshot. The delivery target is a versioned JSON contract and validation taxonomy. Downstream parser/projection tickets may add missing document DTOs, validators, diagnostics, metadata adapters, or projection types needed to realize the contract, while preserving compatibility with the visible metadata-first/code-first semantics where those surfaces already exist.
- critic-item-3: `answered` - The recursive link and hierarchy bridge requirement is restated as a schema/import validation requirement, not as a claim about an already-existing role-aware public metadata API. For dvault.model.v1, link participants remain ordered and participant objects may include role values so recursive or repeated-hub links can be validated and bound to hierarchy endpoints. If the existing projection surface only supports ordinal participants or rejects repeated hubs, the downstream implementation must add a narrow model-first adapter or metadata representation for role/endpoint binding rather than silently projecting ambiguous recursive links.

Clarifications
- v1 uses a required top-level schemaVersion field with literal value dvault.model.v1; missing values, unsupported major versions, or alternate dialect strings are validation errors.
- The v1 document format is JSON-first. YAML parsing or YAML-to-JSON conversion remains owned by the YAML boundary ticket and does not block this JSON contract.
- The v1 default naming baseline is the visible DefaultNamingPolicy usage in code-first metadata projection: logical declaration names are provider-neutral, produced names are deterministic, and arbitrary provider-specific table or column overrides are out of scope.
- The only provider-relevant v1 schema choice is loadTimestampStorage, with values for provider default, ISO 8601 UTC text, and UTC ticks based on the v0.5 release capability baseline.
- This ticket defines a durable schema and validation contract; it does not assert that model-first document DTOs, parser APIs, PIT metadata APIs, or role-aware recursive bridge projection APIs already exist in the current branch. Missing surfaces may be created by downstream implementation tickets within this contract.
- The related v0.6.0 documentation context is historical only: v0.6.0 explicitly says model-first import/export specs were not delivered.

Scope In
- Define the dvault.model.v1 JSON artifact contract for hubs, links, satellites, multi-active driving keys, PIT declarations, bridges, naming policy selection, and load timestamp storage selection.
- Define required and optional top-level fields, default values, supported token values, schemaVersion compatibility behavior, and unknown-field handling.
- Define validation rules for invalid shape, duplicate names or roles, missing references, wrong reference kinds, unsupported capability combinations, naming collisions, provider-choice errors, and ambiguous recursive participant bindings.
- Define representative valid and invalid fixture expectations for downstream parser, diagnostics, and projection tickets.
- Define mapping expectations into existing visible metadata semantics where present and allow downstream implementation to create narrow missing model-first/PIT/bridge adapters or metadata types where current-branch public API evidence is absent.

Scope Out
- Implementing the JSON parser, validator, importer, exporter, CLI integration, build integration, or runtime model mutation.
- Implementing YAML parsing or adding YAML package dependencies.
- Implementing drift tooling, code generation, read APIs, PIT row maintenance, bridge traversal helpers, provider-specific read optimizations, or provider-specific DDL behavior.
- Adding arbitrary provider-specific DDL, SQL, table naming, column naming, or migration behavior to the model-first artifact.
- Changing existing metadata-first or code-first public APIs except where a later implementation ticket needs a narrow adapter or additive type to project imported model-first documents.

Open questions
- none

Follow-up questions
- The YAML boundary ticket should decide whether YAML is parsed directly or supported only through documented JSON conversion; this does not block the JSON-first schema contract.
- A future export/drift ticket can decide whether dvault.model.v1 needs canonical formatting or round-trip preservation rules beyond parser/projection requirements.
- Future versions may add richer naming overrides, provider-specific capability sections, model governance metadata, or public role-aware code-first link APIs; those should be versioned additions rather than implicit v1 behavior.

Risks
- If downstream implementation silently ignores unknown fields, misspelled model-first documents could drift from intended metadata; v1 should prefer explicit diagnostics.
- Recursive link and hierarchy bridge support will fail if participant order, role values, and endpoint bindings are not preserved through schema validation and projection; current visible code-first link APIs do not provide a role-bearing repeated-hub surface, so model-first implementation must add or use a narrow adapter where required.
- Over-broad provider sections would undermine the provider-neutral model-first contract and should remain out of v1 except for the load timestamp storage choice.

Split recommendations
- No new split is recommended. Existing downstream tickets already cover parser/diagnostics, YAML boundary, projection, and governance documentation; this ticket should remain the schema and validation contract source for those tickets.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 8

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment