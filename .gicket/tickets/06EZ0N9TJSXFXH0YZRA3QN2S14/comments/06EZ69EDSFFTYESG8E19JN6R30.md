[gicket-bot] PO-critic review contract

Summary
- Persisted contract, ticket relations, and repository evidence are aligned; the story is ready for developer workflow with no unresolved PO questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0N9TJSXFXH0YZRA3QN2S14/description.md contains the authoritative delivery contract and its persisted `## Open Questions` section is `- none`.
- Relation files .gicket/relations/14/ZW/06EZ0N9TJSXFXH0YZRA3QN2S14--06EZ0NA180RA0FQ64KXQTHEVZW--parentOf.json and .gicket/relations/14/7M/06EZ0N9TJSXFXH0YZRA3QN2S14--06EZ0NA7CWDYJ7ZS3K5GM0187M--parentOf.json persist the existing implementation and integration split; blocker relations are persisted in .gicket/relations/VR/14/06EZ0N8HW9PZAFKMM5WQD564VR--06EZ0N9TJSXFXH0YZRA3QN2S14--blocks.json and .gicket/relations/28/14/06EZ0N9AM9AJ3AB8DQ6Y1JBS28--06EZ0N9TJSXFXH0YZRA3QN2S14--blocks.json.
- `git log --oneline --decorate develop --grep '06EZ0NA180RA0FQ64KXQTHEVZW\|06EZ0NA7CWDYJ7ZS3K5GM0187M' -n 10` returned `d040b552 [06EZ0NA180RA0FQ64KXQTHEVZW] AUTO-INTEGRATION squash into develop` and `df307cf9 [06EZ0NA7CWDYJ7ZS3K5GM0187M] AUTO-INTEGRATION squash into develop`.
- `git diff --name-only develop..HEAD -- . ':(exclude).gicket/**'` returned no paths, so the story branch carries ticket metadata only and no unmerged repository source/doc changes.
- src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:15-20 directly registers `IDataVaultProviderSaveStrategy` through `AddDVaultPostgres()`, and src/DCoding.Data.DVault.Postgres/PostgresDataVaultSaveStrategy.cs:19-26 gates the optimized path to clean `Npgsql.EntityFrameworkCore.PostgreSQL` contexts.
- src/DCoding.Data.DVault/DataVaultSaveService.cs:401-414 shows the core dispatcher evaluates registered provider strategies first and falls back to the provider-neutral writer when none accepts the request batch.
- src/DCoding.Data.DVault.Postgres/PostgresDataVaultSaveStrategy.cs:268-332 loads latest satellite hash diffs and suppresses unchanged satellite writes, and :422-458 builds PostgreSQL `INSERT ... ON CONFLICT (...) DO NOTHING` statements for set-based inserts.
- tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:47-52 provides `ProviderSmoke.Default` registration proof for `AddDVaultPostgres()`.
- tests/DCoding.Data.DVault.Tests/Integration/PostgresOptimizedDataVaultSaveServiceTests.cs:10-25 and :67-199 cover opt-in PostgreSQL live validation for hub, link, unchanged-satellite, and changed-satellite behavior and assert no fallback-tracked rows; gating is aligned by tests/DCoding.Data.DVault.Tests/Shared/ProviderTestCategories.cs:7-12, tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs:4-7, tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:16-18, README.md:183-220, and docs/architecture/dvault-v1-explicit-save-service.md:43-55.
- benchmarks/DCoding.Data.DVault.Benchmarks/README.md:3-37 and README.md:195-203 explicitly keep benchmark coverage SQLite-only, matching docs/architecture/dvault-v1-explicit-save-service.md:47-55.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Live PostgreSQL proof remains opt-in and depends on a developer-managed `DVAULT_TEST_POSTGRES_CONNECTION_STRING`; default unattended local validation does not execute that path.
- The legacy draft still mentions PostgreSQL benchmark evidence, so humans must continue treating the persisted contract and architecture matrix as authoritative for the bounded release scope.

AC / test suggestions
- If product later wants measured PostgreSQL-versus-fallback performance evidence, track it as a separate benchmark follow-up ticket instead of widening this story.
- If future provider-optimization stories need stronger fallback observability, reuse the current explicit pattern of strategy compatibility plus `no fallback-tracked rows` as the acceptance proof surface.

Implementation watchouts
- Keep `AddDVaultPostgres()` registration and `CanSave` self-gating aligned so dirty or incompatible contexts continue to fall back through `DefaultDataVaultSaveService`.
- Preserve the `ProviderSmoke.Default` versus `ProviderIntegration.ExternalOptIn` split so default `dotnet test` execution stays free of mandatory PostgreSQL/Npgsql provisioning.

Non-blocking notes
- The persisted comment history under .gicket/tickets/06EZ0N9TJSXFXH0YZRA3QN2S14/comments/ is automation and PO-handoff only; no later comment re-opened product questions after the refinement contract was published.

Split recommendations
- No new split is needed; the existing `parentOf` children 06EZ0NA180RA0FQ64KXQTHEVZW and 06EZ0NA7CWDYJ7ZS3K5GM0187M already cover implementation and opt-in integration.
- If benchmark evidence becomes a release requirement later, create a dedicated PostgreSQL benchmark follow-up instead of reopening this story or widening the current children.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment