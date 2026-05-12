<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the model-first import story against the existing schema contract, repository evidence, and completed child-ticket evidence. The story is ready for PO-critic review with no blocking product questions and no new ticket, relation, attachment, or planning-document writes materialized in this run.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 artifact baseline is JSON-first with required schemaVersion exactly dvault.model.v1, strict unknown-field rejection, default naming.policy = default, optional declaration arrays defaulting to empty, and ordinal string comparison semantics.
- Already-created child work covers the split: schema contract 06F0MEE8T9PKPKQH8EPWNQ2CRW is done, parser/diagnostics 06F0MEEGJE9QCHC8YN4FEXYX10 is done, YAML boundary 06F0MEERJ7D5Q4WYBQAJD3GFVC is done, and import/projection 06F0MEF08AJ1K52STF42T74B04 is done.
- Governance documentation ticket 06F0MEGAGJCEHQ8QRHGH8W7804 remains a separate todo consumer and does not reopen the import story scope.
- The v1 YAML decision is external pre-conversion only; DVault v1 should not add direct YAML parsing, YAML-only semantics, or a core YAML dependency.
- Projection should reuse the existing registry and EF metadata pipeline rather than create a parallel model-first projection stack.

### Scope In
- Story-level delivery of dvault.model.v1 import across schema contract, strict JSON parser and validation diagnostics, YAML boundary decision, and import-to-registry/import-to-EF projection.
- Model-first declarations for hubs, links, hub-parent and link-parent satellites, multi-active driving keys, PIT tables, bridges, naming policy, and load timestamp storage choices as defined by the v1 contract.
- Structured diagnostics for version, shape, unknown field, reference, duplicate, naming collision, provider-choice, capability, and recursive participant binding failures.
- Parity between imported model artifacts and metadata-first or Code-First semantics where those repository surfaces currently overlap.

### Scope Out
- Runtime model mutation after import.
- Code generation beyond import/projection.
- Direct YAML ingestion or YAML-specific schema semantics.
- Export tooling, drift reporting, CLI/build integration, and governance docs beyond linking to their existing tickets.
- Provider-specific DDL, SQL, migrations, or read optimizations outside the existing provider capability profile mechanism.
- Expanding the public Code-First API to cover link-parent satellites, PITs, bridges, or role-bearing recursive links.

## Acceptance Criteria
- The v1 schema contract remains the authoritative source for top-level fields, token names, defaults, compatibility behavior, validation categories, and representative fixture expectations.
- Valid dvault.model.v1 JSON artifacts are accepted with documented defaults and can produce a usable metadata model or registry for existing DVault registration/projection flows.
- Invalid artifacts fail with deterministic structured diagnostics that include severity, stable category/code, message, and JSON Pointer or declaration path where feasible, without partial model application.
- YAML authoring is documented as an external conversion path into canonical JSON, with no direct DVault YAML parser dependency in v1.
- Imported-model projection preserves provider-aware timestamp/index behavior for provider-default, iso-8601-utc-text, and utc-ticks loadTimestampStorage choices.
- Imported projection matches Code-First and metadata-first behavior for the shared surface, and uses metadata-first or narrow model-first adapters for advanced shapes outside current Code-First coverage.

## Definition of Done
- Existing child tickets for schema contract, parser/diagnostics, YAML boundary, and projection have delivery contracts satisfied or linked as the implementing work for this story.
- Relevant parser/projection tests cover representative valid full artifacts and invalid version, reference, duplicate, naming collision, unknown field, provider-choice, PIT, bridge, and recursive-role scenarios.
- Import results can drive DataVaultMetadataModel/DataVaultMetadataRegistry and EF metadata projection through the established DVault path without duplicate manual declarations.
- Failure diagnostics remain source-oriented through parser, registry build, and EF projection stages.
- No workflow-only status or label transition is required as product scope; runtime orchestration owns handoff metadata.

## Implementation Notes
- Use docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md as the durable v1 contract.
- Route successful imports through the existing DataVaultMetadataRegistry, AddDVault, UseDataVaultMetadata, and ApplyDataVaultMetadata patterns.
- Use DefaultNamingPolicy and ordinal comparisons for duplicate detection and produced-name collision diagnostics.
- Keep role-bearing recursive link and hierarchy bridge support narrow and additive where current public metadata does not retain participant roles.
- Preserve declaration order in imported metadata and diagnostic ordering so later CLI/build consumers receive stable results.
- No new child tickets, relation updates, attachments, or planning documents were created during this PO refinement run.

## Open Questions
- none

## Follow-Up Questions
- Governance ticket 06F0MEGAGJCEHQ8QRHGH8W7804 should document the recommended choice between model-first, metadata-first, and Code-First flows after the import surface is ready for users.
- Future export and drift tickets should consider consuming the same import result surface so artifact normalization and comparison behavior stay centralized.
- A later release can revisit optional YAML tooling or richer naming/provider extension sections as versioned additions, not implicit v1 behavior.

## Risks
- If unknown fields are ignored, misspelled artifacts can silently drift from intended metadata.
- If loadTimestampStorage is not propagated into provider capability profiles, imported projection can diverge from metadata-first and Code-First behavior.
- If post-parse failures collapse to generic metadata exceptions, users will lose the source-path diagnostics promised by the story.
- Recursive-role and hierarchy bridge cases remain sensitive because current public link metadata may not carry enough role information without a narrow model-first adapter.

## Split Recommendations
- No new split is recommended. The existing child-ticket set already covers schema, parser/diagnostics, YAML boundary, import/projection, and downstream governance documentation.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Define and implement a versioned model-first specification that can describe DVault hubs, links, satellites, PIT tables, bridges, naming options, and provider-relevant choices outside C# code.

## Scope In

- dvault.model schema versioning and validation rules.
- JSON parser and validation diagnostics.
- YAML ingestion decision and implementation boundary.
- Projection into registry and EF metadata.

## Scope Out

- Runtime model mutation.
- Code generation beyond import/projection.

## Acceptance Criteria

- Invalid model artifacts fail with line/path-oriented diagnostics where feasible.
- Imported models produce the same registry/metadata semantics as Code-First for covered scenarios.
- The format is stable enough to document and version.