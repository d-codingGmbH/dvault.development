# Model-First Governance Workflow

Status: v0.7.0 branch documentation

This guide describes how teams should use governed `dvault.model.v1` JSON artifacts alongside the existing Code-First and metadata-first DVault paths. The v0.6.0 release notes remain historical package context; they are not the current v0.7.0 branch baseline for model-first import, export, projection, or drift comparison APIs.

## Choose A Declaration Path

Use Code-First declarations when the Data Vault model is local to one EF model and fits the implemented fluent surface for hubs, hub-parent satellites, multi-active driving keys, and ordered hub links. This keeps schema intent close to `OnModelCreating` through `ApplyDataVaultMetadata(vault => ...)` and is the simplest application-local path.

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
          $"{diagnostic.Severity} {diagnostic.Code} {diagnostic.LogicalSourcePath}{diagnostic.JsonPointer}: {diagnostic.Message}"));
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

Compare the expected artifact or metadata model against generated/current EF metadata with `DataVaultModelDriftReporter.Compare`. Use the structured differences and `ToDisplayString()` as review evidence before accepting a model change. Drift comparison is design-time EF metadata comparison; it does not inspect a live database.

```csharp
using DCoding.Data.DVault;

using var context = new SalesVaultContext(options);
var report = DataVaultModelDriftReporter.Compare(importResult, context);

if (report.HasBlockingDifferences) {
  throw new InvalidOperationException(report.ToDisplayString());
}
```

## Versioning Rules

Keep `dvault.model.v1` strict and additive only through an explicit future contract. Current v1 artifacts must use the exact `schemaVersion`, the `default` naming policy, one of the supported `loadTimestampStorage` tokens, and the stable declaration categories `hubs`, `links`, `satellites`, `pits`, and `bridges`.

Do not use unknown fields for comments, vendor metadata, experimental parser hints, or future v2 values inside v1 artifacts. If a future schema adds fields or changes semantics, introduce a distinct schema version and keep v1 consumers rejecting it until they deliberately implement that contract.

## Current Limitations

The current branch does not provide first-party CLI commands, documented CI gate snippets, direct YAML ingestion, live database drift introspection, or extraction from arbitrary EF `ModelBuilder` state into a model artifact.

The model-first APIs operate on JSON artifacts, fluent Code-First declaration callbacks, and already-materialized metadata. Keep command-line automation, CI policy examples, YAML semantics, database schema inspection, and EF model reverse-engineering as separate future contracts instead of implying support in current examples.
