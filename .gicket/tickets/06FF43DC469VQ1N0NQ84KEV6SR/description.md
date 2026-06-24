<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this into a bounded evidence-first Oracle PIT maintenance investigation anchored to the existing v1 asymmetry: PostgreSQL and SQL Server have provider-native PIT full-rebuild paths, while Oracle currently exposes save/read optimizations but no Oracle PIT maintenance implementation.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current repository evidence shows `AddDVaultOracle()` registers Oracle provider capability, save, and read surfaces, but `src/DCoding.Data.DVault.Oracle` contains no Oracle PIT maintenance strategy and no Oracle-specific `IDataVaultPitMaintenanceService`.
- The investigation stays inside the explicit PIT maintenance boundary from `docs/architecture/dvault-v1-pit-bridge-boundary.md`: caller-invoked rebuild only, with no read-time refresh, `SaveChanges` interception, startup automation, or background scheduling.
- Oracle feasibility should be judged against the existing provider-native rebuild guardrails already used in code: clean-context gating, provider-name and shape-evidence checks, provider-neutral fallback on guard failure, and rollback-clean failure behavior for full rebuilds.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized during this refinement.

### Scope In
- Inspect current Oracle startup and provider code to confirm what Oracle-specific PIT maintenance surface exists today and what is missing.
- Compare any Oracle full-rebuild candidate against the current PostgreSQL and SQL Server PIT maintenance baselines for supported shapes, fallback rules, and transaction safety.
- Record supported and unsupported PIT shapes, SQL shape constraints, transaction caveats, and an explicit implement-or-defer recommendation for Oracle full-rebuild push-down.

### Scope Out
- Implementing an Oracle PIT maintenance strategy or service in this ticket.
- Oracle `MaintainParentsAsync(...)`, bridge maintenance push-down, or automatic PIT maintenance orchestration.
- Changing Oracle latest-satellite, PIT-read, or bridge-read strategies except where read-path evidence is referenced as comparison context.

## Acceptance Criteria
- The investigation documents that current Oracle startup registers provider capability, read, and save strategy surfaces, but no Oracle PIT maintenance push-down surface is presently implemented.
- The investigation compares Oracle feasibility with the current provider-native PIT maintenance baselines: PostgreSQL strategy-based full rebuilds and SQL Server's service-based ordinary hub-parent full rebuilds with rollback-clean failure handling.
- The investigation explicitly states whether Oracle can safely support bounded PIT full-rebuild push-down for each relevant shape category: ordinary hub-parent PITs, multi-active hub-parent PITs, link-parent PITs, and full-rebuild-only versus parent-maintenance scope.
- The investigation records concrete SQL and provider API risks, including transaction/savepoint or equivalent rollback behavior, partial-refresh risk on fault/cancellation, and any Oracle-specific SQL construction complexity needed beyond the provider-neutral path.
- The investigation ends with an explicit recommendation to either implement a narrowly guarded Oracle provider path now or defer it, with the required guardrails or blocking reasons spelled out.

## Definition of Done
- The ticket-authoritative output captures the evidence, supported and unsupported shapes, risks, and final recommendation in one place.
- The recommended next step is bounded enough that development can either implement a specific Oracle candidate or leave Oracle on provider-neutral PIT maintenance without reopening PO scope.
- The refinement leaves no PO-stage blocker questions for critic review.

## Implementation Notes
- Use `docs/architecture/dvault-v1-pit-bridge-boundary.md` as the source-of-truth boundary: current accepted PIT maintenance push-down is intentionally asymmetric and explicit.
- Use `src/DCoding.Data.DVault.Postgres/PostgresDataVaultPitMaintenanceStrategy.cs`, `src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultPitMaintenanceService.cs`, and `src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs` as the comparison baseline for supported shapes, gating, and fallback semantics.
- Treat Oracle read optimization evidence separately from maintenance feasibility; `OracleDataVaultReadStrategy` and Oracle PIT read benchmarks do not prove Oracle PIT rebuild push-down.
- If Oracle only supports a smaller safe subset than PostgreSQL, the recommendation should ratify that subset explicitly and keep all other Oracle PIT rebuild requests on provider-neutral fallback.
- Any implementation recommendation must preserve explicit caller-owned maintenance and diagnostics/fallback reporting rather than introducing automatic refresh behavior.

## Open Questions
- none

## Follow-Up Questions
- If the investigation recommends defer, should the downstream blocked work stay blocked on new provider evidence or be rescheduled behind a separate future-facing Oracle optimization ticket?
- If the investigation recommends implementation, does Oracle fit the existing strategy-selection seam cleanly or does it require SQL Server-style service ownership because of transaction semantics?

## Risks
- Oracle may not offer rollback-clean full-rebuild behavior through the same EF Core transaction/savepoint surfaces relied on by the current SQL Server safeguard, which raises partial PIT refresh risk.
- The PostgreSQL rebuild path depends on SQL patterns such as `WITH`, `UNION`, and lateral snapshot selection; Oracle may require materially different SQL that expands the proof surface.
- Existing Oracle PIT read evidence can be misread as maintenance evidence, creating scope pressure to ship a provider push-down path without equivalent rebuild-specific proof.

## Split Recommendations
- No split is needed during refinement; only create a follow-up implementation ticket if the evaluation produces a clearly bounded Oracle full-rebuild candidate.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Investigate whether Oracle can safely support bounded PIT full-rebuild push-down through EF Core provider APIs. Acceptance: records supported/unsupported shapes, transaction caveats, SQL shape risks, and explicit implementation/defer recommendation.