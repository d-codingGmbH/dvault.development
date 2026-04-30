[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket is well bounded around the stable-hashing contract, but it assumes a default stable hash service and normalization surface that is not present in the current source tree and is not explicitly authorized as implementation scope for this test task.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- gicket-read-ticket-comments returned totalComments=10 and returnedComments=10; observed comments are gicket-bot claim/refinement/handover/lease/relation automation entries, with no human comment requirements.
- docs/plans/stable-hashing-contract.md was read directly and defines IStableHashService.AlgorithmId, IStableHashService.ComputeHash(string normalizedInput), StableHashDigest.AlgorithmId, StableHashDigest.Value, sha256-v1, SHA-256, UTF-8 without BOM, lowercase 64-character hex, null input ArgumentNullException, and published vectors.
- rg -n "IStableHashService|StableHashDigest|StableHash|sha256-v1|ComputeHash" . found hash-service public type names only in docs/plans/stable-hashing-contract.md; source hits were limited to src/DCoding.Data.DVault/Modeling/DataVaultConventions.cs exposing StableHashAlgorithmId and tests/DCoding.Data.DVault.Tests/Modeling/DefaultNamingPolicyTests.cs asserting sha256-v1.
- src/DCoding.Data.DVault/Modeling/DataVaultConventions.cs exposes StableHashAlgorithmId as a string reserved for the default hash service boundary, but no IStableHashService, StableHashDigest, ComputeHash implementation, or normalizer API was observed in source.
- tests/DCoding.Data.DVault.Tests/Unit contains ConventionFirstEntryPointCoverageTests.cs, DataVaultMetadataTests.cs, DCoding.Data.DVault.Tests.Unit.csproj, and TestDiscoverySmokeTests.cs; no stable hashing unit tests are present yet.
- DVault.slnx includes tests/DCoding.Data.DVault.Tests/Unit, Integration, and Shared projects, matching the delivery contract's intended test layout.
- git show --stat --oneline --name-only 0a1be87386a3 showed the PO handoff commit only changed .gicket ticket/comment/event files for 06EXB76NNRDP7WH1F2R5VYYPMR, not source or test implementation files.

Blocking findings
- The delivery contract asks for unit tests for the default stable hash service and model normalization behavior, but direct source evidence does not show the required public service, digest type, ComputeHash member, or normalizer API. The ticket does not state whether dev may introduce that production surface as part of this test task, so developer handoff would require inference beyond ticket-level scope.

Required PO actions
- Clarify whether this is test-only against an existing public implementation or a combined implementation-and-test handoff that may introduce the default stable hash service and normalization boundary.
- If test-only, add an explicit prerequisite/blocking relation to the implementation ticket that introduces the stable hash service/normalizer API, and keep this ticket from dev handoff until that prerequisite is ready or complete.
- If implementation is intended here, update the ticket contract to make that production scope explicit at ticket level, including the public boundary developers should target or the accepted equivalence to the documented IStableHashService/StableHashDigest shape.
- Resolve how this task relates to parent story 06EXB765S2X2MR2K18ZBV8RC38 while that story still has needs-po, so dev is not handed a child ticket with an unrefined parent dependency.

Open issues ledger
- critic-item-1 [required-po-action] Clarify whether this is test-only against an existing public implementation or a combined implementation-and-test handoff that may introduce the default stable hash service and normalization boundary.
- critic-item-2 [required-po-action] If test-only, add an explicit prerequisite/blocking relation to the implementation ticket that introduces the stable hash service/normalizer API, and keep this ticket from dev handoff until that prerequisite is ready or complete.
- critic-item-3 [required-po-action] If implementation is intended here, update the ticket contract to make that production scope explicit at ticket level, including the public boundary developers should target or the accepted equivalence to the documented IStableHashService/StableHashDigest shape.
- critic-item-4 [required-po-action] Resolve how this task relates to parent story 06EXB765S2X2MR2K18ZBV8RC38 while that story still has needs-po, so dev is not handed a child ticket with an unrefined parent dependency.
- critic-item-5 [blocking-finding] The delivery contract asks for unit tests for the default stable hash service and model normalization behavior, but direct source evidence does not show the required public service, digest type, ComputeHash member, or normalizer API. The ticket does not state whether dev may introduce that production surface as part of this test task, so developer handoff would require inference beyond ticket-level scope.

Missing examples / edge cases
- Guid scalar encoding is named in Clarifications as part of the v1 baseline, but the Scope In and AC do not explicitly require a guid normalization test or vector; add it or state that guid coverage is intentionally deferred.
- Invalid supported values are required to fail before hashing, but the ticket does not identify concrete invalid supported-value examples for the current scalar set. The stable-hashing contract mentions non-finite floating point as an example, while the scalar list does not otherwise define float/double as supported scalar encodings.
- Line-ending and NFC normalization tests should include expected canonical strings and byte-count-sensitive string encodings so developers do not infer behavior from digest values alone.

Risky assumptions
- Assumes an implementation-facing stable hash API exists even though the current source tree only exposes a StableHashAlgorithmId convention value.
- Assumes model normalization belongs in this ticket even though docs/plans/stable-hashing-contract.md says the hash service consumes already-normalized text and model-specific code is responsible for canonicalization.
- Assumes binary coverage means UTF-8/no-BOM string byte materialization, not byte array, stream, or base64 scalar normalization; the contract says that, but the ticket title can still mislead without the clarification being preserved.

AC / test suggestions
- Add an AC stating the exact implementation surface the tests should bind to, or state that the tests may introduce only failing characterization tests until the service implementation ticket lands.
- Keep the exact sha256-v1 vectors from docs/plans/stable-hashing-contract.md as required assertions for AlgorithmId and digest value.
- Require ordering tests to use at least two differently ordered source inputs and assert both the canonical text and resulting digest.
- Require culture tests to restore CurrentCulture and CurrentUICulture after execution.

Implementation watchouts
- Do not create standalone binary scalar behavior under this ticket; binary means UTF-8 without BOM materialization of normalized strings and lowercase hex digest text.
- Avoid serializer-based structured-value tests; the contract requires deliberate field mapping and ordinal field-path sorting.
- Ensure null service input and the null stable scalar value n: remain distinct from empty service input and empty string stable value s:0:.

Non-blocking notes
- The persisted contract has ## Open Questions: none, so the open-question gate alone does not block approval.
- The repository has the intended Unit, Integration, and Shared test project layout and DVault.slnx includes those projects.
- The contract's scope-out language correctly excludes password hashing, MACs, signatures, key management, persisted schema work, and standalone binary scalar encoding.

Split recommendations
- Split or explicitly sequence the missing stable hash service/normalizer implementation before this test-focused task, unless PO chooses to re-scope this ticket as implementation plus tests.
- Keep first-class byte array/stream/base64 binary scalar normalization as a separate follow-up contract ticket if product wants it later.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment