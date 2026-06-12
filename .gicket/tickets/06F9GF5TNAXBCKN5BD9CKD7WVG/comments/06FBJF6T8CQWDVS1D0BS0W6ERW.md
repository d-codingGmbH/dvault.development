[gicket-bot] PO refinement contract

Summary
- Repository and ticket evidence already bound this story to the six built-in provider capability profiles; the v1 binary mapping baseline is SQLite BLOB, Oracle RAW(n), PostgreSQL bytea, SQL Server varbinary(n), DB2 VARBINARY(n), and MySQL varbinary(n), with no blocking PO questions.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence ratifies the v1 provider-mapping baseline instead of reopening the original draft wording: SQLite uses BLOB, Oracle uses RAW(n), PostgreSQL uses bytea, SQL Server uses varbinary(n), DB2 uses VARBINARY(n), and MySQL uses varbinary(n), sized by the active stable-hash digest byte length except PostgreSQL bytea.
- The visible built-in provider baseline remains finite and exact: sqlite-v1, oracle-v1, postgres-v1, sqlserver-v1, db2-v1, and mysql-pomelo-v1 selected from exact provider-name registration.
- The done predecessor story 06F9GF5N4N3Q685XQPKTM5EC00 already owns provider-neutral lowercase-hex string to byte conversion; this story is limited to provider-specific store-type projection and the downstream task 06F9GF60BKEW0CC9FCZRPVX0SR remains the owner of schema/save/read integration coverage.
- No child-ticket, relation, attachment, description, or planning-document writes were materialized in this refinement run.

Scope In
- Provider-specific Binary hash-key and participant-reference store-type projection in DataVault provider capability profiles for the six built-in provider profiles.
- Sizing each Binary mapping by the active stable-hash digest byte length while keeping the model CLR and public hash-key boundary as canonical lowercase hexadecimal string values.
- Flowing provider profile, store type, value format, storage profile, algorithmId, digestByteLength, digestEncoding, and conversionBehavior facts into EF metadata, explain/support-bundle diagnostics, and migration/preflight guardrail surfaces.
- Deterministic diagnostics for unresolved provider capability selection through the existing capability-profile-defaulted and provider-behavior-defaulted warning surfaces rather than silent new provider-specific claims.

Scope Out
- Changing public or EF CLR hash-key values from string to byte[] or revisiting the provider-neutral converter implemented in 06F9GF5N4N3Q685XQPKTM5EC00.
- HashDiff storage changes, provider-side SQL hashing, or caller migration/backfill/dual-write tooling.
- End-to-end schema, save, and read integration coverage across providers, owned by 06F9GF60BKEW0CC9FCZRPVX0SR.
- DB2 live-schema reader parity; current contract keeps DB2 live-schema reads on the existing unsupported-provider path.

Open questions
- none

Follow-up questions
- Should a later optimization ticket justify provider-specific fixed-length binary(n) or DB2 FOR BIT DATA variants if benchmarks or vendor constraints show material benefit over the current varbinary baseline?
- After this story lands, 06F9GF60BKEW0CC9FCZRPVX0SR should confirm end-to-end schema generation, save paths, and latest/as-of/PIT/bridge read behavior across the supported-provider baseline.
- Should later work add DB2 live-schema reader parity so runtime drift checks can validate the persisted Binary store type against catalog metadata instead of the current unsupported-provider outcome?

Risks
- If callers rely on implicit provider fallback instead of a resolved built-in or registered provider profile, SQLite-default capability selection could be mistaken for a provider-specific guarantee unless the existing defaulted diagnostics warnings remain visible.
- DB2 parity is only partially proven at the repository level for this story because the live-schema reader intentionally remains unsupported, so full drift verification for DB2 stays outside the current scope.

Split recommendations
- No further split recommended; the current story is already bounded between done provider-neutral conversion work in 06F9GF5N4N3Q685XQPKTM5EC00 and downstream integration coverage in 06F9GF60BKEW0CC9FCZRPVX0SR.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment