# Provider Identifier And DDL Guardrail Contract

Status: v1 planning contract
Ticket: 06F8KZMRXRHRKHV56Y96M4S90G

## Purpose

This document defines the v1 provider identifier and DDL guardrail contract for DVault-owned schema generation and Entity Framework migration review. It fixes the finite supported-provider baseline, the provider profile facts required for identifier safety, deterministic physical-name projection, generated index and constraint caveats, load timestamp storage implications, bounded diagnostics, and the fail-fast boundary for unsafe provider-specific DDL shapes.

This contract ratifies the existing provider-neutral logical naming baseline. Logical Data Vault names still come from `docs/naming/default-naming-policy.md` and remain provider-neutral. Provider profiles may derive physical names only when a generated logical name is unsafe for a supported provider because of identifier length, reserved-word, escaping, native included-index, duplicate-index, or post-truncation collision rules.

## Source Of Truth Boundaries

Use these repository surfaces as the authoritative anchors for implementation:

| Responsibility | Source |
| --- | --- |
| Provider-neutral logical table and column naming | `docs/naming/default-naming-policy.md` |
| `dvault.model.v1` load timestamp tokens | `docs/plans/dvault-model-v1-schema-contract.md` |
| Provider-neutral persistence convention boundary | `docs/plans/dvault-v1-default-persistence-convention-policy.md` |
| Provider capability profiles and type mappings | `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs` |
| Built-in model artifact provider profile registration order | `src/DCoding.Data.DVault/DataVaultModelArtifactImporter.cs` |
| Logical-to-physical traceability annotations | `src/DCoding.Data.DVault/DataVaultAnnotationNames.cs` |
| Explain diagnostics, migration guardrail reports, and profile facts | `src/DCoding.Data.DVault/DataVaultDiagnostics.cs`, `src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs`, `src/DCoding.Data.DVault/DataVaultMigrationGuardrailReport.cs` |
| Activity failure tags and bounded failure classes | `src/DCoding.Data.DVault/DataVaultActivityTracing.cs` |

## Supported Provider Baseline

The v1 supported-provider baseline is finite. It is exactly the package/profile set already visible in the repository.

| Provider | EF provider name evidence | DVault profile | Identifier cap currently enforced by profile | Included-index handling | Duplicate index covered by primary key |
| --- | --- | --- | --- | --- | --- |
| SQLite | `Microsoft.EntityFrameworkCore.Sqlite` | `sqlite-v1` | none declared | append include columns to key when needed | allowed |
| Oracle | `Oracle.EntityFrameworkCore` | `oracle-v1` | none declared | append include columns to key when needed | not allowed |
| PostgreSQL | `Npgsql.EntityFrameworkCore.PostgreSQL` | `postgres-v1` | none declared | native included-index columns | allowed |
| SQL Server | `Microsoft.EntityFrameworkCore.SqlServer` | `sqlserver-v1` | none declared | native included-index columns | allowed |
| MySQL | `Pomelo.EntityFrameworkCore.MySql`, `MySql.EntityFrameworkCore` | `mysql-pomelo-v1` | 64 characters | ignore include columns | allowed |

No other provider is part of this contract. An unrecognized provider name may use the existing default/fallback path only where current APIs already do so; it must not inherit provider-specific DDL safety claims from one of the five supported profiles.

## Required Provider Profile Facts

Every supported provider profile used by DVault-owned schema generation or migration guardrails must contribute these identifier-safety inputs:

| Fact | Contract |
| --- | --- |
| Profile name | Stable, deterministic profile name used in annotations, diagnostics, and import/export profile identity. |
| EF provider names | Exact EF Core provider names that select the profile. Multiple provider names may select the same profile only when the generated DDL contract is intentionally shared. |
| Identifier length limits | A table of maximum unquoted physical identifier lengths by generated object class: schema, table, column, primary key, secondary index, unique constraint, foreign key constraint, check constraint, default constraint, and sequence where applicable. A profile may expose one shared limit for all classes only when the provider has no narrower class-specific limit in the supported profile. |
| Reserved word catalog | A finite, static set of provider-reserved tokens plus the comparison rule used for unquoted identifiers. The catalog is a tested v1 profile input, not an evergreen vendor promise. |
| Unquoted identifier rules | Allowed characters, first-character rules, normalization/folding behavior, and whether generated names that need quoting are considered unsafe for the no-rewrite path. |
| Native included-index support | Whether EF generated indexes may preserve include columns natively, must append include columns to the key, must ignore include columns, or must fail fast for the generated shape. |
| Duplicate-index caveat | Whether a secondary index whose effective key is covered by the primary key is legal, redundant-but-safe, or unsafe for the provider. |
| Load timestamp mappings | Store type, model CLR type, and `DataVaultProviderValueFormat` for `provider-default`, `iso-8601-utc-text`, and `utc-ticks`. |

Current code exposes part of this matrix through `DataVaultProviderCapabilityProfile.MaximumIdentifierLength`, `AllowsIndexesCoveredByPrimaryKey`, `UnsupportedIncludedIndexColumnMode`, and provider type mappings. Downstream implementation may add narrower internal profile facts to satisfy this contract without adding a broad consumer override surface.

## Logical And Physical Names

The `ProducedName` annotation remains the authoritative DVault logical name for generated tables, columns, primary keys, indexes, and future DVault-owned constraints. Provider-specific projection must not overwrite that annotation.

When a physical name differs from the produced logical name, implementations must preserve traceability by keeping:

- `DataVaultAnnotationNames.ProducedName` on the EF metadata item.
- `DataVaultAnnotationNames.MetadataName` where the generated object maps to source metadata.
- `DataVaultAnnotationNames.ProviderProfile` on provider-mapped properties and equivalent explain output for provider-selected schema generation.
- The physical name visible through EF relational metadata, live-schema snapshots, migration summaries, or guardrail diagnostics.

Physical projection is scoped to generated DVault-owned names. Consumer-authored migration operations, raw SQL, and third-party DDL are not automatically rewritten.

## Deterministic Physical-Name Projection

Provider-safe physical names are derived after provider-neutral logical names are produced. Projection must use ordinal string comparison, invariant formatting, UTF-8 input bytes, lowercase hexadecimal hashes, and no current culture, current time, random value, process-local state, machine identifier, current directory, or provider-generated value.

The v1 projection pipeline is:

1. Start with the logical produced name.
2. Classify the generated object class and lookup the selected provider profile facts.
3. Reject an empty name, an unsupported object class, or a profile with no required safety facts.
4. If the candidate is already safe for the provider and object class, use it unchanged.
5. If the candidate is unsafe only because it is a reserved word, would require quoting, exceeds an identifier length limit, or collides after physical projection, derive a provider-safe candidate.
6. If no deterministic provider-safe candidate can be produced within the profile's limits, fail fast before unsafe DDL is emitted.

When derivation is required, the candidate must preserve readable logical traceability:

- Keep the longest valid left prefix that fits the provider and object-class limit.
- Append `_`.
- Append at least 8 lowercase hexadecimal characters from `SHA256(logical-produced-name)`.
- For a post-truncation collision in the same provider, object class, and identifier scope, expand the hash suffix in 4-character increments until the candidate is unique.
- If the limit is too small to hold a one-character prefix, `_`, and the required hash suffix, fail fast.

Identifier scopes are provider profile plus object class plus the natural EF relational scope: schema for tables, table for columns, table for keys and constraints, and table/schema according to EF metadata for indexes. Implementations must not use declaration order as the only collision resolver because adding a new declaration could churn existing physical names.

Reserved-word and escaping avoidance may add an object-class suffix before length projection when that keeps the name readable, such as `Table`, `Column`, `Index`, `Key`, or `Constraint`. The suffix itself is then processed by the same length and collision rules.

## Index, Key, And Constraint Caveats

Primary key names, default secondary index names, uniqueness constraints, and future DVault-owned constraint names must go through the same physical-name safety pipeline as tables and columns.

Generated indexes must apply provider caveats before EF emits or migration guardrails approve DDL:

- PostgreSQL and SQL Server profiles may preserve include columns through native provider extensions.
- SQLite and Oracle profiles currently project unsupported include columns by appending them to the index key.
- MySQL currently ignores unsupported include columns.
- Oracle currently treats secondary indexes covered by the primary key as unsafe/redundant and omits them from generated metadata.

When a provider caveat changes the effective index shape, explain diagnostics and live-schema/idempotency comparisons must report the provider profile and effective index columns rather than pretending the provider emitted the provider-neutral index shape unchanged.

Generated primary keys and indexes that cannot be safely named or shaped for the selected provider are blocking validation failures. DVault must not silently drop a generated key or unique requirement merely to make a migration apply.

## Load Timestamp Storage

`dvault.model.v1` has exactly three load timestamp storage tokens:

- `provider-default`
- `iso-8601-utc-text`
- `utc-ticks`

This contract does not add storage tokens. The selected token transforms only the provider capability profile's load timestamp and satellite snapshot reference mappings.

| Token | Contract |
| --- | --- |
| `provider-default` | Use the selected profile's default mapping. SQLite, Oracle, and MySQL currently use ISO 8601 UTC text storage; PostgreSQL and SQL Server currently use native `DateTimeOffset` mappings. |
| `iso-8601-utc-text` | Use provider text storage capable of preserving the UTC ISO 8601 representation. Current store types are `TEXT`, `VARCHAR2(33 CHAR)`, `varchar(33)`, `nvarchar(33)`, or `varchar(33)` according to profile. |
| `utc-ticks` | Use native 64-bit integer storage for UTC `DateTime` ticks. Current store types are `INTEGER` for SQLite, `NUMBER(19)` for Oracle, and `bigint` for PostgreSQL, SQL Server, and MySQL. |

Migration and DDL guardrails must treat load timestamp storage as a provider profile fact. A generated migration that changes the store type or value format away from the selected profile is a schema mismatch unless the owning metadata contract changed the token intentionally.

## Diagnostics And Failure Classes

Provider identifier and DDL guardrails must fail before unsafe DDL is emitted. Diagnostics must stay bounded and automation-friendly.

Every provider guardrail failure must include:

- The selected provider name when available.
- The provider capability profile name.
- The generated object class.
- The logical produced name and metadata name when available.
- The attempted physical name when derivation happened.
- The limit, reserved word, unsupported provider feature, or collision that caused the failure.
- The deterministic path to the model element or migration operation.
- A safe remediation boundary.

The safe remediation boundary is one of:

- Rename the source model declaration or role so provider-neutral naming produces a safe logical name.
- Change the approved `loadTimestampStorage` token when the mismatch is only a timestamp storage mapping decision.
- Keep the EF migration aligned with the generated DVault model.
- Split the model or declaration when a provider cannot safely represent the generated shape.
- Choose a supported provider/profile that can represent the generated shape.

Diagnostics may use existing `DMV####` validation and `DVM2###` migration guardrail families. New codes must remain finite and cataloged through the same central diagnostic-definition pattern. Activity telemetry should classify provider guardrail failures with the existing provider, failure kind, and failure class vocabulary rather than adding unbounded activity names or raw SQL payloads.

## Fail-Fast Boundary

DVault may deterministically project provider-safe physical names for DVault-owned generated metadata. DVault must fail fast for these cases:

- Unsupported provider profile or missing profile safety facts.
- Generated name that cannot be made safe within the selected provider/object-class limit.
- Post-projection collision that cannot be resolved by deterministic hash expansion.
- Provider-required quoting or escaping that the selected no-rewrite path does not support.
- Generated index, key, or constraint shape that the provider cannot safely represent.
- Load timestamp mapping mismatch between the selected token and generated DDL.
- Migration operation that drops, renames, or replaces DVault-owned generated tables, columns, keys, indexes, or constraints outside an intentional metadata evolution.

Fail-fast means validation or migration guardrail diagnostics are returned before applying a migration or emitting unsupported generated DDL. It does not mean DVault repairs the migration or rewrites arbitrary SQL.

## Non-Goals

This contract does not:

- Change `docs/naming/default-naming-policy.md`, default logical object names, or `dvault.model.v1` tokens.
- Add a consumer-visible physical naming override API.
- Add provider packages or broaden the supported-provider baseline.
- Promise exhaustive evergreen vendor keyword catalogs.
- Rewrite consumer-authored migrations, arbitrary raw SQL, or third-party DDL.
- Intercept `dotnet ef migrations add`, `dotnet ef database update`, or provider command execution.
- Add a migration runner, schema repair tool, or automatic live database mutation.

## Follow-Up Decisions

These decisions are intentionally deferred and do not block v1 guardrail implementation from this contract:

- Whether v1 should expose a consumer override surface for provider-specific physical-name shortening or quoting.
- Whether provider-profile safety data should be version-pinned to exact EF Core and provider package major versions.
- Whether implementation work should split into separate provider-profile data, model-validation, migration-guardrail, and diagnostics/test tickets.
