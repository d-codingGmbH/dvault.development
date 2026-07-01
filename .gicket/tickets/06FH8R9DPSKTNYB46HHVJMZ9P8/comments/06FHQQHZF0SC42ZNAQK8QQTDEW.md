[gicket-bot] PO-critic review contract

Summary
- Persisted contract, ticket comments, repository docs, and direct source evidence align on a closed provider-optimization tracking story with no unresolved PO questions; the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `docs/plans/provider-optimization-gap-matrix.md` directly states PostgreSQL, SQL Server, MySQL, Oracle, and DB2 P0-P3 latest-satellite, save, PIT, and bridge rows are closed by the `2026-06-23` closure bundle, and that remaining items are fallback/deferred-maintenance boundaries, including unimplemented DB2 ordinary hub-parent PIT full rebuild.
- `docs/plans/provider-optimization-evidence-matrix.md` and `docs/performance-profiles.md` both name `artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-<redacted>/` as the authoritative completed-timing source and explicitly keep `pit-as-of-read` / `bridge-traversal-read` separate from PIT maintenance timing.
- `docs/architecture/dvault-v1-pit-bridge-boundary.md` and `docs/plans/db2-pit-maintenance-full-rebuild-feasibility.md` both state DB2 currently remains provider-neutral for PIT maintenance and that the only accepted future expansion is a separate `IBM.EntityFrameworkCore` ordinary hub-parent `RebuildAsync(...)` lane through `IDataVaultProviderPitMaintenanceStrategy`.
- `src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs` registers DB2 save/read/PIT-read/bridge-read strategies but no `IDataVaultProviderPitMaintenanceStrategy`; `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs` and `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs` do register MySQL/PostgreSQL PIT maintenance strategies, matching the contract boundary.
- `docs/releases/v0.46.0.md` and `artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-<redacted>/README.md` publish the completed PostgreSQL, SQL Server, MySQL, Oracle, and DB2 save/latest/PIT/bridge timings cited by the story.
- `git status --short --branch` returned `## HEAD (no branch)` and `git rev-parse HEAD` returned `214d1d9a73682ae1369e8a4feb3eff5f881f5866`, matching the provided scratch-source ref and showing no local working-tree changes in the review surface.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Developers will follow the authoritative Delivery Contract rather than the broader legacy draft/title wording; the contract is precise, but the short legacy framing still reads like fresh implementation discovery.

AC / test suggestions
- If this story later spawns implementation work, keep acceptance and test citations pinned to matrix rows by `scenario`, `provider`, `baseline`, and `posture` instead of copying timing prose into child tickets.
- Any future DB2 PIT-maintenance child should require source/test proof of rollback-clean delete-plus-insert behavior and must not reuse `pit-as-of-read` timing rows as maintenance evidence.

Implementation watchouts
- Do not reopen closed save/latest/PIT/bridge rows or require new benchmark reruns under this ticket; the contract and repository docs treat those rows as already closed baseline evidence.
- Do not treat MySQL, Oracle, or DB2 PIT read timings as write-side PIT maintenance proof; the repository docs keep read timing separate from maintenance timing.
- DB2 PIT maintenance remains provider-neutral on the current baseline because `DVaultDb2ServiceCollectionExtensions.cs` has no PIT maintenance strategy registration.

Non-blocking notes
- This ticket is a planning/closure-tracking parent, not a fresh implementation-discovery story, even though the legacy draft wording is broader.
- The current review surface is detached at `214d1d9a73682ae1369e8a4feb3eff5f881f5866` with no local file changes, which is acceptable for this pre-development contract review.

Split recommendations
- Do not split save, read, or documentation work further; the contract already points to children `06FH8RATZGZRVAJVC4ERV0ACYW`, `06FH8RC9F0QEWF356WF7YYNNGM`, `06FH8RDS25081N5S181C7TQGTG`, and `06FH8REKX113JRZQ42HEB1NVZ8`.
- If the team later pursues DB2 PIT-maintenance parity, create one separate child limited to `IBM.EntityFrameworkCore` ordinary hub-parent `RebuildAsync(...)` push-down through `IDataVaultProviderPitMaintenanceStrategy`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment