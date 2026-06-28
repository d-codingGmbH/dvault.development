[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706FGX69QJYHGNKBV8MJ1HG7MMG\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06FGX69QJYHGNKBV8MJ1HG7MMG-task-implement-hash-key-storage-migration-manife\u0027 and commit \u00276e5b33c5a023\u0027 (verification-source contract).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06FGX69QJYHGNKBV8MJ1HG7MMG-task-implement-hash-key-storage-migration-manife\u0027 from source \u00276e5b33c5a023\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06FGX69QJYHGNKBV8MJ1HG7MMG-task-implement-hash-key-storage-migration-manife\u0027.",
    "Evidence: git diff --name-only develop...6e5b33c5a023 shows six implementation files under review: the new validator, the new validation finding/result types, DataVaultDesignTimeCommandTests.cs, DataVaultHashKeyStorageMigrationManifestValidatorTests.cs, and the DCoding.Data.DVault public API snapshot.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs adds a producer-backed acceptance check that parses the emitted hash-key-storage-migration JSON and asserts DataVaultHashKeyStorageMigrationManifestValidator.ValidateJson(firstJson) is valid with info code hash-key-migration-manifest-compatible.",
    "Evidence: src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestValidator.cs:181-242 validates endpoint metadataSourceKind, providerName, capabilityProfile, and capabilityProfileDefaulted, but the file has no metadataSourceFingerprint validation path.",
    "Evidence: src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestExporter.cs:102-108 serializes MetadataSourceFingerprint into each endpoint, and :238-241 rejects changed source/target metadata fingerprints during exporter-side pairing.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/DataVaultHashKeyStorageMigrationManifestValidatorTests.cs exercises schema version, missing sections, duplicate coverage, unsupported provider/profile/value-format/conversion/hash facts, mixed storage profiles, warning behavior, and ordering; its metadataSourceFingerprint occurrences are the valid fixture fields only.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/diagnostics, area/hashing, area/migrations, area/tests, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06FGX69QJYHGNKBV8MJ1HG7MMG-task-implement-hash-key-storage-migration-manife\u0027.",
    "Evidence: Ticket history references implementation commit \u00276e5b33c5a023\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: At least one validator acceptance case feeds the validator a manifest matching the current emitted top-level shape schemaVersion, dryRun, source, target, comparison, and entries, and that artifact validates successfully when it preserves the checked-in HexString-to-Binary storage-only semantics. (tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs exercises a producer-emitted dvault.hash-key-storage-migration.v1 artifact and asserts ValidateJson(firstJson) succeeds, and DataVaultHashKeyStorageMigrationManifestValidatorTests.cs also includes a valid current-shape fixture case.).",
    "AC check passed: Invalid-manifest tests use deterministic inline or helper-built current-shape JSON fixtures derived from a known-valid producer artifact shape; the ticket does not depend on the fail-closed producer to emit invalid output files. (tests/DCoding.Data.DVault.Tests/Unit/DataVaultHashKeyStorageMigrationManifestValidatorTests.cs builds invalid manifests by mutating the helper-created valid JSON fixture in memory rather than depending on producer-emitted invalid files.).",
    "AC check passed: Warning findings remain limited to non-blocking supplemental-evidence gaps after authoritative source evidence is complete, info findings remain deterministic and redacted, and overall finding order remains stable by severity, code, table, column, and JSON path. (CreateResult/SortFindings in the validator impose deterministic ordering by severity, code, table, column, and path, and the tests cover warning behavior plus stable finding ordering.).",
    "DoD check passed: Implementation lands under the existing DVault source and test layout with validator-side automated coverage for one valid current-producer artifact and the bounded invalid current-shape fixture cases. (The implementation is under the existing src/tests layout and includes validator-side coverage for a producer-generated valid artifact plus helper-built invalid fixtures.).",
    "DoD check passed: Tests cover invalid schemaVersion, missing coverage, duplicate coverage, unsupported provider, profile, value-format, conversion, or hash facts, mixed storage-profile cases, algorithm, digest-length, or digest-encoding drift, and deterministic finding ordering. (DataVaultHashKeyStorageMigrationManifestValidatorTests.cs covers invalid schemaVersion, missing sections, missing and duplicate coverage, unsupported provider/profile/value-format/conversion/hash facts, mixed storage profiles, digest/hash drift, and deterministic ordering.).",
    "DoD check passed: The validator surface stays diagnostics and preflight only and does not mutate the producer, emit a new manifest version, or require live database access. (The added surface is diagnostics/preflight only: a new ValidateJson API plus result/finding types, with no producer mutation, new manifest version, or live database access added.).",
    "DoD check passed: Checked-in code and tests continue to honor the visible built-in provider profile and stable-hash baselines already present in repository code. (The validator hard-codes the visible provider-profile ids and stable-hash ids/digest lengths that match the repository baselines already present in DataVaultProviderCapabilityProfiles.cs and BuiltInStableHashService.cs.).",
    "DoD check passed: Ticket wording and risks reflect that 06FGX67TZV1F6S949F96ZE201W is done upstream context while 06FGX6B9KQME0NJ8B810239DG0 remains the active downstream dependent. (The ticket snapshot already states 06FGX67TZV1F6S949F96ZE201W is done upstream context and 06FGX6B9KQME0NJ8B810239DG0 remains the downstream dependent.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: The validator maps the current serialized shape to the v1 semantic contract: source and target prove boundary and provider facts, entries is complete column coverage, and comparison plus per-entry facts prove the intended HexString-to-Binary change and aggregate counts. (src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestValidator.cs maps endpoint metadataSourceKind/provider facts but never reads or validates metadataSourceFingerprint, so the current serialized shape is not fully mapped to the v1 semantic contract described for source/target provenance.).",
    "AC check failed: The validator returns deterministic error findings for malformed or semantically invalid current-shape manifests, including missing required sections or per-entry facts, duplicate or missing coverage identity, mixed or ambiguous source or target profiles, unsupported provider, profile, value-format, conversion, or hash facts, algorithm drift, digest-length drift, or digest-encoding drift. (ValidateEndpointPair(...) only compares capability profile and provider name; there is no deterministic error path for endpoint provenance drift such as changed metadataSourceFingerprint, so semantically invalid current-shape manifests can still validate successfully.).",
    "Endpoint provenance validation is incomplete: changed source/target metadata fingerprints are not rejected by the new validator, so a semantically invalid storage-only migration manifest can be reported as compatible.",
    "Regression coverage does not include an invalid current-shape fixture for endpoint provenance drift, so the provenance gap is not caught by tests."
  ],
  "evidence": [
    "git diff --name-only develop...6e5b33c5a023 shows six implementation files under review: the new validator, the new validation finding/result types, DataVaultDesignTimeCommandTests.cs, DataVaultHashKeyStorageMigrationManifestValidatorTests.cs, and the DCoding.Data.DVault public API snapshot.",
    "tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs adds a producer-backed acceptance check that parses the emitted hash-key-storage-migration JSON and asserts DataVaultHashKeyStorageMigrationManifestValidator.ValidateJson(firstJson) is valid with info code hash-key-migration-manifest-compatible.",
    "src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestValidator.cs:181-242 validates endpoint metadataSourceKind, providerName, capabilityProfile, and capabilityProfileDefaulted, but the file has no metadataSourceFingerprint validation path.",
    "src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestExporter.cs:102-108 serializes MetadataSourceFingerprint into each endpoint, and :238-241 rejects changed source/target metadata fingerprints during exporter-side pairing.",
    "tests/DCoding.Data.DVault.Tests/Unit/DataVaultHashKeyStorageMigrationManifestValidatorTests.cs exercises schema version, missing sections, duplicate coverage, unsupported provider/profile/value-format/conversion/hash facts, mixed storage profiles, warning behavior, and ordering; its metadataSourceFingerprint occurrences are the valid fixture fields only.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/diagnostics, area/hashing, area/migrations, area/tests, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06FGX69QJYHGNKBV8MJ1HG7MMG-task-implement-hash-key-storage-migration-manife\u0027.",
    "Ticket history references implementation commit \u00276e5b33c5a023\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Update DataVaultHashKeyStorageMigrationManifestValidator to validate the full endpoint provenance contract for the current producer shape, at minimum rejecting source/target metadataSourceFingerprint drift and any other required endpoint facts needed to prove the storage-only boundary.",
    "Add regression coverage in DataVaultHashKeyStorageMigrationManifestValidatorTests for endpoint provenance drift or omission using mutated valid-shape fixtures.",
    "After the fix, rerun deterministic verification for dotnet test DVault.slnx --nologo and bash tools/check-format.sh in the supported environment."
  ],
  "branchName": "ticket/06FGX69QJYHGNKBV8MJ1HG7MMG-task-implement-hash-key-storage-migration-manife",
  "commitSha": "6e5b33c5a023"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06FGX69QJYHGNKBV8MJ1HG7MMG`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06FGX69QJYHGNKBV8MJ1HG7MMG-task-implement-hash-key-storage-migration-manife`