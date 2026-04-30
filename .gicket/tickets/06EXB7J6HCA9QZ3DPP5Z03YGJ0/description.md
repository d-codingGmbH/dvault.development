<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket to name `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` as the explicit v1 consumer, bind Sqlite v1 to explicit no-function/no-concurrency support plus concrete text/timestamp mappings, and define deterministic unsupported-capability failure behavior.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- No child tickets, new relations, or planning documents were created in this refinement pass; the persisted relation context remains parent `06EXB7HYG17X73GH0K535GYJH8`, blocker `06EXB7FYXNBPMH8VGQCGP2R41R`, and downstream blocked tickets `06EXB7JEF55Y007XK28DAD1E2R` and `06EXB817Q8RAXCQH5QQR5RFY34`.
- The exact v1 consumer path is `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs`; `ApplyProperty` is the required capability reader and its hub/link/satellite caller methods provide the logical role context.
- The initial Sqlite profile explicitly declares no required SQL functions in v1 and no concurrency-token or mutable-conflict capability in v1; those categories are present only as explicit none/unsupported baseline declarations.
- The initial Sqlite profile must cover the current bounded type-mapping baseline: load timestamps remain `DateTimeOffset` in the EF model and persist as SQLite `TEXT` in ISO 8601 UTC form, while hash keys, hash diffs, record sources, participant references, business keys, and current text payload columns map to SQLite `TEXT`.
- Unsupported required capability lookups must fail deterministically with `NotSupportedException` naming the provider profile and missing capability; the abstraction must not silently infer fallbacks from raw provider identity.

### Scope In
- Define one provider capability abstraction in `DCoding.Data.DVault` for provider-dependent decisions currently consumed by `DataVaultEfMetadataTranslator`.
- Wire `DataVaultEfMetadataTranslator.ApplyProperty` and its hub/link/satellite call chain to read the capability abstraction for current logical-to-native mapping decisions instead of future raw provider checks.
- Define the initial Sqlite profile with explicit `none in v1 / unsupported` declarations for SQL functions and concurrency signals.
- Define the initial Sqlite profile's bounded type mappings for load timestamps and current text-backed Data Vault technical, business-key, participant-reference, and payload fields.
- Add unit and integration tests that cover the Sqlite profile, the translator consumer path, and deterministic unsupported-capability failure behavior.

### Scope Out
- Concrete provider profiles beyond Sqlite.
- Generated columns, computed SQL functions, triggers, rowversion/xmin tokens, mutable-record conflict behavior, or other provider-specific concurrency mechanisms.
- Exhaustive native type matrices beyond the current text/timestamp baseline used by the repository-backed hub/link/satellite metadata path.
- Broader public configuration API design or provider plug-in framework work.
- Changes to naming, hashing, record-source, timestamp semantics, or migration policy beyond what is needed to express the bounded capability contract.

## Acceptance Criteria
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` is the explicit first consumer path and reads one capability abstraction for provider-aware property projection decisions instead of introducing raw provider-name/provider-type branching.
- The abstraction exposes explicit v1 values for all scoped categories: SQL functions required by the initial Sqlite profile = `none in v1 / unsupported`; concurrency signals relevant to current persistence behavior = `none in v1 / unsupported`; type mappings = load timestamp plus current text-backed Data Vault fields.
- The initial Sqlite profile declares that load timestamp values project as `DateTimeOffset` and persist as SQLite `TEXT` using ISO 8601 UTC text, while hash key, hash diff, record source, participant reference, business key, and current text payload fields persist as SQLite `TEXT`.
- When the translator consumer path requests a required capability missing from the active profile, the implementation fails with deterministic `NotSupportedException` naming the provider profile and missing capability; unsupported categories are never silently inferred.
- Tests cover the Sqlite profile, the translator consumer path, and at least one unsupported-capability case.

## Definition of Done
- The acceptance criteria are satisfied in `src/DCoding.Data.DVault` and covered in the existing `tests/DCoding.Data.DVault.Tests` layout.
- Any new shared or public contract surface includes repository-standard XML documentation where applicable.
- The implementation preserves provider-neutral logical naming, hashing, record-source, and timestamp semantics from `docs/plans/optional-advanced-configuration-hooks.md` and `docs/plans/dvault-v1-default-persistence-convention-policy.md`.
- Required repository verification for touched files passes under the shared implementation standards.

## Implementation Notes
- Keep the design capability-profile-oriented and anchored to the existing translator path; do not introduce a generic provider plug-in system.
- Model SQL-function and concurrency categories as explicit declarations even though the initial Sqlite profile resolves both to none/unsupported in v1.
- Use `DataVaultEfMetadataTranslator.ApplyProperty` as the mandatory consumer because it already owns the repository's current logical field projection branch between `DateTimeOffset` and `string`.
- Use the SQLite baseline already documented and tested in `docs/architecture/mvp-data-vault-concepts.md`, `tests/DCoding.Data.DVault.Tests/Shared/SqliteTestDatabase.cs`, and related integration tests: ISO 8601 UTC text for timestamps and text-backed columns for current hash, record-source, business-key, participant-reference, and payload fields.
- Keep future provider-function, mutable-concurrency, and broader native-type expansion additive and deferred to later provider or DDL tickets.

## Open Questions
- none

## Follow-Up Questions
- Which downstream ticket should become the first consumer of a non-empty SQL-function capability set once provider-specific query or DDL behavior exists?
- If a later mutable-record ticket introduces conflict handling, should concurrency capability be represented as a closed enum, a structured profile value, or separate additive flags?
- Which provider ecosystem should receive the next concrete capability profile after Sqlite once the currently blocked downstream tickets are ready to consume this abstraction?
- When advanced-configuration work is scheduled, should provider capability selection remain internal first or become public immediately?

## Risks
- If the implementation expands beyond the explicit no-function/no-concurrency baseline, it can reopen the speculative provider-matrix problem the critic flagged.
- If tests do not exercise `DataVaultEfMetadataTranslator` as the first consumer, the abstraction could still land as dormant infrastructure.
- Future provider tickets that require non-text native mappings may need additive contract growth; that is acceptable only if kept versioned and provider-neutral at the logical boundary.

## Split Recommendations
- No split recommended: the ticket is now bounded to one concrete consumer path, one Sqlite profile, explicit none/unsupported baselines for speculative categories, and one small test surface.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Represent provider-specific features without scattering provider checks.

## Scope
- Model differences such as SQL functions, concurrency features, and type mappings.

## Acceptance Criteria
- Provider checks go through one abstraction.
- Sqlite has a concrete capability profile.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.