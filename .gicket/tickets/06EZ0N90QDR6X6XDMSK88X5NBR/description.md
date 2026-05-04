<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified repository and ticket evidence: SQLite is the only current v0.5 optimization target with an optimized save strategy, required local integration coverage, and benchmark coverage; PostgreSQL has opt-in external validation only; SQL Server, Oracle, and MySQL remain compatibility-baseline rows. No split is recommended.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The matrix should name the compatibility baseline as the core provider-neutral AddDVault()/IDataVaultSaveService path without a provider-specific save strategy.
- SQLite is the only visible provider-specific optimization baseline in the current branch: DataVaultProviderCapabilityProfiles.Sqlite exists, AddDVaultSqlite() registers SqliteDataVaultSaveStrategy, and the benchmark plus required-local integration suites target SQLite.
- PostgreSQL already has an opt-in external database validation path through PostgresDataVaultSchemaTests and DVAULT_TEST_POSTGRES_CONNECTION_STRING; document that as external validation, not required local validation.
- SQL Server, Oracle, and MySQL packages currently register only the core DVault service and no provider save strategy, so their v0.5 rows should be documented as compatibility-baseline-only.
- Verified relation context: this ticket is a child of 06EZ0N8HW9PZAFKMM5WQD564VR and currently blocks 06EZ0N9AM9AJ3AB8DQ6Y1JBS28.

### Scope In
- Produce one concise v0.5 capability matrix covering SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- State for each provider whether optimized insert-only save behavior is required in this release.
- State for each provider whether set-based existence checks are required in this release.
- State for each provider whether validation is required locally, available only as opt-in external database validation, or not required in this release.
- State for each provider whether benchmark coverage is required in this release.
- Explain that providers without a provider-specific strategy inherit the compatibility baseline.

### Scope Out
- Implementing new provider strategies, capability profiles, or persistence behavior.
- Adding new SQL Server, Oracle, or MySQL integration harnesses or benchmark baselines in this ticket.
- Changing the explicit save-service architecture, concurrency semantics, or package-family release policy.
- Expanding the matrix beyond SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.

## Acceptance Criteria
- The document contains one matrix row each for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- The document identifies the compatibility baseline as the core provider-neutral AddDVault()/IDataVaultSaveService path without a provider-specific save strategy.
- The SQLite row marks optimized insert-only save behavior and set-based existence checks as the only provider-specific optimization capabilities required in v0.5, and it marks integration plus benchmark coverage as required local validation.
- The PostgreSQL row marks provider-specific optimization capabilities as not required in v0.5 and marks validation as opt-in external database coverage rather than required local coverage.
- The SQL Server, Oracle, and MySQL rows mark optimized insert-only save behavior, set-based existence checks, integration validation, and benchmark coverage as not required in v0.5, with compatibility baseline only.
- The document explicitly separates required local validation from opt-in external database validation and does not imply that non-SQLite providers must ship provider-specific optimizations in this release.

## Definition of Done
- A repository document or ticket refinement artifact records the provider matrix with the five required providers and the compatibility-baseline label.
- The wording aligns with current repository evidence: SQLite required-local benchmark and integration coverage, PostgreSQL external opt-in validation, and SQL Server/Oracle/MySQL core-service-only baseline coverage.
- The document stays concise and release-scoped, without reopening broader provider roadmap decisions that are outside v0.5.

## Implementation Notes
- Use provider names exactly as SQLite, PostgreSQL, SQL Server, Oracle, and MySQL to stay aligned with current package and test naming.
- Use the existing test category vocabulary as the validation legend: ProviderIntegration.RequiredLocal, ProviderIntegration.ExternalOptIn, and ProviderSmoke.Default.
- Source the matrix from current repository evidence: DataVaultProviderCapabilityProfiles.Sqlite, AddDVaultSqlite(), the provider service-registration extension packages, ProviderIntegrationCategoryDiscoveryTests, BenchmarkScenarioExecutionTests, and docs/architecture/dvault-v1-explicit-save-service.md.
- For SQL Server, Oracle, and MySQL, describe the release expectation as compatibility baseline only rather than unsupported, because their provider packages already exist and register the core DVault service.
- Benchmark notes should stay SQLite-specific because the benchmark runner uses SQLite temporary files and explicitly states that Postgres, Docker, and other external services are not required.

## Open Questions
- none

## Follow-Up Questions
- When provider-specific optimization work resumes after v0.5, which non-SQLite provider should receive the next external-validation or optimization ticket first.
- If additional provider strategies are added later, should a future matrix split hub/link insert optimization and satellite existence-check optimization into separate sub-capabilities.

## Risks
- If a non-SQLite provider strategy or new integration harness lands before this document ships, the matrix will need a quick recheck against the updated registrations and test categories.
- The current repository proves external database validation only for PostgreSQL; documenting broader external-validation expectations for SQL Server, Oracle, or MySQL would overpromise.
- Benchmark evidence is intentionally SQLite-only and machine-context-specific, so the matrix must not generalize those timings or coverage claims to other providers.

## Split Recommendations
- No split recommended; current evidence supports one bounded documentation ticket because the provider matrix can be derived from the existing provider packages, tests, and benchmark surfaces.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: create a concise provider capability matrix for the v0.5 optimization work.

Acceptance Criteria:
- The matrix covers SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- Each provider entry states whether optimized insert-only save behavior, set-based existence checks, integration coverage, and benchmark coverage are required in this release.
- The document distinguishes required local validation from opt-in external database validation.
- The document names the provider-neutral fallback as the compatibility baseline.