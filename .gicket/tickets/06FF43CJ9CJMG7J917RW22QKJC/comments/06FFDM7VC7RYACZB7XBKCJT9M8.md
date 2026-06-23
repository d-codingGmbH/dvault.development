## Developer evaluation outcome: MySQL PIT full-rebuild push-down

Decision: implement a narrow follow-up, not a runtime behavior in this ticket.

Repository-proven facts:
- `AddDVaultMySql()` currently registers MySQL save plus latest-satellite, PIT, and bridge read strategy candidates and provider capability profile selection for `Pomelo.EntityFrameworkCore.MySql` and `MySql.EntityFrameworkCore`.
- The MySQL package does not register `IDataVaultProviderPitMaintenanceStrategy` and does not replace `IDataVaultPitMaintenanceService`.
- Live provider timing evidence covers MySQL save/read lanes through `MySql.EntityFrameworkCore`; Pomelo is capability/profile-covered but not live-maintenance-proven here.
- Existing MySQL PIT read timing proves reads over explicitly maintained PIT rows. It is not write-side PIT maintenance push-down proof.

Shape decisions:
- Accepted: clean ordinary hub-parent PIT full rebuilds as the first MySQL implementation slice, initially on the repository-proven `MySql.EntityFrameworkCore` lane.
- Deferred: shared-driving-key multi-active hub-parent PIT full rebuilds and link-parent non-multi-active PIT full rebuilds. PostgreSQL proves those maintenance shapes today; MySQL has read evidence only.
- Rejected for the initial lane: `MaintainParentsAsync(...)`, link-parent multi-active PITs, incompatible driving-key-family PITs, bridge maintenance, automatic maintenance, read-time refresh, dirty contexts, provider mismatches, and caller transactions without verified rollback-clean savepoint support.

Implementation recommendation:
- Use the existing `IDataVaultProviderPitMaintenanceStrategy` seam rather than a SQL Server-style `IDataVaultPitMaintenanceService` replacement. The strategy seam already scopes to `RebuildAsync(...)` full rebuilds and preserves provider-neutral `MaintainParentsAsync(...)` fallback.
- A future MySQL strategy must add MySQL-aware gate and diagnostic evaluation, require complete maintenance-shape evidence and a clean context, and prove rollback-clean delete-plus-insert behavior through a strategy-owned transaction or an explicitly verified savepoint when an ambient transaction exists.
- If rollback/savepoint behavior cannot be proven, the MySQL strategy must fall back to provider-neutral maintenance rather than claiming SQL Server-style pre-rebuild row preservation.

Follow-up materialized:
- Created ticket `06FFDG522514HX2J17GT9VE77W` (`Task: Implement MySQL ordinary hub-parent PIT full-rebuild maintenance strategy`) for the accepted implementation slice.

Downstream matrix instruction:
- The blocked matrix task `06FF43F283QFQ56290AVJ3AXSM` can consume this as: accept ordinary hub-parent MySQL full rebuild as a future implementation lane; defer Pomelo live validation, multi-active hub-parent maintenance, link-parent maintenance, and benchmark-backed maintenance timing; do not treat current MySQL PIT read timing as maintenance evidence.