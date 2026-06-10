<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the DB2 provider-capability contract story as the architecture gate for epic 06F9G8GH969DQXD7WZ8JHD1GRR. Repository evidence shows DB2 is not part of the current five-provider baseline, the epic is already adequately split across downstream children, and no persistent planning writes were applied in this run.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket remains the contract-definition prerequisite under epic 06F9G8GH969DQXD7WZ8JHD1GRR and continues to block package story 06F9G8GZ384VKA7RVF039WKX1M until the DB2 dependency and capability contract is defined.
- Repository evidence is finite today: DataVaultProviderCapabilityProfiles, DataVaultModelArtifactImporter.CreateProviderCapabilityProfiles, DataVaultProviderCapabilityProfileSelection, and KnownProviderNames only cover SQLite, PostgreSQL, SQL Server, MySQL, and Oracle, so DB2 must be introduced as an explicit supported profile and not by fallback.
- The incoming blocks relation from done epic 06F9G8EE7ZA666MW8YEB2QP8BW is treated as historical compatibility-baseline evidence, not as a remaining PO blocker for this story.
- The DB2 epic is already adequately decomposed into this contract story plus package, schema/guardrail, integration, package-verification, and documentation children; no new child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run.

### Scope In
- Define the authoritative DB2 dependency contract for DVault: IBM.EntityFrameworkCore 8.0.0.400 for net8.0 and 10.0.0.100 for net10.0, aligned with the downstream 8.34.0 and 10.34.0 DVault DB2 package lines described by sibling tickets.
- Define the exact DB2 provider-name detection contract used by registration, capability selection, diagnostics, and model-artifact/profile selection.
- Define the DB2 capability-profile facts expected by DataVaultProviderCapabilityProfile: profile name, logical-property type mappings, load-timestamp mapping behavior, identifier-length or escaping caveats, included-index handling, and duplicate-index or primary-key behavior.
- Define the DB2 compatibility posture for DVault-owned schema generation, migration guardrails, diagnostics, provider-neutral save and read behavior, and live-schema proof, including any explicit unsupported boundaries that must fail fast or stay documented as unsupported.
- Define the external DB2 test posture as opt-in external-provider evidence only, with developer-managed database and container lifecycle outside DVault.

### Scope Out
- Implementing the DB2 provider package, service registration, or solution and project wiring; that belongs to story 06F9G8GZ384VKA7RVF039WKX1M.
- Implementing DB2 schema, naming, live-schema, or migration-guardrail code and tests; that belongs to story 06F9G8H5HE1CJHQXGC2C2YK7P8.
- Implementing DB2 save and read integration coverage; that belongs to story 06F9G8HBXS7Y42J7XFSQKZ2AZ8.
- Updating package-verifier expectations; that belongs to task 06F9G8HJJDJH4KF9VK6TZ8B1Z0.
- Updating README, release notes, and adoption guidance; that belongs to task 06F9G8HRZ72XP5Z7FNWM6MBMQC.
- Adding DB2-specific benchmark claims, provider-specific SQL artifact support, platform provisioning, migration execution, or runtime orchestration beyond the documented DVault provider patterns.

## Acceptance Criteria
- The story lands an authoritative DB2 provider-capability contract in an approved ticket surface that names the exact IBM.EntityFrameworkCore package version for net8.0 and net10.0 and states that the contract is a prerequisite for downstream DB2 implementation tickets.
- The contract names the exact DB2 EF provider identifier or identifiers that must drive DataVaultProviderCapabilityProfileSelection, diagnostics, and provider registration, and it explicitly forbids silent reliance on the current unknown-provider fallback path.
- The contract defines the DB2 capability-profile facts required by existing provider-contract surfaces: stable profile name, logical-property type mappings, load-timestamp behavior, identifier and DDL caveats, included-index behavior, and whether indexes fully covered by a primary key are acceptable.
- The contract states the DB2 boundary for schema generation, migration-guardrail review, live-schema proof, and save and read compatibility, including any fail-fast unsupported cases that downstream tickets must preserve instead of inferring parity with the existing five-provider baseline.
- The contract states that DB2 external validation is opt-in only and does not make DB2 databases, Podman or Docker containers, credentials, schemas, or CI infrastructure part of default local validation or DVault-owned provisioning responsibility.

## Definition of Done
- The ticket description or attached planning artifact is updated with the DB2 contract and cites the repository surfaces it governs, including provider capability profiles, provider selection, diagnostics provider-name lists, and the external-provider integration and package-matrix pattern.
- Downstream DB2 child tickets can implement package wiring, schema guardrails, integration coverage, package verification, and documentation updates without reopening provider-version, provider-name, external-test-posture, or provisioning-scope questions at PO level.
- The contract explicitly records any DB2 unsupported boundaries instead of leaving them implicit or inherited from SQLite fallback behavior.
- The completed contract stays architecture and planning level only and does not ship product-code changes outside the downstream implementation tickets.

## Implementation Notes
- Current repository evidence hard-codes a five-provider baseline in DataVaultProviderCapabilityProfiles, DataVaultModelArtifactImporter.CreateProviderCapabilityProfiles, DataVaultProviderCapabilityProfileSelection, and KnownProviderNames; this story should define DB2 as an explicit sixth profile contract before any code or package work starts.
- Current built-in profiles all use DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported and DataVaultProviderConcurrencySupport.NoneInV1Unsupported. Unless DB2 needs a documented exception, keep that same v1 default rather than inventing new SQL-function or concurrency claims.
- Current provider packages register exact provider names via AddDVault Postgres, SqlServer, Oracle, and MySql style startup extensions; the DB2 contract should follow that explicit registration pattern and should not rely on provider-neutral AddDVault() alone.
- Current integration-matrix and external-live lanes use conditional provider PackageReference entries and ProviderIntegration.ExternalOptIn gating. The DB2 contract should reuse that posture instead of expanding default local validation.
- Current README and release guidance explicitly say DVault does not provision containers, databases, users, credentials, schemas, or external-provider CI infrastructure. The DB2 contract should carry forward the same non-goal wording.
- Current provider-specific SQL artifact and provider-read-optimization planning contracts explicitly stop at the existing five-provider baseline, so this story should not implicitly expand those separate lanes.

## Open Questions
- none

## Follow-Up Questions
- After the contract lands, should the documentation task standardize the exact DB2 opt-in environment-variable name and example local connection-string workflow so it matches the existing external-provider README pattern?
- If DB2 live-schema proof cannot match the existing Postgres, SqlServer, Oracle, and MySql reader boundary in v1, should a later follow-up ticket add DB2 live-schema reader support after baseline provider support ships?
- After baseline DB2 support is implemented, is there any need for DB2-specific performance or provider-specific SQL-artifact planning, or should DB2 remain provider-neutral outside the core support lane?

## Risks
- IBM DB2 provider behavior may diverge from the existing five-provider assumptions on identifier length, generated DDL, included indexes, or live-schema introspection, so the contract must record explicit caveats instead of implying parity.
- Because the repository currently treats unknown providers as fallback rather than explicit support, an incomplete DB2 contract could let downstream implementation accidentally inherit unsupported SQLite-oriented behavior or misleading diagnostics.
- DB2 validation will depend on opt-in external database availability and developer-managed lifecycle, so proof beyond default local SQLite and smoke coverage may remain environment-sensitive even after the contract is defined.
- The live relation set still includes a historical incoming blocks edge from done epic 06F9G8EE7ZA666MW8YEB2QP8BW; if tracker automation interprets done-source blocks strictly, that relation may need later housekeeping even though it is not a PO blocker here.

## Split Recommendations
- No additional split is recommended. Epic 06F9G8GH969DQXD7WZ8JHD1GRR already separates the DB2 work into this contract story plus package, schema and guardrail, integration, package-verification, and documentation children.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Define the DB2 provider contract before implementation. Include IBM.EntityFrameworkCore 8.0.0.400 for net8.0 and 10.0.0.100 for net10.0, provider name detection, capability profile, identifier/DDL caveats, live-schema and migration guardrail expectations, external Podman/container test posture, and explicit non-goals around provisioning or platform responsibilities.

<!-- gicket-bot:developer-delivery-contract:v1:start -->
## Developer Delivery: DB2 Provider Capability And Dependency Contract

### Decision

DB2 support is defined as an explicit sixth DVault provider contract. Downstream DB2 implementation tickets must not rely on the current unknown-provider fallback path, must not inherit SQLite capability behavior, and must not claim DB2 support until the provider name, capability profile, diagnostics, package matrix, schema guardrails, and opt-in external evidence all bind to this contract.

This contract is architecture and planning level only. It intentionally does not add product-code changes, package projects, solution wiring, provider strategy implementation, schema-reader implementation, migration execution, or documentation-release prose; those remain owned by the downstream DB2 child tickets named in the delivery contract.

### Dependency Contract

- DVault `net8.0` DB2 package line: `8.34.0`.
- DVault `net10.0` DB2 package line: `10.34.0`.
- EF provider package for `net8.0`: `IBM.EntityFrameworkCore` version `8.0.0.400`.
- EF provider package for `net10.0`: `IBM.EntityFrameworkCore` version `10.0.0.100`.
- Any DB2 package, integration-test, package-verifier, or sample project references must be target-framework conditioned to the matching line and must not introduce mixed EF Core lines beyond the explicit DB2 package pins above.
- Conditional DB2 live-test references must use the same opt-in package-matrix pattern as the current external providers and remain absent from default local validation when the DB2 gate is not configured.

### Provider Identifier Contract

- The canonical DB2 EF provider identifier for DVault is `IBM.EntityFrameworkCore`.
- Downstream implementation must add that identifier anywhere provider names are enumerated for capability selection, diagnostics, provider registration, integration/package matrix evidence, and external-provider guidance.
- No DB2 alias is approved by this contract. If the IBM provider later exposes a different `DbContext.Database.ProviderName`, downstream implementation must fail fast or return `UnknownOrUnregisteredProviderName` until a new ticket explicitly amends the contract with the observed alias.
- `DataVaultProviderCapabilityProfileSelection.Select(...)` must select the DB2 profile for `IBM.EntityFrameworkCore`; it must not allow DB2 to fall through to `DataVaultProviderCapabilityProfiles.Sqlite`.
- The DB2 provider package startup extension must follow the explicit provider package pattern: register `IBM.EntityFrameworkCore` with the DB2 capability profile before adding DB2-specific behavior or strategy services. Provider-neutral `AddDVault()` alone is not sufficient to claim DB2 support.

### Capability Profile Contract

The stable DB2 capability profile name is `db2-v1`.

The profile must declare:

- `DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported`.
- `DataVaultProviderConcurrencySupport.NoneInV1Unsupported`.
- `MaximumIdentifierLength = 128` for generated DB2 physical identifiers unless a downstream verified DB2 object-class rule requires a smaller internal guardrail.
- `AllowsIndexesCoveredByPrimaryKey = false`; generated secondary indexes whose ordered key columns are fully covered by the primary key must be suppressed or diagnosed instead of emitted as duplicate DB2 DDL.
- `UnsupportedIncludedIndexColumnMode = DataVaultUnsupportedIncludedIndexColumnMode.AppendToKey`; DVault v1 must not claim SQL Server-style native included-index-column support for DB2.

Required logical-property mappings:

| Logical property kind | CLR model type | DB2 store type | Value format |
| --- | --- | --- | --- |
| `HashKey` | `string` | `VARCHAR(64)` | `Text` |
| `HashDiff` | `string` | `VARCHAR(64)` | `Text` |
| `LoadTimestamp` | `string` | `VARCHAR(33)` | `Iso8601UtcText` |
| `RecordSource` | `string` | `VARCHAR(255)` | `Text` |
| `ParticipantReference` | `string` | `VARCHAR(64)` | `Text` |
| `BusinessKey` | `string` | `VARCHAR(255)` | `Text` |
| `PayloadText` | `string` | `CLOB` | `Text` |
| `SatelliteSnapshotReference` | `string` | `VARCHAR(33)` | `Iso8601UtcText` |
| `BridgeDepth` | `int` | `INTEGER` | `NativeInteger` |
| `DrivingKey` | `string` | `VARCHAR(255)` | `Text` |

Load timestamps and PIT satellite snapshot references are stored as UTC ISO 8601 text in `VARCHAR(33)` by default. The DB2 profile must preserve deterministic UTC formatting and must not infer native `DateTimeOffset` parity from PostgreSQL or SQL Server. `DataVaultLoadTimestampStorage.Iso8601UtcText` remains `VARCHAR(33)` for DB2. `DataVaultLoadTimestampStorage.UtcTicks` maps DB2 load timestamps and snapshot references to CLR `long`, store type `BIGINT`, and `DataVaultProviderValueFormat.UtcTicks`.

### Identifier And DDL Guardrail Contract

- DB2 generated identifiers are provider-specific physical identifiers governed by the provider identifier and DDL guardrail contract, not by SQLite fallback behavior.
- Downstream DB2 schema work must add DB2 identifier safety facts for unquoted-name comparison, reserved words, invalid characters, first-character rules, folding behavior, and object-class length limits before emitting provider-specific DB2 DDL.
- The v1 DB2 path must prefer safe generated unquoted identifiers. If a generated Data Vault table, column, key, constraint, or index name requires DB2 quoting/escaping, collides after physical-name projection, exceeds the applicable DB2 limit, or cannot be represented by the IBM provider, migration guardrails must fail fast with bounded diagnostics.
- DB2 DDL must not silently quote names to preserve case, silently truncate, silently drop unsupported indexes, or emit provider-specific objects outside the reviewed Data Vault schema-generation boundary.

### Diagnostics And Selection Contract

- Diagnostics must list DB2 as a finite known provider only when `IBM.EntityFrameworkCore` maps to `db2-v1`.
- Unknown or unregistered DB2-like provider names must surface `UnknownOrUnregisteredProviderName` rather than using SQLite profile facts.
- Explain output must report provider name `IBM.EntityFrameworkCore`, profile name `db2-v1`, DB2 type mappings, `MaximumIdentifierLength`, `AllowsIndexesCoveredByPrimaryKey`, `UnsupportedIncludedIndexColumnMode`, selected save/read strategy status, and finite fallback causes using the existing diagnostics vocabulary.
- DB2 strategy candidates must advertise only `IBM.EntityFrameworkCore` unless a later contract explicitly adds another DB2 provider identifier.

### Save, Read, And Provider Strategy Boundary

- Baseline DB2 compatibility is provider-neutral save and read behavior plus explicit diagnostics. Provider-specific DB2 optimized save or read strategies are downstream implementation work and must be diagnostics-gated.
- DB2 must not be added to the provider-specific SQL artifact lane, benchmark-performance claims, stored-procedure dispatch, or provider-native strategy claims by implication.
- A DB2 provider package may register DB2 behavior, save strategy, PIT read strategy, or bridge read strategy only when the downstream ticket also supplies representative evidence and preserves provider-neutral fallback for unsupported shapes.
- Latest-satellite optimized reads are unsupported for DB2 in v1 unless a later ticket provides explicit DB2 evidence; absence of a DB2 latest-satellite optimized strategy must fall back to provider-neutral reads.
- PIT and bridge DB2 strategy candidates, if implemented, must use the same complete read-shape evidence and stale-maintenance gates as the existing relational provider read-strategy boundary.

### Schema, Migration, And Live-Schema Proof Boundary

- DB2 schema generation must be driven by `db2-v1` and its DB2 identifier guardrails; it must not reuse SQLite, MySQL, Oracle, PostgreSQL, or SQL Server DDL caveats by fallback.
- Migration guardrail review must fail before unsafe DB2 DDL is emitted for unsupported identifiers, duplicate primary-key-covered indexes, unsupported included-index projection, unsupported DB2 store types, or provider-specific operations outside the reviewed DVault-owned schema boundary.
- DB2 live-schema proof is opt-in external-provider evidence. DB2 must not be treated as live-schema-supported until a DB2 reader path is implemented and exercised against a developer-managed DB2 database.
- Before DB2 live-schema reader support lands, live-schema requests for `IBM.EntityFrameworkCore` must report unsupported provider status rather than pass through another provider reader or silently succeed.
- Once DB2 live-schema proof is implemented, it must cover the same DVault-owned table, column, primary-key, and secondary-index evidence shape used by the existing live-schema proof boundary.

### External Validation And Provisioning Contract

- DB2 external validation is `ProviderIntegration.ExternalOptIn` only.
- The canonical DB2 opt-in gate is `DVAULT_TEST_DB2_CONNECTION_STRING`; conditional restore/build properties should follow the existing pattern by using a non-secret configured marker such as `-p:DVAULT_TEST_DB2_CONNECTION_STRING=Configured` when the lane is intentionally enabled.
- Default local test execution and required SQLite-backed integration coverage must not require a DB2 server, Podman, Docker, credentials, schemas, users, CI infrastructure, or network-reachable external services.
- DVault does not provision DB2 containers, databases, users, credentials, schemas, lifecycle cleanup, or CI isolation. The developer or consuming application owns DB2 host selection, container lifecycle if used, database creation, credentials, schema permissions, cleanup, and secret storage.
- Missing DB2 opt-in configuration must produce deterministic skip or unavailable diagnostics, not test failures caused by missing infrastructure and not a hidden fallback to another provider.

### Governed Repository Surfaces

Downstream tickets should update and test the following repository surfaces consistently with this contract:

- `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs` for `DataVaultProviderCapabilityProfiles.Db2` and load-timestamp storage variants.
- `src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs` for `IBM.EntityFrameworkCore` mapping and no SQLite fallback for DB2.
- `src/DCoding.Data.DVault/DataVaultModelArtifactImporter.cs` for DB2 profile inclusion in model-artifact capability profile output.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` for `KnownProviderNames.Db2`, DB2 diagnostics, and provider strategy candidate names.
- The downstream DB2 provider package startup extension for explicit `IBM.EntityFrameworkCore` profile registration and DB2 service registration.
- `tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs` and related package-verifier tests for the `IBM.EntityFrameworkCore` `8.0.0.400` / `10.0.0.100` matrix and opt-in restore gate.
- External-provider integration tests and README/adoption guidance for the `DVAULT_TEST_DB2_CONNECTION_STRING` opt-in lane and non-provisioning posture.

### Unsupported Boundaries To Preserve

- No silent fallback from DB2 to SQLite capability, DDL, diagnostics, live-schema, save-strategy, or read-strategy behavior.
- No DB2 provider package or solution/project wiring in this contract story.
- No DB2 schema-reader, migration-guardrail implementation, provider strategy, package-verifier implementation, documentation update, benchmark claim, provider-specific SQL artifact expansion, or runtime orchestration in this contract story.
- No default local DB2 database, container, credential, schema, user, or CI service requirement.
- No unapproved DB2 provider-name aliases.

<!-- gicket-bot:developer-delivery-contract:v1:end -->