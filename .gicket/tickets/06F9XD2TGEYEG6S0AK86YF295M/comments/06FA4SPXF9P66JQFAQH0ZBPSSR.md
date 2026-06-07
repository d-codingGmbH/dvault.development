[gicket-bot] PO refinement contract

Summary
- Refined the Oracle high-volume threshold task around the repository-proven 50-operation minimum, 10000-satellite maximum, direct-batching baseline, and the completed v0.32.0 all-provider Podman evidence; no split or persistent planning mutation was needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already ratifies the Oracle v1 baseline as direct optimized batching only; stagedOracleBulk=not-selected-no-measured-win is the current intended state until new evidence proves otherwise.
- Oracle save-strategy gates are already finite and repository-proven in code and tests: minimum 50 total operations and maximum 10000 satellite operations.
- Done evidence ticket 06F9XD26D2MHVAKZ2GCZ67BEFC already supplies the authoritative all-provider baseline bundle at artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-<redacted>/; this ticket should consume that bundle instead of redefining benchmark format or provider matrix.
- Parent story 06F9XD1T3TJK7NEBYNVT2JEPZW already groups this Oracle task with sibling SQL Server and PostgreSQL/MySQL threshold-tuning tasks, so no additional child split is justified.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized in this refinement pass.

Scope In
- Use the completed v0.32.0 all-provider Oracle artifact bundle to decide whether the current 10000-satellite cap stays, moves, or is replaced by a better bounded Oracle path.
- Benchmark and diagnose Oracle customer-profile high-volume saves at and around the current boundary, including the scenarios that currently keep provider selection at 10000 satellites and fall back at <redacted> satellites.
- Keep the evaluation on the existing IDataVaultSaveService and OracleDataVaultSaveStrategy boundary, preserving caller-owned transaction, cancellation, ordering, hash-key/hash-diff, load-timestamp, record-source, and idempotency semantics.
- Tighten diagnostics or documentation when needed so OracleMinimumOperationThreshold and OracleMaximumSatelliteOperationThreshold remain actionable for adopters and benchmark reports.

Scope Out
- Redefining benchmark artifact filenames, provider matrix, or evidence schema; reuse docs/plans/performance-evidence-benchmark-artifact-contract.md and the completed v0.32.0 all-provider bundle.
- Automatic stored procedure deployment, consumer schema ownership, runtime artifact invocation, or any new public persistence API.
- Cross-provider threshold retuning outside Oracle except for shared wording needed to explain the Oracle boundary consistently.
- Promoting staged Oracle bulk as the default selected path without a measured win and a bounded transaction/cleanup contract.

Open questions
- none

Follow-up questions
- If the evaluation ends with a no-change decision, should 06F8KZVRARQPG482YKCQ686PNM explicitly call out Oracle's retained 10000-satellite safety boundary and stagedOracleBulk=not-selected-no-measured-win in the v0.32.0 docs?
- If a staged Oracle experiment shows a clear win only for very large satellite batches, should that staged path stay in this ticket or move to a separate follow-up with its own transaction/cleanup contract?

Risks
- Current evidence shows Oracle direct batching only modestly beats fallback at customer-profile-scale-1000x10 (849.163 ms vs <redacted> ms) and still lags conventional EF badly at customer-profile-scale-10000x10 <redacted> ms optimized-baseline row vs <redacted> ms conventional), so simply raising the 10000-satellite cap could lock in a slower path.
- Any threshold or path change risks transaction, rollback, cancellation, ordering, hash-key/hash-diff, load-timestamp, record-source, and idempotency regressions that current Oracle smoke tests guard.
- The live relation set still contains an incoming blocks edge from done evidence ticket 06F9XD26D2MHVAKZ2GCZ67BEFC; it is satisfied by completed evidence but may need later relation cleanup so workflow state matches the finished dependency.

Split recommendations
- No new split is justified; parent story 06F9XD1T3TJK7NEBYNVT2JEPZW already separates Oracle (06F9XD2TGEYEG6S0AK86YF295M), SQL Server (06F9XD2M71D1XFT7FJX62KD8HM), and PostgreSQL/MySQL (06F9XD33MNNVHHW232TC7T1CN8) threshold work.
- If staged Oracle bulk later proves a win but requires new temporary-object cleanup or transaction-contract work, split that staged implementation from this ticket instead of widening the current threshold-evaluation pass.

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