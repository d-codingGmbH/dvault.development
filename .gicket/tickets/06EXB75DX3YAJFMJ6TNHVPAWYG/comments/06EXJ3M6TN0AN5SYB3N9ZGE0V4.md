[gicket-bot] PO refinement contract

Summary
- Refined the deterministic naming conventions story against the existing v1 default naming policy, MVP Data Vault concept baseline, and current DVault modeling project shape. The ticket has enough bounded product and architecture context for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 default naming policy is the provider-neutral PascalCase policy documented in docs/naming/default-naming-policy.md, not the lowercase snake_case logical persistence convention used for canonical DVault record artifacts in the separate persistence policy.
- This story covers generated Data Vault modeling names for hubs, links, satellites, technical columns, business-key columns, payload columns, indexes, and constraints.
- The visible src/DVault modeling project and tests/DVault.Tests test root are the bounded v1 implementation targets for this story.

Scope In
- Implement the default naming policy used when a model does not provide custom naming configuration.
- Generate deterministic table names for hubs, links, and satellites using the documented Hub, Link, and Sat prefixes and normalized object tokens.
- Generate deterministic column names for business keys, satellite payload properties, hash keys, hash diffs, load timestamps, and record sources.
- Apply documented normalization, finite singularization for object tokens, finite reserved-word handling, technical-column collision handling, and duplicate column disambiguation from the default naming policy document.
- Generate deterministic index names for the model index kinds exposed by the current modeling API, including hub business-key, link relationship, and satellite parent lookup indexes.
- Generate deterministic constraint names for the current modeling constraint kinds, including primary-key constraints.
- Expose and use a naming-policy override path so callers can replace the default policy without changing the input model declarations.
- Cover optionless model creation and configured naming-policy creation with focused automated tests.

Scope Out
- Provider-specific quoting, reserved-word catalogs, physical name length limits, SQL dialect casing rules, and adapter-specific name rewriting.
- Hash algorithm implementation, hash input canonicalization, delimiter rules, null handling, or generated hash values.
- Schema generation, migrations, database provider adapters, loading automation, or persistence execution.
- PIT tables, bridge tables, multi-active satellites, and other deferred Data Vault capabilities.
- Changing the separate dvault_* logical persistence object naming policy for canonical DVault record artifacts.
- Public API naming bikeshedding beyond the existing architecture needed to inject or resolve a naming policy.

Open questions
- none

Follow-up questions
- Should later provider adapter tickets define physical-name mapping for quoting, reserved words, identifier length limits, and provider-native casing while preserving these logical default names?
- Should a later story add optional link declarations without explicit relationship names if the public modeling API wants to rely on participant-order fallback naming?
- Should future persistence or schema generation tickets expand constraint kinds beyond the current primary-key baseline, such as foreign keys or uniqueness constraints for satellite history?

Risks
- The default naming policy document is more detailed than the original ticket description; implementation should treat the document as the accepted v1 baseline to avoid reopening already-set naming decisions.
- There are two naming domains in the repository: PascalCase Data Vault modeling identifiers and lowercase snake_case dvault_* persistence artifact identifiers. Mixing them would create product ambiguity and test churn.
- Custom naming-policy coverage must be broad enough to prove override behavior without forcing every property-column normalization detail into the override contract unless the existing API already does so.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment