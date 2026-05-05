# Deferred Data Vault Capability Decision Record

Status: v0.5 architecture decision
Ticket: 06EZ0NSHJVC9SD2KS6PWWNHPJM
Decision date: 2026-05-05

## Purpose

This record publishes the v0.5 architecture stance for deferred Data Vault capability families. It consolidates the earlier deferred-capabilities note and the optional advanced-configuration hook plan into one governing reference for PIT tables, bridge tables, multi-active satellites, and the hooks those features will need.

The record is intentionally architecture-level. It does not implement runtime behavior, finalize public API names, define provider-specific optimization posture, or replace the current MVP hub, link, satellite, and SQLite-oriented baseline.

## Decision

DVault v0.5 keeps the default path small and convention-first. The current baseline remains:

- Hub, link, and satellite concepts as the first Data Vault persistence model.
- Deterministic default conventions for technical names, metadata, stable hashing, load timestamps, and record sources.
- Optionless `AddDVault()` service registration.
- Convention-first `UseDataVault()` and `ApplyDataVaultMetadata()` model configuration, with the default capability profile remaining SQLite-oriented unless a provider profile is selected.
- The explicit `IDataVaultSaveService` write boundary, where callers supply load timestamp, record source, and vault row intent instead of relying on hidden `SaveChanges` interception.
- SQLite-backed examples, tests, and benchmark expectations as the required local baseline.

PIT table generation, bridge table generation, and multi-active satellites are v0.5 deferred capability families. They are valuable expansion work, but they are opt-in and must not become prerequisites for ordinary hub, link, and satellite setup.

Advanced hooks are also opt-in. Naming, hashing, record source, timestamp, and provider behavior may become configurable extension categories, but unset hooks inherit the default behavior. Future hook implementations can wrap or replace only their own category and must not make normal DVault setup require configuration.

## Why These Capabilities Are Deferred

The MVP baseline explains how DVault represents business identity, relationships, and descriptive history through hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources. That model is enough for the first portable SQLite-oriented package and for current EF metadata projection.

The deferred capability families need additional decisions that are not required to preserve that baseline:

| Capability family | Planning value | Why it stays opt-in for v0.5 | Downstream owner |
| --- | --- | --- | --- |
| PIT tables | Point-in-time tables can simplify historical joins across multiple satellites and make time-sliced reads easier for consumers. | PIT behavior depends on refresh strategy, query patterns, temporal grain, persisted versus computed shape, and how late-arriving data is reconciled. Those decisions should not change the current satellite baseline. | PIT story `06EZ0NSXY2Y1JZ8SSCX177C770` |
| Bridge tables | Bridge tables can support many-to-many traversal, hierarchy flattening, and downstream relationship query ergonomics. | Bridge generation depends on relationship semantics, hierarchy depth, business rules, consuming workload expectations, and maintenance strategy. Those assumptions are outside ordinary link projection. | Bridge story `06EZ0NTV4SVAKV98C418T8A3CC` |
| Multi-active satellites | Multi-active satellites can represent multiple simultaneous descriptive records for one parent at the same load window. | Multi-active modeling needs explicit driving-key, uniqueness, ordering, conflict, and example decisions beyond the current parent hash key plus load timestamp satellite shape. | Multi-active story `06EZ0NVN71BN0QWJDCWGVZ2PYG` |
| Advanced hooks | Hooks let advanced users adapt naming, hashing, lineage, timestamps, and provider behavior without destabilizing defaults. | Hook behavior must be scoped by category, validated clearly, and kept additive. It should not force API or configuration depth into the ordinary setup path before concrete implementation work needs it. | Hooks story `06EZ0NWKC9ZME5BSCJFSQEQ02R` |

The API snapshot task `06EZ0NSQFCD3W4CDCJ44GFSKA0` should use this decision as the architecture guardrail for future public-surface checks. It should not infer concrete PIT, bridge, multi-active, or hook API names from this record.

## Current Support Versus Expansion Points

Supported or assumed now:

- The architecture documents hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources as the MVP concept set.
- EF metadata translation projects hubs, links, and satellites into shared-type EF metadata.
- Default naming, hashing, record source, timestamp, and provider behavior are deterministic defaults.
- `AddDVault()` stays optionless and ordinary setup remains zero-configuration.
- `UseDataVault()` defaults to the SQLite capability profile, while provider packages may supply or select other profiles.
- `ApplyDataVaultMetadata()` remains the convention-first projection path for the current metadata model.
- The explicit save service remains the caller-visible persistence boundary and keeps provider-specific save strategy dispatch separate from the core caller contract.
- SQLite local execution remains the required example and validation baseline.

Expansion points for later tickets:

- PIT scope, refresh semantics, supported temporal grain, and persisted versus computed read models.
- Bridge relationship and hierarchy scenarios, validation rules, and maintenance strategy.
- Multi-active satellite driving keys, uniqueness rules, conflict behavior, and examples.
- Advanced hook implementation depth for naming, hashing, record source, timestamp, and provider behavior.
- Public API stability, experimental markings, and snapshot expectations for any new hook or deferred-capability surface.
- Provider-specific physical behavior only where separate provider tickets own it.

Unsupported advanced shapes for the current baseline include generated PIT tables, generated bridge tables, multi-active satellite projection or loading behavior, required custom hook configuration, provider-specific hook matrices, and final public APIs for deferred features.

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

Future PIT, bridge, multi-active, or hook tickets may identify provider implications, but they must make provider-specific commitments explicitly in their own scope. This record does not promise provider-specific DDL, indexing, migrations, native SQL, optimization depth, or provider option matrices for deferred capabilities.

## Downstream Ownership

The existing ticket tree is the intended decomposition. No new split is required by this decision record.

- PIT scope and behavior: `06EZ0NSXY2Y1JZ8SSCX177C770`
- Bridge scope and behavior: `06EZ0NTV4SVAKV98C418T8A3CC`
- Multi-active satellite scope and behavior: `06EZ0NVN71BN0QWJDCWGVZ2PYG`
- Optional advanced hooks: `06EZ0NWKC9ZME5BSCJFSQEQ02R`
- API snapshot guardrails after this decision: `06EZ0NSQFCD3W4CDCJ44GFSKA0`

Those owners can proceed without reopening whether PIT, bridge, multi-active, and hooks are part of the preserved default baseline. They are deferred, opt-in expansion work around the baseline.

## Guardrails

- Do not treat PIT table generation, bridge table generation, multi-active satellites, or advanced hooks as requirements for ordinary DVault setup.
- Do not change `AddDVault()`, `UseDataVault()`, `ApplyDataVaultMetadata()`, or the explicit save-service caller contract merely to satisfy this decision record.
- Do not infer concrete class names, method names, parameter names, configuration file shapes, or provider option matrices from this record.
- Do not require custom configuration for existing hub, link, and satellite modeling.
- Do not replace SQLite-oriented examples or tests with advanced capability examples.
- Do not move provider-specific optimization scope into a deferred capability ticket unless that ticket explicitly owns the provider decision.

## Cross-Check Against Source Records

- `docs/architecture/mvp-data-vault-concepts.md` remains the concept baseline for hubs, links, satellites, hash keys, hash diffs, load timestamps, record sources, and SQLite-friendly examples.
- `docs/plans/optional-advanced-configuration-hooks.md` remains the detailed hook planning input. This decision record ratifies its default-first, optional, additive hook stance for v0.5 deferred capability work.
- `docs/architecture/dvault-v1-explicit-save-service.md` remains the explicit save-service and provider-specific save-strategy boundary. Deferred capabilities must extend around that boundary rather than silently replacing it.
- Current source evidence keeps `AddDVault()` optionless, routes metadata projection through `UseDataVault()` and `ApplyDataVaultMetadata()`, defaults model metadata to the SQLite capability profile, and projects only hub, link, and satellite EF shapes.
