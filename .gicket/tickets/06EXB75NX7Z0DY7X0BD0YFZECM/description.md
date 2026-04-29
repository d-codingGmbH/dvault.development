<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the ticket, comments, incoming parent relation, sibling naming/technical-column tickets, repository snapshot, and charter attachment. No child tickets or planning documents were created; refinement is ready for PO critic.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket defines the v1 default naming policy for table and column identifiers used by DVault when the user supplies no naming configuration.
- The parent story is 06EXB75DX3YAJFMJ6TNHVPAWYG, Story: Implement deterministic naming conventions. The sibling ticket 06EXB75XTWD7FTRAFE5GNDCS5R covers public override points, so override API design is not required here.
- The charter attachment establishes a convention-first, provider-neutral .NET 10 library focused on hubs, links, satellites, hash keys, hash diffs, load timestamps, record source, deterministic naming, and Sqlite examples by default.
- The current repository snapshot contains ticket/project metadata and no visible source or test roots, so this refinement defines behavior without assuming existing production file paths.

### Scope In
- Document and test the default table names for hubs, links, and satellites.
- Document and test default column names for business-key/payload columns and technical columns: hash key, hash diff, load timestamp, and record source.
- Normalize entity, role, satellite, and property tokens by trimming, splitting whitespace/punctuation/snake/kebab/Pascal inputs, removing invalid identifier characters, singularizing object tokens with finite v1 rules, and emitting PascalCase identifiers.
- Handle singular/plural inputs deterministically so common variants such as Customer and Customers resolve to the same object base name.
- Handle reserved words, empty/invalid normalized tokens, technical-column collisions, and duplicate identifiers with deterministic fallback names or suffixes.
- Include examples for hub, link, satellite, reserved-word, casing, pluralization, and technical-column behavior.

### Scope Out
- Public naming policy interface/options hook implementation; covered by 06EXB75XTWD7FTRAFE5GNDCS5R.
- Provider-specific identifier styles such as quoted SQL names, snake_case, all-uppercase SQL style, or provider-reserved-word catalogs beyond the v1 provider-neutral fallback.
- Index and constraint naming unless a developer needs a minimal internal helper to keep table/column tests coherent.
- PIT tables, bridge tables, multi-active satellites, and provider-specific optimizations.
- Full natural-language inflection or an irregular noun dictionary beyond documented finite v1 singularization rules.

## Acceptance Criteria
- Naming behavior is documented in English with concrete examples for hub, link, satellite, business-key/payload, hash-key, hash-diff, load-timestamp, and record-source names.
- The v1 default table format is documented as PascalCase with Data Vault prefixes: Hub{Entity}, Link{ParticipantOrRelationshipName}, and Sat{Parent}{SatelliteDescriptor}; examples include HubCustomer, LinkCustomerOrder, and SatCustomerContact.
- The v1 default technical columns are documented as deterministic PascalCase names, including {Base}HashKey for hash keys plus HashDiff, LoadTimestamp, and RecordSource where applicable.
- Common singular/plural and casing variants produce stable object names, with documented finite singularization rules and fallback behavior for names the rules do not change.
- Reserved words and collisions are covered by examples, including appending Value or Entity for unsafe base tokens and deterministic numeric suffixes for same-scope duplicates.
- Tests cover common edge cases: whitespace and punctuation normalization, snake/kebab/Pascal input, Customer versus Customers, reserved property names such as Order, collisions with technical columns, duplicate normalized names, and repeat calls returning identical names.

## Definition of Done
- The documented policy and tests satisfy the refined acceptance criteria.
- Default behavior works without user-supplied configuration and remains compatible with later override hooks.
- Documentation and sample text are in English and align with the charter attachment's convention-first, provider-neutral guidance.
- Public or protected API introduced for the policy is documented if implementation work creates such API.
- Relevant unit tests pass in the repository's established or newly created test layout.

## Implementation Notes
- Prefer a small deterministic normalizer over provider-specific quoting as the v1 baseline: normalize to PascalCase, use finite singularization for object/table base tokens, and leave unrecognized words unchanged.
- Suggested finite singularization baseline: convert trailing ies to y, strip es from common sibilant plurals, strip a trailing s except ss, and otherwise keep the token unchanged.
- For columns derived from user properties, preserve the normalized semantic token except when it is empty, reserved, duplicates another column in the same scope, or collides with technical columns; then apply the documented fallback and suffix rules.
- For links, use an explicit relationship/link name when available; otherwise concatenate normalized participant role/entity names in model declaration order so the same input model always yields the same name.
- Coordinate examples with 06EXB755X9TGQW2EG1G30GJG28, Task: Define technical metadata column contracts, because that ticket owns the detailed metadata contract while this ticket owns the naming policy.
- Sqlite should be the default example/test provider when provider interaction is needed, consistent with the charter attachment.

## Open Questions
- none

## Follow-Up Questions
- Should a later provider-specific naming policy add snake_case or all-uppercase SQL-style identifiers for teams that prefer traditional Data Vault database naming?
- Should a later enhancement add a richer inflection dictionary for irregular plural forms after the finite v1 rules are in place?
- Should index and constraint naming receive a dedicated child ticket under the deterministic naming story if the parent story needs those names specified separately?

## Risks
- The technical metadata column contract is a sibling ticket; if it changes the canonical technical fields, the naming examples and tests here must be kept aligned.
- Expanding reserved-word handling into provider-specific catalogs or full SQL quoting would broaden this task beyond the provider-neutral v1 policy.
- Full linguistic singularization can introduce surprising behavior; documenting finite rules keeps v1 deterministic but may require follow-up for irregular domain terms.

## Split Recommendations
- No split recommended for this ticket. Override points and technical metadata contracts already exist as separate sibling tasks, so this ticket should stay focused on the default policy and its examples/tests.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Specify the default naming policy used by the library.

## Scope
- Cover singular/plural inputs, casing, reserved words, and technical columns.

## Acceptance Criteria
- Naming behavior is documented with examples.
- Tests cover common edge cases.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.