[gicket-bot] PO refinement contract

Summary
- Refined the ticket to a bounded PostgreSQL PIT full-rebuild observability task: add SQL Server-parity strategy-selection and provider-neutral fallback visibility on the existing maintenance Activity surface, keep benchmark/docs work on sibling tickets, and preserve the current parent/blocking relation context.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository evidence already proves PostgreSQL PIT full-rebuild push-down through AddDVaultPostgres() registering PostgresDataVaultPitMaintenanceStrategy, but the default PIT maintenance service does not currently expose explicit strategy-selected or fallback-cause maintenance Activity evidence for that path.
- SQL Server is the in-repo parity baseline: SqlServerDataVaultPitMaintenanceService already records dvault.strategy.status, dvault.strategy.type, and dvault.fallback.recorded maintenance events for selected and fallback PIT-maintenance paths.
- This ticket is a child of story 06FF437W1CHG9QVJPGZM4Y98AR and currently blocks documentation task 06FF43JEA6C3HNJ6AQA9XY7EC8; current evidence does not justify more child tickets or relation cleanup.

Scope In
- Add bounded observability for IDataVaultPitMaintenanceService.RebuildAsync(...) on PostgreSQL full rebuilds using the existing maintenance Activity surface.
- Surface selected-strategy facts for successful PostgresDataVaultPitMaintenanceStrategy execution.
- Surface finite provider-neutral fallback reasons for declined PostgreSQL provider-strategy evaluation, including no registered strategy when AddDVaultPostgres() is absent.
- Add source/test coverage proving the selected and fallback surfaces stay redacted and do not expose SQL text, hash keys, payload values, or connection data.

Scope Out
- Changing PostgreSQL PIT maintenance shape support beyond the current ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active full-rebuild baseline.
- Provider-specific MaintainParentsAsync(...) work; PostgreSQL parent maintenance remains provider-neutral.
- Transaction/savepoint policy or rollback-clean behavior changes owned by sibling ticket 06FF43GFC5F2VAA0Q7CS9KTX68.
- Benchmark lanes, comparator rows, and evidence-matrix changes owned by 06FF43AH9SK6J07GV5EKYV3AMM, 06FF43BPP5NRJR3JTY48ZNEKHM, and 06FF438KMPKSBT6KXZ5DBY85QC.
- Release and architecture/performance documentation updates owned by blocked task 06FF43JEA6C3HNJ6AQA9XY7EC8.

Open questions
- none

Follow-up questions
- Should 06FF43JEA6C3HNJ6AQA9XY7EC8 explicitly update the activity-tracing contract and release/performance docs so the PostgreSQL maintenance fallback vocabulary is documented alongside the existing SQL Server behavior?
- Should benchmark ticket 06FF43AH9SK6J07GV5EKYV3AMM reuse the exact selected-strategy and fallback-cause names proven here in its artifact executionDetail rows?

Risks
- DefaultDataVaultPitMaintenanceService has no existing save/read-style selector object, so ad hoc fallback capture could drift from the repository's established finite diagnostics pattern unless it explicitly reuses the gate evaluator.
- The current activity-tracing documentation still treats maintenance fallback causes as effectively undocumented, so code landing before docs could create a temporary source-versus-doc mismatch.
- Sibling transaction-review or benchmark tickets may later narrow or expand PostgreSQL maintenance eligibility; this ticket should keep the observability vocabulary stable across those later changes.

Split recommendations
- Do not split further now; transaction review, benchmark lane, comparator/evidence-matrix work, and documentation already exist as bounded sibling tickets.
- Only create a new follow-up if implementation proves the existing maintenance Activity surface cannot carry the required bounded facts cleanly; keep any such follow-up limited to a dedicated maintenance diagnostics surface.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 3
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment