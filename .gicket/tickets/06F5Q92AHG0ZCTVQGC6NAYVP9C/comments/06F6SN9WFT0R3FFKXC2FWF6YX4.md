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
    "Selected verification source branch \u0027ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite\u0027 and commit \u0027d85f41d6c614\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite\u0027 from source \u0027d85f41d6c614\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite\u0027.",
    "Evidence: git -C /mnt/c/Projects/DVault diff --name-only develop...d85f41d6c614 shows the implementation changes are concentrated in src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs, src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs, src/DCoding.Data.DVault.Analyzers/README.md, and tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs.",
    "Evidence: docs/plans/typed-read-model-generator-contract.md:22-38 requires one authoritative normalized metadata source and preservation of MetadataSourceKind, MetadataSourceFingerprint, ProducedName, MetadataName, PropertyRole, TechnicalColumnRole, ProviderLogicalPropertyKind, ordinal, CLR type, and nullability.",
    "Evidence: src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:43-66 builds declarations only from compilation syntax trees and additional texts; it does not resolve an authoritative EF/DVault annotated model or registry-backed descriptor.",
    "Evidence: src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:549-624 computes fingerprints and produced names locally, and :677-703 emits those synthesized values into generated constants and DataVaultSatelliteMetadata construction.",
    "Evidence: src/DCoding.Data.DVault/DataVaultAnnotationNames.cs:13-80 and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:693-699 plus :852-859 show the repository already exposes authoritative ProducedName, MetadataName, ParentReference, PropertyRole, ProviderLogicalPropertyKind, and MetadataSourceFingerprint annotations on translated EF metadata.",
    "Evidence: src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs:16-75 plus tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelFirstDesignTimeWorkflowTests.cs:17-25 and tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs:33-66 show supported UseDataVaultMetadata(...) and UseModel(...) repository paths that the generator does not consume.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:379-560 uses simplified stubs without authoritative DVault annotations, reinforcing that the current tests validate syntax parsing rather than annotated-model normalization.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/analyzers, area/developer-experience, area/ef-core, area/read-models, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite\u0027.",
    "Evidence: Ticket history references implementation commit \u0027d85f41d6c614\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: Generated methods use the existing latest-satellite read contract through \u0060IDataVaultReadService\u0060 and \u0060DataVaultLatestSatelliteReadRequest\u0060, or an equivalent stable direct EF projection explicitly allowed by the contract, without introducing provider-specific SQL or caller-owned projector delegates. (The generated helper methods do use the existing read-service contract: src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:717-760 emits Current/AsOf calls through DataVaultReadServiceCurrentSatelliteExtensions and Latest through DataVaultLatestSatelliteReadRequest plus DataVaultReadServiceTypedProjectionExtensions, without adding a new runtime read surface.).",
    "DoD check passed: Analyzer-package implementation and tests land in the existing \u0060DCoding.Data.DVault.Analyzers\u0060 and \u0060tests/DCoding.Data.DVault.Tests/Analyzers\u0060 generator harness, and generated helpers compile against the current \u0060DCoding.Data.DVault\u0060 runtime APIs without introducing a new public runtime query surface. (The implementation landed in the existing analyzer package and analyzer test harness: src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs, src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs, and tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs. The generated methods target existing runtime APIs rather than introducing a new public read interface.).",
    "DoD check passed: Generated satellite helpers behave consistently with the current/latest/as-of satellite semantics already exposed by \u0060DataVaultReadServiceCurrentSatelliteExtensions\u0060 and \u0060DataVaultSatelliteProjectionRow\u0060. (The emitted helpers align with the existing current/latest/as-of runtime semantics at the call surface: src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:737-760 delegates to DataVaultReadServiceCurrentSatelliteExtensions, DataVaultReadServiceTypedProjectionExtensions, DataVaultLatestSatelliteReadRequest, and DataVaultSatelliteProjectionRow.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: For each supported satellite metadata declaration, the consuming compilation receives generated \u0060ReadModel\u0060 and \u0060ReadExtensions\u0060 source under the documented namespace and naming rules, with \u0060Current\u0060, \u0060Latest\u0060, and \u0060AsOf\u0060 methods bound to that satellite. (Not every supported satellite metadata declaration is handled. src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:126-155 only recognizes source-visible ApplyDataVaultMetadata(...) syntax for code-first input, while docs/plans/typed-read-model-generator-contract.md:24-38 requires generation from the authoritative projected metadata source, including UseDataVaultMetadata(...) and compiled-model paths.).",
    "AC check failed: Generated satellite row types preserve exact produced table/column bindings and expose the parent hash key, driving keys in metadata order, \u0060HashDiff\u0060, \u0060LoadTimestamp\u0060, \u0060RecordSource\u0060, and payload properties with nullability derived from authoritative CLR/EF metadata. (The generated row bindings are synthesized instead of preserved from authoritative metadata. src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:549-624 computes fingerprints, produced table names, hash-key names, and payload/driving-key produced columns locally instead of reading authoritative ProducedName/MetadataSourceFingerprint/property-role metadata required by docs/plans/typed-read-model-generator-contract.md:30-38 and :151-170.).",
    "AC check failed: When authoritative metadata cannot be resolved deterministically, fingerprints drift, bindings or normalized public names collide, or the requested shape falls outside the bounded satellite contract, generation stops or skips with the documented \u0060DMV196x\u0060 diagnostics instead of emitting unstable helpers. (Deterministic authoritative-source failure handling is incomplete. docs/plans/typed-read-model-generator-contract.md:38 requires generation to fail when more than one source is visible for the same scope, but src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:52-66 and :92-123 only merges declarations and checks generated type-name collisions. DMV1960 is only reported for literal parse failures at :382-386 and :500-505.).",
    "AC check failed: Repository tests cover positive generation for representative hub-parent, link-parent, and multi-active satellite shapes plus negative diagnostics for stale fingerprints, unsupported bindings, nullability fallback, and naming-collision edge cases. (Repository tests do not cover the full required surface. tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:12-236 covers ApplyDataVaultMetadata, literal DataVaultMetadataModel, JSON additional files, stale fingerprint, non-string payloads, and type-name collisions, but it does not cover authoritative-source normalization, UseDataVaultMetadata/UseModel inputs, or exact produced-name preservation from annotated metadata.).",
    "DoD check failed: Developer-facing analyzer/generator documentation is updated enough to explain the typed satellite read-model generator boundary, supported inputs, and \u0060DMV196x\u0060 failure cases. (Documentation was updated, but src/DCoding.Data.DVault.Analyzers/README.md:54-58 documents a narrower source-visible ApplyDataVaultMetadata/literal-metadata/JSON boundary than the authoritative contract, so the supported-input boundary is still not documented correctly for this ticket.).",
    "DoD check failed: Regression coverage protects deterministic naming, metadata-source and fingerprint handling, payload nullability, multi-active driving-key ordering, and unsupported-shape diagnostics. (Regression coverage does not protect authoritative metadata-source resolution end-to-end. tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs does not exercise mixed-source ambiguity, UseDataVaultMetadata/compiled-model inputs, or authoritative produced-name/fingerprint preservation.).",
    "The generator does not resolve the authoritative translated EF/DVault metadata source required by the contract, so supported UseDataVaultMetadata(...) and compiled-model inputs are not implemented.",
    "Produced table names, produced column names, and metadata source fingerprints are recomputed inside the generator instead of being preserved from authoritative annotations, which can drift from the real runtime metadata contract.",
    "The regression suite does not cover authoritative-source ambiguity or annotated-model binding preservation, so the contract miss is currently unprotected."
  ],
  "evidence": [
    "git -C /mnt/c/Projects/DVault diff --name-only develop...d85f41d6c614 shows the implementation changes are concentrated in src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs, src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs, src/DCoding.Data.DVault.Analyzers/README.md, and tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs.",
    "docs/plans/typed-read-model-generator-contract.md:22-38 requires one authoritative normalized metadata source and preservation of MetadataSourceKind, MetadataSourceFingerprint, ProducedName, MetadataName, PropertyRole, TechnicalColumnRole, ProviderLogicalPropertyKind, ordinal, CLR type, and nullability.",
    "src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:43-66 builds declarations only from compilation syntax trees and additional texts; it does not resolve an authoritative EF/DVault annotated model or registry-backed descriptor.",
    "src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:549-624 computes fingerprints and produced names locally, and :677-703 emits those synthesized values into generated constants and DataVaultSatelliteMetadata construction.",
    "src/DCoding.Data.DVault/DataVaultAnnotationNames.cs:13-80 and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:693-699 plus :852-859 show the repository already exposes authoritative ProducedName, MetadataName, ParentReference, PropertyRole, ProviderLogicalPropertyKind, and MetadataSourceFingerprint annotations on translated EF metadata.",
    "src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs:16-75 plus tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelFirstDesignTimeWorkflowTests.cs:17-25 and tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs:33-66 show supported UseDataVaultMetadata(...) and UseModel(...) repository paths that the generator does not consume.",
    "tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:379-560 uses simplified stubs without authoritative DVault annotations, reinforcing that the current tests validate syntax parsing rather than annotated-model normalization.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/analyzers, area/developer-experience, area/ef-core, area/read-models, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite\u0027.",
    "Ticket history references implementation commit \u0027d85f41d6c614\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Rework the generator to normalize one authoritative translated EF/DVault descriptor for code-first, metadata-first, and model-first inputs, including UseDataVaultMetadata(...) and compiled-model annotated paths.",
    "Drive generated names, bindings, source kind/fingerprint, property roles, ordinals, provider logical metadata, and payload nullability from authoritative DVault annotations instead of ComputeFingerprint/GetSatelliteTableName/GetColumnNames heuristics.",
    "Add regression tests for authoritative-source ambiguity, UseDataVaultMetadata/UseModel inputs, and exact produced-name/fingerprint preservation, then rerun deterministic verification with dotnet test DVault.slnx --nologo and bash tools/check-format.sh."
  ],
  "branchName": "ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite",
  "commitSha": "d85f41d6c614"
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