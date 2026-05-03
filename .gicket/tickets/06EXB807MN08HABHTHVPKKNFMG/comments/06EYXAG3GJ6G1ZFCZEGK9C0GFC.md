[gicket-bot] PO-critic review contract

Summary
- The ticket contract is grounded in the current DVault test layout, public API surface, and existing child-ticket split, with no unresolved PO questions; it is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB807MN08HABHTHVPKKNFMG/description.md contains '## Open Questions' with the single item 'none' and explicitly keeps child tickets 06EXB80FPE3REH11RQ1YR6BW1G and 06EXB80QQHAYH61RY4X3T1E8S0 as the implementation split.
- README.md lists tests/DCoding.Data.DVault.Tests as 'Unit, integration, and shared test projects for DVault', and find tests/DCoding.Data.DVault.Tests -maxdepth 2 -type d shows Integration, Modeling, Shared, and Unit directories under that root.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs exposes AddDVault and registers IStableHashService, IStableHashNormalizer, and IDataVaultSaveService; src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs exposes AddDVaultSqlite and registers SqliteDataVaultSaveStrategy.
- tests/DCoding.Data.DVault.Tests/Shared/ProviderTestCategories.cs defines ProviderIntegration.RequiredLocal, ProviderIntegration.ExternalOptIn, and ProviderSmoke.Default, and tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs assigns the SQLite integration classes to required-local coverage and PostgresDataVaultSchemaTests to external opt-in coverage.
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj references Microsoft.EntityFrameworkCore.Sqlite unconditionally and Npgsql.EntityFrameworkCore.PostgreSQL only when DVAULT_TEST_POSTGRES_CONNECTION_STRING is set; tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs publishes the missing-configuration skip message and PostgresDataVaultSchemaTests.cs calls Assert.Skip when configuration is absent.
- tests/DCoding.Data.DVault.Tests/Unit/TestDiscoverySmokeTests.cs names DataVaultEfMetadataTranslationTests, DataVaultModelBuilderExtensionsTests, StableHashNormalizerTests, StableHashServiceTests, TechnicalMetadataColumnContractTests, ExplicitDataVaultSaveServiceTests, and DataVaultProviderCapabilityProfileTests as the fast coverage surface, while tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs includes AddDVault fallback and AddDVaultSqlite optimized SQLite coverage.
- git log --oneline --decorate --grep '06EXB80FPE3REH11RQ1YR6BW1G|06EXB80QQHAYH61RY4X3T1E8S0|06EXB807MN08HABHTHVPKKNFMG' shows develop already contains c0f6bec2 for 06EXB80FPE3REH11RQ1YR6BW1G and 3e3bf4a2 for 06EXB80QQHAYH61RY4X3T1E8S0, while this parent branch adds only workflow/ticket commits; git diff --name-only develop..HEAD lists only .gicket ticket/comment/event files and no src/, tests/, docs/, or DVault.slnx changes.
- rg -n '06EXB807MN08HABHTHVPKKNFMG|06EXB80FPE3REH11RQ1YR6BW1G|06EXB80QQHAYH61RY4X3T1E8S0|06EXB8202A88KJJP7WEGBESBYM' .gicket shows parentOf relations from 06EXB807MN08HABHTHVPKKNFMG to both child tickets and a blocks relation from 06EXB807MN08HABHTHVPKKNFMG to 06EXB8202A88KJJP7WEGBESBYM.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A short explicit example of the default repository test invocation via DVault.slnx versus an opt-in Postgres run using DVAULT_TEST_POSTGRES_CONNECTION_STRING would reduce interpretation drift.
- If future SQL Server, Oracle, or MySQL live-database harnesses are introduced, the ticket set should add an explicit example of how their opt-in configuration and skip behavior mirror or intentionally differ from the current Postgres template.

Risky assumptions
- The story assumes SQL Server, Oracle, and MySQL can remain smoke-only in v1; current repository evidence shows a configured external-provider path only for Postgres.
- The contract assumes project organization plus trait/category discovery is sufficient documentation for default-versus-opt-in behavior; downstream CI or release-gate work may still need explicit invocation guidance.

AC / test suggestions
- Keep closure evidence tied to concrete surfaces already present in the repo: DVault.slnx as the root test entry point, tests/DCoding.Data.DVault.Tests/Shared/ProviderTestCategories.cs for category names, and DVAULT_TEST_POSTGRES_CONNECTION_STRING for opt-in Postgres coverage.
- When the story is closed, verify that AddDVault fallback coverage and AddDVaultSqlite optimized coverage remain visible through named tests or documented discovery assertions rather than only being implied by project layout.

Implementation watchouts
- Do not widen the default run to require external services or unconditional external-provider package loading beyond the current conditional Npgsql pattern in tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj.
- Preserve the existing child ownership split: 06EXB80FPE3REH11RQ1YR6BW1G for unit-group coverage and 06EXB80QQHAYH61RY4X3T1E8S0 for provider-integration category boundaries.
- Keep shared taxonomy helpers under tests/DCoding.Data.DVault.Tests/Shared instead of inventing a parallel classification surface.

Non-blocking notes
- The current parent branch is effectively a ticket-state branch on top of develop, so the substantive repository baseline for this story is already represented by the child-ticket integrations on develop.

Split recommendations
- No additional split is needed; the existing parentOf links to 06EXB80FPE3REH11RQ1YR6BW1G and 06EXB80QQHAYH61RY4X3T1E8S0 and the blocks link to 06EXB8202A88KJJP7WEGBESBYM already match the repository and workflow evidence.
- Reserve any future SQL Server, Oracle, or MySQL live-database harness work for separate tickets rather than broadening this parent story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment