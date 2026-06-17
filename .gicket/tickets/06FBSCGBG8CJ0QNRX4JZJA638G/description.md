<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Applied the parent delivery-contract rewrite to ratify the existing five provider-specific PIT/bridge follow-up tickets, and queued child-description updates so PostgreSQL, SQL Server, MySQL, and Oracle explicitly target provider-configured timing evidence while DB2 is deferred into planning-only follow-up.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The authoritative follow-up split is the already-persisted five-ticket provider graph: PostgreSQL 06FBSCGGN528A2NC6TTA5A99X0, SQL Server 06FBSCGNY2R6PC7P4Y91RD0HVR, MySQL 06FBSCGVAZ5G8NP1TRXFNEP6DW, Oracle 06FBSCH0M358R5J3RGFB6GRDM4, and DB2 06FBSCH65R88BT6PS7XV32NQ1M.
- PostgreSQL, SQL Server, MySQL, and Oracle remain evidence-completion tickets for already-registered PIT and bridge strategy candidates; they are not new public API or strategy-design tickets.
- DB2 remains deferred out of the v0.41 implementation batch and uses the existing DB2 child as planning-only follow-up for any later explicit DB2 evidence approval.
- No relation rewrites, attachments, planning documents, or new child tickets were needed because the existing provider-specific graph was already the correct topology.

### Scope In
- Ratify the persisted five-child provider split as the authoritative PIT and bridge follow-up plan.
- Align parent and child ticket descriptions with provider-configured PIT and bridge timing-evidence scope for PostgreSQL, SQL Server, MySQL, and Oracle.
- Carry forward the bounded DB2 defer posture as planning-only follow-up.
- Preserve SQLite as a no-op audit baseline and keep PIT and bridge architecture boundaries tied to the checked-in repository evidence.

### Scope Out
- Runtime code changes to PIT or bridge reads, provider dispatch, diagnostics, or maintenance services.
- Benchmark reruns, external database provisioning, connection-string setup, or new measured provider timing claims in this ticket.
- Any regrouping of the already-persisted provider-specific child graph into new combined tickets.
- DB2 boundary expansion, non-SQLite latest-satellite work, or save-strategy follow-up outside this PIT and bridge audit.

## Acceptance Criteria
- The parent delivery contract names the five existing provider-specific downstream tickets as the authoritative split and no longer recommends grouped PostgreSQL plus SQL Server or MySQL plus Oracle follow-up tickets.
- The PostgreSQL, SQL Server, MySQL, and Oracle child tickets describe provider-configured PIT and bridge timing evidence for existing strategy candidates.
- The DB2 child ticket description preserves DB2 as deferred planning only, outside the active v0.41 implementation batch.
- The refined contract keeps repository evidence anchored to docs/plans/provider-optimization-gap-matrix.md, docs/plans/provider-optimization-evidence-matrix.md, benchmark-summary.md, benchmark-summary.json, docs/architecture/dvault-v1-pit-bridge-boundary.md, and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs.

## Definition of Done
- The parent ticket description update is applied and is the authoritative handoff surface for this audit ticket.
- Queued owner-branch description replays exist for the PostgreSQL, SQL Server, MySQL, Oracle, and DB2 child tickets, with durable outbox records captured in this run.
- SQLite remains a no-op audit baseline and no refinement text opens new SQLite PIT or bridge work.
- DB2 remains plan-only follow-up and no refinement text turns skipped-placeholder, diagnostics-only, or smoke-only DB2 evidence into completed timing claims.

## Implementation Notes
- Queued child-description replays: PostgreSQL 06FBSCGGN528A2NC6TTA5A99X0 to mutation-d78fbef40dbbda1b, SQL Server 06FBSCGNY2R6PC7P4Y91RD0HVR to mutation-4c87e150a36f9176, MySQL 06FBSCGVAZ5G8NP1TRXFNEP6DW to mutation-271ebbbf0ea4550f, Oracle 06FBSCH0M358R5J3RGFB6GRDM4 to mutation-f7bd3be1be048800, and DB2 06FBSCH65R88BT6PS7XV32NQ1M to mutation-1249180e0474cd96.
- docs/architecture/dvault-v1-pit-bridge-boundary.md names AddDVaultPostgres(), AddDVaultSqlServer(), AddDVaultMySql(), AddDVaultOracle(), and AddDVaultDb2() as diagnostics-gated PIT and bridge strategy-candidate registrations, which keeps the active-provider children in evidence-completion scope rather than fresh design scope.
- tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs plus benchmark-summary.md and benchmark-summary.json preserve skipped external-provider PIT and bridge rows with deterministic planned strategy names, so downstream work remains provider-configured timing evidence collection.
- No new tickets, relation mutations, attachments, or planning documents were created in this pass.

## Open Questions
- none

## Follow-Up Questions
- Which configured PostgreSQL, SQL Server, MySQL, and Oracle environments should stay available for repeat PIT and bridge benchmark reruns after the first evidence pass?
- When the queued child-description mutations replay on their owner branches, should each child ticket immediately re-enter PO-critic or wait until the implementing owner is ready to pick up provider-specific evidence work?
- If DB2 evidence work is later approved, should the team first reopen the narrower v0.34 DB2 boundary explicitly or keep the DB2 child strictly as evidence-planning without implementation scope?

## Risks
- The five child-description updates are durable queued mutations on their owner branches; until replay completes, the canonical child branches may briefly lag the parent contract text.
- External-provider PIT and bridge evidence still depends on configured connection strings and benchmark infrastructure, so the active-provider child tickets can still stall after refinement.
- A downstream ticket could over-claim performance if it treats skipped-placeholder or diagnostics-only rows as completed timing evidence without new benchmark artifact triplets.

## Split Recommendations
- Use existing child 06FBSCGGN528A2NC6TTA5A99X0 for PostgreSQL provider-configured PIT and bridge timing evidence against PostgresDataVaultReadStrategy.
- Use existing child 06FBSCGNY2R6PC7P4Y91RD0HVR for SQL Server provider-configured PIT and bridge timing evidence against SqlServerDataVaultReadStrategy.
- Use existing child 06FBSCGVAZ5G8NP1TRXFNEP6DW for MySQL provider-configured PIT and bridge timing evidence against MySqlDataVaultReadStrategy.
- Use existing child 06FBSCH0M358R5J3RGFB6GRDM4 for Oracle provider-configured PIT and bridge timing evidence against OracleDataVaultReadStrategy.
- Use existing child 06FBSCH65R88BT6PS7XV32NQ1M only as deferred DB2 planning until explicit DB2 evidence scope and environment-backed benchmark work are approved.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Use v0.39 evidence to identify provider-specific PIT and bridge read gaps worth implementing in v0.41. Acceptance: each provider is marked implement, no-op, or defer with reason before provider implementation tickets run.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Added repository-backed regression coverage in `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs` for the provider-specific PIT/bridge audit split.
- The new test pins PostgreSQL, SQL Server, MySQL, and Oracle as active provider-configured PIT/bridge timing-evidence follow-ups, while DB2 remains non-timing narrowed-boundary evidence.
- No runtime PIT/bridge dispatch, provider strategy, benchmark artifact, or external database behavior was changed.

### Verification
- `bash tools/check-format.sh` passed.
- `git diff --check -- tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs` passed.
- `dotnet test tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj --nologo --no-restore --filter FullyQualifiedName~ProviderPitBridgeAuditKeepsProviderSpecificFollowUpSplit` could not start because the local NuGet cache is missing `Microsoft.EntityFrameworkCore.Analyzers` 8.0.28 and 10.0.9.
- `dotnet build DVault.slnx --nologo --no-restore` could not complete for the same missing EF Core analyzer cache entries, plus `xunit.analyzers` 1.27.0.

### Notes
- The implementation preserves skipped-placeholder benchmark rows as non-timing evidence.
- External provider timing remains downstream child-ticket work for configured provider environments.

<!-- gicket-bot:developer-delivery:v1:end -->