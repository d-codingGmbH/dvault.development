[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F8KZPN02NWFGMRC2Q1PKYKDR\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc\u0027 and commit \u0027ab2d0a0649af\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc\u0027 from source \u0027ab2d0a0649af\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc\u0027.",
    "Evidence: Branch diff \u0060develop...ab2d0a0649af\u0060 touches \u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0060, \u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0060, \u0060src/DCoding.Data.DVault.Analyzers/README.md\u0060, \u0060docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0060; it does not touch \u0060docs/plans/typed-read-model-generator-contract.md\u0060.",
    "Evidence: \u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:42-67\u0060 adds \u0060metadataSourceResolutionFailed\u0060 and returns before generation when a raw model or incompatible support-bundle source is detected.",
    "Evidence: \u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:194-214\u0060 reports \u0060DMV1960\u0060 for unsupported \u0060dvault.support-bundle.*\u0060 schema versions and raw \u0060dvault.model.*\u0060 additional files.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:495-583\u0060 adds analyzer coverage for raw model plus valid bundle, incompatible support-bundle version, and ambiguous authoritative bundles; each test asserts \u0060DMV1960\u0060 and no generated sources.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:683-719\u0060 and \u0060:745-778\u0060 prove unsupported PIT or bridge helpers emit \u0060DMV1963\u0060 or \u0060DMV1964\u0060 while \u0060DVault.GeneratedReadModels.SatCustomerProfile.g.cs\u0060 still generates.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:211-246\u0060 and \u0060:903-944\u0060 still generate bridge and PIT helpers from model-first support bundles with request-bound read-shape data and embedded source fingerprint metadata.",
    "Evidence: \u0060src/DCoding.Data.DVault.Analyzers/README.md:83-91\u0060, \u0060docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md:18,134-140\u0060, and \u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs:80-87\u0060 now align on \u0060DMV1960\u0060 for raw or residual model-first source-boundary failures and \u0060DMV1968\u0060 as reserved.",
    "Evidence: \u0060docs/plans/typed-read-model-generator-contract.md:111-119\u0060 still says \u0060DMV1960\u0060 is only missing, invalid, non-authoritative, or ambiguous support-bundle input and \u0060DMV1968\u0060 is the model-first outcome, which conflicts with the shipped mapping.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/analyzers, area/developer-experience, area/diagnostics, area/ef-core, area/read-models, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc\u0027.",
    "Evidence: Ticket history references implementation commit \u0027ab2d0a0649af\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: With \u0060DVaultGenerateTypedReadModels=true\u0060, resolving anything other than exactly one authoritative \u0060dvault.support-bundle.v1\u0060 additional file results in \u0060DMV1960\u0060 and no generated helpers. (\u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:42-84,194-214\u0060 now treats incompatible support-bundle versions and raw \u0060dvault.model.v1\u0060 inputs as \u0060DMV1960\u0060 resolution failures, and \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:464-583,613-619\u0060 cover raw-model, mixed-input, incompatible-version, ambiguous-source, and no-source suppression with no generated helpers.).",
    "AC check passed: When \u0060DVaultTypedReadModelMetadataSourceFingerprint\u0060 is configured and does not match the resolved bundle fingerprint, the generator reports \u0060DMV1961\u0060 and suppresses generation. (The fingerprint gate in \u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:96-113\u0060 still suppresses only on mismatch, and \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:585-610\u0060 asserts \u0060DMV1961\u0060 with generation suppressed.).",
    "AC check passed: When PIT explain metadata or request-bound \u0060diagnostics.readShape.pit\u0060 facts are missing, mismatched, or outside the bounded PIT helper contract, the generator reports \u0060DMV1963\u0060 for the affected PIT helper while leaving unrelated supported helpers eligible. (PIT validation remains entity-specific, and \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:683-719\u0060 shows \u0060DMV1963\u0060 for the unsupported PIT helper while \u0060DVault.GeneratedReadModels.SatCustomerProfile.g.cs\u0060 still generates.).",
    "AC check passed: When bridge explain metadata or request-bound \u0060diagnostics.readShape.bridge\u0060 facts are missing, mismatched, or outside the bounded bridge helper contract, the generator reports \u0060DMV1964\u0060 or \u0060DMV1967\u0060 as appropriate for the affected bridge helper while leaving unrelated supported helpers eligible. (Bridge validation remains entity-specific; \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:745-778\u0060 shows \u0060DMV1964\u0060 only for the affected bridge helper while the satellite helper still generates, and \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:821-853\u0060 still covers the \u0060DMV1967\u0060 bridge path for unbounded hierarchy shapes.).",
    "AC check passed: A projected model-first support bundle with matching fingerprint and required ReadShape facts continues to generate supported PIT and bridge helpers. (By inspection, \u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:100-113\u0060 only suppresses on fingerprint mismatch, and existing model-first support-bundle tests at \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:211-246\u0060 and \u0060:903-944\u0060 still generate supported bridge and PIT helpers when request-bound \u0060ReadShape\u0060 evidence is present.).",
    "AC check passed: Raw or residual \u0060dvault.model.v1\u0060 artifacts presented outside the projected support-bundle contract report \u0060DMV1960\u0060 under the current source-boundary baseline and do not widen generator inputs. (\u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:205-214\u0060 now rejects raw or residual \u0060dvault.model.v1\u0060 inputs at the source boundary with \u0060DMV1960\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:464-523\u0060 verify rejection both alone and alongside a valid support bundle without widening generator inputs.).",
    "DoD check passed: Generator code paths and analyzer tests cover the \u0060DMV1960\u0060, \u0060DMV1961\u0060, \u0060DMV1963\u0060, \u0060DMV1964\u0060, and \u0060DMV1967\u0060 paths touched by this story, plus the accepted raw-model rejection behavior. (The changed generator paths and analyzer tests cover \u0060DMV1960\u0060, \u0060DMV1961\u0060, \u0060DMV1963\u0060, \u0060DMV1964\u0060, and \u0060DMV1967\u0060, including raw-model rejection (\u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:464-583,585-610,657-853\u0060).).",
    "DoD check passed: Supported satellite, PIT, and bridge helpers continue generating for unaffected entities in mixed bundles. (Mixed-bundle analyzer coverage at \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:683-719\u0060 and \u0060:745-778\u0060 shows unrelated satellite helper generation continues when PIT or bridge helpers are rejected.).",
    "DoD check passed: No direct raw-model parsing path or unreviewed metadata-source fallback is introduced. (The diff adds source-boundary rejection rather than parsing: \u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:194-214\u0060 reports \u0060DMV1960\u0060 for raw model or incompatible bundle schema versions, and the updated contract text at \u0060docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md:12-18\u0060 and \u0060src/DCoding.Data.DVault.Analyzers/README.md:67,83-91\u0060 continues to say raw model files are not parsed directly and no fallback source is used.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "DoD check failed: README and any in-repo generator contract text that mention these scenarios match the shipped diagnostic mapping. (\u0060src/DCoding.Data.DVault.Analyzers/README.md:83-91\u0060, \u0060docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md:18,134-140\u0060, and \u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs:80-87\u0060 were updated, but \u0060docs/plans/typed-read-model-generator-contract.md:107-120\u0060 still documents raw or residual model-first cases under \u0060DMV1968\u0060 instead of the shipped \u0060DMV1960\u0060 mapping.).",
    "Blocking: \u0060docs/plans/typed-read-model-generator-contract.md:111-119\u0060 still documents raw or residual model-first source-boundary failures as \u0060DMV1968\u0060, while the updated generator, catalog, README, and architecture contract now ship those failures as \u0060DMV1960\u0060.",
    "Related drift: \u0060docs/releases/v0.25.0.md:102-110\u0060 still repeats the old \u0060DMV1968\u0060 mapping for raw or residual model-first evidence."
  ],
  "evidence": [
    "Branch diff \u0060develop...ab2d0a0649af\u0060 touches \u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0060, \u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0060, \u0060src/DCoding.Data.DVault.Analyzers/README.md\u0060, \u0060docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0060; it does not touch \u0060docs/plans/typed-read-model-generator-contract.md\u0060.",
    "\u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:42-67\u0060 adds \u0060metadataSourceResolutionFailed\u0060 and returns before generation when a raw model or incompatible support-bundle source is detected.",
    "\u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:194-214\u0060 reports \u0060DMV1960\u0060 for unsupported \u0060dvault.support-bundle.*\u0060 schema versions and raw \u0060dvault.model.*\u0060 additional files.",
    "\u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:495-583\u0060 adds analyzer coverage for raw model plus valid bundle, incompatible support-bundle version, and ambiguous authoritative bundles; each test asserts \u0060DMV1960\u0060 and no generated sources.",
    "\u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:683-719\u0060 and \u0060:745-778\u0060 prove unsupported PIT or bridge helpers emit \u0060DMV1963\u0060 or \u0060DMV1964\u0060 while \u0060DVault.GeneratedReadModels.SatCustomerProfile.g.cs\u0060 still generates.",
    "\u0060tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:211-246\u0060 and \u0060:903-944\u0060 still generate bridge and PIT helpers from model-first support bundles with request-bound read-shape data and embedded source fingerprint metadata.",
    "\u0060src/DCoding.Data.DVault.Analyzers/README.md:83-91\u0060, \u0060docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md:18,134-140\u0060, and \u0060src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs:80-87\u0060 now align on \u0060DMV1960\u0060 for raw or residual model-first source-boundary failures and \u0060DMV1968\u0060 as reserved.",
    "\u0060docs/plans/typed-read-model-generator-contract.md:111-119\u0060 still says \u0060DMV1960\u0060 is only missing, invalid, non-authoritative, or ambiguous support-bundle input and \u0060DMV1968\u0060 is the model-first outcome, which conflicts with the shipped mapping.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/analyzers, area/developer-experience, area/diagnostics, area/ef-core, area/read-models, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc\u0027.",
    "Ticket history references implementation commit \u0027ab2d0a0649af\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Update \u0060docs/plans/typed-read-model-generator-contract.md\u0060 so its diagnostics table matches the shipped mapping: \u0060DMV1960\u0060 covers incompatible-version and raw-residual source-boundary failures, and \u0060DMV1968\u0060 remains reserved.",
    "Decide whether the stale historical note in \u0060docs/releases/v0.25.0.md\u0060 should be corrected or explicitly called out as historical to avoid repeating the old \u0060DMV1968\u0060 mapping.",
    "After the documentation mismatch is fixed, rerun the tester gate."
  ],
  "branchName": "ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc",
  "commitSha": "ab2d0a0649af"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F8KZPN02NWFGMRC2Q1PKYKDR`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc`