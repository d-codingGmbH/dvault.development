[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706FH8RKDJTS3BB11J6J6QJVVD4\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06FH8RKDJTS3BB11J6J6QJVVD4-task-add-privacy-configuration-api-for-custom-or\u0027 and commit \u0027388f7f925889\u0027 (verification-source contract).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06FH8RKDJTS3BB11J6J6QJVVD4-task-add-privacy-configuration-api-for-custom-or\u0027 from source \u0027388f7f925889\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06FH8RKDJTS3BB11J6J6QJVVD4-task-add-privacy-configuration-api-for-custom-or\u0027.",
    "Evidence: git diff --name-status develop...388f7f925889 changed only shared packages and unit-test snapshots for this feature: src/DCoding.Data.DVault*, src/DCoding.Data.DVault.Privacy*, and tests/DCoding.Data.DVault.Tests/Unit*; no provider-package extension files were changed.",
    "Evidence: src/DCoding.Data.DVault.Privacy/DataVaultPrivacyOptions.cs:65-80 at 388f7f925889 adds the public RegisterProviderNativeCryptoSelection(...) entrypoint, and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.Privacy.approved.txt:84-106 snapshots that public API plus the new public DataVaultProviderNativeCryptoSelection type.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/DataVaultPrivacyServiceCollectionExtensionsTests.cs:57-99 registers a SQL Server native selection directly through AddDVaultPrivacy(...) and asserts configuration.KeyProvider is null, showing the shared package owns the consumer-facing registration surface.",
    "Evidence: src/DCoding.Data.DVault.Privacy/DataVaultProviderNativeCryptoSelectionProvider.cs:45-53 only rejects missing prerequisites when the selection list is empty, but src/DCoding.Data.DVault.Privacy/DataVaultPrivacyOptions.cs:129-140 already rejects empty prerequisite lists at registration time, leaving no implemented path that validates prerequisite satisfaction.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:798-823 reports result.Validation.IsValid == true and SelectionStatus == provider-native-requested for a native selection registered without any caller-owned key-provider wiring.",
    "Evidence: src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs:1035-1072 still sources provider-native diagnostics from DataVaultProviderCryptoCapabilityCatalog, and src/DCoding.Data.DVault/DataVaultProviderCryptoCapabilityCatalog.cs:11-117 still provides the reviewed static matrix for SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 without live probing.",
    "Evidence: docs/architecture/dvault-v1-optional-privacy-extension-boundary.md:49-53 and :105 require provider-specific privacy behavior to sit behind provider package seams and keep provider-native lanes out of shared runtime ownership.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/ef-core, area/privacy, area/security, area/tests, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06FH8RKDJTS3BB11J6J6QJVVD4-task-add-privacy-configuration-api-for-custom-or\u0027.",
    "Evidence: Ticket history references implementation commit \u0027388f7f925889\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: The refinement contract ratifies the existing AddDVaultPrivacy(...) plus UseCallerOwnedKeyProvider(...) path as the bounded v1 default when no provider-native option is explicitly selected. (The existing AddDVaultPrivacy(...) plus UseCallerOwnedKeyProvider(...) path is still present; tests at tests/DCoding.Data.DVault.Tests/Unit/DataVaultPrivacyServiceCollectionExtensionsTests.cs:10-28 keep ProviderNativeCryptoSelections empty and register the caller-owned key provider.).",
    "AC check passed: When a caller explicitly requests a native capability that is unsupported or unavailable for the active provider/profile/shape, the flow fails closed with redacted diagnostics and never silently persists plaintext or silently downgrades to implicit behavior. (Unsupported reviewed capabilities fail closed with a diagnostics error: src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs:1954-1966 converts rejected selection facts into errors, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:827-850 asserts provider-native-crypto-selection-unavailable for an unsupported SQLite capability.).",
    "AC check passed: The selection contract remains alias-driven and EF Core compatible by building on encryptedPayloadAlias, IDataVaultEncryptedPayloadKeyProvider, and ordinary mapped-property/value-converter constraints rather than new provider-specific metadata fields in the shared model. (The new selection contract remains alias-driven and keeps shared model metadata unchanged; DataVaultProviderNativeCryptoSelection carries EncryptedPayloadAlias and DataVaultPrivacyOptions auto-registers the alias without adding provider-specific fields to the shared EF model.).",
    "AC check passed: The contract consumes the existing static capability-fact lane for SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 and does not require live capability probing by default. (Diagnostics still consume DataVaultProviderCryptoCapabilityCatalog static facts for SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 without live probing (src/DCoding.Data.DVault/DataVaultProviderCryptoCapabilityCatalog.cs:11-117; src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs:1035-1072).).",
    "AC check passed: Provider-native execution proof and fallback tests remain downstream implementation work in ticket 06FH8RMFZSVNW0KKTZT9HMGM8G rather than being absorbed into this configuration-contract ticket. (The diff adds configuration/diagnostics code and unit tests only; it does not add provider-native runtime execution or absorb downstream execution-proof work into this ticket.).",
    "DoD check passed: The refined contract preserves the current non-goals: no shared managed native-encryption runtime, no provider-name branching, no live probing by default, and no DVault-owned key lifecycle or compliance workflow. (The changes keep the non-goals intact: there is no shared managed native-encryption runtime, no provider-name auto-selection, no live capability probing by default, and no DVault-owned key lifecycle or compliance workflow.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Any provider-native selection is explicit, opt-in, and owned by the matching provider package for one exact reviewed capability; the shared privacy package must not auto-select native behavior from provider identity alone. (The shared privacy package now exposes public DataVaultPrivacyOptions.RegisterProviderNativeCryptoSelection(...) and a public DataVaultProviderNativeCryptoSelection record (src/DCoding.Data.DVault.Privacy/DataVaultPrivacyOptions.cs:60-80; tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.Privacy.approved.txt:84-106), while git diff --name-status develop...388f7f925889 shows no provider-package extension changes. That makes native selection shared-package-owned instead of owned by a matching provider package seam.).",
    "DoD check failed: The ticket-level contract clearly distinguishes the shipped caller-owned custom path from any future provider-specific native opt-in path and aligns with the checked-in privacy boundary documents and done predecessor tickets. (The code distinguishes caller-owned and native-selection lanes, but it does not align with the checked-in privacy boundary because docs/architecture/dvault-v1-optional-privacy-extension-boundary.md:49-53 and :105 place provider-specific privacy behavior behind provider-package seams, while the committed API exposes native selection directly from DCoding.Data.DVault.Privacy.).",
    "DoD check failed: The refined contract makes the API placement decision explicit: provider-specific native selection belongs in matching provider-package extension methods or provider-owned seams, not in implicit shared dispatch. (API placement is not explicit in the required direction. Consumer code can call AddDVaultPrivacy(options =\u003E options.RegisterProviderNativeCryptoSelection(...)) directly, and DataVaultPrivacyOptions.Apply(...) also replaces any existing IDataVaultProviderNativeCryptoSelectionProvider registration (src/DCoding.Data.DVault.Privacy/DataVaultPrivacyOptions.cs:99-120), which undercuts provider-owned composition.).",
    "DoD check failed: A developer can implement the next proof slice without reopening PO decisions about ownership boundary, fail-closed behavior, diagnostics input, or EF Core compatibility. (The next proof slice still needs PO-level decisions reopened. The code introduces CallerOwnedPrerequisites strings, but the only missing-prerequisite rejection checks for an empty list (src/DCoding.Data.DVault.Privacy/DataVaultProviderNativeCryptoSelectionProvider.cs:45-53), which DataVaultPrivacyOptions already forbids at registration time (src/DCoding.Data.DVault.Privacy/DataVaultPrivacyOptions.cs:129-140). tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:798-823 then accept a provider-native selection as valid with no caller-owned key provider or other prerequisite proof wiring, so the fail-closed prerequisite contract is not actually settled.).",
    "The consumer-facing provider-native selection API landed in the shared privacy package instead of a matching provider package seam. The committed public surface is DataVaultPrivacyOptions.RegisterProviderNativeCryptoSelection(...) plus a public DataVaultProviderNativeCryptoSelection record, and the diff for 388f7f925889 does not add any provider-package extension entrypoint.",
    "The fail-closed prerequisite branch is effectively unimplemented. Registration forbids empty prerequisite lists, the analyzer only rejects empty lists, and the new diagnostics test accepts a provider-native selection as valid even though no caller-owned prerequisite proof mechanism is configured."
  ],
  "evidence": [
    "git diff --name-status develop...388f7f925889 changed only shared packages and unit-test snapshots for this feature: src/DCoding.Data.DVault*, src/DCoding.Data.DVault.Privacy*, and tests/DCoding.Data.DVault.Tests/Unit*; no provider-package extension files were changed.",
    "src/DCoding.Data.DVault.Privacy/DataVaultPrivacyOptions.cs:65-80 at 388f7f925889 adds the public RegisterProviderNativeCryptoSelection(...) entrypoint, and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.Privacy.approved.txt:84-106 snapshots that public API plus the new public DataVaultProviderNativeCryptoSelection type.",
    "tests/DCoding.Data.DVault.Tests/Unit/DataVaultPrivacyServiceCollectionExtensionsTests.cs:57-99 registers a SQL Server native selection directly through AddDVaultPrivacy(...) and asserts configuration.KeyProvider is null, showing the shared package owns the consumer-facing registration surface.",
    "src/DCoding.Data.DVault.Privacy/DataVaultProviderNativeCryptoSelectionProvider.cs:45-53 only rejects missing prerequisites when the selection list is empty, but src/DCoding.Data.DVault.Privacy/DataVaultPrivacyOptions.cs:129-140 already rejects empty prerequisite lists at registration time, leaving no implemented path that validates prerequisite satisfaction.",
    "tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:798-823 reports result.Validation.IsValid == true and SelectionStatus == provider-native-requested for a native selection registered without any caller-owned key-provider wiring.",
    "src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs:1035-1072 still sources provider-native diagnostics from DataVaultProviderCryptoCapabilityCatalog, and src/DCoding.Data.DVault/DataVaultProviderCryptoCapabilityCatalog.cs:11-117 still provides the reviewed static matrix for SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 without live probing.",
    "docs/architecture/dvault-v1-optional-privacy-extension-boundary.md:49-53 and :105 require provider-specific privacy behavior to sit behind provider package seams and keep provider-native lanes out of shared runtime ownership.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/ef-core, area/privacy, area/security, area/tests, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06FH8RKDJTS3BB11J6J6QJVVD4-task-add-privacy-configuration-api-for-custom-or\u0027.",
    "Ticket history references implementation commit \u0027388f7f925889\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Move the consumer-facing native-selection registration surface out of DataVaultPrivacyOptions and into provider-specific extension methods or provider-owned seams, keeping the shared package limited to provider-neutral abstractions and diagnostics facts if they are still needed.",
    "Implement real prerequisite-proof evaluation for native selections, with failing tests for missing prerequisite satisfaction, or revise the design so the contract no longer claims fail-closed prerequisite validation that the shared code cannot perform.",
    "After the rework, rerun dotnet test DVault.slnx --nologo and bash tools/check-format.sh in a supported environment before handing the branch back to test."
  ],
  "branchName": "ticket/06FH8RKDJTS3BB11J6J6QJVVD4-task-add-privacy-configuration-api-for-custom-or",
  "commitSha": "388f7f925889"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06FH8RKDJTS3BB11J6J6QJVVD4`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06FH8RKDJTS3BB11J6J6QJVVD4-task-add-privacy-configuration-api-for-custom-or`