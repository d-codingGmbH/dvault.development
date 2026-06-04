[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Verified persisted parentOf children 06F8KZHZ27SDTNCFNMFDQRVCKM, 06F8KZJAKN7Q2QXXP9PRK2V94G, 06F8KZJNZ999C8NKY0S92VBDN0, 06F8KZK2MSFQP9G2DBM61ZVGD4, and 06F8KZKFTCC0YXAPRTXA53DNEC; all are already done, so no further split is needed.
- Repository evidence on this branch already aligns docs/releases/v0.28.0.md, README.md, docs/production-adoption-checklist.md, docs/performance-profiles.md, and docs/architecture/dvault-v1-pit-bridge-boundary.md around the same provider-read-optimization baseline.
- The current baseline is bounded to SQLite-only optimized latest-satellite reads plus diagnostics-gated PIT/bridge candidate strategies for SQLite, PostgreSQL, SQL Server, MySQL, and Oracle, with provider-neutral fallback for unsupported providers or ungated shapes.
- Benchmark and diagnostics evidence is already preserved by tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs, tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs, src/DCoding.Data.DVault/DataVaultDiagnostics.cs, and the provider service-collection extensions under src/DCoding.Data.DVault.*.
- A ticket-bound planning note was materialized at docs/plans/provider-read-optimization-evidence-expansion-epic.md to persist the verified epic scope and the remaining relation-housekeeping note.

Scope In
- Track the already-materialized provider read strategy evidence contract, provider PIT/bridge candidate expansion, benchmark/verifier coverage, and v0.28.0 documentation baseline under one epic.
- Treat the provider matrix as SQLite latest-satellite optimization plus PIT/bridge candidate support for SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.
- Preserve diagnostics-gated fallback behavior, explicit PIT/bridge maintenance ownership, and redacted read diagnostics as part of the epic boundary.
- Close the epic as a tracking and coordination item once the completed child work and relation housekeeping are verified.

Scope Out
- New runtime read strategy implementation work beyond the already completed child tickets.
- Non-SQLite latest-satellite provider optimization claims.
- Automatic PIT or bridge maintenance, scheduling, raw SQL disclosure, query-plan promises, or physical-tuning promises.
- Fresh external-provider benchmark reruns or new measured non-SQLite timing claims for this ticket.

Open questions
- none

Follow-up questions
- At final epic closure, does the queued owner-branch cleanup for the historical 06F8KZKFTCC0YXAPRTXA53DNEC -> 06F8KZHNYE6PAGC74BSF70WZ3W blocks relation still need to replay, or has it already been retired elsewhere?
- After v0.28.0, should a later release ticket rerun PostgreSQL, SQL Server, MySQL, and Oracle PIT and bridge benchmarks with configured connections so public docs can cite completed non-SQLite timings rather than skipped guidance rows?

Risks
- The live relation set still contains a historical blocks relation from done child 06F8KZKFTCC0YXAPRTXA53DNEC to the epic; if owner-branch replay is skipped, closure metadata may stay noisier than the actual done-child state.
- If future documentation edits blur the line between completed SQLite timing rows and optional skipped external-provider guidance rows, the release posture could overstate measured non-SQLite evidence.
- If later work reopens provider-specific latest-satellite expectations outside SQLite, it would expand beyond this epic's validated boundary and should be tracked in a new ticket.

Split recommendations
- No further split recommended; the epic already has the correct five-child breakdown and all children are done.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment