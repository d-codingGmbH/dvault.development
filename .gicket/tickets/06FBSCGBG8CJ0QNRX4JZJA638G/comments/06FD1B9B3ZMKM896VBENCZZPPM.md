[gicket-bot] PO refinement contract

Summary
- Using the supplied ticket snapshot plus the checked-in v0.39 evidence surfaces, the audit resolves PIT/bridge follow-up as SQLite=no-op, PostgreSQL/SQL Server/MySQL/Oracle=implement, and DB2=defer. Live ticket/comment/relation re-reads were trust-blocked, so no persistent ticket or planning writes were applied.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The checked-in gap matrix already classifies PIT rows P2.01-P2.05 and bridge rows P3.01-P3.05 as evidence gaps, not missing runtime strategy implementations.
- SQLite PIT/bridge is a no-op for this audit because the matrix keeps SQLite as the completed-timing reference baseline rather than an open backlog gap.
- PostgreSQL, SQL Server, MySQL, and Oracle PIT/bridge should be marked implement for v0.41 follow-up planning because the repo already registers provider read-strategy candidates and the missing deliverable is provider-configured timing evidence.
- DB2 PIT/bridge should be marked defer because the repo keeps DB2 at diagnostics-only and smoke-only plus skipped timing placeholders and explicitly rejects promoting that posture into completed timing claims.

Scope In
- Audit PIT as-of and bridge traversal read gaps across the repository-supported providers using the gap matrix, evidence matrix, benchmark triplet, and PIT/bridge boundary docs as the authoritative sources.
- Record one explicit provider decision for PIT and bridge follow-up planning: SQLite no-op; PostgreSQL implement; SQL Server implement; MySQL implement; Oracle implement; DB2 defer.
- Carry forward the existing finite stop conditions for any later implementation ticket: explicit PIT/bridge maintenance required, complete read-shape evidence required, and stale maintenance or unsupported shapes fall back to provider-neutral reads.

Scope Out
- Runtime code changes to PIT or bridge readers, provider dispatch, telemetry, or maintenance services.
- Benchmark reruns, external database provisioning, credential setup, or new measured provider timing claims in this ticket.
- Non-SQLite latest-satellite strategy work, save-strategy gap work, or DB2 boundary expansion beyond the current v0.34 posture.

Open questions
- none

Follow-up questions
- Which external-provider environments should the team keep available for repeat PIT/bridge benchmark reruns after the first v0.41 evidence pass?
- If DB2 timing work is later approved, should it remain evidence-only against the current clean-context PIT/bridge candidate posture, or should that approval explicitly reopen the narrower v0.34 DB2 boundary first?

Risks
- All external-provider PIT and bridge rows remain blocked on configured connection strings and benchmark infrastructure; implement tickets can stall even though the repository-side strategy candidates already exist.
- A downstream ticket could accidentally widen skipped placeholders or DB2 smoke evidence into performance claims unless it keeps the evidence-matrix posture rules intact.
- Live ticket relation state was not re-verified in this run because the bounded ticket-read tools were trust-blocked, so any hidden dependency cleanup would need a later ticket-surface pass.

Split recommendations
- Keep SQLite out of any follow-up split and open no PIT/bridge child work for it.
- Split PostgreSQL plus SQL Server into one evidence ticket covering PIT and bridge timing proof for their already-registered strategies, because both share the same diagnostics-gated relational posture and highest current priority order.
- Split MySQL plus Oracle into a second evidence ticket covering PIT and bridge timing proof for their already-registered strategies while preserving the same explicit-maintenance and fallback boundaries.
- Keep DB2 out of the v0.41 implementation batch and open a separate deferred planning ticket only if the team later approves DB2 environment-backed timing work.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment