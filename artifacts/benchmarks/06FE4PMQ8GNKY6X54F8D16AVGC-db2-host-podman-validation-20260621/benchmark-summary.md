# DVault Benchmark Summary

## Summary

- Benchmark baselines: 5
- Required provider: SQLite local temporary files
- Optional PostgreSQL provider: PostgreSQL external provider
- PostgreSQL execution status: skipped
- PostgreSQL skip reason: not configured: DVAULT_TEST_POSTGRES_CONNECTION_STRING is not set or empty.
- Optional provider status:
  - DB2 external provider: completed

## Run Context

- Iterations: 1
- Warmup iterations: 0
- Load timestamp storage: ProviderDefault
- Provider filter: db2
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
| provider-native-bulk-ingestion | DB2 external provider | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | completed |  | 1 | 402.576 | 402.576 | 402.576 | 5637368 | 5637368 | 5637368 | scenario=provider-native-bulk-ingestion; provider=DB2 external provider; baseline=dvault-adddvault-fallback; strategyFamily=provider-neutral-dvault-fallback; executionPath=DVault provider-neutral fallback path; selectedStrategy=<none>; providerSpecificSaveStrategy=fallback; hashKeyVariant=sha256-v1-hex; stableHashAlgorithm=sha256-v1; digestBytes=32; hashKeyStorage=HexString; hashKeyPayloadBytes=64; saveStrategyStatus=ProviderNeutralFallback; provider=IBM.EntityFrameworkCore; selectedStrategy=<none>; candidateStrategies=none; candidates=0; fallbackCauses=NoProviderSpecificStrategyRegistered; requestCount=5; hubOperations=40; linkOperations=20; satelliteOperations=3; nativeBulkGate=clean-context,no-multi-active-satellites,provider-eligible-bulk-request | 20 order hubs, 20 product hubs, 20 order-product links, and 2 fulfillment satellite rows |
| provider-native-bulk-ingestion | DB2 external provider | dvault-adddvaultdb2-optimized | db2-optimized-dvault | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | completed |  | 1 | 55.076 | 55.076 | 55.076 | 974904 | 974904 | 974904 | scenario=provider-native-bulk-ingestion; provider=DB2 external provider; baseline=dvault-adddvaultdb2-optimized; strategyFamily=db2-optimized-dvault; executionPath=DVault DB2 optimized save path; selectedStrategy=Db2DataVaultSaveStrategy; db2SaveBoundary=clean-context-set-based; stagedBulkBoundary=not-supported; cleanupBoundary=direct-provider-transaction; hashKeyVariant=sha256-v1-hex; stableHashAlgorithm=sha256-v1; digestBytes=32; hashKeyStorage=HexString; hashKeyPayloadBytes=64; saveStrategyStatus=ProviderStrategySelected; provider=IBM.EntityFrameworkCore; selectedStrategy=Db2DataVaultSaveStrategy; candidateStrategies=Db2DataVaultSaveStrategy; candidates=1; fallbackCauses=none; requestCount=5; hubOperations=40; linkOperations=20; satelliteOperations=3; nativeBulkGate=clean-context,no-multi-active-satellites,provider-eligible-bulk-request | 20 order hubs, 20 product hubs, 20 order-product links, and 2 fulfillment satellite rows |
| latest-satellite-read | DB2 external provider | dvault-adddvaultdb2-optimized | db2-optimized-dvault | 100 customers, 10 profile states each | 90% repeat-change history latest read | completed |  | 1 | 20.498 | 20.498 | 20.498 | 843176 | 843176 | 843176 | scenario=latest-satellite-read; provider=DB2 external provider; baseline=dvault-adddvaultdb2-optimized; strategyFamily=db2-optimized-dvault; executionPath=DVault DB2 optimized latest satellite read path; selectedStrategy=Db2DataVaultReadStrategy; plannedReadStrategy=Db2DataVaultReadStrategy; readShape=LatestSatellite; hashKeyVariant=sha256-v1-hex; stableHashAlgorithm=sha256-v1; digestBytes=32; hashKeyStorage=HexString; hashKeyPayloadBytes=64; readStrategyStatus=ProviderStrategySelected; provider=IBM.EntityFrameworkCore; selectedStrategy=Db2DataVaultReadStrategy; candidates=1; fallbackCauses=none; readShape=LatestSatellite; readShapeProviderStatus=ProviderStrategySelected; readShapeFallbackCauses=none | 100 latest profile satellite rows read from 1000 seeded profile states |
| pit-as-of-read | DB2 external provider | dvault-adddvaultdb2-optimized | db2-optimized-dvault | 100 customers, 100 PIT rows, 2 satellite segments | as-of read after latest profile/status snapshots | completed |  | 1 | 52.921 | 52.921 | 52.921 | 5449816 | 5449816 | 5449816 | scenario=pit-as-of-read; provider=DB2 external provider; baseline=dvault-adddvaultdb2-optimized; strategyFamily=db2-optimized-dvault; executionPath=DVault DB2 optimized PIT read path; selectedStrategy=Db2DataVaultReadStrategy; plannedReadStrategy=Db2DataVaultReadStrategy; readShape=PitAsOf; hashKeyVariant=sha256-v1-hex; stableHashAlgorithm=sha256-v1; digestBytes=32; hashKeyStorage=HexString; hashKeyPayloadBytes=64; readStrategyStatus=ProviderStrategySelected; provider=IBM.EntityFrameworkCore; selectedStrategy=Db2DataVaultReadStrategy; candidates=1; fallbackCauses=none; readShape=PitAsOf; readShapeProviderStatus=ProviderStrategySelected; readShapeFallbackCauses=none | 100 PIT as-of rows read across profile and status satellite snapshots |
| bridge-traversal-read | DB2 external provider | dvault-adddvaultdb2-optimized | db2-optimized-dvault | 1 hierarchy ancestor with 100 descendant bridge rows | maximum depth 3 of 5 | completed |  | 1 | 9.897 | 9.897 | 9.897 | 261656 | 261656 | 261656 | scenario=bridge-traversal-read; provider=DB2 external provider; baseline=dvault-adddvaultdb2-optimized; strategyFamily=db2-optimized-dvault; executionPath=DVault DB2 optimized bridge read path; selectedStrategy=Db2DataVaultReadStrategy; plannedReadStrategy=Db2DataVaultReadStrategy; readShape=Bridge; hashKeyVariant=sha256-v1-hex; stableHashAlgorithm=sha256-v1; digestBytes=32; hashKeyStorage=HexString; hashKeyPayloadBytes=64; readStrategyStatus=ProviderStrategySelected; provider=IBM.EntityFrameworkCore; selectedStrategy=Db2DataVaultReadStrategy; candidates=1; fallbackCauses=none; readShape=Bridge; readShapeProviderStatus=ProviderStrategySelected; readShapeFallbackCauses=none | 60 bridge traversal rows read from 100 seeded hierarchy rows |
