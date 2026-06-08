# SQL Server Save Threshold Diagnostics

Ticket: `06F9XD2M71D1XFT7FJX62KD8HM`

## Evidence Bundle

- Before source: `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-20260607`
- Before copy: `artifacts/benchmarks/06F9XD2M71D1XFT7FJX62KD8HM-sqlserver-save-threshold-diagnostics/before`
- After run: `artifacts/benchmarks/06F9XD2M71D1XFT7FJX62KD8HM-sqlserver-save-threshold-diagnostics/after`
- Shared inputs: scale matrix, 5 measured iterations, 1 warmup iteration, ProviderDefault load-timestamp storage.
- After isolation: SQL Server provider filter with PostgreSQL, MySQL, and Oracle connection string variables cleared.

The before set is the completed v0.32.0 scale baseline copied into this ticket-labeled bundle. The after set was produced from the ticket branch with the current SQL Server diagnostics wording.

## Threshold Decision

The SQL Server save gates stay unchanged at 50 minimum operations and 500 maximum satellite operations.

The after run preserves one provider-native SQL Server scale lane at 100 satellite operations:

| Scenario | Satellite operations | Selected strategy | Fallback causes | After mean ms |
| --- | ---: | --- | --- | ---: |
| customer-profile-scale-10x1 | 10 | `<none>` | SqlServerMinimumOperationThreshold | 17.989 |
| customer-profile-scale-100x1 | 100 | SqlServerDataVaultSaveStrategy | none | 60.101 |
| customer-profile-scale-10x10 | 100 | SqlServerDataVaultSaveStrategy | none | 23.860 |
| customer-profile-scale-1000x1 | 1000 | `<none>` | SqlServerMaximumSatelliteOperationThreshold | 146.983 |
| customer-profile-scale-100x10 | 1000 | `<none>` | SqlServerMaximumSatelliteOperationThreshold | 75.376 |
| customer-profile-scale-1000x10 | 10000 | `<none>` | SqlServerMaximumSatelliteOperationThreshold | 537.142 |
| customer-profile-scale-10000x10 | 100000 | `<none>` | SqlServerMaximumSatelliteOperationThreshold | 6082.673 |

The after optimized-lane fallback rows now report `executionPath=DVault provider-neutral fallback path` with `selectedStrategy=<none>` and the SQL Server candidate retained in `candidateStrategies=SqlServerDataVaultSaveStrategy`. They no longer claim the SQL Server staged native bulk path executed when diagnostics show `saveStrategyStatus=ProviderNeutralFallback`.

## Before/After Notes

The v0.32.0 before rows already showed the same gate outcomes, but fallback rows still used planned SQL Server staged native wording. The after rows keep the measured 50/500 threshold posture and correct the completed-row wording to match the observed diagnostics.
