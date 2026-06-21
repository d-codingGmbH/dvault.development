# Getting Started

This guide expands the root README quickstart without turning DVault into an application platform. DVault remains an EF Core library family: applications own provider configuration, database provisioning, transactions, scheduling, credentials, and deployment.

## Choose The Metadata Boundary

DVault supports three compatible declaration paths:

- Code-First declarations in `OnModelCreating` for application-local EF models.
- Metadata-first declarations through `DataVaultMetadataModel` or `DataVaultMetadataRegistry` when one public metadata object should drive schema projection, explicit saves, reads, diagnostics, support bundles, examples, and provider setup.
- Model-first governance through reviewed `dvault.model.v1` JSON artifacts when metadata should be imported, exported canonically, compared for drift, and routed through support-bundle export.

Pick one authoritative path per model boundary. The other paths are alternatives for different ownership needs, not additional layers that must all be used.

## Register Services

Use the provider-neutral registration plus the provider extension package that matches the EF Core database provider configured for the `DbContext`. New projects should select the binary-first profile explicitly while keeping DVault persistence caller-driven.

```csharp
services.AddDVault(options => options.UseBinaryFirstProfile());
services.AddDVaultSqlite();

services.AddDbContext<SalesVaultContext>(options =>
    options.UseSqlite(connectionString));
```

The binary-first profile changes the recommended physical hash-key storage for new generated schemas; it does not migrate existing databases or configurations automatically. Existing `HexString`-compatible setups remain valid until the application owner intentionally plans and executes a separate reviewed migration, reset, or data-move change. Logical and public hash-key values stay lowercase hexadecimal strings even when binary physical storage is selected for new projects.

Provider packages can register provider capability profiles, behavior, diagnostics, read strategies, or save strategies behind the shared `IDataVaultSaveService` and `IDataVaultReadService` boundaries. They do not replace the application's normal EF Core provider configuration.

## Declare Metadata

Code-First declarations are additive over EF Core model building. Business keys, participants, driving keys, and payloads use direct scalar member selectors. Composite keys use repeated calls in canonical order.

```csharp
modelBuilder.ApplyDataVaultMetadataWithBinaryFirstProfile(vault => {
  vault.Hub<Customer>(hub => {
    hub.BusinessKey(customer => customer.CustomerId);
    hub.Satellite("Profile", satellite => {
      satellite.Payload(customer => customer.Name);
      satellite.Payload(customer => customer.Status);
    });
  });

  vault.Link("CustomerOrder", link => {
    link.Participant<Customer>();
    link.Participant<Order>();
  });
});
```

`UseDataVaultBinaryFirstProfile()` followed by `ApplyDataVaultMetadata(...)` remains supported for existing callers that already use the separate prelude. Plain `ApplyDataVaultMetadata(...)` without an explicit binary-first opt-in keeps the compatible `HexString` default.

For shared metadata, build or import a `DataVaultMetadataModel` and register it with EF options through the documented metadata APIs. For reviewed JSON artifacts, use the model-first workflow in [Model-First Governance](model-first-governance.md).

## Save Explicitly

`IDataVaultSaveService` is the public write entry point. The caller supplies load timestamp, record source, ordering, cancellation, and transaction context. DVault does not intercept ordinary EF `SaveChanges` to create vault rows.

Use `DataVaultSaveRequest` for direct ordered writes, `DataVaultBulkSaveRequest` when the full ordered request set is already materialized, `DataVaultChunkedSaveRequest` when the caller has bounded chunks, and async chunk/source helpers when the producer naturally yields bounded chunks asynchronously.

Provider-specific save strategies are optimizations around that same public contract. When a provider strategy cannot safely run, the implementation falls back to a smaller native path or the provider-neutral writer as documented by diagnostics.

## Read And Maintain

DVault exposes service boundaries for latest/as-of satellite reads, PIT maintenance/read flows, bridge maintenance/read flows, diagnostics, explain metadata, and support-bundle export. Typed read-model generation is opt-in and support-bundle-driven; it does not parse raw Code-First callbacks or model-first JSON directly.

PIT and bridge maintenance are explicit caller-invoked operations. They are not scheduler-driven, trigger-driven, read-time maintenance, or provider-specific background jobs.

## Hashing And Storage

The default stable hash algorithm id is `sha256-v1`. Built-in non-default ids such as `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1` are explicit opt-in choices for non-adversarial Data Vault identity hashing only.

Hash-key values stay logical lowercase hexadecimal strings in public APIs. `HexString` is the default compatible physical storage profile. `Binary` is an opt-in physical storage profile for generated hash-key columns when the application has planned migration, index, and provider evidence.

For existing persisted databases, use the [Hash-Key Storage Migration Guide](hash-key-storage-migration.md) before changing hash-key storage profile. Binary-first setup for new schemas is not an automatic migration path for existing `HexString` storage.

## Next Documents

- [Production Adoption Checklist](production-adoption-checklist.md)
- [Hash-Key Storage Migration Guide](hash-key-storage-migration.md)
- [Performance Profiles](performance-profiles.md)
- [Local Validation](local-validation.md)
- [Manual NuGet Publication Checklist](manual-nuget-publication.md)
- [Analyzer README](../src/DCoding.Data.DVault.Analyzers/README.md)
