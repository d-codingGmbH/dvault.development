[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F1XQ0T5WQWN1AES5Z3E0RMSR/description.md contains `## Open Questions` with `- none` in the persisted delivery contract.
- Attachment manifest records `v0.10.0-adoption-tooling-plan.md` with sha256 d3b8771fa43a75029cc1867a5b4aa8446c4b6d64e073723d6a142ac77d24e4ef; the blob sequence is analyzer package foundation, Testcontainers helper/examples, then production checklist/examples refresh.
- Branch log shows HEAD ae8409041 on `ticket/06F1XQ0T5WQWN1AES5Z3E0RMSR-epic-developer-adoption-tooling`, with ed2948195 as the PO-to-PO-critic handoff commit and child auto-integration commits d3201dc61, b1164de95, and 70d3f0007 already on develop ancestry.
- `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` has `<IsPackable>true</IsPackable>`, `<PackageId>DCoding.Data.DVault.Analyzers</PackageId>`, and package assets under `analyzers/dotnet/cs/`.
- `src/DCoding.Data.DVault.Analyzers/CodeFirstDiagnosticCatalog.cs` defines DMV1901 and DMV1902, and `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultCodeFirstAnalyzerTests.cs` asserts both descriptors and diagnostic behavior.
- Source grep confirmed public API names used by the docs: `AddDVault`, `AddDVaultSqlite`, `AddDVaultPostgres`, `AddDVaultSqlServer`, `AddDVaultOracle`, `AddDVaultMySql`, `UseDataVaultMetadata`, `ApplyDataVaultMetadata`, `IDataVaultSaveService`, `IDataVaultReadService`, `DataVaultModelDriftReporter`, and `DataVaultLiveSchemaReader`.
- Package csproj grep confirmed package IDs for `DCoding.Data.DVault`, the five provider packages, and `DCoding.Data.DVault.Analyzers`; `src/DCoding.Data/DCoding.Data.csproj` is non-packable.
- `examples/DCoding.Data.DVault.PostgresQuickstart/Program.cs` uses `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, skips successfully when it is missing, then registers `AddDVaultPostgres()` and `UseDataVaultMetadata()` when configured.
- `examples/DCoding.Data.DVault.PostgresQuickstart/README.md` documents `docker.io/postgres:18`, the same connection-string variable, the non-secret `-p:DVAULT_TEST_POSTGRES_CONNECTION_STRING=Configured` marker, and expected missing-setup outcomes.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` conditionally restores `Npgsql.EntityFrameworkCore.PostgreSQL` only when `$(DVAULT_TEST_POSTGRES_CONNECTION_STRING)` is non-empty, and `ProviderTestCategories.cs` defines `ProviderIntegration.ExternalOptIn`.
- README.md, examples/README.md, docs/production-adoption-checklist.md, and docs/architecture/dvault-dotnet-ef-design-time-workflow.md document NuGet-oriented package IDs, provider startup extensions, explicit `IDataVaultSaveService` persistence, optional metadata interceptor behavior, normal EF design-time ownership, and SQLite-first live-schema drift limits.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No blocking missing examples observed. MySQL, SQL Server, and Oracle container fixtures plus analyzer rule families beyond DMV1901/DMV1902 are explicitly deferred as future follow-up scope rather than acceptance for this epic.

Risky assumptions
- Readers may still interpret the epic title as requiring a reusable Testcontainers helper library or full provider matrix; the persisted contract and repository docs should continue to keep this bounded to the PostgreSQL opt-in fixture.
- Analyzer adoption messaging can overpromise if presented as complete DVault model validation; source and docs support only the high-confidence DMV1901/DMV1902 baseline.
- External provider validation should remain opt-in so default local build/test guidance does not gain hidden Docker, Podman, or live database dependencies.

AC / test suggestions
- A future ticket can add provider-specific fixture tests for MySQL, SQL Server, or Oracle if Product chooses to expand beyond the PostgreSQL baseline.

Implementation watchouts
- Treat this as a tracking/closure epic: do not add new core DVault semantics, provider implementations, migration automation, EF CLI shims, or runtime behavior under this parent ticket.
- Preserve the explicit `IDataVaultSaveService` boundary and consumer-owned EF design-time workflow in docs and examples.
- Keep provider support promises tied to runnable evidence or documented limitations; live-schema drift remains SQLite-first with other providers behind external opt-in evidence.

Non-blocking notes
- The prompt snapshot said recent comments were absent, but repository-local comments exist and include the PO refinement contract, handoff to PO-critic, and run report; these comments support the persisted handoff.
- `git status --short --branch` showed unrelated local modifications to `.gicket-bot/.gitignore`, `.gicket/.gitignore`, `.gicket/project.json`, and `.gicket/types.json`; no product-scope blocker was inferred from those unrelated dirty files.

Split recommendations
- No additional split is recommended now; the existing three direct child stories cover the epic scope and are done.
- Future provider fixture expansion or broader analyzer coverage should remain separate provider-specific or rule-family tickets rather than expanding this epic.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment