[gicket-bot] PO refinement contract

Summary
- Refined the PO contract response against the latest critic findings by removing any dependency on unevidenced current-branch model-first, PIT, bridge, parser, or projection APIs. The ticket remains a schema and validation contract: existing branch evidence is used only for current hub/link/satellite, naming, annotations, diagnostics, and release-limit baselines; all missing implementation surfaces are explicitly allowed to be created by downstream implementation tickets.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The delivery contract is restated so it does not require model-first document DTOs, parser APIs, PIT metadata APIs, bridge metadata APIs, or role-aware recursive bridge projection APIs to already exist. Current-branch source evidence is cited only for existing code-first hub/link/satellite metadata projection, ordered participants, multi-active driving keys, default naming use, provider-neutral annotation roles, and release-documented limitations. Missing surfaces are explicit implementation scope for downstream tickets, not inferred current facts.
- critic-item-2: `answered` - The persisted contract should be read as authorizing a v1 JSON artifact contract and validation taxonomy, not as claiming an existing public model-first API/type. Projection expectations apply only where visible metadata-first/code-first semantics already exist; otherwise downstream parser/projection tickets may add narrow document DTOs, validators, diagnostics, metadata adapters, or projection types required to implement the contract.
- critic-item-3: `answered` - The unsupported inferred API claim is resolved by making the sentence explicit product scope rather than source evidence: missing model-first document DTOs, parser APIs, PIT metadata APIs, bridge metadata APIs, and role-aware recursive bridge projection APIs may be created by downstream implementation tickets. The only current-branch claim about recursive links is negative: the visible code-first link surface records ordered participants and rejects repeated same-hub participants, so model-first recursive bridge support must not depend on that code-first API already supporting role-bound repeated hubs.

Clarifications
- v1 uses a required top-level schemaVersion field with literal value dvault.model.v1; missing values, unsupported major versions, or alternate dialect strings are validation errors.
- The v1 document format is JSON-first. YAML parsing or YAML-to-JSON conversion remains owned by the YAML boundary ticket and does not block this JSON contract.
- Current-branch evidence supports hub/link/satellite metadata projection, ordered participant recording, multi-active driving keys, default naming policy use, and provider-neutral annotation roles. It does not prove that model-first document DTOs, parser APIs, PIT metadata APIs, bridge metadata APIs, or role-aware recursive bridge projection APIs already exist.
- Any missing model-first, PIT, bridge, diagnostics, or projection adapter/type needed to realize this contract may be created by downstream implementation tickets as an additive implementation surface.
- The only provider-relevant v1 schema choice is loadTimestampStorage, with values for provider default, ISO 8601 UTC text, and UTC ticks based on the v0.5 release capability baseline.
- The related v0.6.0 documentation context is historical only: v0.6.0 explicitly says model-first import/export specs were not delivered.

Scope In
- Define the dvault.model.v1 JSON artifact contract for hubs, links, satellites, multi-active driving keys, PIT declarations, bridges, naming policy selection, and load timestamp storage selection.
- Define required and optional top-level fields, default values, supported token values, schemaVersion compatibility behavior, and unknown-field handling.
- Define validation rules for invalid shape, duplicate names or roles, missing references, wrong reference kinds, unsupported capability combinations, naming collisions, provider-choice errors, and ambiguous recursive participant bindings.
- Define representative valid and invalid fixture expectations for downstream parser, diagnostics, and projection tickets.
- Define mapping expectations into existing visible metadata semantics where present and explicitly permit downstream implementation to create narrow missing model-first/PIT/bridge adapters or metadata types where current-branch public API evidence is absent.

Scope Out
- Implementing the JSON parser, validator, importer, exporter, CLI integration, build integration, or runtime model mutation.
- Implementing YAML parsing or adding YAML package dependencies.
- Implementing drift tooling, code generation, read APIs, PIT row maintenance, bridge traversal helpers, provider-specific read optimizations, or provider-specific DDL behavior.
- Adding arbitrary provider-specific DDL, SQL, table naming, column naming, or migration behavior to the model-first artifact.
- Requiring existing code-first APIs to support recursive role-bound links; model-first implementation can add narrow additive surfaces instead.

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