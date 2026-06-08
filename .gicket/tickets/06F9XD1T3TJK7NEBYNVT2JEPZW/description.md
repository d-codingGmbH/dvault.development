<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Ratified 06F9XD1T3TJK7NEBYNVT2JEPZW as a tracking parent over one completed evidence task and three completed provider-specific calibration tasks; the v0.32.0 all-provider Podman bundle is the authoritative baseline, no new split was justified, and no bounded planning write was applied.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The live relation set already matches the intended split: the story has outgoing parentOf links to 06F9XD26D2MHVAKZ2GCZ67BEFC, 06F9XD2M71D1XFT7FJX62KD8HM, 06F9XD2TGEYEG6S0AK86YF295M, and 06F9XD33MNNVHHW232TC7T1CN8, plus one incoming parent relation from 06F8KZTCEMNNFBFTVMFXEN268M; no relation cleanup was required in this pass.
- All four child tickets above are already done, so no additional child split is justified for this story.
- Treat artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-20260607/ as the authoritative threshold-tuning baseline and artifacts/benchmarks/v0.31.0-scale-5-all-providers-20260606 as historical seed evidence when they conflict.
- The story's decision surface is now bounded by the completed child contracts: SQL Server centers on the 50-minimum/500-satellite gate plus accurate fallback wording, Oracle centers on the 50-minimum/10000-satellite direct-batching boundary, PostgreSQL stays on current eligibility unless a fresh small-batch regression is reproduced, and MySQL stays bounded to tiny-workload eligibility plus diagnostic clarity.
- No child-ticket creation, relation mutation, description update, attachment, or planning-document write was materialized in this refinement pass.

### Scope In
- Keep this story as the aggregation surface for the completed all-provider baseline evidence and the provider-specific threshold-calibration outcomes across SQL Server, Oracle, PostgreSQL, and MySQL.
- Ratify or adjust only bounded provider save-strategy thresholds, eligibility rules, and benchmark/diagnostic wording that are backed by before/after evidence or an explicit evidence-backed no-change decision under the shared artifact contract.
- Preserve EF Core-facing save semantics across every provider-specific decision, including transactions, cancellation, ordering, hash keys/hash diffs, load timestamps, idempotency, and actionable fallback diagnostics.
- Reuse the existing Podman-backed provider lanes for evidence capture, including PostgreSQL access through the Podman network when a benchmark runs inside an SDK container.

### Scope Out
- Adding DB2 or any provider lane beyond SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.
- Automatic stored-procedure deployment, database administration automation, or new runtime orchestration surfaces.
- Replacing IDataVaultSaveService, redefining the shared benchmark artifact contract, or widening the work into an unbounded cross-provider redesign.
- Promoting root benchmark-summary.md / .csv / .json rollups or broader release-note wording unless a follow-up documentation ticket explicitly owns that lift.

## Acceptance Criteria
- The story cites one authoritative pre-tuning evidence source: the completed v0.32.0 all-provider Podman bundle from child task 06F9XD26D2MHVAKZ2GCZ67BEFC, with provider-visible benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json artifacts that follow the shared benchmark contract.
- The story resolves the original threshold surface into four bounded outcomes: baseline evidence captured; SQL Server 50-minimum/500-satellite decision plus diagnostic clarity; Oracle 50-minimum/10000-satellite decision plus direct-batching posture; PostgreSQL/MySQL small-batch posture with PostgreSQL change gated on reproduced regression and MySQL bounded to tiny-workload calibration.
- Any threshold change or explicit no-change decision is backed by comparable before/after evidence or an explicit evidence-backed no-change rationale, and benchmark/report output clearly distinguishes provider strategy selection, staged decline, and provider-neutral fallback.
- The calibrated provider posture preserves transactions, cancellation, ordering, hash keys/hash diffs, load timestamps, idempotency, and fallback diagnostics across the affected providers.
- No further split is introduced unless later evidence opens a materially new provider-specific boundary that is not already covered by the completed child tickets.

## Definition of Done
- The parent story is a clean tracking surface over completed child tickets 06F9XD26D2MHVAKZ2GCZ67BEFC, 06F9XD2M71D1XFT7FJX62KD8HM, 06F9XD2TGEYEG6S0AK86YF295M, and 06F9XD33MNNVHHW232TC7T1CN8.
- The contract records the evidence hierarchy without reopening it: the 2026-06-07 v0.32.0 all-provider bundle is the live baseline, and the 2026-06-06 v0.31.0 scale bundle is comparison context only.
- The story-level posture is explicit enough for downstream documentation and release work to lift without reopening which provider boundaries stayed unchanged, which were tuned, or how fallback was reported.
- No PO-blocking questions remain for the next workflow step; any remaining release-note or artifact-rollup decisions are downstream follow-up rather than blockers.

## Implementation Notes
- Use child task 06F9XD26D2MHVAKZ2GCZ67BEFC as the authoritative evidence owner for artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-20260607/ and for the shared benchmark-summary.md / .csv / .json contract.
- Use child task 06F9XD2M71D1XFT7FJX62KD8HM for the SQL Server boundary: preserve the public fallback vocabulary around SqlServerMinimumOperationThreshold and SqlServerMaximumSatelliteOperationThreshold and keep benchmark detail aligned with whether fallback or provider-native execution actually occurred.
- Use child task 06F9XD2TGEYEG6S0AK86YF295M for the Oracle boundary: direct batching remains the repository-proven v1 baseline, stagedOracleBulk=not-selected-no-measured-win remains intentional until fresh evidence proves otherwise, and the 10000-satellite ceiling stays evidence-driven.
- Use child task 06F9XD33MNNVHHW232TC7T1CN8 for PostgreSQL/MySQL: treat the 2026-06-07 v0.32.0 bundle as authoritative when it conflicts with 2026-06-06 seed evidence, keep PostgreSQL threshold changes gated on reproduced regressions, and keep MySQL calibration bounded to tiny-workload eligibility and diagnostic clarity.
- No child-ticket creation, relation cleanup, description mutation, attachment binding, or planning-document write was applied during this refinement pass.

## Open Questions
- none

## Follow-Up Questions
- Should the downstream documentation or release task lift the final calibrated provider thresholds and fallback wording into the v0.32.0 public posture once this story closes?
- After all provider-specific changes are merged, should a separate artifact-lane follow-up decide whether any completed all-provider rows belong in the root benchmark-summary rollup or whether that rollup should remain a lightweight shared baseline?
- If later reruns expose fresh instability outside the bounded SQL Server, Oracle, PostgreSQL, and MySQL decisions already captured here, should that be handled as new evidence-only follow-up tickets instead of reopening this parent story?

## Risks
- The root checked-in benchmark-summary rollup can still lag the ticket-specific v0.32.0 evidence bundle, so downstream readers may cite the wrong baseline unless documentation points at the calibrated child evidence explicitly.
- Benchmark execution-detail wording can still mislead release-note or documentation consumers if provider-specific planned-path labels drift away from actual diagnostics and fallback state.
- Reopening this parent story for broader provider-policy work would collapse boundaries that were intentionally split into finished child tickets and make later evidence harder to interpret.

## Split Recommendations
- No additional split is justified. The story already owns one completed baseline-evidence task and three completed provider-specific calibration tasks that cover the bounded decision surfaces raised by the original ticket.
- If future evidence introduces a materially new provider-specific boundary or a documentation-only release-posture gap, create a new follow-up ticket instead of reopening this parent story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Use the full external-provider benchmark run as concrete evidence for provider save strategy tuning in v0.32.0.

Context:
- Seed evidence exists under `artifacts/benchmarks/v0.31.0-scale-5-all-providers-20260606`.
- The completed matrix had 120/120 rows across SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.
- Findings to turn into bounded work:
  - SQL Server optimized rows frequently report `SqlServerMinimumOperationThreshold` or `SqlServerMaximumSatelliteOperationThreshold`.
  - Oracle `customer-profile-scale-10000x10` reports `OracleMaximumSatelliteOperationThreshold` and stays close to provider-neutral fallback.
  - PostgreSQL and MySQL have small-batch rows where the optimized path is slower than provider-neutral fallback.
- This story is not a new platform/tool-suite, not automatic stored-procedure deployment, and not DB administration automation.

Acceptance criteria:
- The work defines which provider thresholds or eligibility rules are safe to tune and which must remain documented-only.
- Any tuning decision is backed by before/after benchmark artifacts following the v0.32.0 artifact/evidence requirements.
- The test plan explicitly uses the existing Podman containers for PostgreSQL, SQL Server, MySQL, and Oracle; PostgreSQL must be reached through the Podman network, not assumed to be reachable as localhost from every runner.
- DB2 remains out of scope for this story unless the benchmark harness has first gained DB2 provider support in the DB2 release lane.
- The implementation preserves EF Core-facing library behavior: transactions, cancellation, ordering, hash keys/hash diffs, load timestamps, idempotency, and fallback diagnostics remain correct.