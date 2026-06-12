[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F9GF5N4N3Q685XQPKTM5EC00\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F9GF5N4N3Q685XQPKTM5EC00-story-implement-provider-neutral-binary-hash-con\u0027 and commit \u0027be34aaf6e95a\u0027 (ticket-comment branch\u002Bcommit reference; advanced to branch tip after newer repository changes).",
    "Advanced tester verification from stale pinned commit \u0027720f02b3bc8e\u0027 to branch tip \u0027be34aaf6e95a\u0027 because branch \u0027ticket/06F9GF5N4N3Q685XQPKTM5EC00-story-implement-provider-neutral-binary-hash-con\u0027 contains newer committed repository changes after the pinned commit.",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F9GF5N4N3Q685XQPKTM5EC00-story-implement-provider-neutral-binary-hash-con\u0027 from source \u0027be34aaf6e95a\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F9GF5N4N3Q685XQPKTM5EC00-story-implement-provider-neutral-binary-hash-con\u0027.",
    "Evidence: \u0060git rev-parse ticket/06F9GF5N4N3Q685XQPKTM5EC00-story-implement-provider-neutral-binary-hash-con\u0060 resolved the reviewed branch tip to \u0060be34aaf6e95aac5c5d094ef309245a5dc81da1e6\u0060.",
    "Evidence: \u0060git diff --name-only develop...ticket/06F9GF5N4N3Q685XQPKTM5EC00-story-implement-provider-neutral-binary-hash-con | rg -v \u0027^\\.gicket/\u0027\u0060 returned only \u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs\u0060 and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs\u0060.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:968-1106\u0060 wires \u0060LowercaseHexStringToBytesConverter(digestByteLength)\u0060 for \u0060LowercaseHexBinary\u0060 string properties and validates canonical lowercase hex length/content plus provider byte length.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs:429-488\u0060 adds binary-profile metadata wiring, one \u0060sha256-128-v1\u0060 round-trip/null test, invalid-model-value tests, and wrong-provider-length rejection.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs:408-484\u0060 still covers built-in digest-size projection and binary opt-in mapping metadata, but not binary converter round-trips across those built-ins.",
    "Evidence: \u0060rg -n \u0022GetValueComparer|SetValueComparer|ValueComparer\u003C|GetKeyValueComparer|GetProviderValueComparer\u0022 tests/DCoding.Data.DVault.Tests src/DCoding.Data.DVault -g \u0027*.cs\u0027\u0060 returned no matches, so there is no direct repo evidence of binary-profile comparer coverage.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/ef-core, area/hashing, area/modeling, area/schema, automation/bot-ready, type/story, needs-test, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 2 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F9GF5N4N3Q685XQPKTM5EC00-story-implement-provider-neutral-binary-hash-con\u0027.",
    "Evidence: Ticket history references implementation commit \u0027720f02b3bc8e\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "AC check passed: When a model selects HashKeyStorageProfile.Binary, translated EF metadata applies provider-neutral conversion for DVault-owned HashKey and ParticipantReference properties while keeping the model and public value boundary as canonical lowercase hex string. (\u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs\u0060 now applies \u0060LowercaseHexStringToBytesConverter\u0060 only for \u0060LowercaseHexBinary\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs\u0060 asserts binary conversion on both \u0060HashKey\u0060 and \u0060ParticipantReference\u0060 properties while the model CLR type stays \u0060string\u0060.).",
    "AC check passed: The binary conversion path uses the active stable-hash algorithmId and digestByteLength and rejects invalid hex payloads or mismatched digest sizes with deterministic failures. (The converter is created from \u0060typeMapping.DigestByteLength\u0060 and throws deterministic \u0060FormatException\u0060s for wrong hex length, uppercase/non-hex model values, and provider byte arrays with the wrong digest length; the new unit tests cover those failure modes.).",
    "AC check passed: HexString remains the default storage profile and preserves the existing none-string-model behavior without regression. (The non-binary path is unchanged apart from the binary conversion branch, and the existing default \u0060HexString\u0060 annotation assertions in \u0060DataVaultEfMetadataTranslationTests\u0060 still verify the \u0060none-string-model\u0060 behavior.).",
    "DoD check passed: The ticket description remains the authoritative handoff surface and reflects the binary profile contract, scope boundaries, and test expectations. (\u0060.gicket/tickets/06F9GF5N4N3Q685XQPKTM5EC00/description.md\u0060 remains present and still contains the authoritative contract, acceptance criteria, and definition-of-done sections for this story.).",
    "DoD check passed: Implementation is confined to the provider-neutral EF metadata projection and conversion layer and preserves existing storage-profile annotations and metadata facts. (\u0060git diff --name-only develop...ticket/06F9GF5N4N3Q685XQPKTM5EC00-story-implement-provider-neutral-binary-hash-con | rg -v \u0027^\\.gicket/\u0027\u0060 shows only \u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs\u0060 and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs\u0060, keeping the implementation inside the provider-neutral metadata/conversion layer and its unit tests.).",
    "DoD check passed: No provider-specific mapping or broader integration-test work is pulled into this story. (No provider-specific mapping files or broader integration-test files are changed; the observed non-gicket diff stays out of the downstream provider-mapping and integration-test scopes.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: EF comparison and snapshot behavior remains stable for equivalent canonical values, nulls, and change-tracking scenarios under the binary profile. (The branch adds no automated assertion for EF comparer, snapshot, or change-tracking behavior under the binary profile, so stability for equivalent canonical values and null tracking is not directly proven.).",
    "AC check failed: Tests cover round-tripping for the built-in digest sizes plus equality, null handling, and invalid-input cases. (The new tests cover one 16-byte round trip plus null and invalid-input cases, but they do not round-trip all built-in digest sizes or add explicit equality coverage.).",
    "DoD check failed: Automated tests prove binary round-trip, comparer or snapshot semantics, null behavior, and deterministic failure cases. (Automated tests in the branch do not yet prove binary comparer/snapshot semantics or round-tripping across the full built-in digest-size matrix required by the contract.).",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs:453-488\u0060 only exercises a single 16-byte converter instance from \u0060GetBinaryHashKeyConverter()\u0060. The contract requires round-tripping for the built-in digest sizes, so \u0060sha256-v1\u0060, \u0060sha1-v1\u0060, and \u0060sha256-160-v1\u0060 binary conversion cases remain unproven.",
    "No test under \u0060tests/DCoding.Data.DVault.Tests\u0060 directly exercises binary-profile EF comparer, snapshot, or change-tracking behavior; the only binary-profile references found are metadata/converter wiring checks and provider-capability mapping checks."
  ],
  "evidence": [
    "\u0060git rev-parse ticket/06F9GF5N4N3Q685XQPKTM5EC00-story-implement-provider-neutral-binary-hash-con\u0060 resolved the reviewed branch tip to \u0060be34aaf6e95aac5c5d094ef309245a5dc81da1e6\u0060.",
    "\u0060git diff --name-only develop...ticket/06F9GF5N4N3Q685XQPKTM5EC00-story-implement-provider-neutral-binary-hash-con | rg -v \u0027^\\.gicket/\u0027\u0060 returned only \u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs\u0060 and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs\u0060.",
    "\u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:968-1106\u0060 wires \u0060LowercaseHexStringToBytesConverter(digestByteLength)\u0060 for \u0060LowercaseHexBinary\u0060 string properties and validates canonical lowercase hex length/content plus provider byte length.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs:429-488\u0060 adds binary-profile metadata wiring, one \u0060sha256-128-v1\u0060 round-trip/null test, invalid-model-value tests, and wrong-provider-length rejection.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs:408-484\u0060 still covers built-in digest-size projection and binary opt-in mapping metadata, but not binary converter round-trips across those built-ins.",
    "\u0060rg -n \u0022GetValueComparer|SetValueComparer|ValueComparer\u003C|GetKeyValueComparer|GetProviderValueComparer\u0022 tests/DCoding.Data.DVault.Tests src/DCoding.Data.DVault -g \u0027*.cs\u0027\u0060 returned no matches, so there is no direct repo evidence of binary-profile comparer coverage.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/ef-core, area/hashing, area/modeling, area/schema, automation/bot-ready, type/story, needs-test, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 2 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic.",
    "Ticket history references implementation branch \u0027ticket/06F9GF5N4N3Q685XQPKTM5EC00-story-implement-provider-neutral-binary-hash-con\u0027.",
    "Ticket history references implementation commit \u0027720f02b3bc8e\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Extend \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs\u0060 to round-trip binary conversion for each built-in digest size (\u0060sha256-v1\u0060, \u0060sha1-v1\u0060, \u0060sha256-128-v1\u0060, \u0060sha256-160-v1\u0060).",
    "Add direct EF metadata or change-tracking assertions proving binary-profile equality/snapshot behavior for equivalent canonical strings and nulls.",
    "After that coverage is added, rerun the declared verification commands before handing the ticket back to test."
  ],
  "branchName": "ticket/06F9GF5N4N3Q685XQPKTM5EC00-story-implement-provider-neutral-binary-hash-con",
  "commitSha": "be34aaf6e95a"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F9GF5N4N3Q685XQPKTM5EC00`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F9GF5N4N3Q685XQPKTM5EC00-story-implement-provider-neutral-binary-hash-con`