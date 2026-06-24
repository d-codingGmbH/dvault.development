# DB2 PIT Maintenance Full-Rebuild Feasibility

Status: ticket-bound evaluation note
Ticket: `06FF43E0JCE7BSBFBWB49HGB4G`
Decision: accept one DB2 ordinary hub-parent full-rebuild implementation slice through the provider strategy seam

## Purpose

Evaluate whether DB2 should receive a bounded `IDataVaultProviderPitMaintenanceStrategy` for
`IDataVaultPitMaintenanceService.RebuildAsync(...)` full rebuilds. This note does not implement the
strategy, does not claim DB2 PIT maintenance timing, and does not widen DB2 read, save, binary
storage, bridge maintenance, or automatic maintenance behavior.

## Current DB2 Baseline

`AddDVaultDb2()` registers the provider capability profile plus DB2 save, latest-satellite read, PIT
read, and bridge read strategies for the IBM EF Core provider lane. It does not register an
`IDataVaultProviderPitMaintenanceStrategy`, and it does not replace `IDataVaultPitMaintenanceService`.
DB2 callers therefore use `DefaultDataVaultPitMaintenanceService` provider-neutral PIT maintenance
today.

The checked-in DB2 smoke lane uses `IBM.EntityFrameworkCore` and
`DVAULT_TEST_DB2_CONNECTION_STRING`. It proves representative DB2 save behavior plus optimized
latest-satellite, PIT, and bridge reads when configured. The PIT read smoke test inserts
`PitCustomerContact` rows directly before reading them, so it proves optimized reads over
already-maintained PIT rows. It is not write-side PIT maintenance push-down proof.

The 2026-06-23 provider optimization closure bundle and the earlier DB2 hotspot/host-to-Podman
bundles are evidence for clean-context DB2 save and maintained read-model read lanes. They remain
outside DB2 PIT maintenance timing because no DB2 maintenance strategy or maintenance benchmark row
exists.

## Architecture Decision

The feasible DB2 architecture is a provider strategy added through the existing
`IDataVaultProviderPitMaintenanceStrategy` seam, following the PostgreSQL shape more closely than the
SQL Server shape. The default PIT maintenance service already attempts registered provider strategies
for `RebuildAsync(...)` and keeps `MaintainParentsAsync(...)` on the provider-neutral pipeline. That
matches the DB2 evaluation scope and avoids replacing the whole maintenance service.

Do not use a SQL Server-style `IDataVaultPitMaintenanceService` replacement for DB2 in the first
slice. SQL Server has a narrower ordinary hub-parent gate and explicit savepoint fallback behavior in
its replacement service. DB2 has no live source or integration proof for that replacement model, and
using the provider-strategy seam keeps fallback behavior aligned with the existing PostgreSQL and
future-provider path.

Recommendation: implement one follow-up slice limited to clean ordinary hub-parent full rebuilds on
`IBM.EntityFrameworkCore`, with provider-neutral fallback for every unproven or unsafe case. Keep
maintenance timing, multi-active hub-parent expansion, link-parent expansion, and binary-storage
compatibility remediation as separate later work.

## Candidate Shape Classification

| Candidate | Decision | DB2 evaluation result |
| --- | --- | --- |
| Ordinary hub-parent full rebuild | Accepted implementation candidate | Feasible as the first DB2 strategy lane if it proves provider-name gating, clean-context gating, complete projection evidence, SQL parity, and rollback-clean delete-plus-insert behavior on `IBM.EntityFrameworkCore`. Current branch remains provider-neutral fallback until implemented. |
| Shared-driving-key multi-active hub-parent full rebuild | Deferred | PostgreSQL proves this maintenance shape today, but DB2 has only maintained-PIT read evidence. Tuple identity, tuple source generation, and DB2 snapshot lookup parity need a separate follow-up after the ordinary lane. |
| Link-parent non-multi-active full rebuild | Deferred | PostgreSQL proves this maintenance shape today, but DB2 has only maintained-PIT read evidence. Link-parent SQL parity and parent hash-key semantics should not ride in the first DB2 implementation slice. |
| `MaintainParentsAsync(...)` | Fallback-only | The provider strategy seam is full-rebuild-only today. Parent-targeted replacement, empty-key no-op behavior, and late-arriving correction must stay provider-neutral for DB2. |
| Link-parent multi-active PITs | Fallback-only | This shape is outside the current PIT maintenance boundary and remains unsupported for DB2. |
| Incompatible driving-key-family PITs | Fallback-only | The provider-neutral validator/read boundary rejects incompatible multi-active families; DB2 should not add provider-specific behavior here. |
| Provider mismatch | Fallback-only | A DB2 strategy must decline unless `DbContext.Database.ProviderName` is the IBM provider name and the registered DB2 capability profile is selected. |
| Dirty `DbContext` | Fallback-only | Pending tracked changes can diverge from persisted satellite history. DB2 must require a clean context before provider SQL deletes and reinserts PIT rows. |
| Incomplete maintenance-shape evidence | Fallback-only | Missing generated PIT or referenced satellite projection evidence must decline to provider-neutral maintenance before SQL generation. |
| Caller transaction already active | Fallback-only unless savepoints are proven | The initial DB2 lane may own a local transaction. If an ambient caller transaction exists, DB2 must prove IBM-provider savepoint behavior and roll back to a strategy-owned savepoint, or decline to provider-neutral maintenance. |

## Transaction And Rollback Gate

DB2 full rebuild push-down is only acceptable when delete-plus-insert execution is rollback-clean.

When the DB2 strategy owns the transaction, it should open the connection if needed, begin a local
transaction, count affected parent keys, delete the PIT rows, insert the rebuilt rows, detach stale PIT
rows from the EF change tracker, and commit only after all commands and cancellation checks succeed.
Faults and cancellations must roll back the local transaction and preserve the pre-rebuild PIT rows.

When a caller transaction is already active, the first DB2 implementation must not assume rollback
safety. It can use the provider path only if source and live integration evidence prove that the IBM EF
Core transaction supports a strategy-owned savepoint and that rollback to that savepoint preserves the
pre-rebuild rows after a fault or cancellation. Without that proof, the DB2 strategy must decline and
allow the provider-neutral maintenance path to handle the request.

This rollback requirement is a behavior gate, not a performance optimization. A DB2 implementation
that cannot prove rollback-clean behavior for the accepted lane should be deferred rather than merged
with a partial delete-plus-insert claim.

## SQL Shape Risks

The ordinary hub-parent SQL shape appears tractable for DB2 because the repository already has DB2
provider SQL using quoted identifiers, typed parameter casts, derived `VALUES` tables, and
`ROW_NUMBER()` window functions. A DB2 PIT rebuild can use the same provider-package boundary for SQL
generation and keep SQL text out of diagnostics.

The risky parity points are:

- Set-based row generation must produce one PIT source row for each distinct parent hash key and each
  satellite load timestamp that participates in the PIT, with deterministic ordering matching the
  provider-neutral service.
- Snapshot lookup must pick, per PIT source row and satellite, the latest satellite load timestamp at
  or before the PIT source timestamp, and return null when no satellite snapshot exists yet.
- Parent-key counting must count distinct parents across all participating ordinary satellites before
  the delete-plus-insert rebuild reports `ParentHashKeyCount`.
- Identifier quoting and name folding must preserve generated table and column identities under the
  DB2 provider.
- Load timestamp value handling must stay compatible with the repository's DB2 provider-default
  storage lane and must not widen into the separate binary hash-key caveat.
- Tuple handling for shared-driving-key multi-active PITs is intentionally deferred; DB2 should not
  claim tuple parity until DB2-specific tuple source, tuple identity, snapshot join, and tests exist.

Diagnostics should reuse the existing fallback vocabulary where possible:
`ProviderNameMismatch`, `UnknownOrUnregisteredProviderName`, `DirtyDbContext`,
`IncompleteMaintenanceShapeEvidence`, and `UnsupportedPitShape`. The DB2 slice also needs an explicit
ambient-transaction rollback fallback equivalent to the SQL Server no-savepoint guard so unsupported
caller transactions do not appear as generic strategy declines.

## Evidence Used

- `src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs` registers DB2 behavior plus
  save/read/PIT-read/bridge-read strategies, and no PIT maintenance strategy or service replacement.
- `src/DCoding.Data.DVault/DefaultDataVaultPitMaintenanceService.cs` dispatches `RebuildAsync(...)`
  through registered provider PIT maintenance strategies before provider-neutral fallback, while
  `MaintainParentsAsync(...)` remains provider-neutral.
- `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs` and
  `PostgresDataVaultPitMaintenanceStrategy` show the preferred strategy-seam pattern for provider
  full rebuilds.
- `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs` and
  `SqlServerDataVaultPitMaintenanceService` show the narrower service-replacement comparison and the
  rollback-clean savepoint guard that DB2 must not assume.
- `src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs` currently knows
  PostgreSQL PIT maintenance gates only; DB2 implementation must add its own known gate before
  claiming diagnostics.
- `tests/DCoding.Data.DVault.Tests/Unit/Db2ProviderCapabilityTests.cs` proves DB2 save/read strategy
  registration and the IBM provider-name boundary.
- `tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs` proves the IBM opt-in DB2
  lane for save plus latest-satellite/PIT/bridge reads, with PIT rows inserted before reads.
- `docs/architecture/dvault-v1-pit-bridge-boundary.md`,
  `docs/performance-profiles.md`, and `docs/plans/provider-optimization-gap-matrix.md` keep the
  current DB2 save/read evidence boundary separate from write-side PIT maintenance push-down.

## Completion Recommendation

Create one follow-up implementation ticket limited to `IBM.EntityFrameworkCore` ordinary hub-parent
`RebuildAsync(...)` full rebuild push-down through `IDataVaultProviderPitMaintenanceStrategy`.

That follow-up should include source, unit, and opt-in DB2 integration proof for:

- `AddDVaultDb2()` registering the DB2 PIT maintenance strategy without replacing
  `IDataVaultPitMaintenanceService`.
- DB2 gate diagnostics for provider mismatch, unknown/unregistered provider, dirty context, incomplete
  projection evidence, unsupported shapes, and ambient transactions without proven savepoint rollback.
- Set-based ordinary hub-parent delete-plus-insert SQL parity with
  `DefaultDataVaultPitMaintenanceService.CreatePitProjection(...)` and provider-neutral rebuild
  semantics.
- Rollback-clean local transaction behavior on fault and cancellation, plus caller-transaction
  fallback unless IBM savepoint behavior is proven.

Do not create DB2 maintenance timing claims, multi-active support, link-parent support,
`MaintainParentsAsync(...)` push-down, bridge maintenance push-down, automatic maintenance, or binary
hash-key compatibility changes in that first implementation slice.
