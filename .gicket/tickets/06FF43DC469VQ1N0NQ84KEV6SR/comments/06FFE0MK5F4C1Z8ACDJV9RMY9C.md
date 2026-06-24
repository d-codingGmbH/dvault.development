## Developer investigation: Oracle PIT full-rebuild push-down

### Decision
Recommendation: defer Oracle PIT full-rebuild push-down for now. Keep Oracle PIT maintenance on the provider-neutral implementation until an Oracle-specific rebuild candidate proves both SQL parity and rollback-clean failure/cancellation behavior.

The only future candidate that looks bounded enough to reopen is a full-rebuild-only, ordinary hub-parent Oracle path. It should not cover parent maintenance, multi-active PITs, or link-parent PITs until those shapes have separate Oracle rebuild SQL and live-provider proof.

### Evidence inspected
- `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs` registers Oracle provider capability, provider behavior, save strategy, latest-satellite read strategy, PIT read strategy, and bridge read strategy. It does not register `IDataVaultProviderPitMaintenanceStrategy` and does not replace `IDataVaultPitMaintenanceService`.
- `git grep -n "IDataVaultPitMaintenanceService|IDataVaultProviderPitMaintenanceStrategy|PitMaintenance" -- src/DCoding.Data.DVault.Oracle` returned no Oracle PIT maintenance matches.
- `docs/architecture/dvault-v1-pit-bridge-boundary.md` defines the current accepted maintenance baseline as asymmetric: PostgreSQL contributes a provider PIT maintenance strategy; SQL Server replaces the PIT maintenance service for a narrower ordinary hub-parent full-rebuild gate; all provider paths remain explicit caller-invoked fallbacks.
- `src/DCoding.Data.DVault.Postgres/PostgresDataVaultPitMaintenanceStrategy.cs` implements a broad strategy path with provider-name, clean-context, complete-shape-evidence, hub/link-parent, distinct-satellite, and link/non-multi-active gates. Its SQL generation uses set-based CTE/UNION source construction and lateral snapshot lookup, including tuple-aware handling for shared-driving-key multi-active hub PITs.
- `src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs` intentionally owns the full service seam, not just the shared strategy seam, so it can reject unsupported shapes and require rollback-clean behavior through a local transaction or caller-transaction savepoint. It falls back for `MaintainParentsAsync(...)`, dirty contexts, provider mismatch, link-parent PITs, multi-active PITs, and current transactions without savepoints.
- `tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs` has live SQL Server PIT rebuild checks for rollback on insert-select failure and cancellation before commit. The Oracle live smoke coverage observed here covers save-path rollback, not PIT rebuild rollback.
- `src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs`, `src/DCoding.Data.DVault.Oracle/OracleDataVaultReadStrategy.cs`, and `tests/DCoding.Data.DVault.Tests/Unit/OracleProviderOptimizationTests.cs` show Oracle-specific SQL conventions for read/save paths: quoted identifiers, `:pN` bind placeholders, `ROW_NUMBER()` selection, `INSERT ALL`, `FROM DUAL`, array binding reflection, and no `AS` keyword for table aliases in read SQL. Those are useful implementation inputs but are not PIT rebuild proof.

### Shape assessment
| PIT request shape | Oracle provider push-down decision | Reason |
| --- | --- | --- |
| Ordinary hub-parent full rebuild | Defer; plausible future candidate only | SQL Server proves this shape can be safely narrowed, but Oracle lacks PIT-specific delete/insert SQL, diagnostics selection, and rollback-clean live-provider proof. A future candidate should require Oracle provider-name match, clean context, complete maintenance projection evidence, ordinary hub parent, at least one distinct non-multi-active satellite, and either a local transaction or caller savepoint/equivalent rollback boundary. |
| Shared-driving-key multi-active hub-parent full rebuild | Unsupported; keep provider-neutral fallback | PostgreSQL handles this through tuple-aware source generation and lateral snapshot joins. No Oracle rebuild SQL currently proves equivalent tuple identity, snapshot selection, ordering, and null/late-arrival semantics. This should not be included in the first Oracle candidate. |
| Link-parent full rebuild | Unsupported; keep provider-neutral fallback | PostgreSQL supports non-multi-active link-parent PITs through the broad provider strategy, while SQL Server deliberately rejects link-parent PITs. Oracle has no link-parent rebuild SQL or shape test evidence, so enabling it would expand the proof surface beyond the bounded candidate. |
| Parent maintenance / `MaintainParentsAsync(...)` | Unsupported; keep provider-neutral fallback | The ticket scope and current provider baseline are full-rebuild focused. SQL Server already falls back for parent maintenance; Oracle should do the same unless a separate parent-scoped delete/insert plan and transaction proof are designed. |

### SQL and provider risks
- Oracle PIT rebuild SQL cannot be a direct copy of the PostgreSQL implementation. The existing Oracle code uses Oracle-specific aliasing, bind placeholders, `FROM DUAL`, `INSERT ALL`, array binding, and analytic `ROW_NUMBER()` patterns; a rebuild candidate would need its own set-based `INSERT INTO ... SELECT` plan and snapshot lookup syntax.
- The highest safety risk is partial refresh: deleting PIT rows and then faulting or observing cancellation before the insert/commit must leave the old PIT rows intact. SQL Server solves this with local transaction rollback or savepoint rollback. Oracle needs the same proof for local transactions and caller transactions before registration.
- Oracle save-path rollback evidence does not prove Oracle PIT rebuild rollback. Save uses a different command sequence and failure surface than a full rebuild that deletes then repopulates generated PIT rows.
- Oracle read optimization and PIT-read benchmark evidence do not prove maintenance safety. Reads consume already-maintained PIT rows; they do not validate rebuild SQL, tracked-row detachment, parent counts, rollback, or diagnostics fallback behavior.
- A first Oracle candidate must preserve explicit caller-owned maintenance. It must not introduce read-time refresh, `SaveChanges` interception, startup rebuilds, background scheduling, or bridge/parent maintenance push-down.

### Required guardrails before implementation
- Register no Oracle PIT maintenance strategy/service until ordinary hub-parent full rebuild SQL is unit-tested for generated command shape and integration-tested against the live Oracle provider.
- Gate selection on `KnownProviderNames.Oracle`, clean `DbContext`, complete maintenance shape evidence, ordinary hub parent, non-empty distinct satellites, no multi-active satellites, and full-rebuild scope only.
- Use SQL Server-style service ownership if the shared provider-strategy seam cannot guarantee rollback-clean execution inside caller transactions. At minimum, reject caller transactions without savepoint or equivalent rollback capability.
- Add diagnostics/fallback reporting matching the existing maintenance strategy status model so unsupported Oracle shapes remain observable provider-neutral fallbacks.
- Add failure and cancellation tests that preserve pre-rebuild PIT rows, plus parity tests against provider-neutral output for the accepted ordinary hub-parent shape.

### Final recommendation
Do not implement or register Oracle PIT full-rebuild push-down on this ticket. Defer behind a future implementation ticket scoped to one candidate: Oracle ordinary hub-parent, full-rebuild-only PIT maintenance with SQL Server-level rollback-clean guarantees. All Oracle multi-active hub-parent, link-parent, and parent-maintenance requests should remain provider-neutral until separately proven.