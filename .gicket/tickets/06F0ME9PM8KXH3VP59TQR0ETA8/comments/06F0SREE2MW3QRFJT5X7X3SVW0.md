[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F0ME9PM8KXH3VP59TQR0ETA8\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F0ME9PM8KXH3VP59TQR0ETA8-task-implement-fluent-hub-and-satellite-metadata\u0027 and commit \u00272b96d65d28ce\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F0ME9PM8KXH3VP59TQR0ETA8-task-implement-fluent-hub-and-satellite-metadata\u0027 from source \u00272b96d65d28ce\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F0ME9PM8KXH3VP59TQR0ETA8-task-implement-fluent-hub-and-satellite-metadata\u0027.",
    "Evidence: \u0060git rev-parse --verify 2b96d65d28ce^{commit}\u0060 resolved to \u00602b96d65d28ce2c0ce4023f3e686de50bb31eb514\u0060.",
    "Evidence: \u0060git diff --name-only develop...2b96d65d28ce -- src/DCoding.Data.DVault\u0060 returned only \u0060src/DCoding.Data.DVault/DataVaultCodeFirstHubBuilder.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultCodeFirstSatelliteBuilder.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultCodeFirstSelector.cs\u0060, and \u0060src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs\u0060.",
    "Evidence: \u0060sed -n \u00271,220p\u0027 src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs\u0060 shows the new fluent overload creates \u0060new DataVaultCodeFirstModelBuilder()\u0060, invokes the callback, then calls \u0060modelBuilder.ApplyDataVaultMetadata(codeFirstModelBuilder.BuildMetadataModel(), providerCapabilities)\u0060.",
    "Evidence: \u0060sed -n \u00271,260p\u0027 tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstMetadataTranslationTests.cs\u0060 shows parity tests for ordered \u0060BusinessKey(...)\u0060 and \u0060Payload(...)\u0060, a multi-active \u0060DrivingKey(...)\u0060 parity test, and actionable selector-validation tests.",
    "Evidence: \u0060git diff --name-only develop...2b96d65d28ce -- tests/DCoding.Data.DVault.Tests\u0060 returned \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstMetadataTranslationTests.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060.",
    "Evidence: \u0060git diff --name-only develop...2b96d65d28ce -- docs/plans\u0060 returned no changed planning files, and \u0060test -f docs/plans/06F0ME9PM8KXH3VP59TQR0ETA8-fluent-hub-satellite-child-boundary.md \u0026\u0026 echo present\u0060 plus \u0060test -f docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md \u0026\u0026 echo present\u0060 both returned \u0060present\u0060.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/api, area/ef-core, area/modeling, area/tests, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06F0MEAD1BAA5QEVM3F9QJA38G-task-add-code-first-migration-and-schema-parity\u0027.",
    "Evidence: Ticket history references implementation commit \u00272b96d65d28ce\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: A new additive fluent overload accepts hub declarations by CLR entity type, builds provider-neutral metadata, and reuses the existing ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel, ...) projection path without regressing current metadata-first overloads. (\u0060src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs\u0060 adds additive \u0060ApplyDataVaultMetadata(Action\u003CDataVaultCodeFirstModelBuilder\u003E...)\u0060 overloads, builds provider-neutral metadata through \u0060DataVaultCodeFirstModelBuilder\u0060, and then reuses the existing \u0060ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel, ...)\u0060 path.).",
    "AC check passed: Repeated direct single-member BusinessKey(...), Payload(...), and DrivingKey(...) calls preserve declaration order and produce deterministic hub and satellite tables, columns, keys, and indexes matching the existing metadata-first schema rules for the covered hub-parent shapes. (\u0060DataVaultCodeFirstHubBuilder\u0060 appends repeated \u0060BusinessKey(...)\u0060 member names in call order, \u0060DataVaultCodeFirstSatelliteBuilder\u0060 appends repeated \u0060Payload(...)\u0060 and \u0060DrivingKey(...)\u0060 names in call order, and \u0060DataVaultCodeFirstMetadataTranslationTests\u0060 compares the fluent result against a metadata-first baseline for covered hub-parent shapes.).",
    "AC check passed: DrivingKey(...) is the only fluent multi-active opt-in for this child; one or more calls populate DataVaultSatelliteMetadata.DrivingKeyNames and yield the existing multi-active satellite key and index ordering for hub-parent satellites. (\u0060DataVaultCodeFirstModelBuilder.CreateSatelliteMetadata(...)\u0060 passes non-empty \u0060DrivingKeyNames\u0060 into \u0060DataVaultSatelliteMetadata\u0060, and the multi-active parity test configures ordered \u0060DrivingKey(...)\u0060 calls on a hub-parent satellite that is projected through the existing translator.).",
    "AC check passed: Unsupported selector shapes such as anonymous-object, computed, or non-member selectors fail with actionable validation messages that direct callers to use repeated single-member selector calls. (\u0060src/DCoding.Data.DVault/DataVaultCodeFirstSelector.cs\u0060 accepts only direct single-member access on the configured entity parameter and throws an actionable \u0060ArgumentException\u0060 instructing callers to use repeated single-member selector calls; the new unit tests assert failures for \u0060BusinessKey\u0060, \u0060Payload\u0060, and \u0060DrivingKey\u0060.).",
    "DoD check passed: Public API and snapshot coverage reflect the additive fluent overload and new root-namespace DataVaultCodeFirst*Builder types without breaking the existing DCoding.Data.DVault.Modeling builders. (\u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060 now includes the additive overloads and root-namespace \u0060DataVaultCodeFirst*Builder\u0060 types, and the branch source diff under \u0060src/DCoding.Data.DVault\u0060 does not modify the existing \u0060DCoding.Data.DVault.Modeling\u0060 builder surface.).",
    "DoD check passed: The fluent path emits the same provider-neutral metadata names and canonical ordering that the current translator and provider capability profiles already expect, including multi-active driving-key columns. (\u0060DataVaultCodeFirstModelBuilder\u0060 derives the hub name from \u0060typeof(TEntity).Name\u0060, preserves selector order in list-backed declarations, and hands the resulting \u0060DataVaultMetadataModel\u0060 to the current translator/provider-capability path without introducing alternate naming or ordering logic.).",
    "DoD check passed: Tests cover ordinary hub-parent satellites, the covered DrivingKey(...) multi-active hub-parent satellite scenario, and validation failures for unsupported selectors. (\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstMetadataTranslationTests.cs\u0060 covers ordinary hub-parent satellite parity, multi-active \u0060DrivingKey(...)\u0060 parity, and validation failures for unsupported selector shapes.).",
    "DoD check passed: No link, link-parent satellite, save-service, registry/model-first, PIT, or bridge behavior is introduced by this ticket. (\u0060git diff --name-only develop...2b96d65d28ce -- src/DCoding.Data.DVault\u0060 shows only \u0060DataVaultCodeFirstHubBuilder.cs\u0060, \u0060DataVaultCodeFirstModelBuilder.cs\u0060, \u0060DataVaultCodeFirstSatelliteBuilder.cs\u0060, \u0060DataVaultCodeFirstSelector.cs\u0060, and \u0060DataVaultModelBuilderExtensions.cs\u0060; no link, link-parent satellite, save-service, registry/model-first, PIT, or bridge source files were changed.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Targeted tests prove schema equivalence for covered hub and hub-parent satellite scenarios, and existing metadata-first tests continue to pass unchanged. (Targeted parity tests were added in \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstMetadataTranslationTests.cs\u0060, but existing metadata-first test file \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs\u0060 was also edited, so the committed repo state does not satisfy the contract clause that existing metadata-first tests continue unchanged.).",
    "Blocking: \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs\u0060 was modified to accommodate the new overload set, which conflicts with the persisted acceptance criterion requiring existing metadata-first tests to continue unchanged.",
    "No direct \u0060dotnet test DVault.slnx --nologo\u0060 or \u0060bash tools/check-format.sh\u0060 execution was performed in this read-only review session."
  ],
  "evidence": [
    "\u0060git rev-parse --verify 2b96d65d28ce^{commit}\u0060 resolved to \u00602b96d65d28ce2c0ce4023f3e686de50bb31eb514\u0060.",
    "\u0060git diff --name-only develop...2b96d65d28ce -- src/DCoding.Data.DVault\u0060 returned only \u0060src/DCoding.Data.DVault/DataVaultCodeFirstHubBuilder.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultCodeFirstSatelliteBuilder.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultCodeFirstSelector.cs\u0060, and \u0060src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs\u0060.",
    "\u0060sed -n \u00271,220p\u0027 src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs\u0060 shows the new fluent overload creates \u0060new DataVaultCodeFirstModelBuilder()\u0060, invokes the callback, then calls \u0060modelBuilder.ApplyDataVaultMetadata(codeFirstModelBuilder.BuildMetadataModel(), providerCapabilities)\u0060.",
    "\u0060sed -n \u00271,260p\u0027 tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstMetadataTranslationTests.cs\u0060 shows parity tests for ordered \u0060BusinessKey(...)\u0060 and \u0060Payload(...)\u0060, a multi-active \u0060DrivingKey(...)\u0060 parity test, and actionable selector-validation tests.",
    "\u0060git diff --name-only develop...2b96d65d28ce -- tests/DCoding.Data.DVault.Tests\u0060 returned \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstMetadataTranslationTests.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060.",
    "\u0060git diff --name-only develop...2b96d65d28ce -- docs/plans\u0060 returned no changed planning files, and \u0060test -f docs/plans/06F0ME9PM8KXH3VP59TQR0ETA8-fluent-hub-satellite-child-boundary.md \u0026\u0026 echo present\u0060 plus \u0060test -f docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md \u0026\u0026 echo present\u0060 both returned \u0060present\u0060.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/api, area/ef-core, area/modeling, area/tests, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F0MEAD1BAA5QEVM3F9QJA38G-task-add-code-first-migration-and-schema-parity\u0027.",
    "Ticket history references implementation commit \u00272b96d65d28ce\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Rework the branch so the ticket satisfies AC5 as written, or explicitly revise the persisted contract if adapting existing metadata-first tests is intended to be acceptable.",
    "After rework, run deterministic verification in a writable tester environment with \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060."
  ],
  "branchName": "ticket/06F0ME9PM8KXH3VP59TQR0ETA8-task-implement-fluent-hub-and-satellite-metadata",
  "commitSha": "2b96d65d28ce"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F0ME9PM8KXH3VP59TQR0ETA8`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F0ME9PM8KXH3VP59TQR0ETA8-task-implement-fluent-hub-and-satellite-metadata`