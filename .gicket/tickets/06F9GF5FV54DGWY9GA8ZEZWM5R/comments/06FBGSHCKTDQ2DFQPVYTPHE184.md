[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 6/6 acceptance criteria and 5/5 definition-of-done expectations on branch \u0027ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract\u0027 at commit \u00272575cbbb0ef3\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract",
    "commitSha": "2575cbbb0ef3",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "Logical hash-key values remain canonical lowercase hexadecimal strings at API, request, metadata, and diagnostics boundaries regardless of HexString or Binary physical storage.",
      "satisfied": true,
      "reason": "\u0060docs/plans/hash-key-storage-profile-contract.md\u0060 fixes the logical boundary at canonical lowercase-hex strings, \u0060DataVaultProviderCapabilityProfile.WithHashKeyStorageProfile(...)\u0060 keeps hash-key model CLR type as \u0060string\u0060 for both \u0060HexString\u0060 and \u0060Binary\u0060, \u0060DataVaultEfMetadataTranslator\u0060 applies the lowercase-hex-to-bytes converter only at persistence time, and \u0060SqliteProviderCapabilityProfileTests\u0060 confirms lowercase-hex text remains the observed boundary value."
    },
    {
      "expectation": "The contract defines a bounded model-level storage-profile vocabulary with HexString as default and Binary as explicit opt-in, applied consistently to every DVault-owned hash-key and hash-key-reference column in scope.",
      "satisfied": true,
      "reason": "The contract and \u0060DataVaultHashKeyStorageProfile\u0060 define the bounded \u0060HexString\u0060/\u0060Binary\u0060 vocabulary, \u0060DataVaultProviderCapabilityProfiles\u0060 default built-in mappings to \u0060HexString\u0060, and \u0060DataVaultModelBuilderExtensions.UseDataVaultCore(...)\u0060 plus the EF translator project the selected model-level storage profile onto hash-key and hash-key-reference columns."
    },
    {
      "expectation": "Storage sizing binds to the active stable-hash algorithm\u0027s fixed digest byte length for the whole model and explicitly covers \u0060sha256-v1\u0060, \u0060sha1-v1\u0060, \u0060sha256-128-v1\u0060, and \u0060sha256-160-v1\u0060.",
      "satisfied": true,
      "reason": "\u0060DataVaultOptions.UseStableHashAlgorithm(...)\u0060, \u0060DataVaultConventions\u0060, and \u0060DataVaultModelCacheKeyFactory\u0060 bind model sizing to one active stable-hash algorithm, and \u0060DataVaultProviderCapabilityProfileTests\u0060 explicitly cover \u0060sha256-v1\u0060, \u0060sha1-v1\u0060, \u0060sha256-128-v1\u0060, and \u0060sha256-160-v1\u0060 across the built-in profiles."
    },
    {
      "expectation": "Provider capability profiles and translated EF metadata expose storage profile, provider store type, logical property kind, CLR projection or conversion behavior, declared digest length, and active \u0060algorithmId\u0060 for all six visible built-in provider profiles.",
      "satisfied": true,
      "reason": "\u0060DataVaultAnnotationNames\u0060, \u0060DataVaultEfMetadataTranslator\u0060, \u0060DefaultDataVaultDiagnosticsService\u0060, \u0060DataVaultPropertyExplain\u0060, and \u0060DataVaultProviderTypeMappingExplain\u0060 expose storage profile, store type, logical kind, CLR projection/conversion, digest length, and \u0060algorithmId\u0060; the provider-capability and EF-translation tests verify those facts on the built-in provider set."
    },
    {
      "expectation": "Explain and support-bundle diagnostics expose \u0060algorithmId\u0060, \u0060digestByteLength\u0060, \u0060digestEncoding\u0060, and selected hash-key storage facts without raw hash values, and the reviewed support-bundle artifact is the authoritative preflight baseline for algorithm or storage drift checks.",
      "satisfied": true,
      "reason": "The contract names the reviewed support bundle as the authoritative preflight baseline, and \u0060DataVaultDiagnosticsTests\u0060 verify explain/support-bundle exposure of \u0060algorithmId\u0060, \u0060digestByteLength\u0060, \u0060digestEncoding\u0060, storage facts, same-width \u0060sha1-v1\u0060 versus \u0060sha256-160-v1\u0060 distinction, and redaction of raw business keys and raw digests."
    },
    {
      "expectation": "Migration and preflight guardrails fail closed when a DVault-owned hash-key or hash-key-reference column changes storage profile, stable-hash \u0060algorithmId\u0060, digest length, provider store type, or equivalent persisted shape without an intentional contract change; specifically, \u0060sha1-v1\u0060 to \u0060sha256-160-v1\u0060 must be rejected even though both are 20-byte / 40-hex digests.",
      "satisfied": true,
      "reason": "\u0060DataVaultMigrationOperationDiagnostics\u0060 compares hash-key storage profile, \u0060algorithmId\u0060, digest byte length, digest encoding, conversion behavior, and provider shape facts, while migration tests block generated hash-key \u0060AlterColumn\u0060 drift and explicitly reject same-width \u0060sha1-v1\u0060 to \u0060sha256-160-v1\u0060 drift for both a hash key and a hash-key reference."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "Provider capability profile tests cover the six visible built-in profiles for default HexString storage and digest-length sizing.",
      "satisfied": true,
      "reason": "\u0060DataVaultProviderCapabilityProfileTests\u0060 cover all six visible built-in profiles and assert default \u0060HexString\u0060 sizing for the supported stable-hash digest lengths."
    },
    {
      "expectation": "EF translation tests prove DVault-owned hash-key and hash-key-reference properties carry authoritative storage annotations, \u0060algorithmId\u0060, and diagnostics facts required by the contract.",
      "satisfied": true,
      "reason": "\u0060DataVaultEfMetadataTranslationTests\u0060 verify hash-key and hash-key-reference EF annotations, including storage profile, \u0060algorithmId\u0060, digest byte length, digest encoding, and conversion behavior, and also verify explicit \u0060sha1-v1\u0060 sizing on translated metadata."
    },
    {
      "expectation": "Diagnostics and support-bundle tests cover \u0060algorithmId\u0060 plus \u0060digestByteLength\u0060 exposure, verify that no raw hash values are emitted, and prove the reviewed support-bundle preflight baseline distinguishes \u0060sha1-v1\u0060 from \u0060sha256-160-v1\u0060 when width and store type are unchanged.",
      "satisfied": true,
      "reason": "\u0060DataVaultDiagnosticsTests\u0060 cover explain/support-bundle \u0060algorithmId\u0060 and \u0060digestByteLength\u0060 exposure, assert raw-value redaction, and prove support-bundle output distinguishes \u0060sha1-v1\u0060 from \u0060sha256-160-v1\u0060 even when width and store type match."
    },
    {
      "expectation": "Migration or preflight guardrail tests cover unsupported HexString-to-Binary transitions, digest-length mismatches, same-length \u0060algorithmId\u0060 drift, and provider-shape mismatches for DVault-owned hash-key columns.",
      "satisfied": true,
      "reason": "\u0060DataVaultMigrationOperationDiagnosticsTests\u0060 cover fail-closed hash-key \u0060AlterColumn\u0060 guardrails for provider-shape drift and separately cover same-width \u0060algorithmId\u0060 drift, which is the critical case not detectable from width/store type alone."
    },
    {
      "expectation": "Final contract documentation is published on an approved planning or equivalent authoritative handoff surface and aligned with the v0.35.0 stable-hash guidance baseline.",
      "satisfied": true,
      "reason": "The final contract is published at \u0060docs/plans/hash-key-storage-profile-contract.md\u0060, indexed from \u0060docs/plans/README.md\u0060, and linked from \u0060docs/production-adoption-checklist.md\u0060 alongside the existing stable-hashing baseline guidance."
    }
  ],
  "evidence": [
    "\u0060git diff --name-status develop...2575cbbb0ef3 -- docs src tests .gicket/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/description.md\u0060 shows the contract document, planning index/checklist updates, \u0060src/DCoding.Data.DVault\u0060 hash-key storage and guardrail changes, and the related unit/integration test updates.",
    "\u0060git diff --name-only 2575cbbb0ef3..HEAD -- docs src tests\u0060 returned no paths, and \u0060git diff --name-only 2575cbbb0ef3..HEAD\u0060 listed only \u0060.gicket/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/...\u0060, so the reviewed \u0060docs/src/tests\u0060 implementation still matches commit \u00602575cbbb0ef3\u0060.",
    "\u0060docs/plans/hash-key-storage-profile-contract.md\u0060 defines the \u0060HexString\u0060/\u0060Binary\u0060 storage-profile vocabulary, the four built-in stable-hash sizing baselines, the reviewed support bundle as the authoritative preflight baseline, and fail-closed rejection of \u0060sha1-v1\u0060 to \u0060sha256-160-v1\u0060 drift.",
    "\u0060src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs\u0060, \u0060DataVaultModelBuilderExtensions.cs\u0060, \u0060DataVaultEfMetadataTranslator.cs\u0060, \u0060DefaultDataVaultDiagnosticsService.cs\u0060, and \u0060DataVaultMigrationOperationDiagnostics.cs\u0060 project model-level storage-profile, stable-hash, diagnostics, and guardrail facts into provider mappings, EF metadata, explain output, and migration checks.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs\u0060, \u0060DataVaultEfMetadataTranslationTests.cs\u0060, \u0060DataVaultDiagnosticsTests.cs\u0060, \u0060DataVaultMigrationOperationDiagnosticsTests.cs\u0060, and \u0060Integration/SqliteProviderCapabilityProfileTests.cs\u0060 cover six built-in profiles, EF annotations, support-bundle redaction/baselines, same-width algorithm drift rejection, and lowercase-hex SQLite TEXT persistence.",
    "Persisted ticket evidence in \u0060.gicket/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/description.md\u0060 records \u0060dotnet build DVault.slnx --nologo\u0060 passing, \u0060dotnet test DVault.slnx --nologo\u0060 passing, refreshed \u0060timeout 600s dotnet test DVault.slnx --nologo --no-build\u0060 passing, and \u0060bash tools/check-format.sh\u0060 passing on the ticket branch.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/architecture, area/ef-core, area/hashing, area/schema, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 11 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 4 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 4 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract\u0027.",
    "Ticket history references implementation commit \u00272575cbbb0ef3\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 3 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to \u0060integrator\u0060."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F9GF5FV54DGWY9GA8ZEZWM5R`
- target-role: `integrator`
- verification-summary: Tester verified 6/6 acceptance criteria and 5/5 definition-of-done expectations on branch 'ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract' at commit '2575cbbb0ef3'.
- acceptance-criteria: `6/6` satisfied
- definition-of-done: `5/5` satisfied
- implementation-branch: `ticket/06F9GF5FV54DGWY9GA8ZEZWM5R-story-define-hash-key-storage-profile-contract`
- implementation-commit: `2575cbbb0ef3`
- implementation-pr: `<none>`
- implementation-change: `<none>`