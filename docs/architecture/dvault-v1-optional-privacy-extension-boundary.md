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

## Provider-Neutral EF Core Boundary

Shared contracts must stay provider-neutral. The shared package may define abstractions, request models, metadata markers, diagnostics facts, validation rules, and fallback behavior that do not depend on one database provider.

Provider-specific privacy behavior must sit behind provider package seams such as `AddDVaultSqlite()`, `AddDVaultPostgres()`, `AddDVaultSqlServer()`, `AddDVaultMySql()`, `AddDVaultOracle()`, `AddDVaultDb2()`, or later provider-specific extensions. A provider package may optimize eligible privacy work only when it can prove compatibility for that provider and decline unsupported shapes without changing caller-visible semantics.

The core package must not infer privacy capabilities from provider names alone, must not promise provider-specific DDL from the provider-neutral surface, and must not require every provider to implement the same privacy optimization before the shared opt-in contract can exist. Unsupported providers and unsupported shapes must fall back to a bounded provider-neutral behavior or fail with explicit diagnostics, depending on the future capability contract.

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

- field-level encryption with caller-owned key-provider integration;
- pseudonymization helpers for selected hub, link, or satellite fields;
- redaction or export controls for explicit read paths;
- retention metadata that applications can inspect without DVault owning retention execution;
- provider-specific optimization or DDL lanes for a named provider when implementation evidence exists.

Each follow-on ticket should name the exact capability, package ownership, opt-in API, provider scope, fallback behavior, diagnostics evidence, and tests. No follow-on ticket should treat this boundary as approval for compliance guarantees, KMS ownership, automatic deletion workflows, or default behavior changes for existing callers.
