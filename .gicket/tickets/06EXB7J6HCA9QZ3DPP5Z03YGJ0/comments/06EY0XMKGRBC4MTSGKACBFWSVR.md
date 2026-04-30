[gicket-bot] PO refinement contract

Summary
- Refined the ticket to name `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` as the explicit v1 consumer, bind Sqlite v1 to explicit no-function/no-concurrency support plus concrete text/timestamp mappings, and define deterministic unsupported-capability failure behavior.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The exact v1 consumer path is `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs`, with `ApplyProperty` as the mandatory capability reader and `CreateHubEntity`, `CreateLinkEntity`, and `CreateSatelliteEntity` supplying the logical role context. This is the existing main-library path that already chooses between `DateTimeOffset` and `string` projections, so it is the repository-backed place where capability lookups must replace any future raw provider branching.
- critic-item-2: `answered` - The scoped v1 examples are now explicit: required SQL functions for the initial Sqlite profile are `none in v1 / unsupported`; bounded concurrency signals are `none in v1 / unsupported` because the v1 persistence baseline is immutable and mutable-record conflict semantics are deferred; logical-to-native mappings the initial Sqlite profile must cover are load timestamp -> `DateTimeOffset` logical projection with SQLite `TEXT` persisted as ISO 8601 UTC text, and hash key/hash diff/record source/business key/participant reference/payload text -> `string` logical projection with SQLite `TEXT` persistence.
- critic-item-3: `answered` - Unsupported capabilities must surface deterministically: if the `DataVaultEfMetadataTranslator` consumer path requests a required capability that the active profile marks unsupported or does not declare, the lookup must fail with `NotSupportedException` naming the provider profile and missing capability. The implementation must not silently infer a fallback or branch on raw provider identity. Categories intentionally absent in v1, such as SQL functions and concurrency signals for the initial Sqlite profile, must be exposed as explicit none/unsupported declarations rather than guessed at runtime.
- critic-item-4: `answered` - The blocking consumer-path finding is resolved by ratifying `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` as the mandatory first consumer. The capability abstraction is not a dormant side contract; its first required use is the property projection path that currently owns the only repository-backed logical-to-type branch in the main library.
- critic-item-5: `answered` - The speculative-scope finding is resolved by shrinking each category to explicit repository-backed v1 values: SQL functions = `none in v1 / unsupported`, concurrency = `none in v1 / unsupported`, and type mappings = SQLite text-backed storage for current DVault metadata/payload fields plus ISO 8601 UTC text storage for load timestamps. This removes the need for the developer to invent provider features beyond what the repository already documents.

Clarifications
- No child tickets, new relations, or planning documents were created in this refinement pass; the persisted relation context remains parent `06EXB7HYG17X73GH0K535GYJH8`, blocker `06EXB7FYXNBPMH8VGQCGP2R41R`, and downstream blocked tickets `06EXB7JEF55Y007XK28DAD1E2R` and `06EXB817Q8RAXCQH5QQR5RFY34`.
- The exact v1 consumer path is `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs`; `ApplyProperty` is the required capability reader and its hub/link/satellite caller methods provide the logical role context.
- The initial Sqlite profile explicitly declares no required SQL functions in v1 and no concurrency-token or mutable-conflict capability in v1; those categories are present only as explicit none/unsupported baseline declarations.
- The initial Sqlite profile must cover the current bounded type-mapping baseline: load timestamps remain `DateTimeOffset` in the EF model and persist as SQLite `TEXT` in ISO 8601 UTC form, while hash keys, hash diffs, record sources, participant references, business keys, and current text payload columns map to SQLite `TEXT`.
- Unsupported required capability lookups must fail deterministically with `NotSupportedException` naming the provider profile and missing capability; the abstraction must not silently infer fallbacks from raw provider identity.

Scope In
- Define one provider capability abstraction in `DCoding.Data.DVault` for provider-dependent decisions currently consumed by `DataVaultEfMetadataTranslator`.
- Wire `DataVaultEfMetadataTranslator.ApplyProperty` and its hub/link/satellite call chain to read the capability abstraction for current logical-to-native mapping decisions instead of future raw provider checks.
- Define the initial Sqlite profile with explicit `none in v1 / unsupported` declarations for SQL functions and concurrency signals.
- Define the initial Sqlite profile's bounded type mappings for load timestamps and current text-backed Data Vault technical, business-key, participant-reference, and payload fields.
- Add unit and integration tests that cover the Sqlite profile, the translator consumer path, and deterministic unsupported-capability failure behavior.

Scope Out
- Concrete provider profiles beyond Sqlite.
- Generated columns, computed SQL functions, triggers, rowversion/xmin tokens, mutable-record conflict behavior, or other provider-specific concurrency mechanisms.
- Exhaustive native type matrices beyond the current text/timestamp baseline used by the repository-backed hub/link/satellite metadata path.
- Broader public configuration API design or provider plug-in framework work.
- Changes to naming, hashing, record-source, timestamp semantics, or migration policy beyond what is needed to express the bounded capability contract.

Open questions
- none

Follow-up questions
- Which downstream ticket should become the first consumer of a non-empty SQL-function capability set once provider-specific query or DDL behavior exists?
- If a later mutable-record ticket introduces conflict handling, should concurrency capability be represented as a closed enum, a structured profile value, or separate additive flags?
- Which provider ecosystem should receive the next concrete capability profile after Sqlite once the currently blocked downstream tickets are ready to consume this abstraction?
- When advanced-configuration work is scheduled, should provider capability selection remain internal first or become public immediately?

Risks
- If the implementation expands beyond the explicit no-function/no-concurrency baseline, it can reopen the speculative provider-matrix problem the critic flagged.
- If tests do not exercise `DataVaultEfMetadataTranslator` as the first consumer, the abstraction could still land as dormant infrastructure.
- Future provider tickets that require non-text native mappings may need additive contract growth; that is acceptable only if kept versioned and provider-neutral at the logical boundary.

Split recommendations
- No split recommended: the ticket is now bounded to one concrete consumer path, one Sqlite profile, explicit none/unsupported baselines for speculative categories, and one small test surface.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment