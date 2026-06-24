<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Delivery contract refined and ready for PO-critic review.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Provider PIT maintenance evidence is a distinct contract from PIT and bridge read evidence: pit-full-rebuild-maintenance is the only maintenance timing row family, while pit-as-of-read and bridge-traversal-read prove reads over already-maintained rows only.
- The accepted provider-maintenance baseline is intentionally asymmetric. PostgreSQL supports clean full rebuilds for ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active PITs when no caller transaction is active. SQL Server supports clean ordinary hub-parent full rebuilds only. MySQL supports clean ordinary hub-parent full rebuilds only on MySql.EntityFrameworkCore. Oracle remains deferred. DB2 remains provider-neutral until a future ordinary hub-parent lane is implemented.
- Benchmark-backed maintenance claims must carry the preserved benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json triplet from the same run plus run context, maintenanceScope=FullRebuild, selected or planned strategy, fallback posture, and bounded fallback causes. Source and test backed availability alone is not a timing claim.

### Scope In
- Define the evidence contract for provider-specific PIT full-rebuild maintenance rows, artifact requirements, and claim boundaries.
- Ratify the current provider-specific maintenance shape support and provider-neutral fallback boundaries for PostgreSQL, SQL Server, MySQL, Oracle, and DB2.
- Require the bounded diagnostics and fallback vocabularies used to explain selected strategy, declined strategy, or provider-neutral fallback for PIT maintenance claims.
- Keep the canonical documentation surfaces aligned: Provider Optimization Evidence Matrix, Performance Evidence And Benchmark Artifact Contract, Performance Profiles, DVault V1 PIT And Bridge Boundary, and the v0.47 release notes.

### Scope Out
- Bridge maintenance push-down, bridge maintenance timing, or using bridge-traversal-read rows as maintenance evidence.
- Automatic PIT or bridge refresh, background scheduling, EF SaveChanges orchestration, or read-time maintenance.
- Oracle PIT maintenance implementation and DB2 PIT maintenance implementation beyond the current evidence contract.
- New benchmark runs, provider code changes, or widening provider maintenance shape support beyond the repository-proven v0.47 baseline.

## Acceptance Criteria
- The contract states that pit-full-rebuild-maintenance is the only PIT maintenance timing scenario and explicitly rejects pit-as-of-read and bridge-traversal-read as maintenance timing proof.
- The contract records the supported provider-maintenance lanes exactly as the repository proves them: PostgreSQL clean ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active full rebuilds; SQL Server clean ordinary hub-parent full rebuilds only; MySQL clean ordinary hub-parent full rebuilds only on MySql.EntityFrameworkCore; Oracle deferred; DB2 future-only and provider-neutral today.
- The contract requires maintenance evidence to cite the artifact triplet, run context, provider, baseline or comparator, maintenanceScope=FullRebuild, selected or planned maintenance strategy, execution path or fallback posture, and bounded fallback causes.
- The contract points to the bounded fallback vocabularies already proved in code and tests: DataVaultPitMaintenanceStrategyFallbackCauseKind for the shared strategy seam and SqlServerPitMaintenanceFallbackCauseKind for the SQL Server service-replacement lane.
- The contract keeps bridge maintenance, automatic scheduling, parent-maintenance push-down expansion, and unsupported PIT shapes outside this ticket's accepted claim surface.

## Definition of Done
- The canonical evidence and documentation surfaces all describe the same maintenance boundary without promoting read rows or source/test-backed availability into benchmark-backed maintenance wins.
- BenchmarkScenarioExecutionTests and the evidence matrix remain aligned on provider-evidence manifest mapping for PIT maintenance rows, including workloadShape=pit-full-rebuild-maintenance and readShape=null.
- Service registration and capability tests continue to prove the accepted provider boundary: PostgreSQL and MySQL register IDataVaultProviderPitMaintenanceStrategy, SQL Server replaces IDataVaultPitMaintenanceService, and Oracle and DB2 do not register PIT maintenance today.
- The parent story relation state remains consistent with delivery: the existing child tickets stay done and no active blocks relations remain on the parent.

## Implementation Notes
- The canonical row and manifest contract lives in docs/plans/provider-optimization-evidence-matrix.md and docs/plans/performance-evidence-benchmark-artifact-contract.md. These documents already require the deterministic execution-detail tokens used for maintenance claims, including executionPath, selectedStrategy, fallbackCauses, maintenanceScope, and pitShapeBoundary.
- Repository code matches the documented asymmetry: AddDVaultPostgres registers PostgresDataVaultPitMaintenanceStrategy, AddDVaultMySql registers MySqlDataVaultPitMaintenanceStrategy, AddDVaultSqlServer replaces IDataVaultPitMaintenanceService with SqlServerDataVaultPitMaintenanceService, and AddDVaultOracle/AddDVaultDb2 do not add PIT maintenance registration.
- Repository tests already prove the important gates and decline paths: PostgresProviderCapabilityTests, MySqlProviderCapabilityTests, PostgresPitMaintenanceServiceTests, MySqlPitMaintenanceServiceTests, and SqlServerDataVaultPitMaintenanceServiceTests cover provider mismatch, dirty context, savepoint or transaction fallback, incomplete evidence, and unsupported-shape boundaries.
- BenchmarkScenarioExecutionTests already verify provider-evidence manifest mapping for skipped and completed PIT maintenance rows, including PostgreSQL and SQL Server skipped-placeholder rows and the completed-row contract for selected strategy versus provider-neutral fallback.
- The story already owns the necessary child work rather than needing a new split. Key delivered children cover maintenance-row contract work 06FF438KMPKSBT6KXZ5DBY85QC, read-versus-maintenance doc separation 06FF439ETZKD6WBB5G2MPS9EG8, MySQL/Oracle/DB2 feasibility decisions 06FF43CJ9CJMG7J917RW22QKJC, 06FF43DC469VQ1N0NQ84KEV6SR, and 06FF43E0JCE7BSBFBWB49HGB4G, diagnostics hardening 06FF43FQ8NRX04T9HZHBMFS0PC and 06FF43HQ8E0435ZZSRZQQJW1HC, and release documentation 06FF43JEA6C3HNJ6AQA9XY7EC8.

## Open Questions
- none

## Follow-Up Questions
- When provider-configured pit-full-rebuild-maintenance artifact triplets are later produced for SQL Server, MySQL, or a future DB2 lane, should the evidence matrix rows be promoted from source/test-backed or skipped-placeholder posture to completed-timing without reopening the v0.47 shape boundary?
- If the accepted future DB2 ordinary hub-parent full-rebuild implementation is opened, keep it as a separate ticket limited to IBM.EntityFrameworkCore, rollback-clean transaction behavior, and provider-strategy seam parity rather than widening this story.

## Risks
- Downstream documentation or release summaries could still overstate PIT read rows or source/test-backed provider lanes as maintenance timing evidence if they stop citing the evidence matrix and artifact contract.
- The provider-maintenance baseline is intentionally asymmetric, so careless summaries can imply Oracle or DB2 parity that the repository does not currently implement or benchmark.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Define the v0.47 acceptance contract for provider-specific PIT maintenance evidence. Acceptance: states supported shapes, required diagnostics, required benchmark artifact fields, fallback boundaries, and non-goals; keeps bridge maintenance and scheduling out of scope.