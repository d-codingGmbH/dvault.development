[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket is close, but it assumes a SQL Server optimized save path that the current repo does not provide yet and it does not explicitly resolve the repo's current SQL Server validation-posture docs.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NAWNDDEP32P497E39MQXR/description.md has '## Open Questions - none' and its second acceptance criterion requires the SQL Server smoke lane to exercise the provider-optimized explicit save flow.
- src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs only calls services.AddDVault(); it does not register a SQL Server IDataVaultProviderSaveStrategy.
- tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs asserts services.AddDVaultSqlServer() with expectProviderStrategy: false, which confirms the current SQL Server package is still fallback-only.
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj conditionally references Npgsql through DVAULT_TEST_POSTGRES_CONNECTION_STRING, but it has no SQL Server EF Core package reference and no SQL Server provider project reference today.
- tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs and PostgresIntegrationTestConfigurationTests.cs show the existing opt-in pattern the ticket wants to mirror: FromEnvironment discovery, IsConfigured, trimmed ConnectionString, and a deterministic MissingConfigurationSkipMessage.
- README.md says Category=ProviderIntegration.ExternalOptIn is currently Postgres only and provides only Postgres opt-in commands; docs/architecture/dvault-v1-explicit-save-service.md says SQL Server validation is ProviderSmoke.Default and AddDVaultSqlServer() is compatibility-only in v0.5.
- tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs currently hard-codes PostgresDataVaultSchemaTests as the external opt-in live-provider case and does not include any SQL Server integration class.
- git log --oneline -n 5 ticket/06EZ0NAWNDDEP32P497E39MQXR-task-add-sql-server-opt-in-integration-configura shows only PO/PO-critic claim and handoff commits at the branch head, and git diff --stat develop...ticket/06EZ0NAWNDDEP32P497E39MQXR-task-add-sql-server-opt-in-integration-configura reports only .gicket files changed, so this branch does not already contain the missing SQL Server strategy work.

Blocking findings
- The ticket requires smoke tests against an optimized SQL Server save path, but the observed repo still exposes SQL Server only as a fallback compatibility registration and a separate sibling task, 06EZ0NAMGKJ63WCXAK1J7B08TR, owns strategy implementation. The ticket does not say whether this test task is blocked on that sibling, should wait for it, or should absorb part of that scope.
- The contract implicitly moves SQL Server from the current documented ProviderSmoke.Default posture to ProviderIntegration.ExternalOptIn, but it does not explicitly resolve that change against the existing README, architecture matrix, and provider-category discovery baseline. That leaves the intended validation posture ambiguous at developer handoff.

Required PO actions
- Make the dependency/sequence explicit: either add a blocking relation to 06EZ0NAMGKJ63WCXAK1J7B08TR, or broaden this ticket so it intentionally includes the SQL Server strategy work needed to satisfy the optimized-path acceptance criterion.
- State explicitly that SQL Server is intended to join ProviderIntegration.ExternalOptIn and identify the source-of-truth surfaces that must change with this ticket, at minimum README.md, docs/architecture/dvault-v1-explicit-save-service.md, and tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs.
- Pin the opt-in contract at ticket level: define the expected SQL Server environment-variable name, the representative run command/filter, and whether the integration project should mirror the Postgres conditional-provider-package/reflection pattern to keep default runs clean.

Open issues ledger
- critic-item-1 [required-po-action] Make the dependency/sequence explicit: either add a blocking relation to 06EZ0NAMGKJ63WCXAK1J7B08TR, or broaden this ticket so it intentionally includes the SQL Server strategy work needed to satisfy the optimized-path acceptance criterion.
- critic-item-2 [required-po-action] State explicitly that SQL Server is intended to join ProviderIntegration.ExternalOptIn and identify the source-of-truth surfaces that must change with this ticket, at minimum README.md, docs/architecture/dvault-v1-explicit-save-service.md, and tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs.
- critic-item-3 [required-po-action] Pin the opt-in contract at ticket level: define the expected SQL Server environment-variable name, the representative run command/filter, and whether the integration project should mirror the Postgres conditional-provider-package/reflection pattern to keep default runs clean.
- critic-item-4 [blocking-finding] The ticket requires smoke tests against an optimized SQL Server save path, but the observed repo still exposes SQL Server only as a fallback compatibility registration and a separate sibling task, 06EZ0NAMGKJ63WCXAK1J7B08TR, owns strategy implementation. The ticket does not say whether this test task is blocked on that sibling, should wait for it, or should absorb part of that scope.
- critic-item-5 [blocking-finding] The contract implicitly moves SQL Server from the current documented ProviderSmoke.Default posture to ProviderIntegration.ExternalOptIn, but it does not explicitly resolve that change against the existing README, architecture matrix, and provider-category discovery baseline. That leaves the intended validation posture ambiguous at developer handoff.

Missing examples / edge cases
- There is no concrete SQL Server opt-in command example analogous to the README Postgres examples.
- There is no explicit edge case for 'configuration is present but the SQL Server EF Core provider package is not available' if the implementation mirrors the Postgres conditional package-loading pattern.
- There is no explicit example of how the configured-path smoke proves that the optimized SQL Server strategy was selected instead of the provider-neutral fallback writer.

Risky assumptions
- Assumes the SQL Server optimized strategy will already exist by the time this ticket is implemented, even though the separate implementation task is still unrefined and todo.
- Assumes adding SQL Server live integration coverage is acceptable for the current v0.5 validation matrix even though the checked-in docs still describe SQL Server as default smoke only.
- Assumes a developer-managed SQL Server instance can be documented generically without pinning version, auth mode, or LocalDB/container expectations strongly enough to avoid environment-specific failures.

AC / test suggestions
- Add an acceptance criterion that the configured SQL Server smoke lane proves a compatible IDataVaultProviderSaveStrategy is selected for AddDVaultSqlServer(), not just that IDataVaultSaveService writes succeed.
- Add a ticket-level example for running only the SQL Server opt-in tests with the intended environment variable and trait filter.
- If the project mirrors the Postgres conditional dependency pattern, add a negative-path expectation for the provider-package-missing case so the failure mode stays a clean skip rather than a loader error.

Implementation watchouts
- The integration csproj currently only carries SQLite plus conditional Npgsql wiring; SQL Server opt-in likely needs a new conditional provider dependency path and possibly a reflection helper to preserve the default local test surface.
- ProviderIntegrationCategoryDiscoveryTests hard-codes the full public integration test class set, so any new SQL Server classes will need coordinated discovery-baseline updates.
- README.md and docs/architecture/dvault-v1-explicit-save-service.md currently document SQL Server as compatibility-only default smoke, so documentation drift will occur unless those surfaces are updated together with the tests.

Non-blocking notes
- The persisted contract itself is internally complete on Open Questions: .gicket/tickets/06EZ0NAWNDDEP32P497E39MQXR/description.md says '## Open Questions - none'.
- The current ticket comment history is workflow/bot-only; there is no extra human clarification in comments that resolves the dependency or validation-posture gaps.

Split recommendations
- Keep this as a separate test/config task once the dependency on 06EZ0NAMGKJ63WCXAK1J7B08TR is made explicit; no further split is needed.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment