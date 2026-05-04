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

When no provider-specific strategy is registered, or when every registered strategy rejects the current context and request batch, the dispatcher uses the built-in provider-neutral `IDataVaultSaveService` writer. Unsupported, unknown, or compatibility-only provider packages therefore keep the same public caller contract and fall back without requiring provider-name checks in the core package.

## V0.5 Provider Optimization Capability Matrix

The v0.5 compatibility baseline is the core provider-neutral `AddDVault()`/`IDataVaultSaveService` path without a provider-specific save strategy. Provider packages that only register `AddDVault()` inherit this compatibility baseline; they are not unsupported, but they do not carry a v0.5 requirement for provider-specific optimized save behavior.

Validation vocabulary:

- `ProviderIntegration.RequiredLocal`: required local integration coverage.
- `ProviderIntegration.ExternalOptIn`: optional external database validation that runs only when explicitly configured.
- `ProviderSmoke.Default`: default smoke or contract coverage; provider integration validation is not required for v0.5.

| Provider | V0.5 release posture | Optimized insert-only save behavior required | Set-based existence checks required | Validation expectation | Benchmark coverage required |
| --- | --- | --- | --- | --- | --- |
| SQLite | Provider-specific optimization baseline through `AddDVaultSqlite()` and `SqliteDataVaultSaveStrategy`. | Yes. This is the only provider-specific optimized insert-only save behavior required in v0.5. | Yes. SQLite is the only provider required to provide set-based existence checks in v0.5. | `ProviderIntegration.RequiredLocal` integration coverage is required locally. | Yes. Required benchmark coverage is SQLite-specific and uses local SQLite temporary files. |
| PostgreSQL | Compatibility baseline through `AddDVaultPostgres()`; no provider-specific save strategy is required in v0.5. | No. | No. | `ProviderIntegration.ExternalOptIn` only, gated by `DVAULT_TEST_POSTGRES_CONNECTION_STRING`; it is not required local validation. | No. |
| SQL Server | Compatibility baseline only through `AddDVaultSqlServer()`. | No. | No. | `ProviderSmoke.Default`; provider integration validation is not required in v0.5. | No. |
| Oracle | Compatibility baseline only through `AddDVaultOracle()`. | No. | No. | `ProviderSmoke.Default`; provider integration validation is not required in v0.5. | No. |
| MySQL | Compatibility baseline only through `AddDVaultMySql()`. | No. | No. | `ProviderSmoke.Default`; provider integration validation is not required in v0.5. | No. |

This matrix is release-scoped to v0.5. It does not require SQL Server, Oracle, MySQL, or PostgreSQL to ship provider-specific optimized writers, set-based satellite existence checks, required local integration suites, or benchmark baselines in this release. Benchmark notes remain SQLite-specific because the benchmark runner uses SQLite temporary files and does not require Postgres, Docker, `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, or other external services.

Current optimization-hook ownership is:

- `src/DCoding.Data.DVault`: shared capability-profile contracts, provider save strategy contracts, deterministic dispatch, and provider-neutral fallback.
- `src/DCoding.Data.DVault.Sqlite`: the v0.5 optimized save strategy and SQLite set-based existence-check behavior exposed by `AddDVaultSqlite()`.
- `src/DCoding.Data.DVault.Postgres`, `src/DCoding.Data.DVault.SqlServer`, `src/DCoding.Data.DVault.Oracle`, and `src/DCoding.Data.DVault.MySql`: compatibility-only registration surfaces for v0.5 and future provider-specific optimization hooks.
