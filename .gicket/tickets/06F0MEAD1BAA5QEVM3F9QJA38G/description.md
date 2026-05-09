<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the attached parity-child addendum, current repository test baselines, and live ticket relations; no new split or relation cleanup is needed, and the ticket is ready for PO-critic.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- `docs/plans/06F0MEAD1BAA5QEVM3F9QJA38G-fluent-parity-child-boundary.md` is already attached to this ticket and remains the authoritative supplement to the short ticket draft.
- The repository already contains the bounded fluent projection work from done tickets `06F0ME9PM8KXH3VP59TQR0ETA8` and `06F0MEA1FF743S14XQW02H4A3W`, plus focused translator-level parity tests in `DataVaultCodeFirstMetadataTranslationTests` and `DataVaultCodeFirstLinkTests`; this child owns the broader schema and provider-profile parity layer on top of that baseline.
- Live relations are consistent as-is: story `06F0ME8NFJX6CD20MEA10J761R` is the parent, done tasks `06F0ME976PM5455JK04S6GPNNW`, `06F0ME9PM8KXH3VP59TQR0ETA8`, and `06F0MEA1FF743S14XQW02H4A3W` are completed upstream dependency context, and this ticket still blocks `06F0MEDBFZ25YA1M7RJ71Z7ZCM`.
- No new child tickets, planning documents, attachments, or relation updates were materialized during this refinement.

### Scope In
- Add parity tests that compare metadata-first and code-first output for the bounded fluent baseline: hub, ordinary hub-parent satellite, hub-parent multi-active satellite with ordered `DrivingKey(...)`, and link relationship projection.
- Use SQLite schema creation or create-script inspection to prove code-first and metadata-first declarations generate the same table, column, primary-key, and secondary-index shape for the covered baseline.
- Add provider-profile parity assertions for the finite built-in profiles already present in the repository: `Sqlite`, `Oracle`, `Postgres`, `SqlServer`, and `MySql`.
- Add model-level or relational-metadata inspection tests that keep naming collisions, identifier truncation, included-index behavior, load-timestamp storage, and provider capability profile differences explicit.
- Fail on accidental drift in canonical declaration ordering, especially business keys, link participants, payload members, and driving-key columns.

### Scope Out
- Implementing or changing the fluent builders themselves; that projection work is already owned by the completed sibling tickets.
- Link-parent satellites, PIT, bridge, save-service, typed save/read helpers, and any other shapes outside the bounded parent fluent contract.
- Running full integration coverage against every external database server.
- Introducing checked-in EF migration files or a new repository-wide migration infrastructure baseline; this ticket uses inspection-style parity tests instead.

## Acceptance Criteria
- A new parity test fixture builds equivalent metadata-first and code-first models for the covered fluent baseline and proves they match in table, column, primary-key, and index shape.
- SQLite parity coverage uses the repository's existing schema-test style to compare actual generated schema or canonical schema snapshots without requiring external infrastructure.
- Provider-profile parity coverage compares metadata-first and code-first projection for each built-in profile `sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, and `mysql-pomelo-v1`, keeping provider-specific storage and identifier differences visible instead of abstracting them away.
- The covered multi-active hub-parent satellite scenario proves one or more `DrivingKey(...)` calls preserve canonical driving-key ordering and match the metadata-first primary-key and index column order.
- Parity tests fail when code-first translation drifts on naming collisions, provider-capability-driven index behavior, or other schema-shape semantics already defined by the metadata-first translator.

## Definition of Done
- Repository test coverage includes focused code-first-vs-metadata-first parity assertions in the existing test projects, with no requirement for local Oracle, PostgreSQL, MySQL, or SQL Server instances.
- SQLite schema parity and provider-profile parity both pass using the current translator path rather than a second schema-generation implementation.
- The attached child-boundary addendum and this ticket contract stay aligned: hub, ordinary hub-parent satellite, covered `DrivingKey(...)` multi-active satellite, and link parity are in scope; link-parent satellites remain out.
- No relation cleanup, child-ticket split, or extra planning artifact is required to complete this ticket.

## Implementation Notes
- Use the existing metadata-first baselines in `tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs`, `Unit/DataVaultCodeFirstMetadataTranslationTests.cs`, and `Unit/DataVaultCodeFirstLinkTests.cs` as the starting point instead of inventing a second naming or translation harness.
- The repository already exposes a finite provider-profile set through `DataVaultProviderCapabilityProfiles`: `Sqlite`, `Oracle`, `Postgres`, `SqlServer`, and `MySql`. Treat that visible set as the v1 provider-profile matrix for this ticket.
- Because the repository does not currently carry a migrations directory or migration scaffolding baseline, use EF model or relational metadata, `EnsureCreated()`, and `GenerateCreateScript()`-style inspection where helpful rather than checked-in migration artifacts.
- Keep parity assertions independent on the two sides; avoid helpers that normalize away provider annotations or reuse the same expected-shape object too early, or the tests can hide real drift.
- The already attached addendum `06F0MEAD1BAA5QEVM3F9QJA38G-fluent-parity-child-boundary.md` remains the authoritative boundary document, so no new planning write is needed.

## Open Questions
- none

## Follow-Up Questions
- If the fluent surface later expands to link-parent satellites, should this parity matrix be extended in a separate ticket rather than reopening this bounded child?
- After this parity layer lands, does the team want a future ticket that scaffolds actual EF migration artifacts, or is relational-model and create-script parity sufficient for v0.6?
- Once non-SQLite CI infrastructure exists, should one or more live provider smoke tests be added on top of the provider-profile inspection matrix?

## Risks
- If parity coverage only checks translator-level metadata and never exercises SQLite schema creation, EF relational-name or index-order drift can slip through.
- If any built-in provider profile is omitted from the inspection matrix, provider-specific behaviors such as Oracle primary-key-covered indexes or MySQL identifier limits can regress unnoticed.
- If the code-first and metadata-first assertions share too much normalization logic, the test suite can produce false positives and miss real schema divergence.

## Split Recommendations
- No new split is required. Keep the existing parent and sibling dependency structure: `06F0ME8NFJX6CD20MEA10J761R` remains the parent, and done tickets `06F0ME976PM5455JK04S6GPNNW`, `06F0ME9PM8KXH3VP59TQR0ETA8`, and `06F0MEA1FF743S14XQW02H4A3W` remain the established upstream dependency context.
- Keep the current downstream relation unchanged: this ticket still blocks `06F0MEDBFZ25YA1M7RJ71Z7ZCM` for runnable quickstart examples.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Prove the new fluent Code-First API produces the same schema semantics as the v0.5 metadata-first API for the covered hub, link, and satellite baseline.

## Scope In

- Schema parity tests for SQLite and provider-profile projections.
- Migration-model inspection tests where useful without requiring every database server locally.
- Regression coverage for naming collisions and provider capability profile effects.

## Scope Out

- Full runtime integration tests for every external database.
- Read/write helper behavior.

## Acceptance Criteria

- Code-First and metadata-first declarations produce equivalent table, column, key, and index shapes.
- Provider-specific capability profile differences remain explicit in tests.
- Tests fail on accidental drift between Code-First and metadata-first behavior.