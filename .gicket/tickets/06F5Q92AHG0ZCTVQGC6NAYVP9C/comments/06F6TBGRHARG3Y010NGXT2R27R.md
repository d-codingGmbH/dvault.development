[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F5Q92AHG0ZCTVQGC6NAYVP9C\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite\u0027 and commit \u002706dcc4104508\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite\u0027 from source \u002706dcc4104508\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite\u0027.",
    "Evidence: git -C /mnt/c/Projects/DVault diff --name-only develop...06dcc4104508 shows the implementation is concentrated in src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs, src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs, src/DCoding.Data.DVault.Analyzers/README.md, src/DCoding.Data.DVault/DataVaultDiagnostics.cs, tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs, and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt.",
    "Evidence: docs/plans/typed-read-model-generator-contract.md:20-38 requires every generated helper to come from one resolved authoritative metadata source, with metadata-first/model-first/code-first inputs projected into the same translated EF/DVault descriptor and model-first unknown fields rejected.",
    "Evidence: src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:57-97 first scans for dvault.support-bundle.v1 additional files, but if none are present it falls back to CreateCodeFirstDeclarations, CreateMetadataFirstDeclarations, and CreateModelFirstDeclarations.",
    "Evidence: src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:492-703 parses ApplyDataVaultMetadata syntax directly, and :731-990 parses literal metadata/model-first declarations then computes fingerprints and produced names locally in CreateSatelliteDeclaration.",
    "Evidence: src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs:15-24, 45-50, 123-132, and 1216-1240 already implement strict dvault.model.v1 field validation, but src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:806-890 uses an ad hoc JsonDocument reader and does not call that parser/importer.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:93-215 adds support-bundle, metadata-first/model-first nullability fallback, stale-fingerprint, and unsupported non-string payload coverage, and src/DCoding.Data.DVault.Analyzers/README.md:52-58 documents the new typed satellite generator scope and fallback behavior.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/analyzers, area/developer-experience, area/ef-core, area/read-models, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite\u0027.",
    "Evidence: Ticket history references implementation commit \u002706dcc4104508\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Evidence: Ticket history contains 1 structured return-routing contract comment(s).",
    "Evidence: Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths.",
    "AC check passed: Generated methods use the existing latest-satellite read contract through \u0060IDataVaultReadService\u0060 and \u0060DataVaultLatestSatelliteReadRequest\u0060, or an equivalent stable direct EF projection explicitly allowed by the contract, without introducing provider-specific SQL or caller-owned projector delegates. (Generated helpers build DataVaultSatelliteMetadata and route Current/AsOf through DataVaultReadServiceCurrentSatelliteExtensions and Latest through DataVaultLatestSatelliteReadRequest plus DataVaultReadServiceTypedProjectionExtensions, without adding a new runtime read API surface.).",
    "AC check passed: Repository tests cover positive generation for representative hub-parent, link-parent, and multi-active satellite shapes plus negative diagnostics for stale fingerprints, unsupported bindings, nullability fallback, and naming-collision edge cases. (The analyzer test suite now covers positive hub-parent, link-parent, multi-active, and support-bundle generation plus stale fingerprint, unsupported non-string payload, metadata/model-first nullability fallback, and deterministic type-name collision cases.).",
    "DoD check passed: Analyzer-package implementation and tests land in the existing \u0060DCoding.Data.DVault.Analyzers\u0060 and \u0060tests/DCoding.Data.DVault.Tests/Analyzers\u0060 generator harness, and generated helpers compile against the current \u0060DCoding.Data.DVault\u0060 runtime APIs without introducing a new public runtime query surface. (The implementation and tests landed in the existing analyzer package and analyzer test harness, and the generated code targets the current IDataVaultReadService/DataVaultLatestSatelliteReadRequest/DataVaultSatelliteProjectionRow runtime surface rather than introducing a new public read-service API.).",
    "DoD check passed: Generated satellite helpers behave consistently with the current/latest/as-of satellite semantics already exposed by \u0060DataVaultReadServiceCurrentSatelliteExtensions\u0060 and \u0060DataVaultSatelliteProjectionRow\u0060. (The generated methods delegate directly to the existing current/latest/as-of satellite read helpers and project through DataVaultSatelliteProjectionRow, so their runtime behavior is aligned with the current semantics surface that already exists in the repository.).",
    "DoD check passed: Developer-facing analyzer/generator documentation is updated enough to explain the typed satellite read-model generator boundary, supported inputs, and \u0060DMV196x\u0060 failure cases. (Analyzer README documentation was updated with a typed satellite read-model generator section that explains the namespace/method boundary, preferred support-bundle input, fallback inputs, and DMV196x outcomes.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: For each supported satellite metadata declaration, the consuming compilation receives generated \u0060ReadModel\u0060 and \u0060ReadExtensions\u0060 source under the documented namespace and naming rules, with \u0060Current\u0060, \u0060Latest\u0060, and \u0060AsOf\u0060 methods bound to that satellite. (The generator emits helpers for support-bundle inputs, but when no support bundle is present it falls back to raw Code-First syntax, literal DataVaultMetadataModel constructor parsing, and ad hoc dvault.model.v1 JSON parsing instead of one authoritative normalized source, so the supported declaration paths are not fully covered per contract.).",
    "AC check failed: Generated satellite row types preserve exact produced table/column bindings and expose the parent hash key, driving keys in metadata order, \u0060HashDiff\u0060, \u0060LoadTimestamp\u0060, \u0060RecordSource\u0060, and payload properties with nullability derived from authoritative CLR/EF metadata. (The support-bundle path preserves produced bindings and authoritative nullability, but the fallback paths synthesize produced table/column names and fingerprints locally in CreateSatelliteDeclaration instead of preserving exact projected bindings and authoritative CLR/EF nullability for all supported inputs.).",
    "AC check failed: When authoritative metadata cannot be resolved deterministically, fingerprints drift, bindings or normalized public names collide, or the requested shape falls outside the bounded satellite contract, generation stops or skips with the documented \u0060DMV196x\u0060 diagnostics instead of emitting unstable helpers. (Authoritative-source failures are not handled consistently: if no support bundle is present the generator still produces helpers from fallback inputs, and the model-first path does not use the repository\u0027s strict dvault.model.v1 parser/importer to reject unknown or provider-specific fields.).",
    "DoD check failed: Regression coverage protects deterministic naming, metadata-source and fingerprint handling, payload nullability, multi-active driving-key ordering, and unsupported-shape diagnostics. (Regression coverage does not protect the contract-critical authoritative-source normalization requirement or strict model-first import behavior; the current tests explicitly accept metadata-first/model-first fallback generation without a projected authoritative descriptor.).",
    "The generator still produces satellite helpers from non-authoritative fallback inputs when no support bundle is present (src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:84-97, 492-990), which does not satisfy the contract\u0027s authoritative normalization requirement for supported code-first, metadata-first, and model-first inputs (docs/plans/typed-read-model-generator-contract.md:22-38).",
    "The model-first path bypasses the repository\u0027s strict dvault.model.v1 parser/importer. Its ad hoc JSON reader (src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:806-890) does not reject unknown/provider-specific fields the way DataVaultModelArtifactParser does (src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs:123-132, 1216-1240), so unsupported artifacts can still drive generation."
  ],
  "evidence": [
    "git -C /mnt/c/Projects/DVault diff --name-only develop...06dcc4104508 shows the implementation is concentrated in src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs, src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs, src/DCoding.Data.DVault.Analyzers/README.md, src/DCoding.Data.DVault/DataVaultDiagnostics.cs, tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs, and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt.",
    "docs/plans/typed-read-model-generator-contract.md:20-38 requires every generated helper to come from one resolved authoritative metadata source, with metadata-first/model-first/code-first inputs projected into the same translated EF/DVault descriptor and model-first unknown fields rejected.",
    "src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:57-97 first scans for dvault.support-bundle.v1 additional files, but if none are present it falls back to CreateCodeFirstDeclarations, CreateMetadataFirstDeclarations, and CreateModelFirstDeclarations.",
    "src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:492-703 parses ApplyDataVaultMetadata syntax directly, and :731-990 parses literal metadata/model-first declarations then computes fingerprints and produced names locally in CreateSatelliteDeclaration.",
    "src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs:15-24, 45-50, 123-132, and 1216-1240 already implement strict dvault.model.v1 field validation, but src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:806-890 uses an ad hoc JsonDocument reader and does not call that parser/importer.",
    "tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:93-215 adds support-bundle, metadata-first/model-first nullability fallback, stale-fingerprint, and unsupported non-string payload coverage, and src/DCoding.Data.DVault.Analyzers/README.md:52-58 documents the new typed satellite generator scope and fallback behavior.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/analyzers, area/developer-experience, area/ef-core, area/read-models, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite\u0027.",
    "Ticket history references implementation commit \u002706dcc4104508\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 1 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Rework the generator so code-first, metadata-first, and model-first inputs all flow through one authoritative projected descriptor, and fail with DMV1960/DMV1962 when that descriptor cannot be resolved deterministically.",
    "Replace the ad hoc model-first JSON path with the existing dvault.model.v1 parser/importer contract and add regression tests for unknown fields, provider-specific fields, and unresolved authoritative-source cases.",
    "After the normalization/import fixes land, rerun the declared verification commands: dotnet test DVault.slnx --nologo and bash tools/check-format.sh."
  ],
  "branchName": "ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite",
  "commitSha": "06dcc4104508"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F5Q92AHG0ZCTVQGC6NAYVP9C`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite`