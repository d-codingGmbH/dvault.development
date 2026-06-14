<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this ticket to the existing SQLite-local hash-key storage evidence baseline; no new PO blockers were found and no bounded writes were needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The representative provider scenario is ratified to the existing SQLite local temporary files baseline; current repository evidence already scopes the visible hash-key storage bundle there.
- Use the repository storage-profile vocabulary `HexString` and `Binary`; this ticket does not introduce a new public profile name or a `byte[]` hash-key boundary.
- The current repository already contains the compliant benchmark bundle at `artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/` plus release/adoption pointers in `docs/releases/v0.36.0.md` and `hash-key-footprint.md`.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized during this refinement because the existing evidence already bounds the ticket.

### Scope In
- Confirm or refresh benchmark evidence that compares `HexString` and `Binary` storage within the existing SQLite local temporary files benchmark lane.
- Keep the benchmark artifact triplet and matching footprint sidecars under one checked-in benchmark label.
- Preserve the standard local scenario rows already required by the performance-evidence contract, including save, streaming-save, latest-satellite, PIT, bridge, and latest-lookup coverage.
- Update release/adoption documentation only as needed to point at the checked-in bundle and restate the storage-profile caveats.

### Scope Out
- Adding a new cross-provider benchmark matrix or claiming PostgreSQL, SQL Server, MySQL, Oracle, or DB2 results from this ticket.
- Changing the default storage profile, stable-hash defaults, or the public logical hash-key representation.
- Automatic rehashing, backfill, dual-write, migration tooling, or persisted-key repair.
- Replacing the v1 benchmark harness or inventing a ticket-specific artifact schema.

## Acceptance Criteria
- A checked-in evidence label contains `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` for the selected hash-key storage comparison run.
- Because the claim varies hash-key storage profile, the same label also contains `hash-key-footprint.md`, `hash-key-footprint.csv`, and `hash-key-footprint.json` sidecars.
- The run context records SQLite local temporary files as the required provider, `sqlite` as the provider filter, the preserved iteration, warmup, runtime, and hash-key-variant fields, and does not imply the exercised variants.
- The evidence keeps the standard local scenario baseline visible, with deterministic `executionDetail` fields and contract-compliant completed/skipped semantics.
- Documentation or release evidence that references the bundle explicitly states that `HexString` remains the compatible default, `Binary` is explicit opt-in physical storage, and the measured claims stay scoped to the preserved SQLite-local run context.

## Definition of Done
- The repository contains the finalized benchmark bundle and any supporting doc updates in their normal checked-in paths.
- The benchmark triplet and footprint sidecars tell one consistent story about `HexString` versus `Binary` without widening the claim beyond the preserved provider and run context.
- Any refreshed bundle preserves or explicitly justifies the currently visible variant set instead of silently dropping rows from the existing evidence baseline.
- No ticket text or docs imply automatic migration or cross-provider guarantees that the repository evidence does not support.

## Implementation Notes
- Reuse the existing evidence pattern under `artifacts/benchmarks/<label>/`; the current repository baseline is `artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/`.
- Treat `docs/plans/performance-evidence-benchmark-artifact-contract.md` as the artifact contract and `docs/plans/hash-key-storage-profile-contract.md` as the storage-profile authority.
- Keep adopter-facing pointers aligned with `docs/releases/v0.36.0.md` and the root `hash-key-footprint.md` summary instead of duplicating raw benchmark tables into multiple docs.
- The current visible bundle already carries `sha256-v1-hex`, `sha256-v1-binary`, `sha256-128-v1-hex`, and `sha256-128-v1-binary`; narrowing that baseline should require an explicit rationale, not an accidental omission.
- Live relations currently show this ticket blocked by `06FBSBZY1XEJYK1DRV4RV2ZN88` and blocking `06FBSC0TMZBXVVECGQGESWPCY4` plus `06FBSC40N01AH5PRZ1QNKRVTWR`; no relation cleanup was applied in this refinement.

## Open Questions
- none

## Follow-Up Questions
- After the SQLite-local bundle is accepted, decide separately whether a provider-specific follow-up benchmark is needed for one non-SQLite provider instead of broadening this ticket.
- If a later change needs before/after regression claims rather than side-by-side storage variants, decide whether to create a distinct benchmark label with `before/` and `after/` artifact sets.

## Risks
- The current measured evidence is SQLite-local only, so any broader provider-performance conclusion would exceed the verified repository baseline.
- Hash algorithm or storage-profile changes after data already exists remain caller-owned compatibility work; careless wording could overpromise migration behavior.
- Downstream sequencing still depends on the existing relation chain because this ticket is currently blocked by one ticket and blocks two others.

## Split Recommendations
- No split recommended; current repository evidence already bounds the work to one representative SQLite provider scenario plus aligned release/adoption documentation.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add or update focused benchmark evidence comparing binary-first and hex-compatible storage in at least one representative provider scenario. Acceptance: results are stored under artifacts/benchmarks or documented release evidence and include caveats.