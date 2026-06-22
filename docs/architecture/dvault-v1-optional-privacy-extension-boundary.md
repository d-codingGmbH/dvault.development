# DVault V1 Optional Privacy Extension Boundary

Status: v1 contract
Ticket: 06FE4R9PP99G6Q1PTPK4TKD460

## Decision

DVault v1 treats privacy-oriented behavior for EU GDPR/DSGVO projects as an optional add-on boundary. The boundary is additive to the existing DVault library family: provider-neutral DVault abstractions stay in `DCoding.Data.DVault`, while any provider-specific behavior must live behind provider package extensions and strategy seams in the same style as the current save and read strategy packages.

The add-on is explicitly opt-in. Existing callers that use `AddDVault()`, metadata registration, `IDataVaultSaveService`, `IDataVaultReadService`, PIT maintenance, bridge maintenance, diagnostics, hashing, and telemetry keep their current behavior unless they intentionally reference and enable a future privacy extension package. This contract does not approve default runtime behavior changes, automatic `SaveChanges` privacy work, hidden background processing, or provider-name branching in the shared core package.

The v1 privacy boundary is a library extension boundary, not an application platform, governance system, legal opinion, key-management platform, or retention engine.

## Supported Shape

A future privacy package may compose with the existing explicit DVault surfaces:

- service registration through a dedicated opt-in extension layered on top of `AddDVault()`;
- metadata annotations or sidecar metadata that are visible at model-configuration or registry-registration time;
- satellite payload personal-data metadata that references existing `dvault.model.v1` payload names and declares stable logical encrypted-payload aliases;
- caller-driven save or read helpers that wrap, prepare, filter, redact, pseudonymize, encrypt, or decrypt data before invoking the existing explicit save and read services;
- request-bound diagnostics that describe privacy strategy selection without exposing raw hash keys, payload values, provider SQL, connection strings, secrets, or policy decisions;
- provider-specific implementations registered by provider packages through extension methods and DI-discovered strategies.

One acceptable activation shape is:

```csharp
services
    .AddDVault()
    .AddDVaultPrivacy(options => {
      options.UseCallerOwnedKeyProvider(keyProvider);
      options.EnablePseudonymizationProfile("customer-profile");
    });
```

This example is illustrative. It fixes the activation posture only: the caller explicitly references a privacy package, calls an opt-in registration method, and supplies application-owned policy or key material dependencies. It does not define the final API shape for encryption, pseudonymization, redaction, export, or retention metadata.

## Privacy Status And Effectivity Modeling

DVault v1 privacy workflows should model status, consent, relationship validity, and other effectivity-style state through the existing satellite surfaces. Entity-local privacy state belongs in an ordinary hub-parent satellite. Relationship-local state, consent-link state, and other relationship effectivity state belongs in a link-parent satellite declared with the same `Payload(...)` and optional `DrivingKey(...)` semantics used for other satellites. When a privacy workflow needs multiple concurrent status or validity series, it should use the existing multi-active driving-key contract instead of introducing an STS- or RTS-specific table family.

This recommendation follows the shipped v0.13 effectivity baseline: effectivity is caller-owned descriptive state attached to a relationship link, not a separate fluent API, metadata kind, entity family, validation layer, or technical-column family. The visible core model remains finite: produced table kinds are hub, link, satellite, PIT/point-in-time, and bridge, while satellite metadata supports ordinary satellites and multi-active satellites with driving keys. Privacy terminology must not be used to widen those core Data Vault semantics.

Future privacy-specific labels, validation rules, annotations, conventions, or helper APIs may still be added inside the optional privacy extension boundary. They must compile to the existing provider-neutral metadata, save, read, diagnostics, and provider-extension seams instead of adding first-class STS/RTS core entity families or changing default behavior for callers that did not opt in.

## Provider-Neutral EF Core Boundary

Shared contracts must stay provider-neutral. The shared package may define abstractions, request models, metadata markers, diagnostics facts, validation rules, and fallback behavior that do not depend on one database provider.

Provider-specific privacy behavior must sit behind provider package seams such as `AddDVaultSqlite()`, `AddDVaultPostgres()`, `AddDVaultSqlServer()`, `AddDVaultMySql()`, `AddDVaultOracle()`, `AddDVaultDb2()`, or later provider-specific extensions. A provider package may optimize eligible privacy work only when it can prove compatibility for that provider and decline unsupported shapes without changing caller-visible semantics.

The core package must not infer privacy capabilities from provider names alone, must not promise provider-specific DDL from the provider-neutral surface, and must not require every provider to implement the same privacy optimization before the shared opt-in contract can exist. Unsupported providers and unsupported shapes must fall back to a bounded provider-neutral behavior or fail with explicit diagnostics, depending on the future capability contract.

## Personal-Data Satellite Metadata

The authoritative model-first shape for personal-data satellite field metadata is the additive `personalData` contract in `docs/plans/dvault-model-v1-schema-contract.md`. That contract marks existing satellite `payload` fields by exact logical name and assigns one stable provider-neutral `encryptedPayloadAlias` per marked field.

This metadata is descriptive unless a later opt-in privacy package consumes it. It does not create encryption behavior by itself, does not replace the base satellite payload declaration, and does not imply any provider column, ciphertext store type, algorithm, key id, generated SQL, migration, or DDL shape. Unmarked payload fields remain ordinary payload fields.

The metadata surface applies only to satellite payload fields. It must not be used to tag hub business keys, link participant references, driving keys, hash keys, hash diffs, load timestamps, record sources, PIT rows, bridge rows, diagnostics payloads, or workflow orchestration state. Validators should reject unknown payload references, duplicate marked fields, duplicate encrypted-payload aliases within one satellite, non-payload targets, and provider-specific storage or execution fields before model application.

Personal-data metadata preserves Data Vault semantics. Satellite parent identity, row history, hash-diff presence, multi-active driving-key behavior, load timestamp, record source, and provider-neutral EF payload/logical-property mapping remain compatible with the existing baseline. Downstream parser, code-first or registry registration, EF translation, optional privacy package behavior, and provider-specific execution lanes must be implemented through follow-on tickets that consume this metadata contract instead of redefining it.

## Provider-Native Encryption Decision

For the v0.44 privacy-extension baseline, DVault may pursue only caller-invoked, provider-neutral encrypted payload mapping in the shared surface. The approved shared lane is an explicit helper, metadata marker, or EF Core value-conversion proof that stores caller-prepared encrypted payload values through ordinary DVault model mapping. It must keep key material, key lookup, encryption policy, decryption policy, and operation timing owned by the consuming application.

The current supported-provider baseline for this decision is finite: SQLite, PostgreSQL, SQL Server, MySQL through `MySql.EntityFrameworkCore` or `Pomelo.EntityFrameworkCore.MySql`, Oracle, and DB2. The baseline does not create a separate MariaDB capability profile or any guarantee for providers outside that set.

Database-native encryption features are guidance-only for v0.44 and are not DVault shared-runtime behavior:

- database-at-rest or deployment features such as SQL Server TDE, PostgreSQL deployment or TDE posture, Oracle TDE, MySQL or MariaDB tablespace or file encryption, SQLite encrypted-file builds, and DB2 native database encryption remain caller, operator, or database-admin responsibilities;
- application-integrated provider features such as SQL Server Always Encrypted, PostgreSQL `pgcrypto`, Oracle `DBMS_CRYPTO`, MySQL or MariaDB SQL crypto functions, provider driver key-store integration, provider-specific encrypted column DDL, and row or cell encryption functions stay outside the shared v0.44 contract;
- database-level at-rest encryption must not be documented or diagnosed as equivalent to DVault field-level privacy semantics.

The shared core must not probe for provider-native encryption capabilities, branch on provider-native encryption availability, issue provider-specific encryption DDL or SQL functions, configure provider key stores, or negotiate database encryption modes. A future provider-native encryption lane requires a separate provider-specific ticket that names one provider and one exact capability, owns the provider package surface, defines diagnostics and fallback behavior, supplies tests, and records evidence before DVault can expose it.

## Ownership Boundary

DVault-owned responsibilities are limited to library behavior that can be implemented and tested inside the package boundary:

- provider-neutral public contracts for opt-in privacy extension points;
- deterministic metadata interpretation when privacy metadata is supplied;
- explicit helper or strategy dispatch for caller-invoked operations;
- diagnostics that identify selected strategy, fallback, unsupported shape, and redaction-safe evidence;
- provider package registration seams and provider-owned strategy implementations when later tickets approve them;
- documentation of finite non-goals, fallback behavior, and application-owned inputs.

Application and operator responsibilities remain outside DVault:

- compliance interpretation, legal review, data-protection-impact analysis, and controller or processor policy decisions;
- database provisioning, provider selection, schema deployment, migrations, backups, restore policy, and environment isolation;
- credentials, secret storage, key lifecycle, key rotation, key escrow, HSM or KMS integration, and access-control policy;
- transaction scope, retry policy, operational scheduling, background workers, retention jobs, purge workflows, archival, and audit workflow routing;
- deciding which fields are personal data, which transformations are appropriate, and how transformed data is presented to users or downstream systems.

DVault may provide explicit extension points that applications call from those workflows, but ownership of the workflows themselves stays with the consuming application.

## Non-Goals

This story does not approve:

- a claim that DVault, the privacy add-on, or any provider package makes an application GDPR/DSGVO compliant;
- legal advice, compliance certification, records-of-processing automation, data-subject workflow orchestration, consent management, breach notification, or policy attestation;
- a key-management platform, secret vault, HSM abstraction, KMS integration layer, key escrow service, or automatic key rotation orchestration;
- automatic deletion, retention scheduling, purge orchestration, archival orchestration, backfill orchestration, or background workflow ownership;
- default `SaveChanges` interception as the primary privacy behavior path;
- implicit encryption, pseudonymization, redaction, export filtering, or deletion for callers that did not opt in;
- provider-native cell, column, row, tablespace, file, or database encryption as a shared v0.44 runtime feature;
- provider-specific DDL, migrations, storage optimizations, generated SQL artifacts, or runtime dispatch unless a later implementation ticket approves the exact provider lane.

For example, a future helper that encrypts a satellite payload when a caller supplies a key provider and invokes an explicit save helper can fit this boundary. A background service that scans all DVault tables nightly, chooses retention policy, deletes rows, rotates keys, and reports compliance status does not fit this boundary.

## Existing Surface Compatibility

The privacy add-on must preserve the existing explicit DVault architecture:

- `AddDVault()` remains the provider-neutral default service registration path.
- `IDataVaultSaveService` remains the default explicit write boundary for caller-supplied load timestamp, record source, and Data Vault row intent.
- `IDataVaultReadService` and `IDataVaultReadDiagnosticsService` remain explicit read and diagnostics surfaces.
- PIT and bridge maintenance stay caller-driven and are not converted into background refresh or deletion workflows.
- Provider packages may add strategy registrations around the same shared contracts, with unsupported providers or shapes declining to provider-neutral behavior where the future contract defines a fallback.

Stable hashing and telemetry can inform diagnostics and evidence language, but they are not encryption controls, key-management controls, compliance controls, or privacy guarantees. Hash-key generation remains a Data Vault identity mechanism unless a later security-specific contract proves and documents a separate privacy capability.

## Follow-On Work

Concrete privacy capabilities must be implemented through separate tickets after this boundary is accepted. Candidate follow-on tickets include:

- model-first parser support for `personalData` satellite field metadata and its validation diagnostics;
- code-first or registry APIs for registering the same payload-field and encrypted-payload-alias metadata;
- EF metadata translation or diagnostics that expose provider-neutral personal-data metadata without changing ordinary payload mapping;
- field-level encryption with caller-owned key-provider integration;
- provider-neutral encrypted payload value-conversion proof with caller-owned key-provider integration;
- pseudonymization helpers for selected hub, link, or satellite fields;
- redaction or export controls for explicit read paths;
- retention metadata that applications can inspect without DVault owning retention execution;
- provider-specific optimization or DDL lanes for a named provider when implementation evidence exists.

Each follow-on ticket should name the exact capability, package ownership, opt-in API, provider scope, fallback behavior, diagnostics evidence, and tests. No follow-on ticket should treat this boundary as approval for compliance guarantees, KMS ownership, automatic deletion workflows, or default behavior changes for existing callers.
