# DVault V1 Explicit Save Service

Status: v1 implementation note
Ticket: 06EXB7H6KV753KM125XN3VDRTM

## Decision

DVault v1 uses an explicit DI-resolved save service as its default write entry point. Callers invoke `IDataVaultSaveService` with a focused request that carries the load timestamp, record source, and hub or link row intent.

The default `AddDVault()` path registers the save service without requiring an options object. Callers that need a different implementation can register their own `IDataVaultSaveService` through normal dependency injection override behavior.

## Rationale

The explicit service matches the existing convention-first public surface built around `AddDVault()`, `UseDataVault()`, and `ApplyDataVaultMetadata()`. Those APIs make Data Vault intent visible at startup or model configuration time, so the write path keeps the same visible boundary instead of hiding Data Vault persistence behind Entity Framework `SaveChanges` interception.

The v1 service owns the initial write orchestration for representative hub and link rows:

- Record source is supplied at the service request boundary.
- Load timestamp is supplied at the service request boundary and normalized to a UTC instant.
- Hub and link hash keys are computed through the registered `IStableHashNormalizer` and `IStableHashService`.
- The first concrete proof uses the existing SQLite EF Core test baseline.

Hub and link writes use the generated hash-key value as the reuse key. When a requested hub or link hash key is already present in the translated table, the default service leaves the existing row unchanged and still returns a deterministic `DataVaultSavedRecord` for the requested operation. `DataVaultSaveResult.RowsWritten` counts only rows inserted by that explicit invocation, so a fully reused request reports `0` while preserving the same hub-then-link saved-record ordering.

The current SQLite provider baseline is `DataVaultProviderCapabilityProfiles.Sqlite`, which declares `DataVaultProviderConcurrencySupport.NoneInV1Unsupported`. The default service therefore performs deterministic pre-insert reuse lookup for ordinary repeated saves, but it does not claim provider-neutral multi-writer conflict signals, retry behavior, merge semantics, or provider-specific upsert support.

SaveChanges interceptors remain outside the default v1 path. An optional interceptor or convenience wrapper can be considered later without changing the explicit save boundary that downstream persistence work builds on.

## Provider-Specific Save Strategy Dispatch

The shared provider optimization boundary is `IDataVaultProviderSaveStrategy`, `DataVaultProviderSaveStrategyContext`, and explicit provider capability profiles. `src/DCoding.Data.DVault` owns those contracts and the provider-neutral fallback dispatcher. Provider packages own provider-specific strategy implementations and any provider-specific SQL they require.

The core save service does not branch on provider names. It captures the registered `IDataVaultProviderSaveStrategy` implementations from dependency injection, sorts them by descending `Priority`, and preserves dependency-injection registration order when multiple strategies have the same priority. For every explicit save or ordered bulk save, the dispatcher calls `CanSave` with the current `DbContext` and the ordered request batch. The first compatible strategy receives a `DataVaultProviderSaveStrategyContext` carrying the context, ordered requests, stable hash service, and normalizer.

When no provider-specific strategy is registered, or when every registered strategy rejects the current context and request batch, the dispatcher uses the built-in provider-neutral `IDataVaultSaveService` writer. Unsupported or unknown provider packages, and provider packages without a compatible strategy for the current batch, therefore keep the same public caller contract and fall back without requiring provider-name checks in the core package.

Provider-specific save-strategy registration is separate from provider-name capability-profile auto-registration. In the current startup surface, `AddDVaultSqlite()` and `AddDVaultMySql()` call `DataVaultProviderCapabilityProfileSelection.Register(...)` for their EF provider names. `AddDVaultPostgres()`, `AddDVaultSqlServer()`, and `AddDVaultOracle()` register provider-specific save strategies, but their visible startup extensions do not auto-register provider-name capability profiles.

## V0.5 Provider Optimization Capability Matrix

The v0.5 compatibility baseline is the core provider-neutral `AddDVault()`/`IDataVaultSaveService` path without a provider-specific save strategy. Provider packages that only register `AddDVault()` inherit this compatibility baseline; they are not unsupported, but they do not carry a v0.5 requirement for provider-specific optimized save behavior.

Validation vocabulary:

- `ProviderIntegration.RequiredLocal`: required local integration coverage.
- `ProviderIntegration.ExternalOptIn`: optional external database validation that runs only when explicitly configured.
- `ProviderSmoke.Default`: default smoke or contract coverage; provider integration validation is not required for v0.5.

| Provider | V0.5 release posture | Optimized insert-only save behavior required | Set-based existence checks required | Validation expectation | Benchmark coverage required |
| --- | --- | --- | --- | --- | --- |
| SQLite | Provider-specific optimization baseline through `AddDVaultSqlite()` and `SqliteDataVaultSaveStrategy`. | Yes. | Yes. | `ProviderIntegration.RequiredLocal` integration coverage is required locally. | Yes. Required benchmark coverage is SQLite-specific and uses local SQLite temporary files. |
| PostgreSQL | Provider-specific optimization through `AddDVaultPostgres()` and the Npgsql-compatible PostgreSQL save strategy, with provider-neutral fallback when `CanSave` declines. | Yes, for clean `Npgsql.EntityFrameworkCore.PostgreSQL` contexts. | Yes, using PostgreSQL set-based insert/reuse and latest-state satellite checks. | `ProviderSmoke.Default` covers registration locally. Live execution remains `ProviderIntegration.ExternalOptIn`, gated by `DVAULT_TEST_POSTGRES_CONNECTION_STRING`; it is not required local validation. | No. |
| SQL Server | Provider-specific optimization through `AddDVaultSqlServer()` and `SqlServerDataVaultSaveStrategy`, with provider-neutral fallback when `CanSave` declines. | Yes, for clean SQL Server contexts. | Yes, using SQL Server set-based unique-row inserts and latest-state satellite checks. | `ProviderSmoke.Default` covers non-live strategy behavior locally. Live execution remains `ProviderIntegration.ExternalOptIn`, gated by `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`; it is not required local validation. | No. |
| Oracle | Provider-specific optimization boundary through `AddDVaultOracle()` and an Oracle-gated hub/link insert strategy. Unsupported shapes, including dirty tracked contexts and request batches containing satellite operations, still fall back through the provider-neutral writer. | Yes, for clean `Oracle.EntityFrameworkCore` hub/link batches only. | No. | `ProviderSmoke.Default` covers registration locally. Live execution remains `ProviderIntegration.ExternalOptIn`, gated by `DVAULT_TEST_ORACLE_CONNECTION_STRING`; it is not required local validation. | No. |
| MySQL | Provider-specific optimization through `AddDVaultMySql()` and `MySqlDataVaultSaveStrategy`, with provider-neutral fallback when `CanSave` declines. | Yes, for clean `Pomelo.EntityFrameworkCore.MySql` contexts. | Yes, using MySQL set-based unique-row inserts and latest-state satellite checks. | `ProviderSmoke.Default` covers registration locally. Live execution remains `ProviderIntegration.ExternalOptIn`, gated by `DVAULT_TEST_MYSQL_CONNECTION_STRING`; it is not required local validation. | No. |

This matrix is release-scoped to v0.5. It requires provider-specific optimized writers for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL within their supported request shapes. Oracle does not require set-based satellite existence checks, required local integration suites, or benchmark baselines in this release. PostgreSQL, SQL Server, Oracle, and MySQL live execution evidence remain opt-in because they require developer-managed databases through `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, `DVAULT_TEST_ORACLE_CONNECTION_STRING`, and `DVAULT_TEST_MYSQL_CONNECTION_STRING`. SQL Server non-live strategy behavior remains covered by default smoke coverage. Benchmark artifact scope is SQLite-required plus optional PostgreSQL comparison rows; SQL Server, Oracle, and MySQL are out of the v1 benchmark artifact without being out of the provider-specific save-strategy baseline.

Current optimization-hook ownership is:

- `src/DCoding.Data.DVault`: shared capability-profile contracts, provider-name keyed profile selection, provider save strategy contracts, deterministic dispatch, and provider-neutral fallback.
- `src/DCoding.Data.DVault.Sqlite`: the v0.5 optimized save strategy, SQLite provider-name profile registration, and SQLite set-based existence-check behavior exposed by `AddDVaultSqlite()`.
- `src/DCoding.Data.DVault.Postgres`: the Npgsql-compatible optimized save strategy exposed by `AddDVaultPostgres()` for clean PostgreSQL contexts, with fallback for incompatible contexts and no visible provider-name profile auto-registration in the startup extension.
- `src/DCoding.Data.DVault.SqlServer`: SQL Server optimized insert-only save strategy registration and SQL Server set-based existence-check behavior exposed by `AddDVaultSqlServer()`, with the opt-in smoke lane validating one hub, one link, and one satellite save through `ProviderIntegration.ExternalOptIn`; the startup extension does not visibly auto-register a provider-name profile.
- `src/DCoding.Data.DVault.Oracle`: an Oracle-gated insert-only hub/link save strategy exposed by `AddDVaultOracle()` for clean `Oracle.EntityFrameworkCore` batches, with dirty tracked contexts and satellite request batches declined for provider-neutral fallback; the startup extension does not visibly auto-register a provider-name profile.
- `src/DCoding.Data.DVault.MySql`: the Pomelo provider-name profile registration and optimized MySQL save strategy exposed by `AddDVaultMySql()` for clean Pomelo contexts, with fallback for incompatible contexts.
