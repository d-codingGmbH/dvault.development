[gicket-bot] PO refinement contract

Summary
- Scoped the ticket to the DB2 save-path evidence gap: evaluate the existing clean-context DB2 save baseline and resolve it as a bounded recommendation, not as a broader DB2 provider expansion.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The relevant backlog row is `P1.05` in `docs/plans/provider-optimization-gap-matrix.md`; DB2 latest-satellite (`P0.05`) and DB2 PIT/bridge evidence gaps (`P2.05`/`P3.05`) are separate follow-ups, not part of this ticket.
- The checked-in DB2 save baseline already exists through `AddDVaultDb2()` and `Db2DataVaultSaveStrategy` for clean-context hub, link, and ordinary satellite saves.
- The root benchmark triplet keeps the DB2 `provider-native-bulk-ingestion` rows as skipped placeholders when `DVAULT_TEST_DB2_CONNECTION_STRING` is unset and records `db2SaveBoundary=clean-context-set-based` with `stagedBulkBoundary=not-supported`.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement because the visible repository evidence already gives a bounded recommendation-only contract.

Scope In
- Evaluate the DB2 `provider-native-bulk-ingestion` save lane against the current clean-context `Db2DataVaultSaveStrategy` baseline.
- Compare staged DB2 bulk, multi-row-style variants, and provider-native chunk ideas only as candidate follow-up options against the checked-in DB2 stop conditions and fallback boundaries.
- Produce one short repository-backed recommendation for this ticket.

Scope Out
- New DB2 latest-satellite optimization work.
- DB2 PIT or bridge timing/read-strategy evidence work.
- New DB2 benchmark runs, connection-string provisioning, CI/container setup, or checked-in timing artifacts.
- DB2 live-schema reader work or broader provider-release documentation changes.

Open questions
- none

Follow-up questions
- If product later wants measured DB2 save claims, should that be scheduled as a dedicated DB2 benchmark/evidence ticket instead of expanding this recommendation ticket into execution work?
- If humans later want staged DB2 bulk despite the current `stagedBulkBoundary=not-supported` baseline, should that start with a separate architecture/provider-limitation investigation ticket?

Risks
- The checked-in root DB2 benchmark lane is skipped, so this ticket can only close with a recommendation based on planned-path, diagnostics, smoke, and code evidence rather than measured DB2 timings.
- Reopening staged bulk or threshold tuning inside this ticket would blur the current DB2 save boundary and risk unsupported release claims.
- Mixing DB2 latest-satellite or PIT/bridge evidence work into this ticket would conflate separate backlog rows that already have independent stop conditions.

Split recommendations
- No split recommended: keep this as a bounded recommendation-only DB2 save-path evaluation ticket.
- If the recommendation later changes to implementation, create a separate child ticket for the chosen DB2 save-path change rather than combining implementation with this evaluation.

Persisted contract coverage
- acceptance-criteria items: 3
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment