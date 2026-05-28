[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F5Q93H60W6X8FJ88PWTR6NG4\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F5Q93H60W6X8FJ88PWTR6NG4-task-update-v0-22-0-typed-read-and-hash-governan\u0027 and commit \u002705b8e276fe19\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F5Q93H60W6X8FJ88PWTR6NG4-task-update-v0-22-0-typed-read-and-hash-governan\u0027 from source \u002705b8e276fe19\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F5Q93H60W6X8FJ88PWTR6NG4-task-update-v0-22-0-typed-read-and-hash-governan\u0027.",
    "Evidence: git diff --name-only develop...05b8e276fe19 shows the repository-side delivery is confined to README.md, docs/model-first-governance.md, docs/production-adoption-checklist.md, docs/releases/v0.22.0.md, and src/DCoding.Data.DVault.Analyzers/README.md outside .gicket metadata.",
    "Evidence: git ls-files confirms all required evidence paths exist, including docs/quality/api-surface-snapshots.md, tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs, docs/plans/stable-hashing-contract.md, tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs, docs/architecture/dvault-ef-compiled-compatibility.md, and the six approved files under tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/.",
    "Evidence: README.md adds a typed satellite helper section with DVaultGenerateTypedReadModels, DVaultTypedReadModelMetadataSourceFingerprint, exactly one authoritative support-bundle workflow, dynamic IDataVaultReadService alternatives, compiled EF query guidance, and README local validation commands.",
    "Evidence: src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs defines build_property.DVaultGenerateTypedReadModels, build_property.DVaultTypedReadModelMetadataSourceFingerprint, and dvault.support-bundle.v1; tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs asserts Read...CurrentAsync/LatestAsync/AsOfAsync generation and DMV1960, DMV1961, DMV1963, DMV1964, DMV1966, DMV1967, DMV1968, and DMV1969 coverage.",
    "Evidence: docs/model-first-governance.md documents reviewed dvault.model.v1 import, projection, and consumer-owned support-bundle export, and states raw dvault.model.v1 files are not direct generator inputs.",
    "Evidence: docs/releases/v0.22.0.md links DataVaultTypedReadModelSourceGeneratorTests.cs, StableHashServiceTests.cs, docs/quality/api-surface-snapshots.md, ApiSurfaceSnapshotTests.cs, the six PublicApi approved files, docs/architecture/dvault-ef-compiled-compatibility.md, and the README local validation section.",
    "Evidence: README.md:10-16 and src/DCoding.Data.DVault.Analyzers/README.md:20 hardcode 0.22.0 NuGet installation snippets, while docs/releases/v0.22.0.md:6,20 says publication is separate/manual and docs/production-adoption-checklist.md:106-108 says publication evidence belongs to the manual publication checklist and unpublished future versions must not be implied.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/analyzers, area/documentation, area/ef-core, area/modeling, area/read-models, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06F5Q93H60W6X8FJ88PWTR6NG4-task-update-v0-22-0-typed-read-and-hash-governan\u0027.",
    "Evidence: Ticket history references implementation commit \u002705b8e276fe19\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: README documents opt-in typed read-model generation with DVaultGenerateTypedReadModels=true, the consumer-owned support-bundle command and artifact workflow, and the existing alternatives of dynamic IDataVaultReadService requests and consumer-owned compiled EF queries. (README.md adds explicit typed satellite helper guidance with DVaultGenerateTypedReadModels, reviewed support-bundle workflow, dynamic IDataVaultReadService alternatives, and compiled EF query guidance.).",
    "AC check passed: src/DCoding.Data.DVault.Analyzers/README.md states the visible generator boundary exactly: one authoritative dvault.support-bundle.v1 additional file, satellite-only current, latest, and as-of helpers, fingerprint drift handling, and DMV1960 through DMV1969 outcomes for missing, stale, unsupported, collision, nullability-fallback, and skipped-helper cases. (src/DCoding.Data.DVault.Analyzers/README.md documents one authoritative dvault.support-bundle.v1 input, satellite-only helper emission, fingerprint drift handling, and DMV1960-DMV1969 outcomes.).",
    "AC check passed: docs/model-first-governance.md and docs/production-adoption-checklist.md route readers through the reviewed dvault.model.v1 artifact, projected EF and DVault metadata, and consumer-invoked dvault.support-bundle.v1 export flow without implying repo-checked baseline files or a standalone CLI. (docs/model-first-governance.md and docs/production-adoption-checklist.md route reviewed dvault.model.v1 artifacts through projection and consumer-owned support-bundle export without treating repo-root artifacts or a standalone CLI as baseline behavior.).",
    "AC check passed: docs/releases/v0.22.0.md is created and links only existing evidence surfaces: docs/quality/api-surface-snapshots.md, tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs, tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/*.approved.txt, tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs, docs/plans/stable-hashing-contract.md, tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs, docs/architecture/dvault-ef-compiled-compatibility.md, and README local validation commands. (docs/releases/v0.22.0.md is present and links branch-visible API snapshot, generator-test, stable-hash, compiled-compatibility, and README validation evidence surfaces.).",
    "AC check passed: Across the targeted docs, the wording consistently states that the current generator does not parse raw dvault.model.v1 additional files directly, does not emit PIT or bridge helpers, does not generate provider-specific SQL or dynamic-request compilation, and does not rely on non-existent analyzer or generator approval snapshots. (Across the targeted docs, the generator is consistently described as support-bundle-driven, satellite-only, non-PIT/non-bridge, non-provider-SQL, non-dynamic-request-compiling, and not dependent on nonexistent approval snapshots.).",
    "DoD check passed: The targeted docs and new docs/releases/v0.22.0.md resolve to existing repository paths, commands, and evidence surfaces that are visible in the current branch. (The targeted docs and referenced evidence and command surfaces resolve to paths and commands visible in this branch.).",
    "DoD check passed: Public API references are limited to the committed core and provider snapshot surface that exists today. (Public API references stay limited to docs/quality/api-surface-snapshots.md, ApiSurfaceSnapshotTests.cs, and the six committed approved files under tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/.).",
    "DoD check passed: Generator evidence references are limited to the source-generator implementation and test surface that exists today. (Generator evidence references stay on the existing source-generator implementation and DataVaultTypedReadModelSourceGeneratorTests.cs surfaces.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "DoD check failed: The docs tell one consistent v0.22.0 story for support-bundle-driven typed satellite helpers and sha256-v1 compatibility without reintroducing unsourced API or snapshot claims. (The v0.22.0 story is not fully consistent: README.md and src/DCoding.Data.DVault.Analyzers/README.md hardcode NuGet install snippets for 0.22.0 while docs/releases/v0.22.0.md and docs/production-adoption-checklist.md say publication is separate/manual and unpublished future versions should not be implied.).",
    "DoD check failed: The docs keep package publication, support-bundle transport, and approval-snapshot generation explicitly manual and consumer-owned. (Package publication is not kept consistently manual and published-only across the targeted docs because README.md and src/DCoding.Data.DVault.Analyzers/README.md present 0.22.0 NuGet installation commands before publication evidence exists in-branch.).",
    "README.md and src/DCoding.Data.DVault.Analyzers/README.md currently imply immediate NuGet availability of 0.22.0 by hardcoding install snippets, which conflicts with the same branch\u0027s manual-publication posture in docs/releases/v0.22.0.md and docs/production-adoption-checklist.md."
  ],
  "evidence": [
    "git diff --name-only develop...05b8e276fe19 shows the repository-side delivery is confined to README.md, docs/model-first-governance.md, docs/production-adoption-checklist.md, docs/releases/v0.22.0.md, and src/DCoding.Data.DVault.Analyzers/README.md outside .gicket metadata.",
    "git ls-files confirms all required evidence paths exist, including docs/quality/api-surface-snapshots.md, tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs, docs/plans/stable-hashing-contract.md, tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs, docs/architecture/dvault-ef-compiled-compatibility.md, and the six approved files under tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/.",
    "README.md adds a typed satellite helper section with DVaultGenerateTypedReadModels, DVaultTypedReadModelMetadataSourceFingerprint, exactly one authoritative support-bundle workflow, dynamic IDataVaultReadService alternatives, compiled EF query guidance, and README local validation commands.",
    "src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs defines build_property.DVaultGenerateTypedReadModels, build_property.DVaultTypedReadModelMetadataSourceFingerprint, and dvault.support-bundle.v1; tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs asserts Read...CurrentAsync/LatestAsync/AsOfAsync generation and DMV1960, DMV1961, DMV1963, DMV1964, DMV1966, DMV1967, DMV1968, and DMV1969 coverage.",
    "docs/model-first-governance.md documents reviewed dvault.model.v1 import, projection, and consumer-owned support-bundle export, and states raw dvault.model.v1 files are not direct generator inputs.",
    "docs/releases/v0.22.0.md links DataVaultTypedReadModelSourceGeneratorTests.cs, StableHashServiceTests.cs, docs/quality/api-surface-snapshots.md, ApiSurfaceSnapshotTests.cs, the six PublicApi approved files, docs/architecture/dvault-ef-compiled-compatibility.md, and the README local validation section.",
    "README.md:10-16 and src/DCoding.Data.DVault.Analyzers/README.md:20 hardcode 0.22.0 NuGet installation snippets, while docs/releases/v0.22.0.md:6,20 says publication is separate/manual and docs/production-adoption-checklist.md:106-108 says publication evidence belongs to the manual publication checklist and unpublished future versions must not be implied.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/analyzers, area/documentation, area/ef-core, area/modeling, area/read-models, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F5Q93H60W6X8FJ88PWTR6NG4-task-update-v0-22-0-typed-read-and-hash-governan\u0027.",
    "Ticket history references implementation commit \u002705b8e276fe19\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Remove or explicitly qualify the 0.22.0 NuGet installation snippets in README.md and src/DCoding.Data.DVault.Analyzers/README.md so the docs stop implying published availability before the manual publication step.",
    "After the publication-posture wording is corrected, re-run tester review; if command evidence is still required from the read-only tester path, obtain deterministic verification for dotnet test DVault.slnx --nologo and bash tools/check-format.sh."
  ],
  "branchName": "ticket/06F5Q93H60W6X8FJ88PWTR6NG4-task-update-v0-22-0-typed-read-and-hash-governan",
  "commitSha": "05b8e276fe19"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F5Q93H60W6X8FJ88PWTR6NG4`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F5Q93H60W6X8FJ88PWTR6NG4-task-update-v0-22-0-typed-read-and-hash-governan`