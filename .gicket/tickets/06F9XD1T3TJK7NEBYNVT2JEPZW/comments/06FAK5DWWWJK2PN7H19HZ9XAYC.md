[gicket-bot] PO refinement contract

Summary
- Ratified 06F9XD1T3TJK7NEBYNVT2JEPZW as a tracking parent over one completed evidence task and three completed provider-specific calibration tasks; the v0.32.0 all-provider Podman bundle is the authoritative baseline, no new split was justified, and no bounded planning write was applied.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The live relation set already matches the intended split: the story has outgoing parentOf links to 06F9XD26D2MHVAKZ2GCZ67BEFC, 06F9XD2M71D1XFT7FJX62KD8HM, 06F9XD2TGEYEG6S0AK86YF295M, and 06F9XD33MNNVHHW232TC7T1CN8, plus one incoming parent relation from 06F8KZTCEMNNFBFTVMFXEN268M; no relation cleanup was required in this pass.
- All four child tickets above are already done, so no additional child split is justified for this story.
- Treat artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-<redacted>/ as the authoritative threshold-tuning baseline and artifacts/benchmarks/v0.31.0-scale-5-all-providers-<redacted> as historical seed evidence when they conflict.
- The story's decision surface is now bounded by the completed child contracts: SQL Server centers on the 50-minimum/500-satellite gate plus accurate fallback wording, Oracle centers on the 50-minimum/10000-satellite direct-batching boundary, PostgreSQL stays on current eligibility unless a fresh small-batch regression is reproduced, and MySQL stays bounded to tiny-workload eligibility plus diagnostic clarity.
- No child-ticket creation, relation mutation, description update, attachment, or planning-document write was materialized in this refinement pass.

Scope In
- Keep this story as the aggregation surface for the completed all-provider baseline evidence and the provider-specific threshold-calibration outcomes across SQL Server, Oracle, PostgreSQL, and MySQL.
- Ratify or adjust only bounded provider save-strategy thresholds, eligibility rules, and benchmark/diagnostic wording that are backed by before/after evidence or an explicit evidence-backed no-change decision under the shared artifact contract.
- Preserve EF Core-facing save semantics across every provider-specific decision, including transactions, cancellation, ordering, hash keys/hash diffs, load timestamps, idempotency, and actionable fallback diagnostics.
- Reuse the existing Podman-backed provider lanes for evidence capture, including PostgreSQL access through the Podman network when a benchmark runs inside an SDK container.

Scope Out
- Adding DB2 or any provider lane beyond SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.
- Automatic stored-procedure deployment, database administration automation, or new runtime orchestration surfaces.
- Replacing IDataVaultSaveService, redefining the shared benchmark artifact contract, or widening the work into an unbounded cross-provider redesign.
- Promoting root benchmark-summary.md / .csv / .json rollups or broader release-note wording unless a follow-up documentation ticket explicitly owns that lift.

Open questions
- none

Follow-up questions
- Should the downstream documentation or release task lift the final calibrated provider thresholds and fallback wording into the v0.32.0 public posture once this story closes?
- After all provider-specific changes are merged, should a separate artifact-lane follow-up decide whether any completed all-provider rows belong in the root benchmark-summary rollup or whether that rollup should remain a lightweight shared baseline?
- If later reruns expose fresh instability outside the bounded SQL Server, Oracle, PostgreSQL, and MySQL decisions already captured here, should that be handled as new evidence-only follow-up tickets instead of reopening this parent story?

Risks
- The root checked-in benchmark-summary rollup can still lag the ticket-specific v0.32.0 evidence bundle, so downstream readers may cite the wrong baseline unless documentation points at the calibrated child evidence explicitly.
- Benchmark execution-detail wording can still mislead release-note or documentation consumers if provider-specific planned-path labels drift away from actual diagnostics and fallback state.
- Reopening this parent story for broader provider-policy work would collapse boundaries that were intentionally split into finished child tickets and make later evidence harder to interpret.

Split recommendations
- No additional split is justified. The story already owns one completed baseline-evidence task and three completed provider-specific calibration tasks that cover the bounded decision surfaces raised by the original ticket.
- If future evidence introduces a materially new provider-specific boundary or a documentation-only release-posture gap, create a new follow-up ticket instead of reopening this parent story.

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