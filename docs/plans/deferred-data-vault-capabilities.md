# Deferred Data Vault Capability Decision Record

Status: v0.5 architecture decision with PIT and bridge metadata baselines
Ticket: 06EZ0NSHJVC9SD2KS6PWWNHPJM
Decision date: 2026-05-05

## Purpose

This record publishes the v0.5 architecture stance for deferred Data Vault capability families. It consolidates the earlier deferred-capabilities note and the optional advanced-configuration hook plan into one governing reference for PIT tables, bridge tables, multi-active satellites, and the hooks those features will need.

The record is intentionally architecture-level. It does not implement runtime row population behavior, define provider-specific optimization posture, or replace the current MVP hub, link, satellite, and SQLite-oriented baseline. The PIT and bridge stories now add bounded public metadata baselines for opt-in EF table projection while leaving PIT refresh, bridge traversal maintenance, and provider-specific physical behavior deferred.

## Decision

DVault v0.5 keeps the default path small and convention-first. The current baseline remains:

- Hub, link, and satellite concepts as the first Data Vault persistence model.
- Deterministic default conventions for technical names, metadata, stable hashing, load timestamps, and record sources.
- Optionless `AddDVault()` service registration.
- Convention-first `UseDataVault()` and `ApplyDataVaultMetadata()` model configuration, with the default capability profile remaining SQLite-oriented unless a provider profile is selected.
- The explicit `IDataVaultSaveService` write boundary, where callers supply load timestamp, record source, and vault row intent instead of relying on hidden `SaveChanges` interception.
- SQLite-backed examples, tests, and benchmark expectations as the required local baseline.

PIT table projection, bridge metadata projection, and multi-active satellites are opt-in capability families. They are valuable expansion work, but they must not become prerequisites for ordinary hub, link, and satellite setup. The current runtime `DataVaultPitMetadata` path supports provider-neutral projection, explicit maintenance, and PIT-backed reads for one hub or link parent plus ordered non-multi-active satellites attached to that same parent. Refresh orchestration, provider-specific physical optimization, model-first link-parent PIT artifacts, and multi-active PIT semantics remain deferred. For bridge tables, the source-backed provider-neutral metadata and EF shared-type projection baseline remains bounded; bridge row population orchestration, traversal scheduling, provider-specific physical optimization, complex traversal semantics, advanced hierarchy behavior, and PIT or multi-active interactions remain deferred.

Advanced hooks are also opt-in. Naming, hashing, record source, timestamp, and provider behavior may become configurable extension categories, but unset hooks inherit the default behavior. Future hook implementations can wrap or replace only their own category and must not make normal DVault setup require configuration.

## Why These Capabilities Are Deferred

The MVP baseline explains how DVault represents business identity, relationships, and descriptive history through hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources. That model is enough for the first portable SQLite-oriented package and for current EF metadata projection.

The deferred capability families need additional decisions that are not required to preserve that baseline:

| Capability family | Planning value | Why it stays opt-in for v0.5 | Downstream owner |
| --- | --- | --- | --- |
| PIT tables | Point-in-time tables can simplify historical joins across multiple satellites and make time-sliced reads easier for consumers. | The runtime PIT baseline now supports explicit metadata-driven EF projection, maintenance, and PIT-backed reads for one hub or link plus ordered same-parent non-multi-active satellite snapshots. Refresh orchestration, model-first link-parent PIT artifacts, multi-active PITs, and physical optimization stay deferred. | PIT story `06EZ0NSXY2Y1JZ8SSCX177C770` |
| Bridge tables | Bridge tables can support many-to-many traversal, hierarchy flattening, and downstream relationship query ergonomics. | The v0.5 bridge story supports only explicit metadata declarations, validation against declared hubs and links, and provider-neutral EF projection for bounded many-to-many and hierarchy shapes. Row population, traversal maintenance, deeper hierarchy semantics, provider-specific tuning, and PIT or multi-active interactions stay deferred. | Bridge story `06EZ0NTV4SVAKV98C418T8A3CC` |
| Multi-active satellites | Multi-active satellites can represent multiple simultaneous descriptive records for one parent at the same load window. | Multi-active modeling needs explicit driving-key, uniqueness, ordering, conflict, and example decisions beyond the current parent hash key plus load timestamp satellite shape. | Multi-active story `06EZ0NVN71BN0QWJDCWGVZ2PYG` |
| Advanced hooks | Hooks let advanced users adapt naming, hashing, lineage, timestamps, and provider behavior without destabilizing defaults. | Hook behavior must be scoped by category, validated clearly, and kept additive. It should not force API or configuration depth into the ordinary setup path before concrete implementation work needs it. | Hooks story `06EZ0NWKC9ZME5BSCJFSQEQ02R` |

The API snapshot task `06EZ0NSQFCD3W4CDCJ44GFSKA0` should use this decision as the architecture guardrail for future public-surface checks. It should not infer multi-active or hook API names from this record. The PIT baseline described here is intentionally limited to `DataVaultPitMetadata`, `DataVaultPitSatelliteReferenceMetadata`, and `DataVaultMetadataModel.Pits`. The bridge baseline described here is intentionally limited to `DataVaultBridgeMetadata`, `DataVaultBridgeKind`, `DataVaultMetadataModel.Bridges`, `DataVaultTableKind.Bridge`, `DataVaultPropertyRole.BridgeDepth`, `DataVaultLogicalPropertyKind.BridgeDepth`, and provider-neutral `ApplyDataVaultMetadata()` projection for many-to-many and hierarchy declarations.

## Current Support Versus Expansion Points

Supported or assumed now:

- The architecture documents hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources as the MVP concept set.
- EF metadata translation projects hubs, links, and satellites into shared-type EF metadata.
- Default naming, hashing, record source, timestamp, and provider behavior are deterministic defaults.
- `AddDVault()` stays optionless and ordinary setup remains zero-configuration.
- `UseDataVault()` defaults to the SQLite capability profile, while provider packages may supply or select other profiles.
- `ApplyDataVaultMetadata()` remains the convention-first projection path for the current metadata model.
- `DataVaultMetadataModel.Pits` carries explicit opt-in PIT declarations through `DataVaultPitMetadata` and ordered `DataVaultPitSatelliteReferenceMetadata` items.
- `ApplyDataVaultMetadata()` projects the supported PIT baseline as provider-aware EF shared-type metadata when a PIT declaration resolves to one declared hub and one or more unique, non-multi-active satellites attached to that hub.
- `DataVaultMetadataModel.Bridges` carries explicit opt-in bridge declarations through `DataVaultBridgeMetadata` and `DataVaultBridgeKind.ManyToMany` or `DataVaultBridgeKind.Hierarchy`.
- `ApplyDataVaultMetadata()` projects the supported bridge baseline as provider-aware EF shared-type metadata when a bridge declaration resolves to the declared hubs and source link. Many-to-many bridges project ordered endpoint hash-key columns; hierarchy bridges project ancestor and descendant endpoint hash-key columns plus integer `TraversalDepth`.
- The explicit save service remains the caller-visible persistence boundary and keeps provider-specific save strategy dispatch separate from the core caller contract.
- SQLite local execution remains the required example and validation baseline.

Expansion points for later tickets:

- PIT refresh orchestration, supported temporal grain beyond persisted maintained rows, model-first link-parent PIT artifacts, provider-specific link-parent PIT read optimization, and multi-active PIT semantics.
- Advanced bridge traversal semantics, bridge row population, maintenance strategy, hierarchy edge cases, PIT interactions, multi-active interactions, and provider-specific physical behavior.
- Multi-active satellite driving keys, uniqueness rules, conflict behavior, and examples.
- Advanced hook implementation depth for naming, hashing, record source, timestamp, and provider behavior.
- Public API stability, experimental markings, and snapshot expectations for any new hook or deferred-capability surface.
- Provider-specific physical behavior only where separate provider tickets own it.

Unsupported advanced shapes for the current baseline include automatic PIT population or refresh, model-first link-parent PIT declarations/import-export/diagnostics, multi-active PIT snapshot semantics, bridge row population or maintenance, bridge effectivity-window columns, bridge path payload columns, bridge closure-maintenance state, generated EF bridge relationships or navigations, required custom hook configuration, provider-specific hook matrices, and final public APIs for deferred features outside the bounded PIT and bridge metadata baselines.

## PIT Metadata Baseline

The supported PIT baseline is explicit and metadata-only. A model declares one hub plus ordered satellite snapshot references through `DataVaultPitMetadata`, stores those declarations on `DataVaultMetadataModel.Pits`, and lets `ApplyDataVaultMetadata()` translate them into EF shared-type table metadata.

```csharp
var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
var profile = new DataVaultSatelliteMetadata(
    "Profile",
    customer.ToReference(),
    ["Email Address"]);
var status = new DataVaultSatelliteMetadata(
    "Status",
    customer.ToReference(),
    ["Status Code"]);

var metadataModel = new DataVaultMetadataModel(
    [customer],
    [],
    [profile, status],
    [new DataVaultPitMetadata(customer.ToReference(), ["Profile", "Status"])]);

modelBuilder.ApplyDataVaultMetadata(metadataModel);
```

The default translated table for that declaration is `PitCustomerProfileStatus`. Its canonical column order is `[CustomerHashKey, LoadTimestamp, ProfileLoadTimestamp, StatusLoadTimestamp]`, using the satellite declaration order for snapshot references. The PIT primary key is `[CustomerHashKey, LoadTimestamp]`; the baseline creates no EF foreign-key relationships, navigations, or secondary indexes.

PIT snapshot columns are provider-neutral satellite snapshot reference properties and use the existing provider capability profile pipeline through `DataVaultLogicalPropertyKind.SatelliteSnapshotReference`. Unsupported declarations fail deterministically before a PIT entity is left in the EF model. Unsupported cases include missing hubs or links, empty satellite sets, duplicate satellite references, missing satellites, satellites attached to a different hub or link, mixed hub/link parent shapes, and multi-active satellite references. This runtime baseline uses `DataVaultPitMetadata`; `dvault.model.v1` PIT declarations remain hub-parent-only.

PIT row population is explicit caller work through `IDataVaultPitMaintenanceService`; the baseline does not define automatic refresh, scheduling, provider-specific indexes, migrations, physical tuning, or provider-specific SQL maintenance behavior.

The repository still contains the older public `DataVaultPointInTimeMetadata` and `DataVaultModelBuilder.PointInTime(...)` modeling surface. That surface is separate from this PIT metadata translation baseline, remains outside this ticket's scope, and is not reconciled, renamed, or deprecated here. Examples for this baseline should use `LoadTimestamp` plus `<Satellite>LoadTimestamp`; `PitLoadTimestamp` belongs to the older point-in-time modeling surface and is not the canonical naming example for `DataVaultPitMetadata`.

## Bridge Documentation Baseline

Bridge tables remain an opt-in v0.5 capability layered on the current hub, link, and satellite baseline. They are not part of ordinary DVault setup, they are not required by `AddDVault()`, `UseDataVault()`, or `IDataVaultSaveService`, and they do not change the current explicit save-service boundary. `ApplyDataVaultMetadata()` does support the bounded bridge metadata projection baseline when callers supply explicit bridge declarations.

The source-backed baseline exposes bridge metadata in `DCoding.Data.DVault.Modeling`. A bridge declaration uses `DataVaultBridgeMetadata` with `DataVaultBridgeKind.ManyToMany` or `DataVaultBridgeKind.Hierarchy`, references one source link, binds the endpoint hubs through ordered participant selectors, and is carried on `DataVaultMetadataModel.Bridges`. Metadata validation requires referenced hubs and links to exist, rejects endpoint selectors that do not match the source link, rejects ambiguous hierarchy role binding, and rejects hierarchy self-cycles where ancestor and descendant use the same participant ordinal.

`DataVaultEfMetadataTranslator` translates supported bridge declarations into provider-aware EF shared-type metadata. The produced entity carries `DataVaultTableKind.Bridge`, `MetadataName`, `ProducedName`, provider capability annotations, participant-reference endpoint hash-key properties, and no EF foreign keys or navigations. A many-to-many bridge projects only the ordered endpoint hash-key columns. A hierarchy bridge projects ordered ancestor and descendant hash-key columns plus `TraversalDepth`, where `TraversalDepth` uses `DataVaultPropertyRole.BridgeDepth`, `DataVaultLogicalPropertyKind.BridgeDepth`, CLR `int`, and the active provider capability profile's integer storage mapping.

Source-backed many-to-many metadata example:

```csharp
var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
var order = new DataVaultHubMetadata("Order", ["Order Id"]);
var customerOrder = new DataVaultLinkMetadata(
    "CustomerOrder",
    [customer.ToReference(), order.ToReference()]);
var customerOrderBridge = DataVaultBridgeMetadata.ManyToMany(
    "CustomerOrder",
    customer.ToReference(),
    customerOrder.ToReference(),
    order.ToReference(),
    sourceParticipantOrdinal: 0,
    targetParticipantOrdinal: 1);

modelBuilder.ApplyDataVaultMetadata(
    new DataVaultMetadataModel(
        [customer, order],
        [customerOrder],
        [],
        [customerOrderBridge]));
```

The default EF projection for that bridge is `BridgeCustomerOrder` with ordered columns `[CustomerHashKey, OrderHashKey]`, primary key `PkBridgeCustomerOrderCustomerHashKeyOrderHashKey`, and traversal index `IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey`. The endpoint properties use participant-reference role annotations, retain produced names equal to the column names, and retain metadata names `Customer` and `Order`.

For the source-backed hierarchy example, `DataVaultBridgeMetadata.Hierarchy("SalesRegionHierarchy", ...)` over a recursive `SalesRegionParentChild` link projects `BridgeSalesRegionHierarchy` with ordered columns `[AncestorSalesRegionHashKey, DescendantSalesRegionHashKey, TraversalDepth]`, primary key `PkBridgeSalesRegionHierarchyAncestorSalesRegionHashKeyDescendantSalesRegionHashKey`, and traversal indexes over `[AncestorSalesRegionHashKey, TraversalDepth]` and `[DescendantSalesRegionHashKey, AncestorSalesRegionHashKey]`. This is schema metadata only; it does not compute transitive closure, flatten hierarchy rows, populate traversal depth values, or enforce provider-specific recursive-query behavior.

Bridge behavior beyond that baseline remains deferred. Unsupported bridge projection features include effectivity windows, path payload columns, closure maintenance state, generated EF relationship graph metadata, provider-specific DDL or SQL, migrations, indexes beyond the provider-neutral metadata baseline, bridge row population, traversal refresh, and PIT or multi-active satellite interactions unless later tickets define those contracts explicitly.

## Migration Guardrail Pre-Apply Example

Consumers can run migration guardrails before applying a generated migration by pairing EF Core `MigrationOperation` input with the same Data Vault diagnostics explain baseline used elsewhere. The check is metadata-only and does not require a live database connection or SQL parsing.

```csharp
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;

using var serviceProvider = new ServiceCollection()
    .AddDVault()
    .BuildServiceProvider();

var diagnostics = serviceProvider.GetRequiredService<IDataVaultDiagnosticsService>();
DataVaultDiagnosticsResult baseline = diagnostics.Analyze(metadataModel);
IReadOnlyList<MigrationOperation> generatedOperations = migration.UpOperations;

var report = DataVaultMigrationOperationDiagnostics.AnalyzeReport(baseline, generatedOperations);
Console.WriteLine(report.ToDisplayString());

if (report.HasFindings) {
  Environment.ExitCode = 1;
}
```

`metadataModel` can also be replaced with an existing `DataVaultMetadataRegistry`, a code-first declaration callback, or a configured `DbContext` passed to the matching `AnalyzeReport` overload. DbContext analysis reads the configured model metadata and does not apply the migration. The report keeps stable DVM codes, paths, severities, messages, and remediation text for local scripts, CI/build steps, or tests.

## Hook Stance

Advanced hooks are additive overrides. They must preserve the ordinary default path:

- Naming hooks may adapt model, technical column, logical object, index, or provider physical names, but they must not remove required logical mappings.
- Hashing hooks may support compatibility, version migration, or deterministic test behavior, but they must not introduce hidden non-deterministic inputs.
- Record source hooks may derive or normalize lineage values, but they must not hide missing or ambiguous source lineage.
- Timestamp hooks may support replay or controlled clock behavior, but logical timestamps must preserve UTC instant semantics unless a later contract explicitly changes that boundary.
- Provider behavior hooks may adapt physical provider behavior, but they must not redefine naming, hashing, record source, or timestamp semantics unless those hooks are separately configured.

Unset hooks inherit the documented defaults. Users should be able to configure one category without restating unrelated defaults.

## Provider Boundary

Provider-specific save strategies and provider-name capability-profile registration remain separate architecture concerns documented by `docs/architecture/dvault-v1-explicit-save-service.md`. This record references that boundary but does not broaden it.

Future PIT, bridge, multi-active, or hook tickets may identify provider implications, but they must make provider-specific commitments explicitly in their own scope. The PIT and bridge metadata baselines use existing provider capability mappings for logical EF properties, including bridge hierarchy depth, but this record does not promise provider-specific DDL beyond generated EF metadata, indexing, migrations, native SQL, optimization depth, or provider option matrices for deferred capabilities.

## Downstream Ownership

The existing ticket tree is the intended decomposition. No new split is required by this decision record.

- PIT scope and behavior: `06EZ0NSXY2Y1JZ8SSCX177C770`
- Bridge scope and behavior: `06EZ0NTV4SVAKV98C418T8A3CC`
- Multi-active satellite scope and behavior: `06EZ0NVN71BN0QWJDCWGVZ2PYG`
- Optional advanced hooks: `06EZ0NWKC9ZME5BSCJFSQEQ02R`
- API snapshot guardrails after this decision: `06EZ0NSQFCD3W4CDCJ44GFSKA0`

Those owners can proceed without reopening whether PIT, bridge, multi-active, and hooks are part of the preserved default baseline. They are deferred, opt-in expansion work around the baseline.

## Guardrails

- Do not treat opt-in PIT metadata projection, bridge metadata projection, multi-active satellites, or advanced hooks as requirements for ordinary DVault setup.
- Do not change `AddDVault()`, `UseDataVault()`, or the explicit save-service caller contract merely to satisfy this decision record; keep `ApplyDataVaultMetadata()` limited to explicit metadata projection.
- Do not infer bridge behavior outside the source-backed metadata projection baseline, or concrete multi-active, hook, configuration file, or provider option API shapes from this record.
- Do not require custom configuration for existing hub, link, and satellite modeling.
- Do not replace SQLite-oriented examples or tests with advanced capability examples.
- Do not move provider-specific optimization scope into a deferred capability ticket unless that ticket explicitly owns the provider decision.

## Cross-Check Against Source Records

- `docs/architecture/mvp-data-vault-concepts.md` remains the concept baseline for hubs, links, satellites, hash keys, hash diffs, load timestamps, record sources, and SQLite-friendly examples.
- `docs/plans/optional-advanced-configuration-hooks.md` remains the detailed hook planning input. This decision record ratifies its default-first, optional, additive hook stance for v0.5 deferred capability work.
- `docs/architecture/dvault-v1-explicit-save-service.md` remains the explicit save-service and provider-specific save-strategy boundary. Deferred capabilities must extend around that boundary rather than silently replacing it.
- Current source evidence keeps `AddDVault()` optionless, routes metadata projection through `UseDataVault()` and `ApplyDataVaultMetadata()`, defaults model metadata to the SQLite capability profile, projects hub, link, satellite, PIT, and bounded bridge EF shapes, and adds PIT or bridge EF projection only when `DataVaultMetadataModel.Pits` or `DataVaultMetadataModel.Bridges` contains explicit declarations.
