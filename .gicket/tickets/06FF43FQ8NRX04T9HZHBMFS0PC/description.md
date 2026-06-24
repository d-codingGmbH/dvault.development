<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket to a bounded PostgreSQL PIT full-rebuild observability task: add SQL Server-parity strategy-selection and provider-neutral fallback visibility on the existing maintenance Activity surface, keep benchmark/docs work on sibling tickets, and preserve the current parent/blocking relation context.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current repository evidence already proves PostgreSQL PIT full-rebuild push-down through AddDVaultPostgres() registering PostgresDataVaultPitMaintenanceStrategy, but the default PIT maintenance service does not currently expose explicit strategy-selected or fallback-cause maintenance Activity evidence for that path.
- SQL Server is the in-repo parity baseline: SqlServerDataVaultPitMaintenanceService already records dvault.strategy.status, dvault.strategy.type, and dvault.fallback.recorded maintenance events for selected and fallback PIT-maintenance paths.
- This ticket is a child of story 06FF437W1CHG9QVJPGZM4Y98AR and currently blocks documentation task 06FF43JEA6C3HNJ6AQA9XY7EC8; current evidence does not justify more child tickets or relation cleanup.

### Scope In
- Add bounded observability for IDataVaultPitMaintenanceService.RebuildAsync(...) on PostgreSQL full rebuilds using the existing maintenance Activity surface.
- Surface selected-strategy facts for successful PostgresDataVaultPitMaintenanceStrategy execution.
- Surface finite provider-neutral fallback reasons for declined PostgreSQL provider-strategy evaluation, including no registered strategy when AddDVaultPostgres() is absent.
- Add source/test coverage proving the selected and fallback surfaces stay redacted and do not expose SQL text, hash keys, payload values, or connection data.

### Scope Out
- Changing PostgreSQL PIT maintenance shape support beyond the current ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active full-rebuild baseline.
- Provider-specific MaintainParentsAsync(...) work; PostgreSQL parent maintenance remains provider-neutral.
- Transaction/savepoint policy or rollback-clean behavior changes owned by sibling ticket 06FF43GFC5F2VAA0Q7CS9KTX68.
- Benchmark lanes, comparator rows, and evidence-matrix changes owned by 06FF43AH9SK6J07GV5EKYV3AMM, 06FF43BPP5NRJR3JTY48ZNEKHM, and 06FF438KMPKSBT6KXZ5DBY85QC.
- Release and architecture/performance documentation updates owned by blocked task 06FF43JEA6C3HNJ6AQA9XY7EC8.

## Acceptance Criteria
- The ticket ratifies the current PostgreSQL maintenance baseline: AddDVaultPostgres() registers PostgresDataVaultPitMaintenanceStrategy for explicit full rebuilds only, with no scope increase to parent-maintenance or new PIT shapes.
- When a PostgreSQL full rebuild runs through the provider path, maintenance tracing or equivalent bounded diagnostics show ProviderStrategySelected and the selected strategy name PostgresDataVaultPitMaintenanceStrategy.
- When PostgreSQL full rebuilds fall back to provider-neutral maintenance, the bounded fallback surface reports finite reasons from the existing maintenance gate vocabulary, covering provider mismatch, dirty DbContext, incomplete maintenance-shape evidence, unsupported PIT shape, and no provider-specific strategy registered or strategy declined when applicable.
- Fallback and selected-strategy evidence is verified by tests and remains redacted: no raw SQL text, connection strings, hash-key values, driving-key values, or payload values appear.
- The change preserves current Postgres rebuild results and registration behavior; it adds observability without widening eligibility or claiming benchmark-backed performance evidence.
- The resulting vocabulary is stable enough for blocked docs task 06FF43JEA6C3HNJ6AQA9XY7EC8 and benchmark sibling 06FF43AH9SK6J07GV5EKYV3AMM to cite directly.

## Definition of Done
- Repository tests cover at least one selected PostgreSQL PIT rebuild path and one provider-neutral fallback PostgreSQL PIT rebuild path on the agreed observability surface.
- The implementation reuses a finite existing maintenance fallback vocabulary instead of introducing unbounded free-text diagnostics.
- The ticket-visible outcome states that PostgreSQL parent maintenance stays provider-neutral and that benchmark/doc follow-up remains on the existing sibling tickets.

## Implementation Notes
- DefaultDataVaultPitMaintenanceService currently selects provider strategies by CanRebuild(...) and falls straight to provider-neutral rebuilds without save/read-style selection diagnostics; that selection seam is the main implementation touchpoint.
- Reuse DataVaultProviderPitMaintenanceStrategyGateEvaluator and DataVaultPitMaintenanceStrategyFallbackCauseKind as the bounded fallback source, including NoProviderSpecificStrategyRegistered when no provider PIT maintenance strategy is registered.
- Use SqlServerDataVaultPitMaintenanceService plus DataVaultMaintenanceActivity as the parity model for dvault.strategy.status, dvault.strategy.type, and dvault.fallback.recorded maintenance events.
- Keep PostgresDataVaultPitMaintenanceStrategy, its SQL generation, and AddDVaultPostgres() registration semantics unchanged unless required to emit the bounded observability facts.
- Current repository evidence already lives in DVaultPostgresServiceCollectionExtensions.cs, PostgresProviderCapabilityTests.cs, and PostgresPitMaintenanceServiceTests.cs; extend that source/test baseline instead of inventing a separate public maintenance diagnostics API unless the existing Activity surface cannot carry the required finite facts.
- If code introduces maintenance fallback vocabulary that exceeds the current tracing docs, let blocked docs task 06FF43JEA6C3HNJ6AQA9XY7EC8 reconcile the contract wording after the implementation lands rather than reopening this ticket's scope.

## Open Questions
- none

## Follow-Up Questions
- Should 06FF43JEA6C3HNJ6AQA9XY7EC8 explicitly update the activity-tracing contract and release/performance docs so the PostgreSQL maintenance fallback vocabulary is documented alongside the existing SQL Server behavior?
- Should benchmark ticket 06FF43AH9SK6J07GV5EKYV3AMM reuse the exact selected-strategy and fallback-cause names proven here in its artifact executionDetail rows?

## Risks
- DefaultDataVaultPitMaintenanceService has no existing save/read-style selector object, so ad hoc fallback capture could drift from the repository's established finite diagnostics pattern unless it explicitly reuses the gate evaluator.
- The current activity-tracing documentation still treats maintenance fallback causes as effectively undocumented, so code landing before docs could create a temporary source-versus-doc mismatch.
- Sibling transaction-review or benchmark tickets may later narrow or expand PostgreSQL maintenance eligibility; this ticket should keep the observability vocabulary stable across those later changes.

## Split Recommendations
- Do not split further now; transaction review, benchmark lane, comparator/evidence-matrix work, and documentation already exist as bounded sibling tickets.
- Only create a new follow-up if implementation proves the existing maintenance Activity surface cannot carry the required bounded facts cleanly; keep any such follow-up limited to a dedicated maintenance diagnostics surface.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Harden PostgreSQL PIT maintenance observability so selected provider strategy and fallback causes are visible consistently with SQL Server. Acceptance: tests prove selected strategy tags or diagnostics and provider-neutral fallback reasons without leaking SQL, keys, or connection data.