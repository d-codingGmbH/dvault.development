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
| customer-profile-history | SQLite local temporary files | conventional-ef | classic-ef | 1 customer, 2 profile states | 50% repeat-change history | completed |  | 3 | 1.921 | 1.551 | 2.615 | 94560 | 94560 | 94560 | 2 customer profile history rows for C-100 |
| customer-profile-history | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 1 customer, 2 profile states | 50% repeat-change history | completed |  | 3 | 4.092 | 3.945 | 4.316 | 300824 | 300824 | 300824 | 1 customer hub row and 2 profile satellite rows for C-100 |
| customer-profile-history | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 1 customer, 2 profile states | 50% repeat-change history | completed |  | 3 | 3.290 | 3.092 | 3.543 | 260032 | 260032 | 260032 | 1 customer hub row and 2 profile satellite rows for C-100 |
| customer-profile-bulk-insert-only | SQLite local temporary files | conventional-ef-bulk | classic-ef | 100 customers, 1 profile state each | 0% repeat-change history | completed |  | 3 | 3.858 | 3.849 | 3.870 | 1201056 | 1201056 | 1201056 | 100 customer profile history rows for 100 customers |
| customer-profile-bulk-insert-only | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 100 customers, 1 profile state each | 0% repeat-change history | completed |  | 3 | 14.361 | 12.155 | 17.055 | 4181987 | 4181960 | 4182040 | 100 customer hubs and 100 profile satellite rows |
| customer-profile-bulk-insert-only | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 100 customers, 1 profile state each | 0% repeat-change history | completed |  | 3 | 5.355 | 5.333 | 5.381 | 1979904 | 1979904 | 1979904 | 100 customer hubs and 100 profile satellite rows |
| customer-profile-bulk-history | SQLite local temporary files | conventional-ef-bulk | classic-ef | 100 customers, 10 profile states each | 90% repeat-change history | completed |  | 3 | 36.385 | 27.207 | 47.325 | 11272112 | 11272112 | 11272112 | 1000 customer profile history rows for 100 customers |
| customer-profile-bulk-history | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 100 customers, 10 profile states each | 90% repeat-change history | completed |  | 3 | 83.745 | 80.383 | 87.022 | 22091424 | 22091424 | 22091424 | 100 customer hubs and 1000 profile satellite rows |
| customer-profile-bulk-history | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 100 customers, 10 profile states each | 90% repeat-change history | completed |  | 3 | 35.435 | 31.676 | 42.036 | 10179360 | 10179360 | 10179360 | 100 customer hubs and 1000 profile satellite rows |
| order-product-fulfillment-history | SQLite local temporary files | conventional-ef | classic-ef | 1 order-product relationship, 2 fulfillment states | 50% repeat-change history | completed |  | 3 | 3.836 | 3.623 | 4.200 | 187048 | 187024 | 187096 | 1 order, 1 product, 1 relationship, and 2 fulfillment history rows for O-1000/SKU-COFFEE |
| order-product-fulfillment-history | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 1 order-product relationship, 2 fulfillment states | 50% repeat-change history | completed |  | 3 | 6.121 | 5.973 | 6.246 | 414560 | 414560 | 414560 | 1 order hub, 1 product hub, 1 link, and 2 fulfillment satellite rows for O-1000/SKU-COFFEE |
| order-product-fulfillment-history | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 1 order-product relationship, 2 fulfillment states | 50% repeat-change history | completed |  | 3 | 4.177 | 4.015 | 4.411 | 318600 | 318600 | 318600 | 1 order hub, 1 product hub, 1 link, and 2 fulfillment satellite rows for O-1000/SKU-COFFEE |
| latest-satellite-read | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 100 customers, 10 profile states each | 90% repeat-change history latest read | completed |  | 3 | 9.719 | 7.196 | 14.185 | 1752680 | 1752680 | 1752680 | 100 latest profile satellite rows read from 1000 seeded profile states |
| latest-satellite-read | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 100 customers, 10 profile states each | 90% repeat-change history latest read | completed |  | 3 | 3.231 | 3.208 | 3.269 | 251888 | 251888 | 251888 | 100 latest profile satellite rows read from 1000 seeded profile states |
| pit-as-of-read | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 100 customers, 100 PIT rows, 2 satellite segments | as-of read after latest profile/status snapshots | completed |  | 3 | 11.678 | 11.333 | 12.187 | 2519512 | 2519512 | 2519512 | 100 PIT as-of rows read across profile and status satellite snapshots |
| pit-as-of-read | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 100 customers, 100 PIT rows, 2 satellite segments | as-of read after latest profile/status snapshots | completed |  | 3 | 10.213 | 10.006 | 10.459 | 2345560 | 2345560 | 2345560 | 100 PIT as-of rows read across profile and status satellite snapshots |
| bridge-traversal-read | SQLite local temporary files | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 1 hierarchy ancestor with 100 descendant bridge rows | maximum depth 3 of 5 | completed |  | 3 | 1.651 | 1.573 | 1.754 | 149656 | 149656 | 149656 | 60 bridge traversal rows read from 100 seeded hierarchy rows |
| bridge-traversal-read | SQLite local temporary files | dvault-adddvaultsqlite-optimized | sqlite-optimized-dvault | 1 hierarchy ancestor with 100 descendant bridge rows | maximum depth 3 of 5 | completed |  | 3 | 1.298 | 1.257 | 1.321 | 120552 | 120552 | 120552 | 60 bridge traversal rows read from 100 seeded hierarchy rows |
| compiled-model-startup | SQLite local temporary files | dvault-design-model | ef-model-build | 1 generated order hub row | runtime model precomputed outside measured operation | completed |  | 3 | 7.362 | 7.252 | 7.438 | 659264 | 658184 | 661424 | 1 generated order hub row read through ordinary DVault model building |
| compiled-model-startup | SQLite local temporary files | dvault-usemodel-runtime-model | ef-usemodel-runtime-model | 1 generated order hub row | runtime model precomputed outside measured operation | completed |  | 3 | 1.490 | 1.340 | 1.577 | 80248 | 80200 | 80344 | 1 generated order hub row read through precomputed UseModel(runtimeModel) |
| compiled-query-hub-read | SQLite local temporary files | ordinary-ef-query | direct-ef-query | 1 generated order hub row | stable shared-type table projection | completed |  | 3 | 1.344 | 1.300 | 1.375 | 81288 | 81240 | 81384 | 1 generated order hub row read through equivalent ordinary EF projection |
| compiled-query-hub-read | SQLite local temporary files | ef-compilequery | compiled-ef-query | 1 generated order hub row | stable shared-type table projection | completed |  | 3 | 1.026 | 0.982 | 1.104 | 63251 | 63208 | 63336 | 1 generated order hub row read through EF.CompileQuery stable projection |
| dbcontext-pooling-dvault-operation | SQLite local temporary files | adddbcontext | non-pooled-dvault-context | 1 generated order hub row | fixed metadata source and options-only context | completed |  | 3 | 2.462 | 2.185 | 2.871 | 164136 | 164136 | 164136 | 1 generated order hub row saved and read through AddDbContext fixed-model configuration |
| dbcontext-pooling-dvault-operation | SQLite local temporary files | adddbcontextpool | pooled-dvault-context | 1 generated order hub row | fixed metadata source and options-only context | completed |  | 3 | 1.724 | 1.631 | 1.779 | 86296 | 86296 | 86296 | 1 generated order hub row saved and read through AddDbContextPool fixed-model configuration |
| provider-native-bulk-ingestion | PostgreSQL external provider | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_POSTGRES_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | PostgreSQL external provider | dvault-adddvaultpostgres-optimized | postgres-optimized-dvault | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_POSTGRES_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | SQL Server external provider | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_SQLSERVER_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | SQL Server external provider | dvault-adddvaultsqlserver-optimized | sqlserver-optimized-dvault | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_SQLSERVER_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | MySQL external provider | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_MYSQL_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | MySQL external provider | dvault-adddvaultmysql-optimized | mysql-optimized-dvault | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_MYSQL_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | Oracle external provider | dvault-adddvault-fallback | provider-neutral-dvault-fallback | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_ORACLE_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
| provider-native-bulk-ingestion | Oracle external provider | dvault-adddvaultoracle-optimized | oracle-optimized-dvault | 20 order-product pairs, 3 fulfillment satellite operations | provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay | skipped | not configured: DVAULT_TEST_ORACLE_CONNECTION_STRING is not set or empty. | 0 |  |  |  |  |  |  | not executed |
