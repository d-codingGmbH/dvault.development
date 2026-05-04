[gicket-bot] PO refinement contract

Summary
- Verified repository and ticket evidence: SQLite is the only current v0.5 optimization target with an optimized save strategy, required local integration coverage, and benchmark coverage; PostgreSQL has opt-in external validation only; SQL Server, Oracle, and MySQL remain compatibility-baseline rows. No split is recommended.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The matrix should name the compatibility baseline as the core provider-neutral AddDVault()/IDataVaultSaveService path without a provider-specific save strategy.
- SQLite is the only visible provider-specific optimization baseline in the current branch: DataVaultProviderCapabilityProfiles.Sqlite exists, AddDVaultSqlite() registers SqliteDataVaultSaveStrategy, and the benchmark plus required-local integration suites target SQLite.
- PostgreSQL already has an opt-in external database validation path through PostgresDataVaultSchemaTests and DVAULT_TEST_POSTGRES_CONNECTION_STRING; document that as external validation, not required local validation.
- SQL Server, Oracle, and MySQL packages currently register only the core DVault service and no provider save strategy, so their v0.5 rows should be documented as compatibility-baseline-only.
- Verified relation context: this ticket is a child of 06EZ0N8HW9PZAFKMM5WQD564VR and currently blocks 06EZ0N9AM9AJ3AB8DQ6Y1JBS28.

Scope In
- Produce one concise v0.5 capability matrix covering SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- State for each provider whether optimized insert-only save behavior is required in this release.
- State for each provider whether set-based existence checks are required in this release.
- State for each provider whether validation is required locally, available only as opt-in external database validation, or not required in this release.
- State for each provider whether benchmark coverage is required in this release.
- Explain that providers without a provider-specific strategy inherit the compatibility baseline.

Scope Out
- Implementing new provider strategies, capability profiles, or persistence behavior.
- Adding new SQL Server, Oracle, or MySQL integration harnesses or benchmark baselines in this ticket.
- Changing the explicit save-service architecture, concurrency semantics, or package-family release policy.
- Expanding the matrix beyond SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.

Open questions
- none

Follow-up questions
- When provider-specific optimization work resumes after v0.5, which non-SQLite provider should receive the next external-validation or optimization ticket first.
- If additional provider strategies are added later, should a future matrix split hub/link insert optimization and satellite existence-check optimization into separate sub-capabilities.

Risks
- If a non-SQLite provider strategy or new integration harness lands before this document ships, the matrix will need a quick recheck against the updated registrations and test categories.
- The current repository proves external database validation only for PostgreSQL; documenting broader external-validation expectations for SQL Server, Oracle, or MySQL would overpromise.
- Benchmark evidence is intentionally SQLite-only and machine-context-specific, so the matrix must not generalize those timings or coverage claims to other providers.

Split recommendations
- No split recommended; current evidence supports one bounded documentation ticket because the provider matrix can be derived from the existing provider packages, tests, and benchmark surfaces.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment