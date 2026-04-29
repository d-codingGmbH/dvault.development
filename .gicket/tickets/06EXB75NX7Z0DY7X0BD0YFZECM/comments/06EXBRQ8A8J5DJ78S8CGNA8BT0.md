[gicket-bot] PO refinement contract

Summary
- Verified the ticket, comments, incoming parent relation, sibling naming/technical-column tickets, repository snapshot, and charter attachment. No child tickets or planning documents were created; refinement is ready for PO critic.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This ticket defines the v1 default naming policy for table and column identifiers used by DVault when the user supplies no naming configuration.
- The parent story is 06EXB75DX3YAJFMJ6TNHVPAWYG, Story: Implement deterministic naming conventions. The sibling ticket 06EXB75XTWD7FTRAFE5GNDCS5R covers public override points, so override API design is not required here.
- The charter attachment establishes a convention-first, provider-neutral .NET 10 library focused on hubs, links, satellites, hash keys, hash diffs, load timestamps, record source, deterministic naming, and Sqlite examples by default.
- The current repository snapshot contains ticket/project metadata and no visible source or test roots, so this refinement defines behavior without assuming existing production file paths.

Scope In
- Document and test the default table names for hubs, links, and satellites.
- Document and test default column names for business-key/payload columns and technical columns: hash key, hash diff, load timestamp, and record source.
- Normalize entity, role, satellite, and property tokens by trimming, splitting whitespace/punctuation/snake/kebab/Pascal inputs, removing invalid identifier characters, singularizing object tokens with finite v1 rules, and emitting PascalCase identifiers.
- Handle singular/plural inputs deterministically so common variants such as Customer and Customers resolve to the same object base name.
- Handle reserved words, empty/invalid normalized tokens, technical-column collisions, and duplicate identifiers with deterministic fallback names or suffixes.
- Include examples for hub, link, satellite, reserved-word, casing, pluralization, and technical-column behavior.

Scope Out
- Public naming policy interface/options hook implementation; covered by 06EXB75XTWD7FTRAFE5GNDCS5R.
- Provider-specific identifier styles such as quoted SQL names, snake_case, all-uppercase SQL style, or provider-reserved-word catalogs beyond the v1 provider-neutral fallback.
- Index and constraint naming unless a developer needs a minimal internal helper to keep table/column tests coherent.
- PIT tables, bridge tables, multi-active satellites, and provider-specific optimizations.
- Full natural-language inflection or an irregular noun dictionary beyond documented finite v1 singularization rules.

Open questions
- none

Follow-up questions
- Should a later provider-specific naming policy add snake_case or all-uppercase SQL-style identifiers for teams that prefer traditional Data Vault database naming?
- Should a later enhancement add a richer inflection dictionary for irregular plural forms after the finite v1 rules are in place?
- Should index and constraint naming receive a dedicated child ticket under the deterministic naming story if the parent story needs those names specified separately?

Risks
- The technical metadata column contract is a sibling ticket; if it changes the canonical technical fields, the naming examples and tests here must be kept aligned.
- Expanding reserved-word handling into provider-specific catalogs or full SQL quoting would broaden this task beyond the provider-neutral v1 policy.
- Full linguistic singularization can introduce surprising behavior; documenting finite rules keeps v1 deterministic but may require follow-up for irregular domain terms.

Split recommendations
- No split recommended for this ticket. Override points and technical metadata contracts already exist as separate sibling tasks, so this ticket should stay focused on the default policy and its examples/tests.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment