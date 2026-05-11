[gicket-bot] PO refinement contract

Summary
- Refined the model-first schema ticket using the provided ticket snapshot, related-ticket reads, release notes, bridge and multi-active planning contracts, and current metadata/diagnostics source evidence. No child tickets, relation changes, planning documents, or attachments were created in this pass; existing downstream parser, YAML, projection, and governance tickets remain the implementation consumers of this contract.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- v1 uses a required top-level schemaVersion field with the literal value dvault.model.v1; missing values, unsupported major versions, or alternate dialect strings are validation errors.
- The v1 document format is JSON-first. YAML is not decided by this ticket; the existing YAML boundary ticket owns whether YAML is parsed directly or converted into the same JSON contract.
- The v1 default naming baseline is the existing DefaultNamingPolicy behavior: logical declaration names are provider-neutral, produced names remain deterministic, and arbitrary provider-specific table or column overrides are out of scope.
- The only provider-relevant choice in the v1 schema baseline is loadTimestampStorage, with allowed values mapping to the existing provider-default, ISO 8601 UTC text, and UTC ticks capability options.
- The related v0.6.0 documentation ticket is done and is historical context only; v0.6.0 explicitly left model-first import/export specs for future work.

Scope In
- Define the dvault.model.v1 JSON artifact contract for hubs, links, satellites, multi-active driving keys, PIT declarations, bridges, naming policy selection, and load timestamp storage selection.
- Define compatibility rules for schemaVersion and additive v1 evolution without silently accepting unknown or misspelled fields.
- Define validation rules that map invalid document shape, duplicate names, missing references, unsupported capability combinations, naming collisions, and provider choice errors into structured diagnostics.
- Define representative valid and invalid fixture expectations for downstream parser and projection tickets.
- Ensure the schema maps into the existing DataVaultMetadataModel, DataVaultMetadataRegistry, bridge metadata, PIT metadata, multi-active satellite, provider capability, naming, and diagnostics surfaces.

Scope Out
- Implementing the JSON parser, validator, importer, or CLI/build integration.
- Implementing YAML parsing or adding YAML package dependencies.
- Implementing export, drift tooling, runtime model mutation, code generation, read APIs, PIT row maintenance, bridge traversal helpers, or provider-specific read optimizations.
- Adding arbitrary provider-specific DDL, SQL, table naming, column naming, or migration behavior to the model-first artifact.
- Changing v0.6 Code-First or metadata-first public APIs except where a later implementation ticket needs a narrow adapter for imported model projection.

Open questions
- none

Follow-up questions
- The YAML boundary ticket should decide whether YAML is parsed directly or supported only through documented JSON conversion, but that does not block this JSON-first schema contract.
- A future export/drift ticket can decide whether dvault.model.v1 needs canonical formatting or round-trip preservation rules beyond parser/projection requirements.
- Future versions may add richer naming overrides, provider-specific capability sections, or model governance metadata, but those should be versioned additions rather than implicit v1 behavior.

Risks
- If downstream implementation silently ignores unknown fields, misspelled model-first documents could drift from intended metadata; v1 should prefer explicit diagnostics.
- Recursive link and hierarchy bridge support depends on preserving participant order and role/endpoint binding through import diagnostics, even where existing public metadata APIs are ordinal-oriented.
- Over-broad provider sections would undermine the provider-neutral model-first contract and should remain out of v1 except for the existing load timestamp storage choice.

Split recommendations
- No new split is recommended. Existing downstream tickets already cover parser/diagnostics, YAML boundary, projection, and governance documentation; this ticket should remain the schema and validation contract source for those tickets.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment