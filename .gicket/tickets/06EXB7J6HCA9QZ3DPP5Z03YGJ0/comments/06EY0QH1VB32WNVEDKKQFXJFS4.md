[gicket-bot] PO refinement contract

Summary
- Bounded this ticket to a v1 provider-capability contract in `DCoding.Data.DVault`: one centralized capability abstraction plus an initial Sqlite profile backed by the existing Sqlite test surface. No child tickets, relations, or planning documents were created; the reviewed relation context remains parent `06EXB7HYG17X73GH0K535GYJH8`, blocker `06EXB7FYXNBPMH8VGQCGP2R41R`, and downstream blocked tickets `06EXB7JEF55Y007XK28DAD1E2R` and `06EXB817Q8RAXCQH5QQR5RFY34`.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 deliverable is a bounded provider capability abstraction, not an exhaustive provider matrix; it centralizes only the provider-dependent decisions DVault currently needs.
- Repository evidence already shows a provider-neutral core in `src/DCoding.Data.DVault` and an existing Sqlite validation surface in `tests/DCoding.Data.DVault.Tests/Shared/SqliteTestDatabase.cs`, so Sqlite is the required first concrete profile.
- The abstraction must stay aligned with `docs/plans/optional-advanced-configuration-hooks.md`: provider behavior is an additive extension point and must not redefine naming, hashing, record-source, or timestamp semantics.
- Any concurrency coverage in this ticket is descriptive of provider support that DVault may consume; it does not define mutable-record update or conflict semantics, which the persistence policy keeps deferred.

Scope In
- Define one provider capability abstraction owned by `DCoding.Data.DVault` for provider-dependent decisions used by DVault.
- Cover the bounded v1 capability categories already named by the ticket: SQL-function availability needed by current DVault behavior, concurrency behavior only where it affects current persistence behavior, and logical-to-native type mapping decisions required by the current artifact model.
- Define a concrete Sqlite capability profile as the first repository baseline.
- Route provider-aware branches in the touched implementation path through the abstraction instead of raw provider-name or provider-type checks.
- Add unit and integration coverage for the abstraction and the Sqlite profile in the existing `tests/DCoding.Data.DVault.Tests` structure.

Scope Out
- Concrete profiles or option matrices for providers other than Sqlite.
- Provider-specific optimizations, dialect-specific DDL details, or exhaustive cross-provider feature catalogs.
- Broader runtime configuration API design for all advanced provider options.
- Changes to naming, hashing, record-source, timestamp, migration, or mutable-record conflict contracts beyond what is needed to define this capability boundary.

Open questions
- none

Follow-up questions
- Which provider ecosystem should receive the next concrete capability profile after Sqlite once the currently blocked downstream tickets are ready to consume this abstraction?
- When the later advanced-configuration work is scheduled, should provider capability selection remain internal first or be exposed immediately as a documented public configuration surface?
- Do future migration or DDL tickets need additional capability categories beyond SQL functions, bounded concurrency behavior, and type mappings, or can those stay deferred until a concrete provider requires them?

Risks
- If the abstraction tries to model every possible provider feature now, it will become a speculative provider matrix and slow the downstream tickets this work is supposed to unblock.
- If provider-neutral logical contracts leak provider-native terms into the core API, later non-Sqlite adapters may inherit avoidable coupling.
- If no real consumer path is wired to the abstraction in this ticket, the result may remain a dormant contract that does not actually eliminate scattered provider checks.

Split recommendations
- No split recommended: current evidence keeps the work bounded to one abstraction plus one Sqlite profile, which is sufficient to support the downstream tickets already blocked by this item.

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