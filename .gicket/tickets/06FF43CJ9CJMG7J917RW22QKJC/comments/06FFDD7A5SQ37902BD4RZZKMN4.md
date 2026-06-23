[gicket-bot] PO-critic review contract

Summary
- Delivery contract is ready for developer handoff: scope is bounded to MySQL PIT full-rebuild feasibility, repository evidence matches the stated baseline, and persisted Open Questions remain empty.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- '.gicket/tickets/06FF43CJ9CJMG7J917RW22QKJC/description.md' is the persisted delivery contract and its '## Open Questions' section says 'none'; the acceptance criteria explicitly require provider-name coverage, PIT-shape decisions, transaction/rollback caveats, seam choice, and an implementation-or-defer recommendation for blocked ticket '06FF43F283QFQ56290AVJ3AXSM'.
- 'git log --oneline -- .gicket/tickets/06FF43CJ9CJMG7J917RW22QKJC' shows commit 'c0fcf9fecb' as the PO->PO-critic handoff, and 'git show --stat c0fcf9fecbaf' shows that commit updated '.gicket/tickets/06FF43CJ9CJMG7J917RW22QKJC/description.md' and 'ticket.json'.
- 'src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:15-30' registers MySQL save plus latest-satellite/PIT/bridge read strategies only; a direct 'rg' over 'src/DCoding.Data.DVault.MySql' found no MySQL PIT-maintenance service or strategy registration.
- 'src/DCoding.Data.DVault/DefaultDataVaultPitMaintenanceService.cs:17-22,83-100' shows the generic PIT maintenance seam is 'IEnumerable<IDataVaultProviderPitMaintenanceStrategy>'.
- 'src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs:36-53' special-cases only 'PostgresDataVaultPitMaintenanceStrategy' in 'TryEvaluateKnownStrategy'.
- 'src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:15-28' registers 'IDataVaultProviderPitMaintenanceStrategy', while 'src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:15-27' replaces 'IDataVaultPitMaintenanceService'; 'src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs:77-115,157-209' shows SQL Server's narrower ordinary-hub-only gate and savepoint-based rollback-clean path.
- 'docs/architecture/dvault-v1-pit-bridge-boundary.md:27-32' and 'docs/performance-profiles.md:69-75' document the current asymmetric PIT-maintenance baseline: PostgreSQL strategy seam, SQL Server service replacement, rollback/fallback caveats, and no bridge-maintenance push-down claim.
- 'artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-<redacted>/mysql-live/benchmark-summary.md:31-37' records completed MySQL save plus latest-satellite/PIT/bridge read rows selecting 'MySqlDataVaultSaveStrategy' or 'MySqlDataVaultReadStrategy', and every row names provider 'MySql.EntityFrameworkCore'.
- 'tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:22,32' and 'benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj:19' reference 'MySql.EntityFrameworkCore', while 'tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs:203-232' shows Pomelo coverage is provider-profile selection in tests rather than a live integration lane.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Non-blocking: the final evaluation should say whether already-unsupported declarations outside the shared PIT maintenance v1 contract stay out of scope for the MySQL feasibility matrix rather than being treated as MySQL-specific rejections.
- Non-blocking: call out caller-transaction/no-savepoint behavior explicitly in the ticket-visible outcome so SQL Server-style rollback-clean expectations are not assumed by downstream readers.

Risky assumptions
- Assuming Pomelo can share the same maintenance claim as 'MySql.EntityFrameworkCore' without live Pomelo execution evidence would exceed current repository proof.
- Assuming MySQL can preserve pre-rebuild PIT rows on fault or cancellation like SQL Server without verified transaction/savepoint behavior would be risky.
- Assuming the existing provider-strategy seam is sufficient without accounting for 'DataVaultProviderPitMaintenanceStrategyGateEvaluator' currently recognizing only Postgres would understate required boundary work.

AC / test suggestions
- Have the evaluation cite the exact repository surfaces it relies on: 'DVaultMySqlServiceCollectionExtensions', 'DefaultDataVaultPitMaintenanceService', 'DataVaultProviderPitMaintenanceStrategyGateEvaluator', 'DVaultPostgresServiceCollectionExtensions', 'DVaultSqlServerServiceCollectionExtensions', and the MySQL closure-bundle benchmark summary.
- Present the outcome as a matrix by PIT shape ('ordinary hub-parent', 'shared-driving-key multi-active hub-parent', 'link-parent non-multi-active') and by provider surface ('MySql.EntityFrameworkCore', Pomelo) with columns for evidence type, accepted/deferred/rejected, fallback boundary, and implementation/defer recommendation.
- Require an explicit statement that existing MySQL PIT read timing is read-side evidence only and does not prove maintenance push-down.

Implementation watchouts
- Do not treat MySQL PIT or bridge read timing rows as maintenance evidence; they prove 'MySqlDataVaultReadStrategy', not 'IDataVaultPitMaintenanceService.RebuildAsync(...)' push-down.
- If a MySQL lane uses the provider-strategy seam, the known-strategy gate/diagnostics path currently needs explicit MySQL handling because 'TryEvaluateKnownStrategy' only recognizes Postgres.
- If rollback-clean behavior under caller transactions cannot be proved, narrow the accepted MySQL lane or defer it instead of mirroring SQL Server's guarantee.

Non-blocking notes
- The blocks relation file '.gicket/relations/JC/SM/06FF43CJ9CJMG7J917RW22QKJC--06FF43F283QFQ56290AVJ3AXSM--blocks.json' plus '.gicket/tickets/06FF43F283QFQ56290AVJ3AXSM/description.md' confirm this evaluation is meant to feed the provider PIT decision-matrix ticket.

Split recommendations
- No additional pre-development split is needed now; if the evaluation later recommends implementation, create a separate bounded MySQL PIT full-rebuild implementation ticket and keep benchmark-backed maintenance timing as a follow-up.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment