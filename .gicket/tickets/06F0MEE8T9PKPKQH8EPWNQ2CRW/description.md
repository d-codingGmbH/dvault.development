<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the PO contract response against the latest critic findings by removing any dependency on unevidenced current-branch model-first, PIT, bridge, parser, or projection APIs. The ticket remains a schema and validation contract: existing branch evidence is used only for current hub/link/satellite, naming, annotations, diagnostics, and release-limit baselines; all missing implementation surfaces are explicitly allowed to be created by downstream implementation tickets.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- v1 uses a required top-level schemaVersion field with literal value dvault.model.v1; missing values, unsupported major versions, or alternate dialect strings are validation errors.
- The v1 document format is JSON-first. YAML parsing or YAML-to-JSON conversion remains owned by the YAML boundary ticket and does not block this JSON contract.
- Current-branch evidence supports hub/link/satellite metadata projection, ordered participant recording, multi-active driving keys, default naming policy use, and provider-neutral annotation roles. It does not prove that model-first document DTOs, parser APIs, PIT metadata APIs, bridge metadata APIs, or role-aware recursive bridge projection APIs already exist.
- Any missing model-first, PIT, bridge, diagnostics, or projection adapter/type needed to realize this contract may be created by downstream implementation tickets as an additive implementation surface.
- The only provider-relevant v1 schema choice is loadTimestampStorage, with values for provider default, ISO 8601 UTC text, and UTC ticks based on the v0.5 release capability baseline.
- The related v0.6.0 documentation context is historical only: v0.6.0 explicitly says model-first import/export specs were not delivered.

### Scope In
- Define the dvault.model.v1 JSON artifact contract for hubs, links, satellites, multi-active driving keys, PIT declarations, bridges, naming policy selection, and load timestamp storage selection.
- Define required and optional top-level fields, default values, supported token values, schemaVersion compatibility behavior, and unknown-field handling.
- Define validation rules for invalid shape, duplicate names or roles, missing references, wrong reference kinds, unsupported capability combinations, naming collisions, provider-choice errors, and ambiguous recursive participant bindings.
- Define representative valid and invalid fixture expectations for downstream parser, diagnostics, and projection tickets.
- Define mapping expectations into existing visible metadata semantics where present and explicitly permit downstream implementation to create narrow missing model-first/PIT/bridge adapters or metadata types where current-branch public API evidence is absent.

### Scope Out
- Implementing the JSON parser, validator, importer, exporter, CLI integration, build integration, or runtime model mutation.
- Implementing YAML parsing or adding YAML package dependencies.
- Implementing drift tooling, code generation, read APIs, PIT row maintenance, bridge traversal helpers, provider-specific read optimizations, or provider-specific DDL behavior.
- Adding arbitrary provider-specific DDL, SQL, table naming, column naming, or migration behavior to the model-first artifact.
- Requiring existing code-first APIs to support recursive role-bound links; model-first implementation can add narrow additive surfaces instead.

## Acceptance Criteria
- A v1 schema contract is documented or encoded clearly enough for downstream parser, diagnostics, and projection work to proceed without reopening top-level field names, token names, or compatibility policy.
- Valid examples cover at minimum a customer hub with ordered business keys, a hub-parent satellite, a link with ordered participants, a multi-active satellite with ordered driving keys, a PIT declaration over hub satellites, a many-to-many bridge, a hierarchy bridge with role-bound recursive participants, default naming, and each supported loadTimestampStorage value.
- Invalid examples cover at minimum missing or unsupported schemaVersion, duplicate declaration names or roles, missing references, wrong reference kinds, ambiguous link participants, repeated-hub link participants without roles where needed, satellite driving-key and payload overlap, PIT satellite parent mismatch, invalid bridge endpoints, naming collisions after default normalization, unknown fields, and unsupported provider-specific fields.
- Diagnostics are structured with severity, stable category/code, message, and JSON Pointer-style path where feasible; invalid documents return diagnostics without partial model application.
- The contract avoids provider-specific leakage except the explicit loadTimestampStorage capability choice and maps accepted documents into registry-compatible metadata semantics only where those semantics are visible, while permitting additive missing model-first/PIT/bridge projection metadata or adapters where current-branch public APIs are insufficient.

## Definition of Done
- The v1 artifact contract identifies required and optional top-level fields, default values, supported token values, and schemaVersion compatibility behavior.
- The validation taxonomy is explicit enough for downstream tests to assert stable categories for schema/version, shape, reference, duplicate, naming, capability, provider-choice, and recursive participant binding failures.
- Representative fixture names and scenarios are available to parser/projection implementers, either in tests/fixtures or in a durable planning/spec document created by the implementation work.
- Downstream implementation can project valid model-first documents into existing metadata semantics where current-branch evidence shows those semantics exist, and can add narrow missing model-first/PIT/bridge metadata adapters where visible current-branch public APIs are insufficient.
- No workflow-only metadata transition is required as product scope; runtime handoff labels and statuses remain outside the delivery definition.

## Implementation Notes
- Use top-level arrays named hubs, links, satellites, pits, and bridges. Each declaration carries a stable logical name; comparisons and duplicate checks should use ordinal string semantics to match visible metadata validation patterns.
- Hub declarations require at least one ordered businessKeys entry. Link declarations require at least two ordered participants; participant objects should contain hub and may contain role so recursive or repeated-hub relationships can be disambiguated for diagnostics and hierarchy bridge endpoint binding.
- Do not infer that the existing code-first link API can represent recursive role-bound links: visible source records ordered participants only and rejects repeated same-hub participants. Model-first implementation may add a narrow role-aware metadata adapter or type as needed.
- Satellite declarations should use parent references with kind and name, ordered payload names, and optional ordered drivingKeys. A non-empty drivingKeys list is the multi-active opt-in and must not overlap payload names.
- Bridge declarations should align with the bridge planning contract: kind is many-to-many or hierarchy, source is one link, many-to-many endpoints are from and to, hierarchy endpoints are ancestor and descendant, and hierarchy bridges traverse a two-participant self-link over one hub with explicit role bindings.
- PIT declarations should reference one hub and one or more satellites that belong to that hub; duplicate satellite references and parent mismatches are validation errors.
- Naming should default to the existing default naming policy. Produced table/entity/index/column collisions after normalization should be diagnosed rather than left to provider-specific behavior.
- Existing related implementation tickets remain the consumers of this contract: parser/diagnostics 06F0MEEGJE9QCHC8YN4FEXYX10, YAML boundary 06F0MEERJ7D5Q4WYBQAJD3GFVC, projection 06F0MEF08AJ1K52STF42T74B04, and governance docs 06F0MEGAGJCEHQ8QRHGH8W7804.

## Open Questions
- none

## Follow-Up Questions
- The YAML boundary ticket should decide whether YAML is parsed directly or supported only through documented JSON conversion; this does not block the JSON-first schema contract.
- A future export/drift ticket can decide whether dvault.model.v1 needs canonical formatting or round-trip preservation rules beyond parser/projection requirements.
- Future versions may add richer naming overrides, provider-specific capability sections, model governance metadata, or public role-aware code-first link APIs; those should be versioned additions rather than implicit v1 behavior.

## Risks
- If downstream implementation silently ignores unknown fields, misspelled model-first documents could drift from intended metadata; v1 should prefer explicit diagnostics.
- Recursive link and hierarchy bridge support will fail if participant order, role values, and endpoint bindings are not preserved through schema validation and projection; current visible code-first link APIs do not provide a role-bearing repeated-hub surface, so model-first implementation must add or use a narrow adapter where required.
- Over-broad provider sections would undermine the provider-neutral model-first contract and should remain out of v1 except for the load timestamp storage choice.

## Split Recommendations
- No new split is recommended. Existing downstream tickets already cover parser/diagnostics, YAML boundary, projection, and governance documentation; this ticket should remain the schema and validation contract source for those tickets.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Design the model-first artifact contract before implementation so import/export and drift tooling share the same vocabulary.

## Scope In

- Schema version field and compatibility policy.
- Hub, link, satellite, multi-active, PIT, bridge, naming, and timestamp-storage representation.
- Validation rules and diagnostics taxonomy.

## Scope Out

- Parser implementation.
- Export or drift tooling implementation.

## Acceptance Criteria

- Representative valid and invalid model documents are captured as tests or fixtures.
- The schema avoids provider-specific leakage except where explicit provider capability choices are required.
- The contract maps cleanly to the existing registry/metadata model.