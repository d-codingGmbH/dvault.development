# Production Adoption Checklist

Use this checklist when preparing a DVault-consuming application for production. It is a routing document for adopter readiness; follow the linked source documents for setup examples, governance details, migration guardrails, release evidence, and current limitations.

## Package And Provider Baseline

- [ ] Install the provider-neutral `DCoding.Data.DVault` package from NuGet and use the published installation guidance in the [README](../README.md#installation).
- [ ] Select the DVault provider package that matches the application database and keep every DVault package on one aligned published release version.
- [ ] Treat the coordinated DVault package family as exactly these package ids: `DCoding.Data.DVault`, `DCoding.Data.DVault.Analyzers`, `DCoding.Data.DVault.MySql`, `DCoding.Data.DVault.Oracle`, `DCoding.Data.DVault.Postgres`, `DCoding.Data.DVault.Sqlite`, and `DCoding.Data.DVault.SqlServer`.
- [ ] Install `DCoding.Data.DVault.Analyzers` only in projects that own DVault Code-First declarations or compile-time generated row mapping declarations, and keep it local with `PrivateAssets="all"`.
- [ ] Also install and configure the normal Entity Framework Core database provider package used by the application, such as SQLite, PostgreSQL, SQL Server, Oracle, or MySQL.
- [ ] Do not treat `src/DCoding.Data` as a consumer package. It is the non-packable source-root build anchor for the namespace family.
- [ ] Register `AddDVault()` and, when using a provider package, the matching provider startup extension shown in the [README quickstart](../README.md#register-dvault-services).
- [ ] Use the runnable SQLite or PostgreSQL quickstarts as setup evidence when a small local proof is useful; see [examples/README.md](../examples/README.md).

## Model Declaration Readiness

- [ ] Choose one authoritative model declaration path for each model boundary: Code-First metadata, metadata-first registry-backed metadata, or governed model-first `dvault.model.v1` artifacts.
- [ ] Use Code-First declarations when the model is local to one EF model and fits the fluent hub, hub-parent satellite, link-parent satellite, multi-active driving-key, explicit or derived link, and repeated same-hub role-bearing link surface described in the [README quickstart](../README.md#quickstart).
- [ ] Use metadata-first registry-backed metadata when one shared `DataVaultMetadataModel` or `DataVaultMetadataRegistry` should drive schema projection, explicit saves, typed reads, diagnostics, examples, or provider setup.
- [ ] Use model-first governance when source-controlled `dvault.model.v1` JSON artifacts need review, strict import diagnostics, canonical export, projection into EF metadata, and drift-report evidence. Follow [Model-First Governance Workflow](model-first-governance.md).
- [ ] For repeated same-hub Code-First links, require an explicit relationship name plus distinct non-blank `Participant<TEntity>(string role)` roles so generated participant names and hash-key columns are unambiguous.
- [ ] Model effectivity in v0.13 as caller-owned link-parent satellite state declared with `Link(...).Satellite<TSatellite>(...)`, `Payload(...)`, and optional `DrivingKey(...)`; do not assume an effectivity-specific builder or metadata kind exists.
- [ ] Mark multi-active satellites, PIT declarations, and bridge declarations as explicit opt-in model features in the adopter's design notes. They are not prerequisites for ordinary hub, link, and satellite setup.

## Migration And Drift Guardrails

- [ ] Keep the configured `DbContext`, DVault metadata registration, EF design-time factory, and preflight entrypoint in the consumer project that owns migrations.
- [ ] Run DVault diagnostics against the configured design-time model before applying migrations. Use [DVault Dotnet EF Design-Time Workflow](architecture/dvault-dotnet-ef-design-time-workflow.md) for the supported v1 order and GitHub Actions baseline.
- [ ] Add a consumer-owned CI step that invokes `dotnet run --project <consumer-project> -- validate` through the application's design-time command host.
- [ ] Use `dotnet run --project <consumer-project> -- export --output <path>` only for artifact maintenance or reviewed refresh workflows, not as the default blocking CI gate.
- [ ] When a reviewed `dvault.model.v1` artifact exists, make `dotnet run --project <consumer-project> -- drift --artifact <path>` a blocking artifact-versus-design-time-model check. Do not generate a fresh artifact with `export` as the default CI gate.
- [ ] Run `dotnet run --project <consumer-project> -- guardrail --migration <name>` after scaffolding a migration and before apply or integration.
- [ ] Use live-schema drift checks only within the documented boundary. Built-in reader coverage includes SQLite, PostgreSQL, SQL Server, Oracle, and MySQL, with both `MySql.EntityFrameworkCore` and Pomelo mapped to the MySQL reader.
- [ ] Keep PostgreSQL, SQL Server, Oracle, and MySQL live-schema checks opt-in and operationally managed by the consumer application, including connection strings, credentials, reachable databases, lifecycle cleanup, and CI isolation.
- [ ] Do not expect DVault to ship a `dotnet ef` command shim, intercept EF CLI commands, auto-run migrations, or apply schema repairs. Those behaviors are outside the current v1 workflow.

## Save And Read Boundaries

- [ ] Use `IDataVaultSaveService` as the default write boundary. Each save request should carry an explicit UTC load timestamp and record source.
- [ ] Treat generated mapper helpers as compile-time ergonomics around the same explicit save boundary: they construct registry-backed operations but do not choose timestamps, record sources, contexts, providers, or save orchestration.
- [ ] Keep ordinary EF `SaveChanges` separate from DVault persistence unless the application deliberately owns generated DVault rows and opts into metadata fill.
- [ ] Treat `UseDataVaultSaveChangesMetadataInterceptor(...)` as optional and metadata-only. It fills missing `LoadTimestamp` and `RecordSource` values on already tracked generated DVault rows; it does not create rows, compute hash keys, compute hash diffs, or replace `IDataVaultSaveService`.
- [ ] Prefer registry-backed requests or typed save helpers when they reduce repeated metadata declarations in loaders.
- [ ] Use `IDataVaultReadService` for provider-neutral latest and as-of satellite reads with caller-owned typed projectors, as shown in the [README read examples](../README.md#read-typed-latest-and-as-of-satellite-projections).
- [ ] Use PIT-backed and bridge read helpers only when the model already projects those tables and the application loading path has populated them. Current helpers do not refresh PIT rows, maintain bridge rows, infer graph closure, or provide provider-specific PIT or bridge read optimization.

## Provider And Advanced Feature Posture

- [ ] Confirm the selected provider package matches the configured EF provider and the application's supported database. Use the [README provider package guidance](../README.md#provider-packages) as the authoritative package responsibility reference.
- [ ] Keep provider-specific save strategies as optimizations around the same public save contract. Unsupported request shapes or dirty tracked contexts can decline to the provider-neutral writer; see [DVault V1 Explicit Save Service](architecture/dvault-v1-explicit-save-service.md).
- [ ] Treat provider-specific live database integration tests for PostgreSQL, SQL Server, Oracle, and MySQL as opt-in evidence behind their documented connection-string environment variables. Follow the [README local validation](../README.md#local-validation) guidance and the optional provider test sections for [PostgreSQL](../README.md#optional-local-postgres-integration-tests), [SQL Server](../README.md#optional-local-sql-server-integration-tests), [Oracle](../README.md#optional-local-oracle-integration-tests), and [MySQL](../README.md#optional-local-mysql-integration-tests).
- [ ] Treat advanced configuration hooks as optional or future-facing unless the application has a specific deterministic rule to configure. The current source-backed custom path is record-source resolver replacement; broader naming, hashing, timestamp, and provider hooks are planned boundaries. See [Optional Advanced Configuration Hooks](plans/optional-advanced-configuration-hooks.md).
- [ ] Do not make ordinary production adoption depend on PIT maintenance, bridge maintenance, multi-active PIT behavior, provider-specific physical tuning, custom hook matrices, or unpublished provider capabilities.

## Validation Evidence

- [ ] Run the adopter application's own build, test, migration-preflight, and drift checks before promoting a production configuration.
- [ ] For repository validation evidence, use the [README local validation](../README.md#local-validation) section as the authoritative command baseline:

```sh
dotnet build DVault.slnx --nologo
dotnet test DVault.slnx --nologo
bash tools/check-format.sh
```

- [ ] For package publication or release approval, use [Manual NuGet Publication Checklist](manual-nuget-publication.md) instead of this adoption checklist. Publication evidence adds packing, package verification, release notes, signing or approval records where applicable, stop conditions, and publish order.
- [ ] Before any coordinated publication, verify all seven package ids are validated and published together with one aligned version. Do not publish only a subset of the DVault package family.
- [ ] Keep consumer-facing release notes and internal adoption records limited to published package versions and documented current behavior. Do not imply availability for unpublished future releases.

## Current Limitations To Keep Visible

- DVault's default path is explicit and service-based. It does not make Data Vault persistence implicit through EF entity tracking.
- Dependent child key modeling is outside the v0.13 public documentation baseline.
- Repeated same-hub runtime and metadata support does not imply typed mapper or source-generator parity for repeated same-hub mappings.
- Design-time guardrails are explicit library APIs owned by the consumer project. DVault does not intercept EF migration commands.
- PIT-backed reads and bridge reads operate over already materialized tables and do not maintain those tables.
- SQLite is the default local live-schema proof because it does not need external infrastructure. PostgreSQL, SQL Server, Oracle, and MySQL live-schema readers are built in but remain external opt-in operational checks.
- Advanced configuration hooks beyond the documented record-source resolver path remain optional planning boundaries, not required setup.
