[gicket-bot] PO refinement contract

Summary
- Refined the ticket to one bounded first read optimization: SQLite latest/as-of satellite reads via the provider read-strategy hook, with fallback, correctness tests, and before/after benchmark evidence.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 target is SQLite latest/as-of satellite reads by parent hash key. SQLite is the required local provider baseline, and the existing benchmark fixture exposes the clearest bounded over-read: 100 latest rows selected from 1000 seeded profile states through the provider-neutral path.
- Use the provider read-strategy hook contract from the completed hook task; do not add provider selection to caller code or invent a second dispatch model.
- The existing benchmark task provides the latest satellite, PIT, and bridge baseline matrix. This ticket uses that matrix to justify one implementation and does not reopen benchmark scope.

Scope In
- Implement a SQLite provider-specific read strategy for supported DataVaultLatestSatelliteReadRequest latest/as-of satellite reads, registered through AddDVaultSqlite().
- Push parent-key filtering and latest/as-of row selection into SQLite SQL for the supported satellite shape, returning the same public read records and typed projection behavior as the fallback path.
- Decline unsupported providers, read request families, metadata shapes, or timestamp storage modes back to the provider-neutral read service without failing the read.
- Add focused tests for strategy selection, fallback, latest-row correctness, as-of correctness, and typed projection parity.
- Capture before/after benchmark evidence with command/options/run context and document why the selected shape was optimized first.
- Preserve write strategy behavior and public API compatibility.

Scope Out
- Optimizing PIT reads, bridge traversal reads, or every satellite/read shape in this ticket.
- Optimizing PostgreSQL, SQL Server, MySQL, or Oracle read paths in this ticket.
- Changing save strategy behavior, schema generation semantics, or write benchmark contracts.
- Adding new caller-facing read request types or requiring application code to choose providers explicitly.
- Provisioning external databases, secrets, containers, or persistent benchmark infrastructure.

Open questions
- none

Follow-up questions
- After this first optimization lands, use the benchmark matrix to choose the next provider/read shape, if any.
- Decide later whether SQLite driving-key satellites, PIT reads, or bridge reads deserve separate provider-specific optimization tickets.
- Decide later whether benchmark artifacts should be archived in release notes or CI artifacts for trend comparison.

Risks
- Benchmark timings are machine-specific, so evidence must keep run context attached to the result.
- SQLite timestamp storage and duplicate timestamp edge cases can produce subtle parity issues if SQL ordering differs from fallback behavior.
- The completed hook dependency may need branch refresh or reconciliation before the provider strategy can be wired cleanly.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment