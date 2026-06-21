# DVault Allocation Hotspot Report

## Context

- Ticket: `06FE4R1XJVQZTQ8S9WN2YE3ZKW`
- Evidence posture: measured DVault-owned allocation hotspots; database write boundary samples are excluded from the ranked table.
- Required provider: SQLite local temporary files
- Provider filter: sqlite
- Iterations: 3
- Warmup iterations: 1
- Load timestamp storage: ProviderDefault
- Hash key variants: sha256-v1-hex
- Stable hash baseline: `sha256-v1` with `HexString` hash-key storage.
- Runtime: .NET 10.0.9 10.0.9 on Debian GNU/Linux 13 (trixie).

## Workload Shapes

| Workload | Dataset size | Change ratio | Persisted outcome |
| --- | --- | --- | --- |
| stable-hash-canonicalization | 1000 structured customer profile field sets | sha256-v1 HexString canonical text only | 1000 stable-hash field sets normalized; total canonical characters=125332 |
| stable-hash-digest-generation | 1000 pre-normalized customer profile payloads | sha256-v1 HexString digest generation only | 1000 normalized payloads hashed with sha256-v1; total digest characters=64000 |
| customer-profile-hub-only-save-prep | 100 customer hub save operations | hub-only customer-profile save shape | 100 customer hub rows persisted from hub-only save shape |
| order-product-link-bearing-save-prep | 100 order/product hub pairs and order-product links | link-bearing order-product save shape | 100 order-product link rows persisted from link-bearing save shape |
| satellite-unchanged-replay-filter | 100 customers, 20 existing profile states each | unchanged satellite replay across 4 retained-state chunks | 2000 profile satellite rows after unchanged replay latest lookup |
| satellite-changed-replay-filter | 100 customers, 20 existing profile states each | changed satellite replay across 4 retained-state chunks | 2100 profile satellite rows after changed replay latest lookup |

## Ranked Hotspots

| Rank | Surface | Step | Workload | Mean allocated bytes | Mean ms | Mean calls | Recommendation |
| ---: | --- | --- | --- | ---: | ---: | ---: | --- |
| 1 | pre-write save preparation | DefaultDataVaultSaveService.AddSatellitesAsync | satellite-changed-replay-filter | 4456800 | 18.362 | 4.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 2 | pre-write save preparation | DefaultDataVaultSaveService.AddSatellitesAsync | satellite-unchanged-replay-filter | 4099432 | 12.656 | 4.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 3 | satellite latest-hash-diff replay filtering | DefaultDataVaultSaveService.FilterSatellitePlansAsync | satellite-changed-replay-filter | 3616672 | 16.041 | 4.0 | Target latest-state lookup materialization, replay-dedup dictionaries, and retained chunk state; keep caller-supplied HashDiff generation out of scope. |
| 4 | satellite latest-hash-diff replay filtering | DefaultDataVaultSaveService.LoadLatestSatelliteHashDiffsAsync | satellite-changed-replay-filter | 3547104 | 15.396 | 4.0 | Target latest-state lookup materialization, replay-dedup dictionaries, and retained chunk state; keep caller-supplied HashDiff generation out of scope. |
| 5 | satellite latest-hash-diff replay filtering | DefaultDataVaultSaveService.FilterSatellitePlansAsync | satellite-unchanged-replay-filter | 3485400 | 11.765 | 4.0 | Target latest-state lookup materialization, replay-dedup dictionaries, and retained chunk state; keep caller-supplied HashDiff generation out of scope. |
| 6 | satellite latest-hash-diff replay filtering | DefaultDataVaultSaveService.LoadLatestSatelliteHashDiffsAsync | satellite-unchanged-replay-filter | 3471360 | 11.648 | 4.0 | Target latest-state lookup materialization, replay-dedup dictionaries, and retained chunk state; keep caller-supplied HashDiff generation out of scope. |
| 7 | pre-write save preparation | DefaultDataVaultSaveService.CreateUniqueRowSavePlans | order-product-link-bearing-save-prep | 1728376 | 2.913 | 1.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 8 | stable-hash canonicalization | DefaultStableHashNormalizer.NormalizeFields | stable-hash-canonicalization | 1589328 | 1.325 | 1000.0 | Target normalized-field collection, sorting, and canonical string materialization before changing hash contracts. |
| 9 | pre-write save preparation | DefaultDataVaultSaveService.AddUniqueRowsAsync | order-product-link-bearing-save-prep | 1462173 | 5.526 | 1.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 10 | pre-write save preparation | DefaultDataVaultSaveService.CreateSatelliteSavePlans | satellite-changed-replay-filter | 610304 | 0.984 | 4.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 11 | pre-write save preparation | DefaultDataVaultSaveService.CreateSatelliteSavePlans | satellite-unchanged-replay-filter | 610304 | 0.837 | 4.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 12 | digest generation | BuiltInStableHashService.ComputeHash | stable-hash-digest-generation | 584456 | 1.418 | 1001.0 | Target UTF-8 byte materialization, digest byte arrays, and lowercase hex materialization while preserving sha256-v1 output. |
| 13 | pre-write save preparation | DefaultDataVaultSaveService.CreateUniqueRowSavePlans | customer-profile-hub-only-save-prep | 536216 | 0.993 | 1.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 14 | pre-write save preparation | DefaultDataVaultSaveService.AddUniqueRowsAsync | customer-profile-hub-only-save-prep | 451485 | 2.490 | 1.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 15 | stable-hash canonicalization | DefaultStableHashNormalizer.NormalizeFields | order-product-link-bearing-save-prep | 246400 | 0.191 | 300.0 | Target normalized-field collection, sorting, and canonical string materialization before changing hash contracts. |
| 16 | pre-write save preparation | DefaultDataVaultSaveService.StageSatelliteRows | satellite-changed-replay-filter | 226672 | 1.278 | 4.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 17 | digest generation | BuiltInStableHashService.ComputeHash | order-product-link-bearing-save-prep | 156800 | 0.412 | 300.0 | Target UTF-8 byte materialization, digest byte arrays, and lowercase hex materialization while preserving sha256-v1 output. |
| 18 | stable-hash canonicalization | DefaultStableHashNormalizer.NormalizeFields | customer-profile-hub-only-save-prep | 53600 | 0.058 | 100.0 | Target normalized-field collection, sorting, and canonical string materialization before changing hash contracts. |
| 19 | digest generation | BuiltInStableHashService.ComputeHash | customer-profile-hub-only-save-prep | 48000 | 0.156 | 100.0 | Target UTF-8 byte materialization, digest byte arrays, and lowercase hex materialization while preserving sha256-v1 output. |
| 20 | satellite latest-hash-diff replay filtering | ChunkedSaveContinuityState.TrackLatestSatelliteHashDiff | satellite-changed-replay-filter | 19128 | 0.071 | 100.0 | Target latest-state lookup materialization, replay-dedup dictionaries, and retained chunk state; keep caller-supplied HashDiff generation out of scope. |
| 21 | satellite latest-hash-diff replay filtering | ChunkedSaveContinuityState.ApplyRetainedState | satellite-changed-replay-filter | 13384 | 0.045 | 4.0 | Target latest-state lookup materialization, replay-dedup dictionaries, and retained chunk state; keep caller-supplied HashDiff generation out of scope. |
| 22 | pre-write save preparation | DataVaultTelemetryStrategySelector.SelectSaveStrategy | satellite-changed-replay-filter | 5280 | 0.084 | 4.0 | Keep as measured context unless it remains above the save-plan and stable-hash rows in a follow-up run. |
| 23 | pre-write save preparation | DataVaultTelemetryStrategySelector.SelectSaveStrategy | satellite-unchanged-replay-filter | 5280 | 0.076 | 4.0 | Keep as measured context unless it remains above the save-plan and stable-hash rows in a follow-up run. |
| 24 | pre-write save preparation | DefaultDataVaultSaveService.AddSatellitesAsync | customer-profile-hub-only-save-prep | 1416 | 0.026 | 1.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 25 | pre-write save preparation | DefaultDataVaultSaveService.AddSatellitesAsync | order-product-link-bearing-save-prep | 1416 | 0.022 | 1.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 26 | pre-write save preparation | DataVaultTelemetryStrategySelector.SelectSaveStrategy | customer-profile-hub-only-save-prep | 1320 | 0.016 | 1.0 | Keep as measured context unless it remains above the save-plan and stable-hash rows in a follow-up run. |
| 27 | pre-write save preparation | DataVaultTelemetryStrategySelector.SelectSaveStrategy | order-product-link-bearing-save-prep | 1320 | 0.011 | 1.0 | Keep as measured context unless it remains above the save-plan and stable-hash rows in a follow-up run. |
| 28 | pre-write save preparation | DefaultDataVaultSaveService.StageSatelliteRows | satellite-unchanged-replay-filter | 960 | 0.009 | 4.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 29 | pre-write save preparation | DefaultDataVaultSaveService.CreateUniqueRowSavePlans | satellite-changed-replay-filter | 736 | 0.031 | 4.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 30 | pre-write save preparation | DefaultDataVaultSaveService.CreateUniqueRowSavePlans | satellite-unchanged-replay-filter | 736 | 0.027 | 4.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 31 | pre-write save preparation | DefaultDataVaultSaveService.ResolveRequests | satellite-changed-replay-filter | 736 | 0.010 | 4.0 | Keep as measured context unless it remains above the save-plan and stable-hash rows in a follow-up run. |
| 32 | pre-write save preparation | DefaultDataVaultSaveService.ResolveRequests | satellite-unchanged-replay-filter | 736 | 0.010 | 4.0 | Keep as measured context unless it remains above the save-plan and stable-hash rows in a follow-up run. |
| 33 | pre-write save preparation | DefaultDataVaultSaveService.AddUniqueRowsAsync | satellite-changed-replay-filter | 544 | 0.017 | 4.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 34 | pre-write save preparation | DefaultDataVaultSaveService.AddUniqueRowsAsync | satellite-unchanged-replay-filter | 544 | 0.012 | 4.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 35 | digest generation | BuiltInStableHashService.ComputeHash | stable-hash-canonicalization | 456 | 0.018 | 1.0 | Target UTF-8 byte materialization, digest byte arrays, and lowercase hex materialization while preserving sha256-v1 output. |
| 36 | pre-write save preparation | DefaultDataVaultSaveService.CreateSatelliteSavePlans | customer-profile-hub-only-save-prep | 304 | 0.009 | 1.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 37 | pre-write save preparation | DefaultDataVaultSaveService.CreateSatelliteSavePlans | order-product-link-bearing-save-prep | 304 | 0.006 | 1.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 38 | pre-write save preparation | DefaultDataVaultSaveService.StageSatelliteRows | customer-profile-hub-only-save-prep | 240 | 0.002 | 1.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 39 | pre-write save preparation | DefaultDataVaultSaveService.StageSatelliteRows | order-product-link-bearing-save-prep | 240 | 0.003 | 1.0 | Target save-plan row dictionaries, grouping, and pre-write dedupe materialization before provider-specific SQL tuning. |
| 40 | satellite latest-hash-diff replay filtering | DefaultDataVaultSaveService.FilterSatellitePlansAsync | customer-profile-hub-only-save-prep | 240 | 0.005 | 1.0 | Target latest-state lookup materialization, replay-dedup dictionaries, and retained chunk state; keep caller-supplied HashDiff generation out of scope. |
| 41 | satellite latest-hash-diff replay filtering | DefaultDataVaultSaveService.FilterSatellitePlansAsync | order-product-link-bearing-save-prep | 240 | 0.004 | 1.0 | Target latest-state lookup materialization, replay-dedup dictionaries, and retained chunk state; keep caller-supplied HashDiff generation out of scope. |
| 42 | pre-write save preparation | DefaultDataVaultSaveService.ResolveRequests | customer-profile-hub-only-save-prep | 184 | 0.013 | 1.0 | Keep as measured context unless it remains above the save-plan and stable-hash rows in a follow-up run. |
| 43 | pre-write save preparation | DefaultDataVaultSaveService.ResolveRequests | order-product-link-bearing-save-prep | 184 | 0.003 | 1.0 | Keep as measured context unless it remains above the save-plan and stable-hash rows in a follow-up run. |
| 44 | satellite latest-hash-diff replay filtering | ChunkedSaveContinuityState.ApplyRetainedState | satellite-unchanged-replay-filter | 160 | 0.003 | 4.0 | Target latest-state lookup materialization, replay-dedup dictionaries, and retained chunk state; keep caller-supplied HashDiff generation out of scope. |

## Recommended Optimization Order

- pre-write save preparation: start with DefaultDataVaultSaveService.AddSatellitesAsync in satellite-changed-replay-filter (4456800 mean allocated bytes).
- satellite latest-hash-diff replay filtering: start with DefaultDataVaultSaveService.FilterSatellitePlansAsync in satellite-changed-replay-filter (3616672 mean allocated bytes).
- stable-hash canonicalization: start with DefaultStableHashNormalizer.NormalizeFields in stable-hash-canonicalization (1589328 mean allocated bytes).
- digest generation: start with BuiltInStableHashService.ComputeHash in stable-hash-digest-generation (584456 mean allocated bytes).

## Measurement Boundary

- Caller-owned satellite `HashDiff` generation is outside the measured operation; replay requests are created before the profiled save call.
- SQLite database setup, seeding, verification, and cleanup run outside the profiled save action.
- `DbContext.SaveChangesAsync` is sampled as context but excluded from the ranked DVault-owned hotspot table.
