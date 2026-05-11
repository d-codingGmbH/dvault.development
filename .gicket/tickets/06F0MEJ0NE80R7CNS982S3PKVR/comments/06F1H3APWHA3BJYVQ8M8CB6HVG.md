[gicket-bot] PO refinement contract

Summary
- Refined the benchmark ticket around one bounded goal: add comparable read-performance benchmark coverage for latest satellite, PIT as-of, and bridge traversal reads using the existing provider package/read-model baseline. The ticket is ready for PO-critic because current docs and source establish the provider family, read-service contracts, and deterministic skip posture.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 provider matrix is the visible DVault provider package family: SQLite, MySQL, Postgres, SQL Server, and Oracle. SQLite is the required local baseline; the others may run when configured or be skipped deterministically with the missing configuration named in output.
- The benchmarks are for read baselines only and should not implement provider-specific read optimizations, PIT refresh, bridge maintenance, or persistent database provisioning.
- Latest satellite reads should benchmark the existing IDataVaultReadService latest/as-of satellite path. PIT reads should follow the DataVaultPitAsOfReadRequest/DataVaultPitReadRecord planning contract. Bridge reads should follow the existing provider-neutral DataVaultBridgeReadRequest/DataVaultBridgeReadRecord pipeline.

Scope In
- Add or extend benchmark scenarios for latest satellite read performance using the established read-service path.
- Add PIT as-of read benchmark coverage against the planned v1 PIT read contract shape, using provider-neutral seeded data suitable for repeatable measurement.
- Add bridge traversal read benchmark coverage for the existing provider-neutral bridge read shape, including hierarchy depth behavior where supported by the benchmark fixture.
- Run each scenario across the visible provider package matrix when local configuration is present, and emit deterministic skip rows when configuration is absent.
- Summarize results so baseline comparisons and optimization candidates are visible per scenario and provider.

Scope Out
- Provider-specific read optimization implementation.
- PIT refresh orchestration, bridge traversal maintenance, or new read-model semantics beyond the documented read contracts.
- Provisioning secrets, containers, cloud databases, or persistent local database state.
- Changing public API contracts except for benchmark-only plumbing needed to exercise existing or already-planned read paths.
- Creating subtickets or expanding this into release posture/documentation alignment work.

Open questions
- none

Follow-up questions
- After baseline numbers exist, decide which read path and provider combinations should receive provider-specific optimization tickets first.
- Decide later whether benchmark artifacts should be archived in release notes or CI build artifacts for trend comparison.
- Decide later whether non-SQLite providers should gain standardized local container profiles for easier manual benchmarking.

Risks
- Non-SQLite providers may produce sparse measured data on machines without local configuration, so the summary must distinguish measured results from deterministic skips.
- PIT and bridge read baselines are provider-neutral and may expose fixture/setup costs if the benchmark does not separate seeding from measured operations.
- Provider differences in timestamp storage or query translation can make raw numbers hard to compare unless scenario labels and fixture sizes are consistent.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment