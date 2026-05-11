[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff: the persisted contract is bounded, has no unresolved Open Questions, and direct repository evidence confirms the required read-service APIs, provider matrix, benchmark host, and deterministic skip posture.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEJ0NE80R7CNS982S3PKVR/description.md lines 7-14 records PO Handoff `ready_for_po_critic`, provider matrix SQLite/MySQL/Postgres/SQL Server/Oracle, read-baseline-only scope, and named latest/PIT/bridge read paths.
- .gicket/tickets/06F0MEJ0NE80R7CNS982S3PKVR/description.md lines 30-42 defines AC/DoD for scenario rows, provider measured-or-skipped rows, SQLite local baseline, Release build, smoke run, skip detection, README/docs, and no provider-specific read optimization.
- .gicket/tickets/06F0MEJ0NE80R7CNS982S3PKVR/description.md lines 51-52 records `## Open Questions` as `- none`.
- git log shows current branch `ticket/06F0MEJ0NE80R7CNS982S3PKVR-task-benchmark-latest-pit-and-bridge-reads-acros` at cd6b3524, based after develop integrations for 06F0MEH660Y5QTNR5P8JPS2QXC and 06F0MEHKYTBJEJH2DVZ2CFH9Z0.
- src/DCoding.Data.DVault/IDataVaultReadService.cs exposes `ReadLatestSatelliteRowsAsync(...)` and `ReadPitRowsAsync(...)`; src/DCoding.Data.DVault/DataVaultReadServicePitExtensions.cs exposes typed `ReadPitAsync(...)`.
- src/DCoding.Data.DVault/DataVaultBridgeReadRequest.cs, DataVaultBridgeReadRecord.cs, and DataVaultReadServiceBridgeExtensions.cs provide the provider-neutral bridge read request/record and `ReadBridgeRowsAsync(...)`/`ReadBridgeAsync(...)` surface.
- tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt includes public API entries for DataVaultPitAsOfReadRequest, DataVaultPitReadRecord, DataVaultBridgeReadRequest, DataVaultBridgeReadRecord, ReadPitRowsAsync, ReadBridgeRowsAsync, and ReadLatestSatelliteRowsAsync.
- DVault.slnx includes benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj, and that csproj references all five provider packages plus conditional external EF provider packages for SQL Server, Postgres, Oracle, and MySQL.
- benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkProviderAvailability.cs defines deterministic skip reasons for missing connection-string env vars, unavailable provider dependency, and connection failure; BenchmarkExternalProviderDefinitions names DVAULT_TEST_POSTGRES_CONNECTION_STRING, DVAULT_TEST_SQLSERVER_CONNECTION_STRING, DVAULT_TEST_MYSQL_CONNECTION_STRING, and DVAULT_TEST_ORACLE_CONNECTION_STRING.
- benchmarks/DCoding.Data.DVault.Benchmarks/README.md documents SQLite as the required local baseline, optional external-provider env vars, skipped rows with `executionStatus=skipped` and `skipReason`, provider filters `all/sqlite/postgres/sqlserver/mysql/oracle`, and summary artifacts benchmark-summary.md/csv/json.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Non-SQLite measured rows depend on local external provider configuration; the contract mitigates this by requiring deterministic skipped rows with exact missing configuration names.
- Benchmark timings can be distorted if fixture seeding is included in measured operations; the contract flags this risk and should be enforced during implementation review.

AC / test suggestions
- Add a deterministic no-secret smoke path such as Release run with `--provider sqlite --iterations 1 --warmup 0` that emits latest, PIT as-of, and bridge traversal rows.
- Cover provider discovery/skip output for all optional providers with exact env var names in either unit tests around availability detection or a deterministic smoke artifact.
- Include PIT cases with a visible as-of row and a missing/not-yet-visible as-of row, and bridge hierarchy cases that demonstrate traversal depth/maximum depth behavior where supported by the fixture.

Implementation watchouts
- Keep benchmarks in the existing benchmarks/DCoding.Data.DVault.Benchmarks host and avoid adding provider-specific read optimization behavior under this ticket.
- Use the public read-service surfaces directly: latest/as-of satellite through IDataVaultReadService, PIT through DataVaultPitAsOfReadRequest/DataVaultPitReadRecord, and bridge through DataVaultBridgeReadRequest/DataVaultBridgeReadRecord.
- Separate repeatable fixture setup from measured read operations so baseline numbers describe read behavior rather than seeding/setup cost.
- Ensure summary rows label scenario, provider, baseline/strategy family, measured vs skipped status, skip reason/configuration, and timing values consistently with the existing benchmark artifact shape.

Non-blocking notes
- none

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment