[gicket-bot] PO refinement contract

Summary
- Refined the ticket around the repository-backed MySQL latest-satellite baseline: MySQL still has no latest-satellite provider strategy registration, the checked-in benchmark and test guidance still asserts provider-neutral fallback for that shape, and no branch-local implementation delta was visible from the supplied scratch ref.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current repository baseline is explicit: SQLite is the only optimized latest-satellite provider path; MySQL currently registers PIT and bridge read strategies only, not a latest-satellite `IDataVaultProviderReadStrategy`.
- For this ticket, closing the gap means either implementing a real MySQL latest-satellite strategy end to end or closing the item with explicit no-work-required or rejection documentation; partial code or evidence-only churn does not satisfy the ticket.
- No repository-local v0.41 planning or release artifact was visible in this branch, so the operative criteria surface for this refinement is the checked-in matrix, benchmark, and test contract already present in the repository.
- Benchmark evidence for this ticket is bounded to the existing provider-evidence surfaces: checked-in guidance rows and automated expectations may change, but measured MySQL timing must not be claimed unless provider-configured artifacts are actually produced.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run.

Scope In
- The MySQL latest-satellite read decision only, including provider strategy registration or dispatch, diagnostics posture, fallback behavior, benchmark guidance rows, and automated coverage for that single read shape.
- The no-work-required closure path if repository-backed investigation concludes MySQL latest-satellite optimization should remain rejected in the current baseline.

Scope Out
- MySQL PIT or bridge strategy changes beyond preserving their existing baseline.
- PostgreSQL, SQL Server, Oracle, DB2, or SQLite latest-satellite work.
- MySQL save-strategy work, staging thresholds, or non-read provider behavior.
- Any claim of measured MySQL latest-satellite performance without configured provider benchmark artifacts.

Open questions
- none

Follow-up questions
- If the team later wants measured MySQL latest-satellite timing rather than guidance-only evidence, should that be tracked as a separate provider-configured benchmark ticket after this closure decision lands?

Risks
- The main delivery risk is a partial implementation that adds some MySQL-specific code but leaves benchmark guidance, diagnostics, or tests asserting the old no-strategy baseline.
- The checked-in root benchmark triplet currently keeps MySQL external-provider rows as skipped placeholders when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is unset, so measured performance claims remain easy to overstate unless guarded carefully.

Split recommendations
- No split recommended; this is one bounded provider and shape closure decision and should either land end to end or close with explicit no-work-required documentation.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment