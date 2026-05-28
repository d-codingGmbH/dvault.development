<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined as ready for PO critic: the repository already treats `docs/plans/stable-hashing-contract.md` as the v1 canonicalization manifest and backs it with stable-hash normalizer/service vector tests; no child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- For this ticket, the durable manifest is the existing repository document `docs/plans/stable-hashing-contract.md`; a separate JSON/YAML manifest file is not required for v1.
- The v1 binary-handling decision is already bounded by repository evidence: unsupported scalar types such as `byte[]` fail fast before hashing rather than receiving a new binary encoding.
- The compatibility baseline is provider-neutral published vectors plus unit assertions against the shared hashing services, not a provider-specific database matrix.
- No bounded ticket write was applied in this run; live parent/block relations remain unchanged.

### Scope In
- Ratify the v1 stable-hashing contract as the authoritative manifest for algorithm id, UTF-8 encoding, lowercase digest shape, null handling, field ordering, delimiter rules, culture invariance, and failure behavior.
- Treat the published normalized-input/digest pairs as the compatibility vectors that future versions and providers must continue to satisfy.
- Guard the shared canonicalization path used by DVault hub and link hash-key computation through normalizer/service tests and negative-case regression coverage.

### Scope Out
- Automatic derivation of satellite `hashDiff` values from payload fields; current save APIs still accept caller-supplied hash diff strings.
- Domain-specific field-selection rules for individual hubs, links, or satellites beyond the shared stable-hash contract.
- A new binary canonicalization format, provider-specific hash implementations, or migration tooling for changing `sha256-v1` semantics.

## Acceptance Criteria
- The authoritative manifest documents the v1 shared hashing contract, including `sha256-v1`, UTF-8 without BOM, lowercase hex output, NFC string normalization, LF line endings, invariant formatting, null encoding, ordinal structured-field ordering, and delimiter/path rules.
- The manifest or directly paired tests publish compatibility vectors for empty input, empty string, null, repeated deterministic text, ordered structured values with nulls, and culture-invariant decimal-plus-timestamp inputs.
- Regression tests prove the default normalizer and hash service reproduce the published vectors, stay independent of current culture and source field order, and fail before hashing unsupported or invalid values.
- The refined contract makes clear that current DVault shared hashing covers the normalizer/service used for hash-key generation, while any future automatic hash-diff producer must either reuse the same contract or ship a separately versioned contract.

## Definition of Done
- `docs/plans/stable-hashing-contract.md` remains the single source of truth for shared stable-hash canonicalization and compatibility vectors.
- `tests/DCoding.Data.DVault.Tests/Unit/StableHashNormalizerTests.cs` and `StableHashServiceTests.cs` cover the published vectors plus negative cases for invalid field paths, unsupported types, invalid values, and culture independence.
- The default DI registration continues to expose overridable `IStableHashService` and `IStableHashNormalizer` implementations without bypassing the documented contract.
- Shared hash-key computation paths in DVault continue to normalize structured fields and hash them through the documented services instead of relying on serializer defaults, current culture, or unordered enumeration.

## Implementation Notes
- `DefaultStableHashNormalizer` already normalizes strings to NFC, converts CRLF/CR to LF, counts UTF-8 bytes, sorts structured field paths ordinally, includes nulls, and rejects duplicate or invalid field paths.
- `DefaultStableHashService` already computes SHA-256 over UTF-8 bytes without BOM and returns algorithm id `sha256-v1` with lowercase hex digests.
- `DataVaultSaveService` computes hub and link hash keys through `_stableHashNormalizer.NormalizeFields(...)` and `_stableHashService.ComputeHash(...)`, which is the shared regression surface this ticket should protect.
- Satellite save operations currently require caller-supplied deterministic `hashDiff` values, so this ticket should document compatibility expectations for shared canonicalization rather than imply automatic satellite hash-diff generation.

## Open Questions
- none

## Follow-Up Questions
- If DVault later needs automatic satellite hash-diff generation, should that land as a separate story that defines participating payload fields and publishes its own contract-aligned vectors?
- If binary value hashing becomes a real v1/v2 requirement, should it be introduced as a separately versioned scalar encoding or algorithm identifier instead of altering `sha256-v1` behavior?

## Risks
- The current downstream `blocks` relation should remain until this story's documented contract and tests are accepted on the canonical target branch; current branch evidence alone is not closure evidence.
- Changing published scalar encodings, ordering, or failure behavior later without versioning would break the compatibility vectors that downstream hash-key producers depend on.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Add a durable hash canonicalization manifest and test vectors.

Acceptance criteria:
- Records algorithm, encoding, null handling, ordering, culture, binary, and delimiter behavior.
- Adds compatibility vectors that can be verified across providers and future versions.
- Detects accidental changes to hash-key or hash-diff canonicalization.

<!-- gicket-bot:dev-delivery:06F5Q934MSKVCQAHPCWEM29CZW:v1:start -->
## Developer Delivery

Summary
- Repository branch already satisfies the stable hashing story contract; no repository file edits were required.
- The durable manifest remains `docs/plans/stable-hashing-contract.md`.
- Compatibility vectors are published in the manifest and asserted by `tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs`.
- Normalizer regression coverage remains in `tests/DCoding.Data.DVault.Tests/Unit/StableHashNormalizerTests.cs`.
- Shared hub and link hash-key computation continues to use `IStableHashNormalizer.NormalizeFields(...)` and `IStableHashService.ComputeHash(...)` through `DefaultDataVaultSaveService`.

Verification
- `dotnet test DVault.slnx --nologo --filter FullyQualifiedName~StableHash` exited 0. Microsoft Testing Platform warned that the VSTest filter is ignored, so the command ran a broader solution test set; observed summaries included unit tests `403` passed and integration tests `176` passed, `21` skipped for missing external provider connection strings.
- `bash tools/check-format.sh` passed.
- `git diff --name-only -- docs/plans/stable-hashing-contract.md tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs tests/DCoding.Data.DVault.Tests/Unit/StableHashNormalizerTests.cs src/DCoding.Data.DVault/DataVaultSaveService.cs src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs` produced no output.

Repository changes
- none

Open questions
- none
<!-- gicket-bot:dev-delivery:06F5Q934MSKVCQAHPCWEM29CZW:v1:end -->