<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket to concrete opt-in live bulk-provider coverage for Postgres, SQL Server, Oracle, and MySQL using existing repository fixtures and relation context; no child tickets, relation edits, attachments, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The current ticket has no persisted attachments and no substantive ticket comment beyond the bot claim comment, so refinement decisions came from repository and relation evidence rather than missing human follow-up.
- The existing parent relation from story 06F2PGNGVQ3TZZWSABAK5SNFK4 remains correct, and the existing blocks relations from done tickets 06F2PGMSQ4D4FV8W5ZERD4GS8C, 06F2PGN4GPQCGC5WHZQBGP4SD0, and 06F2PGK4QJ0YGXK5479W83Z2J0 remain valid release-ordering context; no relation cleanup was needed.
- Repository evidence already fixes the external-provider baseline: live provider tests use Category=ProviderIntegration.ExternalOptIn, provider traits Postgres / SQL Server / Oracle / MySQL, and the existing DVAULT_TEST_POSTGRES_CONNECTION_STRING, DVAULT_TEST_SQLSERVER_CONNECTION_STRING, DVAULT_TEST_ORACLE_CONNECTION_STRING, and DVAULT_TEST_MYSQL_CONNECTION_STRING opt-in environment variables.
- The repository already contains reusable external-provider infrastructure in ExternalProviderLiveSchemaFixture, ExternalProviderLiveSchemaReaderTests, the provider-specific IntegrationTestConfiguration classes, and ProviderIntegrationCategoryDiscoveryTests; this ticket should extend those lanes to real bulk-save proof instead of inventing a second harness.
- Current live tests prove schema and representative single-save or smoke behavior, but they do not yet give end-to-end proof of the ordered bulk-save path for SQL Server, Oracle, or MySQL, and Postgres still proves repeated single saves instead of the bulk entry point.
- Current gate logic in src/DCoding.Data.DVault/DataVaultDiagnostics.cs is the bounded baseline for eligible native bulk runs: Postgres requires a clean Npgsql context, SQL Server requires a clean context plus at least 50 total operations and at most 500 satellite operations, MySQL requires a clean supported MySQL provider plus at least 50 total operations, Oracle requires a clean Oracle context plus at least 50 total operations, and all four decline multi-active satellite batches.
- No persistent planning action was materialized in this pass: no child ticket was created, no relation was added or removed, no attachment was added, and no planning document was written.

### Scope In
- Live external-provider integration coverage for the existing ordered bulk-save contract behind IDataVaultSaveService.SaveAsync(DbContext, DataVaultBulkSaveRequest) or an equivalent bulk entry path that exercises the same ordered request batch.
- Provider-native bulk proof for Postgres, SQL Server, Oracle, and MySQL using the repository's existing opt-in environment-variable and provider-category conventions.
- Eligible hub, link, and satellite batch coverage that proves provider-native strategy selection rather than provider-neutral fallback when the current gate conditions are satisfied.
- Assertions on observable batch correctness: persisted rows and saved-record results, deterministic ordering, and latest-state satellite HashDiff behavior where the provider strategy supports satellite writes.
- Minimal test-run guidance or README wording updates only when the live-provider coverage shape changes documented commands, filters, or prerequisites.

### Scope Out
- Redesigning the bulk SPI, renaming DataVaultBulkSaveRequest, or changing provider-neutral fallback semantics already owned by 06F2PGN4GPQCGC5WHZQBGP4SD0.
- Implementing new provider-native save-strategy algorithms or changing provider gate thresholds; those stay with story 06F2PGNGVQ3TZZWSABAK5SNFK4.
- Benchmark runs, crossover analysis, or comparative performance reporting; those stay with 06F2PGNZBRNCQ1SV2KKP6F3BA8.
- Checked-in secrets, repository-managed Docker or Podman orchestration, or automatic external database provisioning.
- Multi-active satellite optimized-provider support or other batch shapes that the current gate logic explicitly declines.
- Broad v0.14 documentation and release-note closure beyond any narrow test-guidance edits; that stays with 06F2PGP2B2RZGGK3CVKK5WRRP8.

## Acceptance Criteria
- External opt-in integration tests exercise the ordered bulk-save path itself for each scoped provider lane instead of proving bulk behavior only through repeated single-request saves.
- PostgreSQL coverage proves AddDVaultPostgres on a clean Npgsql-backed context persists an ordered bulk hub, link, and satellite batch through the provider strategy and leaves no fallback-tracked DVault rows.
- SQL Server coverage proves AddDVaultSqlServer on a clean SQL Server context persists an eligible ordered bulk batch that meets the current native gate of at least 50 total operations and no more than 500 satellite operations through the provider strategy rather than the fallback writer.
- MySQL coverage proves AddDVaultMySql on a clean supported MySQL provider context persists an eligible ordered bulk batch of at least 50 total operations through the provider strategy rather than the fallback writer.
- Oracle coverage proves AddDVaultOracle on a clean Oracle context persists an eligible ordered bulk batch of at least 50 total operations through the currently implemented provider strategy rather than only through the existing single-hub smoke lane.
- Each live-provider bulk test validates observable batch correctness for its provider lane: expected row counts and saved-record results, provider-visible hub, link, and satellite data, and latest-state satellite HashDiff suppression when the current strategy path supports satellite writes.
- All new or updated live-provider tests continue to use the existing opt-in contract: deterministic skip when the matching DVAULT_TEST_*_CONNECTION_STRING value is absent, Category=ProviderIntegration.ExternalOptIn, the correct Provider trait, and no checked-in machine-specific configuration.
- If provider test commands or coverage wording change, the affected README and v0.14 documentation text are updated to match the actual opt-in execution path.

## Definition of Done
- The integration project contains external opt-in bulk-provider tests for the scoped providers and those tests pass when their documented local connection-string prerequisites are supplied.
- The live tests reuse the existing provider configuration helpers, schema or table cleanup approach, and provider category discovery conventions instead of adding a second external-provider harness.
- The bulk tests explicitly prove native-strategy execution under eligible conditions and would fail if the provider path silently fell back to the core writer.
- Ticket scope, repository guidance, and relation context remain aligned with the current split: fallback baseline in 06F2PGN4GPQCGC5WHZQBGP4SD0, native strategy implementation in 06F2PGNGVQ3TZZWSABAK5SNFK4, benchmarks in 06F2PGNZBRNCQ1SV2KKP6F3BA8, and broader docs closure in 06F2PGP2B2RZGGK3CVKK5WRRP8.

## Implementation Notes
- Reuse tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderLiveSchemaFixture.cs, the provider-specific IntegrationTestConfiguration files, ProviderIntegrationCategoryDiscoveryTests.cs, and ProviderTestCategories.cs rather than creating a new opt-in infrastructure layer.
- Shape bulk batches to satisfy the current gate evaluator in src/DCoding.Data.DVault/DataVaultDiagnostics.cs: no pending tracked changes, no multi-active satellite operations, SQL Server at least 50 total operations and at most 500 satellite operations, MySQL at least 50 total operations, Oracle at least 50 total operations, and the existing Npgsql provider identity for Postgres.
- Follow the existing optimized-path assertion style from PostgresOptimizedDataVaultSaveServiceTests.cs and the existing live cleanup patterns from the SQL Server, Oracle, and MySQL integration tests so the coverage distinguishes provider-native execution from fallback behavior.
- Keep MySQL on the existing reflection and restore-marker contract described in README.md: the live lane still depends on DVAULT_TEST_MYSQL_CONNECTION_STRING plus the non-secret MSBuild marker used for conditional provider restore.
- Use actual bulk request shapes that exercise mixed hub, link, and satellite batches where the current provider strategy supports them; a loop of single-request SaveAsync calls is not sufficient bulk-provider proof.
- No child ticket was created, no relation was added or removed, no attachment was added, and no planning document was written during this refinement.

## Open Questions
- none

## Follow-Up Questions
- Once bulk coverage lands, should 06F2PGP2B2RZGGK3CVKK5WRRP8 rewrite the SQL Server, Oracle, and MySQL README sections from smoke wording to explicit bulk-lane wording?
- Should benchmark story 06F2PGNZBRNCQ1SV2KKP6F3BA8 mirror the same eligible batch sizes and provider shapes used by the live-provider bulk tests so performance evidence and integration proof stay comparable?
- If later product validation wants external coverage for provider-decline fallback cases or multi-active satellite batches, should that be separate follow-on work instead of widening this ticket?

## Risks
- External-provider tests depend on developer-managed databases, privileges to create and drop temporary schemas or tables, and conditional provider restore markers; weak environment isolation can produce flaky evidence.
- If the live tests use undersized batches or dirty DbContexts, they can accidentally prove only fallback behavior and miss the intended provider-native bulk path.
- Oracle documentation in the repository still contains older hub and link only wording from the v0.5 architecture note, so the new live coverage could diverge from published claims unless the docs ticket reconciles them.
- MySQL coverage spans supported provider names through a reflection helper; bypassing that helper can make the live lane prove the wrong provider combination.

## Split Recommendations
- No additional split is recommended; the existing graph already separates provider-neutral fallback 06F2PGN4GPQCGC5WHZQBGP4SD0, provider-native strategy implementation story 06F2PGNGVQ3TZZWSABAK5SNFK4, this provider integration task, benchmark story 06F2PGNZBRNCQ1SV2KKP6F3BA8, and documentation task 06F2PGP2B2RZGGK3CVKK5WRRP8.
- If later work needs live-provider proof of provider-decline fallback behavior or multi-active satellite rejection, create a follow-on task instead of widening this ticket.
- If documentation work grows beyond narrow execution-guidance updates, keep it on 06F2PGP2B2RZGGK3CVKK5WRRP8 instead of opening another planning split.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Test provider bulk strategies with available containers or opt-in external configuration.

## Scope
- Refine and complete the work for "Add provider bulk integration coverage" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.