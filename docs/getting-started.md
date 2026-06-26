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

For PostgreSQL, install `DCoding.Data.DVault.Postgres` and the ordinary EF Core provider package `Npgsql.EntityFrameworkCore.PostgreSQL`, then keep the same binary-first DVault registration while switching the provider extension and DbContext configuration:

```csharp
services.AddDVault(options => options.UseBinaryFirstProfile());
services.AddDVaultPostgres();

services.AddDbContext<SalesVaultContext>(options =>
    options.UseNpgsql(connectionString));
```

The checked-in PostgreSQL quickstart and live PostgreSQL provider tests are optional local flows gated by `DVAULT_TEST_POSTGRES_CONNECTION_STRING`. Use the existing [PostgreSQL quickstart fixture](../examples/DCoding.Data.DVault.PostgresQuickstart/README.md) and [local validation](local-validation.md#postgresql) notes when you want to run them. DVault does not create PostgreSQL containers, databases, users, credentials, or deployment infrastructure.

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
});
```

`UseDataVaultBinaryFirstProfile()` followed by `ApplyDataVaultMetadata(...)` remains supported for existing callers that already use the separate prelude. Plain `ApplyDataVaultMetadata(...)` without an explicit binary-first opt-in keeps the compatible `HexString` default.

For shared metadata, build or import a `DataVaultMetadataModel` and register it with EF options through the documented metadata APIs. For reviewed JSON artifacts, use the model-first workflow in [Model-First Governance](model-first-governance.md).

## Create The Quickstart Schema

DVault projects use the application's normal EF Core schema lifecycle. For a disposable first-run SQLite database, create the configured model before saving rows:

```csharp
using var serviceProvider = services.BuildServiceProvider(validateScopes: true);
using var scope = serviceProvider.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<SalesVaultContext>();
await context.Database.EnsureCreatedAsync(cancellationToken);
```

Use migrations or a reviewed deployment process instead of `EnsureCreatedAsync(...)` for production-owned schemas.

## Save Explicitly

`IDataVaultSaveService` is the public write entry point. The caller supplies load timestamp, record source, ordering, cancellation, and transaction context. DVault does not intercept ordinary EF `SaveChanges` to create vault rows.

Use `DataVaultSaveRequest` for direct ordered writes, `DataVaultBulkSaveRequest` when the full ordered request set is already materialized, `DataVaultChunkedSaveRequest` when the caller has bounded chunks, and async chunk/source helpers when the producer naturally yields bounded chunks asynchronously.

Provider-specific save strategies are optimizations around that same public contract. When a provider strategy cannot safely run, the implementation falls back to a smaller native path or the provider-neutral writer as documented by diagnostics.

The direct save request uses the same logical hub and satellite names declared in `OnModelCreating`. The load timestamp, record source, and satellite hash diff stay caller-owned and visible:

```csharp
var saveService = scope.ServiceProvider.GetRequiredService<IDataVaultSaveService>();
var loadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);
var customerHub = new DataVaultHubMetadata("Customer", ["CustomerId"]);
var customerProfile = new DataVaultSatelliteMetadata(
    "Profile",
    customerHub.ToReference(),
    ["Name", "Status"]);

var hubResult = await saveService.SaveAsync(
    context,
    new DataVaultSaveRequest(
        loadTimestamp,
        "crm-import",
        [
            new DataVaultHubSaveOperation(
                customerHub,
                [new("CustomerId", "C-100")]),
        ],
        []),
    cancellationToken);
var customerHashKey = hubResult.SavedRecords.Single(record =>
    record.Kind == DataVaultTableKind.Hub &&
    record.MetadataName == "Customer").HashKey;

await saveService.SaveAsync(
    context,
    new DataVaultSaveRequest(
        loadTimestamp,
        "crm-import",
        [],
        [],
        [
            new DataVaultSatelliteSaveOperation(
                customerProfile,
                customerHashKey,
                [
                    new("Name", "Alice Adams"),
                    new("Status", "prospect"),
                ],
                "profile-hash-001"),
        ]),
    cancellationToken);
```

## Read The Current Row

`IDataVaultReadService` provides latest/current satellite helpers over explicit parent hash keys. The minimal proof can read back the current customer profile without PIT setup, bridge setup, background jobs, or SaveChanges interception:

```csharp
var readService = scope.ServiceProvider.GetRequiredService<IDataVaultReadService>();

var latestProfiles = await readService.ReadLatestSatelliteAsync(
    context,
    new DataVaultLatestSatelliteReadRequest(customerProfile, [customerHashKey]),
    row => new {
      Name = row.RequiredString("Name"),
      Status = row.RequiredString("Status"),
      LoadTimestamp = row.RequiredDateTimeOffset("LoadTimestamp"),
      RecordSource = row.RequiredString("RecordSource"),
    },
    cancellationToken);
var latestProfile = latestProfiles.Single();
```

## Read And Maintain

DVault exposes service boundaries for latest/as-of satellite reads, PIT maintenance/read flows, bridge maintenance/read flows, diagnostics, explain metadata, and support-bundle export. Typed read-model generation is opt-in and support-bundle-driven; it does not parse raw Code-First callbacks or model-first JSON directly.

PIT and bridge maintenance are explicit caller-invoked operations. They are not scheduler-driven, trigger-driven, read-time maintenance, or provider-specific background jobs.

## Optional Privacy Proof

Install `DCoding.Data.DVault.Privacy` only when the application explicitly opts into the provider-neutral privacy proof package. The package provides registration, options, and alias-driven encrypted payload conversion on mapped payload properties through ordinary EF Core value conversion. It is not a GDPR/DSGVO compliance guarantee, automatic encryption feature, automatic redaction feature, provider-native encryption feature, retention engine, deletion workflow, or key-management platform.

Model-first personal-data metadata uses `personalData[].encryptedPayloadAlias` as the stable logical alias for a marked payload. The runtime privacy proof registers that same alias explicitly; it does not add a new metadata authoring API or infer aliases from database columns.

```json
{
  "payload": ["EmailAddress"],
  "personalData": [
    {
      "field": "EmailAddress",
      "encryptedPayloadAlias": "CustomerProfileEmailEncrypted"
    }
  ]
}
```

The provider passed to `UseCallerOwnedKeyProvider(...)` is typed as `IDataVaultPrivacyKeyProvider`. Encrypted payload conversion has a narrower runtime requirement: the configured provider must also implement `IDataVaultEncryptedPayloadKeyProvider`, because `DataVaultEncryptedPayloadValueConverter` asks it to approve and perform each conversion.

The checked-in SQLite quickstart is the local binary-first proof for this shape. It keeps the existing metadata-first DVault registration and SQLite provider setup, adds explicit privacy registration beside it, then writes one ordinary EF Core row whose mapped payload property uses the encrypted-payload converter. The same run still writes and reads the Data Vault history through `IDataVaultSaveService` and `IDataVaultReadService`; privacy conversion is visible only on the opt-in mapped property.

```csharp
using DCoding.Data.DVault;
using DCoding.Data.DVault.Privacy;
using DCoding.Data.DVault.Quickstarts.Shared;
using DCoding.Data.DVault.SqliteQuickstart;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddDVault(options => options
    .UseBinaryFirstProfile()
    .UseMetadataModel(QuickstartHistoryFlow.MetadataModel));
services.AddDVaultPrivacy(options => options
    .RegisterEncryptedPayloadAlias(SqlitePrivacyQuickstartFlow.CustomerProfileEmailEncryptedPayloadAlias)
    .UseCallerOwnedKeyProvider(new SqliteDemoEncryptedPayloadKeyProvider()));
services.AddDVaultSqlite();

services.AddDbContext<SqliteQuickstartVaultContext>(options => options
    .UseSqlite(connectionString)
    .UseDataVaultMetadata());
services.AddScoped<QuickstartVaultContext>(
    provider => provider.GetRequiredService<SqliteQuickstartVaultContext>());

public sealed class SqliteQuickstartVaultContext(
    DbContextOptions<SqliteQuickstartVaultContext> options,
    IDataVaultPrivacyConfiguration privacyConfiguration) : QuickstartVaultContext(options) {
  public DbSet<CustomerProfilePrivacyProofRow> CustomerProfilePrivacyProofs =>
      Set<CustomerProfilePrivacyProofRow>();

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<CustomerProfilePrivacyProofRow>(entity => {
      entity.ToTable("CustomerProfilePrivacyProof");
      entity.HasKey(row => row.Id);
      entity.Property(row => row.CustomerBusinessKey).IsRequired();
      entity.Property(row => row.EmailAddress)
          .IsRequired()
          .HasConversion(new DataVaultEncryptedPayloadValueConverter(
              privacyConfiguration,
              SqlitePrivacyQuickstartFlow.CustomerProfileEmailEncryptedPayloadAlias));
    });
  }
}
```

`SqliteDemoEncryptedPayloadKeyProvider` is only a caller-owned proof provider that makes the SQLite-friendly round trip visible. The quickstart prints whether the stored provider value uses the demo encrypted prefix and whether the converter returns the original value, without printing raw payload values, ciphertext, key material, connection strings, or provider messages. Production applications must replace the demo provider with their own cryptography, key lookup, rotation, authorization, diagnostics, and decline policy. DVault does not create, store, rotate, select, escrow, destroy, or recover key material.

The proof fails closed. If the alias is not registered with `RegisterEncryptedPayloadAlias(...)`, no key provider is wired with `UseCallerOwnedKeyProvider(...)`, the provider does not also satisfy `IDataVaultEncryptedPayloadKeyProvider`, or the provider declines a conversion, `DataVaultEncryptedPayloadValueConverter` throws instead of silently storing plaintext or silently treating ciphertext as decrypted payload data.

Crypto-shredding remains caller-owned. Withdrawing, losing, or destroying the caller-owned key material for an `encryptedPayloadAlias` means reads or writes for that alias fail closed. It does not delete rows, rewrite historical satellite values, clean PIT or bridge rows, purge backups, complete retention, or complete legal erasure.

Provider caveats stay bounded to ordinary EF Core mapping. The proof stores the provider value through a normal mapped payload property and is covered by the SQLite-friendly test path; it is not provider-specific encrypted DDL, transparent database encryption, a special encrypted column type, or a claim that any provider package performs native encryption.

The finite provider baseline for this caveat is SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2. MySQL follows the repository MySQL profile used for `MySql.EntityFrameworkCore` and Pomelo; it does not create a separate MariaDB capability profile. Provider-native encryption features remain guidance-only: SQL Server TDE or Always Encrypted, PostgreSQL deployment encryption or `pgcrypto`, Oracle TDE or `DBMS_CRYPTO`, MySQL SQL crypto or file or tablespace encryption, SQLite encrypted-file builds, and DB2 native database encryption stay outside the shared privacy runtime. DVault does not emit provider-native encrypted-column DDL, call provider SQL crypto functions, probe provider encryption capabilities, or route runtime behavior based on native encryption availability. Any future provider-native encryption support needs a separate provider-specific ticket or contract.

## Hashing And Storage

The default stable hash algorithm id is `sha256-v1`. Built-in non-default ids such as `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1` are explicit opt-in choices for non-adversarial Data Vault identity hashing only.

Hash-key values stay logical lowercase hexadecimal strings in public APIs. `HexString` is the default compatible physical storage profile. `Binary` is an opt-in physical storage profile for generated hash-key columns when the application has planned migration, index, and provider evidence.

For existing persisted databases, use the [Hash-Key Storage Migration Guide](hash-key-storage-migration.md) and review the generated `dvault.hash-key-storage-migration.v1` dry-run manifest before changing hash-key storage profile. Binary-first setup for new schemas is not an automatic migration path for existing `HexString` storage.

## Next Documents

- [Production Adoption Checklist](production-adoption-checklist.md)
- [Hash-Key Storage Migration Guide](hash-key-storage-migration.md)
- [Performance Profiles](performance-profiles.md)
- [DVault V1 Optional Privacy Extension Boundary](architecture/dvault-v1-optional-privacy-extension-boundary.md)
- [Local Validation](local-validation.md)
- [Manual NuGet Publication Checklist](manual-nuget-publication.md)
- [Analyzer README](../src/DCoding.Data.DVault.Analyzers/README.md)
