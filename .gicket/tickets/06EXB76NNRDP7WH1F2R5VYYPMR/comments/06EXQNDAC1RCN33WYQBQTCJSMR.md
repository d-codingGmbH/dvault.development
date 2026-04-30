[gicket-bot] PO refinement contract

Summary
- Refined the ticket as a combined implementation-and-test handoff for the stable hash service and canonical normalizer, using docs/plans/stable-hashing-contract.md as the governing contract. No child tickets, relations, attachments, or planning documents were created in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - This is not test-only. Because repository evidence does not show an existing public IStableHashService, StableHashDigest, ComputeHash member, or model normalizer API, this ticket is now a combined implementation-and-test handoff that may introduce the default stable hash service and canonical normalization boundary required by docs/plans/stable-hashing-contract.md.
- critic-item-2: `answered` - The test-only branch is not selected, so no prerequisite implementation ticket or blocking relation is required. This ticket itself authorizes the bounded production implementation and its tests. The existing incoming parentOf relation from 06EXB765S2X2MR2K18ZBV8RC38 and outgoing blocks relation to 06EXB80FPE3REH11RQ1YR6BW1G remain the observed relation context.
- critic-item-3: `answered` - Production scope is explicit: developers may introduce a public/default stable hash service boundary equivalent to IStableHashService with AlgorithmId and ComputeHash(string normalizedInput), a digest value equivalent to StableHashDigest with AlgorithmId and Value, default sha256-v1 behavior, and a DVault-owned production canonical normalization boundary for supported v1 scalar and structured rules. AddDVault or the established DI/options registration point should expose the default service without requiring custom configuration.
- critic-item-4: `answered` - The parent story 06EXB765S2X2MR2K18ZBV8RC38 remains the broad hash key/hash diff story and still needs separate PO refinement, but this child is independently bounded by docs/plans/stable-hashing-contract.md and this refined contract. Dev should not infer full parent-story behavior from the parent relation; the parentOf relation is ancestry for this handoff, not an additional blocking dependency. Full entity hash key/hash diff services remain outside this ticket.
- critic-item-5: `answered` - The source-surface gap is acknowledged and resolved by making production surface creation in scope. Tests are not expected to target nonexistent APIs; the implementation must add the service, digest, and normalizer boundary first or alongside the tests, with names equivalent to the documented IStableHashService and StableHashDigest shape and behavior equivalent to the stable hashing contract.

Clarifications
- This ticket is a combined implementation-and-test handoff, not a test-only task against an existing implementation.
- The normative source is docs/plans/stable-hashing-contract.md: sha256-v1, SHA-256, UTF-8 input bytes without BOM, lowercase 64-character hex digest, deterministic AlgorithmId propagation, and the listed canonical normalization rules.
- The hash service consumes already normalized text; the production normalizer boundary produces canonical text for supported scalar values and structured field mappings before hashing.
- Binary coverage for this ticket means UTF-8 byte materialization of normalized .NET strings without BOM and lowercase hexadecimal SHA-256 digest output; byte array, stream, or base64 scalar normalization is not approved here.
- The parent story 06EXB765S2X2MR2K18ZBV8RC38 remains broad and needs separate PO refinement, but this child is independently bounded and should not require dev inference from the parent story.
- No bounded child tickets, relation writes, attachments, or planning documents were created during this pass.

Scope In
- Introduce the default stable hash service production surface in src/DCoding.Data.DVault, equivalent to IStableHashService with AlgorithmId and ComputeHash(string normalizedInput).
- Introduce the digest production value shape equivalent to StableHashDigest with AlgorithmId copied from the producing service and Value as lowercase hexadecimal digest text.
- Implement default sha256-v1 behavior using SHA-256 over UTF-8 bytes of normalizedInput without BOM; empty normalized input is valid and null input throws ArgumentNullException.
- Introduce a DVault-owned production canonical normalization boundary for null, string, boolean, integer, decimal, timestamp, guid, and structured field values as defined by the stable hashing contract.
- Register or expose the default service through the existing convention-first startup path or established DI/options pattern so consumers do not have to construct the implementation directly.
- Add focused unit tests under tests/DCoding.Data.DVault.Tests/Unit for published vectors, null versus empty behavior, deterministic repeated hashing, UTF-8/no-BOM materialization, AlgorithmId propagation, and lowercase digest shape.
- Add normalization tests for NFC string normalization, CRLF/CR to LF conversion, invariant decimal and timestamp formatting, null field inclusion, ordinal field-path sorting, and source-order independence.
- Add failure tests showing unsupported value types and invalid supported values fail before a digest is produced.
- Add culture-sensitivity tests that change CurrentCulture and CurrentUICulture and verify number, decimal, timestamp, normalized text, and digest results remain stable.

Scope Out
- Full Data Vault hash key and hash diff entity services for hubs, links, satellites, or model-specific participating-field decisions.
- Persisted storage schema, migrations, provider-specific physical types, compatibility migrations, or database integration for stored hashes.
- A standalone binary scalar encoding for byte arrays, streams, or base64 canonicalization.
- Password hashing, encryption, MACs, signatures, key management, salts, secret rotation, or other security-specific hashing behavior.
- Advanced hash replacement UI or broad configuration design beyond the minimal DI/options registration behavior needed for the default service boundary.
- Renaming or restructuring the existing source and test roots outside the established src/DCoding.Data.DVault and tests/DCoding.Data.DVault.Tests layout.

Open questions
- none

Follow-up questions
- Parent story 06EXB765S2X2MR2K18ZBV8RC38 still needs separate PO refinement for the broader hash key/hash diff service roadmap beyond this bounded stable hash service and normalizer slice.
- Should a later contract ticket add first-class binary scalar normalization for byte arrays or streams? Current evidence does not define it, so it remains outside this ticket.
- When persisted hashes are introduced, confirm where AlgorithmId and canonicalization version are stored for compatibility and migration planning.
- Future entity-specific hashing tickets should identify participating fields and add vectors for those model-specific canonical inputs.
- Advanced alternate hash configuration beyond basic DI/options replacement should be refined with the optional advanced configuration hook work if product needs it.

Risks
- The scope is broader than the prior test-only wording; developers must keep production changes limited to the stable-hashing contract and avoid implementing the full parent hash key/hash diff story here.
- Public API additions can fail build policy if XML documentation is missing because the library treats CS1591 as an error.
- Culture tests can leak process-global state if CurrentCulture and CurrentUICulture are not restored.
- Timestamp and decimal normalization can drift if implementation accidentally uses current culture, local time, serializer defaults, or platform-default encoding.

Split recommendations
- No child split or prerequisite relation is recommended now because this ticket is the bounded implementation-and-test slice for the default stable hash service and canonical normalizer.
- Split full Data Vault hash key/hash diff entity services, persistence integration, and participating-field selection under the parent story or later follow-up tickets.
- Create a separate binary scalar canonicalization ticket only if product approves byte array, stream, or base64 normalization beyond UTF-8 materialization of normalized strings.

Persisted contract coverage
- acceptance-criteria items: 9
- definition-of-done items: 6
- implementation-notes items: 8

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment