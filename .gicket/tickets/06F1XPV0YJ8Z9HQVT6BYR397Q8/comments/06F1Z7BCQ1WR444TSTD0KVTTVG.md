[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the refined ticket now closes the earlier PO-critic gaps, and its contract aligns with the current diagnostics/public-API and schema baseline in the repository.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- HEAD is 042b465573a3341322c02a86f73f5729ac638260, matching the provided scratch-source-ref; git log on .gicket/tickets/06F1XPV0YJ8Z9HQVT6BYR397Q8 and .gicket/relations shows the earlier blocking PO-critic handoff at 5335ed1d7 and the later PO re-handoff at 1ab35bc94.
- .gicket/tickets/06F1XPV0YJ8Z9HQVT6BYR397Q8/description.md:11-21 now defines MI-1..MI-5, deterministic migration/{OperationType}/{Target}/{Member?} paths, and safe/finding rules for AddColumn, DropColumn, DropTable, RenameColumn, CreateIndex, and AlterColumn.
- .gicket/tickets/06F1XPV0YJ8Z9HQVT6BYR397Q8/description.md:36-43 binds acceptance criteria to the six operations, DVM2001-DVM2006, exact path assertions, DataVaultDiagnosticsResult.Issues, unchanged public API snapshots, and removal of the stale 06F1XPS7KGKBP5SVMQPJC49J2G dependency text.
- .gicket/tickets/06F1XPV0YJ8Z9HQVT6BYR397Q8/description.md:61-62 says Open Questions: none.
- .gicket/tickets/06F1XPV0YJ8Z9HQVT6BYR397Q8/comments/06F1Z5CED4TBKVW3ET4XJN08SM.md marks critic-item-1 through critic-item-5 answered, including the diagnostics ownership boundary and relation cleanup.
- git show --stat --oneline 1ab35bc94583 records deletion of the stale blocks relation file and a rewrite of .gicket/tickets/06F1XPV0YJ8Z9HQVT6BYR397Q8/description.md before the current handoff.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs:164-168 and 310-449 shows the existing public diagnostics surface is DataVaultDiagnosticsIssue(Severity, Code, Message, Path), DataVaultDiagnosticsResult(..., Issues), and IDataVaultDiagnosticsService Analyze overloads; a repository-wide search for Analyze(MigrationOperation) or MigrationOperation in src/DCoding.Data.DVault and tests/DCoding.Data.DVault.Tests returned no matches.
- tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs:12-86 and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:101-134,753-764 snapshot-lock the current public API, while src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:52-239 and tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs:65-118 provide the current Hub/Link/Satellite owned-name and index baseline named in the contract.
- rg for 06F1XPV0YJ8Z9HQVT6BYR397Q8 in docs/plans and .gicket-bot returned no matches, consistent with the contract claim that no planning artifacts were materialized.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No acceptance-criteria example explicitly names a multi-finding migration batch that locks cross-issue ordering; description.md:70-74 documents that risk, so this is a non-blocking test-design gap rather than a PO blocker.

Risky assumptions
- Implementation still has to derive DVault-owned objects from metadata, naming rules, and schema baselines rather than simple Hub*/Link*/Sat* prefix matching, as warned in description.md:55-59 and 70-71.

AC / test suggestions
- Add one deterministic test where a single migration operation or batch emits multiple findings so exact issue ordering is locked, not only single-finding cases.
- For CreateIndex, include one safe supplemental index on a DVault table with a non-DVault name and one finding that reuses the DVault-owned default name with wrong semantics, to keep the boundary explicit.

Implementation watchouts
- Keep migration analysis behind the existing diagnostics surface; DataVaultDiagnostics.cs and the approved API snapshots do not currently expose a public MigrationOperation entrypoint.
- Derive owned table, column, key, and index expectations from DataVaultEfMetadataTranslator.cs and SqliteDataVaultSchemaTests.cs, not from provider-specific SQL or string-prefix heuristics.
- Do not widen scope into Bridge/PIT guardrails, provider-specific DDL parsing, live migration execution, or automatic rewriting; those remain explicitly out of scope in description.md:29-34.

Non-blocking notes
- The ticket remains a child of 06F1XPTCGWTJHHQVNPN13KANMG via .gicket/relations/MG/Q8/06F1XPTCGWTJHHQVNPN13KANMG--06F1XPV0YJ8Z9HQVT6BYR397Q8--parentOf.json; only the stale block on 06F1XPS7KGKBP5SVMQPJC49J2G was removed.

Split recommendations
- No split is needed for the current six-operation Hub/Link/Satellite guardrail scope.
- If later work needs Bridge/PIT guardrails or a public migration-analysis API, split that follow-up from this ticket instead of widening the current delivery contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment