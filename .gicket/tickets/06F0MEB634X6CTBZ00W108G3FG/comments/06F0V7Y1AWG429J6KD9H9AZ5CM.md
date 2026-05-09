[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F0MEB634X6CTBZ00W108G3FG\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F0MEB634X6CTBZ00W108G3FG-task-register-metadata-model-through-adddvault-a\u0027 and commit \u0027359afe6f0b42\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F0MEB634X6CTBZ00W108G3FG-task-register-metadata-model-through-adddvault-a\u0027 from source \u0027359afe6f0b42\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F0MEB634X6CTBZ00W108G3FG-task-register-metadata-model-through-adddvault-a\u0027.",
    "Evidence: \u0060git diff --name-only develop...359afe6f0b42\u0060 shows the implementation is concentrated in \u0060README.md\u0060, \u0060src/DCoding.Data.DVault/*\u0060, and \u0060tests/DCoding.Data.DVault.Tests/*\u0060 for the ticketed feature work.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultOptions.cs:61-82\u0060 adds app-level metadata registration through \u0060UseMetadataModel(...)\u0060 and \u0060UseMetadataRegistry(...)\u0060.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs:16-60\u0060 and \u0060src/DCoding.Data.DVault/DataVaultDbContextOptionsExtension.cs:70-179\u0060 add the DbContext-scoped opt-in, registry resolution, and model-cache-key wiring.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:122-179\u0060 records metadata-source annotations and routes registry-backed projection through the existing EF metadata translator.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs:13-86\u0060 covers app-default projection, explicit context override, model-cache-key separation, and source-conflict diagnostics.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataRegistrationTests.cs:39-72\u0060 is the added parity test, and its helper only serializes entity names plus property names.",
    "Evidence: \u0060git diff --check develop...359afe6f0b42 -- src/DCoding.Data.DVault tests/DCoding.Data.DVault.Tests README.md\u0060 returned no whitespace or patch-format errors.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/configuration, area/developer-experience, area/ef-core, area/modeling, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06F0MEBFTW8FY5T7PY5HJ5JXJ4-task-refactor-save-and-read-services-to-consume\u0027.",
    "Evidence: Ticket history references implementation commit \u0027359afe6f0b42\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: A caller can register DVault metadata once during service setup through AddDVault(...) by supplying either a DataVaultMetadataModel or a prebuilt DataVaultMetadataRegistry, and the resulting default registry is immutable and deterministic. (\u0060src/DCoding.Data.DVault/DataVaultOptions.cs:61-82\u0060 adds \u0060UseMetadataModel\u0060 and \u0060UseMetadataRegistry\u0060; the model path builds a \u0060DataVaultMetadataRegistry\u0060 once during registration and the registry path stores a singleton default source. \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataRegistrationTests.cs:12-36\u0060 covers both registration paths.).",
    "AC check passed: An opted-in DbContext can project the registered metadata through ordinary model configuration without recreating the same metadata declarations in OnModelCreating; a context that uses only the existing UseDataVault() baseline without the new opt-in surface continues to create no DVault tables. (\u0060src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs:16-60\u0060 adds the DbContext opt-in surface, and \u0060src/DCoding.Data.DVault/DataVaultDbContextOptionsExtension.cs:70-179\u0060 resolves and projects the selected registry without user-authored service location in \u0060OnModelCreating\u0060. The preserved baseline remains covered by \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelBuilderExtensionsTests.cs:82-89\u0060 and \u0060tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs:183-200\u0060.).",
    "AC check passed: Registry-backed projection uses the existing provider-aware metadata translation baseline for the same metadata source, so the produced entities, columns, keys, indexes, and DVault annotations match the current explicit metadata path. (\u0060src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:156-179\u0060 unwraps registry-backed projection to \u0060DataVaultEfMetadataTranslator.Apply(...)\u0060 after selecting provider capabilities, so the implementation reuses the existing translation baseline rather than forking it.).",
    "AC check passed: Source selection is deterministic: an explicit context-scoped source overrides the app-level default for that context, but a single EF model that receives two distinct metadata sources fails fast with an actionable validation error that identifies the conflicting source kinds. (\u0060src/DCoding.Data.DVault/DataVaultDbContextOptionsExtension.cs:119-156\u0060 resolves an explicit context registry before falling back to the app default, and \u0060src/DCoding.Data.DVault/DataVaultMetadataSourceAnnotations.cs:24-44\u0060 fails fast when a second distinct fingerprint is recorded for the same EF model. \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs:52-86\u0060 exercises the override and conflict paths.).",
    "AC check passed: When a caller explicitly applies metadata through the existing model-level path and a different registry-backed source is also configured for the same model, DVault throws before silent divergence or duplicate projection occurs. (\u0060src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:122-133\u0060 now records model-level metadata fingerprints before translation, and the shared conflict guard in \u0060DataVaultMetadataSourceAnnotations.TryRecordSource(...)\u0060 throws before a different registry-backed source can silently diverge or duplicate projection. \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs:73-86\u0060 covers the explicit-model conflict case.).",
    "AC check passed: Automated tests cover app-level model registration, prebuilt registry registration, context opt-in consumption, preserved UseDataVault() no-table baseline, and conflict diagnostics. (Automated coverage exists for app-level model registration and prebuilt registry registration in \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataRegistrationTests.cs:12-36\u0060, for DbContext opt-in and conflict diagnostics in \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs:13-86\u0060, and for the preserved no-opt-in baseline in the existing unit and SQLite integration tests.).",
    "DoD check passed: Public API and snapshot coverage reflect the additive startup and DbContext integration surface while keeping the current optionless AddDVault() and explicit ApplyDataVaultMetadata(...) entry points source-compatible. (\u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:11-115\u0060 now includes the new \u0060DataVaultOptions\u0060 members, the new \u0060DataVaultDbContextOptionsBuilderExtensions\u0060, and the new registry overload on \u0060ApplyDataVaultMetadata(...)\u0060, while the existing optionless \u0060AddDVault()\u0060 and explicit metadata entry points remain present.).",
    "DoD check passed: The implementation stores one authoritative registry selection per EF model and validates source conflicts before translation begins. (\u0060src/DCoding.Data.DVault/DataVaultAnnotationNames.cs:72-80\u0060 introduces authoritative source annotations, \u0060src/DCoding.Data.DVault/DataVaultMetadataSourceAnnotations.cs:24-44\u0060 records them before translation and throws on conflicts, and \u0060src/DCoding.Data.DVault/DataVaultDbContextOptionsExtension.cs:86-107\u0060 varies the EF model cache key by source kind and fingerprint.).",
    "DoD check passed: README or equivalent visible docs show the one-time registration flow and the no-service-location DbContext/model usage. (\u0060README.md:70-101\u0060 adds a visible one-time registration example using \u0060services.AddDVault(options =\u003E options.UseMetadataModel(...))\u0060 together with \u0060options.UseDataVaultMetadata()\u0060 on the DbContext, and the sample DbContext has no service-location logic in \u0060OnModelCreating\u0060.).",
    "DoD check passed: No child tickets, planning documents, or relation mutations are required to complete this refinement pass. (The delivered repo changes relevant to this ticket are limited to product code, tests, the public API snapshot, and README updates; there is no required child-ticket, planning-document, or relation-mutation deliverable in the contract, and none is needed to assess completion here.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "DoD check failed: Tests prove registry-backed projection and explicit metadata projection produce the same schema shape for the same metadata source, and prove the no-opt-in baseline still leaves UseDataVault() annotation-only. (The no-opt-in baseline is still proven by existing tests, but the new registry-vs-explicit parity proof in \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataRegistrationTests.cs:39-72\u0060 only compares entity names and property names. It does not prove primary keys, indexes, ordinals, or DVault annotations match, so this DoD is not fully met.).",
    "Blocking: \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataRegistrationTests.cs:39-72\u0060 is the only new registry-vs-explicit parity proof, but its \u0060ModelShape\u0060 omits key definitions, indexes, ordinals, table kinds, and other DVault annotations. A regression in those schema-shape details would still pass, so Definition of Done 3 is not satisfied."
  ],
  "evidence": [
    "\u0060git diff --name-only develop...359afe6f0b42\u0060 shows the implementation is concentrated in \u0060README.md\u0060, \u0060src/DCoding.Data.DVault/*\u0060, and \u0060tests/DCoding.Data.DVault.Tests/*\u0060 for the ticketed feature work.",
    "\u0060src/DCoding.Data.DVault/DataVaultOptions.cs:61-82\u0060 adds app-level metadata registration through \u0060UseMetadataModel(...)\u0060 and \u0060UseMetadataRegistry(...)\u0060.",
    "\u0060src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs:16-60\u0060 and \u0060src/DCoding.Data.DVault/DataVaultDbContextOptionsExtension.cs:70-179\u0060 add the DbContext-scoped opt-in, registry resolution, and model-cache-key wiring.",
    "\u0060src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:122-179\u0060 records metadata-source annotations and routes registry-backed projection through the existing EF metadata translator.",
    "\u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs:13-86\u0060 covers app-default projection, explicit context override, model-cache-key separation, and source-conflict diagnostics.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataRegistrationTests.cs:39-72\u0060 is the added parity test, and its helper only serializes entity names plus property names.",
    "\u0060git diff --check develop...359afe6f0b42 -- src/DCoding.Data.DVault tests/DCoding.Data.DVault.Tests README.md\u0060 returned no whitespace or patch-format errors.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/configuration, area/developer-experience, area/ef-core, area/modeling, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F0MEBFTW8FY5T7PY5HJ5JXJ4-task-refactor-save-and-read-services-to-consume\u0027.",
    "Ticket history references implementation commit \u0027359afe6f0b42\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Strengthen the registry-vs-explicit parity test to compare full schema shape, for example by reusing the richer shape assertions already present in \u0060DataVaultEfMetadataTranslationTests\u0060 or by adding equivalent SQLite schema assertions.",
    "After the parity coverage is fixed, run the policy verification commands in the supported environment: \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060."
  ],
  "branchName": "ticket/06F0MEB634X6CTBZ00W108G3FG-task-register-metadata-model-through-adddvault-a",
  "commitSha": "359afe6f0b42"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F0MEB634X6CTBZ00W108G3FG`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F0MEB634X6CTBZ00W108G3FG-task-register-metadata-model-through-adddvault-a`