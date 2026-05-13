[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket is bounded and `## Open Questions` is cleared, but the safe/unsafe migration cases and the diagnostics integration contract are still underspecified, and the persisted blocker state is stale.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Branch history shows only PO handoff metadata changes: commit `a098a48de1c61bdb5838448fcbb8eb0c6d3c91e7` (`git show --stat --summary --decorate`) updated `.gicket/tickets/06F1XPV0YJ8Z9HQVT6BYR397Q8/description.md`, comments, events, and `ticket.json`; it did not touch `src/` or `tests/` files.
- The persisted contract is present and bounded in `.gicket/tickets/06F1XPV0YJ8Z9HQVT6BYR397Q8/description.md:18-39`, and `.gicket/tickets/06F1XPV0YJ8Z9HQVT6BYR397Q8/description.md:47-48` shows `## Open Questions` -> `- none`.
- Existing diagnostics test surfaces are real and already in the repo: `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:10-188` and `tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs:13-236` exercise `IDataVaultDiagnosticsService` / `IDataVaultReadDiagnosticsService`.
- Current public diagnostics shape is `DataVaultDiagnosticsIssue(Severity, Code, Message, Path)` in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:164-168`, wrapped by `DataVaultDiagnosticsResult(..., IReadOnlyList<DataVaultDiagnosticsIssue> Issues)` in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:310-324`; the approved public API snapshot matches that four-field issue shape in `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:101-134`.
- `IDataVaultDiagnosticsService` currently exposes analyze overloads only for metadata models, registries, code-first builders, `DbContext`, save requests, and registry save requests in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:383-451`; there is no migration-operation entrypoint there.
- The only repository diagnostic catalog I found is the internal model-artifact catalog in `src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs:3-149` (`DMV1001`-`DMV1801`). A repository search for `MigrationOperation`, `AddColumnOperation`, `DropColumnOperation`, `DropTableOperation`, `RenameColumnOperation`, `CreateIndexOperation`, and `AlterColumnOperation` across `src`, `tests`, and `docs` returned no matches.
- The contract/comment still says the ticket is blocked by `06F1XPS7KGKBP5SVMQPJC49J2G` in `.gicket/tickets/06F1XPV0YJ8Z9HQVT6BYR397Q8/description.md:5,16,59` and comment `06F1YYGR53FC7H9J4JKHWKSQSW.md:4,15,40`, but `.gicket/tickets/06F1XPS7KGKBP5SVMQPJC49J2G/ticket.json:7` shows that ticket is already `done`.

Blocking findings
- The ticket does not define the invariant decision matrix that makes each of the six operation types safe vs. finding-producing. `.gicket/tickets/06F1XPV0YJ8Z9HQVT6BYR397Q8/description.md:44-45` says fixtures must prove invariant-specific cases and that operations are not categorically safe/unsafe, but neither the contract nor existing repo docs/tests enumerate the concrete cases. With no existing migration-operation fixtures or validator code in the repo, the developer would have to invent the product rules.
- The handoff does not resolve how migration findings enter the current diagnostics contract. The existing public diagnostics surface only exposes metadata/DbContext/save/read analysis and `DataVaultDiagnosticsIssue` only carries `Severity`, `Code`, `Message`, and `Path` (`src/DCoding.Data.DVault/DataVaultDiagnostics.cs:164-168,383-451`), while the ticket requires an 'existing diagnostics surface' plus catalog-backed id/severity/location/remediation. PO needs to state whether public diagnostics API/snapshot changes are in scope or whether remediation is expected to stay internal-catalog-only.

Required PO actions
- Add a concrete safe/unsafe example matrix for AddColumn, DropColumn, DropTable, RenameColumn, CreateIndex, and AlterColumn, tied to named DVault invariants and expected diagnostic codes.
- Clarify the diagnostics contract: which current `IDataVaultDiagnosticsService` entrypoint should own migration-operation analysis, whether a new public entrypoint is allowed, and whether public `DataVaultDiagnosticsIssue` / `DataVaultDiagnosticsResult` shape changes are in scope.
- Refresh the dependency text in the ticket contract so it no longer says this ticket is blocked by `06F1XPS7KGKBP5SVMQPJC49J2G` unless there is newer evidence reopening that dependency.

Open issues ledger
- critic-item-1 [required-po-action] Add a concrete safe/unsafe example matrix for AddColumn, DropColumn, DropTable, RenameColumn, CreateIndex, and AlterColumn, tied to named DVault invariants and expected diagnostic codes.
- critic-item-2 [required-po-action] Clarify the diagnostics contract: which current `IDataVaultDiagnosticsService` entrypoint should own migration-operation analysis, whether a new public entrypoint is allowed, and whether public `DataVaultDiagnosticsIssue` / `DataVaultDiagnosticsResult` shape changes are in scope.
- critic-item-3 [required-po-action] Refresh the dependency text in the ticket contract so it no longer says this ticket is blocked by `06F1XPS7KGKBP5SVMQPJC49J2G` unless there is newer evidence reopening that dependency.
- critic-item-4 [blocking-finding] The ticket does not define the invariant decision matrix that makes each of the six operation types safe vs. finding-producing. `.gicket/tickets/06F1XPV0YJ8Z9HQVT6BYR397Q8/description.md:44-45` says fixtures must prove invariant-specific cases and that operations are not categorically safe/unsafe, but neither the contract nor existing repo docs/tests enumerate the concrete cases. With no existing migration-operation fixtures or validator code in the repo, the developer would have to invent the product rules.
- critic-item-5 [blocking-finding] The handoff does not resolve how migration findings enter the current diagnostics contract. The existing public diagnostics surface only exposes metadata/DbContext/save/read analysis and `DataVaultDiagnosticsIssue` only carries `Severity`, `Code`, `Message`, and `Path` (`src/DCoding.Data.DVault/DataVaultDiagnostics.cs:164-168,383-451`), while the ticket requires an 'existing diagnostics surface' plus catalog-backed id/severity/location/remediation. PO needs to state whether public diagnostics API/snapshot changes are in scope or whether remediation is expected to stay internal-catalog-only.

Missing examples / edge cases
- One concrete no-finding case and one concrete finding case are missing for each of AddColumn, DropColumn, DropTable, RenameColumn, CreateIndex, and AlterColumn.
- Location semantics are unspecified: the ticket does not say whether the stable location token should be an operation index, column/property name, model path, or another deterministic identifier.
- Deterministic ordering is required, but the contract does not say whether multiple findings are ordered by fixture order, operation order, diagnostic code, or invariant priority.
- No example covers technical-column edge cases such as hash key, load timestamp, record source, hash diff, or link participant changes during rename/alter/drop scenarios.

Risky assumptions
- Assuming the internal model-artifact catalog pattern can be reused for migration diagnostics without explicit PO approval.
- Assuming a public diagnostics API expansion is acceptable even though `ApiSurfaceSnapshotTests` currently lock the `DataVaultDiagnosticsIssue` and `DataVaultDiagnosticsResult` shape.
- Assuming the current `blocks` language is stale because ticket `06F1XPS7KGKBP5SVMQPJC49J2G` is `done`, despite the persisted contract still describing this ticket as blocked.

AC / test suggestions
- Name the expected invariant(s) per fixture and the exact diagnostic code(s) so tests can assert exact ordered outputs instead of general 'safe/unsafe' behavior.
- Add an acceptance-criteria note that defines the stable location token format and the ordering rule for multiple findings.
- If remediation is intended to remain catalog-backed rather than a public result field, say that tests should assert remediation through catalog definition lookup keyed by the emitted diagnostic code.

Implementation watchouts
- Any public change to `DataVaultDiagnosticsIssue` or `DataVaultDiagnosticsResult` will require updating `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt` and `tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs`.
- Current diagnostics validation already emits freeform issue codes such as `duplicate-logical-name` and `missing-reference` in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:<redacted>`; a migration validator should not introduce a second incompatible diagnostics convention by accident.
- `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` already references `Microsoft.EntityFrameworkCore.Relational`, so EF migration operation types are available, but there is still no existing migration-validator integration point in the current diagnostics APIs.

Non-blocking notes
- The ticket is otherwise well bounded: `.gicket/tickets/06F1XPV0YJ8Z9HQVT6BYR397Q8/description.md:23-27` keeps provider-specific SQL/runtime database access out of scope, and `.gicket/tickets/06F1XPV0YJ8Z9HQVT6BYR397Q8/description.md:61-62` says no split is currently recommended.
- The existing unit and integration test roots already fit the stated test plan, so no separate test project appears necessary from the current repository layout.

Split recommendations
- No split is needed if PO only clarifies the rule matrix and diagnostics contract. If PO decides this work must redesign the public diagnostics API, split that API-contract change from the first migration-validator rule set.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment