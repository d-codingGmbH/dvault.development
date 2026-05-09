[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 6/6 acceptance criteria and 5/5 definition-of-done expectations on branch \u0027ticket/06F0MEB634X6CTBZ00W108G3FG-task-register-metadata-model-through-adddvault-a\u0027 at commit \u00270fa20e69a1da\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F0MEB634X6CTBZ00W108G3FG-task-register-metadata-model-through-adddvault-a",
    "commitSha": "0fa20e69a1da",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "A caller can register DVault metadata once during service setup through AddDVault(...) by supplying either a DataVaultMetadataModel or a prebuilt DataVaultMetadataRegistry, and the resulting default registry is immutable and deterministic.",
      "satisfied": true,
      "reason": "AddDVault now accepts both a metadata model and a prebuilt metadata registry, converts a model to DataVaultMetadataRegistry.Create(...) once, and registers the default registry as a singleton; unit tests cover both paths."
    },
    {
      "expectation": "An opted-in DbContext can project the registered metadata through ordinary model configuration without recreating the same metadata declarations in OnModelCreating; a context that uses only the existing UseDataVault() baseline without the new opt-in surface continues to create no DVault tables.",
      "satisfied": true,
      "reason": "UseDataVaultMetadata(...) lets an opted-in DbContext project registered metadata without recreating declarations in OnModelCreating, and the existing UseDataVault()-only SQLite test still proves the no-table baseline."
    },
    {
      "expectation": "Registry-backed projection uses the existing provider-aware metadata translation baseline for the same metadata source, so the produced entities, columns, keys, indexes, and DVault annotations match the current explicit metadata path.",
      "satisfied": true,
      "reason": "Registry-backed projection reuses the existing DataVaultEfMetadataTranslator path after provider selection, and the parity test now compares model, entity, property, primary-key, index, and DVault annotation shape between explicit and registry-backed projection."
    },
    {
      "expectation": "Source selection is deterministic: an explicit context-scoped source overrides the app-level default for that context, but a single EF model that receives two distinct metadata sources fails fast with an actionable validation error that identifies the conflicting source kinds.",
      "satisfied": true,
      "reason": "Context-scoped registries override the app default for that context, model cache keys include source kind plus fingerprint, and conflicting distinct sources throw a DVault-specific error that names the source kinds."
    },
    {
      "expectation": "When a caller explicitly applies metadata through the existing model-level path and a different registry-backed source is also configured for the same model, DVault throws before silent divergence or duplicate projection occurs.",
      "satisfied": true,
      "reason": "When explicit model metadata and a different registry-backed source are configured for the same model, TryRecordSource rejects the second source before registry translation runs, and the integration test asserts the conflict diagnostic."
    },
    {
      "expectation": "Automated tests cover app-level model registration, prebuilt registry registration, context opt-in consumption, preserved UseDataVault() no-table baseline, and conflict diagnostics.",
      "satisfied": true,
      "reason": "Automated coverage exists for app-level model registration, prebuilt registry registration, DbContext opt-in consumption, the preserved UseDataVault() no-table baseline, and conflict diagnostics."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "Public API and snapshot coverage reflect the additive startup and DbContext integration surface while keeping the current optionless AddDVault() and explicit ApplyDataVaultMetadata(...) entry points source-compatible.",
      "satisfied": true,
      "reason": "The public API snapshot captures the additive options, annotations, and DbContext surface while both AddDVault overloads and the explicit ApplyDataVaultMetadata(...) entry point remain present."
    },
    {
      "expectation": "The implementation stores one authoritative registry selection per EF model and validates source conflicts before translation begins.",
      "satisfied": true,
      "reason": "The model records one authoritative DVault source selection via source-kind and fingerprint annotations, registry resolution is deterministic per context/model, and mismatched sources are rejected before the conflicting registry projection reaches the translator."
    },
    {
      "expectation": "Tests prove registry-backed projection and explicit metadata projection produce the same schema shape for the same metadata source, and prove the no-opt-in baseline still leaves UseDataVault() annotation-only.",
      "satisfied": true,
      "reason": "Tests now prove registry-backed and explicit projection match on schema shape and DVault annotations, and the SQLite baseline test still proves UseDataVault() alone remains annotation-only/no-table."
    },
    {
      "expectation": "README or equivalent visible docs show the one-time registration flow and the no-service-location DbContext/model usage.",
      "satisfied": true,
      "reason": "README documents one-time metadata registration and DbContext opt-in usage without service resolution inside OnModelCreating."
    },
    {
      "expectation": "No child tickets, planning documents, or relation mutations are required to complete this refinement pass.",
      "satisfied": true,
      "reason": "The contract lists no required repository output paths, and the observed product diff stays within README, src/DCoding.Data.DVault, and tests/DCoding.Data.DVault.Tests."
    }
  ],
  "evidence": [
    "git diff --name-only develop...0fa20e69a1da shows product changes in README.md, src/DCoding.Data.DVault/*, and tests/DCoding.Data.DVault.Tests/* alongside workflow .gicket artifacts.",
    "git diff --check develop...0fa20e69a1da -- README.md src/DCoding.Data.DVault tests/DCoding.Data.DVault.Tests returned no output.",
    "src/DCoding.Data.DVault/DataVaultOptions.cs:66-80 adds UseMetadataModel(...) and UseMetadataRegistry(...).",
    "src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs:16-60 adds UseDataVaultMetadata() overloads for app-default, metadata-model, and prebuilt-registry opt-in.",
    "src/DCoding.Data.DVault/DataVaultDbContextOptionsExtension.cs:70-155 resolves app-default/context registries and fingerprints the selected source into the model cache key.",
    "src/DCoding.Data.DVault/DataVaultMetadataSourceAnnotations.cs:16-43 and src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:122-177 record source annotations, detect conflicts, and route registry-backed projection through DataVaultEfMetadataTranslator.Apply(...).",
    "tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataRegistrationTests.cs:40-178 now compares model, entity, property, primary-key, index, and annotation shape between explicit and registry-backed projection and also covers AddDVault model/registry registration.",
    "tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs:14-85 covers app-default opt-in, explicit context override, model-cache separation, and explicit-model conflict diagnostics.",
    "tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs:182-200 asserts UseDataVault() alone creates 0 DVault tables.",
    "README.md:70-101 documents one-time registration plus no-service-location DbContext opt-in usage, and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:7-55 captures the additive public surface.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/configuration, area/developer-experience, area/ef-core, area/modeling, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F0MEBFTW8FY5T7PY5HJ5JXJ4-task-refactor-save-and-read-services-to-consume\u0027.",
    "Ticket history references implementation commit \u00270fa20e69a1da\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 1 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to integrator; run dotnet test DVault.slnx --nologo and bash tools/check-format.sh in the supported validation environment if that gate has not already executed there."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F0MEB634X6CTBZ00W108G3FG`
- target-role: `integrator`
- verification-summary: Tester verified 6/6 acceptance criteria and 5/5 definition-of-done expectations on branch 'ticket/06F0MEB634X6CTBZ00W108G3FG-task-register-metadata-model-through-adddvault-a' at commit '0fa20e69a1da'.
- acceptance-criteria: `6/6` satisfied
- definition-of-done: `5/5` satisfied
- implementation-branch: `ticket/06F0MEB634X6CTBZ00W108G3FG-task-register-metadata-model-through-adddvault-a`
- implementation-commit: `0fa20e69a1da`
- implementation-pr: `<none>`
- implementation-change: `<none>`