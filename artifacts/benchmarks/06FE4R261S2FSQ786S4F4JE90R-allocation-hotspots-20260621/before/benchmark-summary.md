# DVault Benchmark Summary

## Summary

- Benchmark baselines: 6
- Required provider: SQLite local temporary files
- Optional PostgreSQL provider: PostgreSQL external provider
- PostgreSQL execution status: skipped
- PostgreSQL skip reason: not configured: DVAULT_TEST_POSTGRES_CONNECTION_STRING is not set or empty.

## Run Context

- Iterations: 3
- Warmup iterations: 1
- Load timestamp storage: ProviderDefault
- Provider filter: sqlite
- Hash key variants: sha256-v1-hex
- OS description: Debian GNU/Linux 13 (trixie)
- OS architecture: X64
- Process architecture: X64
- Processor count: 32
- .NET runtime description: .NET 10.0.9
- .NET runtime version: 10.0.9

## Results

| Scenario | Provider | Baseline | Strategy family | Dataset size | Change ratio | Execution status | Skip reason | Iterations | Mean ms | Min ms | Max ms | Mean allocated bytes | Min allocated bytes | Max allocated bytes | Execution detail | Persisted outcome |
| --- | --- | --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- | --- |
| stable-hash-canonicalization | SQLite local temporary files | dvault-allocation-hotspots/stable-hash-canonicalization | allocation-hotspot-profile | 1000 structured customer profile field sets | sha256-v1 HexString canonical text only | completed |  | 3 | 3.559 | 3.184 | 4.297 | 1773205 | 1773168 | 1773280 | scenario=stable-hash-canonicalization; provider=SQLite local temporary files; baseline=sha256-v1-hex; surface=DefaultStableHashNormalizer | 1000 stable-hash field sets normalized; total canonical characters=125332 |
| stable-hash-digest-generation | SQLite local temporary files | dvault-allocation-hotspots/stable-hash-digest-generation | allocation-hotspot-profile | 1000 pre-normalized customer profile payloads | sha256-v1 HexString digest generation only | completed |  | 3 | 3.428 | 3.367 | 3.497 | 775904 | 775904 | 775904 | scenario=stable-hash-digest-generation; provider=SQLite local temporary files; baseline=sha256-v1-hex; surface=BuiltInStableHashService | 1000 normalized payloads hashed with sha256-v1; total digest characters=64000 |
| customer-profile-hub-only-save-prep | SQLite local temporary files | dvault-allocation-hotspots/customer-profile-hub-only | allocation-hotspot-profile | 100 customer hub save operations | hub-only customer-profile save shape | completed |  | 3 | 6.913 | 5.238 | 9.832 | 1872101 | 1872048 | 1872208 | scenario=customer-profile-hub-only-save-prep; provider=SQLite local temporary files; baseline=provider-neutral-dvault-fallback; hashKeyVariant=sha256-v1-hex; storageProfile=HexString | 100 customer hub rows persisted from hub-only save shape |
| order-product-link-bearing-save-prep | SQLite local temporary files | dvault-allocation-hotspots/order-product-link-bearing | allocation-hotspot-profile | 100 order/product hub pairs and order-product links | link-bearing order-product save shape | completed |  | 3 | 17.853 | 16.302 | 20.890 | 5843261 | 5843208 | 5843368 | scenario=order-product-link-bearing-save-prep; provider=SQLite local temporary files; baseline=provider-neutral-dvault-fallback; hashKeyVariant=sha256-v1-hex; storageProfile=HexString | 100 order-product link rows persisted from link-bearing save shape |
| satellite-unchanged-replay-filter | SQLite local temporary files | dvault-allocation-hotspots/satellite-unchanged-replay | allocation-hotspot-profile | 100 customers, 20 existing profile states each | unchanged satellite replay across 4 retained-state chunks | completed |  | 3 | 13.658 | 12.975 | 14.193 | 4165488 | 4165488 | 4165488 | scenario=satellite-unchanged-replay-filter; provider=SQLite local temporary files; baseline=provider-neutral-dvault-fallback; hashKeyVariant=sha256-v1-hex; storageProfile=HexString; callerHashDiffGeneration=outside-profile | 2000 profile satellite rows after unchanged replay latest lookup |
| satellite-changed-replay-filter | SQLite local temporary files | dvault-allocation-hotspots/satellite-changed-replay | allocation-hotspot-profile | 100 customers, 20 existing profile states each | changed satellite replay across 4 retained-state chunks | completed |  | 3 | 27.466 | 24.751 | 29.214 | 5657104 | 5652304 | 5659504 | scenario=satellite-changed-replay-filter; provider=SQLite local temporary files; baseline=provider-neutral-dvault-fallback; hashKeyVariant=sha256-v1-hex; storageProfile=HexString; callerHashDiffGeneration=outside-profile | 2100 profile satellite rows after changed replay latest lookup |
