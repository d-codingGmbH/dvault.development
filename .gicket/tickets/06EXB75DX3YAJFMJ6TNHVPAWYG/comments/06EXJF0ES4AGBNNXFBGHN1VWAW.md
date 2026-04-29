[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706EXB75DX3YAJFMJ6TNHVPAWYG\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06EXB75DX3YAJFMJ6TNHVPAWYG-story-implement-deterministic-naming-conventions\u0027 and commit \u0027a49b131ff0d9\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06EXB75DX3YAJFMJ6TNHVPAWYG-story-implement-deterministic-naming-conventions\u0027 from source \u0027a49b131ff0d9\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06EXB75DX3YAJFMJ6TNHVPAWYG-story-implement-deterministic-naming-conventions\u0027.",
    "Evidence: git show --name-status --oneline -1 a49b131ff0d9 shows implementation commit a49b131f modifying src/DVault/Modeling/DataVaultModel.cs, DataVaultModelBuilder.cs, DefaultDataVaultNamingPolicy.cs, DefaultNamingPolicy.cs, IDataVaultNamingPolicy.cs, and tests/DVault.Tests files.",
    "Evidence: git diff --name-status develop...a49b131ff0d9 -- src/DVault/Modeling tests/DVault.Tests docs/naming lists only modeling source and test changes; docs/naming/default-naming-policy.md exists and was used as the normative contract.",
    "Evidence: docs/naming/default-naming-policy.md lines 15 and 43 specify explicit link names when provided and object fallback @@@ -\u003E Entity.",
    "Evidence: src/DVault/Modeling/DefaultNamingPolicy.cs lines 82-97 calls TryNormalizeObjectName for relationshipName and falls back to participant names when no semantic token is found.",
    "Evidence: src/DVault/Modeling/DefaultNamingPolicy.cs lines 234-252 show NormalizeObjectNameCore would return Entity for no semantic token, but TryNormalizeObjectName returns false for that case.",
    "Evidence: tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs lines 94-101 test @@@ fallback for hubs and invalid property fallback, but no explicit unsafe link relationship case is present.",
    "Evidence: tests/DVault.Tests/Modeling/NamingPolicyTests.cs lines 37-72 assert deterministic produced table, column, index, and constraint names; lines 157-188 assert custom policy override behavior.",
    "Evidence: git diff --check develop...a49b131ff0d9 -- src/DVault/Modeling tests/DVault.Tests exited 0.",
    "Evidence: rg for provider/database dependencies in src/DVault/Modeling returned no matches.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/modeling, automation/bot-ready, backlog/initial-dvault, needs-test, type/story, bot/lease:hp-ai-2026-001.2].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06EXB75DX3YAJFMJ6TNHVPAWYG-story-implement-deterministic-naming-conventions\u0027.",
    "Evidence: Ticket history references implementation commit \u0027a49b131ff0d9\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: Given the same model declarations and default naming policy, repeated model builds produce identical table, column, index, and constraint names in the same order. (DataVaultModelBuilder preserves declaration order in lists and NamingPolicyTests compares produced table, column, index, and constraint names across repeated CreateModel builds.).",
    "AC check passed: Default business-key and payload column names follow the documented property-column rule, including PascalCase normalization, no property singularization, documented fallbacks, unsafe property token handling, technical-column reservation, and duplicate disambiguation within the relevant column scope. (Business-key and payload columns are generated through DefaultDataVaultNamingPolicy.GetColumnNames with technical names reserved, and tests cover HashDiffValue, CustomerHashKeyValue, LoadTimestampValue, RecordSourceValue, and duplicate suffixes.).",
    "AC check passed: Default technical columns are named according to the documented Data Vault concepts: {Base}HashKey, HashDiff, LoadTimestamp, and RecordSource. (DefaultDataVaultNamingPolicy maps HashKey to {Base}HashKey and returns HashDiff, LoadTimestamp, and RecordSource for the other technical kinds.).",
    "AC check passed: Default index and constraint names are deterministic, derived from produced table and participating column names, and distinguish the current model index and constraint kinds visible in source during implementation. (DefaultDataVaultNamingPolicy builds Ix/Pk/Fk names from produced table names, kind tokens, and participating column names; tests assert business-key, relationship, satellite-parent, and primary-key examples.).",
    "AC check passed: When no custom naming configuration is supplied, the model-building or conventions path uses the default naming policy. (DataVaultModelOptions.ResolveNamingPolicy returns DefaultDataVaultNamingPolicy.Instance when no custom policy is configured, and DataVaultConventions.Default exposes DefaultNamingPolicy.Instance.).",
    "AC check passed: A caller can supply a custom IDataVaultNamingPolicy through an existing or newly introduced provider-neutral configuration path, and the model builder uses it for hub, link, satellite, technical-column, index, and constraint name generation. (DataVaultModel.Create and DataVaultModelBuilder accept DataVaultModelOptions, UseNamingPolicy stores IDataVaultNamingPolicy, and NamingPolicyTests verify custom hub/link/satellite, technical column, index, and constraint naming calls.).",
    "AC check passed: Custom-policy tests demonstrate override behavior across the source-backed policy families without requiring every property-column normalization detail to be externally overridable unless the story adds such public methods. (The custom policy test asserts override behavior across table names, technical columns, indexes, and constraints without adding property-column override API.).",
    "DoD check passed: Implementation is in the existing DVault modeling source layout and follows repository formatting and nullable C# conventions. (Implementation changes are under src/DVault/Modeling and source/test scoped git diff --check exited 0.).",
    "DoD check passed: Automated tests are added or updated in the existing DVault test layout for the default policy and custom-policy path. (Tests were added or updated under tests/DVault.Tests/Modeling and tests/DVault.Tests/Program.cs wires the modeling suites into the executable test project.).",
    "DoD check passed: Public XML documentation is present for new public types or members introduced or changed for the naming-policy contract. (New and changed public members inspected in DataVaultModel, DataVaultModelBuilder, DataVaultModelOptions, DefaultNamingPolicy, DefaultDataVaultNamingPolicy, and IDataVaultNamingPolicy have XML documentation comments.).",
    "DoD check passed: Implementation remains provider-neutral and introduces no database-provider dependency or persistence execution behavior. (rg found no database-provider or persistence execution references in src/DVault/Modeling, and the changes stay in provider-neutral modeling code.).",
    "DoD check passed: Any newly introduced options/model-creation API for custom naming policy is documented as part of this story rather than treated as pre-existing. (The new optional model-creation/configuration path is documented by XML comments on DataVaultModel.Create, DataVaultModelBuilder constructors/link overloads, DataVaultModelOptions.NamingPolicy, and UseNamingPolicy.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Default hub, link, and satellite table names follow docs/naming/default-naming-policy.md, including PascalCase normalization, finite object singularization, documented fallbacks, and unsafe object token handling. (docs/naming/default-naming-policy.md says explicit link names are used when provided and @@@ normalizes as object fallback Entity; DefaultNamingPolicy.GetLinkTableName instead uses participant fallback when TryNormalizeObjectName returns no semantic token, so GetLinkTableName(\u0022@@@\u0022, [\u0022Customer\u0022]) would produce LinkCustomer rather than LinkEntity.).",
    "AC check failed: Tests demonstrate deterministic output, documented normalization examples, singular/plural object equivalence, reserved-word handling, collision behavior, index and constraint naming, and the custom naming-policy override path. (Tests cover many required categories, but they do not cover the explicit unsafe link relationship fallback that the documentation requires and the implementation currently mishandles.).",
    "DoD check failed: Relevant .NET build/test commands and repository formatting checks pass, or unavailable local tooling is explicitly reported with the attempted command. (dotnet test --nologo was not run in this read-only tester session because it would require build/test outputs; only read-only diff and grep checks were performed.).",
    "Blocking: explicit link relationship names that normalize to the object fallback are treated as missing and replaced by participant-order naming, violating the documented Link{ParticipantOrRelationshipName} rule and unsafe object token handling."
  ],
  "evidence": [
    "git show --name-status --oneline -1 a49b131ff0d9 shows implementation commit a49b131f modifying src/DVault/Modeling/DataVaultModel.cs, DataVaultModelBuilder.cs, DefaultDataVaultNamingPolicy.cs, DefaultNamingPolicy.cs, IDataVaultNamingPolicy.cs, and tests/DVault.Tests files.",
    "git diff --name-status develop...a49b131ff0d9 -- src/DVault/Modeling tests/DVault.Tests docs/naming lists only modeling source and test changes; docs/naming/default-naming-policy.md exists and was used as the normative contract.",
    "docs/naming/default-naming-policy.md lines 15 and 43 specify explicit link names when provided and object fallback @@@ -\u003E Entity.",
    "src/DVault/Modeling/DefaultNamingPolicy.cs lines 82-97 calls TryNormalizeObjectName for relationshipName and falls back to participant names when no semantic token is found.",
    "src/DVault/Modeling/DefaultNamingPolicy.cs lines 234-252 show NormalizeObjectNameCore would return Entity for no semantic token, but TryNormalizeObjectName returns false for that case.",
    "tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs lines 94-101 test @@@ fallback for hubs and invalid property fallback, but no explicit unsafe link relationship case is present.",
    "tests/DVault.Tests/Modeling/NamingPolicyTests.cs lines 37-72 assert deterministic produced table, column, index, and constraint names; lines 157-188 assert custom policy override behavior.",
    "git diff --check develop...a49b131ff0d9 -- src/DVault/Modeling tests/DVault.Tests exited 0.",
    "rg for provider/database dependencies in src/DVault/Modeling returned no matches.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/modeling, automation/bot-ready, backlog/initial-dvault, needs-test, type/story, bot/lease:hp-ai-2026-001.2].",
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
    "Ticket history references implementation branch \u0027ticket/06EXB75DX3YAJFMJ6TNHVPAWYG-story-implement-deterministic-naming-conventions\u0027.",
    "Ticket history references implementation commit \u0027a49b131ff0d9\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Change DefaultNamingPolicy.GetLinkTableName so a non-null/non-whitespace relationship name always uses NormalizeObjectName, including the Entity fallback, and only null/whitespace relationship names use participant fallback.",
    "Add tests for explicit unsafe link relationship names such as @@@ producing LinkEntity through DefaultNamingPolicy and the model builder path.",
    "After the fix, run deterministic verification for dotnet test --nologo in a writable legacy/test environment."
  ],
  "branchName": "ticket/06EXB75DX3YAJFMJ6TNHVPAWYG-story-implement-deterministic-naming-conventions",
  "commitSha": "a49b131ff0d9"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06EXB75DX3YAJFMJ6TNHVPAWYG`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06EXB75DX3YAJFMJ6TNHVPAWYG-story-implement-deterministic-naming-conventions`