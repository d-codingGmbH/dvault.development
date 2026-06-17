<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this as a bounded implementation ticket to add Oracle latest-satellite provider-strategy support and align diagnostics, fallback, benchmark guidance, and evidence docs; no child split or durable planning writes were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence shows AddDVaultOracle() currently registers Oracle save plus PIT and bridge read strategies only; it does not register an IDataVaultProviderReadStrategy for latest-satellite reads, and benchmark-summary.*, the provider evidence matrix, and the gap matrix still record Oracle latest-satellite-read as selectedStrategy=<none> with no provider-specific latest-satellite strategy.
- No separate v0.41 repository note was found; the operative refinement criteria already live in the current performance-evidence contract, benchmark README, performance profiles, and provider evidence and gap matrices.
- The bounded Oracle latest-satellite v1 shape should follow the existing latest and as-of satellite contract already used by SQLite and SQL Server: hub-parent satellites only, no multi-active driving keys, the same DataVaultLatestSatelliteReadRequest current and as-of semantics, and provider-neutral fallback on mismatch or unsupported shape.
- Post-implementation, Oracle latest-satellite should move from a capability-gap posture to an evidence-gap or planned-strategy posture until provider-configured Oracle timing artifacts exist, matching the current SQL Server latest-satellite evidence model.
- No child tickets, relation updates, description edits, attachments, or planning documents were materialized in this run; the provided ticket snapshot remained the authoritative persisted ticket context because prior gicket read commands for live ticket, comment, and relation data were trust-blocked.

### Scope In
- Extend Oracle provider read support so AddDVaultOracle() exposes an Oracle latest-satellite strategy within the existing IDataVaultReadService and IDataVaultReadDiagnosticsService boundary.
- Reuse the existing Oracle provider read-strategy path so supported Oracle latest-satellite current and as-of requests can select OracleDataVaultReadStrategy for the same bounded latest-satellite shape already documented for SQL Server and SQLite.
- Add or update unit, parity, diagnostics, registration, and integration tests that prove Oracle latest-satellite selection, provider-neutral parity, and finite fallback behavior without changing PIT or bridge semantics.
- Update benchmark guidance and repository evidence docs that currently encode Oracle latest-satellite as a no-strategy capability gap so they instead reflect a planned or diagnostics-gated Oracle latest-satellite strategy with skipped-placeholder timing allowed when Oracle is not configured.

### Scope Out
- Broadening latest-satellite support beyond hub-parent, non-multi-active current or as-of requests.
- Changing Oracle PIT or bridge behavior, save-strategy behavior, or introducing staged Oracle save or bulk work.
- Claiming completed Oracle timing evidence, release publication, or consumer release-note work when Oracle benchmark execution is still unconfigured.
- Implementing PostgreSQL, MySQL, or DB2 latest-satellite provider strategies as part of this ticket.

## Acceptance Criteria
- With AddDVaultOracle() registered and an Oracle provider context in use, IDataVaultReadDiagnosticsService selects OracleDataVaultReadStrategy for supported DataVaultLatestSatelliteReadRequest shapes and preserves provider-neutral fallback for provider mismatch, unsupported satellite parents, and multi-active driving-key satellites.
- Oracle latest-satellite current and as-of reads return the same rows and typed projections as the provider-neutral read path for supported shapes, with repository tests covering both semantics.
- Oracle latest-satellite gate metadata becomes first-class repository evidence: registration tests, gate-requirement or fallback tests, and diagnostics tests all surface the finite latest-satellite requirements and fallback causes for Oracle.
- Benchmark expectation surfaces are updated so the Oracle latest-satellite-read row records readShape=LatestSatellite and OracleDataVaultReadStrategy as the planned or selected provider strategy instead of a no-strategy fallback-only posture; skipped rows remain visible with normalized skip reasons when Oracle is not configured.
- Repository evidence documents and guidance that currently classify Oracle latest-satellite as a capability gap are updated consistently to the post-implementation evidence posture, with no checked-in document still claiming Oracle latest-satellite has no provider-specific strategy.

## Definition of Done
- Affected Oracle latest-satellite code, tests, diagnostics surfaces, benchmark expectation or verifier surfaces, and evidence or guidance docs are updated together and pass repository validation.
- No existing SQLite or SQL Server latest-satellite behavior regresses, and existing Oracle PIT or bridge candidate behavior remains intact.
- If Oracle is not configured in the validation environment, checked-in evidence still preserves a truthful skipped-placeholder posture and does not fabricate completed Oracle timing results.
- If implementation cannot satisfy the bounded parity and fallback contract without widening supported shapes, the ticket stops at explicit no-work-required evidence instead of silently broadening the Oracle read contract.

## Implementation Notes
- Repository structure already provides the narrow implementation path: SqlServerDataVaultReadStrategy and SqliteDataVaultReadStrategy show the latest-satellite pattern, while OracleDataVaultReadStrategy already owns Oracle quoting and parameter behavior for PIT and bridge reads.
- The Oracle latest-satellite gate should mirror the existing latest-satellite rules already used in DataVaultProviderReadStrategyGateEvaluator: Oracle provider-name match only, hub-parent satellite only, and no multi-active driving keys.
- Registration expectations currently prove Oracle PIT and bridge only; this ticket should update provider registration tests and any diagnostics candidate metadata that assume Oracle lacks a latest-satellite strategy.
- Benchmark and docs alignment is part of the work, not a follow-up cleanup: BenchmarkScenarioExecutionTests, the benchmark README, docs/performance-profiles.md, and the provider evidence and gap matrices currently encode the fallback-only Oracle latest-satellite posture and need consistent updates.
- The branch diff versus the supplied scratch source ref was empty at inspection time, so this refinement assumes no in-progress Oracle latest-satellite implementation is already landed on the ticket branch.

## Open Questions
- none

## Follow-Up Questions
- After this capability gap is closed, should a separate evidence ticket collect provider-configured Oracle latest-satellite timing artifacts so Oracle can move from planned-strategy posture to completed timing evidence?
- When the next release-note ticket is created beyond the current checked-in v0.40.0 baseline, which release or backlog ticket should own publishing the Oracle latest-satellite posture change outside the benchmark and evidence docs updated here?

## Risks
- Oracle benchmark execution may remain skipped in unattended validation if DVAULT_TEST_ORACLE_CONNECTION_STRING is unset, so the ticket must avoid overclaiming completed Oracle timing evidence.
- Docs drift is likely unless all evidence surfaces move together; multiple checked-in files currently state that Oracle latest-satellite has no provider-specific strategy.
- If Oracle-specific SQL behavior cannot preserve provider-neutral latest or as-of parity within the existing narrow shape, the safe outcome is explicit no-work-required documentation rather than a widened or partially specified strategy contract.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Use v0.39 evidence and v0.41 criteria to implement or reject an Oracle latest-satellite read strategy improvement. Acceptance: tests, diagnostics, fallback, and benchmark evidence are updated, or no-work-required is documented.