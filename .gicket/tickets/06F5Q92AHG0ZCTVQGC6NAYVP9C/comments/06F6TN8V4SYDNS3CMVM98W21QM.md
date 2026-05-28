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
    "Selected verification source branch \u0027ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite\u0027 and commit \u00278e0ea8742ab6\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite\u0027 from source \u00278e0ea8742ab6\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite\u0027.",
    "Evidence: \u0060git -C /mnt/c/Projects/DVault diff --name-only develop...8e0ea8742ab6\u0060 shows the implementation is concentrated in \u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0060, \u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0060, \u0060src/DCoding.Data.DVault.Analyzers/README.md\u0060, \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060.",
    "Evidence: \u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:323-339\u0060 builds row properties from support-bundle produced names and assigns \u0060HashDiff\u0060, \u0060LoadTimestamp\u0060, and \u0060RecordSource\u0060 property names from \u0060hashDiff.ProducedName\u0060, \u0060loadTimestamp.ProducedName\u0060, and \u0060recordSource.ProducedName\u0060.",
    "Evidence: \u0060docs/plans/typed-read-model-generator-contract.md:155-162\u0060 requires generated satellite rows to expose \u0060HashDiff\u0060, \u0060LoadTimestamp\u0060, and \u0060RecordSource\u0060 as public members while preserving exact produced column bindings separately in constants or binding tables.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Modeling/NamingPolicyTests.cs:288-301,448-449\u0060 shows the repository supports custom naming policies that rename technical produced columns to values beginning with \u0060custom_col_\u0060.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultSatelliteProjectionRow.cs:7-15\u0060 fixes the runtime projection mapped-name space to \u0060ParentHashKey\u0060, \u0060HashDiff\u0060, \u0060LoadTimestamp\u0060, and \u0060RecordSource\u0060.",
    "Evidence: \u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:564-609\u0060 wires generated \u0060Current\u0060, \u0060Latest\u0060, and \u0060AsOf\u0060 helpers through the existing \u0060IDataVaultReadService\u0060 latest/current/as-of APIs.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:12-274\u0060 covers positive hub/link/multi-active generation and negative \u0060DMV1960\u0060/\u0060DMV1961\u0060/\u0060DMV1962\u0060/\u0060DMV1965\u0060/\u0060DMV1966\u0060 cases, but no test covers custom technical-column produced names.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/analyzers, area/developer-experience, area/ef-core, area/read-models, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 7 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite\u0027.",
    "Evidence: Ticket history references implementation commit \u00278e0ea8742ab6\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Evidence: Ticket history contains 2 structured return-routing contract comment(s).",
    "Evidence: Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths.",
    "AC check passed: For each supported satellite metadata declaration, the consuming compilation receives generated \u0060ReadModel\u0060 and \u0060ReadExtensions\u0060 source under the documented namespace and naming rules, with \u0060Current\u0060, \u0060Latest\u0060, and \u0060AsOf\u0060 methods bound to that satellite. (\u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0060 emits \u0060{RootNamespace}.DVault.GeneratedReadModels\u0060, \u0060{ProducedName}ReadModel\u0060, \u0060{ProducedName}ReadExtensions\u0060, and \u0060Read...CurrentAsync\u0060/\u0060Read...LatestAsync\u0060/\u0060Read...AsOfAsync\u0060; \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:12-120\u0060 covers hub-parent, link-parent, and multi-active generation.).",
    "AC check passed: Generated methods use the existing latest-satellite read contract through \u0060IDataVaultReadService\u0060 and \u0060DataVaultLatestSatelliteReadRequest\u0060, or an equivalent stable direct EF projection explicitly allowed by the contract, without introducing provider-specific SQL or caller-owned projector delegates. (Generated methods call \u0060DataVaultReadServiceCurrentSatelliteExtensions.ReadCurrentSatelliteAsync\u0060, \u0060DataVaultReadServiceCurrentSatelliteExtensions.ReadAsOfSatelliteAsync\u0060, and \u0060DataVaultReadServiceTypedProjectionExtensions.ReadLatestSatelliteAsync\u0060 with \u0060DataVaultLatestSatelliteReadRequest\u0060 (\u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:564-609\u0060), matching the runtime APIs in \u0060src/DCoding.Data.DVault\u0060.).",
    "AC check passed: When authoritative metadata cannot be resolved deterministically, fingerprints drift, bindings or normalized public names collide, or the requested shape falls outside the bounded satellite contract, generation stops or skips with the documented \u0060DMV196x\u0060 diagnostics instead of emitting unstable helpers. (The generator reports \u0060DMV1960\u0060, \u0060DMV1961\u0060, \u0060DMV1962\u0060, \u0060DMV1965\u0060, and \u0060DMV1966\u0060 for missing authoritative source, fingerprint drift, unsupported non-string payloads, name collisions, and nullability fallback, and the analyzer tests exercise those cases (\u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:124-274\u0060).).",
    "AC check passed: Repository tests cover positive generation for representative hub-parent, link-parent, and multi-active satellite shapes plus negative diagnostics for stale fingerprints, unsupported bindings, nullability fallback, and naming-collision edge cases. (\u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:12-274\u0060 covers positive hub-parent, link-parent, and multi-active generation plus stale fingerprint, unsupported binding, nullability fallback, and naming-collision diagnostics.).",
    "DoD check passed: Analyzer-package implementation and tests land in the existing \u0060DCoding.Data.DVault.Analyzers\u0060 and \u0060tests/DCoding.Data.DVault.Tests/Analyzers\u0060 generator harness, and generated helpers compile against the current \u0060DCoding.Data.DVault\u0060 runtime APIs without introducing a new public runtime query surface. (The implementation landed in \u0060src/DCoding.Data.DVault.Analyzers\u0060 and \u0060tests/DCoding.Data.DVault.Tests/Analyzers\u0060, and the emitted helpers target existing runtime APIs present in \u0060src/DCoding.Data.DVault/DataVaultLatestSatelliteReadRequest.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultReadServiceCurrentSatelliteExtensions.cs\u0060, and \u0060src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs\u0060.).",
    "DoD check passed: Generated satellite helpers behave consistently with the current/latest/as-of satellite semantics already exposed by \u0060DataVaultReadServiceCurrentSatelliteExtensions\u0060 and \u0060DataVaultSatelliteProjectionRow\u0060. (\u0060Current\u0060 and \u0060AsOf\u0060 delegate through \u0060DataVaultReadServiceCurrentSatelliteExtensions\u0060, and \u0060Latest\u0060 delegates through \u0060DataVaultLatestSatelliteReadRequest\u0060 plus \u0060DataVaultReadServiceTypedProjectionExtensions\u0060, which matches the existing runtime current/latest/as-of satellite semantics.).",
    "DoD check passed: Developer-facing analyzer/generator documentation is updated enough to explain the typed satellite read-model generator boundary, supported inputs, and \u0060DMV196x\u0060 failure cases. (\u0060src/DCoding.Data.DVault.Analyzers/README.md:52-58\u0060 documents the support-bundle-only generator boundary, supported inputs, generated helper surface, and \u0060DMV196x\u0060 failure cases.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Generated satellite row types preserve exact produced table/column bindings and expose the parent hash key, driving keys in metadata order, \u0060HashDiff\u0060, \u0060LoadTimestamp\u0060, \u0060RecordSource\u0060, and payload properties with nullability derived from authoritative CLR/EF metadata. (\u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:337-339\u0060 derives the \u0060HashDiff\u0060, \u0060LoadTimestamp\u0060, and \u0060RecordSource\u0060 row member names from produced column names. The contract requires those public members to remain \u0060HashDiff\u0060, \u0060LoadTimestamp\u0060, and \u0060RecordSource\u0060 while preserving produced bindings separately (\u0060docs/plans/typed-read-model-generator-contract.md:155-162\u0060). Because custom naming can rename technical columns (\u0060tests/DCoding.Data.DVault.Tests/Modeling/NamingPolicyTests.cs:288-301,448-449\u0060), supported satellites can emit the wrong public row shape.).",
    "DoD check failed: Regression coverage protects deterministic naming, metadata-source and fingerprint handling, payload nullability, multi-active driving-key ordering, and unsupported-shape diagnostics. (Regression coverage does not protect the custom naming path that renames satellite technical produced columns. The analyzer tests assert only default technical names, so they would not catch the contract break in acceptance criterion 2.).",
    "\u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:337-339\u0060 violates the satellite contract by deriving public \u0060HashDiff\u0060, \u0060LoadTimestamp\u0060, and \u0060RecordSource\u0060 row member names from produced technical column names. With a supported custom naming policy (\u0060tests/DCoding.Data.DVault.Tests/Modeling/NamingPolicyTests.cs:288-301,448-449\u0060), the generator will emit \u0060CustomCol...\u0060 members instead of the contract-fixed technical properties required by \u0060docs/plans/typed-read-model-generator-contract.md:155-162\u0060.",
    "\u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:12-274\u0060 do not cover the custom technical-column naming path, so the regression in the public row shape is currently unprotected."
  ],
  "evidence": [
    "\u0060git -C /mnt/c/Projects/DVault diff --name-only develop...8e0ea8742ab6\u0060 shows the implementation is concentrated in \u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0060, \u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0060, \u0060src/DCoding.Data.DVault.Analyzers/README.md\u0060, \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060.",
    "\u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:323-339\u0060 builds row properties from support-bundle produced names and assigns \u0060HashDiff\u0060, \u0060LoadTimestamp\u0060, and \u0060RecordSource\u0060 property names from \u0060hashDiff.ProducedName\u0060, \u0060loadTimestamp.ProducedName\u0060, and \u0060recordSource.ProducedName\u0060.",
    "\u0060docs/plans/typed-read-model-generator-contract.md:155-162\u0060 requires generated satellite rows to expose \u0060HashDiff\u0060, \u0060LoadTimestamp\u0060, and \u0060RecordSource\u0060 as public members while preserving exact produced column bindings separately in constants or binding tables.",
    "\u0060tests/DCoding.Data.DVault.Tests/Modeling/NamingPolicyTests.cs:288-301,448-449\u0060 shows the repository supports custom naming policies that rename technical produced columns to values beginning with \u0060custom_col_\u0060.",
    "\u0060src/DCoding.Data.DVault/DataVaultSatelliteProjectionRow.cs:7-15\u0060 fixes the runtime projection mapped-name space to \u0060ParentHashKey\u0060, \u0060HashDiff\u0060, \u0060LoadTimestamp\u0060, and \u0060RecordSource\u0060.",
    "\u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:564-609\u0060 wires generated \u0060Current\u0060, \u0060Latest\u0060, and \u0060AsOf\u0060 helpers through the existing \u0060IDataVaultReadService\u0060 latest/current/as-of APIs.",
    "\u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:12-274\u0060 covers positive hub/link/multi-active generation and negative \u0060DMV1960\u0060/\u0060DMV1961\u0060/\u0060DMV1962\u0060/\u0060DMV1965\u0060/\u0060DMV1966\u0060 cases, but no test covers custom technical-column produced names.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/analyzers, area/developer-experience, area/ef-core, area/read-models, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 7 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite\u0027.",
    "Ticket history references implementation commit \u00278e0ea8742ab6\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 2 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Update the generator so satellite row public members stay \u0060HashDiff\u0060, \u0060LoadTimestamp\u0060, and \u0060RecordSource\u0060 regardless of produced technical column names, while still preserving exact produced bindings in constants or binding metadata.",
    "Add analyzer coverage with a support-bundle fixture that uses renamed technical produced columns and asserts the generated row still exposes the contract-fixed technical members plus the correct produced-column constants.",
    "After the fix lands, verify the branch with \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060 in the supported test environment."
  ],
  "branchName": "ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite",
  "commitSha": "8e0ea8742ab6"
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