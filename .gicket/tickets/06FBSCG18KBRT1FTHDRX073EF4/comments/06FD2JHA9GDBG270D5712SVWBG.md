[gicket-bot] PO refinement contract

Summary
- Refined this as a bounded implementation ticket to add Oracle latest-satellite provider-strategy support and align diagnostics, fallback, benchmark guidance, and evidence docs; no child split or durable planning writes were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence shows AddDVaultOracle() currently registers Oracle save plus PIT and bridge read strategies only; it does not register an IDataVaultProviderReadStrategy for latest-satellite reads, and benchmark-summary.*, the provider evidence matrix, and the gap matrix still record Oracle latest-satellite-read as selectedStrategy=<none> with no provider-specific latest-satellite strategy.
- No separate v0.41 repository note was found; the operative refinement criteria already live in the current performance-evidence contract, benchmark README, performance profiles, and provider evidence and gap matrices.
- The bounded Oracle latest-satellite v1 shape should follow the existing latest and as-of satellite contract already used by SQLite and SQL Server: hub-parent satellites only, no multi-active driving keys, the same DataVaultLatestSatelliteReadRequest current and as-of semantics, and provider-neutral fallback on mismatch or unsupported shape.
- Post-implementation, Oracle latest-satellite should move from a capability-gap posture to an evidence-gap or planned-strategy posture until provider-configured Oracle timing artifacts exist, matching the current SQL Server latest-satellite evidence model.
- No child tickets, relation updates, description edits, attachments, or planning documents were materialized in this run; the provided ticket snapshot remained the authoritative persisted ticket context because prior gicket read commands for live ticket, comment, and relation data were trust-blocked.

Scope In
- Extend Oracle provider read support so AddDVaultOracle() exposes an Oracle latest-satellite strategy within the existing IDataVaultReadService and IDataVaultReadDiagnosticsService boundary.
- Reuse the existing Oracle provider read-strategy path so supported Oracle latest-satellite current and as-of requests can select OracleDataVaultReadStrategy for the same bounded latest-satellite shape already documented for SQL Server and SQLite.
- Add or update unit, parity, diagnostics, registration, and integration tests that prove Oracle latest-satellite selection, provider-neutral parity, and finite fallback behavior without changing PIT or bridge semantics.
- Update benchmark guidance and repository evidence docs that currently encode Oracle latest-satellite as a no-strategy capability gap so they instead reflect a planned or diagnostics-gated Oracle latest-satellite strategy with skipped-placeholder timing allowed when Oracle is not configured.

Scope Out
- Broadening latest-satellite support beyond hub-parent, non-multi-active current or as-of requests.
- Changing Oracle PIT or bridge behavior, save-strategy behavior, or introducing staged Oracle save or bulk work.
- Claiming completed Oracle timing evidence, release publication, or consumer release-note work when Oracle benchmark execution is still unconfigured.
- Implementing PostgreSQL, MySQL, or DB2 latest-satellite provider strategies as part of this ticket.

Open questions
- none

Follow-up questions
- After this capability gap is closed, should a separate evidence ticket collect provider-configured Oracle latest-satellite timing artifacts so Oracle can move from planned-strategy posture to completed timing evidence?
- When the next release-note ticket is created beyond the current checked-in v0.40.0 baseline, which release or backlog ticket should own publishing the Oracle latest-satellite posture change outside the benchmark and evidence docs updated here?

Risks
- Oracle benchmark execution may remain skipped in unattended validation if DVAULT_TEST_ORACLE_CONNECTION_STRING is unset, so the ticket must avoid overclaiming completed Oracle timing evidence.
- Docs drift is likely unless all evidence surfaces move together; multiple checked-in files currently state that Oracle latest-satellite has no provider-specific strategy.
- If Oracle-specific SQL behavior cannot preserve provider-neutral latest or as-of parity within the existing narrow shape, the safe outcome is explicit no-work-required documentation rather than a widened or partially specified strategy contract.

Split recommendations
- none

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