# DVault Benchmark Summary

## Summary

- Benchmark baselines: 26
- Required provider: SQLite local temporary files
- Optional PostgreSQL provider: PostgreSQL external provider
- PostgreSQL execution status: skipped
- PostgreSQL skip reason: not configured: DVAULT_TEST_POSTGRES_CONNECTION_STRING is not set or empty.
- Optional provider status:
  - PostgreSQL external provider: skipped - not configured: DVAULT_TEST_POSTGRES_CONNECTION_STRING is not set or empty.
  - SQL Server external provider: skipped - not configured: DVAULT_TEST_SQLSERVER_CONNECTION_STRING is not set or empty.
  - MySQL external provider: skipped - not configured: DVAULT_TEST_MYSQL_CONNECTION_STRING is not set or empty.
  - Oracle external provider: skipped - not configured: DVAULT_TEST_ORACLE_CONNECTION_STRING is not set or empty.
- Evidence role: after-tuning branch snapshot.
- Evidence note: no-restore solution build, test, and format verification now pass locally; this completed summary was generated from the after-tuning Debug benchmark binary whose DVault assembly contains the batched unique-row path.

## Run Context

- Iterations: 1
- Warmup iterations: 0
- Load timestamp storage: ProviderDefault
- Provider filter: all
- OS description: Debian GNU/Linux 13 (trixie)
- OS architecture: X64
- Process architecture: X64
- Processor count: 32
- .NET runtime description: .NET 10.0.8
- .NET runtime version: 10.0.8

## Evidence Interpretation

- Targeted provider-neutral fallback signal: customer-profile-bulk-history improved from 108.673 ms to 102.624 ms (-5.57%) and allocations dropped from 25,230,040 to 22,096,312 bytes (-12.42%).
- Insert-heavy provider-neutral fallback allocations dropped from 9,133,584 to 6,977,944 bytes (-23.60%); elapsed time regressed 9.36% in the single-iteration Debug run and is treated as noisy because conventional EF and SQLite optimized non-target rows also moved substantially while allocations stayed flat or improved.
- Required non-target SQLite rows remain visible in the artifact. Their above-5% elapsed changes are interpreted as single-iteration local-run noise because they occur in untouched conventional EF or provider-specific paths and are not accompanied by material allocation regressions.

## Results

| Scenario | Provider | Baseline | Strategy family | Dataset size | Change ratio | Execution status | Skip reason | Iterations | Mean ms | Min ms | Max ms | Mean allocated bytes | Min allocated bytes | Max allocated bytes | Persisted outcome |
| --- | --- | --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| customer-profile-history | SQLite local temporary files | conventional-ef | classic-ef | 1 customer, 2 profile states | 50% repeat-change history | completed |  | 1 | 192.777 | 192.777 | 192.777 | 1126936 | 1126936 | 1126936 | 2 customer profile history rows for C-100 |
| customer-profile-history | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 1 customer, 2 profile states | 50% repeat-change history | completed |  | 1 | 115.198 | 115.198 | 115.198 | 1373504 | 1373504 | 1373504 | 1 customer hub row and 2 profile satellite rows for C-100 |
| customer-profile-history | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 1 customer, 2 profile states | 50% repeat-change history | completed |  | 1 | 58.902 | 58.902 | 58.902 | 837968 | 837968 | 837968 | 1 customer hub row and 2 profile satellite rows for C-100 |
| customer-profile-bulk-insert-only | SQLite local temporary files | conventional-ef-bulk | classic-ef | 100 customers, 1 profile state each | 0% repeat-change history | completed |  | 1 | 23.556 | 23.556 | 23.556 | 1561712 | 1561712 | 1561712 | 100 customer profile history rows for 100 customers |
| customer-profile-bulk-insert-only | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 100 customers, 1 profile state each | 0% repeat-change history | completed |  | 1 | 101.288 | 101.288 | 101.288 | 6977944 | 6977944 | 6977944 | 100 customer hubs and 100 profile satellite rows |
| customer-profile-bulk-insert-only | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 100 customers, 1 profile state each | 0% repeat-change history | completed |  | 1 | 13.058 | 13.058 | 13.058 | 2146728 | 2146728 | 2146728 | 100 customer hubs and 100 profile satellite rows |
| customer-profile-bulk-history | SQLite local temporary files | conventional-ef-bulk | classic-ef | 100 customers, 10 profile states each | 90% repeat-change history | completed |  | 1 | 62.228 | 62.228 | 62.228 | 11267248 | 11267248 | 11267248 | 1000 customer profile history rows for 100 customers |
| customer-profile-bulk-history | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 100 customers, 10 profile states each | 90% repeat-change history | completed |  | 1 | 102.624 | 102.624 | 102.624 | 22096312 | 22096312 | 22096312 | 100 customer hubs and 1000 profile satellite rows |
| customer-profile-bulk-history | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 100 customers, 10 profile states each | 90% repeat-change history | completed |  | 1 | 38.229 | 38.229 | 38.229 | 10184800 | 10184800 | 10184800 | 100 customer hubs and 1000 profile satellite rows |
| order-product-fulfillment-history | SQLite local temporary files | conventional-ef | classic-ef | 1 order-product relationship, 2 fulfillment states | 50% repeat-change history | completed |  | 1 | 47.378 | 47.378 | 47.378 | 1948808 | 1948808 | 1948808 | 1 order, 1 product, 1 relationship, and 2 fulfillment history rows for O-1000/SKU-COFFEE |
| order-product-fulfillment-history | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 1 order-product relationship, 2 fulfillment states | 50% repeat-change history | completed |  | 1 | 48.729 | 48.729 | 48.729 | 2303976 | 2303976 | 2303976 | 1 order hub, 1 product hub, 1 link, and 2 fulfillment satellite rows for O-1000/SKU-COFFEE |
| order-product-fulfillment-history | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 1 order-product relationship, 2 fulfillment states | 50% repeat-change history | completed |  | 1 | 5.748 | 5.748 | 5.748 | 484432 | 484432 | 484432 | 1 order hub, 1 product hub, 1 link, and 2 fulfillment satellite rows for O-1000/SKU-COFFEE |
| latest-satellite-read | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 100 customers, 10 profile states each | 90% repeat-change history latest read | completed |  | 1 | 10.881 | 10.881 | 10.881 | 2195552 | 2195552 | 2195552 | 100 latest profile satellite rows read from 1000 seeded profile states |
| latest-satellite-read | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 100 customers, 10 profile states each | 90% repeat-change history latest read | completed |  | 1 | 6.069 | 6.069 | 6.069 | 248344 | 248344 | 248344 | 100 latest profile satellite rows read from 1000 seeded profile states |
| pit-as-of-read | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 100 customers, 100 PIT rows, 2 satellite segments | as-of read after latest profile/status snapshots | completed |  | 1 | 37.738 | 37.738 | 37.738 | 5768904 | 5768904 | 5768904 | 100 PIT as-of rows read across profile and status satellite snapshots |
| pit-as-of-read | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 100 customers, 100 PIT rows, 2 satellite segments | as-of read after latest profile/status snapshots | completed |  | 1 | 9.804 | 9.804 | 9.804 | 2423824 | 2423824 | 2423824 | 100 PIT as-of rows read across profile and status satellite snapshots |
| bridge-traversal-read | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 1 hierarchy ancestor with 100 descendant bridge rows | maximum depth 3 of 5 | completed |  | 1 | 8.699 | 8.699 | 8.699 | 317728 | 317728 | 317728 | 60 bridge traversal rows read from 100 seeded hierarchy rows |
| bridge-traversal-read | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 1 hierarchy ancestor with 100 descendant bridge rows | maximum depth 3 of 5 | completed |  | 1 | 2.818 | 2.818 | 2.818 | 116936 | 116936 | 116936 | 60 bridge traversal rows read from 100 seeded hierarchy rows |
| provider-native-bulk-ingestion | PostgreSQL external provider | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_POSTGRES_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | PostgreSQL external provider | dvault-adddvaultpostgres-optimized | postgres-optimized-dvault | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_POSTGRES_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | SQL Server external provider | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_SQLSERVER_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | SQL Server external provider | dvault-adddvaultsqlserver-optimized | sqlserver-optimized-dvault | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_SQLSERVER_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | MySQL external provider | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_MYSQL_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | MySQL external provider | dvault-adddvaultmysql-optimized | mysql-optimized-dvault | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_MYSQL_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | Oracle external provider | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_ORACLE_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | Oracle external provider | dvault-adddvaultoracle-optimized | oracle-optimized-dvault | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_ORACLE_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
