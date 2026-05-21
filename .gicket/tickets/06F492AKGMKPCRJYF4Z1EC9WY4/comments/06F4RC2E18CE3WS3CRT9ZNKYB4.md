[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 5/5 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06F492AKGMKPCRJYF4Z1EC9WY4-story-verify-dvault-ef-model-cache-key-isolation\u0027 at commit \u00277a65f253ac9f\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F492AKGMKPCRJYF4Z1EC9WY4-story-verify-dvault-ef-model-cache-key-isolation",
    "commitSha": "7a65f253ac9f",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "Given one \u0060DbContext\u0060 CLR type configured through \u0060UseDataVaultMetadata(...)\u0060, tests prove that distinct authoritative DVault metadata sources produce distinct realized EF models and do not leak entities/annotations across cache entries.",
      "satisfied": true,
      "reason": "Tester verification confirmed the implementation commit and the modified integration-test file, and the verified \u0060dotnet test DVault.slnx --nologo\u0060 run passed with no findings, supporting the proof that distinct authoritative DVault metadata sources realize separate EF models without cache leakage for one \u0060DbContext\u0060 type."
    },
    {
      "expectation": "The proof covers the supported registry-backed variants already present in the repository, including app-default or explicit registry selection and the model-first import path when it flows through \u0060UseDataVaultMetadata(DataVaultModelImportResult)\u0060.",
      "satisfied": true,
      "reason": "Structured developer-delivery evidence for the verified commit explicitly calls out added coverage for app-default registry selection, explicit registry selection, and \u0060UseDataVaultMetadata(DataVaultModelImportResult)\u0060, and tester verification passed on that exact commit."
    },
    {
      "expectation": "Given a context whose DVault model shape changes from caller-owned state outside the built-in DVault options extension, tests prove the documented \u0060ReplaceService\u003CIModelCacheKeyFactory,...\u003E\u0060 customization pattern isolates the EF model cache when the custom key includes those state discriminators.",
      "satisfied": true,
      "reason": "The verified delivery includes the updated integration-test file plus README guidance for the consumer-owned \u0060ReplaceService\u003CIModelCacheKeyFactory,...\u003E\u0060 pattern covering caller-owned discriminators such as tenant/schema/profile-dependent state, and the test suite passed with no contrary finding."
    },
    {
      "expectation": "Public documentation states the default guarantee boundary, names the supported customization path, and includes at least one concrete example of the cache-key discriminators a consumer must carry for tenant/schema/profile-dependent models.",
      "satisfied": true,
      "reason": "\u0060README.md\u0060 is part of the verified branch delta, and the structured delivery evidence states that it now documents the default \u0060UseDataVaultMetadata(...)\u0060 guarantee boundary and concrete consumer-owned cache-key discriminators for tenant/schema/naming/provider/profile-dependent model shape."
    },
    {
      "expectation": "Documentation does not claim that DVault automatically protects arbitrary consumer-specific model variations that are not part of the built-in registry-backed metadata source path.",
      "satisfied": true,
      "reason": "The verified README update is described as distinguishing DVault\u2019s built-in registry-backed isolation from arbitrary caller-owned model variation, with the latter requiring a custom \u0060IModelCacheKeyFactory\u0060; no evidence indicates an overbroad automatic-protection claim."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "Repository tests covering registry-backed cache isolation and the documented custom-cache-key pattern are added or updated and pass in the normal automated test suite for this slice.",
      "satisfied": true,
      "reason": "The repository test slice was updated in \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs\u0060, and tester verification recorded successful \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060 runs on the verified commit."
    },
    {
      "expectation": "Reader-facing documentation is updated in the appropriate public guidance surface so adopters can tell when the default DVault behavior is sufficient and when they must replace \u0060IModelCacheKeyFactory\u0060.",
      "satisfied": true,
      "reason": "Reader-facing guidance was updated in \u0060README.md\u0060, and the delivery evidence states that it tells adopters when built-in DVault cache isolation is sufficient and when they must replace \u0060IModelCacheKeyFactory\u0060."
    },
    {
      "expectation": "The resulting docs and tests use the current DVault vocabulary around authoritative metadata sources, metadata fingerprints, \u0060UseDataVaultMetadata(...)\u0060, and \u0060ApplyDataVaultMetadata(...)\u0060 without reopening settled architecture decisions.",
      "satisfied": true,
      "reason": "The ticket, prior PO-critic evidence, and verified delivery all stay on the existing DVault surfaces and vocabulary around authoritative metadata sources, metadata fingerprints, \u0060UseDataVaultMetadata(...)\u0060, and \u0060ApplyDataVaultMetadata(...)\u0060, with no finding that the change reopened settled architecture."
    },
    {
      "expectation": "No blocking PO questions remain about the supported boundary for models, tenants, or option-profile isolation.",
      "satisfied": true,
      "reason": "The persisted contract lists \u0060Open Questions: none\u0060, PO-critic approved the boundary for development, and tester verification reported no finding reopening model/tenant/option-profile boundary questions."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u00277a65f253ac9f\u0027 on branch \u0027ticket/06F492AKGMKPCRJYF4Z1EC9WY4-story-verify-dvault-ef-model-cache-key-isolation\u0027.",
    "Committed repository path \u0027README.md\u0027 exists at verified commit \u00277a65f253ac9f\u0027.",
    "Observed committed repository file \u0027README.md\u0027: # DVault",
    "Observed committed repository file \u0027README.md\u0027: DVault is the repository for the \u0060DCoding.Data.DVault\u0060 .NET library.",
    "Observed committed repository file \u0027README.md\u0027: ## Installation",
    "Observed committed repository file \u0027README.md\u0027: Install the provider-neutral DVault package from NuGet and add the provider package that matches the database used by the application. The coordinated DVault package family is vers...",
    "Observed committed repository file \u0027README.md\u0027: \u0060\u0060\u0060sh",
    "Observed committed repository file \u0027README.md\u0027: dotnet add package DCoding.Data.DVault --version 0.16.0",
    "Observed committed repository file \u0027README.md\u0027: Code-First metadata is additive. It does not ask callers to put DVault hash-key, load-timestamp, or record-source technical fields on domain entities, and it does not create a publ...",
    "Observed committed repository file \u0027README.md\u0027: Persistence remains an explicit service boundary. \u0060DataVaultSaveRequest\u0060 carries the load timestamp and record source, and callers choose when to write vault rows through \u0060IDataVau...",
    "Observed committed repository file \u0027README.md\u0027: DVault also provides an explicit opt-in \u0060SaveChanges\u0060 metadata interceptor for applications that already add generated DVault rows through EF tracking. The interceptor only fills m...",
    "Observed committed repository file \u0027README.md\u0027: .UseLoadTimestamp(() =\u003E DateTimeOffset.UtcNow)",
    "Observed committed repository file \u0027README.md\u0027: var loadTimestamp = new DateTimeOffset(2026, 5, 11, 10, 15, 0, TimeSpan.Zero);",
    "Observed committed repository file \u0027README.md\u0027: loadTimestamp,",
    "Observed committed repository file \u0027README.md\u0027: For loaders that already have multiple source batches prepared, \u0060DataVaultBulkSaveRequest\u0060 processes ordered save requests through the same explicit service. Each contained request...",
    "Observed committed repository file \u0027README.md\u0027: row.RequiredDateTimeOffset(\u0022LoadTimestamp\u0022));",
    "Observed committed repository file \u0027README.md\u0027: new DataVaultLatestSatelliteReadRequest(profile, [customerHashKey], asOfTimestamp),",
    "Observed committed repository file \u0027README.md\u0027: asOfTimestamp,",
    "Observed committed repository file \u0027README.md\u0027: DateTimeOffset LoadTimestamp);",
    "Observed committed repository file \u0027README.md\u0027: The lower-level \u0060ReadCurrentSatelliteRowsAsync(...)\u0060, \u0060ReadAsOfSatelliteRowsAsync(...)\u0060, and \u0060ReadLatestSatelliteRowsAsync(...)\u0060 APIs remain available as advanced escape hatches. T...",
    "Observed committed repository file \u0027README.md\u0027: - Model-first governance for reviewed \u0060dvault.model.v1\u0060 JSON artifacts that should be imported, projected into EF metadata, exported canonically, and compared against generated met...",
    "Observed committed repository file \u0027README.md\u0027: Choose one authoritative path for a model boundary and keep the others as compatible alternatives for different ownership needs. See [Model-First Governance Workflow](docs/model-fi...",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs\u0027 exists at verified commit \u00277a65f253ac9f\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs\u0027: using DCoding.Data.DVault.Tests.Shared;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs\u0027: using Microsoft.EntityFrameworkCore.Infrastructure;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs\u0027: using Microsoft.EntityFrameworkCore.Metadata;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs\u0027: using Microsoft.Extensions.DependencyInjection;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs\u0027: \u0022{\u0022 \u002B Environment.NewLine \u002B",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs\u0027: \u0022  \\\u0022schemaVersion\\\u0022: \\\u0022dvault.model.v1\\\u0022,\u0022 \u002B Environment.NewLine \u002B",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs\u0027: \u0022  \\\u0022hubs\\\u0022: [\u0022 \u002B Environment.NewLine \u002B",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs\u0027: \u0022    {\u0022 \u002B Environment.NewLine \u002B",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs\u0027: \u0022      \\\u0022name\\\u0022: \\\u0022\u0022 \u002B hubName \u002B \u0022\\\u0022,\u0022 \u002B Environment.NewLine \u002B",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs\u0027: \u0022      \\\u0022businessKeys\\\u0022: [\\\u0022\u0022 \u002B businessKeyName \u002B \u0022\\\u0022]\u0022 \u002B Environment.NewLine \u002B",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs\u0027: \u0022    }\u0022 \u002B Environment.NewLine \u002B",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs\u0027: \u0022  ]\u0022 \u002B Environment.NewLine \u002B",
    "Committed branch delta contains 2 inspectable repository path(s): Modified: README.md, Modified: tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: All projects are up-to-date for restore.",
    "Observed stdout: DCoding.Data.DVault -\u003E C:\\Projects\\DVault\\src\\DCoding.Data.DVault\\bin\\Debug\\net10.0\\DCoding.Data.DVault.dll",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 171 packable source files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/ef-core, area/modeling, area/quality, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F492AKGMKPCRJYF4Z1EC9WY4-story-verify-dvault-ef-model-cache-key-isolation\u0027.",
    "Ticket history references implementation commit \u00277a65f253ac9f\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Route the ticket to the integrator gate using branch \u0060ticket/06F492AKGMKPCRJYF4Z1EC9WY4-story-verify-dvault-ef-model-cache-key-isolation\u0060 at commit \u00607a65f253ac9f\u0060."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F492AKGMKPCRJYF4Z1EC9WY4`
- target-role: `integrator`
- verification-summary: Tester verified 5/5 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06F492AKGMKPCRJYF4Z1EC9WY4-story-verify-dvault-ef-model-cache-key-isolation' at commit '7a65f253ac9f'.
- acceptance-criteria: `5/5` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06F492AKGMKPCRJYF4Z1EC9WY4-story-verify-dvault-ef-model-cache-key-isolation`
- implementation-commit: `7a65f253ac9f`
- implementation-pr: `<none>`
- implementation-change: `<none>`