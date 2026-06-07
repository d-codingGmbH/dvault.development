## Developer Evidence Capture

Persisted benchmark evidence for ticket 06F9XD26D2MHVAKZ2GCZ67BEFC.

Scale baseline bundle:
- Path: artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-20260607
- Command shape: benchmark executable with --provider all --scale --iterations 5 --warmup 1
- Result: 120 rows, all completed; SQLite plus PostgreSQL, SQL Server, MySQL, and Oracle provider lanes are present.

Smoke/read cleanup verification bundle:
- Path: artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607
- Command shape: benchmark executable with --provider all --iterations 1 --warmup 0
- Result: 50 rows, all completed. Bridge-traversal-read rows completed for PostgreSQL, SQL Server, MySQL, and Oracle with provider read strategies selected and fallbackCauses=none.

Operational note:
- PostgreSQL, MySQL, and Oracle used the running Podman provider containers and existing configured databases.
- SQL Server used the running sqlserver Podman container against its existing master database because the dvault_tests database/login was not available in this container. No database provisioning or product behavior change was performed.

Downstream tuning row callouts from the scale bundle:
- SQL Server customer-profile-scale-10x1 optimized row: mean 20.938 ms; fallbackCauses=SqlServerMinimumOperationThreshold; satelliteOperations=10.
- SQL Server customer-profile-scale-1000x1 optimized row: mean 144.780 ms; fallbackCauses=SqlServerMaximumSatelliteOperationThreshold; satelliteOperations=1000.
- SQL Server customer-profile-scale-10000x1 optimized row: mean 1021.385 ms; fallbackCauses=SqlServerMaximumSatelliteOperationThreshold; satelliteOperations=10000.
- SQL Server customer-profile-scale-100x10 optimized row: mean 80.569 ms; fallbackCauses=SqlServerMaximumSatelliteOperationThreshold; satelliteOperations=1000.
- SQL Server customer-profile-scale-1000x10 optimized row: mean 560.067 ms; fallbackCauses=SqlServerMaximumSatelliteOperationThreshold; satelliteOperations=10000.
- SQL Server customer-profile-scale-10000x10 optimized row: mean 6417.828 ms; fallbackCauses=SqlServerMaximumSatelliteOperationThreshold; satelliteOperations=100000.
- Oracle customer-profile-scale-10000x10 optimized row: mean 10689.765 ms; fallbackCauses=OracleMaximumSatelliteOperationThreshold; satelliteOperations=100000.
- PostgreSQL customer-profile-scale-10x1 optimized row: mean 14.595 ms; selectedStrategy=PostgresDataVaultSaveStrategy; smallBatchBoundary=direct-or-unnest; stagedProviderBulkPhase=Declined; stagedProviderBulkCaveat=UnsupportedShape; stagedProviderBulkOperations=10.
- PostgreSQL customer-profile-scale-100x1 and 10x10 optimized rows: means 26.355 ms and 22.236 ms; selectedStrategy=PostgresDataVaultSaveStrategy; stagedProviderBulkPhase=NativeBulkApplication; stagedProviderBulkOperations=100.
- MySQL customer-profile-scale-10x1 optimized row: mean 28.798 ms; fallbackCauses=MySqlMinimumOperationThreshold|MySqlMinimumOperationThreshold; stagedProviderBulkPhase=Declined; stagedProviderBulkCaveat=UnsupportedShape; stagedProviderBulkOperations=10.
- MySQL customer-profile-scale-100x1 and 10x10 optimized rows: means 62.636 ms and 43.905 ms; selectedStrategy=MySqlStagedDataVaultSaveStrategy; stagedProviderBulkPhase=NativeBulkApplication; stagedProviderBulkOperations=100.