[gicket-bot] PO-critic review contract

Summary
- Refinement now pins one exact v1 dotnet ef boundary, preflight step, and scope split; the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted contract at .gicket/tickets/06F1XPVPKVGYKCV04PY98TSS78/description.md now fixes the v1 boundary to a consumer-owned IDesignTimeDbContextFactory<TContext>, says Microsoft.EntityFrameworkCore.Design stays consumer-owned, marks startup-project/target-project and multi-project layouts unsupported, and shows ## Open Questions as none.
- Ticket comment .gicket/tickets/06F1XPVPKVGYKCV04PY98TSS78/comments/06F21T2QGXZTD31MTWZP038604.md explicitly answers prior critic-item-1 through critic-item-5, including that migration guardrails surface in a separate preflight step after scaffolding and before dotnet ef database update.
- Direct source evidence exists for the required public APIs: src/DCoding.Data.DVault/DataVaultDiagnostics.cs exposes IDataVaultDiagnosticsService.Analyze(DbContext); src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs exposes AnalyzeReport(IDataVaultDiagnosticsService, DbContext, IEnumerable<MigrationOperation>); src/DCoding.Data.DVault/DataVaultMigrationGuardrailReport.cs exposes ToDisplayString().
- Repository tests already exercise the reused analysis/reporting surfaces: tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs calls diagnostics.Analyze(context) and DataVaultMigrationOperationDiagnostics.AnalyzeReport(diagnostics, context, [new DropTableOperation { Name = "HubCustomer" }]); tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs asserts deterministic guardrail ToDisplayString() output.
- src/DCoding.Data.DVault/DCoding.Data.DVault.csproj contains only Microsoft.EntityFrameworkCore, Microsoft.EntityFrameworkCore.Relational, and Microsoft.Extensions.DependencyInjection.Abstractions; repo search for Microsoft.EntityFrameworkCore.Design, IDesignTimeDbContextFactory, and IDesignTimeServices found matches only in the ticket contract, not in src, examples, tests, README.md, or docs.
- Branch history is ticket-only refinement: git log --oneline --decorate --max-count=12 shows HEAD 8d13448c2 on ticket/06F1XPVPKVGYKCV04PY98TSS78-story-add-dvault-design-time-services-for-dotnet, and git diff --stat 4085a0786..HEAD touches only .gicket/tickets/06F1XPVPKVGYKCV04PY98TSS78/** files.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No current repository example demonstrates the exact single-project IDesignTimeDbContextFactory<TContext> workflow, so implementation should add one concrete walkthrough or sample artifact for that exact layout instead of relying on existing multi-project quickstarts.
- Docs should show at least one explicit unsupported case such as a startup-project/target-project split or other multi-project dotnet ef layout so the v1 boundary stays unambiguous.

Risky assumptions
- examples/DCoding.Data.DVault.SqliteQuickstart and examples/DCoding.Data.DVault.PostgresQuickstart both rely on examples/DCoding.Data.DVault.Quickstarts.Shared, so current repo examples do not themselves evidence the promised single-project baseline; developers must avoid treating them as supported dotnet ef layout proof without adjustment.
- The ticket assumes the existing DbContext-based diagnostics and migration-operation report surfaces are sufficient to compose the consumer preflight without introducing repo-owned EF CLI integration.

AC / test suggestions
- Add acceptance evidence that constructs the DbContext through the consumer-owned factory, runs IDataVaultDiagnosticsService.Analyze(DbContext), and emits ToDisplayString() without a live database.
- Add acceptance evidence for the separate post-scaffold preflight using DataVaultMigrationOperationDiagnostics.AnalyzeReport(..., DbContext, operations) and DataVaultMigrationGuardrailReport.ToDisplayString() before any apply/update step.

Implementation watchouts
- Do not add Microsoft.EntityFrameworkCore.Design to src/DCoding.Data.DVault/DCoding.Data.DVault.csproj or introduce a DVault-owned IDesignTimeServices/CLI shim.
- Do not imply automatic output during dotnet ef migrations add or dotnet ef database update; the contract permits only an explicit separate preflight step.
- Do not present the done child proof as evidence of EF CLI interception; it is only evidence of the underlying no-live-database analysis path.

Non-blocking notes
- The earlier PO-critic blocking review in .gicket/tickets/06F1XPVPKVGYKCV04PY98TSS78/comments/06F21QKCBKN0DBC4Z0PPHSFPYG.md has been directly answered by the later refinement comment and updated delivery contract.

Split recommendations
- No split is required now; the existing done child 06F1XPW1N9PATP3R6YG53ZNGV0 covers the proof slice and downstream drift scope remains with 06F1XPWB8DZR4J8EZ00V8DT25G plus its child tasks.
- If first-party packaged tooling, repo-owned IDesignTimeServices, or broader multi-project support is later desired, keep that as a separate follow-up ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment