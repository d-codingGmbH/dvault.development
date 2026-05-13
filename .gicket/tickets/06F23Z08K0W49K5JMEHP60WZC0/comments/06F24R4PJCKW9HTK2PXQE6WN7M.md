[gicket-bot] PO-critic review contract

Summary
- Ready for dev: the ticket is tightly bounded to one missing release-note artifact, cites concrete repository evidence, and the persisted contract has no unresolved Open Questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F23Z08K0W49K5JMEHP60WZC0/description.md contains an authoritative delivery contract with '## Open Questions' = 'none' and defines docs/releases/v0.8.0.md as the only required repository artifact.
- docs/releases/v0.8.0.md is still absent on HEAD (repository check returned 'missing'), so the task remains the intended doc-only creation ticket rather than a mixed docs/code scope.
- docs/releases/v0.7.0.md:6-17 and 69-101 provides the concrete release-note structure and manual-publication caveat the contract tells the developer to mirror.
- docs/architecture/dvault-dotnet-ef-design-time-workflow.md:8-10, 14-32, and 173-193 fixes the supported boundary to a consumer-owned IDesignTimeDbContextFactory<TContext>, single-project layout, explicit preflight, and no DVault-owned IDesignTimeServices or EF CLI shim.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs:10-23 and 69-217 proves stable DVM2001-DVM2006 definitions and deterministic DataVaultMigrationOperationDiagnostics.AnalyzeReport(...) findings.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelFirstDesignTimeWorkflowTests.cs:10-25 proves the non-live drift lane from imported dvault.model.v1 metadata through UseDataVaultMetadata(importResult) and DataVaultModelDriftReporter.Compare(importResult, context).
- tests/DCoding.Data.DVault.Tests/Integration/SqliteLiveSchemaDriftTests.cs:12-34 and 136-168 proves the SQLite live-schema success lane plus explicit UnsupportedProvider and Unavailable outcomes.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- There is still no operator-facing end-to-end sample that chains dotnet ef migrations add, consumer preflight, and dotnet ef database update; the ticket already treats that as follow-up rather than current scope.
- The next non-SQLite live-schema provider remains intentionally unspecified after v0.8.0.
- There is still no single consolidated operator guide that combines artifact review, non-live comparison, and live-schema comparison into one workflow.

Risky assumptions
- Developers must not follow the older 'design-time services' shorthand still present in .gicket/releases/06F1XPRJZBEZFGF8XMH6RCPSS4.json and .gicket/tickets/06F1XPRY3ZDB6W1WQ9ABRRJ2V4/description.md; the current ticket contract and docs/architecture/dvault-dotnet-ef-design-time-workflow.md are the authoritative boundary.
- The release note must treat non-live drift evidence as generated/current EF metadata or ModelSnapshot-style evidence and not imply a separate DVault-owned CLI surface.
- The v0.7.0-style manual-publication caveat must remain explicit so the note does not imply package publication already happened.

AC / test suggestions
- When drafting docs/releases/v0.8.0.md, preserve explicit citations to DataVaultMigrationOperationDiagnosticsTests, DataVaultModelFirstDesignTimeWorkflowTests, and SqliteLiveSchemaDriftTests.
- Optionally cite tests/DCoding.Data.DVault.Tests/Unit/DataVaultDotnetEfDesignTimeWorkflowTests.cs:14-30 and 41-63 as extra direct proof for the consumer-owned design-time preflight workflow.
- Do a final section-by-section comparison against docs/releases/v0.7.0.md to confirm package scope, compatibility notes, known limitations, and validation evidence are all still present.

Implementation watchouts
- Keep the design-time wording aligned with docs/architecture/dvault-dotnet-ef-design-time-workflow.md: consumer-owned factory, single-project ownership, explicit preflight, no DVault-owned IDesignTimeServices, no EF CLI interception.
- Keep migration-guardrail claims anchored to deterministic DVM2001-DVM2006 coverage and AnalyzeReport(...) behavior already proven in DataVaultMigrationOperationDiagnosticsTests.
- Keep drift wording split between the non-live metadata comparison lane and the optional SQLite-first live-schema lane, with explicit UnsupportedProvider and Unavailable outcomes elsewhere.
- Mirror the v0.7.0 release-note package-family and manual-publication caveats without adding publication, provider breadth, or repair-workflow claims the repository does not prove.

Non-blocking notes
- The parent relation already exists at .gicket/relations/V4/C0/06F1XPRY3ZDB6W1WQ9ABRRJ2V4--06F23Z08K0W49K5JMEHP60WZC0--parentOf.json, so no ticket-structure cleanup is needed before dev pickup.

Split recommendations
- No split recommended; the scope remains one missing release-document artifact under epic 06F1XPRY3ZDB6W1WQ9ABRRJ2V4, and the prerequisite design-time and drift implementation stories are already done.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment