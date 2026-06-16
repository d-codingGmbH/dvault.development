[gicket-bot] PO refinement contract

Summary
- Refined this as a repository-backed acceptance contract for future provider bulk expansion on the existing save path: finite supported shapes, caller-owned EF Core transaction semantics, provider-neutral fallback, diagnostics and benchmark gates, and explicit non-goals. No child tickets, relation updates, description updates, attachments, or planning documents were materialized in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current repository baseline already documents provider-specific save-strategy lanes for SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2; this ticket only defines the acceptance gate for later bulk expansion or threshold changes, not a new provider-support decision.
- Provider bulk expansion stays inside the existing IDataVaultSaveService and DI-selected IDataVaultProviderSaveStrategy boundary; it does not add a new public runtime API, SaveChanges interception path, or read-model surface.
- A provider implementation ticket may close as no-work when the exact provider and workload cannot preserve current save semantics or cannot produce repository-backed threshold evidence that beats the provider-neutral fallback.

Scope In
- Define the accepted save-path scope for provider-specific bulk work behind the existing explicit IDataVaultSaveService boundary.
- Define the finite supported shape baseline for optimized bulk candidates: clean EF Core contexts, ordered explicit bulk batches or chunk-internal ordered batches, provider-name match, and ordinary hub/link/satellite operations.
- Define required caller-owned transaction behavior, provider-neutral fallback behavior, diagnostics/telemetry/tracing evidence, and benchmark-threshold evidence for future provider bulk tickets.
- Define that future implementation tickets may close as no-work when the documented gate is not met.

Scope Out
- Read-strategy expansion, PIT or bridge optimization, latest-satellite work, and live-schema reading.
- SaveChanges interception, background ingestion, CDC, scheduler orchestration, file-ingestion lanes, and automatic strategy routing outside the save-service dispatcher.
- Deployable SQL payloads, stored-procedure deployment, runtime artifact dispatch, migration synchronization, database provisioning, credential handling, environment routing, dashboards, or package publication responsibilities.
- Re-deciding the visible supported-provider save baseline already documented in repository architecture and performance guidance.

Open questions
- none

Follow-up questions
- When a concrete provider expansion ticket is opened, which exact provider/workload comparator and artifact bundle will be the preserved threshold-evidence record for that ticket?
- If a later provider proposal needs both runtime save-strategy work and design-time SQL artifact review work, should those be split into separate tickets to preserve the current runtime-versus-artifact boundary?

Risks
- Without an explicit evidence gate, later tickets could overclaim provider performance from skipped-placeholder rows or diagnostics-only evidence.
- Without the finite supported-shape boundary, future stories may accidentally absorb dirty-context handling, multi-active satellite support, or read-model work that the current repository baseline treats as fallback or separate scope.
- Without the explicit non-goal statement, provider bulk work can drift into deployment, migration, or operational ownership that the repository documents currently keep consumer-owned or out of scope.

Split recommendations
- No additional split is justified for this PO refinement ticket.
- Materialize future implementation work per provider and, when needed, separate runtime save-strategy changes from artifact or deployment review work instead of broadening one provider-bulk ticket.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment