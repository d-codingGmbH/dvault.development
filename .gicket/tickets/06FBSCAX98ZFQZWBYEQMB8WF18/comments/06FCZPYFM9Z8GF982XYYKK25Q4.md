[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff. The persisted contract is specific, has no open questions, and the repository already contains the bounded documentation and evidence surfaces this task tells the developer to preserve.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FBSCAX98ZFQZWBYEQMB8WF18/description.md:7-8,50-53` records PO handoff `ready_for_po_critic`, `## Open Questions` = `none`, and only follow-up questions rather than unresolved scope blockers.
- `.gicket/tickets/06FBSCAX98ZFQZWBYEQMB8WF18/comments/06FCZMZS452Y5VQEDPW2NNNQBM.md` scopes the work to documentation alignment only and explicitly names `README.md`, `docs/performance-profiles.md`, `docs/releases/v0.39.0.md`, `CHANGELOG.md`, `docs/architecture/dvault-v1-explicit-save-service.md`, and `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` as the aligned repository surfaces.
- `README.md:139-165` keeps the root README high-level, routes performance details to `docs/performance-profiles.md` and `benchmarks/`, and states that live PostgreSQL/SQL Server/Oracle/MySQL/DB2 validation is opt-in behind `DVAULT_TEST_*` connection strings.
- `docs/performance-profiles.md:9-32`, `docs/releases/v0.39.0.md:80-101`, and `CHANGELOG.md:5,10-13` all separate completed SQLite timing from skipped-placeholder PostgreSQL/SQL Server/MySQL/Oracle/DB2 rows, point follow-up planning to `docs/plans/provider-optimization-gap-matrix.md`, and keep DB2 limited to the current diagnostics and smoke posture.
- `benchmark-summary.md:4-15` and `benchmark-summary.json:31-56,<redacted>` record SQLite as the required provider and PostgreSQL, SQL Server, MySQL, Oracle, and DB2 rows as `executionStatus=skipped` because the corresponding `DVAULT_TEST_*_CONNECTION_STRING` values were unset.
- `docs/architecture/dvault-v1-explicit-save-service.md:23-40,84-105`, `src/DCoding.Data.DVault/IDataVaultSaveService.cs:13`, `src/DCoding.Data.DVault/DataVaultBulkSaveRequest.cs:13`, `src/DCoding.Data.DVault/DataVaultChunkedSaveRequest.cs:13`, and `src/DCoding.Data.DVault/DataVaultProviderSaveStrategyGateEvaluator.cs:10-16,225-263` directly back the contract's referenced reuse, no-op, fallback, and provider-gate semantics.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No additional blocking examples are missing. The only future-facing gaps still called out by the persisted contract are the non-blocking follow-up questions about who should own future provider-configured reruns and whether README should ever summarize provider gates directly.

Risky assumptions
- Approval assumes a developer handoff can legitimately be a preservation or no-op task: `git diff --stat develop..HEAD` shows only `.gicket` ticket metadata changes, so there may be no repository doc delta left to implement.
- Approval assumes the already-landed v0.39.0 documentation surfaces on `develop` are the intended source of truth. If Product wanted new wording beyond those checked-in files, that expectation is not stated in the persisted contract.

AC / test suggestions
- Use the named documentation surfaces as the completion checklist: `README.md`, `docs/performance-profiles.md`, `docs/releases/v0.39.0.md`, `CHANGELOG.md`, `docs/architecture/dvault-v1-explicit-save-service.md`, `benchmarks/DCoding.Data.DVault.Benchmarks/README.md`, and the evidence and gap matrices.
- Verify any doc touch keeps README link-oriented and does not copy raw threshold prose or benchmark tables out of the evidence surfaces.
- Verify any timing claim cites completed evidence with preserved run context and does not promote skipped-placeholder rows from the root triplet into completed external-provider timing.
- Verify DB2 wording stays bounded to clean-context save plus PIT and bridge candidate posture and does not claim latest-satellite optimization, staged bulk, provider-native chunk execution, or live-schema reading.

Implementation watchouts
- Treat the absence of product-file diff on this branch as a developer-handoff watchout, not a PO blocker; the ticket is framed as ratify and preserve existing documentation rather than create new runtime behavior.
- Do not widen provider behavior beyond the existing gates and fallbacks already documented in the save-service contract: dirty contexts, multi-active batches, provider-name mismatch, threshold failures, and unsupported shapes must remain bounded fallback cases.
- Do not rerun benchmarks or reinterpret skipped root rows as measured PostgreSQL, SQL Server, MySQL, Oracle, or DB2 evidence for this ticket.
- If docs are touched, keep `README.md` high-level and continue routing provider-specific threshold detail to `docs/performance-profiles.md`, the benchmark README, and `docs/plans/provider-optimization-evidence-matrix.md`.

Non-blocking notes
- none

Split recommendations
- No split recommended; the persisted contract and repository evidence already bound the work to existing documentation surfaces and claim hygiene.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment