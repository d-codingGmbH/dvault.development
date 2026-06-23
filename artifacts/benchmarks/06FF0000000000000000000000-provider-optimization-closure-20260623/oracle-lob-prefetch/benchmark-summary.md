# DVault Benchmark Summary

## Summary

- Benchmark baselines: 5
- Required provider: SQLite local temporary files
- Optional PostgreSQL provider: PostgreSQL external provider
- PostgreSQL execution status: skipped
- PostgreSQL skip reason: not configured: DVAULT_TEST_POSTGRES_CONNECTION_STRING is not set or empty.
- Optional provider status:
  - Oracle external provider: completed

## Run Context

- Iterations: 5
- Warmup iterations: 1
- Load timestamp storage: ProviderDefault
- Provider filter: oracle
- Hash key variants: sha256-v1-hex
- OS description: Microsoft Windows 10.0.26200
- OS architecture: X64
- Process architecture: X64
- Processor count: 32
- .NET runtime description: .NET 10.0.9
- .NET runtime version: 10.0.9

## Results

| Scenario | Provider | Baseline | Strategy family | Dataset size | Change ratio | Execution status | Skip reason | Iterations | Mean ms | Min ms | Max ms | Mean allocated bytes | Min allocated bytes | Max allocated bytes | Execution detail | Persisted outcome |
| --- | --- | --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- | --- |
| provider-native-bulk-ingestion | Oracle external provider | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 300 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | completed |  | 5 | 302.278 | 243.297 | 453.933 | 17285570 | 17281728 | 17291944 | scenario=provider-native-bulk-ingestion; provider=Oracle external provider; baseline=dvault-adddvault-fallback; strategyFamily=provider-neutral-dvault-fallback; executionPath=DVault provider-neutral fallback path; selectedStrategy=<none>; comparisonBoundary=staged-eligible-903-operations; hashKeyVariant=sha256-v1-hex; stableHashAlgorithm=sha256-v1; digestBytes=32; hashKeyStorage=HexString; hashKeyPayloadBytes=64; saveStrategyStatus=ProviderNeutralFallback; provider=Oracle.EntityFrameworkCore; selectedStrategy=<none>; candidateStrategies=none; candidates=0; fallbackCauses=NoProviderSpecificStrategyRegistered; requestCount=5; hubOperations=600; linkOperations=300; satelliteOperations=3; nativeBulkGate=clean-context,no-multi-active-satellites,provider-eligible-bulk-request | 300 order hubs, 300 product hubs, 300 order-product links, and 2 fulfillment satellite rows |
| provider-native-bulk-ingestion | Oracle external provider | dvault-adddvaultoracle-optimized | oracle-optimized-dvault | 300 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | completed |  | 5 | 92.537 | 80.735 | 128.035 | 3623258 | 3573648 | 3706368 | scenario=provider-native-bulk-ingestion; provider=Oracle external provider; baseline=dvault-adddvaultoracle-optimized; strategyFamily=oracle-optimized-dvault; executionPath=DVault Oracle direct optimized save path; selectedStrategy=OracleDataVaultSaveStrategy; oracleBulkBoundary=direct-oracle-batching; stagedOracleBulk=not-selected-no-measured-win; cleanupBoundary=direct-provider-transaction; hashKeyVariant=sha256-v1-hex; stableHashAlgorithm=sha256-v1; digestBytes=32; hashKeyStorage=HexString; hashKeyPayloadBytes=64; saveStrategyStatus=ProviderStrategySelected; provider=Oracle.EntityFrameworkCore; selectedStrategy=OracleDataVaultSaveStrategy; candidateStrategies=OracleDataVaultSaveStrategy; candidates=1; fallbackCauses=none; requestCount=5; hubOperations=600; linkOperations=300; satelliteOperations=3; nativeBulkGate=clean-context,no-multi-active-satellites,provider-eligible-bulk-request | 300 order hubs, 300 product hubs, 300 order-product links, and 2 fulfillment satellite rows |
| latest-satellite-read | Oracle external provider | dvault-adddvaultoracle-optimized | oracle-optimized-dvault | 100 customers, 10 profile states each | 90% repeat-change history latest read | completed |  | 5 | 18.783 | 12.452 | 37.252 | 652099 | 650184 | 658304 | scenario=latest-satellite-read; provider=Oracle external provider; baseline=dvault-adddvaultoracle-optimized; strategyFamily=oracle-optimized-dvault; executionPath=DVault Oracle optimized latest satellite read path; selectedStrategy=OracleDataVaultReadStrategy; plannedReadStrategy=OracleDataVaultReadStrategy; readShape=LatestSatellite; hashKeyVariant=sha256-v1-hex; stableHashAlgorithm=sha256-v1; digestBytes=32; hashKeyStorage=HexString; hashKeyPayloadBytes=64; readStrategyStatus=ProviderStrategySelected; provider=Oracle.EntityFrameworkCore; selectedStrategy=OracleDataVaultReadStrategy; candidates=1; fallbackCauses=none; readShape=LatestSatellite; readShapeProviderStatus=ProviderStrategySelected; readShapeFallbackCauses=none | 100 latest profile satellite rows read from 1000 seeded profile states |
| pit-as-of-read | Oracle external provider | dvault-adddvaultoracle-optimized | oracle-optimized-dvault | 100 customers, 100 PIT rows, 2 satellite segments | as-of read after latest profile/status snapshots | completed |  | 5 | 26.857 | 25.311 | 28.997 | 1613522 | 1613192 | 1614696 | scenario=pit-as-of-read; provider=Oracle external provider; baseline=dvault-adddvaultoracle-optimized; strategyFamily=oracle-optimized-dvault; executionPath=DVault Oracle optimized PIT read path; selectedStrategy=OracleDataVaultReadStrategy; plannedReadStrategy=OracleDataVaultReadStrategy; readShape=PitAsOf; hashKeyVariant=sha256-v1-hex; stableHashAlgorithm=sha256-v1; digestBytes=32; hashKeyStorage=HexString; hashKeyPayloadBytes=64; readStrategyStatus=ProviderStrategySelected; provider=Oracle.EntityFrameworkCore; selectedStrategy=OracleDataVaultReadStrategy; candidates=1; fallbackCauses=none; readShape=PitAsOf; readShapeProviderStatus=ProviderStrategySelected; readShapeFallbackCauses=none | 100 PIT as-of rows read across profile and status satellite snapshots |
| bridge-traversal-read | Oracle external provider | dvault-adddvaultoracle-optimized | oracle-optimized-dvault | 1 hierarchy ancestor with 100 descendant bridge rows | maximum depth 3 of 5 | completed |  | 5 | 3.922 | 3.561 | 4.290 | 192699 | 192680 | 192728 | scenario=bridge-traversal-read; provider=Oracle external provider; baseline=dvault-adddvaultoracle-optimized; strategyFamily=oracle-optimized-dvault; executionPath=DVault Oracle optimized bridge read path; selectedStrategy=OracleDataVaultReadStrategy; plannedReadStrategy=OracleDataVaultReadStrategy; readShape=Bridge; hashKeyVariant=sha256-v1-hex; stableHashAlgorithm=sha256-v1; digestBytes=32; hashKeyStorage=HexString; hashKeyPayloadBytes=64; readStrategyStatus=ProviderStrategySelected; provider=Oracle.EntityFrameworkCore; selectedStrategy=OracleDataVaultReadStrategy; candidates=1; fallbackCauses=none; readShape=Bridge; readShapeProviderStatus=ProviderStrategySelected; readShapeFallbackCauses=none | 60 bridge traversal rows read from 100 seeded hierarchy rows |
