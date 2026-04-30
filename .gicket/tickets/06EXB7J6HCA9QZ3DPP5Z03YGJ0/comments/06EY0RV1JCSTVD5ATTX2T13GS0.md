[gicket-bot] PO-critic review contract

Summary
- Contract quality improved and `## Open Questions` is resolved, but the ticket still does not identify a concrete first consumer path or concrete v1 examples for the SQL-function, concurrency, and native-type capability slices, so developer handoff would still require guesswork.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB7J6HCA9QZ3DPP5Z03YGJ0/description.md` contains `## Open Questions` with `- none`, so approval is not blocked by unresolved open questions.
- `git -C /mnt/c/Projects/DVault rev-parse ticket/06EXB7J6HCA9QZ3DPP5Z03YGJ0-task-define-provider-capability-abstraction` and `git -C /mnt/c/Projects/DVault rev-parse a7051654adaf172886ff1da1bbb6c1d8e3dae194` both resolved to `a7051654adaf172886ff1da1bbb6c1d8e3dae194`.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` currently projects provider-neutral EF metadata for hubs, links, and satellites; it contains no provider selection, provider-name checks, SQL-function calls, or store-type mapping logic.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs` includes `MetadataAbstractionsUseProviderNeutralClrContracts`, which explicitly rejects provider tokens such as `Sqlite`, `Postgres`, `Npgsql`, `Migration`, `Sequence`, and `Trigger` in the current metadata contract surface.
- Repository search for `HasDbFunction|HasColumnType|TypeMapping|IsConcurrencyToken|rowversion|xmin|ProviderName|Sqlite|Postgres` found matches in the Sqlite test harness `tests/DCoding.Data.DVault.Tests/Shared/SqliteTestDatabase.cs` and test files, but not in a main-library provider-aware consumer path under `src/DCoding.Data.DVault`.
- `docs/plans/optional-advanced-configuration-hooks.md` defines provider behavior as an optional additive extension point and defers concrete provider option matrices, while `docs/plans/dvault-v1-default-persistence-convention-policy.md` explicitly avoids SQL column types and defers mutable-record update, concurrency, and conflict semantics.
- Persisted relations for this ticket are limited to `.gicket/relations/H8/J0/06EXB7HYG17X73GH0K535GYJH8--06EXB7J6HCA9QZ3DPP5Z03YGJ0--parentOf.json`, `.gicket/relations/1R/J0/06EXB7FYXNBPMH8VGQCGP2R41R--06EXB7J6HCA9QZ3DPP5Z03YGJ0--blocks.json`, `.gicket/relations/J0/2R/06EXB7J6HCA9QZ3DPP5Z03YGJ0--06EXB7JEF55Y007XK28DAD1E2R--blocks.json`, and `.gicket/relations/J0/34/06EXB7J6HCA9QZ3DPP5Z03YGJ0--06EXB817Q8RAXCQH5QQR5RFY34--blocks.json`; there is no persisted relation tying this ticket to `06EXB7GESWZZTZG7XYAKTTKQRW` as a first consumer.

Blocking findings
- The contract requires a real consumer (`route provider-aware branches in the touched implementation path` and `at least one consumer path`), but it does not name a concrete current consumer in `src/DCoding.Data.DVault`. Repository inspection found no existing provider-aware branch or raw provider identity check in the main library, so the developer would have to invent the first consumer path.
- The scoped capability categories are not anchored to concrete repository-backed examples. Current source and docs do not identify any required SQL function, current concurrency signal, or current logical-to-native type mapping that DVault already depends on, and the persistence policy still defers SQL column types plus mutable concurrency/conflict behavior. As written, the abstraction boundary is still speculative.

Required PO actions
- Name the exact v1 consumer path in `src/DCoding.Data.DVault` that must read the capability abstraction in this ticket, or explicitly add the intended consumer ticket/relation if the first consumer lives in another ticket.
- For each scoped category, add at least one concrete v1 example or an explicit `none in v1 / unsupported` statement: required SQL function(s), bounded concurrency signal(s), and logical-to-native type mapping(s) the initial Sqlite profile must cover.
- Clarify how unsupported capabilities must surface in the first consumer path (for example explicit unavailable marker versus deterministic exception) so the `fail clearly` acceptance criterion is testable without invention by the developer.

Open issues ledger
- critic-item-1 [required-po-action] Name the exact v1 consumer path in `src/DCoding.Data.DVault` that must read the capability abstraction in this ticket, or explicitly add the intended consumer ticket/relation if the first consumer lives in another ticket.
- critic-item-2 [required-po-action] For each scoped category, add at least one concrete v1 example or an explicit `none in v1 / unsupported` statement: required SQL function(s), bounded concurrency signal(s), and logical-to-native type mapping(s) the initial Sqlite profile must cover.
- critic-item-3 [required-po-action] Clarify how unsupported capabilities must surface in the first consumer path (for example explicit unavailable marker versus deterministic exception) so the `fail clearly` acceptance criterion is testable without invention by the developer.
- critic-item-4 [blocking-finding] The contract requires a real consumer (`route provider-aware branches in the touched implementation path` and `at least one consumer path`), but it does not name a concrete current consumer in `src/DCoding.Data.DVault`. Repository inspection found no existing provider-aware branch or raw provider identity check in the main library, so the developer would have to invent the first consumer path.
- critic-item-5 [blocking-finding] The scoped capability categories are not anchored to concrete repository-backed examples. Current source and docs do not identify any required SQL function, current concurrency signal, or current logical-to-native type mapping that DVault already depends on, and the persistence policy still defers SQL column types plus mutable concurrency/conflict behavior. As written, the abstraction boundary is still speculative.

Missing examples / edge cases
- A concrete unsupported-capability example for the Sqlite profile, so `fail clearly` has an observable expected result.
- A concrete concurrency baseline example if v1 is immutable or has no update-token behavior, so the abstraction does not imply deferred mutable-record semantics.
- A concrete type-mapping example for current logical fields, or an explicit statement that type mapping remains descriptive only until a named artifact consumer exists.
- An example of how the initial Sqlite profile is selected while provider capability selection remains non-public in v1, if that is the intent.

Risky assumptions
- Assumes the test-only helper `tests/DCoding.Data.DVault.Tests/Shared/SqliteTestDatabase.cs` is enough to infer main-library provider capability requirements.
- Assumes a provider-aware branch already exists or is obvious, but repository searches found none under `src/DCoding.Data.DVault`.
- Assumes concurrency can be modeled safely without reintroducing the mutable-record update/conflict semantics that `docs/plans/dvault-v1-default-persistence-convention-policy.md` explicitly defers.

AC / test suggestions
- Add an acceptance criterion that names the first consumer file or API path and requires a test proving that path uses the abstraction rather than raw provider tokens.
- Add a testable example of the `unsupported/unavailable` shape for a missing capability in the Sqlite profile.
- Add a criterion stating whether v1 concurrency is `not applicable/unsupported` for the current immutable baseline or specifying the one bounded behavior that must be exposed.
- Add a criterion or implementation note naming the first logical fields that need native-type mapping in the Sqlite baseline.

Implementation watchouts
- Do not let the abstraction leak provider-native terms into the provider-neutral metadata surface already guarded by `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs`.
- Keep the surface capability-profile-oriented and avoid turning `docs/plans/optional-advanced-configuration-hooks.md` into a concrete provider matrix or public configuration API in this ticket.
- Do not invent SQL/store-type or mutable-concurrency behavior that contradicts the deferred decisions in `docs/plans/dvault-v1-default-persistence-convention-policy.md`.

Non-blocking notes
- The persisted contract and PO comment both resolve `## Open Questions` to `none`.
- The refinement is grounded in real repository artifacts: `src/DCoding.Data.DVault`, `tests/DCoding.Data.DVault.Tests/Shared/SqliteTestDatabase.cs`, and the cited planning docs all exist.

Split recommendations
- No split is required if PO can pin the first consumer path and concrete category examples in this ticket.
- If that consumer cannot be identified now, split `define capability contract` from `wire first consumer path` so the abstraction does not ship as a dormant contract.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment