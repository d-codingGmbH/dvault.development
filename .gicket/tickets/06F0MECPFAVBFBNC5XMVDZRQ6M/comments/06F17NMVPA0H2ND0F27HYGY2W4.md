[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 7/7 acceptance criteria and 5/5 definition-of-done expectations on branch \u0027ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p\u0027 at commit \u00276438c5bbc042\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p",
    "commitSha": "6438c5bbc042",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "Callers can use \u0060ReadLatestSatelliteAsync\u003CTProjection\u003E(DbContext, DataVaultLatestSatelliteReadRequest, Func\u003CDataVaultSatelliteProjectionRow,TProjection\u003E, CancellationToken)\u0060 and the matching overload for \u0060DataVaultRegistryLatestSatelliteReadRequest\u0060, while the existing raw \u0060ReadLatestSatelliteRowsAsync\u0060 API remains source-compatible.",
      "satisfied": true,
      "reason": "\u0060src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs\u0060 adds \u0060ReadLatestSatelliteAsync\u003CTProjection\u003E(..., DataVaultLatestSatelliteReadRequest, Func\u003CDataVaultSatelliteProjectionRow,TProjection\u003E, ...)\u0060, \u0060src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs\u0060 adds the registry-backed overload, and \u0060src/DCoding.Data.DVault/IDataVaultReadService.cs\u0060 still exposes the existing raw \u0060ReadLatestSatelliteRowsAsync\u0060 surface unchanged."
    },
    {
      "expectation": "The same projector delegate contract works for both explicit-metadata and registry-backed request paths, and the registry-backed overload resolves metadata once then reuses the same typed projection pipeline.",
      "satisfied": true,
      "reason": "The registry-backed overload resolves registry metadata once and then calls the explicit typed overload with \u0060new DataVaultLatestSatelliteReadRequest(...)\u0060, so both entry points share the same projector delegate contract and typed pipeline."
    },
    {
      "expectation": "Inside \u0060DataVaultSatelliteProjectionRow\u0060, exact-name access supports \u0060ParentHashKey\u0060, \u0060HashDiff\u0060, \u0060LoadTimestamp\u0060, \u0060RecordSource\u0060, declared driving-key names, and declared payload names using \u0060StringComparer.Ordinal\u0060, while preserving current latest/as-of selection and multi-active series semantics.",
      "satisfied": true,
      "reason": "\u0060src/DCoding.Data.DVault/DataVaultSatelliteProjectionRow.cs\u0060 defines exact-name access for \u0060ParentHashKey\u0060, \u0060HashDiff\u0060, \u0060LoadTimestamp\u0060, and \u0060RecordSource\u0060, documents ordinal matching, and \u0060src/DCoding.Data.DVault/DataVaultSatelliteReadPipeline.cs\u0060 maps those names plus driving keys and payload names while preserving the existing parent/driving-key/load-timestamp series selection flow."
    },
    {
      "expectation": "Required versus nullable behavior is explicit at the mapping call site: \u0060RequiredString(...)\u0060 and \u0060RequiredDateTimeOffset(\u0022LoadTimestamp\u0022)\u0060 fail on missing, null, or invalid values; \u0060NullableString(...)\u0060 returns \u0060null\u0060 only for an existing mapped name whose provider value is null; a missing mapped name always fails.",
      "satisfied": true,
      "reason": "\u0060GetRequiredValue\u0060 raises \u0060missing-name\u0060, \u0060RequiredString\u0060 and \u0060RequiredDateTimeOffset\u0060 raise \u0060null-value\u0060 or \u0060invalid-value\u0060 for null/invalid provider values, and \u0060NullableString\u0060 returns \u0060null\u0060 only for a present mapped name whose provider value is null."
    },
    {
      "expectation": "Projection failures throw \u0060InvalidOperationException\u0060 with deterministic prefix \u0060DVault typed satellite projection failed ({failureKind})\u0060 and include the satellite metadata name and offending mapped name, where v1 \u0060failureKind\u0060 tokens are \u0060missing-name\u0060, \u0060null-value\u0060, or \u0060invalid-value\u0060.",
      "satisfied": true,
      "reason": "\u0060DataVaultSatelliteProjectionFailures.Create\u0060 emits the deterministic \u0060DVault typed satellite projection failed ({failureKind})\u0060 prefix and every typed accessor/validator passes the satellite metadata name and offending mapped name into that message."
    },
    {
      "expectation": "Before any row materialization, the typed helper rejects satellites whose payload or driving-key names equal \u0060ParentHashKey\u0060, \u0060HashDiff\u0060, \u0060LoadTimestamp\u0060, or \u0060RecordSource\u0060 by \u0060StringComparer.Ordinal\u0060.",
      "satisfied": true,
      "reason": "\u0060ValidateReservedProjectionNames(request.Satellite)\u0060 runs before \u0060ReadLatestProjectionRowsAsync\u0060, and the reserved-name test uses an empty parent-key request to prove the rejection happens before any query-driven row materialization."
    },
    {
      "expectation": "Automated tests cover explicit and registry-backed latest/as-of parity, ordinary and multi-active projections, required-versus-nullable behavior, reserved-name rejection, and \u0060LoadTimestamp\u0060 normalization across provider-default, ISO 8601 UTC text, and UTC-ticks storage.",
      "satisfied": true,
      "reason": "\u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedSatelliteReadServiceSqliteTests.cs\u0060 covers explicit latest reads, registry-backed as-of reads, link-parent reads, multi-active series selection, missing/null/invalid diagnostics, reserved-name rejection, and \u0060LoadTimestamp\u0060 normalization for provider-default, ISO 8601 UTC text, and UTC-ticks storage."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "Public API surface, XML docs, and snapshot coverage include the typed helper overloads, \u0060DataVaultSatelliteProjectionRow\u0060, the required/nullability accessors, and one visible registry-backed example plus the explicit-metadata variant.",
      "satisfied": true,
      "reason": "The new overloads and \u0060DataVaultSatelliteProjectionRow\u0060 are documented in XML comments with explicit and registry-backed examples, and \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060 includes the public typed helper and accessor surface."
    },
    {
      "expectation": "Implementation remains additive to the current raw read service and reuses existing batching, satellite resolution, latest/as-of series selection, and timestamp normalization logic instead of introducing a second provider-neutral read engine.",
      "satisfied": true,
      "reason": "\u0060src/DCoding.Data.DVault/DefaultDataVaultReadService.cs\u0060 still serves the raw API, but now delegates both raw and typed reads to \u0060src/DCoding.Data.DVault/DataVaultSatelliteReadPipeline.cs\u0060, which reuses the existing batching, latest/as-of selection, and timestamp normalization logic instead of introducing a separate read engine."
    },
    {
      "expectation": "The typed path validates reserved technical-name collisions before query execution and never relies on the current silent-skip behavior in \u0060DefaultDataVaultReadService\u0060 for required/null diagnostics.",
      "satisfied": true,
      "reason": "Reserved-name validation is performed before typed reads start, and the typed path builds a mapped-name value map that distinguishes missing values from present-null values instead of relying on the old raw-record silent-skip behavior for required/null diagnostics."
    },
    {
      "expectation": "Tests prove parity for hub-parent, link-parent, ordinary, and multi-active satellite reads and prove the typed \u0060LoadTimestamp\u0060 accessor returns the same UTC values across supported storage modes.",
      "satisfied": true,
      "reason": "The SQLite integration suite proves hub-parent and link-parent projections, ordinary and multi-active satellite reads, and UTC-normalized \u0060LoadTimestamp\u0060 behavior across all three supported storage modes."
    },
    {
      "expectation": "Existing raw \u0060DataVaultSatelliteReadRecord\u0060 reads remain available and source-compatible as the advanced escape hatch.",
      "satisfied": true,
      "reason": "The raw \u0060IDataVaultReadService.ReadLatestSatelliteRowsAsync(...)\u0060 API remains unchanged, the raw registry extension remains present, and \u0060DataVaultSatelliteReadRecord\u0060 stays in the approved public API snapshot as the escape hatch."
    }
  ],
  "evidence": [
    "\u0060git diff --stat develop...6438c5bbc042 -- src/DCoding.Data.DVault tests/DCoding.Data.DVault.Tests\u0060 reported 9 relevant file changes touching the typed projection extensions, shared read pipeline, projection row type, integration coverage, provider-category discovery, and public API snapshot.",
    "\u0060git show --stat --oneline 6438c5bbc042\u0060 showed the handoff commit itself only adjusted \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060; the full claimed implementation is present in the cumulative branch diff against \u0060develop\u0060.",
    "\u0060git diff --name-only 6438c5bbc042..HEAD -- src/DCoding.Data.DVault tests/DCoding.Data.DVault.Tests\u0060 returned no paths, so the inspected source/test files under those directories still match the claimed commit for the affected surface.",
    "\u0060src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs\u0060 adds the explicit-metadata typed helper and calls \u0060ValidateReservedProjectionNames\u0060 before reading rows.",
    "\u0060src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs\u0060 adds the registry-backed typed overload and delegates to the explicit typed helper after one registry resolution.",
    "\u0060src/DCoding.Data.DVault/DataVaultSatelliteProjectionRow.cs\u0060 provides \u0060RequiredString\u0060, \u0060NullableString\u0060, and \u0060RequiredDateTimeOffset\u0060, with deterministic \u0060missing-name\u0060, \u0060null-value\u0060, and \u0060invalid-value\u0060 failure construction.",
    "\u0060src/DCoding.Data.DVault/DataVaultSatelliteReadPipeline.cs\u0060 centralizes raw and typed latest/as-of selection, batches parent hash keys in chunks of 500, reuses \u0060DataVaultLoadTimestampValueConverter\u0060, and builds projection values that distinguish missing from present-null provider values.",
    "\u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedSatelliteReadServiceSqliteTests.cs\u0060 adds targeted coverage for explicit/registry typed reads, link-parent reads, multi-active reads, reserved-name rejection, required-vs-nullable behavior, invalid timestamp diagnostics, and timestamp normalization.",
    "\u0060tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs\u0060 adds \u0060DataVaultTypedSatelliteReadServiceSqliteTests\u0060 to required local SQLite coverage, and \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060 includes the new overloads and public row accessors.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/api, area/developer-experience, area/read-models, area/tests, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F0MEDJC732GDD77H60R259P0-task-update-readme-and-release-docs-for-v0-6-0-u\u0027.",
    "Ticket history references implementation commit \u00276438c5bbc042\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [
    "No blocking findings from direct branch-diff and repository inspection."
  ],
  "nextSteps": [
    "Hand off to \u0060integrator\u0060."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F0MECPFAVBFBNC5XMVDZRQ6M`
- target-role: `integrator`
- verification-summary: Tester verified 7/7 acceptance criteria and 5/5 definition-of-done expectations on branch 'ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p' at commit '6438c5bbc042'.
- acceptance-criteria: `7/7` satisfied
- definition-of-done: `5/5` satisfied
- implementation-branch: `ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p`
- implementation-commit: `6438c5bbc042`
- implementation-pr: `<none>`
- implementation-change: `<none>`