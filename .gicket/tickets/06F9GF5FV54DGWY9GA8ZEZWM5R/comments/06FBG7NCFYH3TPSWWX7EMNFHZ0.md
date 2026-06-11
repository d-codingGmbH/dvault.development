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
    "Selected verification source branch \u0027ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract\u0027 and commit \u00272575cbbb0ef3\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract\u0027 from source \u00272575cbbb0ef3\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract\u0027.",
    "Evidence: \u0060git diff --name-status develop...2575cbbb0ef3 -- docs src tests\u0060 shows the contract document, documentation index/checklist updates, source changes under \u0060src/DCoding.Data.DVault\u0060, and updated unit/integration tests for provider capability, EF metadata, diagnostics, migration guardrails, and SQLite integration.",
    "Evidence: \u0060git diff --name-status 12b989cfb189..2575cbbb0ef3 -- docs src tests .gicket/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/description.md\u0060 changed only \u0060.gicket/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/description.md\u0060.",
    "Evidence: \u0060git diff --name-only 2575cbbb0ef3..HEAD\u0060 lists only \u0060.gicket/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/...\u0060 files, so the current \u0060docs/src/tests\u0060 contents match the handoff commit for the files inspected.",
    "Evidence: \u0060docs/plans/hash-key-storage-profile-contract.md\u0060 defines \u0060HexString\u0060 as default, \u0060Binary\u0060 as explicit opt-in, covers \u0060sha256-v1\u0060, \u0060sha1-v1\u0060, \u0060sha256-128-v1\u0060, and \u0060sha256-160-v1\u0060, and names the reviewed support bundle as the authoritative preflight baseline.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs\u0060, \u0060src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs\u0060, and \u0060src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0060 project and compare storage profile, algorithm id, digest byte length, digest encoding, provider store type, provider value format, and conversion behavior for hash keys and participant references.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Integration/SqliteProviderCapabilityProfileTests.cs\u0060 cover six provider profiles, EF annotations, support-bundle redaction/same-width distinction, and SQLite raw TEXT persistence.",
    "Evidence: A targeted \u0060rg -n\u0060 over \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0060 found the explicit same-width drift test at line 565 and helper annotation lines 1026-1027, but no direct Binary-transition or alternate digest-byte-length guardrail case.",
    "Evidence: \u0060.gicket/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/description.md\u0060 lines 123-125 and 156-158 record passing \u0060dotnet build DVault.slnx --nologo\u0060, \u0060timeout 600s dotnet test DVault.slnx --nologo --no-build\u0060, and \u0060bash tools/check-format.sh\u0060 results on the ticket branch.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/architecture, area/ef-core, area/hashing, area/schema, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 9 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract\u0027.",
    "Evidence: Ticket history references implementation commit \u00272575cbbb0ef3\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Evidence: Ticket history contains 2 structured return-routing contract comment(s).",
    "Evidence: Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths.",
    "AC check passed: Logical hash-key values remain canonical lowercase hexadecimal strings at API, request, metadata, and diagnostics boundaries regardless of HexString or Binary physical storage. (\u0060docs/plans/hash-key-storage-profile-contract.md\u0060 fixes the logical boundary at canonical lowercase hexadecimal strings, and the Binary mapping in \u0060src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs\u0060 plus the converter in \u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs\u0060 keep the EF/model boundary string-based.).",
    "AC check passed: The contract defines a bounded model-level storage-profile vocabulary with HexString as default and Binary as explicit opt-in, applied consistently to every DVault-owned hash-key and hash-key-reference column in scope. (\u0060src/DCoding.Data.DVault/DataVaultHashKeyStorageProfile.cs\u0060, \u0060src/DCoding.Data.DVault/Modeling/DataVaultConventions.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs\u0060, and \u0060src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs\u0060 define a bounded model-level \u0060HexString\u0060/\u0060Binary\u0060 vocabulary with default \u0060HexString\u0060 and project it for both \u0060HashKey\u0060 and \u0060ParticipantReference\u0060 mappings.).",
    "AC check passed: Storage sizing binds to the active stable-hash algorithm\u0027s fixed digest byte length for the whole model and explicitly covers \u0060sha256-v1\u0060, \u0060sha1-v1\u0060, \u0060sha256-128-v1\u0060, and \u0060sha256-160-v1\u0060. (\u0060src/DCoding.Data.DVault/Modeling/DataVaultConventions.cs\u0060 and \u0060src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs\u0060 bind one model-wide stable-hash algorithm and digest byte length to hash-key sizing, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs\u0060 covers \u0060sha256-v1\u0060, \u0060sha1-v1\u0060, \u0060sha256-128-v1\u0060, and \u0060sha256-160-v1\u0060 across the six built-in provider profiles.).",
    "AC check passed: Provider capability profiles and translated EF metadata expose storage profile, provider store type, logical property kind, CLR projection or conversion behavior, declared digest length, and active \u0060algorithmId\u0060 for all six visible built-in provider profiles. (\u0060src/DCoding.Data.DVault/DataVaultProviderTypeMapping.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultPropertyExplain.cs\u0060, and \u0060src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs\u0060 expose storage profile, provider store type, logical property kind, conversion behavior, digest length, and active algorithm id, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs\u0060 verifies the translated EF annotations.).",
    "AC check passed: Explain and support-bundle diagnostics expose \u0060algorithmId\u0060, \u0060digestByteLength\u0060, \u0060digestEncoding\u0060, and selected hash-key storage facts without raw hash values, and the reviewed support-bundle artifact is the authoritative preflight baseline for algorithm or storage drift checks. (\u0060src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultProviderTypeMappingExplain.cs\u0060, and \u0060src/DCoding.Data.DVault/DataVaultDiagnosticsResult.cs\u0060 surface \u0060algorithmId\u0060, \u0060digestByteLength\u0060, \u0060digestEncoding\u0060, and hash-key storage facts without raw hash values, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0060 verifies support-bundle redaction plus the \u0060sha1-v1\u0060 versus \u0060sha256-160-v1\u0060 same-width distinction.).",
    "AC check passed: Migration and preflight guardrails fail closed when a DVault-owned hash-key or hash-key-reference column changes storage profile, stable-hash \u0060algorithmId\u0060, digest length, provider store type, or equivalent persisted shape without an intentional contract change; specifically, \u0060sha1-v1\u0060 to \u0060sha256-160-v1\u0060 must be rejected even though both are 20-byte / 40-hex digests. (\u0060src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0060 treats hash-key storage profile, provider store type, provider value format, stable-hash algorithm id, digest byte length, digest encoding, and conversion behavior as compatibility facts, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0060 explicitly rejects \u0060sha1-v1\u0060 to \u0060sha256-160-v1\u0060 same-width drift for both a hub hash key and a link participant reference.).",
    "DoD check passed: Provider capability profile tests cover the six visible built-in profiles for default HexString storage and digest-length sizing. (\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs\u0060 covers all six visible built-in provider profiles for default HexString storage and digest-length sizing, and \u0060tests/DCoding.Data.DVault.Tests/Integration/SqliteProviderCapabilityProfileTests.cs\u0060 verifies the SQLite raw TEXT persistence behavior.).",
    "DoD check passed: EF translation tests prove DVault-owned hash-key and hash-key-reference properties carry authoritative storage annotations, \u0060algorithmId\u0060, and diagnostics facts required by the contract. (\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs\u0060 proves DVault-owned hash-key and hash-key-reference properties carry storage-profile, algorithm-id, digest-length, digest-encoding, and conversion-behavior annotations.).",
    "DoD check passed: Diagnostics and support-bundle tests cover \u0060algorithmId\u0060 plus \u0060digestByteLength\u0060 exposure, verify that no raw hash values are emitted, and prove the reviewed support-bundle preflight baseline distinguishes \u0060sha1-v1\u0060 from \u0060sha256-160-v1\u0060 when width and store type are unchanged. (\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0060 covers diagnostics/support-bundle exposure of \u0060algorithmId\u0060 and \u0060digestByteLength\u0060, verifies raw hash values are not emitted, and proves the support-bundle baseline distinguishes \u0060sha1-v1\u0060 from \u0060sha256-160-v1\u0060 when width and store type are unchanged.).",
    "DoD check passed: Final contract documentation is published on an approved planning or equivalent authoritative handoff surface and aligned with the v0.35.0 stable-hash guidance baseline. (\u0060docs/plans/hash-key-storage-profile-contract.md\u0060 is published as the durable contract, \u0060docs/plans/README.md\u0060 indexes it, and \u0060docs/production-adoption-checklist.md\u0060 links it from the stable-hash guidance baseline.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "DoD check failed: Migration or preflight guardrail tests cover unsupported HexString-to-Binary transitions, digest-length mismatches, same-length \u0060algorithmId\u0060 drift, and provider-shape mismatches for DVault-owned hash-key columns. (\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0060 directly covers same-width algorithm drift, but this review still found no direct guardrail test for a DVault hash-key \u0060HexString\u0060 to \u0060Binary\u0060 transition or for a hash-key digest-byte-length mismatch. \u0060git diff --name-status 12b989cfb189..2575cbbb0ef3 -- docs src tests .gicket/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/description.md\u0060 changed only the ticket description, so the missing test coverage was not added in the rework handoff commit.).",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0060 still lacks direct guardrail coverage for DVault hash-key \u0060HexString\u0060 to \u0060Binary\u0060 transitions and digest-byte-length mismatches, and the rework handoff commit \u00602575cbbb0ef3\u0060 only refreshed ticket description/validation evidence instead of adding those missing tests."
  ],
  "evidence": [
    "\u0060git diff --name-status develop...2575cbbb0ef3 -- docs src tests\u0060 shows the contract document, documentation index/checklist updates, source changes under \u0060src/DCoding.Data.DVault\u0060, and updated unit/integration tests for provider capability, EF metadata, diagnostics, migration guardrails, and SQLite integration.",
    "\u0060git diff --name-status 12b989cfb189..2575cbbb0ef3 -- docs src tests .gicket/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/description.md\u0060 changed only \u0060.gicket/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/description.md\u0060.",
    "\u0060git diff --name-only 2575cbbb0ef3..HEAD\u0060 lists only \u0060.gicket/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/...\u0060 files, so the current \u0060docs/src/tests\u0060 contents match the handoff commit for the files inspected.",
    "\u0060docs/plans/hash-key-storage-profile-contract.md\u0060 defines \u0060HexString\u0060 as default, \u0060Binary\u0060 as explicit opt-in, covers \u0060sha256-v1\u0060, \u0060sha1-v1\u0060, \u0060sha256-128-v1\u0060, and \u0060sha256-160-v1\u0060, and names the reviewed support bundle as the authoritative preflight baseline.",
    "\u0060src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs\u0060, \u0060src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs\u0060, and \u0060src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs\u0060 project and compare storage profile, algorithm id, digest byte length, digest encoding, provider store type, provider value format, and conversion behavior for hash keys and participant references.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Integration/SqliteProviderCapabilityProfileTests.cs\u0060 cover six provider profiles, EF annotations, support-bundle redaction/same-width distinction, and SQLite raw TEXT persistence.",
    "A targeted \u0060rg -n\u0060 over \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0060 found the explicit same-width drift test at line 565 and helper annotation lines 1026-1027, but no direct Binary-transition or alternate digest-byte-length guardrail case.",
    "\u0060.gicket/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/description.md\u0060 lines 123-125 and 156-158 record passing \u0060dotnet build DVault.slnx --nologo\u0060, \u0060timeout 600s dotnet test DVault.slnx --nologo --no-build\u0060, and \u0060bash tools/check-format.sh\u0060 results on the ticket branch.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/architecture, area/ef-core, area/hashing, area/schema, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 9 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 3 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract\u0027.",
    "Ticket history references implementation commit \u00272575cbbb0ef3\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 2 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "nextSteps": [
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Add explicit migration or preflight guardrail tests for DVault hash-key \u0060HexString\u0060 to \u0060Binary\u0060 transition rejection and digest-byte-length mismatch rejection in \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0060.",
    "Rerun the solution test and formatting validation after those tests are added.",
    "Return the ticket to tester review once Definition of Done 4 has direct repository evidence."
  ],
  "branchName": "ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract",
  "commitSha": "2575cbbb0ef3"
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