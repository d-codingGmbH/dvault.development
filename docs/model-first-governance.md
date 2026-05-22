# Model-First Governance Workflow

Status: v0.17.0 public guidance

This guide describes how teams should use governed `dvault.model.v1` JSON artifacts alongside the existing Code-First and metadata-first DVault paths. Earlier release notes remain historical package context; `docs/releases/v0.17.0.md` is the current public baseline for Code-First parity, analyzer/generator ergonomics, EF misuse diagnostics, model-first import, export, projection, design-time drift, explicit snapshot-model preflight, optional live-schema drift guidance, migration guardrail outcomes, aggregate preflight, explicit bulk ingestion, provider benchmark evidence, explicit PIT/bridge maintenance, current/as-of convenience reads, SQLite PIT/bridge read optimization, opt-in save/read telemetry, and redacted support-bundle export.

## Choose A Declaration Path

Use Code-First declarations when the Data Vault model is local to one EF model and fits the implemented fluent surface for hubs, hub-parent satellites, link-parent satellites, multi-active driving keys, explicit or derived hub links, and explicitly named repeated same-hub links with distinct participant roles. This keeps schema intent close to `OnModelCreating` through `ApplyDataVaultMetadata(vault => ...)` and is the simplest application-local path.

Use metadata-first registry-backed metadata when one shared authoritative `DataVaultMetadataModel` or `DataVaultMetadataRegistry` should drive EF projection, explicit save requests, typed latest/as-of reads, diagnostics, examples, or provider setup. Register that model or registry through `AddDVault(...)` and opt DbContexts into it with `UseDataVaultMetadata()`.

Use model-first governance when the authoritative model should be a reviewed, versioned `dvault.model.v1` JSON artifact. This path is intended for source-controlled artifact reviews, strict JSON import diagnostics, projection into EF metadata, canonical JSON export from fluent Code-First declaration callbacks or already-materialized metadata, and drift reports used as review evidence.

## Artifact Baseline

`dvault.model.v1` ingestion is JSON-first. The artifact must use the exact top-level `schemaVersion` string `dvault.model.v1`. A v1 consumer must reject missing values, non-string values, `dvault.model`, `dvault.model.v1.0`, `dvault.model.v2`, vendor-prefixed dialects, and any other future schema version until a separate future contract explicitly supports it.

Canonical v1 JSON uses the stable top-level declaration categories `hubs`, `links`, `satellites`, `pits`, and `bridges`, with `naming.policy` defaulting to `default` and `loadTimestampStorage` defaulting to `provider-default`. The supported `loadTimestampStorage` tokens are `provider-default`, `iso-8601-utc-text`, and `utc-ticks`. Unknown fields are errors at every object level, including nested `naming`, parent reference, participant, and bridge endpoint objects. Review declaration array order as part of the model because import, export, projection, and drift evidence preserve stable declaration ordering.

```json
{
  "schemaVersion": "dvault.model.v1",
  "naming": {
    "policy": "default"
  },
  "loadTimestampStorage": "provider-default",
  "hubs": [
    {
      "name": "Customer",
      "businessKeys": ["CustomerId", "RegionCode"]
    },
    {
      "name": "Order",
      "businessKeys": ["OrderId"]
    }
  ],
  "links": [
    {
      "name": "CustomerOrder",
      "participants": [
        { "hub": "Customer" },
        { "hub": "Order" }
      ]
    },
    {
      "name": "CustomerIdentityMatch",
      "participants": [
        { "hub": "Customer", "role": "SourceCustomer" },
        { "hub": "Customer", "role": "MatchedCustomer" }
      ]
    }
  ],
  "satellites": [
    {
      "name": "CustomerProfile",
      "parent": {
        "kind": "hub",
        "name": "Customer"
      },
      "payload": ["Name", "EmailAddress"],
      "drivingKeys": []
    },
    {
      "name": "CustomerOrderState",
      "parent": {
        "kind": "link",
        "name": "CustomerOrder"
      },
      "payload": ["StatusCode", "StateChangedAt"],
      "drivingKeys": ["StateSource"]
    }
  ],
  "pits": [
    {
      "name": "CustomerPit",
      "hub": "Customer",
      "satellites": ["CustomerProfile"]
    }
  ],
  "bridges": [
    {
      "name": "CustomerOrderBridge",
      "kind": "many-to-many",
      "source": "CustomerOrder",
      "endpoints": {
        "from": { "hub": "Customer" },
        "to": { "hub": "Order" }
      }
    }
  ]
}
```

External YAML can be used only as an authoring convenience before DVault sees the model. Convert YAML outside DVault and review the converted canonical JSON artifact. The first-party ingestion API accepts JSON and does not define direct YAML parser semantics, YAML fixture contracts, or a core package YAML dependency.

## Review Workflow

Store the canonical JSON artifact in source control and review changes like source code. Reviewers should check the exact `schemaVersion`, `naming.policy`, `loadTimestampStorage`, declaration ordering, parent references, participant roles, PIT satellite membership, bridge endpoints, and the absence of unknown fields.

Import the artifact with `DataVaultModelArtifactImporter.ImportJson` and treat `DataVaultModelImportResult.Diagnostics` as validation evidence. A valid import exposes `MetadataModel`, `MetadataRegistry`, and the declared `LoadTimestampStorage` for downstream projection and review steps.

```csharp
using DCoding.Data.DVault;

var json = await File.ReadAllTextAsync("models/sales-vault.json", cancellationToken);
var importResult = DataVaultModelArtifactImporter.ImportJson(json, "models/sales-vault.json");

if (!importResult.IsValid) {
  var diagnostics = string.Join(
      Environment.NewLine,
      importResult.Diagnostics.Select(diagnostic =>
          $"{diagnostic.Severity} {diagnostic.Category} {diagnostic.Code} {diagnostic.LogicalSourcePath}{diagnostic.JsonPointer}: {diagnostic.Message}"));
  throw new InvalidOperationException(diagnostics);
}
```

Project a successful import into EF metadata through DbContext configuration with `UseDataVaultMetadata(DataVaultModelImportResult)`. This is the model-first equivalent of opting a context into an explicit metadata registry.

```csharp
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

services.AddDVault(options => options.UseMetadataModel(importResult));
services.AddDbContext<SalesVaultContext>(options => {
  options.UseSqlite(connectionString);
  options.UseDataVaultMetadata(importResult);
});
```

Export canonical JSON from fluent Code-First declarations or already-materialized metadata with `DataVaultModelArtifactExporter.ExportJson`. The exporter accepts a Code-First declaration callback, `DataVaultMetadataModel`, and `DataVaultMetadataRegistry`; it does not export EF `ModelBuilder` state, runtime save/read state, or legacy `PointInTimeTables` metadata.

```csharp
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;

string jsonFromCodeFirst = DataVaultModelArtifactExporter.ExportJson(vault => {
  vault.Hub<Customer>(hub => hub.BusinessKey(customer => customer.CustomerId));
  vault.Hub<Order>(hub => hub.BusinessKey(order => order.OrderId));
  vault.Link("CustomerOrder", link => {
    link.Participant<Customer>();
    link.Participant<Order>();
  });
});

DataVaultMetadataRegistry registry = DataVaultMetadataRegistry.Create(metadataModel);
string jsonFromRegistry = DataVaultModelArtifactExporter.ExportJson(registry);
string jsonFromModel = DataVaultModelArtifactExporter.ExportJson(metadataModel);
```

Compare the expected artifact or metadata model against generated/current EF metadata with `DataVaultModelDriftReporter.Compare`. Use the structured differences and `ToDisplayString()` as review evidence before accepting a model change. This comparison path remains design-time EF metadata comparison and does not open a database connection.

When a reachable database must be checked, use the separate bounded live-schema path: `DataVaultLiveSchemaReader.ReadAsync(context)` captures DVault-owned tables, ordered columns, named primary-key constraints, and secondary indexes, and `DataVaultLiveSchemaDriftReporter.Compare` compares that result with expected metadata or an imported model. Built-in reader coverage includes SQLite, PostgreSQL, SQL Server, Oracle, and MySQL. Both `MySql.EntityFrameworkCore` and `Pomelo.EntityFrameworkCore.MySql` map to the MySQL reader. Providers without a built-in reader return `UnsupportedProvider`, and reachable-provider failures return `Unavailable`, so requested live checks do not silently pass.

```csharp
using DCoding.Data.DVault;

using var context = new SalesVaultContext(options);
var report = DataVaultModelDriftReporter.Compare(importResult, context);

if (report.HasBlockingDifferences) {
  throw new InvalidOperationException(report.ToDisplayString());
}
```

```csharp
using DCoding.Data.DVault;

using var context = new SalesVaultContext(options);
var liveSchema = await DataVaultLiveSchemaReader.ReadAsync(context);
var liveReport = DataVaultLiveSchemaDriftReporter.Compare(importResult, liveSchema, DataVaultProviderCapabilityProfiles.Sqlite);
```

## Workflow Test Evidence

Run the focused design-time workflow coverage from the repository root with:

```sh
dotnet test DVault.slnx --nologo --filter FullyQualifiedName~DataVaultModelFirstDesignTimeWorkflowTests
```

The valid workflow imports the representative `models/sales-vault.json` `dvault.model.v1` fixture with `DataVaultModelArtifactImporter.ImportJson`, configures a SQLite-backed design-time context with `UseDataVaultMetadata(importResult)`, and compares the imported model against generated EF metadata with `DataVaultModelDriftReporter.Compare(importResult, context)`. The expected valid outcome is `report.HasBlockingDifferences == false` and `report.ToDisplayString()` reporting no differences. SQLite is used only for provider selection and EF metadata shape; the workflow does not open a database connection or initialize schema.

The live-schema workflow is separate from the design-time workflow above. Required local live-schema coverage uses SQLite and does initialize a test database. PostgreSQL, SQL Server, Oracle, and MySQL live-schema readers are built in, but their tests and CI lanes remain opt-in behind the existing provider connection-string environment variables because the consuming environment owns reachable databases, credentials, lifecycle cleanup, and isolation. The invalid workflow imports the same logical source path, `models/sales-vault.json`, with unsupported `schemaVersion` value `dvault.model.v2`. The expected invalid outcome is one import diagnostic with code `DMV1002`, category `schema-version`, logical source path `models/sales-vault.json`, and JSON Pointer `/schemaVersion`.

## Diagnostic Contract

Model-first import and projection diagnostics use the stable `DMV####` id format. The `DMV` prefix is the v1 DVault model-artifact diagnostic family, and the four-digit numeric suffix is compared and reported as an ordinal string. Existing `DMV####` ids are stable once shipped; do not rename them to a different prefix or reuse an existing code for a different meaning.

The central catalog in `DCoding.Data.DVault` is the source of truth for model-artifact diagnostic definitions. Every catalog entry must store these fields on the definition itself: `Code`, `Severity`, `Category`, `Summary`, `Explanation`, and `Remediation`. Affected locations are emitted per diagnostic instance, not stored as catalog metadata. Import diagnostics use JSON Pointer values when the parser can identify the artifact element, and APIs that receive a logical source path preserve that path separately as `LogicalSourcePath`.

The seeded v1 baseline is the importer/projection family below, in ascending code order. All current entries are `error` severity.

| Code | Category | Summary | Remediation |
| --- | --- | --- | --- |
| `DMV1001` | `schema-version` | Missing schema version | Add `schemaVersion` with the supported value `dvault.model.v1`. |
| `DMV1002` | `schema-version` | Unsupported schema version | Change `schemaVersion` to `dvault.model.v1` or process the artifact with a compatible importer. |
| `DMV1101` | `shape` | Unknown artifact field | Remove the unknown field or move the information into a supported artifact field. |
| `DMV1102` | `shape` | Invalid artifact shape | Correct the JSON shape so required objects, arrays, and string values use the documented structure. |
| `DMV1103` | `shape` | Empty required collection | Add the required entries to the collection before importing the artifact. |
| `DMV1201` | `duplicate` | Duplicate declaration name | Rename or remove the duplicate declaration so each logical name is unique within its declaration kind. |
| `DMV1202` | `duplicate` | Duplicate member or participant name | Remove the repeated value or choose a distinct name for each member in the affected declaration. |
| `DMV1203` | `duplicate` | Duplicate PIT or bridge binding | Use each satellite reference once and bind bridge endpoints to distinct source-link participants. |
| `DMV1301` | `reference` | Missing model reference | Declare the referenced model element or update the reference to an existing element of the required kind. |
| `DMV1302` | `reference` | Wrong reference kind | Point the reference at an element of the expected kind or correct the declaration's reference kind. |
| `DMV1303` | `reference` | PIT satellite parent mismatch | Reference only satellites whose parent hub matches the PIT hub. |
| `DMV1401` | `naming` | Default naming collision | Rename one of the colliding declarations or roles so default naming produces unique names. |
| `DMV1501` | `capability` | Unsupported metadata capability | Use only supported `dvault.model.v1` capabilities or split the model into declarations the current runtime can map. |
| `DMV1502` | `provider-choice` | Unsupported provider-specific choice | Remove provider-specific fields or use one of the provider-neutral choices supported by the importer. |
| `DMV1601` | `recursive-participant-binding` | Ambiguous recursive participant binding | Declare distinct participant roles and bind hierarchy endpoints to unambiguous role-specific participants. |
| `DMV1602` | `recursive-participant-binding` | Recursive link role required | Assign explicit roles to each repeated participant so the recursive link can be resolved deterministically. |
| `DMV1701` | `shape` | Driving key overlaps payload | Remove the overlapping field from either `drivingKeys` or `payload` so each field has one satellite role. |
| `DMV1801` | `projection` | Artifact projection failed | Review the projection error, adjust the affected declaration, and retry the import before applying metadata. |

For a parse diagnostic, an unsupported schema version in `models/sales-vault.json` reports the catalog-backed code and category while preserving the JSON Pointer to the value:

```text
error schema-version DMV1002 models/sales-vault.json/schemaVersion: Unsupported schemaVersion 'dvault.model.v2'. Expected 'dvault.model.v1'.
```

The remediation comes from the `DMV1002` catalog definition: change `schemaVersion` to `dvault.model.v1` or process the artifact with a compatible importer.

For a projection diagnostic, the logical source path stays instance-bound alongside the offending declaration pointer. A projection failure for the first PIT declaration in `models/sales-vault.json` reports `LogicalSourcePath` as `models/sales-vault.json`, `JsonPointer` as `/pits/0`, and formats the affected location as `models/sales-vault.json/pits/0`:

```text
error projection DMV1801 models/sales-vault.json/pits/0: The imported artifact could not be projected to Entity Framework metadata: <projection error>
```

The remediation comes from the `DMV1801` catalog definition: review the projection error, adjust the affected declaration, and retry the import before applying metadata.

## Versioning Rules

Keep `dvault.model.v1` strict and additive only through an explicit future contract. Current v1 artifacts must use the exact `schemaVersion`, the `default` naming policy, one of the supported `loadTimestampStorage` tokens, and the stable declaration categories `hubs`, `links`, `satellites`, `pits`, and `bridges`.

Do not use unknown fields for comments, vendor metadata, experimental parser hints, or future v2 values inside v1 artifacts. If a future schema adds fields or changes semantics, introduce a distinct schema version and keep v1 consumers rejecting it until they deliberately implement that contract.

## Current Limitations

The current baseline provides reusable library-hosted design-time command verbs through a consumer-owned command host, but it does not ship a standalone DVault CLI, intercept `dotnet ef`, apply migrations, or repair schema drift automatically. The command verbs are `validate`, `export`, `drift`, `guardrail`, and `support-bundle`; `export` is for artifact maintenance and reviewed refresh workflows, not the default blocking CI gate. `DataVaultPreflight.Run(...)` can aggregate validation/provider explain, reviewed-artifact drift, explicit snapshot-model drift, migration guardrail, and representative request diagnostics when the consumer supplies those inputs. `support-bundle` emits a deterministic redacted `dvault.support-bundle.v1` JSON artifact from the consumer-owned design-time host, with reviewed-artifact and live-schema sections remaining explicit opt-ins. Direct YAML ingestion, automatic snapshot discovery, automatic reviewed-artifact discovery, automatic representative request generation, and extraction from arbitrary EF `ModelBuilder` state into a model artifact remain outside the current contract.

The model-first APIs operate on JSON artifacts, fluent Code-First declaration callbacks, and already-materialized metadata. The current `dvault.model.v1` artifact contract can represent repeated same-hub participant roles and link-parent satellites, including effectivity modeled as ordinary caller-owned link-parent satellite state. Dependent child key modeling, same-hub typed mapper or source-generator parity, and effectivity-specific fluent APIs remain outside the current public claim set. Keep standalone CLI packaging, YAML semantics, database provisioning, secret-management recipes, and EF model reverse-engineering as separate future contracts instead of implying support in current examples.

Telemetry and SaveChanges runtime guarding are also outside the model declaration contract. `AddDVaultTelemetry()` is an explicit application startup choice for observing DVault save/read attempts; it does not change artifact import, projection, drift comparison, support-bundle generation, or model validation behavior. `UseDataVaultSaveChangesGuardInterceptor(...)` is a separate explicit opt-in for warning or blocking unsafe generated-row `SaveChanges` patterns; it does not replace `IDataVaultSaveService` or make model-first artifacts an automatic runtime write gate.
