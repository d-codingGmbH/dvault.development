<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket as a combined implementation-and-test handoff for the stable hash service and canonical normalizer, using docs/plans/stable-hashing-contract.md as the governing contract. No child tickets, relations, attachments, or planning documents were created in this pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket is a combined implementation-and-test handoff, not a test-only task against an existing implementation.
- The normative source is docs/plans/stable-hashing-contract.md: sha256-v1, SHA-256, UTF-8 input bytes without BOM, lowercase 64-character hex digest, deterministic AlgorithmId propagation, and the listed canonical normalization rules.
- The hash service consumes already normalized text; the production normalizer boundary produces canonical text for supported scalar values and structured field mappings before hashing.
- Binary coverage for this ticket means UTF-8 byte materialization of normalized .NET strings without BOM and lowercase hexadecimal SHA-256 digest output; byte array, stream, or base64 scalar normalization is not approved here.
- The parent story 06EXB765S2X2MR2K18ZBV8RC38 remains broad and needs separate PO refinement, but this child is independently bounded and should not require dev inference from the parent story.
- No bounded child tickets, relation writes, attachments, or planning documents were created during this pass.

### Scope In
- Introduce the default stable hash service production surface in src/DCoding.Data.DVault, equivalent to IStableHashService with AlgorithmId and ComputeHash(string normalizedInput).
- Introduce the digest production value shape equivalent to StableHashDigest with AlgorithmId copied from the producing service and Value as lowercase hexadecimal digest text.
- Implement default sha256-v1 behavior using SHA-256 over UTF-8 bytes of normalizedInput without BOM; empty normalized input is valid and null input throws ArgumentNullException.
- Introduce a DVault-owned production canonical normalization boundary for null, string, boolean, integer, decimal, timestamp, guid, and structured field values as defined by the stable hashing contract.
- Register or expose the default service through the existing convention-first startup path or established DI/options pattern so consumers do not have to construct the implementation directly.
- Add focused unit tests under tests/DCoding.Data.DVault.Tests/Unit for published vectors, null versus empty behavior, deterministic repeated hashing, UTF-8/no-BOM materialization, AlgorithmId propagation, and lowercase digest shape.
- Add normalization tests for NFC string normalization, CRLF/CR to LF conversion, invariant decimal and timestamp formatting, null field inclusion, ordinal field-path sorting, and source-order independence.
- Add failure tests showing unsupported value types and invalid supported values fail before a digest is produced.
- Add culture-sensitivity tests that change CurrentCulture and CurrentUICulture and verify number, decimal, timestamp, normalized text, and digest results remain stable.

### Scope Out
- Full Data Vault hash key and hash diff entity services for hubs, links, satellites, or model-specific participating-field decisions.
- Persisted storage schema, migrations, provider-specific physical types, compatibility migrations, or database integration for stored hashes.
- A standalone binary scalar encoding for byte arrays, streams, or base64 canonicalization.
- Password hashing, encryption, MACs, signatures, key management, salts, secret rotation, or other security-specific hashing behavior.
- Advanced hash replacement UI or broad configuration design beyond the minimal DI/options registration behavior needed for the default service boundary.
- Renaming or restructuring the existing source and test roots outside the established src/DCoding.Data.DVault and tests/DCoding.Data.DVault.Tests layout.

## Acceptance Criteria
- A production hash service abstraction or accepted equivalent exposes AlgorithmId and ComputeHash(string normalizedInput), and the default implementation reports AlgorithmId sha256-v1.
- A production digest type or accepted equivalent exposes AlgorithmId and Value, with AlgorithmId copied from the service and Value as 64 lowercase hexadecimal SHA-256 characters.
- The default service hashes empty normalized input to e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855 and treats null input as ArgumentNullException.
- Unit tests assert the exact published digests from docs/plans/stable-hashing-contract.md for empty input, empty string stable value, null stable value, repeated deterministic text, ordered structured value with null, and culture-invariant decimal/timestamp.
- Normalization tests assert the concrete canonical text for string NFC normalization, line-ending normalization, invariant scalar formatting, null field inclusion, and ordinal structured field ordering before digest assertions where practical.
- Tests demonstrate that source field order or dictionary iteration order cannot change normalized structured input or the resulting digest.
- Tests demonstrate that non-invariant current culture settings do not change normalized decimal, number, timestamp, or digest results.
- Tests demonstrate that unsupported value types and invalid supported values fail before any hash digest is produced.
- The default stable hash service is obtainable through the repository's startup or dependency registration path without callers constructing the concrete implementation directly.

## Definition of Done
- Production source changes for the service, digest, normalizer, and registration live under src/DCoding.Data.DVault using the repository's existing namespace and layout conventions.
- New tests live in the appropriate tests/DCoding.Data.DVault.Tests project, preferably Unit unless shared helpers are genuinely reused across test projects.
- Public production APIs introduced by this ticket include XML documentation sufficient for the existing CS1591 warnings-as-errors policy.
- dotnet test succeeds for the affected test projects or the repository solution entry point.
- bash tools/check-format.sh succeeds after the source and test changes.
- Implementation remains within docs/plans/stable-hashing-contract.md and does not introduce full entity-specific hash key/hash diff behavior or persistence concerns.

## Implementation Notes
- Use docs/plans/stable-hashing-contract.md as the normative source for public boundary equivalence, vectors, canonical text, failure behavior, and replacement rules.
- The documented names IStableHashService and StableHashDigest are acceptable target names; if the implementation uses different names, it must still expose equivalent production members and behavior.
- Prefer registering the default stable hash service from AddDVault or the existing DI/options registration pattern, preserving caller overrides where they already exist or are introduced for this boundary.
- Use an explicit UTF-8 encoding path that does not prepend a BOM; SHA-256 input bytes must be exactly the UTF-8 bytes of normalizedInput.
- Use invariant culture for numeric and timestamp formatting, UTC round-trip timestamp text, lowercase d-format GUIDs, and ordinal string comparison for field paths.
- Do not use general-purpose object, dictionary, or JSON serialization as the canonical normalizer; map deliberate field paths and scalar encodings first, then join sorted lines with LF and no trailing LF.
- Culture tests must restore CultureInfo.CurrentCulture and CultureInfo.CurrentUICulture in cleanup logic so the test process is not polluted.
- When asserting hashes, also assert AlgorithmId propagation and lowercase hex shape so replacement and compatibility behavior remains observable.

## Open Questions
- none

## Follow-Up Questions
- Parent story 06EXB765S2X2MR2K18ZBV8RC38 still needs separate PO refinement for the broader hash key/hash diff service roadmap beyond this bounded stable hash service and normalizer slice.
- Should a later contract ticket add first-class binary scalar normalization for byte arrays or streams? Current evidence does not define it, so it remains outside this ticket.
- When persisted hashes are introduced, confirm where AlgorithmId and canonicalization version are stored for compatibility and migration planning.
- Future entity-specific hashing tickets should identify participating fields and add vectors for those model-specific canonical inputs.
- Advanced alternate hash configuration beyond basic DI/options replacement should be refined with the optional advanced configuration hook work if product needs it.

## Risks
- The scope is broader than the prior test-only wording; developers must keep production changes limited to the stable-hashing contract and avoid implementing the full parent hash key/hash diff story here.
- Public API additions can fail build policy if XML documentation is missing because the library treats CS1591 as an error.
- Culture tests can leak process-global state if CurrentCulture and CurrentUICulture are not restored.
- Timestamp and decimal normalization can drift if implementation accidentally uses current culture, local time, serializer defaults, or platform-default encoding.

## Split Recommendations
- No child split or prerequisite relation is recommended now because this ticket is the bounded implementation-and-test slice for the default stable hash service and canonical normalizer.
- Split full Data Vault hash key/hash diff entity services, persistence integration, and participating-field selection under the parent story or later follow-up tickets.
- Create a separate binary scalar canonicalization ticket only if product approves byte array, stream, or base64 normalization beyond UTF-8 materialization of normalized strings.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Add tests that protect deterministic hashing behavior.

## Scope
- Cover nulls, strings, numbers, dates, binary values, ordering, and culture-sensitive cases.

## Acceptance Criteria
- Tests demonstrate stable hash materialization.
- Edge cases are documented through test names.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.