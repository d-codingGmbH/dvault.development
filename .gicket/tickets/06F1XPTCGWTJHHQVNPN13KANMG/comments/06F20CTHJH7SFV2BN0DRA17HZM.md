[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F1XPTCGWTJHHQVNPN13KANMG\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F1XPTCGWTJHHQVNPN13KANMG-story-add-ef-migration-guardrails-for-data-vault\u0027 and commit \u002738ada5ee0c9c\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F1XPTCGWTJHHQVNPN13KANMG-story-add-ef-migration-guardrails-for-data-vault\u0027 from source \u002738ada5ee0c9c\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F1XPTCGWTJHHQVNPN13KANMG-story-add-ef-migration-guardrails-for-data-vault\u0027.",
    "Evidence: git rev-parse --abbrev-ref HEAD returned develop, so the review used explicit target commit 38ada5ee0c9c against develop rather than relying on the checked-out branch.",
    "Evidence: git diff --name-only develop...38ada5ee0c9c excluding .gicket listed product/doc/test changes in docs/plans/deferred-data-vault-capabilities.md, src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs, DataVaultMigrationGuardrailIssue.cs, DataVaultMigrationGuardrailReport.cs, DataVaultMigrationOperationDiagnostics.cs, integration/unit tests, and the public API snapshot.",
    "Evidence: git ls-tree -r --name-only 38ada5ee0c9c confirmed src/DCoding.Data.DVault/DataVaultMigrationGuardrailIssue.cs and DataVaultMigrationGuardrailReport.cs exist at the claimed commit; the DCoding.Data.DVault SDK-style csproj uses default compile inclusion.",
    "Evidence: DataVaultMigrationOperationDiagnostics.cs exposes public AnalyzeReport overloads for baseline, metadata model, registry, code-first callback, and DbContext, and expands the baseline filter to Hub, Link, Satellite, Pit, and Bridge.",
    "Evidence: DataVaultMigrationGuardrailReport.cs exposes Issues, HasFindings, IsValid, ToDisplayString, and maps DVM issues to central catalog remediation.",
    "Evidence: Unit diff adds PIT and bridge metadata to CreateMigrationGuardrailMetadataModel and finding cases for PIT snapshot columns, bridge TraversalDepth, DropIndexOperation, DropPrimaryKeyOperation, and bridge table drops.",
    "Evidence: git grep -n -A 18 AnalyzeAddPrimaryKey at 38ada5ee0c9c shows the method returns [] when operation.Name does not equal entity.PrimaryKey.Name, before checking/reporting a mismatch.",
    "Evidence: git grep AddPrimaryKeyOperation at 38ada5ee0c9c found only src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs references and no unit test references.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/diagnostics, area/ef-core, area/migrations, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.2].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 3 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F1XPVPKVGYKCV04PY98TSS78-story-add-dvault-design-time-services-for-dotnet\u0027.",
    "Evidence: Ticket history references implementation commit \u002738ada5ee0c9c\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: Consumers can run the guardrail against generated MigrationOperation input and a DVault metadata baseline from a metadata model, registry, Code-First declaration, or configured DbContext, and the analysis does not require a live database connection. (Observed public AnalyzeReport overloads for DataVaultDiagnosticsResult, DataVaultMetadataModel, DataVaultMetadataRegistry, code-first Action\u003CDataVaultCodeFirstModelBuilder\u003E, and DbContext; the implementation builds from diagnostics explain metadata and not SQL or a live schema diff.).",
    "AC check passed: Safe changes remain quiet: non-DVault tables are ignored, and safe satellite payload evolution does not emit findings. (Unit test AnalyzeMigrationOperationsKeepsSafeMatrixQuiet covers non-DVault table drop and safe satellite payload add/drop/rename/alter cases, and the analyzer returns no findings for non-baseline tables and satellite payload evolution.).",
    "AC check passed: Hub and link payload-column additions are reported as insert-only violations instead of being treated as safe schema growth. (AnalyzeAddColumn reports DVM2001 for new non-baseline columns on hub and link tables, and the unit matrix asserts HubCustomer CustomerStatus is reported as an MI-1 insert-only violation.).",
    "AC check passed: Documentation includes one pre-integration example that shows how to surface the structured result and fail a local script or CI/build step before applying a migration. (docs/plans/deferred-data-vault-capabilities.md adds a Migration Guardrail Pre-Apply Example that calls AnalyzeReport, writes ToDisplayString, and sets Environment.ExitCode when report.HasFindings is true.).",
    "DoD check passed: The chosen reusable guardrail API is public, covered by API snapshot updates if needed, and returns a stable diagnostics/report contract suitable for automation. (The API is public in the approved public API snapshot, including DataVaultMigrationOperationDiagnostics and DataVaultMigrationGuardrailReport/DataVaultMigrationGuardrailIssue.).",
    "DoD check passed: Any new migration guardrail catalog entries define code, severity, category, summary, explanation, and remediation in the central diagnostics catalog pattern. (No new DVM codes were added; the existing DVM2001-DVM2006 catalog entries remain centralized with code, severity, category, summary, explanation, and remediation, with DVM2002-DVM2004 and DVM2006 text updated.).",
    "DoD check passed: Integration coverage proves the guardrail can run from a configured DbContext without applying a migration or requiring a live database round-trip. (DataVaultDiagnosticsIntegrationTests now calls AnalyzeReport(diagnostics, context, operations) from a configured DbContext and asserts the report issue, remediation, display output, and invalid result without applying a migration.).",
    "DoD check passed: A minimal doc/example is added and kept consistent with current package names, current branch limitations, and the no-SQL-parsing design. (The added doc example uses current package names, DataVaultPitMetadata-era docs context, MigrationOperation input, diagnostics explain metadata, and explicitly states the no-SQL-parsing/no-live-database design.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Risky changes to DVault-owned hub, link, satellite, PIT, or bridge tables emit stable DVM diagnostics with deterministic severity, code, path, message, and remediation guidance. (Most risky operations produce DVM issues and report remediation, but AddPrimaryKeyOperation with an unexpected DVault primary-key name is ignored, so that risky mismatch gets no code, severity, path, message, or remediation.).",
    "AC check failed: Guardrails cover required technical columns, stable key/parent/participant/driving columns, PIT snapshot-reference columns, hierarchy bridge TraversalDepth, DVault-owned table drops, and missing or mismatched DVault primary-key/index/uniqueness contracts. (Column, PIT, bridge, drop-table, drop-index, and drop-primary-key cases are covered, but DataVaultMigrationOperationDiagnostics.cs lines 299-302 return no issue when AddPrimaryKeyOperation.Name differs from the expected DataVault primary key name, leaving a mismatched primary-key contract uncovered.).",
    "DoD check failed: Unit tests cover quiet and finding cases across hub, link, satellite, PIT, and bridge baselines with representative EF migration operation types. (Unit tests cover many quiet and finding cases across hub, link, satellite, PIT, and bridge, but git grep found AddPrimaryKeyOperation only in source and not tests, matching the uncovered primary-key mismatch defect.).",
    "Blocking: AddPrimaryKeyOperation with a wrong DVault primary-key name is silently ignored, so the required mismatched primary-key contract guardrail is incomplete.",
    "Blocking test gap: the implemented AddPrimaryKeyOperation path has no unit coverage, which allowed the primary-key name mismatch branch to remain unverified."
  ],
  "evidence": [
    "git rev-parse --abbrev-ref HEAD returned develop, so the review used explicit target commit 38ada5ee0c9c against develop rather than relying on the checked-out branch.",
    "git diff --name-only develop...38ada5ee0c9c excluding .gicket listed product/doc/test changes in docs/plans/deferred-data-vault-capabilities.md, src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs, DataVaultMigrationGuardrailIssue.cs, DataVaultMigrationGuardrailReport.cs, DataVaultMigrationOperationDiagnostics.cs, integration/unit tests, and the public API snapshot.",
    "git ls-tree -r --name-only 38ada5ee0c9c confirmed src/DCoding.Data.DVault/DataVaultMigrationGuardrailIssue.cs and DataVaultMigrationGuardrailReport.cs exist at the claimed commit; the DCoding.Data.DVault SDK-style csproj uses default compile inclusion.",
    "DataVaultMigrationOperationDiagnostics.cs exposes public AnalyzeReport overloads for baseline, metadata model, registry, code-first callback, and DbContext, and expands the baseline filter to Hub, Link, Satellite, Pit, and Bridge.",
    "DataVaultMigrationGuardrailReport.cs exposes Issues, HasFindings, IsValid, ToDisplayString, and maps DVM issues to central catalog remediation.",
    "Unit diff adds PIT and bridge metadata to CreateMigrationGuardrailMetadataModel and finding cases for PIT snapshot columns, bridge TraversalDepth, DropIndexOperation, DropPrimaryKeyOperation, and bridge table drops.",
    "git grep -n -A 18 AnalyzeAddPrimaryKey at 38ada5ee0c9c shows the method returns [] when operation.Name does not equal entity.PrimaryKey.Name, before checking/reporting a mismatch.",
    "git grep AddPrimaryKeyOperation at 38ada5ee0c9c found only src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs references and no unit test references.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/diagnostics, area/ef-core, area/migrations, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.2].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 3 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F1XPVPKVGYKCV04PY98TSS78-story-add-dvault-design-time-services-for-dotnet\u0027.",
    "Ticket history references implementation commit \u002738ada5ee0c9c\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Update AnalyzeAddPrimaryKey so DVault primary-key name or column mismatches emit deterministic DVM2004 issues instead of returning quiet for wrong names.",
    "Add unit tests for AddPrimaryKeyOperation wrong-name and wrong-column cases; consider adding RenameIndexOperation coverage because it is implemented but not currently exercised.",
    "After the fix, run the declared verification commands in a writable supported environment: dotnet test DVault.slnx --nologo and bash tools/check-format.sh."
  ],
  "branchName": "ticket/06F1XPTCGWTJHHQVNPN13KANMG-story-add-ef-migration-guardrails-for-data-vault",
  "commitSha": "38ada5ee0c9c"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F1XPTCGWTJHHQVNPN13KANMG`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F1XPTCGWTJHHQVNPN13KANMG-story-add-ef-migration-guardrails-for-data-vault`