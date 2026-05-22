[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F492AYE4A3PKA2D20DDPQ37C\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F492AYE4A3PKA2D20DDPQ37C-story-add-optional-ef-runtime-guard-interceptor\u0027 and commit \u002750ff0d792f34\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F492AYE4A3PKA2D20DDPQ37C-story-add-optional-ef-runtime-guard-interceptor\u0027 from source \u002750ff0d792f34\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F492AYE4A3PKA2D20DDPQ37C-story-add-optional-ef-runtime-guard-interceptor\u0027.",
    "Evidence: \u0060git show --name-only 50ff0d792f34\u0060 shows the claimed implementation changes only the new guard source/API files, one guard unit test file, one guard SQLite integration test file, provider test discovery, and the public API snapshot.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs\u0060 adds \u0060UseDataVaultSaveChangesGuardInterceptor(...)\u0060, while \u0060src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs\u0060 still registers only the existing DVault services on \u0060AddDVault()\u0060.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultSaveChangesGuardInterceptor.cs\u0060 evaluates only annotated hub/link/satellite entries and derives required Added-row checks from DVault property-role and technical-column annotations instead of fixed names.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveChangesGuardInterceptorSqliteTests.cs\u0060 contains end-to-end coverage for blocking modified/deleted rows, blocking missing structural values, warning-mode reporting, metadata-fill coexistence, explicit save-service compatibility, and annotation-based detection.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultSaveChangesGuardInterceptorRegistrationTests.cs\u0060 contains six unit tests for registration, option mode toggling, deterministic explanation formatting, and exception/report exposure, but no unit-level execution of the guard decision logic itself.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/diagnostics, area/ef-core, area/persistence, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06F492AYE4A3PKA2D20DDPQ37C-story-add-optional-ef-runtime-guard-interceptor\u0027.",
    "Evidence: Ticket history references implementation commit \u002750ff0d792f34\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: A consumer can opt a DbContext into runtime guard behavior through new explicit DbContextOptionsBuilder API(s), and the existing default AddDVault() path still registers no runtime guard interceptor. (The branch adds explicit \u0060UseDataVaultSaveChangesGuardInterceptor(...)\u0060 DbContextOptionsBuilder opt-in APIs in \u0060src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs\u0060, and \u0060src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs\u0060 still leaves \u0060AddDVault()\u0060 interceptor-free by default.).",
    "AC check passed: In block mode, direct SaveChanges on generated DVault hub, link, or satellite entries that are in Modified or Deleted state, or Added entries that still lack required non-fillable DVault structural values, fails with a deterministic explanation of the offending entries and reasons. (\u0060src/DCoding.Data.DVault/DataVaultSaveChangesGuardInterceptor.cs\u0060 blocks Modified and Deleted generated hub/link/satellite entries and reports Added rows missing required structural values through \u0060DataVaultSaveChangesGuardReport\u0060 and \u0060DataVaultSaveChangesGuardException\u0060; the SQLite guard integration tests cover both cases.).",
    "AC check passed: In warning mode, the same findings are emitted through a deterministic caller-facing explanation surface without silently mutating the tracked rows or requiring a logging dependency. (Warning mode uses a caller-supplied \u0060Action\u003CDataVaultSaveChangesGuardReport\u003E\u0060 without any logging dependency, and the SQLite warning test verifies that findings are reported while \u0060SaveChanges\u0060 still succeeds without mutating the tracked row.).",
    "AC check passed: When UseDataVaultSaveChangesMetadataInterceptor(...) is also configured, rows that are otherwise valid and only rely on interceptor-populated LoadTimestamp or RecordSource are not reported as unsafe. (The guard only requires participant-reference, driving-key, hash-key, and hash-diff values on Added rows, so metadata-filled \u0060LoadTimestamp\u0060 and \u0060RecordSource\u0060 values are not treated as violations; the SQLite coexistence test covers this path.).",
    "AC check passed: IDataVaultSaveService continues to work unchanged as the default write boundary under the guard configuration, and documented direct caller-owned generated-row scenarios that already supply required structural data continue to save successfully. (The SQLite test \u0060ExplicitSaveServiceSucceedsUnderOptInGuard\u0060 shows \u0060IDataVaultSaveService\u0060 still persists generated hub, link, and satellite rows successfully under the opt-in guard, and the default \u0060AddDVault()\u0060 write boundary remains unchanged.).",
    "AC check passed: Detection relies on DVault EF annotations and roles rather than hard-coded table or property names, so effective-name overrides and generated shared-type tables remain supported. (The guard implementation reads DVault EF annotations and roles (\u0060EntityKind\u0060, \u0060PropertyRole\u0060, \u0060TechnicalColumnRole\u0060, produced names, and metadata names) rather than hard-coded table or property names, and the annotated shared-type SQLite test confirms that behavior.).",
    "DoD check passed: Public API snapshot coverage reflects the new runtime guard options, mode or report surface, and DbContextOptionsBuilder opt-in extension methods. (\u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060 includes the new guard opt-in methods and public guard types.).",
    "DoD check passed: SQLite integration tests prove coexistence with the metadata-fill interceptor, safe caller-owned generated-row saves, and guard failures for unsafe tracked DVault hub, link, and satellite mutations. (\u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveChangesGuardInterceptorSqliteTests.cs\u0060 covers metadata-fill coexistence, safe caller-owned generated-row saves, and guard failures for unsafe hub, link, and satellite mutations on SQLite.).",
    "DoD check passed: Tests prove the explicit IDataVaultSaveService path still succeeds under the opt-in guard and remains the documented default write boundary. (The same SQLite guard test file includes explicit \u0060IDataVaultSaveService\u0060 coverage under opt-in guard configuration.).",
    "DoD check passed: The final docs-facing contract remains truthful that this is an optional runtime guardrail, not an implicit persistence model or replacement for analyzers or preflight. (\u0060README.md\u0060 still describes DVault persistence as an explicit service boundary and does not present this work as a default or implicit persistence model; no contradictory docs-facing changes were observed in the claimed commit.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "DoD check failed: Unit tests prove default non-registration, blocking and warning decisions, deterministic explanation content, and annotation-driven detection. (\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultSaveChangesGuardInterceptorRegistrationTests.cs\u0060 covers registration, option toggling, explanation formatting, and exception/report shape, but it does not unit-test guard blocking decisions, warning-report emission, or annotation-driven detection. Those behaviors are only exercised in \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveChangesGuardInterceptorSqliteTests.cs\u0060.).",
    "Definition of Done 2 is not met: the new unit coverage stops at registration and report-surface checks, while the actual guard decision behavior and annotation-driven detection are only validated through SQLite integration tests."
  ],
  "evidence": [
    "\u0060git show --name-only 50ff0d792f34\u0060 shows the claimed implementation changes only the new guard source/API files, one guard unit test file, one guard SQLite integration test file, provider test discovery, and the public API snapshot.",
    "\u0060src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs\u0060 adds \u0060UseDataVaultSaveChangesGuardInterceptor(...)\u0060, while \u0060src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs\u0060 still registers only the existing DVault services on \u0060AddDVault()\u0060.",
    "\u0060src/DCoding.Data.DVault/DataVaultSaveChangesGuardInterceptor.cs\u0060 evaluates only annotated hub/link/satellite entries and derives required Added-row checks from DVault property-role and technical-column annotations instead of fixed names.",
    "\u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveChangesGuardInterceptorSqliteTests.cs\u0060 contains end-to-end coverage for blocking modified/deleted rows, blocking missing structural values, warning-mode reporting, metadata-fill coexistence, explicit save-service compatibility, and annotation-based detection.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultSaveChangesGuardInterceptorRegistrationTests.cs\u0060 contains six unit tests for registration, option mode toggling, deterministic explanation formatting, and exception/report exposure, but no unit-level execution of the guard decision logic itself.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/diagnostics, area/ef-core, area/persistence, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F492AYE4A3PKA2D20DDPQ37C-story-add-optional-ef-runtime-guard-interceptor\u0027.",
    "Ticket history references implementation commit \u002750ff0d792f34\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Add unit tests that exercise \u0060DataVaultSaveChangesGuardInterceptor\u0060 behavior directly for blocking findings, warning callback emission, and annotation-driven detection instead of relying only on SQLite integration coverage.",
    "Keep the existing SQLite guard tests as end-to-end proof for hub/link/satellite behavior, metadata-fill coexistence, and \u0060IDataVaultSaveService\u0060 compatibility.",
    "After the unit coverage gap is closed, rerun \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060."
  ],
  "branchName": "ticket/06F492AYE4A3PKA2D20DDPQ37C-story-add-optional-ef-runtime-guard-interceptor",
  "commitSha": "50ff0d792f34"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F492AYE4A3PKA2D20DDPQ37C`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F492AYE4A3PKA2D20DDPQ37C-story-add-optional-ef-runtime-guard-interceptor`