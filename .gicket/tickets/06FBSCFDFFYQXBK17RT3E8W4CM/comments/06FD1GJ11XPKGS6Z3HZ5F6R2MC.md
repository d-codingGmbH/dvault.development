[gicket-bot] PO refinement contract

Summary
- Verified from the checked-out repository and local .gicket state that PostgreSQL latest-satellite remains a P0 capability gap with no provider-specific strategy registered; this ticket is now bounded to either add that strategy with diagnostics/tests/benchmark proof or explicitly close as no-work-required under the existing provider-neutral fallback baseline.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The done criteria story 06FBSCF61N0TYPYH7008TRD6VR already answers the main PO question: non-SQLite latest-satellite tickets may close as no-work-required unless a new provider-specific strategy is registered, diagnostics select it, and completed timing evidence proves it.
- The repository already classifies PostgreSQL latest-satellite as capability gap P0.01, not as a PIT/bridge evidence gap: docs/plans/provider-optimization-gap-matrix.md and docs/plans/provider-optimization-evidence-matrix.md both record providerSpecificReadStrategy=not registered for latest satellite reads for PostgreSQL.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this pass.

Scope In
- Decide this single provider lane only: PostgreSQL latest-satellite-read either gains a provider-specific optimized path or is explicitly closed as no-work-required against the current fallback baseline.
- If implementation is chosen, cover only PostgreSQL latest-satellite strategy selection, fallback, diagnostics, tests, and benchmark evidence needed to justify that one lane.
- Document the closure rationale so downstream docs/benchmark ticket 06FBSCHBJEYYERDPA7JN34Y8PG can publish the outcome without reopening acceptance rules.

Scope Out
- PostgreSQL PIT or bridge read work, which already sits on the separate diagnostics-gated candidate lane.
- SQL Server, MySQL, Oracle, and DB2 latest-satellite tickets, which remain sibling provider tasks.
- Any provider performance claim based only on skipped-placeholder, diagnostics-only, smoke-only, or storage-footprint evidence.
- Automatic maintenance, raw SQL exposure, physical-plan guarantees, or broader provider platform behavior promises.

Open questions
- none

Follow-up questions
- After this ticket closes, should 06FBSCHBJEYYERDPA7JN34Y8PG publish the outcome as an implemented PostgreSQL optimization or as a documented no-work-required fallback confirmation?
- If product later wants non-SQLite latest-satellite work beyond PostgreSQL, should the remaining priority stay the current gap-matrix order: SQL Server, MySQL, Oracle, then DB2?

Risks
- The current repository baseline strongly supports no-work-required; attempting an implementation without provider-configured benchmark evidence risks overclaiming PostgreSQL latest-satellite performance.
- Mixing this ticket with PostgreSQL PIT/bridge work would violate the existing ticket split and blur a capability-gap decision into a separate evidence-gap lane.
- If optional PostgreSQL benchmark configuration is unavailable, an implemented strategy may still fail the timing-claim closure gate even if diagnostics and functional tests pass.

Split recommendations
- No new split recommended; the live graph already separates this PostgreSQL latest-satellite task from sibling provider latest-satellite tasks and the downstream read docs/benchmark ticket.
- Do not pre-split PIT/bridge or cross-provider work out of this ticket; only create a later follow-on if a concrete PostgreSQL latest-satellite implementation proves functional but still needs separately scheduled benchmark or documentation execution.

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