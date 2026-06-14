[gicket-bot] PO refinement contract

Summary
- Refined this ticket to the existing SQLite-local hash-key storage evidence baseline; no new PO blockers were found and no bounded writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The representative provider scenario is ratified to the existing SQLite local temporary files baseline; current repository evidence already scopes the visible hash-key storage bundle there.
- Use the repository storage-profile vocabulary `HexString` and `Binary`; this ticket does not introduce a new public profile name or a `byte[]` hash-key boundary.
- The current repository already contains the compliant benchmark bundle at `artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-<redacted>/` plus release/adoption pointers in `docs/releases/v0.36.0.md` and `hash-key-footprint.md`.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized during this refinement because the existing evidence already bounds the ticket.

Scope In
- Confirm or refresh benchmark evidence that compares `HexString` and `Binary` storage within the existing SQLite local temporary files benchmark lane.
- Keep the benchmark artifact triplet and matching footprint sidecars under one checked-in benchmark label.
- Preserve the standard local scenario rows already required by the performance-evidence contract, including save, streaming-save, latest-satellite, PIT, bridge, and latest-lookup coverage.
- Update release/adoption documentation only as needed to point at the checked-in bundle and restate the storage-profile caveats.

Scope Out
- Adding a new cross-provider benchmark matrix or claiming PostgreSQL, SQL Server, MySQL, Oracle, or DB2 results from this ticket.
- Changing the default storage profile, stable-hash defaults, or the public logical hash-key representation.
- Automatic rehashing, backfill, dual-write, migration tooling, or persisted-key repair.
- Replacing the v1 benchmark harness or inventing a ticket-specific artifact schema.

Open questions
- none

Follow-up questions
- After the SQLite-local bundle is accepted, decide separately whether a provider-specific follow-up benchmark is needed for one non-SQLite provider instead of broadening this ticket.
- If a later change needs before/after regression claims rather than side-by-side storage variants, decide whether to create a distinct benchmark label with `before/` and `after/` artifact sets.

Risks
- The current measured evidence is SQLite-local only, so any broader provider-performance conclusion would exceed the verified repository baseline.
- Hash algorithm or storage-profile changes after data already exists remain caller-owned compatibility work; careless wording could overpromise migration behavior.
- Downstream sequencing still depends on the existing relation chain because this ticket is currently blocked by one ticket and blocks two others.

Split recommendations
- No split recommended; current repository evidence already bounds the work to one representative SQLite provider scenario plus aligned release/adoption documentation.

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