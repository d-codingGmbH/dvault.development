[gicket-bot] PO refinement contract

Summary
- Refined the live-schema drift task to a SQLite-first, provider-neutral schema snapshot abstraction with deterministic drift reporting, explicit unsupported-provider handling, and bounded optional external-provider evidence.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This task is only the live-schema branch of story 06F1XPWB8DZR4J8EZ00V8DT25G; sibling task 06F1XPWNAWWMDBRK315S66P7AM owns the EF ModelSnapshot adapter and stays out of scope here.
- Blocking prerequisite stories 06F1XPVPKVGYKCV04PY98TSS78 and 06F1XPS7KGKBP5SVMQPJC49J2G are already done, so this ticket can rely on the existing design-time workflow boundary and stable diagnostic-code conventions.
- Current branch evidence already has design-time-only drift via DataVaultModelDriftReporter.Compare; this ticket adds optional live-database schema introspection without reopening or replacing that design-time baseline.
- SQLite is the v1 supported-provider default because the repository already has required-local schema evidence and reusable test fixtures, while Postgres, SQL Server, Oracle, and MySQL remain opt-in external lanes or documented no-support results in this slice.
- If optional external-provider evidence is added, reuse the existing connection-string contracts DVAULT_TEST_POSTGRES_CONNECTION_STRING, DVAULT_TEST_SQLSERVER_CONNECTION_STRING, DVAULT_TEST_ORACLE_CONNECTION_STRING, and DVAULT_TEST_MYSQL_CONNECTION_STRING rather than inventing new configuration keys.
- The bounded v1 live-schema contract should match the DVault physical surface the source currently emits today: tables, ordered columns, named primary-key constraints, and secondary indexes; foreign-key graph diffing is not part of this ticket because the current core source does not generate DVault foreign keys.

Scope In
- Define a provider-neutral live schema snapshot/reader contract for DVault-owned tables, ordered columns, named primary-key constraints, and secondary indexes.
- Wire the live schema snapshot into drift comparison/reporting so a supported live database can be compared against the expected DVault metadata or artifact baseline.
- Implement one reliable supported provider path in the default local lane, with SQLite as the required v1 baseline.
- Add deterministic tests for the supported provider plus explicit unsupported/unavailable-provider coverage for providers that do not implement the live reader in this slice.
- Update repository documentation to explain the bounded live-schema drift support, provider evidence limits, and existing optional connection-string conventions for any external live-provider lanes.

Scope Out
- No EF ModelSnapshot adapter or ModelSnapshot comparison work; that belongs to sibling task 06F1XPWNAWWMDBRK315S66P7AM.
- No destructive migration, repair, or schema rewrite behavior.
- No full provider-specific SQL diff engine or arbitrary catalog/object comparison outside the DVault-owned schema surface.
- No requirement that Postgres, SQL Server, Oracle, and MySQL all ship first-class live-schema readers in this first slice.
- No expansion into foreign-key graph diffing, arbitrary non-DVault database objects, or workflow automation beyond code, tests, and docs.

Open questions
- none

Follow-up questions
- After the SQLite-first slice lands, should Postgres be the next external provider to graduate to supported live-schema reading because the repository already contains an opt-in information_schema schema test?
- If DVault later begins projecting foreign keys or additional named constraints, should a follow-up ticket widen live-schema comparison beyond the current primary-key-plus-index baseline?
- Once both child tasks under the parent drift story are complete, should README and model-first governance docs be consolidated into one end-to-end public comparison guide covering design-time, ModelSnapshot, and live-schema drift?

Risks
- Provider catalog metadata differs on casing, naming, and ordering, so insufficient normalization could create false drift even when the physical schema is semantically correct.
- If unsupported-provider and unavailable-database outcomes are not distinguished clearly, consumers will not know whether they need a provider implementation or only environment configuration.
- Documentation could overstate support if it implies broad multi-provider live drift coverage before evidence exists beyond the SQLite-first baseline and any explicitly opt-in lanes.
- Allowing this task to expand into general-purpose database diffing or repair behavior would break the bounded child-ticket scope and jeopardize delivery.

Split recommendations
- No split is required for PO-critic readiness; this task is bounded as a SQLite-first live-schema abstraction with explicit unsupported-provider handling and documentation.
- If first-class live readers are later needed for Postgres, SQL Server, Oracle, or MySQL, track each provider or broader constraint-surface expansion in separate follow-up tickets instead of widening this task.

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