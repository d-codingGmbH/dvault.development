[gicket-bot] PO refinement contract

Summary
- Refined the read-parity documentation task against the checked-in evidence matrix, gap matrix, benchmark artifacts, and local `.gicket` ticket state, and wrote a ticket-bound planning note at `docs/plans/provider-read-parity-outcomes-benchmarks-refinement.md`. No child tickets, relation writes, attachments, or ticket-description mutations were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the current v1 read baseline: all provider packages now register latest-satellite read strategies, but only SQLite has completed-timing latest-satellite evidence in the root benchmark triplet.
- PostgreSQL, SQL Server, MySQL, and Oracle PIT/bridge timing is already completed and must be cited from the checked-in v0.32.0 smoke-read bundle rather than from skipped root quick-baseline rows.
- DB2 PIT/bridge stays in the defer/no-completed-timing lane: root rows are skipped placeholders and the remaining proof is diagnostics-only and smoke-only.
- No human comments or closure-evidence amendments add new blocker questions for this ticket.
- Created ticket-bound refinement note `docs/plans/provider-read-parity-outcomes-benchmarks-refinement.md` to persist the verified documentation boundary.

Scope In
- Update live documentation surfaces to align `docs/performance-profiles.md`, `docs/architecture/dvault-v1-pit-bridge-boundary.md`, and `docs/releases/v0.40.0.md` around the current provider-read evidence baseline.
- Document provider strategy availability separately from measured benchmark wins, especially for non-SQLite latest-satellite reads.
- Cite the evidence matrix and gap matrix as the row-level source of truth for scenario/provider/baseline/posture facts.
- Preserve the finite provider-neutral fallback and explicit PIT/bridge maintenance caveats already proved in code and architecture docs.

Scope Out
- Rerunning benchmarks, changing benchmark schemas, or inventing new artifact lanes.
- Changing provider read code, supported read shapes, or adding new public read APIs.
- Promoting skipped-placeholder, diagnostics-only, or smoke-only rows into completed timing claims.
- Cleaning up the historical incoming `blocks` relations from done provider-specific tickets as part of this documentation ticket.

Open questions
- none

Follow-up questions
- Which later ticket, if any, should own provider-configured latest-satellite timing collection for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 now that strategy-registration posture is documented?
- If DB2 PIT/bridge timing work is later approved, which explicit environment and benchmark artifact lane will be authoritative for promoting DB2 out of the defer/no-completed-timing lane?

Risks
- Docs can easily overclaim non-SQLite latest-satellite performance because the root benchmark rows already carry planned strategy names while remaining skipped placeholders.
- Docs drift remains likely unless the performance guide, PIT/bridge boundary note, and v0.40.0 release note are updated together against the evidence matrix and gap matrix.
- The stale incoming `blocks` relations from done tickets may confuse later workflow review if they are not cleaned up after documentation delivery.

Split recommendations
- No additional split is justified for this ticket; the current repository already provides a finite documentation baseline.
- If future work is opened, keep it split between latest-satellite timing collection and DB2 PIT/bridge environment-backed evidence activation rather than reopening this documentation ticket.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment