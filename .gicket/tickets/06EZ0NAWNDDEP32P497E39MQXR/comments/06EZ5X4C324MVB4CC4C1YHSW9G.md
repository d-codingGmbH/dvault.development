[gicket-bot] PO-critic review contract

Summary
- Contract is explicit enough for developer handoff; remaining risk is dependency-state alignment between the ticket branch, develop, and sibling ticket metadata.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NAWNDDEP32P497E39MQXR/description.md now states the dependency on 06EZ0NAMGKJ63WCXAK1J7B08TR, pins DVAULT_TEST_SQLSERVER_CONNECTION_STRING, pins the repo-root command dotnet test DVault.slnx --filter FullyQualifiedName~SqlServer, names README.md, docs/architecture/dvault-v1-explicit-save-service.md, and tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs as required source-of-truth updates, and says Open Questions: none.
- .gicket/tickets/06EZ0NAWNDDEP32P497E39MQXR/comments/06EZ5A2ERGM235TG245XDPX984.md marks prior critic items 1-5 as answered, including the downstream dependency, ExternalOptIn classification, env var, run command, and required documentation/test surfaces.
- tests/DCoding.Data.DVault.Tests/Shared/ProviderTestCategories.cs already defines ProviderIntegration.ExternalOptIn, ProviderSmoke.Default, and SqlServerProvider, and tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs, PostgresIntegrationTestConfigurationTests.cs, PostgresDataVaultSchemaTests.cs, and NpgsqlProviderReflection.cs show the exact opt-in pattern the contract cites: FromEnvironment discovery, trimmed connection string, deterministic skip message, Assert.Skip, reflection loading, and conditional external-provider use.
- src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs directly defines IDataVaultProviderSaveStrategy and DataVaultProviderSaveStrategyContext, and src/DCoding.Data.DVault/DataVaultSaveService.cs orders registered strategies by Priority and dispatches them before falling back, so the shared optimization boundary named in the ticket exists in source.
- The current ticket branch head 31a38307804e94721a4bb57019f1ca7665bc53c6 does not yet include the prerequisite SQL Server implementation on this branch: src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs only calls services.AddDVault(), and tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj currently has SQLite plus conditional Npgsql only, with no SQL Server provider wiring.
- Branch-history and develop source prove the dependency exists elsewhere in the repository: git show --stat a8bd81f7a98f1a2ffa63884883873c43dc10b581 is [06EZ0NAMGKJ63WCXAK1J7B08TR] AUTO-INTEGRATION squash into develop, and git grep develop finds src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultSaveStrategy.cs plus AddDVaultSqlServer() registration there.
- Current repo guidance still matches the ticket's required doc-update scope: README.md says ProviderIntegration.ExternalOptIn is currently Postgres-only and docs/architecture/dvault-v1-explicit-save-service.md still lists SQL Server as ProviderSmoke.Default compatibility-only.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not explicitly call out the configured-but-provider-assembly-missing skip case for SQL Server, even though NpgsqlProviderReflection.cs shows that negative path for Postgres today.
- The required command uses a name filter (FullyQualifiedName~SqlServer); the ticket does not include a category/provider-filtered companion example if future SQL Server-named unit or config tests broaden the match set.

Risky assumptions
- Assumes implementation starts from a branch rebased or merged with develop; the current ticket branch head does not yet contain the prerequisite SQL Server strategy code.
- Assumes FullyQualifiedName~SqlServer remains the intended long-term validation selector as the SQL Server test surface grows.

AC / test suggestions
- Keep one explicit acceptance proof that the configured SQL Server lane used the optimized provider strategy from 06EZ0NAMGKJ63WCXAK1J7B08TR, not merely a successful provider-neutral save.
- If the SQL Server lane mirrors the Postgres conditional package-loading pattern, keep a clean skip expectation for both missing configuration and configured-but-provider-missing cases.

Implementation watchouts
- Before this ticket can exercise the optimized path, the working branch will need the SQL Server strategy changes that are on develop but not on the current ticket branch head.
- tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs hard-codes the full public integration test class inventory, so SQL Server classes and traits must be updated in lockstep with the new lane.
- README.md and docs/architecture/dvault-v1-explicit-save-service.md currently document SQL Server as fallback/default-smoke only; the documentation and discovery-baseline updates need to land together or the repo will be internally contradictory.
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj currently only conditionally wires Npgsql; keeping default runs clean requires the SQL Server lane to follow the same opt-in loading pattern rather than unconditional external-provider wiring.

Non-blocking notes
- .gicket/tickets/06EZ0NAWNDDEP32P497E39MQXR/ticket.json still carries blocked/dev, blocked/test, and critic-needed from the earlier blocking review; those are workflow-state leftovers rather than new contract gaps.
- The latest PO refinement comment explicitly notes that a persisted blocks relation write was trust-blocked, so the dependency is carried in the durable contract text rather than as a stored ticket relation.

Split recommendations
- No new split. Keep SQL Server strategy implementation in 06EZ0NAMGKJ63WCXAK1J7B08TR and keep this ticket focused on opt-in configuration, documentation, category-baseline updates, and three representative smoke scenarios.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment