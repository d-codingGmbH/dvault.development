<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement ratified algorithm-aware StableHashDigest validation around a bounded v1 algorithm set, preserved sha256-v1 compatibility, applied the current ticket description update, and queued cleanup of one stale historical blocks relation.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Applied a ticket description update on 06F9GF3TRG65G8MTMG7DH4PREC that makes docs/plans/stable-hashing-contract.md the authoritative boundary for this task: sha256-v1 stays the default 32-byte/64-lowercase-hex digest, sha1-v1 is 20 bytes/40 hex, sha256-128-v1 is 16 bytes/32 hex, sha256-160-v1 is 20 bytes/40 hex, and unknown caller-supplied algorithm ids are accepted only for whole-byte lowercase hex.
- Repository evidence in src/DCoding.Data.DVault/StableHashDigest.cs and tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs already matches that bounded direction: DigestByteLength is derived from Value length, known ids enforce algorithm-specific lengths, custom ids are lower-hex whole-byte only, and published sha256-v1 vectors remain fixed.
- No child tickets, attachments, or new planning documents were materialized for this refinement pass.
- Queued removal of stale relation 06F9GF3MZHKQQ6D4SAQ0AMTKJR--06F9GF3TRG65G8MTMG7DH4PREC--blocks as outbox mutation-ee8323dd972bfc8a so the historical story no longer blocks this task once replay runs on the source ticket owner branch.

### Scope In
- Relax StableHashDigest and directly related validation logic so digest shape is algorithm-aware instead of universally fixed to 64 lowercase hex characters.
- Preserve the public StableHashDigest metadata surface needed by callers: AlgorithmId, Value, and DigestByteLength.
- Keep the default IStableHashService/AddDVault behavior compatible with sha256-v1 and its published test vectors.
- Add or maintain regression tests for valid canonical lowercase hex, invalid lengths, invalid non-lowercase or non-whole-byte values, and existing sha256-v1 vectors.

### Scope Out
- Changing the default stable hash algorithm away from sha256-v1.
- Automatically registering non-default algorithm ids in AddDVault().
- Changing DataVaultConventions persistence content-hash policy or content_hash storage semantics.
- Automatic migration, rehashing, or backfill of persisted hub keys, link keys, hash diffs, or other stored stable-hash values.

## Acceptance Criteria
- Constructing StableHashDigest validates digest text as canonical lowercase whole-byte hexadecimal and applies algorithm-specific length checks instead of assuming every digest is 64 characters.
- sha256-v1 remains exactly 32 digest bytes serialized as 64 lowercase hexadecimal characters, and the published default SHA-256 vectors remain unchanged.
- The bounded non-default v1 ids accepted by repository contract are sha1-v1 at 40 hex characters, sha256-128-v1 at 32 hex characters, and sha256-160-v1 at 40 hex characters.
- Unknown caller-supplied algorithm ids are accepted only when the digest text is even-length lowercase hex, with DigestByteLength derived from the provided value.
- Regression tests cover canonical lowercase valid inputs, invalid lengths for known ids, invalid odd-length or uppercase/non-hex inputs, default sha256-v1 vectors, and a non-default/custom service override path without breaking existing public compatibility.

## Definition of Done
- src/DCoding.Data.DVault/StableHashDigest.cs and any directly related validation code implement the refined algorithm-aware contract without regressing the existing public members.
- tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs or equivalent automated coverage proves the valid and invalid digest-shape cases and preserves the published sha256-v1 vectors.
- Default AddDVault/IStableHashService behavior still resolves sha256-v1 by default and existing default-hash assumptions that are intentionally preserved continue to pass.
- The ticket handoff surface is updated to reflect the bounded scope and the stale historical blocks relation cleanup has been applied or queued consistently with live ticket tooling.

## Implementation Notes
- Treat AlgorithmId as the authoritative digest-semantics key; do not infer sha256-v1 semantics from Value length alone.
- Validate lowercase whole-byte hexadecimal shape before applying known-algorithm length enforcement, then compute DigestByteLength as Value.Length / 2.
- Keep the v1 known-algorithm table bounded to sha256-v1, sha1-v1, sha256-128-v1, and sha256-160-v1; unknown ids remain caller-owned custom compatibility choices rather than DVault-approved defaults.
- Preserve the current default registration and storage-facing sha256 expectations elsewhere in the repo; this ticket refines the digest object and validation boundary, not broader persisted-hash migration semantics.

## Open Questions
- none

## Follow-Up Questions
- If product later wants non-default stable-hash ids to be used for persisted hub/link keys rather than only supported by the digest boundary, should that be handled in a separate compatibility ticket covering storage, migration, and provider/integration expectations?
- Should a future API or documentation ticket expose the bounded known-algorithm digest-length table more directly for consumers that want pre-validation guidance before constructing StableHashDigest?

## Risks
- Other repository areas and downstream consumers still commonly assume default 64-character hash keys; this task intentionally preserves sha256-v1 as the default, so broader algorithm-substitution compatibility remains separate work.
- Accepting unknown custom algorithm ids is deliberate but caller-owned, so documentation and reviews must avoid implying those ids are a DVault cryptographic approval or compliance policy.
- The stale blocks relation cleanup is queued for replay on another ticket owner branch, so relation views may temporarily continue to show the historical block until outbox mutation-ee8323dd972bfc8a replays.

## Split Recommendations
- No child-ticket split is needed for this task; the current scope is already bounded to StableHashDigest validation behavior, preserved sha256-v1 compatibility, and regression coverage.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Update the StableHashDigest and related validation surface so supported digests are not hard-coded to 64 lowercase hex characters. Preserve existing sha256-v1 behavior and public compatibility where possible, expose enough metadata for digest length and bytes, and add regression tests for canonical lowercase hex, invalid lengths, and existing SHA-256 test vectors.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Verified the current ticket branch already satisfies the StableHashDigest algorithm-aware validation contract.
- No repository source, test, or documentation file changes were required during the dev pass.
- The existing implementation preserves sha256-v1 compatibility while allowing the bounded non-default v1 IDs and caller-owned custom whole-byte lowercase hexadecimal digests.

### Repository State
- `docs/plans/stable-hashing-contract.md` defines the bounded v1 digest set and unknown custom-id behavior used as the authoritative contract.
- `src/DCoding.Data.DVault/StableHashDigest.cs` validates canonical lowercase whole-byte hex, applies known-ID length checks for `sha256-v1`, `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1`, and derives `DigestByteLength` from `Value.Length / 2`.
- `tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs` covers published sha256-v1 vectors, known non-default digest lengths, custom algorithm lower-hex acceptance, invalid known lengths, invalid uppercase/non-hex/odd-length values, and the AddDVault override path.

### Verification
- `dotnet build DVault.slnx --nologo` completed successfully with `0 Error(s)` and existing warnings. The run reported NU1900 warnings from sandboxed NuGet vulnerability-cache writes and existing analyzer warnings elsewhere in the repository.
- `dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --nologo --no-build --no-restore --filter StableHashServiceTests` passed for both target frameworks. The Microsoft Testing Platform runner ignored the VSTest-style filter and ran the full unit test assembly: 468 tests passed on net8.0 and 486 tests passed on net10.0.
- `bash tools/check-format.sh` passed.
- `git diff --name-only -- docs/plans/stable-hashing-contract.md src/DCoding.Data.DVault/StableHashDigest.cs tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs` returned no changed paths.

### Notes
- Full `dotnet test DVault.slnx --nologo` was not run after the 35-minute full solution build; ticket-specific unit coverage and repository build/format checks passed.
<!-- gicket-bot:developer-delivery:v1:end -->