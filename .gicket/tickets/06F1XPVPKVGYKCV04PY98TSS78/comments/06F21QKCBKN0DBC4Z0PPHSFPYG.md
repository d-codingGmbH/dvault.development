[gicket-bot] PO-critic review contract

Summary
- The contract is grounded on real diagnostics and migration APIs, but it still leaves the concrete dotnet ef integration boundary and supported layout baseline unresolved.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F1XPVPKVGYKCV04PY98TSS78/description.md persists Open Questions = none, so the remaining issues are contract clarity rather than an explicit open-questions list.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs exposes IDataVaultDiagnosticsService.Analyze(DbContext) and DataVaultDiagnosticsResult.ToDisplayString().
- src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs exposes AnalyzeReport(IDataVaultDiagnosticsService, DbContext, IEnumerable<MigrationOperation>), and src/DCoding.Data.DVault/DataVaultMigrationGuardrailReport.cs exposes ToDisplayString().
- src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs exposes UseDataVaultMetadata(...), and src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs exposes ApplyDataVaultMetadata(...).
- .gicket/tickets/06F1XPW1N9PATP3R6YG53ZNGV0/comments/06F21J7QDVZ9CYDCZM6H4M2W4R.md records tester verification of 4/4 acceptance criteria and 4/4 definition-of-done expectations on branch ticket/06F1XPW1N9PATP3R6YG53ZNGV0-task-wire-design-time-validation-into-a-sample-w at commit 1e302f658912.
- Repository search returned: NO_MATCH: no dotnet-ef/design-time-service references found in README.md docs src tests examples for the pattern dotnet ef|IDesignTimeServices|IDesignTimeDbContextFactory|Microsoft.EntityFrameworkCore.Design.
- Repository search returned: NO_MATCH: no Microsoft.EntityFrameworkCore.Design package/reference strings found in project/build files.
- Search for startup-project|target-project|single-project|multi-project found only .gicket/tickets/06F1XPVPKVGYKCV04PY98TSS78/description.md lines that warn not to over-promise layouts and ask whether a later ticket should add an explicitly exercised startup-project versus target-project proof case.
- The current design-time proof slice is tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelFirstDesignTimeWorkflowTests.cs, and docs/model-first-governance.md documents it as a SQLite-backed design-time metadata workflow with no database connection; it does not show a dotnet ef path.

Blocking findings
- The ticket asks developers to document a supported dotnet ef design-time path and project-layout baseline, but the repository currently contains no dotnet ef, IDesignTimeServices, IDesignTimeDbContextFactory, or Microsoft.EntityFrameworkCore.Design evidence, and the contract never pins one concrete v1 composition point or supported layout. That leaves a user-visible compatibility decision to developer interpretation.
- The reused child proof slice is model-first drift validation, not an EF CLI workflow. The contract says migration guardrail summaries must surface in the design-time path, but it does not say whether that output is required during dotnet ef migrations add, during migration application, or in a separate documented preflight step that analyzes MigrationOperation output.

Required PO actions
- Amend the delivery contract to name one exact v1 dotnet ef integration boundary and ownership model: consumer-owned IDesignTimeServices, consumer-owned IDesignTimeDbContextFactory, a DVault-owned minimal shim, or a docs-only/preflight path. Also state whether any Microsoft.EntityFrameworkCore.Design dependency is consumer-only or allowed in repo code for this story.
- State the single supported project-layout baseline for this story and mark other layouts unsupported for v1. The current contract only says not to over-promise layouts; it does not identify the baseline the developer should implement and document.
- State exactly when migration guardrail summaries must appear in the approved workflow: scaffolding, apply/update, or an explicit separate preflight command.

Open issues ledger
- critic-item-1 [required-po-action] Amend the delivery contract to name one exact v1 dotnet ef integration boundary and ownership model: consumer-owned IDesignTimeServices, consumer-owned IDesignTimeDbContextFactory, a DVault-owned minimal shim, or a docs-only/preflight path. Also state whether any Microsoft.EntityFrameworkCore.Design dependency is consumer-only or allowed in repo code for this story.
- critic-item-2 [required-po-action] State the single supported project-layout baseline for this story and mark other layouts unsupported for v1. The current contract only says not to over-promise layouts; it does not identify the baseline the developer should implement and document.
- critic-item-3 [required-po-action] State exactly when migration guardrail summaries must appear in the approved workflow: scaffolding, apply/update, or an explicit separate preflight command.
- critic-item-4 [blocking-finding] The ticket asks developers to document a supported dotnet ef design-time path and project-layout baseline, but the repository currently contains no dotnet ef, IDesignTimeServices, IDesignTimeDbContextFactory, or Microsoft.EntityFrameworkCore.Design evidence, and the contract never pins one concrete v1 composition point or supported layout. That leaves a user-visible compatibility decision to developer interpretation.
- critic-item-5 [blocking-finding] The reused child proof slice is model-first drift validation, not an EF CLI workflow. The contract says migration guardrail summaries must surface in the design-time path, but it does not say whether that output is required during dotnet ef migrations add, during migration application, or in a separate documented preflight step that analyzes MigrationOperation output.

Missing examples / edge cases
- One explicit supported layout example for the chosen v1 path.
- One explicit non-goal example for unsupported startup-project/target-project or multi-project variants.
- Expected behavior when DVault validation passes but migration guardrail analysis reports findings.
- Expected behavior when the chosen path only has model validation available and no MigrationOperation sequence to inspect.

Risky assumptions
- Assumes a developer can choose the dotnet ef hook and layout baseline without changing the intended public support contract.
- Assumes migration-operation guardrail output can be surfaced inside the chosen design-time path without unintentionally expanding scope into a first-party EF design package or custom CLI behavior.
- Assumes the model-first design-time workflow proven by child ticket 06F1XPW1N9PATP3R6YG53ZNGV0 is sufficient proxy evidence for the parent story's EF CLI workflow.

AC / test suggestions
- Add one acceptance criterion with the exact repo-root reproduction command for the chosen dotnet ef path and the exact expected validation and guardrail output surfaces.
- Add one acceptance criterion that names the only supported layout for v1 and explicitly rejects broader startup-project or target-project claims.
- Add one acceptance criterion that says whether both ApplyDataVaultMetadata(...) and UseDataVaultMetadata(...) must be shown on the chosen path or whether one is the sole v1 baseline.

Implementation watchouts
- Do not let this story absorb ModelSnapshot or live-database drift work already deferred to 06F1XPWB8DZR4J8EZ00V8DT25G and its child tasks.
- Do not introduce provider-specific output contracts or a custom EF CLI.
- Do not add a hard EF design-package dependency to DCoding.Data.DVault unless the PO contract explicitly allows it after clarification.
- Keep any documented workflow design-time-only and independent of a live database connection.

Non-blocking notes
- git log --oneline develop..HEAD shows only ticket-state commits for 06F1XPVPKVGYKCV04PY98TSS78, and git diff --name-only develop..HEAD is limited to .gicket/tickets/06F1XPVPKVGYKCV04PY98TSS78/*, so this branch is still a ticket-refinement surface rather than an implementation branch.
- The prerequisite diagnostic and migration APIs are real and source-backed; the return decision is about unresolved contract choice, not missing foundation work.

Split recommendations
- No new split is needed if the PO narrows this ticket to one explicit v1 dotnet ef path and one explicit layout baseline.
- If stakeholders want both a same-project baseline and a separate startup-project/target-project baseline, keep this ticket to one path and materialize the second layout as follow-up work.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment