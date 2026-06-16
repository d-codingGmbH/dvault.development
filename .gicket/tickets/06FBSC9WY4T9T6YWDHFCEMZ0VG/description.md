<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Scoped the ticket to the DB2 save-path evidence gap: evaluate the existing clean-context DB2 save baseline and resolve it as a bounded recommendation, not as a broader DB2 provider expansion.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The relevant backlog row is `P1.05` in `docs/plans/provider-optimization-gap-matrix.md`; DB2 latest-satellite (`P0.05`) and DB2 PIT/bridge evidence gaps (`P2.05`/`P3.05`) are separate follow-ups, not part of this ticket.
- The checked-in DB2 save baseline already exists through `AddDVaultDb2()` and `Db2DataVaultSaveStrategy` for clean-context hub, link, and ordinary satellite saves.
- The root benchmark triplet keeps the DB2 `provider-native-bulk-ingestion` rows as skipped placeholders when `DVAULT_TEST_DB2_CONNECTION_STRING` is unset and records `db2SaveBoundary=clean-context-set-based` with `stagedBulkBoundary=not-supported`.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement because the visible repository evidence already gives a bounded recommendation-only contract.

### Scope In
- Evaluate the DB2 `provider-native-bulk-ingestion` save lane against the current clean-context `Db2DataVaultSaveStrategy` baseline.
- Compare staged DB2 bulk, multi-row-style variants, and provider-native chunk ideas only as candidate follow-up options against the checked-in DB2 stop conditions and fallback boundaries.
- Produce one short repository-backed recommendation for this ticket.

### Scope Out
- New DB2 latest-satellite optimization work.
- DB2 PIT or bridge timing/read-strategy evidence work.
- New DB2 benchmark runs, connection-string provisioning, CI/container setup, or checked-in timing artifacts.
- DB2 live-schema reader work or broader provider-release documentation changes.

## Acceptance Criteria
- The evaluation cites the DB2 `provider-native-bulk-ingestion` rows from the root benchmark triplet, `docs/plans/provider-optimization-gap-matrix.md` row `P1.05`, and the visible `Db2DataVaultSaveStrategy` / gate-evaluator baseline.
- The ticket resolves as either `document no-op` for the existing clean-context DB2 save path or `defer with reason` for unsupported staged/multi-row/provider-native chunk work; `implement` or `tune threshold` are only acceptable if checked-in repository evidence explicitly contradicts the current baseline.
- The recommendation explicitly states why staged DB2 bulk, provider-native chunk execution, and fresh threshold tuning are not being reopened by default from the current evidence set.

## Definition of Done
- The authoritative handoff text names the cited DB2 save-path sources and records the chosen recommendation.
- The result keeps DB2 within the current v0.34/v0.39 boundary: no staged bulk claim, no provider-native chunk execution claim, and no completed DB2 timing claim unless new checked-in evidence is cited.
- Any later implementation or benchmark work is called out as follow-up work instead of being silently pulled into this evaluation ticket.

## Implementation Notes
- `src/DCoding.Data.DVault.Db2/Db2DataVaultSaveStrategy.cs` already implements the visible DB2 optimized save path, and `src/DCoding.Data.DVault/DataVaultProviderSaveStrategyGateEvaluator.cs` gives DB2 only the common gates (provider-name match, clean context, no multi-active satellites) with no batch-size threshold.
- `benchmark-summary.md` / `.csv` / `.json` record the DB2 save lane as skipped when `DVAULT_TEST_DB2_CONNECTION_STRING` is unset and encode `db2SaveBoundary=clean-context-set-based`, `stagedBulkBoundary=not-supported`, and `cleanupBoundary=direct-provider-transaction`.
- `docs/releases/v0.34.0.md`, `docs/performance-profiles.md`, and `docs/plans/provider-optimization-evidence-matrix.md` all restate the same DB2 boundary: clean-context optimized saves exist, but staged DB2 bulk, provider-native chunk execution, latest-satellite optimization, and live-schema reading are not in the current baseline.
- `tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs` proves representative DB2 clean-context save behavior and PIT/bridge dispatch selection only as smoke/diagnostics evidence, not as completed timing evidence.

## Open Questions
- none

## Follow-Up Questions
- If product later wants measured DB2 save claims, should that be scheduled as a dedicated DB2 benchmark/evidence ticket instead of expanding this recommendation ticket into execution work?
- If humans later want staged DB2 bulk despite the current `stagedBulkBoundary=not-supported` baseline, should that start with a separate architecture/provider-limitation investigation ticket?

## Risks
- The checked-in root DB2 benchmark lane is skipped, so this ticket can only close with a recommendation based on planned-path, diagnostics, smoke, and code evidence rather than measured DB2 timings.
- Reopening staged bulk or threshold tuning inside this ticket would blur the current DB2 save boundary and risk unsupported release claims.
- Mixing DB2 latest-satellite or PIT/bridge evidence work into this ticket would conflate separate backlog rows that already have independent stop conditions.

## Split Recommendations
- No split recommended: keep this as a bounded recommendation-only DB2 save-path evaluation ticket.
- If the recommendation later changes to implementation, create a separate child ticket for the chosen DB2 save-path change rather than combining implementation with this evaluation.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Use the v0.39 evidence matrix to evaluate DB2 staged, multi-row, and provider-native bulk possibilities. Acceptance: produce a small recommendation: implement, tune threshold, document no-op, or defer with reason.