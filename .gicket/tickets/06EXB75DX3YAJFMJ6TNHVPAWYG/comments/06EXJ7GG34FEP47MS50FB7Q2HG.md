[gicket-bot] PO refinement contract

Summary
- Re-refined the naming story against the current ticket, comments, relations, docs, and source evidence. The contract now uses source-backed API names where visible and explicitly allows missing configuration/API surfaces to be created by this story; no new child tickets or planning documents were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract is restated so it no longer assumes unsupported existing types such as DefaultNamingPolicy or DataVaultModelOptions. Current source evidence supports DefaultDataVaultNamingPolicy implementing IDataVaultNamingPolicy, context-based methods for hub/link/satellite table names, technical columns, indexes, and constraints, and DataVaultModelBuilder.UseDataVault applying DataVaultConventions.Default. Any additional custom-policy configuration surface needed for model creation may be introduced by this story.
- critic-item-2: `answered` - The delivery contract now distinguishes existing source-backed surfaces from implementation work. Existing public API claims are limited to DefaultDataVaultNamingPolicy, IDataVaultNamingPolicy method families, DataVaultModelBuilder, UseDataVault, and DataVaultConventions.Default as evidenced in source. The story may create or extend missing model/options APIs needed to configure a custom naming policy.
- critic-item-3: `answered` - Custom naming-policy coverage is narrowed to the source-backed policy boundary: hub, link, and satellite table names; technical column names; index names; and constraint names. Business-key and payload property-column normalization remain default-policy/model-building behavior unless the implementation explicitly adds public policy methods for property columns. Override tests should prove broad override behavior across the evidenced policy families without requiring every property-column normalization detail to be externally overridable.

Clarifications
- The v1 default naming policy is the provider-neutral PascalCase policy in docs/naming/default-naming-policy.md, separate from the lowercase snake_case dvault_* persistence convention document.
- Current branch evidence supports an existing DefaultDataVaultNamingPolicy type, not an assumed DefaultNamingPolicy type name. The implementation may keep that name or introduce a documented successor/alias, but the ticket does not assume a missing type already exists.
- The source-backed naming-policy contract currently covers hub/link/satellite table names, technical column names, index names, and constraint names. Property-column naming for business keys and payloads is required default behavior, but it is not required to be a custom-policy override point unless the implementation adds that public API explicitly.
- Current source-backed builder evidence includes DataVaultModelBuilder.UseDataVault applying DataVaultConventions.Default. A custom-policy configuration path may be added or extended by this story if the current source lacks one.
- Existing parentOf relations from this ticket to 06EXB75NX7Z0DY7X0BD0YFZECM and 06EXB75XTWD7FTRAFE5GNDCS5R were observed; no new relations, child tickets, or planning documents were created in this PO pass.

Scope In
- Implement or update the provider-neutral default naming policy to follow docs/naming/default-naming-policy.md.
- Generate deterministic table names for hubs, links, and satellites using documented Hub, Link, and Sat prefixes, normalized object tokens, explicit link names when present, and participant-order fallback when needed.
- Generate deterministic names for business-key columns, satellite payload columns, hash keys, hash diffs, load timestamps, and record sources using the documented default rules.
- Apply documented normalization, finite object singularization, finite reserved-word handling, technical-column collision handling, and duplicate column disambiguation.
- Generate deterministic names for the current model index and constraint kinds visible during implementation, including hub business-key, link relationship, satellite parent lookup, and primary-key constraint coverage where those kinds exist.
- Use or extend the source-backed DVault modeling surface under src/DVault/Modeling, including DefaultDataVaultNamingPolicy, IDataVaultNamingPolicy, DataVaultModelBuilder, UseDataVault, and DataVaultConventions.Default where applicable.
- Expose a provider-neutral custom naming-policy path for the source-backed policy families: table names, technical columns, indexes, and constraints; include property-column override only if a public policy API is explicitly added.

Scope Out
- Provider-specific quoting, reserved-word catalogs beyond the finite v1 default set, physical identifier length limits, SQL dialect casing rules, and adapter-specific name rewriting.
- Hash algorithm implementation, hash input canonicalization, delimiter rules, null handling, or generated hash values.
- Schema generation, migrations, database provider adapters, loading automation, or persistence execution.
- PIT tables, bridge tables, multi-active satellites, and other deferred Data Vault capabilities.
- Changing the separate dvault_* logical persistence artifact naming policy.
- Treating missing API names as pre-existing requirements; missing configuration surfaces may be created within this story when needed.

Open questions
- none

Follow-up questions
- Should later provider adapter tickets define physical-name mapping for quoting, provider reserved words, identifier length limits, and provider-native casing while preserving these logical default names?
- Should a later story add optional link declarations without explicit relationship names if the public modeling API wants to rely on participant-order fallback naming?
- Should future persistence or schema generation tickets expand constraint kinds beyond the current primary-key baseline, such as foreign keys or uniqueness constraints for satellite history?
- Should a later API cleanup standardize the public default-policy type name if the implementation keeps DefaultDataVaultNamingPolicy but external examples prefer a shorter alias?

Risks
- Current DefaultDataVaultNamingPolicy source behavior appears simpler than the documented v1 naming policy; implementation must treat docs/naming/default-naming-policy.md as the accepted product baseline.
- There are two naming domains in the repository: PascalCase Data Vault modeling identifiers and lowercase snake_case dvault_* persistence artifact identifiers. Mixing them would create product ambiguity and test churn.
- Expanding the custom policy interface to property-level naming would increase API surface; keep override coverage to the evidenced policy families unless implementation explicitly adds and documents property-column methods.

Split recommendations
- No new split is recommended for this refinement pass.
- Existing parentOf child-ticket relations to 06EXB75NX7Z0DY7X0BD0YFZECM and 06EXB75XTWD7FTRAFE5GNDCS5R remain observed context and were not changed.

Persisted contract coverage
- acceptance-criteria items: 9
- definition-of-done items: 6
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment