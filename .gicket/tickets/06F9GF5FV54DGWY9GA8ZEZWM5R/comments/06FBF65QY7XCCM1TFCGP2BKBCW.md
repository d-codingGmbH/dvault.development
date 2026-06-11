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
    "Selected verification source branch \u0027ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract\u0027 and commit \u002712b989cfb189\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract\u0027 from source \u002712b989cfb189\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract\u0027.",
    "Evidence: \u0060git diff --name-status develop...12b989cfb189\u0060 shows a new \u0060docs/plans/hash-key-storage-profile-contract.md\u0060, documentation updates, source changes in translator/diagnostics/guardrails/model conventions, and updated tests in provider capability, metadata translation, diagnostics, migration guardrails, and SQLite integration.",
    "Evidence: \u0060docs/plans/hash-key-storage-profile-contract.md\u0060 defines the two-profile vocabulary (\u0060HexString\u0060, \u0060Binary\u0060), fixed sizing for \u0060sha256-v1\u0060, \u0060sha1-v1\u0060, \u0060sha256-128-v1\u0060, and \u0060sha256-160-v1\u0060, and names the reviewed support bundle as the authoritative preflight baseline.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs:408-483\u0060 covers six built-in profiles for HexString sizing and explicit Binary opt-in mapping behavior.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs:409-425\u0060 verifies algorithm-sized hash-key store types/annotations, and \u0060:1263-1281\u0060 verifies hash-key/hash-key-reference storage annotations are present only where expected.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:58-159\u0060 verifies diagnostics/support-bundle exposure of \u0060algorithmId\u0060, \u0060digestByteLength\u0060, and redaction, including the \u0060sha1-v1\u0060 versus \u0060sha256-160-v1\u0060 same-width distinction.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs:565-603\u0060 verifies same-width stable-hash algorithm drift is blocked for both a hub hash key and a link participant reference.",
    "Evidence: \u0060rg -n \u0022HashKeyStorageProfile\\.Binary|StableHashDigestByteLength, .*column\\.DigestByteLength|digest length|Binary\u0022 tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0060 returned only helper-level digest annotation wiring and no direct Binary-transition or digest-length-mismatch guardrail test case.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/architecture, area/ef-core, area/hashing, area/schema, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract\u0027.",
    "Evidence: Ticket history references implementation commit \u002712b989cfb189\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: Logical hash-key values remain canonical lowercase hexadecimal strings at API, request, metadata, and diagnostics boundaries regardless of HexString or Binary physical storage. (\u0060docs/plans/hash-key-storage-profile-contract.md\u0060 fixes the logical boundary to canonical lowercase hex, and the binary mapping in \u0060DataVaultProviderCapabilityProfile.cs\u0060 plus the converter in \u0060DataVaultEfMetadataTranslator.cs\u0060 keep the EF/model boundary string-based.).",
    "AC check passed: The contract defines a bounded model-level storage-profile vocabulary with HexString as default and Binary as explicit opt-in, applied consistently to every DVault-owned hash-key and hash-key-reference column in scope. (\u0060DataVaultHashKeyStorageProfile.cs\u0060 defines the bounded \u0060HexString\u0060/\u0060Binary\u0060 vocabulary, and \u0060DataVaultProviderCapabilityProfile.cs\u0060, \u0060DataVaultModelBuilderExtensions.cs\u0060, and \u0060DataVaultEfMetadataTranslator.cs\u0060 apply it to both \u0060HashKey\u0060 and \u0060ParticipantReference\u0060 mappings.).",
    "AC check passed: Storage sizing binds to the active stable-hash algorithm\u0027s fixed digest byte length for the whole model and explicitly covers \u0060sha256-v1\u0060, \u0060sha1-v1\u0060, \u0060sha256-128-v1\u0060, and \u0060sha256-160-v1\u0060. (\u0060DataVaultConventions.cs\u0060 and \u0060DataVaultProviderCapabilityProfile.cs\u0060 bind one model-wide stable-hash algorithm and digest byte length to hash-key sizing, and \u0060DataVaultProviderCapabilityProfileTests.cs\u0060 covers \u0060sha256-v1\u0060, \u0060sha1-v1\u0060, \u0060sha256-128-v1\u0060, and \u0060sha256-160-v1\u0060.).",
    "AC check passed: Provider capability profiles and translated EF metadata expose storage profile, provider store type, logical property kind, CLR projection or conversion behavior, declared digest length, and active \u0060algorithmId\u0060 for all six visible built-in provider profiles. (\u0060DataVaultProviderTypeMapping.cs\u0060, \u0060DataVaultPropertyExplain.cs\u0060, \u0060DefaultDataVaultDiagnosticsService.cs\u0060, and \u0060DataVaultEfMetadataTranslator.cs\u0060 expose storage profile, store type, logical kind, conversion behavior, digest length, and \u0060algorithmId\u0060; \u0060DataVaultEfMetadataTranslationTests.cs\u0060 asserts the EF annotations.).",
    "AC check passed: Explain and support-bundle diagnostics expose \u0060algorithmId\u0060, \u0060digestByteLength\u0060, \u0060digestEncoding\u0060, and selected hash-key storage facts without raw hash values, and the reviewed support-bundle artifact is the authoritative preflight baseline for algorithm or storage drift checks. (\u0060DefaultDataVaultDiagnosticsService.cs\u0060, \u0060DataVaultProviderTypeMappingExplain.cs\u0060, and \u0060DataVaultDiagnosticsResult.cs\u0060 surface \u0060algorithmId\u0060, \u0060digestByteLength\u0060, \u0060digestEncoding\u0060, and hash-key storage facts without raw hashes, and \u0060DataVaultDiagnosticsTests.cs\u0060 verifies redacted support-bundle output plus the \u0060sha1-v1\u0060 versus \u0060sha256-160-v1\u0060 distinction.).",
    "AC check passed: Migration and preflight guardrails fail closed when a DVault-owned hash-key or hash-key-reference column changes storage profile, stable-hash \u0060algorithmId\u0060, digest length, provider store type, or equivalent persisted shape without an intentional contract change; specifically, \u0060sha1-v1\u0060 to \u0060sha256-160-v1\u0060 must be rejected even though both are 20-byte / 40-hex digests. (\u0060DataVaultMigrationOperationDiagnostics.cs\u0060 now treats hash-key storage profile, stable-hash \u0060algorithmId\u0060, digest byte length, digest encoding, conversion behavior, and store type as compatibility facts, and \u0060DataVaultMigrationOperationDiagnosticsTests.cs\u0060 verifies same-width \u0060sha1-v1\u0060 to \u0060sha256-160-v1\u0060 drift is rejected.).",
    "DoD check passed: Provider capability profile tests cover the six visible built-in profiles for default HexString storage and digest-length sizing. (\u0060DataVaultProviderCapabilityProfileTests.cs\u0060 covers the six visible built-in profiles, default HexString sizing, the four built-in stable-hash ids, and explicit Binary opt-in mapping behavior.).",
    "DoD check passed: EF translation tests prove DVault-owned hash-key and hash-key-reference properties carry authoritative storage annotations, \u0060algorithmId\u0060, and diagnostics facts required by the contract. (\u0060DataVaultEfMetadataTranslationTests.cs\u0060 proves DVault-owned hash-key and hash-key-reference properties carry storage-profile, \u0060algorithmId\u0060, digest-length, digest-encoding, and conversion-behavior annotations.).",
    "DoD check passed: Diagnostics and support-bundle tests cover \u0060algorithmId\u0060 plus \u0060digestByteLength\u0060 exposure, verify that no raw hash values are emitted, and prove the reviewed support-bundle preflight baseline distinguishes \u0060sha1-v1\u0060 from \u0060sha256-160-v1\u0060 when width and store type are unchanged. (\u0060DataVaultDiagnosticsTests.cs\u0060 covers diagnostics/support-bundle exposure of \u0060algorithmId\u0060 and \u0060digestByteLength\u0060, checks that raw hash values are not emitted, and proves the support-bundle baseline distinguishes \u0060sha1-v1\u0060 from \u0060sha256-160-v1\u0060.).",
    "DoD check passed: Final contract documentation is published on an approved planning or equivalent authoritative handoff surface and aligned with the v0.35.0 stable-hash guidance baseline. (\u0060docs/plans/hash-key-storage-profile-contract.md\u0060 was added as the durable contract, \u0060docs/plans/README.md\u0060 indexes it, and \u0060docs/production-adoption-checklist.md\u0060 links it from the stable-hash adoption guidance.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "DoD check failed: Migration or preflight guardrail tests cover unsupported HexString-to-Binary transitions, digest-length mismatches, same-length \u0060algorithmId\u0060 drift, and provider-shape mismatches for DVault-owned hash-key columns. (\u0060DataVaultMigrationOperationDiagnosticsTests.cs\u0060 adds same-width algorithm-drift coverage, but this review found no direct guardrail test for a hash-key \u0060HexString\u0060 to \u0060Binary\u0060 transition or a hash-key digest-byte-length mismatch, so the full required guardrail matrix is not yet covered.).",
    "Definition of Done 4 is not met: the migration/preflight guardrail tests cover same-width \u0060algorithmId\u0060 drift, but they do not directly cover hash-key \u0060HexString\u0060 to \u0060Binary\u0060 transitions or hash-key digest-byte-length mismatches."
  ],
  "evidence": [
    "\u0060git diff --name-status develop...12b989cfb189\u0060 shows a new \u0060docs/plans/hash-key-storage-profile-contract.md\u0060, documentation updates, source changes in translator/diagnostics/guardrails/model conventions, and updated tests in provider capability, metadata translation, diagnostics, migration guardrails, and SQLite integration.",
    "\u0060docs/plans/hash-key-storage-profile-contract.md\u0060 defines the two-profile vocabulary (\u0060HexString\u0060, \u0060Binary\u0060), fixed sizing for \u0060sha256-v1\u0060, \u0060sha1-v1\u0060, \u0060sha256-128-v1\u0060, and \u0060sha256-160-v1\u0060, and names the reviewed support bundle as the authoritative preflight baseline.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs:408-483\u0060 covers six built-in profiles for HexString sizing and explicit Binary opt-in mapping behavior.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs:409-425\u0060 verifies algorithm-sized hash-key store types/annotations, and \u0060:1263-1281\u0060 verifies hash-key/hash-key-reference storage annotations are present only where expected.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:58-159\u0060 verifies diagnostics/support-bundle exposure of \u0060algorithmId\u0060, \u0060digestByteLength\u0060, and redaction, including the \u0060sha1-v1\u0060 versus \u0060sha256-160-v1\u0060 same-width distinction.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs:565-603\u0060 verifies same-width stable-hash algorithm drift is blocked for both a hub hash key and a link participant reference.",
    "\u0060rg -n \u0022HashKeyStorageProfile\\.Binary|StableHashDigestByteLength, .*column\\.DigestByteLength|digest length|Binary\u0022 tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0060 returned only helper-level digest annotation wiring and no direct Binary-transition or digest-length-mismatch guardrail test case.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/architecture, area/ef-core, area/hashing, area/schema, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract\u0027.",
    "Ticket history references implementation commit \u002712b989cfb189\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Add explicit migration or preflight guardrail tests for hash-key \u0060HexString\u0060 to \u0060Binary\u0060 transitions and digest-byte-length mismatches in \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0060.",
    "After the missing guardrail cases are added, rerun \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060 in the supported environment."
  ],
  "branchName": "ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract",
  "commitSha": "12b989cfb189"
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