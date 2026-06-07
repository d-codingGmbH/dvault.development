<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the Oracle high-volume threshold task around the repository-proven 50-operation minimum, 10000-satellite maximum, direct-batching baseline, and the completed v0.32.0 all-provider Podman evidence; no split or persistent planning mutation was needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The repository already ratifies the Oracle v1 baseline as direct optimized batching only; stagedOracleBulk=not-selected-no-measured-win is the current intended state until new evidence proves otherwise.
- Oracle save-strategy gates are already finite and repository-proven in code and tests: minimum 50 total operations and maximum 10000 satellite operations.
- Done evidence ticket 06F9XD26D2MHVAKZ2GCZ67BEFC already supplies the authoritative all-provider baseline bundle at artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-20260607/; this ticket should consume that bundle instead of redefining benchmark format or provider matrix.
- Parent story 06F9XD1T3TJK7NEBYNVT2JEPZW already groups this Oracle task with sibling SQL Server and PostgreSQL/MySQL threshold-tuning tasks, so no additional child split is justified.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized in this refinement pass.

### Scope In
- Use the completed v0.32.0 all-provider Oracle artifact bundle to decide whether the current 10000-satellite cap stays, moves, or is replaced by a better bounded Oracle path.
- Benchmark and diagnose Oracle customer-profile high-volume saves at and around the current boundary, including the scenarios that currently keep provider selection at 10000 satellites and fall back at 100000 satellites.
- Keep the evaluation on the existing IDataVaultSaveService and OracleDataVaultSaveStrategy boundary, preserving caller-owned transaction, cancellation, ordering, hash-key/hash-diff, load-timestamp, record-source, and idempotency semantics.
- Tighten diagnostics or documentation when needed so OracleMinimumOperationThreshold and OracleMaximumSatelliteOperationThreshold remain actionable for adopters and benchmark reports.

### Scope Out
- Redefining benchmark artifact filenames, provider matrix, or evidence schema; reuse docs/plans/performance-evidence-benchmark-artifact-contract.md and the completed v0.32.0 all-provider bundle.
- Automatic stored procedure deployment, consumer schema ownership, runtime artifact invocation, or any new public persistence API.
- Cross-provider threshold retuning outside Oracle except for shared wording needed to explain the Oracle boundary consistently.
- Promoting staged Oracle bulk as the default selected path without a measured win and a bounded transaction/cleanup contract.

## Acceptance Criteria
- The ticket produces a v0.32.0 before/after or no-change Oracle benchmark artifact set that reuses the benchmark artifact contract and explicitly compares the current high-volume Oracle boundary scenarios.
- The final report states one authoritative decision: keep the 10000-satellite cap, change it to a new bounded value, or introduce a different bounded Oracle path, and it ties that decision to measured Oracle results rather than intuition.
- Any proposed Oracle path change proves the same save semantics as today: caller-owned transaction behavior, rollback on provider failure, cancellation boundaries, request ordering, hash key/hash diff, load timestamp, record source, and idempotency.
- Diagnostics and report output make Oracle decline reasons actionable, at minimum surfacing the OracleMinimumOperationThreshold and OracleMaximumSatelliteOperationThreshold facts whenever they drive fallback.
- Repository validation passes for the landed decision: Oracle boundary coverage plus dotnet test DVault.slnx --nologo and bash tools/check-format.sh.

## Definition of Done
- A benchmark artifact triplet is stored under a v0.32.0 label with matched-input before/after evidence or an explicit no-change rationale for Oracle high-volume saves.
- The ticket outcome records the final Oracle boundary decision and measured rationale in a form that downstream documentation can lift directly.
- Any code, diagnostics text, and tests touched by the implementation align on the same Oracle boundary and fallback explanation.
- The final branch evidence shows that regression validation covered Oracle gate behavior, rollback/ordering/idempotency semantics, and the standard repository test/format commands.

## Implementation Notes
- Current code already fixes the Oracle baseline: OracleDataVaultSaveStrategy returns DirectOracleBatching after standard gate evaluation and reports stagedOracleBulk=not-selected-no-measured-win; no staged Oracle path is currently selected by default.
- Current diagnostics already expose OracleMinimumOperationThreshold (50 total operations) and OracleMaximumSatelliteOperationThreshold (10000 satellite operations), and telemetry guidance already tells callers when to increase or reduce batch size.
- Existing Oracle coverage already proves ordered-batch persistence, rollback on provider failure, direct-path selection, and fallback for below-threshold, multi-active, and over-threshold batches.
- Completed evidence ticket 06F9XD26D2MHVAKZ2GCZ67BEFC is done; use artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-20260607/ as the authoritative all-provider baseline for this tuning work instead of the root skipped-row benchmark triplet.
- That bundle already shows Oracle customer-profile-scale-1000x10 with saveStrategyStatus=ProviderStrategySelected and Oracle customer-profile-scale-10000x10 with saveStrategyStatus=ProviderNeutralFallback plus fallbackCauses=OracleMaximumSatelliteOperationThreshold, so the task starts from measured boundary-adjacent evidence rather than a hypothetical threshold.
- This ticket still blocks documentation task 06F8KZVRARQPG482YKCQ686PNM, so the final Oracle decision should be phrased so v0.32.0 artifact-lane documentation can lift it without reopening scope.

## Open Questions
- none

## Follow-Up Questions
- If the evaluation ends with a no-change decision, should 06F8KZVRARQPG482YKCQ686PNM explicitly call out Oracle's retained 10000-satellite safety boundary and stagedOracleBulk=not-selected-no-measured-win in the v0.32.0 docs?
- If a staged Oracle experiment shows a clear win only for very large satellite batches, should that staged path stay in this ticket or move to a separate follow-up with its own transaction/cleanup contract?

## Risks
- Current evidence shows Oracle direct batching only modestly beats fallback at customer-profile-scale-1000x10 (849.163 ms vs 1255.136 ms) and still lags conventional EF badly at customer-profile-scale-10000x10 (10689.765 ms optimized-baseline row vs 5500.134 ms conventional), so simply raising the 10000-satellite cap could lock in a slower path.
- Any threshold or path change risks transaction, rollback, cancellation, ordering, hash-key/hash-diff, load-timestamp, record-source, and idempotency regressions that current Oracle smoke tests guard.
- The live relation set still contains an incoming blocks edge from done evidence ticket 06F9XD26D2MHVAKZ2GCZ67BEFC; it is satisfied by completed evidence but may need later relation cleanup so workflow state matches the finished dependency.

## Split Recommendations
- No new split is justified; parent story 06F9XD1T3TJK7NEBYNVT2JEPZW already separates Oracle (06F9XD2TGEYEG6S0AK86YF295M), SQL Server (06F9XD2M71D1XFT7FJX62KD8HM), and PostgreSQL/MySQL (06F9XD33MNNVHHW232TC7T1CN8) threshold work.
- If staged Oracle bulk later proves a win but requires new temporary-object cleanup or transaction-contract work, split that staged implementation from this ticket instead of widening the current threshold-evaluation pass.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Evaluate whether the Oracle high-volume satellite save threshold should be changed, refined, or documented as an intentional safety boundary.

Observed seed evidence:
- `customer-profile-scale-10000x10` reported `OracleMaximumSatelliteOperationThreshold`.
- The optimized Oracle row remained close to provider-neutral fallback and much slower than the conventional EF comparison row, so the high-volume boundary deserves a focused review.

Scope:
- Review Oracle direct batching and any staged/native bulk boundary for large satellite batches.
- Consider threshold, chunking, or diagnostics changes only when benchmark evidence proves a stable win without semantic drift.
- Do not introduce automatic stored procedure deployment, consumer schema ownership, or a runtime artifact invocation path.

Podman test environment:
- Use the existing `oracle` Podman container for opt-in integration checks and benchmark before/after evidence.
- Keep the run under the v0.32.0 artifact/evidence path and include fallback causes in the report.

Acceptance criteria:
- The ticket produces Oracle before/after benchmark artifacts or a documented no-change decision with measured rationale.
- Large satellite saves preserve transaction behavior, rollback on provider failure, cancellation boundaries, ordering, hash key/hash diff, load timestamp, record source, and idempotency.
- Diagnostics make the high-volume decline reason actionable.
- `dotnet test DVault.slnx --nologo` and `bash tools/check-format.sh` pass.