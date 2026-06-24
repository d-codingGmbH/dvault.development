## Developer Evaluation Complete

Added `docs/plans/db2-pit-maintenance-full-rebuild-feasibility.md`.

Decision: DB2 PIT full-rebuild push-down is feasible only as one follow-up implementation slice limited to clean ordinary hub-parent `RebuildAsync(...)` full rebuilds on `IBM.EntityFrameworkCore` through `IDataVaultProviderPitMaintenanceStrategy`. The first slice should not replace `IDataVaultPitMaintenanceService`.

Current runtime baseline remains unchanged: `AddDVaultDb2()` registers DB2 save plus latest-satellite/PIT/bridge read strategies, but no DB2 PIT maintenance strategy or service replacement exists today. DB2 PIT maintenance remains provider-neutral until a follow-up implementation lands.

Shape classification:
- Accepted follow-up candidate: ordinary hub-parent full rebuild.
- Deferred: shared-driving-key multi-active hub-parent and link-parent non-multi-active full rebuilds.
- Fallback-only or rejected for the initial lane: `MaintainParentsAsync(...)`, link-parent multi-active PITs, incompatible driving-key-family PITs, provider mismatch, dirty context, incomplete maintenance-shape evidence, and caller transactions without proven IBM savepoint rollback.

Transaction gate: strategy-owned local transactions must preserve pre-rebuild PIT rows on fault or cancellation; ambient caller transactions must fall back unless IBM-provider savepoint behavior is source-, test-, and live-provider-proven.

Verification: `bash tools/check-format.sh` passed.

Bounded next step: create one implementation ticket limited to `IBM.EntityFrameworkCore` ordinary hub-parent full-rebuild push-down through `IDataVaultProviderPitMaintenanceStrategy`, with DB2 diagnostics gates and rollback-clean local transaction proof.