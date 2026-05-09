[gicket-bot] PO refinement contract

Summary
- Verified the attached parity-child addendum, current repository test baselines, and live ticket relations; no new split or relation cleanup is needed, and the ticket is ready for PO-critic.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- `docs/plans/06F0MEAD1BAA5QEVM3F9QJA38G-fluent-parity-child-boundary.md` is already attached to this ticket and remains the authoritative supplement to the short ticket draft.
- The repository already contains the bounded fluent projection work from done tickets `06F0ME9PM8KXH3VP59TQR0ETA8` and `06F0MEA1FF743S14XQW02H4A3W`, plus focused translator-level parity tests in `DataVaultCodeFirstMetadataTranslationTests` and `DataVaultCodeFirstLinkTests`; this child owns the broader schema and provider-profile parity layer on top of that baseline.
- Live relations are consistent as-is: story `06F0ME8NFJX6CD20MEA10J761R` is the parent, done tasks `06F0ME976PM5455JK04S6GPNNW`, `06F0ME9PM8KXH3VP59TQR0ETA8`, and `06F0MEA1FF743S14XQW02H4A3W` are completed upstream dependency context, and this ticket still blocks `06F0MEDBFZ25YA1M7RJ71Z7ZCM`.
- No new child tickets, planning documents, attachments, or relation updates were materialized during this refinement.

Scope In
- Add parity tests that compare metadata-first and code-first output for the bounded fluent baseline: hub, ordinary hub-parent satellite, hub-parent multi-active satellite with ordered `DrivingKey(...)`, and link relationship projection.
- Use SQLite schema creation or create-script inspection to prove code-first and metadata-first declarations generate the same table, column, primary-key, and secondary-index shape for the covered baseline.
- Add provider-profile parity assertions for the finite built-in profiles already present in the repository: `Sqlite`, `Oracle`, `Postgres`, `SqlServer`, and `MySql`.
- Add model-level or relational-metadata inspection tests that keep naming collisions, identifier truncation, included-index behavior, load-timestamp storage, and provider capability profile differences explicit.
- Fail on accidental drift in canonical declaration ordering, especially business keys, link participants, payload members, and driving-key columns.

Scope Out
- Implementing or changing the fluent builders themselves; that projection work is already owned by the completed sibling tickets.
- Link-parent satellites, PIT, bridge, save-service, typed save/read helpers, and any other shapes outside the bounded parent fluent contract.
- Running full integration coverage against every external database server.
- Introducing checked-in EF migration files or a new repository-wide migration infrastructure baseline; this ticket uses inspection-style parity tests instead.

Open questions
- none

Follow-up questions
- If the fluent surface later expands to link-parent satellites, should this parity matrix be extended in a separate ticket rather than reopening this bounded child?
- After this parity layer lands, does the team want a future ticket that scaffolds actual EF migration artifacts, or is relational-model and create-script parity sufficient for v0.6?
- Once non-SQLite CI infrastructure exists, should one or more live provider smoke tests be added on top of the provider-profile inspection matrix?

Risks
- If parity coverage only checks translator-level metadata and never exercises SQLite schema creation, EF relational-name or index-order drift can slip through.
- If any built-in provider profile is omitted from the inspection matrix, provider-specific behaviors such as Oracle primary-key-covered indexes or MySQL identifier limits can regress unnoticed.
- If the code-first and metadata-first assertions share too much normalization logic, the test suite can produce false positives and miss real schema divergence.

Split recommendations
- No new split is required. Keep the existing parent and sibling dependency structure: `06F0ME8NFJX6CD20MEA10J761R` remains the parent, and done tickets `06F0ME976PM5455JK04S6GPNNW`, `06F0ME9PM8KXH3VP59TQR0ETA8`, and `06F0MEA1FF743S14XQW02H4A3W` remain the established upstream dependency context.
- Keep the current downstream relation unchanged: this ticket still blocks `06F0MEDBFZ25YA1M7RJ71Z7ZCM` for runnable quickstart examples.

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