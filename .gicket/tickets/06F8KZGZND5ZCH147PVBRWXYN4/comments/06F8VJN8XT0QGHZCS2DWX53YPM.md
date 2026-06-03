[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 5/5 acceptance criteria and 3/3 definition-of-done expectations on branch \u0027ticket/06F8KZGZND5ZCH147PVBRWXYN4-story-add-ef-lifecycle-analyzer-fixtures-and-reg\u0027 at commit \u00273f7b7fe9270a\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F8KZGZND5ZCH147PVBRWXYN4-story-add-ef-lifecycle-analyzer-fixtures-and-reg",
    "commitSha": "3f7b7fe9270a",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "Analyzer tests prove DMV1912 when caller-owned instance state visibly changes the direct ApplyDataVaultMetadata(...) DVault model shape and the visible model-cache key path omits the varying discriminator.",
      "satisfied": true,
      "reason": "Verification evidence shows DataVaultEfCoreMisuseAnalyzerTests.cs adds ReportsMissingCacheKeyWhenDirectCodeFirstDeclarationUsesContextState and covers DMV1912 for direct ApplyDataVaultMetadata(...) caller-state variation with an omitted cache-key discriminator."
    },
    {
      "expectation": "Analyzer tests prove no DMV1912 for UseDataVaultMetadata(), UseDataVaultMetadata(DataVaultMetadataModel), UseDataVaultMetadata(DataVaultMetadataRegistry), and UseDataVaultMetadata(DataVaultModelImportResult) baselines that already carry DVault metadata-source isolation.",
      "satisfied": true,
      "reason": "Verification evidence shows DoesNotReportMissingCacheKeyForMetadataFirstRegistryBackedOptions and DoesNotReportMissingCacheKeyForModelFirstImportResultOptions, covering UseDataVaultMetadata(), DataVaultMetadataModel, DataVaultMetadataRegistry, and DataVaultModelImportResult non-diagnostic baselines."
    },
    {
      "expectation": "Analyzer tests prove DMV1913 only for source-visible UseModel(...) on variable-shape DVault contexts and prove no diagnostic for fixed-shape or visible design-model-to-runtime-model lanes described in docs/architecture/dvault-ef-compiled-compatibility.md.",
      "satisfied": true,
      "reason": "Verification evidence says the analyzer test file contains UseModel(...) positive and negative coverage, and the verified analyzer README defines DMV1913 only for source-visible variable-shape UseModel(...) misuse, not fixed-shape or documented design-model-to-runtime-model lanes."
    },
    {
      "expectation": "Analyzer tests prove DMV1914 only for direct AddDbContextPool\u003CTContext\u003E(...) on variable-shape DVault contexts and prove no diagnostic for fixed options-only pooled contexts.",
      "satisfied": true,
      "reason": "Verification evidence says the analyzer test file contains AddDbContextPool\u003CTContext\u003E(...) positive and negative coverage, and the verified analyzer README defines DMV1914 only for direct variable-shape pooling misuse, not fixed options-only pooled contexts."
    },
    {
      "expectation": "Regression coverage preserves documented non-diagnostic outcomes for read-only generated-table query patterns, compiled queries, metadata-interceptor opt-in, arbitrary non-DVault dictionary shared-type tables, and intentionally opaque cache-key computations.",
      "satisfied": true,
      "reason": "Verification evidence explicitly notes preserved non-diagnostic cases for read-only generated-table query patterns, compiled queries, metadata-interceptor opt-in, arbitrary dictionary shared-type tables, and opaque helper/cache-key lanes."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "New lifecycle fixtures run in the analyzer test project and distinguish code-first unsafe cases from metadata-first and model-first non-diagnostic baselines with readable, maintainable sources.",
      "satisfied": true,
      "reason": "dotnet test DVault.slnx --nologo succeeded, and the verified additions are behavior-named analyzer tests that separate code-first unsafe, metadata-first, and model-first baselines in the analyzer test project."
    },
    {
      "expectation": "Any failing gap exposed by the new fixtures is resolved with the minimum analyzer/test change set required to satisfy the documented lifecycle contract.",
      "satisfied": true,
      "reason": "No unresolved gap remains at the verified commit: the branch delta is limited to README.md, src/DCoding.Data.DVault.Analyzers/README.md, and the analyzer test file, and both deterministic verification commands passed."
    },
    {
      "expectation": "No existing DMV1910/DMV1911 misuse coverage or current DMV1912 through DMV1914 behavior regresses outside the explicitly intended new declaration-path coverage.",
      "satisfied": true,
      "reason": "The analyzer test project passed, the test file still asserts supported diagnostics DMV1910 through DMV1914, and verification reported no regression findings for existing misuse coverage or current lifecycle behavior."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u00273f7b7fe9270a\u0027 on branch \u0027ticket/06F8KZGZND5ZCH147PVBRWXYN4-story-add-ef-lifecycle-analyzer-fixtures-and-reg\u0027.",
    "Committed repository path \u0027README.md\u0027 exists at verified commit \u00273f7b7fe9270a\u0027.",
    "Observed committed repository file \u0027README.md\u0027: # DVault",
    "Observed committed repository file \u0027README.md\u0027: DVault is the repository for the \u0060DCoding.Data.DVault\u0060 .NET library.",
    "Observed committed repository file \u0027README.md\u0027: ## Installation",
    "Observed committed repository file \u0027README.md\u0027: Install the provider-neutral DVault package from NuGet and add the provider package that matches the database used by the application. The coordinated DVault package family is vers...",
    "Observed committed repository file \u0027README.md\u0027: \u0060\u0060\u0060sh",
    "Observed committed repository file \u0027README.md\u0027: dotnet add package DCoding.Data.DVault --version 0.26.0",
    "Observed committed repository file \u0027README.md\u0027: Code-First metadata is additive. It does not ask callers to put DVault hash-key, load-timestamp, or record-source technical fields on domain entities, and it does not create a publ...",
    "Observed committed repository file \u0027README.md\u0027: Persistence remains an explicit service boundary. \u0060DataVaultSaveRequest\u0060 carries the load timestamp and record source, and callers choose when to write vault rows through \u0060IDataVau...",
    "Observed committed repository file \u0027README.md\u0027: DVault also provides an explicit opt-in \u0060SaveChanges\u0060 metadata interceptor for applications that already add generated DVault rows through EF tracking. The interceptor only fills m...",
    "Observed committed repository file \u0027README.md\u0027: .UseLoadTimestamp(() =\u003E DateTimeOffset.UtcNow)",
    "Observed committed repository file \u0027README.md\u0027: var loadTimestamp = new DateTimeOffset(2026, 5, 11, 10, 15, 0, TimeSpan.Zero);",
    "Observed committed repository file \u0027README.md\u0027: loadTimestamp,",
    "Observed committed repository file \u0027README.md\u0027: For loaders that already have multiple source batches prepared, \u0060DataVaultBulkSaveRequest\u0060 processes ordered save requests through the same explicit service. Each contained request...",
    "Observed committed repository file \u0027README.md\u0027: For bounded loaders that should not materialize the complete ordered request set before saving, \u0060DataVaultChunkedSaveRequest\u0060 and \u0060DataVaultSaveChunk\u0060 are additive explicit-save in...",
    "Observed committed repository file \u0027README.md\u0027: BuildOrderedRequests(loadTimestamp, \u0022crm-import\u0022);",
    "Observed committed repository file \u0027README.md\u0027: The current coordinated release baseline is [DVault v0.26.0 Release Notes](docs/releases/v0.26.0.md), which aligns provider-tuning diagnostics, benchmark artifact verifier evidence...",
    "Observed committed repository file \u0027README.md\u0027: - Model-first governance for reviewed \u0060dvault.model.v1\u0060 JSON artifacts that should be imported, projected into EF metadata, exported canonically, compared against generated metadat...",
    "Observed committed repository file \u0027README.md\u0027: Choose one authoritative path for a model boundary and keep the others as compatible alternatives for different ownership needs. See [Model-First Governance Workflow](docs/model-fi...",
    "Observed committed repository file \u0027README.md\u0027: Applications that want an early runtime check for unsafe generated-row EF tracking can opt into the separate SaveChanges guard interceptor. \u0060AddDVault()\u0060 does not enable this guard...",
    "Committed repository path \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027 exists at verified commit \u00273f7b7fe9270a\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: # DCoding.Data.DVault.Analyzers",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: Roslyn analyzers and source generators for DVault compile-time metadata declarations. The v1 package reports:",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: - \u0060DMV1901\u0060 for unsupported \u0060BusinessKey(...)\u0060, \u0060Payload(...)\u0060, or \u0060DrivingKey(...)\u0060 selector shapes.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: - \u0060DMV1902\u0060 for duplicate logical member declarations inside the same applicable Code-First builder lambda scope.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: - \u0060DMV1910\u0060 for exposing DVault generated shared-type tables as \u0060DbSet\u003CDictionary\u003Cstring, object\u003E\u003E\u0060 members on a \u0060DbContext\u0060.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: - \u0060DMV1911\u0060 for direct EF write calls against DVault generated shared-type \u0060DbSet\u003CDictionary\u003Cstring, object\u003E\u003E\u0060 sets.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: The package also provides bounded code fixes for DMV1901 anonymous-object direct-member expansion and DMV1902 later-duplicate removal. Its mapping source generator emits registry-b...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: Generated code implements the existing \u0060IDataVaultHubMapper\u003CTSource\u003E\u0060, \u0060IDataVaultLinkMapper\u003CTSource\u003E\u0060, or \u0060IDataVaultSatelliteMapper\u003CTSource\u003E\u0060 contracts and constructs \u0060DataVaultR...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: For each supported PIT, the generator emits \u0060{PitProducedName}ReadModel\u0060 and \u0060{PitProducedName}ReadExtensions\u0060 with \u0060Read...AsOfAsync\u0060 over \u0060IDataVaultReadService\u0060. The helper cons...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: The lifecycle diagnostics \u0060DMV1912\u0060 through \u0060DMV1914\u0060 are high-confidence EF Core misuse diagnostics for direct source-visible evidence only. They align with the root README sectio...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: \u0060PrivateAssets=\u0022all\u0022\u0060 keeps the analyzer local to the project that owns the declarations. The package supplies analyzer assets and does not require a runtime reference from applica...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: The analyzer does not attempt whole-application DI inference and does not treat \u0060UseDataVaultSaveChangesMetadataInterceptor(...)\u0060 as a replacement for the explicit save boundary. T...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: \u0060DMV1912\u0060 reports when direct \u0060ApplyDataVaultMetadata(...)\u0060 model projection visibly varies by caller-owned context state or branches and the directly visible \u0060IModelCacheKeyFactor...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: \u0060DMV1913\u0060 reports direct \u0060UseModel(...)\u0060 compiled-model selection when the same source visibly applies that runtime model to a DVault context whose realized model shape can vary. F...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: The lifecycle diagnostics are intentionally limited to direct syntax and semantic facts in the analyzed source: visible instance members read in \u0060OnModelCreating(...)\u0060, direct bran...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: The source generator recognizes mapping declarations from \u0060DCoding.Data.DVault\u0060 runtime attributes on one source type:",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: The typed read-model source generator emits satellite latest/current/as-of helpers, bounded PIT as-of helpers, and bounded bridge traversal helpers from one authoritative \u0060dvault.s...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: For each supported bridge, the generator emits \u0060{BridgeProducedName}ReadModel\u0060 and \u0060{BridgeProducedName}ReadExtensions\u0060 over \u0060IDataVaultReadService\u0060. Many-to-many bridges emit \u0060Rea...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: The v1 generator supports hub-parent, link-parent, and deterministic multi-active satellites whose driving keys and payload values are strings after projection into the support-bun...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: | \u0060DMV1963\u0060 | PIT metadata or request-bound PIT read-shape evidence is incomplete or outside the bounded generated-helper baseline. |",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: | \u0060DMV1964\u0060 | Bridge metadata or request-bound bridge read-shape evidence is incomplete or outside the bounded generated-helper baseline. |",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: | \u0060DMV1966\u0060 | Payload nullability cannot be proven from the support-bundle descriptor, so the generated payload property falls back to nullable. |",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: | \u0060DMV1967\u0060 | The shape would require dynamic runtime query construction, provider SQL, runtime projection selection, or unbounded traversal. |",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: | \u0060DMV1968\u0060 | A raw or residual model-first source appears outside the projected support-bundle helper contract. Complete model-first support bundles with request-bound \u0060ReadShape\u0060...",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027 exists at verified commit \u00273f7b7fe9270a\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027: using System.Collections.Immutable;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027: using DCoding.Data.DVault.Analyzers;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027: using Microsoft.CodeAnalysis;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027: using Microsoft.CodeAnalysis.CSharp;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027: using Microsoft.CodeAnalysis.Diagnostics;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027: using Microsoft.CodeAnalysis.Text;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027: options.UseLoadTimestamp(DateTimeOffset.UtcNow).UseRecordSource(\u0022seed\u0022));",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027: var descriptors = analyzer.SupportedDiagnostics.ToDictionary(descriptor =\u003E descriptor.Id, StringComparer.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027: Assert.Equal([\u0022DMV1910\u0022, \u0022DMV1911\u0022, \u0022DMV1912\u0022, \u0022DMV1913\u0022, \u0022DMV1914\u0022], analyzer.SupportedDiagnostics.Select(descriptor =\u003E descriptor.Id).ToArray());",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027: AssertDescriptor(",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027: descriptors[\u0022DMV1910\u0022],",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027: descriptors[\u0022DMV1911\u0022],",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027: descriptors[\u0022DMV1912\u0022],",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027: descriptors[\u0022DMV1913\u0022],",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027: descriptors[\u0022DMV1914\u0022],",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027: Assert.Equal(\u0022EfCore\u0022, diagnostic.Descriptor.Category);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs\u0027: public async Task DoesNotReportVariableShapeWhenVisibleCacheKeyIncludesContextDiscriminators() {",
    "Committed branch delta contains 3 inspectable repository path(s): Modified: README.md, Modified: src/DCoding.Data.DVault.Analyzers/README.md, Modified: tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: All projects are up-to-date for restore.",
    "Observed stdout: DCoding.Data.DVault -\u003E C:\\Projects\\DVault\\src\\DCoding.Data.DVault\\bin\\Debug\\net10.0\\DCoding.Data.DVault.dll",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 214 packable source files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/analyzers, area/ef-core, area/modeling, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F8KZGZND5ZCH147PVBRWXYN4-story-add-ef-lifecycle-analyzer-fixtures-and-reg\u0027.",
    "Ticket history references implementation commit \u00273f7b7fe9270a\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 1 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to integrator using verified branch ticket/06F8KZGZND5ZCH147PVBRWXYN4-story-add-ef-lifecycle-analyzer-fixtures-and-reg at commit 3f7b7fe9270a."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F8KZGZND5ZCH147PVBRWXYN4`
- target-role: `integrator`
- verification-summary: Tester verified 5/5 acceptance criteria and 3/3 definition-of-done expectations on branch 'ticket/06F8KZGZND5ZCH147PVBRWXYN4-story-add-ef-lifecycle-analyzer-fixtures-and-reg' at commit '3f7b7fe9270a'.
- acceptance-criteria: `5/5` satisfied
- definition-of-done: `3/3` satisfied
- implementation-branch: `ticket/06F8KZGZND5ZCH147PVBRWXYN4-story-add-ef-lifecycle-analyzer-fixtures-and-reg`
- implementation-commit: `3f7b7fe9270a`
- implementation-pr: `<none>`
- implementation-change: `<none>`