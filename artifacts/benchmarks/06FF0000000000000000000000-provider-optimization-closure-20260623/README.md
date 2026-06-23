# Provider Optimization Closure Evidence

This bundle preserves the provider-configured benchmark triplets used to close the v0.42 provider optimization gaps on 2026-06-23. Each subdirectory contains the unmodified `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` emitted by the benchmark runner.

| Provider | Source run | Key completed rows |
| --- | --- | --- |
| PostgreSQL | `postgres-podman-live` | Staged `COPY` save `43.757` ms, latest read `15.068` ms, PIT read `21.324` ms, bridge read `9.002` ms. |
| SQL Server | `sqlserver-live` | `SqlBulkCopy` save `149.490` ms, latest read `20.337` ms, PIT read `59.163` ms, bridge read `9.523` ms. |
| MySQL | `mysql-live` | Multi-row save `15.827` ms, staged save `26.055` ms, large mixed provider-neutral fallback `145.601` ms, latest read `13.878` ms, PIT read `14.461` ms, bridge read `3.083` ms. |
| Oracle | `oracle-lob-prefetch` | Direct optimized save `92.537` ms, latest read `18.783` ms, PIT read `26.857` ms, bridge read `3.922` ms. |
| DB2 | `db2-rowcap-1000` | Row-cap tuned optimized save `101.037` ms, latest read `14.615` ms, PIT read `27.207` ms, bridge read `4.831` ms. |

Oracle read tuning uses the ODP.NET command-level LOB prefetch and fetch-buffer settings added in this change. DB2 save tuning uses the measured 1000-row command cap while preserving the clean-context, transaction-participating set-based save path.
