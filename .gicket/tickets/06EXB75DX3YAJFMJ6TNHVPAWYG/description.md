<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Re-refined the naming story against the current ticket, comments, relations, docs, and source evidence. The contract now uses source-backed API names where visible and explicitly allows missing configuration/API surfaces to be created by this story; no new child tickets or planning documents were created.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 default naming policy is the provider-neutral PascalCase policy in docs/naming/default-naming-policy.md, separate from the lowercase snake_case dvault_* persistence convention document.
- Current branch evidence supports an existing DefaultDataVaultNamingPolicy type, not an assumed DefaultNamingPolicy type name. The implementation may keep that name or introduce a documented successor/alias, but the ticket does not assume a missing type already exists.
- The source-backed naming-policy contract currently covers hub/link/satellite table names, technical column names, index names, and constraint names. Property-column naming for business keys and payloads is required default behavior, but it is not required to be a custom-policy override point unless the implementation adds that public API explicitly.
- Current source-backed builder evidence includes DataVaultModelBuilder.UseDataVault applying DataVaultConventions.Default. A custom-policy configuration path may be added or extended by this story if the current source lacks one.
- Existing parentOf relations from this ticket to 06EXB75NX7Z0DY7X0BD0YFZECM and 06EXB75XTWD7FTRAFE5GNDCS5R were observed; no new relations, child tickets, or planning documents were created in this PO pass.

### Scope In
- Implement or update the provider-neutral default naming policy to follow docs/naming/default-naming-policy.md.
- Generate deterministic table names for hubs, links, and satellites using documented Hub, Link, and Sat prefixes, normalized object tokens, explicit link names when present, and participant-order fallback when needed.
- Generate deterministic names for business-key columns, satellite payload columns, hash keys, hash diffs, load timestamps, and record sources using the documented default rules.
- Apply documented normalization, finite object singularization, finite reserved-word handling, technical-column collision handling, and duplicate column disambiguation.
- Generate deterministic names for the current model index and constraint kinds visible during implementation, including hub business-key, link relationship, satellite parent lookup, and primary-key constraint coverage where those kinds exist.
- Use or extend the source-backed DVault modeling surface under src/DVault/Modeling, including DefaultDataVaultNamingPolicy, IDataVaultNamingPolicy, DataVaultModelBuilder, UseDataVault, and DataVaultConventions.Default where applicable.
- Expose a provider-neutral custom naming-policy path for the source-backed policy families: table names, technical columns, indexes, and constraints; include property-column override only if a public policy API is explicitly added.

### Scope Out
- Provider-specific quoting, reserved-word catalogs beyond the finite v1 default set, physical identifier length limits, SQL dialect casing rules, and adapter-specific name rewriting.
- Hash algorithm implementation, hash input canonicalization, delimiter rules, null handling, or generated hash values.
- Schema generation, migrations, database provider adapters, loading automation, or persistence execution.
- PIT tables, bridge tables, multi-active satellites, and other deferred Data Vault capabilities.
- Changing the separate dvault_* logical persistence artifact naming policy.
- Treating missing API names as pre-existing requirements; missing configuration surfaces may be created within this story when needed.

## Acceptance Criteria
- Given the same model declarations and default naming policy, repeated model builds produce identical table, column, index, and constraint names in the same order.
- Default hub, link, and satellite table names follow docs/naming/default-naming-policy.md, including PascalCase normalization, finite object singularization, documented fallbacks, and unsafe object token handling.
- Default business-key and payload column names follow the documented property-column rule, including PascalCase normalization, no property singularization, documented fallbacks, unsafe property token handling, technical-column reservation, and duplicate disambiguation within the relevant column scope.
- Default technical columns are named according to the documented Data Vault concepts: {Base}HashKey, HashDiff, LoadTimestamp, and RecordSource.
- Default index and constraint names are deterministic, derived from produced table and participating column names, and distinguish the current model index and constraint kinds visible in source during implementation.
- When no custom naming configuration is supplied, the model-building or conventions path uses the default naming policy.
- A caller can supply a custom IDataVaultNamingPolicy through an existing or newly introduced provider-neutral configuration path, and the model builder uses it for hub, link, satellite, technical-column, index, and constraint name generation.
- Custom-policy tests demonstrate override behavior across the source-backed policy families without requiring every property-column normalization detail to be externally overridable unless the story adds such public methods.
- Tests demonstrate deterministic output, documented normalization examples, singular/plural object equivalence, reserved-word handling, collision behavior, index and constraint naming, and the custom naming-policy override path.

## Definition of Done
- Implementation is in the existing DVault modeling source layout and follows repository formatting and nullable C# conventions.
- Automated tests are added or updated in the existing DVault test layout for the default policy and custom-policy path.
- Relevant .NET build/test commands and repository formatting checks pass, or unavailable local tooling is explicitly reported with the attempted command.
- Public XML documentation is present for new public types or members introduced or changed for the naming-policy contract.
- Implementation remains provider-neutral and introduces no database-provider dependency or persistence execution behavior.
- Any newly introduced options/model-creation API for custom naming policy is documented as part of this story rather than treated as pre-existing.

## Implementation Notes
- Use docs/naming/default-naming-policy.md as the normative naming source for this story.
- The current source-backed default policy type is DefaultDataVaultNamingPolicy. Its current placeholder-style composed names must be updated where they conflict with the documented v1 policy.
- Do not rely on the missing DefaultNamingPolicy or DataVaultModelOptions names as existing APIs unless they are found in source during implementation or created intentionally as part of this story.
- Keep the default policy deterministic by avoiding culture-sensitive casing, filesystem state, timestamps, random values, provider metadata, and unordered iteration effects.
- Implement custom-policy override tests around the evidenced IDataVaultNamingPolicy method families: hub/link/satellite table names, technical column names, index names, and constraint names.
- Keep PascalCase Data Vault modeling identifiers separate from the lowercase snake_case dvault_* logical persistence artifact policy.

## Open Questions
- none

## Follow-Up Questions
- Should later provider adapter tickets define physical-name mapping for quoting, provider reserved words, identifier length limits, and provider-native casing while preserving these logical default names?
- Should a later story add optional link declarations without explicit relationship names if the public modeling API wants to rely on participant-order fallback naming?
- Should future persistence or schema generation tickets expand constraint kinds beyond the current primary-key baseline, such as foreign keys or uniqueness constraints for satellite history?
- Should a later API cleanup standardize the public default-policy type name if the implementation keeps DefaultDataVaultNamingPolicy but external examples prefer a shorter alias?

## Risks
- Current DefaultDataVaultNamingPolicy source behavior appears simpler than the documented v1 naming policy; implementation must treat docs/naming/default-naming-policy.md as the accepted product baseline.
- There are two naming domains in the repository: PascalCase Data Vault modeling identifiers and lowercase snake_case dvault_* persistence artifact identifiers. Mixing them would create product ambiguity and test churn.
- Expanding the custom policy interface to property-level naming would increase API surface; keep override coverage to the evidenced policy families unless implementation explicitly adds and documents property-column methods.

## Split Recommendations
- No new split is recommended for this refinement pass.
- Existing parentOf child-ticket relations to 06EXB75NX7Z0DY7X0BD0YFZECM and 06EXB75XTWD7FTRAFE5GNDCS5R remain observed context and were not changed.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Provide stable table and column names without requiring user configuration.

## Scope
- Define naming for hubs, links, satellites, technical columns, indexes, and constraints.

## Acceptance Criteria
- The same input model always yields the same names.
- Users can override naming policy when needed.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.