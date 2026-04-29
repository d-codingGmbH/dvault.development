[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706EXB75XTWD7FTRAFE5GNDCS5R\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06EXB75XTWD7FTRAFE5GNDCS5R-task-provide-override-points-for-naming-policies\u0027 and commit \u00275c6794c6b17c\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06EXB75XTWD7FTRAFE5GNDCS5R-task-provide-override-points-for-naming-policies\u0027 from source \u00275c6794c6b17c\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06EXB75XTWD7FTRAFE5GNDCS5R-task-provide-override-points-for-naming-policies\u0027.",
    "Evidence: git log --oneline shows 5c6794c as the DEV-IMPLEMENTATION commit, followed only by dev/test writeback and lease commits on the ticket branch.",
    "Evidence: git show --stat --oneline 5c6794c6b17c shows 9 added implementation/test files under DVault.Build.csproj, src/DVault, and tests/DVault.Tests.",
    "Evidence: git diff --name-status develop...5c6794c6b17c shows added src/DVault/Modeling/IDataVaultNamingPolicy.cs, DataVaultModelOptions.cs, DefaultDataVaultNamingPolicy.cs, DataVaultModel.cs, and tests/DVault.Tests/Modeling/NamingPolicyTests.cs.",
    "Evidence: git grep at 5c6794c6b17c shows IDataVaultNamingPolicy methods GetHubTableName, GetLinkTableName, GetSatelliteTableName, GetTechnicalColumnName, GetIndexName, and GetConstraintName in src/DVault/Modeling/IDataVaultNamingPolicy.cs.",
    "Evidence: git grep at 5c6794c6b17c shows DataVaultModel.cs calling the configured namingPolicy for hub/link/satellite table names, technical column names, index names, and constraint names.",
    "Evidence: tests/DVault.Tests/Modeling/NamingPolicyTests.cs lines 42-50 assert concrete default names including HubCustomer, SatCustomerContact, LinkCustomerOrder, CustomerHashKey, LoadTimestamp, RecordSource, HashDiff, IX_HubCustomer_CustomerId, and PK_HubCustomer.",
    "Evidence: src/DVault/Modeling/DefaultDataVaultNamingPolicy.cs lines 63-66 hard-code technical column names, and line 138 introduces NormalizePascalCase casing/tokenization behavior.",
    "Evidence: src/DVault/Modeling/DataVaultModel.cs lines 142 and 200 use DefaultDataVaultNamingPolicy.NormalizeColumnName for business-key and payload columns, extending default column naming behavior beyond the override hook.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/modeling, automation/bot-ready, backlog/initial-dvault, needs-test, type/task, bot/lease:hp-ai-2026-001.2].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 5 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06EXB75XTWD7FTRAFE5GNDCS5R-task-provide-override-points-for-naming-policies\u0027.",
    "Evidence: Ticket history references implementation commit \u00275c6794c6b17c\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: A public naming policy interface or equivalent public abstraction exists and can be implemented by consumers. (src/DVault/Modeling/IDataVaultNamingPolicy.cs defines a public IDataVaultNamingPolicy interface with public context types, and tests implement it in CustomNamingPolicy.).",
    "AC check passed: The modeling options/configuration surface accepts an optional custom naming policy and remains usable without supplying one. (src/DVault/Modeling/DataVaultModelOptions.cs exposes nullable IDataVaultNamingPolicy? NamingPolicy plus UseNamingPolicy, and DataVaultModel.Create/DataVaultModelBuilder accept optional options.).",
    "AC check passed: The public abstraction provides override coverage for hub table names, link table names, satellite table names, Data Vault technical column names, index names, and constraint names when those names are produced by the modeling flow. (The interface has methods for hub table, link table, satellite table, technical column, index, and constraint names; DataVaultModel.cs calls those methods when producing the corresponding model artifacts.).",
    "AC check passed: When no custom policy is supplied, produced names come from the built-in/default policy path automatically and require no user action. (DataVaultModelOptions.ResolveNamingPolicy returns NamingPolicy ?? DefaultDataVaultNamingPolicy.Instance, so callers without custom configuration automatically use a built-in policy path.).",
    "AC check passed: A custom-policy test demonstrates that caller-provided policy output is used for at least one produced name in each v1 family available in the modeled test scenario: hub table, link table, satellite table, technical column, index, and constraint. (tests/DVault.Tests/Modeling/NamingPolicyTests.cs includes CustomNamingPolicyOverridesEachV1NameFamily and a CustomNamingPolicy implementation returning custom hub, link, satellite, column, index, and constraint names.).",
    "AC check passed: A default-path test demonstrates that the modeling flow succeeds without supplying a naming policy and uses deterministic built-in names. (tests/DVault.Tests/Modeling/NamingPolicyTests.cs includes DefaultNamingPolicyPathBuildsDeterministicNames, which builds a model without supplying a naming policy and asserts deterministic produced names.).",
    "DoD check passed: The public API is documented with XML comments or project documentation consistent with local standards once a documentation surface exists. (Public API types added in src/DVault/Modeling include XML comments, and src/DVault/DVault.csproj enables GenerateDocumentationFile.).",
    "DoD check passed: Tests are added in the repository\u0027s established test layout, or in the first test layout created for the modeling area if none exists yet. (Tests were added under tests/DVault.Tests/Modeling/NamingPolicyTests.cs with tests/DVault.Tests/DVault.Tests.csproj, which is the first visible test layout in the reviewed branch.).",
    "DoD check passed: Shared project standards from available charter/planning context are followed. (No pre-existing src/tests layout was present on develop, and the implementation follows the DataVaultModelOptions naming host from the ticket context without adding unrelated downstream integrations.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: The implementation does not duplicate or finalize the sibling-owned default naming rules beyond the minimal default behavior required to keep this hook functional. (DefaultDataVaultNamingPolicy is a public concrete default with detailed PascalCase/tokenization behavior and hard-coded technical column conventions, and the default-path test asserts those exact conventions; this exceeds the sibling-boundary requirement for only minimal default plumbing.).",
    "DoD check failed: The implementation and tests satisfy the acceptance criteria. (The implementation and tests do not satisfy all acceptance criteria because AC7 is not met.).",
    "DoD check failed: The implementation respects the boundary with sibling ticket 06EXB75NX7Z0DY7X0BD0YFZECM and avoids making conflicting default naming decisions. (The implementation does not respect the sibling-ticket boundary because it makes concrete default table and column naming decisions in DefaultDataVaultNamingPolicy and locks them into tests.).",
    "Blocking: DefaultDataVaultNamingPolicy and its default-path tests turn detailed default naming choices into delivered behavior even though the contract assigns those decisions to sibling ticket 06EXB75NX7Z0DY7X0BD0YFZECM."
  ],
  "evidence": [
    "git log --oneline shows 5c6794c as the DEV-IMPLEMENTATION commit, followed only by dev/test writeback and lease commits on the ticket branch.",
    "git show --stat --oneline 5c6794c6b17c shows 9 added implementation/test files under DVault.Build.csproj, src/DVault, and tests/DVault.Tests.",
    "git diff --name-status develop...5c6794c6b17c shows added src/DVault/Modeling/IDataVaultNamingPolicy.cs, DataVaultModelOptions.cs, DefaultDataVaultNamingPolicy.cs, DataVaultModel.cs, and tests/DVault.Tests/Modeling/NamingPolicyTests.cs.",
    "git grep at 5c6794c6b17c shows IDataVaultNamingPolicy methods GetHubTableName, GetLinkTableName, GetSatelliteTableName, GetTechnicalColumnName, GetIndexName, and GetConstraintName in src/DVault/Modeling/IDataVaultNamingPolicy.cs.",
    "git grep at 5c6794c6b17c shows DataVaultModel.cs calling the configured namingPolicy for hub/link/satellite table names, technical column names, index names, and constraint names.",
    "tests/DVault.Tests/Modeling/NamingPolicyTests.cs lines 42-50 assert concrete default names including HubCustomer, SatCustomerContact, LinkCustomerOrder, CustomerHashKey, LoadTimestamp, RecordSource, HashDiff, IX_HubCustomer_CustomerId, and PK_HubCustomer.",
    "src/DVault/Modeling/DefaultDataVaultNamingPolicy.cs lines 63-66 hard-code technical column names, and line 138 introduces NormalizePascalCase casing/tokenization behavior.",
    "src/DVault/Modeling/DataVaultModel.cs lines 142 and 200 use DefaultDataVaultNamingPolicy.NormalizeColumnName for business-key and payload columns, extending default column naming behavior beyond the override hook.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/modeling, automation/bot-ready, backlog/initial-dvault, needs-test, type/task, bot/lease:hp-ai-2026-001.2].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 5 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06EXB75XTWD7FTRAFE5GNDCS5R-task-provide-override-points-for-naming-policies\u0027.",
    "Ticket history references implementation commit \u00275c6794c6b17c\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Refactor the default path to be clearly minimal plumbing or an internal placeholder that can delegate/preserve sibling-owned defaults when they exist, without publicizing or test-locking detailed default casing and technical-column conventions.",
    "Keep the public IDataVaultNamingPolicy/DataVaultModelOptions hook and custom-policy coverage, then run the policy-defined dotnet test --nologo verification after the static boundary issue is fixed."
  ],
  "branchName": "ticket/06EXB75XTWD7FTRAFE5GNDCS5R-task-provide-override-points-for-naming-policies",
  "commitSha": "5c6794c6b17c"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06EXB75XTWD7FTRAFE5GNDCS5R`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06EXB75XTWD7FTRAFE5GNDCS5R-task-provide-override-points-for-naming-policies`