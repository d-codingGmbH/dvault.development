[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F9GF5FV54DGWY9GA8ZEZWM5R\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract\u0027 and commit \u002741235d339d6d\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract\u0027 from source \u002741235d339d6d\u0027.",
    "Prompt-injection writeback protection rejected tester interactive assessment writeback on attempt 1/2; retrying tester output generation before creating a durable stop.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract\u0027.",
    "Evidence: git diff --name-status develop...41235d339d6d shows a new docs/plans/hash-key-storage-profile-contract.md, documentation updates, source changes under src/DCoding.Data.DVault, and updated tests in provider capability, EF metadata, diagnostics, migration guardrails, SQLite integration, and the public API snapshot.",
    "Evidence: docs/plans/hash-key-storage-profile-contract.md:9-27, :29-61, and :65-78 defines the logical lowercase-hex boundary, HexString default/Binary opt-in vocabulary, four built-in digest-size baselines, support-bundle facts, and fail-closed drift posture.",
    "Evidence: src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs:158-249 projects storage profile, store type, value format, algorithmId, digest length, and conversion behavior for HashKey and ParticipantReference mappings; src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:939-949 writes those facts onto EF property annotations.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs:408-483 covers all six visible provider profiles for HexString sizing and Binary opt-in, and tests/DCoding.Data.DVault.Tests/Integration/SqliteProviderCapabilityProfileTests.cs:10-36 verifies SQLite still stores canonical lowercase-hex hash keys as raw TEXT.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:59-159 verifies explain/support-bundle algorithmId, digestByteLength, digestEncoding, raw-value redaction, and the sha1-v1 versus sha256-160-v1 same-width distinction.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs:565-603 explicitly rejects same-width stable-hash algorithm drift, while src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs:563-582 contains the storage-profile and digest-length comparison logic.",
    "Evidence: git diff --name-only 1edbf49475ad..41235d339d6d -- docs src tests returned no paths, so the post-return rework pass did not add repository docs/source/test changes.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/architecture, area/ef-core, area/hashing, area/schema, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 7 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract\u0027.",
    "Evidence: Ticket history references implementation commit \u002741235d339d6d\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Evidence: Ticket history contains 1 structured return-routing contract comment(s).",
    "Evidence: Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths.",
    "AC check passed: Logical hash-key values remain canonical lowercase hexadecimal strings at API, request, metadata, and diagnostics boundaries regardless of HexString or Binary physical storage. (docs/plans/hash-key-storage-profile-contract.md:9-11 and :23-27 keep logical boundaries as canonical lowercase-hex strings, and src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs:228-247 keeps Binary persistence on a string CLR boundary with conversion rather than changing caller-facing types.).",
    "AC check passed: The contract defines a bounded model-level storage-profile vocabulary with HexString as default and Binary as explicit opt-in, applied consistently to every DVault-owned hash-key and hash-key-reference column in scope. (docs/plans/hash-key-storage-profile-contract.md:17-27 defines the bounded HexString/Binary vocabulary, src/DCoding.Data.DVault/Modeling/DataVaultConventions.cs:55-68 makes HexString the default, and src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:234-253 projects one storage profile across model hash-key and hash-key-reference mappings.).",
    "AC check passed: Storage sizing binds to the active stable-hash algorithm\u0027s fixed digest byte length for the whole model and explicitly covers \u0060sha256-v1\u0060, \u0060sha1-v1\u0060, \u0060sha256-128-v1\u0060, and \u0060sha256-160-v1\u0060. (docs/plans/hash-key-storage-profile-contract.md:29-45 defines the four built-in sizing baselines, src/DCoding.Data.DVault/DataVaultOptions.cs:68-75 derives model digest length from the selected stable-hash service, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs:408-457 covers sha256-v1, sha1-v1, sha256-128-v1, and sha256-160-v1 across the six visible profiles.).",
    "AC check passed: Provider capability profiles and translated EF metadata expose storage profile, provider store type, logical property kind, CLR projection or conversion behavior, declared digest length, and active \u0060algorithmId\u0060 for all six visible built-in provider profiles. (src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs:158-249 and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:939-949 carry storage profile, store type, logical kind, conversion behavior, digest length, and algorithmId into provider mappings and EF annotations, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs:408-425 and :1263-1281 verify those facts on translated properties.).",
    "AC check passed: Explain and support-bundle diagnostics expose \u0060algorithmId\u0060, \u0060digestByteLength\u0060, \u0060digestEncoding\u0060, and selected hash-key storage facts without raw hash values, and the reviewed support-bundle artifact is the authoritative preflight baseline for algorithm or storage drift checks. (docs/plans/hash-key-storage-profile-contract.md:47-61 names the reviewed support bundle as the authoritative preflight baseline, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:59-130 and :133-159 verify algorithmId/digestByteLength/digestEncoding exposure, redaction of raw values, and same-width sha1-v1 versus sha256-160-v1 distinction in exported support-bundle JSON.).",
    "AC check passed: Migration and preflight guardrails fail closed when a DVault-owned hash-key or hash-key-reference column changes storage profile, stable-hash \u0060algorithmId\u0060, digest length, provider store type, or equivalent persisted shape without an intentional contract change; specifically, \u0060sha1-v1\u0060 to \u0060sha256-160-v1\u0060 must be rejected even though both are 20-byte / 40-hex digests. (src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs:563-582 compares hash-key storage profile, stable-hash algorithm id, digest byte length, and digest encoding during drift analysis, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs:565-603 proves same-width sha1-v1 to sha256-160-v1 drift is rejected while the existing DVM2003 shape-mismatch cases at :94-152 and :233-307 keep incompatible generated-column changes fail-closed.).",
    "DoD check passed: Provider capability profile tests cover the six visible built-in profiles for default HexString storage and digest-length sizing. (tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs:408-457 covers all six visible built-in profiles for default HexString storage and digest-length sizing.).",
    "DoD check passed: EF translation tests prove DVault-owned hash-key and hash-key-reference properties carry authoritative storage annotations, \u0060algorithmId\u0060, and diagnostics facts required by the contract. (tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs:353-387, :408-425, and :1263-1281 verify authoritative hash-key and participant-reference storage annotations, algorithmId, digest length, and related diagnostics facts on translated EF properties.).",
    "DoD check passed: Diagnostics and support-bundle tests cover \u0060algorithmId\u0060 plus \u0060digestByteLength\u0060 exposure, verify that no raw hash values are emitted, and prove the reviewed support-bundle preflight baseline distinguishes \u0060sha1-v1\u0060 from \u0060sha256-160-v1\u0060 when width and store type are unchanged. (tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:59-130 and :133-159 cover algorithmId and digestByteLength exposure, confirm no raw hash values are emitted, and prove the support-bundle baseline distinguishes sha1-v1 from sha256-160-v1 even when width and store type match.).",
    "DoD check passed: Final contract documentation is published on an approved planning or equivalent authoritative handoff surface and aligned with the v0.35.0 stable-hash guidance baseline. (docs/plans/hash-key-storage-profile-contract.md exists as the published planning contract, docs/plans/README.md:5-17 indexes it under current contracts, and docs/production-adoption-checklist.md:100 ties it back to the v0.35.0 stable-hash guidance baseline.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "DoD check failed: Migration or preflight guardrail tests cover unsupported HexString-to-Binary transitions, digest-length mismatches, same-length \u0060algorithmId\u0060 drift, and provider-shape mismatches for DVault-owned hash-key columns. (I confirmed same-width algorithm-drift coverage at tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs:565-603 and generic DVM2003 shape-mismatch coverage, but I did not find direct migration/preflight guardrail tests for HexString-to-Binary rejection or digest-byte-length mismatch rejection; rg -n \u0022HashKeyStorageProfile\\.Binary|LowercaseHexBinary\u0022 against DataVaultMigrationOperationDiagnosticsTests.cs, DataVaultPreflightTests.cs, and DataVaultIdempotencyPreflightTests.cs returned no matches.).",
    "Definition of Done 4 remains unconfirmed. The explicit migration-guardrail addition on this branch is the same-width algorithmId drift case in tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs:565-603; I did not find direct migration/preflight guardrail tests that exercise HexString-to-Binary rejection or digest-byte-length mismatch rejection for DVault-owned hash-key columns.",
    "The current rework pass did not address that gap in repository content: git diff --name-only 1edbf49475ad..41235d339d6d -- docs src tests returned no changed paths after the prior tester handoff commit."
  ],
  "evidence": [
    "git diff --name-status develop...41235d339d6d shows a new docs/plans/hash-key-storage-profile-contract.md, documentation updates, source changes under src/DCoding.Data.DVault, and updated tests in provider capability, EF metadata, diagnostics, migration guardrails, SQLite integration, and the public API snapshot.",
    "docs/plans/hash-key-storage-profile-contract.md:9-27, :29-61, and :65-78 defines the logical lowercase-hex boundary, HexString default/Binary opt-in vocabulary, four built-in digest-size baselines, support-bundle facts, and fail-closed drift posture.",
    "src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs:158-249 projects storage profile, store type, value format, algorithmId, digest length, and conversion behavior for HashKey and ParticipantReference mappings; src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:939-949 writes those facts onto EF property annotations.",
    "tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs:408-483 covers all six visible provider profiles for HexString sizing and Binary opt-in, and tests/DCoding.Data.DVault.Tests/Integration/SqliteProviderCapabilityProfileTests.cs:10-36 verifies SQLite still stores canonical lowercase-hex hash keys as raw TEXT.",
    "tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:59-159 verifies explain/support-bundle algorithmId, digestByteLength, digestEncoding, raw-value redaction, and the sha1-v1 versus sha256-160-v1 same-width distinction.",
    "tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs:565-603 explicitly rejects same-width stable-hash algorithm drift, while src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs:563-582 contains the storage-profile and digest-length comparison logic.",
    "git diff --name-only 1edbf49475ad..41235d339d6d -- docs src tests returned no paths, so the post-return rework pass did not add repository docs/source/test changes.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/architecture, area/ef-core, area/hashing, area/schema, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 7 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract\u0027.",
    "Ticket history references implementation commit \u002741235d339d6d\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 1 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "nextSteps": [
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Add migration or preflight guardrail tests that explicitly fail on DVault-owned hash-key HexString-to-Binary transitions and digest-byte-length mismatches, alongside the existing same-width algorithmId drift coverage.",
    "After those tests are on the branch, rerun tester verification and include the policy build/test/format evidence through the supported verification path."
  ],
  "branchName": "ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract",
  "commitSha": "41235d339d6d"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F9GF5FV54DGWY9GA8ZEZWM5R`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract`