<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined ticket 06EXB765S2X2MR2K18ZBV8RC38 using the persisted ticket snapshot, comments, relations, attached-context documents, and current branch source/test layout. Existing child tickets and blockers already cover the underlying stable hashing contract and downstream consumers, so no additional split or planning artifact was materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 hashing baseline is ratified from docs/plans/stable-hashing-contract.md: default AlgorithmId sha256-v1, SHA-256 over UTF-8 without BOM, lowercase hexadecimal digest output, and deterministic behavior with no salts, timestamps, machine state, current culture, serializer defaults, or dictionary iteration ordering.
- The public service boundary should remain the small stable-hash abstraction described by the contract: IStableHashService exposes AlgorithmId and ComputeHash over already-normalized text, and StableHashDigest carries the algorithm id plus digest value.
- Normalization is a required part of this story for supported scalar and structured inputs: null, string, boolean, integer, decimal, UTC timestamp, and Guid values; string normalization uses Unicode Form C, CR/CRLF to LF conversion, UTF-8 byte counts, invariant formatting, ASCII type tags, ordinal field ordering, explicit null fields, and no trailing LF.
- Repository evidence shows the owning implementation path is src/DCoding.Data.DVault with executable tests under tests/DCoding.Data.DVault.Tests, including Unit stable hashing coverage; these are the v1 default locations for this ticket.
- Persisted relations already link this story as parent of 06EXB76DNVSRBD12T4W03AWQZC and 06EXB76NNRDP7WH1F2R5VYYPMR, and as blocking downstream tickets 06EXB7GYQKBZ8FMQN6YDYCKATG, 06EXB7HEJY18HEB5A5MVTN5KZC, and 06EXB7HPGW3Y9MSP10DEC8RBK4.

### Scope In
- Implement provider-neutral stable hashing services in the main DVault library project for hash key and hash diff use cases.
- Expose a default SHA-256 v1 service through the public abstraction and register it through the convention-first AddDVault service registration path without requiring user options.
- Implement canonical normalization for supported scalar values and structured field sets so business-key and satellite-payload callers can deliberately map fields before hashing.
- Preserve explicit behavior for null, empty string, string Unicode/line-ending normalization, boolean, integer, decimal, UTC timestamp, Guid, duplicate field path, invalid field path, unsupported type, and invalid value cases.
- Add or maintain focused unit tests using the stable hashing contract vectors, including deterministic repeat hashing, empty input, null distinct from empty string, structured-field ordering independence, non-invariant current culture behavior, and failure-before-hashing behavior for invalid inputs.

### Scope Out
- Provider-specific persistence schema, migrations, SQL column types, physical indexes, or adapter-specific storage behavior.
- Domain-specific selection of hub, link, or satellite fields for a particular model; callers must explicitly map participating fields in later entity tickets.
- Security-specific hashing, password hashing, encryption, MACs, signatures, key management, salts, or secret rotation.
- Runtime configuration option objects or broad advanced hook implementation beyond preserving replaceability through dependency injection/service abstractions already needed for the default registration path.
- Changing Data Vault naming conventions, persistence logical object names, load timestamp policy, or record-source policy outside the documented shared standards.

## Acceptance Criteria
- The default stable hash service reports AlgorithmId sha256-v1 and computes the documented lowercase 64-character SHA-256 digest for UTF-8 normalized input without BOM, including the zero-length input vector.
- A null normalized input passed to the hash service fails fast with ArgumentNullException, while an empty normalized input remains valid and hashes as the documented empty byte sequence.
- Supported scalar values normalize to the documented ASCII-tagged canonical forms, with invariant culture formatting and no current-culture-dependent output.
- String normalization converts CRLF and CR to LF, applies Unicode normalization Form C before UTF-8 byte count calculation, and preserves case plus leading, trailing, and internal whitespace.
- Structured fields are deliberately mapped as field-path/value pairs, reject null/blank, duplicate, or unsafe field paths, include explicit null fields, sort by ordinal field path, join lines with LF, and produce no trailing LF.
- Unsupported value types fail with NotSupportedException that identifies the field path or value type, and invalid supported values fail before hashing with ArgumentException or ArgumentOutOfRangeException as appropriate.
- The service and normalizer are available through the DVault dependency-injection registration path and can be replaced by registering the public abstractions without model code depending on concrete implementation types.
- Unit tests assert the contract test vectors and culture/order/null/binary-related edge behavior needed for provider-neutral hash key and hash diff computation.

## Definition of Done
- Implementation lives in src/DCoding.Data.DVault and follows the existing package/root namespace conventions for DCoding.Data.DVault.
- Executable coverage lives under tests/DCoding.Data.DVault.Tests, with stable hashing tests in the unit test area unless integration behavior is explicitly needed.
- dotnet build and dotnet test succeed from the repository solution entry point DVault.slnx.
- bash tools/check-format.sh succeeds and no repository formatting/encoding standards from docs/plans/shared-implementation-standards.md are violated.
- Public XML documentation remains complete for public abstractions and value types because the library treats CS1591 as an error.
- The implementation remains provider-neutral and stores/returns algorithm identity with digest values so future persistence tickets can retain hash version metadata.

## Implementation Notes
- Use docs/plans/stable-hashing-contract.md as the normative contract for algorithm, canonical text, test vectors, replacement rules, and compatibility notes.
- Use docs/plans/shared-implementation-standards.md for repository layout, formatting, .NET baseline, and validation expectations; the visible project baseline is net10.0 in src/DCoding.Data.DVault/DCoding.Data.DVault.csproj.
- The source branch already contains stable hashing-related files: DefaultStableHashService, DefaultStableHashNormalizer, IStableHashService, IStableHashNormalizer, StableHashDigest, and AddDVault registrations. Treat these names and locations as the current v1 implementation shape unless a code review finds a concrete contract mismatch.
- Binary payload hashing is not a separate public raw-byte service in the v1 contract; binary values should either remain out of model normalization or be mapped by a later explicit domain contract. This ticket must still document/fail unsupported binary object values explicitly rather than silently coercing them.
- Decimal callers must define domain scale before hashing persisted decimal values; the shared normalizer should stay deterministic but must not invent domain-specific rounding or scale policy.
- Timestamps used in hashing must be UTC canonical values; non-UTC DateTime values should fail instead of being silently converted at the model-normalization boundary.

## Open Questions
- none

## Follow-Up Questions
- Future entity-specific tickets should decide exactly which hub business-key, link participant-key, and satellite payload fields participate in each hash key or hash diff and add entity-specific vectors.
- A later advanced-configuration ticket can decide the full options API for alternate hash service factories; this story only needs the abstraction and replaceable registration behavior required by the v1 default path.
- Future persistence tickets should define how algorithm id and digest values are stored in provider-specific schemas while preserving the provider-neutral metadata contract.

## Risks
- Hash normalization is compatibility-sensitive: any post-release change to algorithm id, scalar encodings, field ordering, culture formatting, or timestamp handling will require persisted-hash compatibility work.
- Decimal and binary inputs can be misused if callers assume the shared service performs domain-specific scale or byte-payload decisions; the ticket should keep those boundaries explicit in documentation and tests.
- Using serializer output, dictionary iteration order, or current culture anywhere in model-specific callers would break the deterministic contract even if the shared hash service itself is correct.

## Split Recommendations
- No new child ticket is needed for this refinement pass. Existing relations already split the stable hashing contract and downstream blocked work; this story can proceed as the implementation story for the documented v1 stable hashing services.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Create stable hashing services for business keys and satellite payload changes.

## Scope
- Normalize values deterministically.
- Support hash key and hash diff computation for provider-neutral persistence.

## Acceptance Criteria
- Hashing is stable across culture and ordering differences.
- Null and binary values have explicit behavior.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.