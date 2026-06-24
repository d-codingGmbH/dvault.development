[gicket-bot] PO refinement contract

Summary
- Refined this as a bounded fallback-test hardening task. Repository evidence already fixes the PIT maintenance boundary: PostgreSQL supports clean full rebuilds for ordinary hub-parent, multi-active hub-parent, and link-parent non-multi-active PITs; SQL Server supports clean full rebuilds for ordinary hub-parent PITs only, with other cases falling back provider-neutrally. Existing coverage already includes Postgres gate assertions, Postgres happy-path integration rebuilds, and SQL Server provider-mismatch and maintain-parents fallback tests, so this ticket should close the remaining fallback matrix rather than reopen architecture or documentation scope.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The supported-shape baseline is already decided by docs/architecture/dvault-v1-pit-bridge-boundary.md: PostgreSQL provider-native PIT rebuilds are limited to clean full rebuilds for ordinary hub-parent, multi-active hub-parent, and link-parent non-multi-active PITs; SQL Server provider-native PIT rebuilds are limited to clean full rebuilds for ordinary hub-parent PITs only.
- Current repository evidence already covers parts of this matrix in tests/DCoding.Data.DVault.Tests/Unit/PostgresProviderCapabilityTests.cs, tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs, and tests/DCoding.Data.DVault.Tests/Integration/PostgresPitMaintenanceServiceTests.cs. This ticket is for filling remaining fallback gaps and consolidating assertions, not redefining provider behavior.
- No child split or ticket write is justified from the current evidence. The related v0.47 documentation task 06FF43JEA6C3HNJ6AQA9XY7EC8 remains downstream and non-blocking.

Scope In
- Add or consolidate PIT maintenance tests for provider mismatch fallback on PostgreSQL and SQL Server paths.
- Add or consolidate PIT maintenance tests for dirty tracked DbContext fallback on PostgreSQL and SQL Server paths.
- Add or consolidate PIT maintenance tests for unsupported PIT shape fallback using the already documented provider boundaries.
- Add or consolidate coverage that missing provider-specific PIT maintenance registration still leaves rebuild behavior on the provider-neutral maintenance pipeline.
- Assert existing explicit fallback evidence where it already exists, such as gate fallback causes and maintenance activity fallback tags, alongside provider-neutral rebuild or no-op results.

Scope Out
- Changing PIT maintenance architecture, supported provider shapes, or provider-native SQL behavior.
- Bridge maintenance push-down, scheduling, read-path changes, or benchmark evidence work.
- Broader diagnostics API expansion or new public telemetry contracts beyond what current PIT maintenance surfaces already expose.
- Release note, changelog, or evidence-matrix documentation updates beyond whatever the existing docs ticket handles later.

Open questions
- none

Follow-up questions
- After the fallback matrix lands, should the downstream v0.47 documentation task mention the strengthened PIT maintenance fallback test coverage as supporting evidence for the provider maintenance boundary?

Risks
- If PostgreSQL fallback proof relies only on opt-in live-provider integration, CI signal will remain weaker than deterministic unit coverage.
- SQL Server uses service replacement while PostgreSQL uses strategy registration, so missing-registration coverage must be set up deliberately or tests may accidentally recheck provider mismatch instead of the intended registration boundary.
- The PIT maintenance code exposes fewer established diagnostics hooks for missing registration than save and read flows, so test design should avoid accidental scope creep into new public diagnostics behavior.

Split recommendations
- No split recommended; the remaining work is a bounded hardening pass across already-existing PIT maintenance test surfaces.

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