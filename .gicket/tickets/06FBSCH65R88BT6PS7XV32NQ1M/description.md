<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the DB2 PIT/bridge ticket as a defer/no-work-required evidence gate: repository evidence, automation-only comment state, and live relations all support holding DB2 timing claims behind explicit environment approval, with no child tickets or relation/document writes needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The current ticket description already matches the repository-backed posture: DB2 PIT and bridge candidate behavior exists, but active implementation and timing-claim work stay deferred until explicit environment-backed approval lands.
- Root benchmark rows for DB2 pit-as-of-read and bridge-traversal-read are present only as skipped-placeholder rows because DVAULT_TEST_DB2_CONNECTION_STRING is unset; they preserve row identity and planned Db2DataVaultReadStrategy selection but do not prove timing.
- Repository evidence keeps DB2 latest-satellite separate from this ticket: benchmark-summary.md records providerSpecificReadStrategy=not registered for latest satellite reads, and the gap matrix keeps DB2 latest-satellite as a capability gap rather than part of this PIT/bridge evidence ticket.
- The current comments are automation-only, including one relation-follow-up from audit ticket 06FBSCGBG8CJ0QNRX4JZJA638G; there are no human comments, attachments, or closure-evidence amendments adding new scope.
- Live relation state was verified and retained unchanged: audit ticket 06FBSCGBG8CJ0QNRX4JZJA638G blocks this ticket, and this ticket blocks documentation ticket 06FBSCHBJEYYERDPA7JN34Y8PG.

### Scope In
- Ratify DB2 PIT and bridge work as a deferred evidence lane, not an active provider-read implementation slice.
- Document the bounded evidence sources allowed for this ticket: skipped-placeholder benchmark rows, diagnostics-gated strategy registration, and opt-in DB2 smoke coverage.
- Define what a future DB2 activation proposal must supply before any completed DB2 PIT or bridge timing claim can be accepted.

### Scope Out
- Adding or changing DB2 PIT/bridge code, diagnostics gates, or benchmarks in this ticket.
- Claiming completed DB2 PIT or bridge timing from diagnostics-only, smoke-only, or skipped-placeholder evidence.
- Expanding DB2 latest-satellite optimization, staged DB2 bulk, provider-native chunk execution, or DB2 live-schema reading.
- Cleaning up the existing audit or documentation relations before this ticket is formally closed or re-scoped.

## Acceptance Criteria
- The ticket explicitly states that DB2 PIT and bridge work stays out of the active implementation batch unless the team approves environment-backed evidence work beyond the current diagnostics-only, smoke-only, and skipped-placeholder posture.
- The contract cites the checked-in DB2 PIT and bridge benchmark rows as row-identity and planned-strategy evidence only, not completed timing evidence.
- The contract distinguishes DB2 PIT/bridge candidate behavior from DB2 latest-satellite work and does not treat PIT/bridge smoke evidence as proof of a DB2 latest-satellite optimization.
- Any future activation proposal must identify the approved DB2 environment, the benchmark artifact triplet required for PIT and bridge timing claims, and whether the narrower v0.34 DB2 boundary must reopen first.
- Downstream documentation for 06FBSCHBJEYYERDPA7JN34Y8PG must keep DB2 PIT and bridge claims in the defer/no-completed-timing lane until provider-configured benchmark evidence exists.

## Definition of Done
- PO-critic can review this ticket without reopening provider names, read shapes, or evidence-posture vocabulary.
- The ticket records that no code change, benchmark rerun, attachment, child-ticket split, planning document, or relation change is required in the current pass.
- The accepted contract keeps DB2 PIT/bridge candidate registration and smoke coverage distinct from completed timing evidence and preserves the narrower DB2 boundary already documented in v0.34.
- The live relation state remains consistent with the refined contract: upstream audit stays linked and downstream documentation stays blocked pending the provider-outcome set.

## Implementation Notes
- docs/plans/provider-optimization-gap-matrix.md already classifies DB2 pit-as-of-read and bridge-traversal-read as evidence gaps (P2.05 and P3.05) with diagnostics-only/smoke-only candidate posture and skipped-placeholder root rows.
- docs/plans/provider-optimization-evidence-matrix.md is the authoritative posture source: DB2 PIT/bridge rows are not completed timing evidence, and DB2 latest-satellite remains unregistered for provider-specific optimization.
- benchmark-summary.md shows DB2 pit-as-of-read and bridge-traversal-read skipped because DVAULT_TEST_DB2_CONNECTION_STRING is unset, while still naming Db2DataVaultReadStrategy as the planned PIT/bridge strategy.
- docs/releases/v0.34.0.md and tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs bound DB2 to provider-neutral latest-satellite reads plus diagnostics-gated PIT/bridge read dispatch and opt-in smoke coverage when configured.
- src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs registers Db2DataVaultReadStrategy only for PIT and bridge, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs plus DataVaultRelationalPitBridgeReadStrategyParityTests.cs verify accepted shapes, fail-closed fallback, and provider-neutral parity.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized in this refinement pass.

## Open Questions
- none

## Follow-Up Questions
- If product later activates DB2 PIT/bridge timing work, which specific DB2 environment and owner will be approved for the benchmark and smoke evidence lane?
- When that activation is considered, should the team reopen the narrower v0.34 DB2 boundary before allowing any broader DB2 provider-read claims beyond PIT/bridge candidate timing?
- When the provider-outcome set is complete, should the runtime close or re-scope the blocks link from this ticket to 06FBSCHBJEYYERDPA7JN34Y8PG in the same change set?

## Risks
- The main contract risk is overclaiming DB2 performance from smoke-only, diagnostics-only, or skipped-placeholder evidence; the checked-in repo explicitly disallows that promotion.
- Future DB2 evidence work depends on an opt-in external environment and may stall without an approved connection-string-backed benchmark lane.
- Because DB2 keeps the narrower v0.34 boundary, later tickets can accidentally mix PIT/bridge timing follow-up with out-of-scope DB2 latest-satellite or broader provider-expansion work.

## Split Recommendations
- No split is recommended now; the current evidence already justifies a defer/no-work-required refinement rather than more child tickets.
- If DB2 evidence work is later approved, create a new follow-up ticket for the approved environment-backed benchmark run instead of widening this ticket into mixed implementation and evidence scope.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Deferred planning ticket for DB2 PIT/bridge timing evidence. Keep DB2 out of the active implementation batch unless the team explicitly approves environment-backed evidence work beyond the current diagnostics-only, smoke-only, and skipped-placeholder posture. Acceptance: any later activation proposal identifies the approved DB2 environment, the benchmark artifact set required for PIT and bridge timing claims, and whether the narrower v0.34 DB2 boundary must reopen first; until that approval lands, no code change, benchmark rerun, or completed timing claim is required here.