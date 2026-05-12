[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F0MEFX5M9V9SA25N76CPGT4M\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F0MEFX5M9V9SA25N76CPGT4M-task-add-model-diff-and-drift-report-for-generat\u0027 and commit \u002761624c1224f2\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F0MEFX5M9V9SA25N76CPGT4M-task-add-model-diff-and-drift-report-for-generat\u0027 from source \u002761624c1224f2\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F0MEFX5M9V9SA25N76CPGT4M-task-add-model-diff-and-drift-report-for-generat\u0027.",
    "Evidence: git log shows claimed implementation commit 61624c1224f2 on the ticket branch, with later ticket writeback/claim commits; current HEAD is a9356bf924cd341698e64f48ed51ceaeee548156.",
    "Evidence: git show --name-status 61624c1224f2 adds src/DCoding.Data.DVault/DataVaultModelDriftDifference.cs, DataVaultModelDriftElementKind.cs, DataVaultModelDriftReport.cs, DataVaultModelDriftReporter.cs, DataVaultModelDriftSeverity.cs, tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelDriftReporterTests.cs, and updates the public API snapshot.",
    "Evidence: src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs:41-42 builds an expected model and compares snapshots from IReadOnlyModel metadata.",
    "Evidence: src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs:394-419 classifies primary-key property list mismatches as blocking.",
    "Evidence: src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs:484-538 classifies index property, descending-property, and included-property list mismatches as blocking.",
    "Evidence: src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs:675-679 stores primary key membership as primaryKey.Properties.Select(property =\u003E property.Name).",
    "Evidence: src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs:709-719 and 722-730 store index and descending membership as index.Properties property.Name values.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelDriftReporterTests.cs:22-36 only mutates entity ProducedName for informational name drift; no test mutates property ProducedName/EF property name while preserving logical metadata membership.",
    "Evidence: Developer verification commands dotnet test DVault.slnx --nologo and bash tools/check-format.sh were not run in this read-only review; the direct source-level blocker is sufficient for rework.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/diagnostics, area/model-first, area/testing, area/tooling, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.2].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06F0MEGAGJCEHQ8QRHGH8W7804-task-document-model-first-governance-workflow\u0027.",
    "Evidence: Ticket history references implementation commit \u002761624c1224f2\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: A deterministic drift report can be produced from in-memory model metadata without a live database connection. (DataVaultModelDriftReporter.Compare builds an expected EF model in memory from DataVaultMetadataModel/DataVaultModelImportResult and compares IReadOnlyModel metadata without database access.).",
    "AC check passed: Machine-readable output includes stable difference identifiers, severity, logical element kind/name, produced or physical name when available, expected value, actual value, and a concise message. (DataVaultModelDriftDifference exposes Code, Severity, ElementKind, LogicalName, ProducedName, ExpectedValue, ActualValue, PropertyPath, and Message.).",
    "AC check passed: Human-readable output groups or orders differences consistently so repeated runs over the same inputs produce the same content order. (SortDifferences orders by element kind, logical name, produced name, code, and property path, and ToDisplayString emits the already ordered difference list.).",
    "AC check passed: Reports identify affected model elements using both logical Data Vault metadata names and generated EF/physical names when both are available. (Differences include LogicalName and ProducedName fields, and reporter output populates them for model elements when available.).",
    "DoD check passed: Public or internal APIs needed by downstream tooling are named and documented consistently with the existing DVault metadata and diagnostics style. (The new public API names and XML docs follow existing DVault diagnostics naming style: DataVaultModelDriftReporter, DataVaultModelDriftReport, DataVaultModelDriftDifference, severity, and element kind.).",
    "DoD check passed: Report generation is culture-invariant, deterministic, and stable under repeated runs. (Observed report ordering uses StringComparer.Ordinal and CultureInfo.InvariantCulture for numeric rendering; no live database or machine state inputs are used.).",
    "DoD check passed: No live database execution, migration application, or CI gate behavior is introduced as part of this ticket. (The diff adds metadata-only source and unit test files; no migration execution, live database comparison, or CI gate behavior was introduced.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Blocking differences include missing required generated tables/entities, missing required properties, incompatible key/index definitions, incompatible property roles, incompatible timestamp storage, and incompatible provider logical storage/profile metadata. (Required blocking categories are present, but key/index property comparisons use EF property.Name values, so a produced/physical column-name-only drift can be incorrectly reported as blocking primary-key-property-mismatch or index-property-mismatch instead of an informational name-only difference.).",
    "AC check failed: Informational differences are distinguished from blocking incompatibilities and do not prevent the report from representing the full drift set. (Informational severity exists and is used for produced-name/source differences, but the key/index logic can still convert name-only property drift into blocking shape drift, so informational-only name drift is not reliably distinguished from incompatibility.).",
    "AC check failed: Representative tests cover at least one no-drift case, one informational-only case, and multiple blocking drift cases without requiring live database migration or database introspection. (Tests cover no-drift, informational entity/source drift, missing entity/property, role mismatch, timestamp/provider drift, and key/index drift, but they do not cover property produced-name-only drift through key/index membership, leaving the observed contract violation untested.).",
    "DoD check failed: The diff engine uses existing DVault naming policy and EF annotations instead of duplicating independent naming rules where repository APIs already expose the produced names. (The engine uses existing annotations for produced names but compares key/index membership through raw EF property names rather than the matched DVault logical metadata names/annotations, causing name-only drift to affect shape comparison.).",
    "DoD check failed: Unit tests or metadata-only integration tests demonstrate report contents and severity classification for representative table, column, key, index, timestamp, and provider capability drift. (Representative tests exist, but they miss the property produced-name-only case that would expose the key/index false blocking classification.).",
    "Blocking: key/index shape comparison should be based on matched logical DVault property identities, not raw EF property names. As implemented, a physical/produced column rename with unchanged logical metadata can emit blocking primary-key-property-mismatch or index-property-mismatch, violating the contract\u0027s informational handling for name-only drift."
  ],
  "evidence": [
    "git log shows claimed implementation commit 61624c1224f2 on the ticket branch, with later ticket writeback/claim commits; current HEAD is a9356bf924cd341698e64f48ed51ceaeee548156.",
    "git show --name-status 61624c1224f2 adds src/DCoding.Data.DVault/DataVaultModelDriftDifference.cs, DataVaultModelDriftElementKind.cs, DataVaultModelDriftReport.cs, DataVaultModelDriftReporter.cs, DataVaultModelDriftSeverity.cs, tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelDriftReporterTests.cs, and updates the public API snapshot.",
    "src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs:41-42 builds an expected model and compares snapshots from IReadOnlyModel metadata.",
    "src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs:394-419 classifies primary-key property list mismatches as blocking.",
    "src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs:484-538 classifies index property, descending-property, and included-property list mismatches as blocking.",
    "src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs:675-679 stores primary key membership as primaryKey.Properties.Select(property =\u003E property.Name).",
    "src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs:709-719 and 722-730 store index and descending membership as index.Properties property.Name values.",
    "tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelDriftReporterTests.cs:22-36 only mutates entity ProducedName for informational name drift; no test mutates property ProducedName/EF property name while preserving logical metadata membership.",
    "Developer verification commands dotnet test DVault.slnx --nologo and bash tools/check-format.sh were not run in this read-only review; the direct source-level blocker is sufficient for rework.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/diagnostics, area/model-first, area/testing, area/tooling, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.2].",
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
    "Ticket history references implementation branch \u0027ticket/06F0MEGAGJCEHQ8QRHGH8W7804-task-document-model-first-governance-workflow\u0027.",
    "Ticket history references implementation commit \u002761624c1224f2\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Normalize key/index, descending, and included property memberships through DVault metadata names or the already matched property snapshots before comparing shape.",
    "Add a regression test where a property\u0027s produced/physical name changes but its logical MetadataName and role remain the same; the report should stay informational and still identify both logical and physical names.",
    "After the fix, run the declared verification commands in the supported environment: dotnet test DVault.slnx --nologo and bash tools/check-format.sh."
  ],
  "branchName": "ticket/06F0MEFX5M9V9SA25N76CPGT4M-task-add-model-diff-and-drift-report-for-generat",
  "commitSha": "61624c1224f2"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F0MEFX5M9V9SA25N76CPGT4M`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F0MEFX5M9V9SA25N76CPGT4M-task-add-model-diff-and-drift-report-for-generat`