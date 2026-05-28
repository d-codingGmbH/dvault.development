[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 4/4 acceptance criteria and 3/3 definition-of-done expectations on branch \u0027ticket/06F5Q92YGB53W7YG6VCMA3FZJR-story-add-analyzers-and-code-fixes-for-typed-rea\u0027 at commit \u0027f89c6846353b\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F5Q92YGB53W7YG6VCMA3FZJR-story-add-analyzers-and-code-fixes-for-typed-rea",
    "commitSha": "f89c6846353b",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "Unsupported PIT or bridge typed-read shapes visible through the authoritative dvault.support-bundle.v1 path report the appropriate DMV1963, DMV1964, DMV1967, or DMV1969 outcome instead of being silently skipped.",
      "satisfied": true,
      "reason": "The developer delivery outcome says the generator no longer silently ignores PIT/PointInTime/Bridge support-bundle entities and now classifies unsupported PIT, unsupported bridge, dynamic-query-required PIT, and helper-skipped cases as diagnostics; verification also observed matching PIT/bridge diagnostic messages in the committed generator."
    },
    {
      "expectation": "Model-first-projected typed-read inputs outside the public generator contract report DMV1968 and emit no unstable helper source.",
      "satisfied": true,
      "reason": "The developer delivery outcome explicitly includes model-first unsupported classification with no helper emission, and the verified generator commit passed the recorded test run."
    },
    {
      "expectation": "Existing satellite behavior for DMV1960, DMV1961, DMV1962, DMV1965, and DMV1966 remains unchanged.",
      "satisfied": true,
      "reason": "The developer delivery outcome says supported satellite generation was preserved, verification observed satellite-generation assertions in the modified test file, and dotnet test succeeded."
    },
    {
      "expectation": "No typed-read code fix is offered for DMV1963, DMV1964, DMV1967, DMV1968, or DMV1969.",
      "satisfied": true,
      "reason": "The verified branch delta contains only source-generator and source-generator-test changes, with no delivered code-fix-provider changes, so this ticket does not add a typed-read code fix for DMV1963, DMV1964, DMV1967, DMV1968, or DMV1969."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "Analyzer or generator tests cover each residual diagnostic id and verify no-helper-emission behavior for the affected shapes.",
      "satisfied": true,
      "reason": "The developer delivery outcome states that generator tests were added for each residual diagnostic id with no-helper-emission assertions, the modified generator test file is present in the verified commit, and dotnet test succeeded."
    },
    {
      "expectation": "Residual diagnostic paths do not regress supported satellite helper generation.",
      "satisfied": true,
      "reason": "The developer delivery outcome states that supported satellite helper generation was preserved, verification observed satellite helper assertions in the test file, and the recorded test run passed."
    },
    {
      "expectation": "No new typed-read code-fix provider or runtime read or write API is added.",
      "satisfied": true,
      "reason": "The verified branch delta contains only the typed-read source generator and its tests, with no evidence of a new typed-read code-fix provider or runtime read/write API addition."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u0027f89c6846353b\u0027 on branch \u0027ticket/06F5Q92YGB53W7YG6VCMA3FZJR-story-add-analyzers-and-code-fixes-for-typed-rea\u0027.",
    "Committed repository path \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027 exists at verified commit \u0027f89c6846353b\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: using System.Collections.Immutable;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: using System.Text;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: using System.Text.Json;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: using Microsoft.CodeAnalysis;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: using Microsoft.CodeAnalysis.CSharp;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: using Microsoft.CodeAnalysis.Diagnostics;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: !HasSupportBundleTechnicalProperty(entity, \u0022LoadTimestamp\u0022) ||",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: \u0022authoritative explain metadata is missing the PIT parent reference, parent hash key, load timestamp, or satellite snapshot reference binding.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: \u0022PIT driving-key tuple projection requires dynamic runtime query behavior outside the residual generator helper contract.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: \u0022the runtime PIT metadata shape is valid for IDataVaultReadService usage but no typed PIT helper is emitted by this diagnostic-only generator path.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: \u0022the runtime hierarchy bridge metadata shape is valid for IDataVaultReadService usage but no typed bridge helper is emitted by this diagnostic-only generator path.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: \u0022the runtime bridge metadata shape is valid for IDataVaultReadService usage but no typed bridge helper is emitted by this diagnostic-only generator path.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: \u0022\u0027 contains a satellite entity whose produced name, metadata name, parent reference, or property descriptor cannot be resolved.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: if (!TryCreateSupportBundleProperty(property, sourcePath, context, out var descriptor)) {",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: properties.Add(descriptor);",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027 exists at verified commit \u0027f89c6846353b\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: using System.Collections.Immutable;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: using DCoding.Data.DVault.Analyzers;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: using Microsoft.CodeAnalysis;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: using Microsoft.CodeAnalysis.CSharp;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: using Microsoft.CodeAnalysis.Diagnostics;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: using Microsoft.CodeAnalysis.Text;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: loadTimestampColumnName: \u0022custom_col_LoadTimestamp\u0022,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: Assert.Contains(\u0022global::System.DateTimeOffset LoadTimestamp\u0022, source, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: Assert.DoesNotContain(\u0022global::System.DateTimeOffset CustomColLoadTimestamp\u0022, source, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: Assert.Contains(\u0022public const string LoadTimestampProducedColumnName = \\\u0022custom_col_LoadTimestamp\\\u0022;\u0022, source, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: Assert.Contains(\u0022public const string LoadTimestampMappedName = \\\u0022LoadTimestamp\\\u0022;\u0022, source, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: Assert.Contains(\u0022row.RequiredDateTimeOffset(\\\u0022LoadTimestamp\\\u0022)\u0022, source, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: Assert.DoesNotContain(\u0022row.RequiredDateTimeOffset(\\\u0022custom_col_LoadTimestamp\\\u0022)\u0022, source, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: RuntimeStubs,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: Assert.Empty(result.CompilationErrors);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: \u0022SatCustomerProfileRuntime\u0022,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: var source = AssertGeneratedSource(result, \u0022DVault.GeneratedReadModels.SatCustomerProfileRuntime.g.cs\u0022);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: Assert.Contains(\u0022public sealed record SatCustomerProfileRuntimeReadModel(\u0022, source, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: Assert.Contains(\u0022public const string ProducedTableName = \\\u0022SatCustomerProfileRuntime\\\u0022;\u0022, source, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: var result = RunGenerator(RuntimeStubs);",
    "Committed branch delta contains 2 inspectable repository path(s): Modified: src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs, Modified: tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: All projects are up-to-date for restore.",
    "Observed stdout: DCoding.Data.DVault -\u003E C:\\Projects\\DVault\\src\\DCoding.Data.DVault\\bin\\Debug\\net10.0\\DCoding.Data.DVault.dll",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 207 packable source files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/analyzers, area/developer-experience, area/ef-core, area/read-models, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F5Q92YGB53W7YG6VCMA3FZJR-story-add-analyzers-and-code-fixes-for-typed-rea\u0027.",
    "Ticket history references implementation commit \u0027f89c6846353b\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Route the ticket to integrator using branch ticket/06F5Q92YGB53W7YG6VCMA3FZJR-story-add-analyzers-and-code-fixes-for-typed-rea at verified commit f89c6846353b."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F5Q92YGB53W7YG6VCMA3FZJR`
- target-role: `integrator`
- verification-summary: Tester verified 4/4 acceptance criteria and 3/3 definition-of-done expectations on branch 'ticket/06F5Q92YGB53W7YG6VCMA3FZJR-story-add-analyzers-and-code-fixes-for-typed-rea' at commit 'f89c6846353b'.
- acceptance-criteria: `4/4` satisfied
- definition-of-done: `3/3` satisfied
- implementation-branch: `ticket/06F5Q92YGB53W7YG6VCMA3FZJR-story-add-analyzers-and-code-fixes-for-typed-rea`
- implementation-commit: `f89c6846353b`
- implementation-pr: `<none>`
- implementation-change: `<none>`