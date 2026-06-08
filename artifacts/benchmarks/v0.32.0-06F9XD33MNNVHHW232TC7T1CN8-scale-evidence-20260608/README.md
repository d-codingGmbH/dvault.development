# v0.32.0 MySQL And PostgreSQL Small-Batch Evidence

Ticket: `06F9XD33MNNVHHW232TC7T1CN8`

## Baseline

The `before` artifact set is copied from the authoritative v0.32.0 all-provider baseline:

- `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-20260607`

This keeps the 2026-06-07 v0.32.0 evidence as the current comparison source and leaves the 2026-06-06 v0.31.0 seed bundle as historical context only.

## After Captures

The `after/postgres` artifact set was captured with PostgreSQL through the Podman network from the local SDK container:

- provider filter: `postgres`
- iterations: `5`
- warmup iterations: `1`
- image: `mcr.microsoft.com/dotnet/sdk:10.0`
- container: `postgres`
- connection: `Host=10.88.0.2;Port=5432;Database=dvault_tests;Username=dvault;Password=local-secret;Include Error Detail=true;SSL Mode=Disable;Timeout=10;Command Timeout=60`

The `after/mysql` artifact set was captured from the host against the published local MySQL port:

- provider filter: `mysql`
- iterations: `5`
- warmup iterations: `1`
- container: `mysql`
- connection: `Server=127.0.0.1;Port=3306;Database=dvault_tests;User=dvault;Password=local-secret;SslMode=Disabled;AllowPublicKeyRetrieval=True`

## Interpretation

PostgreSQL eligibility remains unchanged. The after artifact distinguishes retained direct or UNNEST execution below the staged boundary and staged COPY at or above the staged boundary:

- `customer-profile-scale-10x1`: optimized `12.824 ms`, fallback `24.446 ms`, execution path `DVault PostgreSQL retained direct or UNNEST save path`
- `customer-profile-scale-10x10`: optimized `20.338 ms`, fallback `28.120 ms`, execution path `DVault PostgreSQL staged bulk save path`
- `customer-profile-scale-100x10`: optimized `28.776 ms`, fallback `56.001 ms`
- `customer-profile-scale-1000x10`: optimized `132.800 ms`, fallback `462.369 ms`

MySQL optimized registration now deliberately routes the tiny satellite-history rows through provider-neutral fallback while retaining staged bulk for larger rows:

- `customer-profile-scale-10x1`: after optimized-registration fallback `21.449 ms`; v0.32.0 optimized baseline `28.798 ms`; fallback cause includes `MySqlTinySatelliteHistoryProviderNeutralFallback`
- `customer-profile-scale-10x10`: after optimized-registration fallback `27.892 ms`; v0.32.0 optimized baseline `43.905 ms`; fallback cause is `MySqlTinySatelliteHistoryProviderNeutralFallback`
- `customer-profile-scale-1000x10`: after optimized `592.903 ms`, fallback `806.769 ms`
- `customer-profile-scale-10000x1`: after optimized `992.123 ms`, fallback `1496.210 ms`
- `customer-profile-scale-10000x10`: after optimized `3728.300 ms`, fallback `6562.204 ms`
