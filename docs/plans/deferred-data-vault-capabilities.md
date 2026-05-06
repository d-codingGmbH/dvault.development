# Deferred Data Vault Capability Decision Record

Status: v0.5 architecture decision with PIT metadata baseline
Ticket: 06EZ0NSHJVC9SD2KS6PWWNHPJM
Decision date: 2026-05-05

## Purpose

This record publishes the v0.5 architecture stance for deferred Data Vault capability families. It consolidates the earlier deferred-capabilities note and the optional advanced-configuration hook plan into one governing reference for PIT tables, bridge tables, multi-active satellites, and the hooks those features will need.

The record is intentionally architecture-level. It does not implement runtime behavior, define provider-specific optimization posture, or replace the current MVP hub, link, satellite, and SQLite-oriented baseline. The PIT story now adds one bounded public metadata baseline for opt-in EF table projection while leaving PIT population and refresh behavior deferred.

## Decision

DVault v0.5 keeps the default path small and convention-first. The current baseline remains:

- Hub, link, and satellite concepts as the first Data Vault persistence model.
- Deterministic default conventions for technical names, metadata, stable hashing, load timestamps, and record sources.
- Optionless `AddDVault()` service registration.
- Convention-first `UseDataVault()` and `ApplyDataVaultMetadata()` model configuration, with the default capability profile remaining SQLite-oriented unless a provider profile is selected.
- The explicit `IDataVaultSaveService` write boundary, where callers supply load timestamp, record source, and vault row intent instead of relying on hidden `SaveChanges` interception.
- SQLite-backed examples, tests, and benchmark expectations as the required local baseline.

PIT table projection, bridge table generation, and multi-active satellites are v0.5 deferred capability families. They are valuable expansion work, but they are opt-in and must not become prerequisites for ordinary hub, link, and satellite setup. For PIT, v0.5 supports only the provider-neutral metadata projection baseline documented below; row population, refresh orchestration, provider-specific physical optimization, link-based PITs, and multi-active PIT semantics remain deferred.

Advanced hooks are also opt-in. Naming, hashing, record source, timestamp, and provider behavior may become configurable extension categories, but unset hooks inherit the default behavior. Future hook implementations can wrap or replace only their own category and must not make normal DVault setup require configuration.

## Why These Capabilities Are Deferred

The MVP baseline explains how DVault represents business identity, relationships, and descriptive history through hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources. That model is enough for the first portable SQLite-oriented package and for current EF metadata projection.

The deferred capability families need additional decisions that are not required to preserve that baseline:

| Capability family | Planning value | Why it stays opt-in for v0.5 | Downstream owner |
| --- | --- | --- | --- |
| PIT tables | Point-in-time tables can simplify historical joins across multiple satellites and make time-sliced reads easier for consumers. | The v0.5 PIT story supports only explicit metadata-driven EF projection for one hub plus ordered hub-attached satellite snapshots. Refresh strategy, temporal grain, persisted maintenance, late-arriving reconciliation, link-based PITs, multi-active PITs, and physical optimization stay deferred. | PIT story `06EZ0NSXY2Y1JZ8SSCX177C770` |
| Bridge tables | Bridge tables can support many-to-many traversal, hierarchy flattening, and downstream relationship query ergonomics. | Bridge generation depends on relationship semantics, hierarchy depth, business rules, consuming workload expectations, and maintenance strategy. Those assumptions are outside ordinary link projection. | Bridge story `06EZ0NTV4SVAKV98C418T8A3CC` |
| Multi-active satellites | Multi-active satellites can represent multiple simultaneous descriptive records for one parent at the same load window. | Multi-active modeling needs explicit driving-key, uniqueness, ordering, conflict, and example decisions beyond the current parent hash key plus load timestamp satellite shape. | Multi-active story `06EZ0NVN71BN0QWJDCWGVZ2PYG` |
| Advanced hooks | Hooks let advanced users adapt naming, hashing, lineage, timestamps, and provider behavior without destabilizing defaults. | Hook behavior must be scoped by category, validated clearly, and kept additive. It should not force API or configuration depth into the ordinary setup path before concrete implementation work needs it. | Hooks story `06EZ0NWKC9ZME5BSCJFSQEQ02R` |

The API snapshot task `06EZ0NSQFCD3W4CDCJ44GFSKA0` should use this decision as the architecture guardrail for future public-surface checks. It should not infer concrete bridge, multi-active, or hook API names from this record. The PIT baseline described here is intentionally limited to `DataVaultPitMetadata`, `DataVaultPitSatelliteReferenceMetadata`, and `DataVaultMetadataModel.Pits`.

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
- The explicit save service remains the caller-visible persistence boundary and keeps provider-specific save strategy dispatch separate from the core caller contract.
- SQLite local execution remains the required example and validation baseline.

Expansion points for later tickets:

- PIT refresh semantics, supported temporal grain, row population, persisted versus computed read models, link-based PITs, and multi-active PIT semantics.
- Bridge relationship and hierarchy scenarios, validation rules, and maintenance strategy.
- Multi-active satellite driving keys, uniqueness rules, conflict behavior, and examples.
- Advanced hook implementation depth for naming, hashing, record source, timestamp, and provider behavior.
- Public API stability, experimental markings, and snapshot expectations for any new hook or deferred-capability surface.
- Provider-specific physical behavior only where separate provider tickets own it.

Unsupported advanced shapes for the current baseline include automatic PIT population or refresh, link-based PITs, multi-active PIT snapshot semantics, generated bridge tables, multi-active satellite projection or loading behavior, required custom hook configuration, provider-specific hook matrices, and final public APIs for deferred features outside the bounded PIT metadata baseline.

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

PIT snapshot columns are provider-neutral satellite snapshot reference properties and use the existing provider capability profile pipeline through `DataVaultLogicalPropertyKind.SatelliteSnapshotReference`. Unsupported declarations fail deterministically before a PIT entity is left in the EF model. Unsupported cases include missing hubs, empty satellite sets, duplicate satellite references, missing satellites, satellites attached to another hub, link-based PIT parents, link-attached satellites, and multi-active satellite references.

This baseline does not populate or refresh PIT rows. It also does not define scheduling, recomputation, late-arriving-data reconciliation, provider-specific indexes, migrations, physical tuning, or SQL maintenance behavior.

The repository still contains the older public `DataVaultPointInTimeMetadata` and `DataVaultModelBuilder.PointInTime(...)` modeling surface. That surface is separate from this PIT metadata translation baseline, remains outside this ticket's scope, and is not reconciled, renamed, or deprecated here. Examples for this baseline should use `LoadTimestamp` plus `<Satellite>LoadTimestamp`; `PitLoadTimestamp` belongs to the older point-in-time modeling surface and is not the canonical naming example for `DataVaultPitMetadata`.

## Bridge Documentation Baseline

Bridge tables remain an opt-in v0.5 deferred capability layered on the current hub, link, and satellite baseline. They are not part of ordinary DVault setup, they are not required by `AddDVault()`, `UseDataVault()`, `ApplyDataVaultMetadata()`, or `IDataVaultSaveService`, and they do not change the current explicit save-service boundary.

The visible repository baseline does not expose a bridge runtime surface today. `DataVaultEfMetadataTranslator` creates EF projections for hubs, links, and satellites only. `DataVaultAnnotationNames` exposes provider-neutral annotation names for conventions, produced names, entity kind, metadata name, parent reference, ordinal, property role, technical column role, and provider metadata, but it does not define a bridge-specific annotation contract. This record therefore documents bridge tables as deferred architecture context rather than as implemented EF metadata output, generated table names, save behavior, or a public modeling API.

Conceptual deferred bridge-use-case example: a reporting consumer may need to traverse from Customer to Product when the current Data Vault model stores Customer, Order, and Product as hubs and stores the relationships through ordinary links such as CustomerOrder and OrderProduct. A future opt-in bridge capability could support that many-to-many Customer-to-Product traversal as a relationship-query convenience around the existing hub and link baseline. This is not a source-backed API walkthrough, does not prescribe a bridge table name or shape, and does not imply current runtime support beyond the existing hub, link, satellite, metadata projection, and explicit save-service vocabulary.

Bridge hierarchy-specific behavior, including hierarchy flattening depth and recursive traversal behavior, is unsupported in the current baseline. Provider-specific bridge DDL, indexes, migrations, native SQL, and maintenance strategies are deferred to later provider-scoped tickets. PIT interactions and multi-active satellite interactions are also deferred unless later tickets define their contracts explicitly.

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

Future PIT, bridge, multi-active, or hook tickets may identify provider implications, but they must make provider-specific commitments explicitly in their own scope. The PIT metadata baseline uses existing provider capability mappings for logical EF properties, but this record does not promise provider-specific DDL beyond generated EF metadata, indexing, migrations, native SQL, optimization depth, or provider option matrices for deferred capabilities.

## Downstream Ownership

The existing ticket tree is the intended decomposition. No new split is required by this decision record.

- PIT scope and behavior: `06EZ0NSXY2Y1JZ8SSCX177C770`
- Bridge scope and behavior: `06EZ0NTV4SVAKV98C418T8A3CC`
- Multi-active satellite scope and behavior: `06EZ0NVN71BN0QWJDCWGVZ2PYG`
- Optional advanced hooks: `06EZ0NWKC9ZME5BSCJFSQEQ02R`
- API snapshot guardrails after this decision: `06EZ0NSQFCD3W4CDCJ44GFSKA0`

Those owners can proceed without reopening whether PIT, bridge, multi-active, and hooks are part of the preserved default baseline. They are deferred, opt-in expansion work around the baseline.

## Guardrails

- Do not treat opt-in PIT metadata projection, bridge table generation, multi-active satellites, or advanced hooks as requirements for ordinary DVault setup.
- Do not change `AddDVault()`, `UseDataVault()`, or the explicit save-service caller contract merely to satisfy this decision record; keep `ApplyDataVaultMetadata()` limited to explicit metadata projection.
- Do not infer concrete bridge, multi-active, hook, configuration file, or provider option API shapes from this record.
- Do not require custom configuration for existing hub, link, and satellite modeling.
- Do not replace SQLite-oriented examples or tests with advanced capability examples.
- Do not move provider-specific optimization scope into a deferred capability ticket unless that ticket explicitly owns the provider decision.

## Cross-Check Against Source Records

- `docs/architecture/mvp-data-vault-concepts.md` remains the concept baseline for hubs, links, satellites, hash keys, hash diffs, load timestamps, record sources, and SQLite-friendly examples.
- `docs/plans/optional-advanced-configuration-hooks.md` remains the detailed hook planning input. This decision record ratifies its default-first, optional, additive hook stance for v0.5 deferred capability work.
- `docs/architecture/dvault-v1-explicit-save-service.md` remains the explicit save-service and provider-specific save-strategy boundary. Deferred capabilities must extend around that boundary rather than silently replacing it.
- Current source evidence keeps `AddDVault()` optionless, routes metadata projection through `UseDataVault()` and `ApplyDataVaultMetadata()`, defaults model metadata to the SQLite capability profile, projects hub, link, and satellite EF shapes, and adds PIT EF projection only when `DataVaultMetadataModel.Pits` contains explicit declarations.
