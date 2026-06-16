[gicket-bot] PO refinement contract

Summary
- Repository evidence shows the accepted SQL Server bulk save improvement is already present, with strategy registration, gate thresholds, smoke coverage, diagnostics and fallback coverage, and benchmark contract updates; no split or persistent planning write was needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Visible repository evidence supports the accepted-and-implemented path, not the spike-rejected path; treat this ticket as ratifying or closing the already-landed SQL Server provider-native bulk save implementation.
- For this ticket, 'benchmark evidence updated' means the repository preserves SQL Server `provider-native-bulk-ingestion` benchmark row identity, execution-detail guidance, and verifier coverage; checked-in optional-provider timing may still remain `skipped-placeholder` when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset.
- Live ticket comments, relations, and attachments were not re-read because the bounded `gicket-read-*` calls were trust-blocked; this refinement relies on the supplied ticket snapshot plus bounded repository evidence, and no relation or planning writes were materialized.

Scope In
- SQL Server provider-native bulk save behavior through `AddDVaultSqlServer()` and `SqlServerDataVaultSaveStrategy`.
- The bounded SQL Server save gate and fallback contract: provider-name match, clean `DbContext`, no multi-active satellite operations, minimum 50 total operations, and maximum 500 satellite operations.
- Unit, integration, smoke, and benchmark-verifier coverage that proves SQL Server strategy selection, fallback causes, transaction and cancellation behavior, and benchmark execution-detail facts for `provider-native-bulk-ingestion`.

Scope Out
- New SQL Server latest-satellite optimization; the visible baseline still records no provider-specific latest-satellite read strategy.
- Completed SQL Server PIT or bridge timing evidence or broader SQL Server read-strategy expansion beyond the already-visible diagnostics-gated candidate baseline.
- Cross-provider optimization work for PostgreSQL, MySQL, Oracle, or DB2.
- Provisioning an opt-in external SQL Server environment or requiring checked-in completed timing rows when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unavailable.

Open questions
- none

Follow-up questions
- When a configured SQL Server environment is available, should a later evidence-gap ticket replace the checked-in SQL Server `skipped-placeholder` benchmark rows with completed `provider-native-bulk-ingestion` timing artifacts?
- Should later backlog work create separate SQL Server tickets for latest-satellite optimization or configured PIT and bridge timing evidence, both of which remain explicit follow-up gaps rather than scope for this ticket?

Risks
- Because the bounded `gicket-read-ticket`, `gicket-read-ticket-comments`, `gicket-read-ticket-relations`, and `gicket-read-ticket-attachments` calls were trust-blocked, live persisted relation, comment, and attachment state was not independently re-verified in this run.
- The checked-in benchmark baseline still shows SQL Server optional-provider rows as skipped when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset; reviewers must not reinterpret the current repository as already carrying completed SQL Server timing evidence.
- If the ticket is treated as a fresh implementation task instead of a closure-oriented ratification of landed code, the next workflow step risks duplicate development against an already-proven baseline.

Split recommendations
- No split recommended; current repository evidence already bounds this ticket to ratifying or closing the landed SQL Server native bulk save implementation and its existing diagnostics, smoke, and benchmark-contract coverage.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment