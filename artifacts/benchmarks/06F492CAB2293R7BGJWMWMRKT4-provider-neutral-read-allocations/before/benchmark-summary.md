# DVault Benchmark Summary

## Summary

- Benchmark baselines: 32
- Required provider: SQLite local temporary files
- Optional PostgreSQL provider: PostgreSQL external provider
- PostgreSQL execution status: skipped
- PostgreSQL skip reason: not configured: DVAULT_TEST_POSTGRES_CONNECTION_STRING is not set or empty.
- Optional provider status:
  - PostgreSQL external provider: skipped - not configured: DVAULT_TEST_POSTGRES_CONNECTION_STRING is not set or empty.
  - SQL Server external provider: skipped - not configured: DVAULT_TEST_SQLSERVER_CONNECTION_STRING is not set or empty.
  - MySQL external provider: skipped - not configured: DVAULT_TEST_MYSQL_CONNECTION_STRING is not set or empty.
  - Oracle external provider: skipped - not configured: DVAULT_TEST_ORACLE_CONNECTION_STRING is not set or empty.

## Run Context

- Iterations: 3
- Warmup iterations: 1
- Load timestamp storage: ProviderDefault
- Provider filter: all
- OS description: Debian GNU/Linux 13 (trixie)
- OS architecture: X64
- Process architecture: X64
- Processor count: 32
- .NET runtime description: .NET 10.0.8
- .NET runtime version: 10.0.8

## Results

| Scenario | Provider | Baseline | Strategy family | Dataset size | Change ratio | Execution status | Skip reason | Iterations | Mean ms | Min ms | Max ms | Mean allocated bytes | Min allocated bytes | Max allocated bytes | Persisted outcome |
| --- | --- | --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| customer-profile-history | SQLite local temporary files | conventional-ef | classic-ef | 1 customer, 2 profile states | 50% repeat-change history | completed |  | 3 | 2.027 | 1.480 | 2.967 | 94560 | 94560 | 94560 | 2 customer profile history rows for C-100 |
| customer-profile-history | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 1 customer, 2 profile states | 50% repeat-change history | completed |  | 3 | 4.663 | 4.415 | 4.924 | 300824 | 300824 | 300824 | 1 customer hub row and 2 profile satellite rows for C-100 |
| customer-profile-history | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 1 customer, 2 profile states | 50% repeat-change history | completed |  | 3 | 3.581 | 3.274 | 3.925 | 260032 | 260032 | 260032 | 1 customer hub row and 2 profile satellite rows for C-100 |
| customer-profile-bulk-insert-only | SQLite local temporary files | conventional-ef-bulk | classic-ef | 100 customers, 1 profile state each | 0% repeat-change history | completed |  | 3 | 3.729 | 3.636 | 3.865 | 1201056 | 1201056 | 1201056 | 100 customer profile history rows for 100 customers |
| customer-profile-bulk-insert-only | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 100 customers, 1 profile state each | 0% repeat-change history | completed |  | 3 | 14.566 | 12.239 | 17.714 | 4181987 | 4181960 | 4182040 | 100 customer hubs and 100 profile satellite rows |
| customer-profile-bulk-insert-only | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 100 customers, 1 profile state each | 0% repeat-change history | completed |  | 3 | 5.919 | 5.672 | 6.385 | 1981779 | 1979904 | 1983936 | 100 customer hubs and 100 profile satellite rows |
| customer-profile-bulk-history | SQLite local temporary files | conventional-ef-bulk | classic-ef | 100 customers, 10 profile states each | 90% repeat-change history | completed |  | 3 | 29.798 | 26.942 | 34.366 | 11272112 | 11272112 | 11272112 | 1000 customer profile history rows for 100 customers |
| customer-profile-bulk-history | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 100 customers, 10 profile states each | 90% repeat-change history | completed |  | 3 | 82.469 | 75.862 | 91.456 | 22091424 | 22091424 | 22091424 | 100 customer hubs and 1000 profile satellite rows |
| customer-profile-bulk-history | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 100 customers, 10 profile states each | 90% repeat-change history | completed |  | 3 | 36.207 | 32.997 | 41.837 | 10179360 | 10179360 | 10179360 | 100 customer hubs and 1000 profile satellite rows |
| order-product-fulfillment-history | SQLite local temporary files | conventional-ef | classic-ef | 1 order-product relationship, 2 fulfillment states | 50% repeat-change history | completed |  | 3 | 4.208 | 3.749 | 4.575 | 186968 | 186944 | 187016 | 1 order, 1 product, 1 relationship, and 2 fulfillment history rows for O-1000/SKU-COFFEE |
| order-product-fulfillment-history | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 1 order-product relationship, 2 fulfillment states | 50% repeat-change history | completed |  | 3 | 6.649 | 6.401 | 6.845 | 414560 | 414560 | 414560 | 1 order hub, 1 product hub, 1 link, and 2 fulfillment satellite rows for O-1000/SKU-COFFEE |
| order-product-fulfillment-history | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 1 order-product relationship, 2 fulfillment states | 50% repeat-change history | completed |  | 3 | 4.360 | 3.996 | 4.595 | 318600 | 318600 | 318600 | 1 order hub, 1 product hub, 1 link, and 2 fulfillment satellite rows for O-1000/SKU-COFFEE |
| latest-satellite-read | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 100 customers, 10 profile states each | 90% repeat-change history latest read | completed |  | 3 | 7.605 | 6.803 | 9.197 | 2201072 | 2201072 | 2201072 | 100 latest profile satellite rows read from 1000 seeded profile states |
| latest-satellite-read | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 100 customers, 10 profile states each | 90% repeat-change history latest read | completed |  | 3 | 3.094 | 3.021 | 3.135 | 251888 | 251888 | 251888 | 100 latest profile satellite rows read from 1000 seeded profile states |
| pit-as-of-read | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 100 customers, 100 PIT rows, 2 satellite segments | as-of read after latest profile/status snapshots | completed |  | 3 | 12.146 | 10.806 | 13.228 | 2603512 | 2603512 | 2603512 | 100 PIT as-of rows read across profile and status satellite snapshots |
| pit-as-of-read | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 100 customers, 100 PIT rows, 2 satellite segments | as-of read after latest profile/status snapshots | completed |  | 3 | 11.791 | 11.611 | 12.020 | 2429560 | 2429560 | 2429560 | 100 PIT as-of rows read across profile and status satellite snapshots |
| bridge-traversal-read | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 1 hierarchy ancestor with 100 descendant bridge rows | maximum depth 3 of 5 | completed |  | 3 | 1.729 | 1.677 | 1.828 | 181960 | 181960 | 181960 | 60 bridge traversal rows read from 100 seeded hierarchy rows |
| bridge-traversal-read | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 1 hierarchy ancestor with 100 descendant bridge rows | maximum depth 3 of 5 | completed |  | 3 | 1.375 | 1.332 | 1.418 | 122952 | 122952 | 122952 | 60 bridge traversal rows read from 100 seeded hierarchy rows |
| compiled-model-startup | SQLite local temporary files | dvault-design-model | ef-model-build | 1 generated order hub row | runtime model precomputed outside measured operation | completed |  | 3 | 7.355 | 6.909 | 7.719 | 659144 | 658192 | 661048 | 1 generated order hub row read through ordinary DVault model building |
| compiled-model-startup | SQLite local temporary files | dvault-usemodel-runtime-model | ef-usemodel-runtime-model | 1 generated order hub row | runtime model precomputed outside measured operation | completed |  | 3 | 1.527 | 1.405 | 1.732 | 80200 | 80200 | 80200 | 1 generated order hub row read through precomputed UseModel(runtimeModel) |
| compiled-query-hub-read | SQLite local temporary files | ordinary-ef-query | direct-ef-query | 1 generated order hub row | stable shared-type table projection | completed |  | 3 | 1.354 | 1.306 | 1.417 | 81240 | 81240 | 81240 | 1 generated order hub row read through equivalent ordinary EF projection |
| compiled-query-hub-read | SQLite local temporary files | ef-compilequery | compiled-ef-query | 1 generated order hub row | stable shared-type table projection | completed |  | 3 | 1.064 | 1.001 | 1.176 | 63251 | 63208 | 63336 | 1 generated order hub row read through EF.CompileQuery stable projection |
| dbcontext-pooling-dvault-operation | SQLite local temporary files | adddbcontext | non-pooled-dvault-context | 1 generated order hub row | fixed metadata source and options-only context | completed |  | 3 | 2.487 | 2.133 | 3.052 | 164269 | 163968 | 164872 | 1 generated order hub row saved and read through AddDbContext fixed-model configuration |
| dbcontext-pooling-dvault-operation | SQLite local temporary files | adddbcontextpool | pooled-dvault-context | 1 generated order hub row | fixed metadata source and options-only context | completed |  | 3 | 1.828 | 1.728 | 2.008 | 88768 | 88768 | 88768 | 1 generated order hub row saved and read through AddDbContextPool fixed-model configuration |
| provider-native-bulk-ingestion | PostgreSQL external provider | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_POSTGRES_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | PostgreSQL external provider | dvault-adddvaultpostgres-optimized | postgres-optimized-dvault | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_POSTGRES_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | SQL Server external provider | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_SQLSERVER_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | SQL Server external provider | dvault-adddvaultsqlserver-optimized | sqlserver-optimized-dvault | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_SQLSERVER_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | MySQL external provider | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_MYSQL_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | MySQL external provider | dvault-adddvaultmysql-optimized | mysql-optimized-dvault | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_MYSQL_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | Oracle external provider | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_ORACLE_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | Oracle external provider | dvault-adddvaultoracle-optimized | oracle-optimized-dvault | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_ORACLE_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
