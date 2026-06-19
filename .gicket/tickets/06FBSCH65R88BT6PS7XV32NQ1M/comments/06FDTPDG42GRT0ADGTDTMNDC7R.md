[gicket-bot] PO refinement contract

Summary
- Refined the DB2 PIT/bridge ticket as a defer/no-work-required evidence gate: repository evidence, automation-only comment state, and live relations all support holding DB2 timing claims behind explicit environment approval, with no child tickets or relation/document writes needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current ticket description already matches the repository-backed posture: DB2 PIT and bridge candidate behavior exists, but active implementation and timing-claim work stay deferred until explicit environment-backed approval lands.
- Root benchmark rows for DB2 pit-as-of-read and bridge-traversal-read are present only as skipped-placeholder rows because DVAULT_TEST_DB2_CONNECTION_STRING is unset; they preserve row identity and planned Db2DataVaultReadStrategy selection but do not prove timing.
- Repository evidence keeps DB2 latest-satellite separate from this ticket: benchmark-summary.md records providerSpecificReadStrategy=not registered for latest satellite reads, and the gap matrix keeps DB2 latest-satellite as a capability gap rather than part of this PIT/bridge evidence ticket.
- The current comments are automation-only, including one relation-follow-up from audit ticket 06FBSCGBG8CJ0QNRX4JZJA638G; there are no human comments, attachments, or closure-evidence amendments adding new scope.
- Live relation state was verified and retained unchanged: audit ticket 06FBSCGBG8CJ0QNRX4JZJA638G blocks this ticket, and this ticket blocks documentation ticket 06FBSCHBJEYYERDPA7JN34Y8PG.

Scope In
- Ratify DB2 PIT and bridge work as a deferred evidence lane, not an active provider-read implementation slice.
- Document the bounded evidence sources allowed for this ticket: skipped-placeholder benchmark rows, diagnostics-gated strategy registration, and opt-in DB2 smoke coverage.
- Define what a future DB2 activation proposal must supply before any completed DB2 PIT or bridge timing claim can be accepted.

Scope Out
- Adding or changing DB2 PIT/bridge code, diagnostics gates, or benchmarks in this ticket.
- Claiming completed DB2 PIT or bridge timing from diagnostics-only, smoke-only, or skipped-placeholder evidence.
- Expanding DB2 latest-satellite optimization, staged DB2 bulk, provider-native chunk execution, or DB2 live-schema reading.
- Cleaning up the existing audit or documentation relations before this ticket is formally closed or re-scoped.

Open questions
- none

Follow-up questions
- If product later activates DB2 PIT/bridge timing work, which specific DB2 environment and owner will be approved for the benchmark and smoke evidence lane?
- When that activation is considered, should the team reopen the narrower v0.34 DB2 boundary before allowing any broader DB2 provider-read claims beyond PIT/bridge candidate timing?
- When the provider-outcome set is complete, should the runtime close or re-scope the blocks link from this ticket to 06FBSCHBJEYYERDPA7JN34Y8PG in the same change set?

Risks
- The main contract risk is overclaiming DB2 performance from smoke-only, diagnostics-only, or skipped-placeholder evidence; the checked-in repo explicitly disallows that promotion.
- Future DB2 evidence work depends on an opt-in external environment and may stall without an approved connection-string-backed benchmark lane.
- Because DB2 keeps the narrower v0.34 boundary, later tickets can accidentally mix PIT/bridge timing follow-up with out-of-scope DB2 latest-satellite or broader provider-expansion work.

Split recommendations
- No split is recommended now; the current evidence already justifies a defer/no-work-required refinement rather than more child tickets.
- If DB2 evidence work is later approved, create a new follow-up ticket for the approved environment-backed benchmark run instead of widening this ticket into mixed implementation and evidence scope.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment