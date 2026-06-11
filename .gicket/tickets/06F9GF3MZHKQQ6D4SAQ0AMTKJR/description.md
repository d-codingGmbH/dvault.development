<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story around the existing `sha256-v1` baseline: widen the stable-hash digest contract from fixed 64-character SHA-256 hex to an algorithm-aware variable-digest contract, keep current relation state unchanged, and do not reopen persistence content-hash policy.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already fixes the zero-config baseline: `docs/plans/stable-hashing-contract.md`, `DefaultStableHashService`, `StableHashServiceTests`, and `DataVaultConventions.StableHashAlgorithmId` all ratify `sha256-v1` as the default stable-hash algorithm id.
- The present public `StableHashDigest` contract still hard-codes exactly 64 lowercase hexadecimal characters, so this story should explicitly remove the fixed-SHA-256 assumption from the digest contract while preserving `sha256-v1` compatibility.
- Persistence content hashing is a separate policy surface with its own fixed `sha-256` and 64-character lowercase-hex rules; this story should not weaken or broaden `content_hash` storage semantics.
- No child tickets, relation mutations, description updates, attachments, or planning documents were materialized during this refinement because the scope is already bounded as a single story.

### Scope In
- Define stable `AlgorithmId` semantics so an algorithm id covers the digest algorithm, any truncation policy, and the compatibility version for identical normalized input.
- Redefine the stable-hash digest contract so canonical lowercase hex is the required serialized form and digest length is validated per algorithm instead of globally fixed at 64 characters.
- Preserve `sha256-v1` as the default out-of-box algorithm, current vectors, UTF-8/no-BOM behavior, and `AddDVault()` zero-configuration registration path.
- Document explicit, non-default candidate ids for `sha1-v1` and truncated SHA-256 variants, including their opt-in status and deterministic compatibility expectations.
- Document collision-risk and compliance guidance for non-adversarial Data Vault key hashing, plus the explicit non-goal of automatic key migration when algorithms change.
- Allow optional digest-byte access only if it is read-only and guaranteed to round-trip with the canonical hex value.

### Scope Out
- Automatic rehashing, backfill, or migration of persisted hub keys, link keys, hash diffs, or other stored values when callers change algorithms.
- Changes to the persistence `content_hash` tuple contract or `DataVaultConventions.PersistenceContentHashAlgorithm`, which remain fixed to the separate storage policy.
- Provider-side SQL hashing, provider-specific canonicalization, or silent replacement of the shared .NET normalizer and service semantics.
- Any claim that SHA-1 or truncated digests are a security control, password-hashing policy, or adversarial-collision defense.
- An open-ended algorithm plug-in matrix beyond the bounded ids and candidates approved by this story.

## Acceptance Criteria
- The stable-hash public contract states that `AlgorithmId` is stable, non-empty, versioned, and authoritative for digest semantics; equal `AlgorithmId` plus equal normalized input must yield equal digest bytes and equal canonical hex.
- The digest value contract uses lowercase hexadecimal without prefixes as the required serialized form, and validation is algorithm-aware so non-`sha256-v1` digests are not rejected solely for being shorter than 64 characters.
- `sha256-v1` remains the default registered behavior, continues to hash UTF-8 bytes without a BOM, and preserves every published `sha256-v1` test vector and current zero-config `AddDVault()` behavior.
- The contract documents `sha1-v1` and explicitly named truncated SHA-256 candidates as non-default opt-in algorithms, including digest byte length, hex length, and the requirement that they never masquerade as `sha256-v1`.
- If optional digest-byte access is exposed, it is read-only and byte-for-byte equivalent to the canonical hex value for the same digest.
- The contract explicitly states that adopting a different algorithm or truncation after hashes are persisted is caller-owned compatibility work and is not handled by automatic key migration.

## Definition of Done
- The authoritative stable-hashing planning and documentation surfaces are updated consistently so they no longer describe every stable-hash digest as fixed 64-character SHA-256 output, while still preserving `sha256-v1` as the compatibility baseline.
- Source, unit tests, and public API approval artifacts are updated so the stable-hash digest surface matches the new algorithm-aware contract and no public stable-hash validation path assumes all digests are 64 characters.
- Validation coverage proves `sha256-v1` backward compatibility and also proves the widened digest contract accepts at least one shorter algorithm-specific hex shape or equivalent algorithm-aware test double.
- All updated docs keep stable model and key hashing separate from persistence `content_hash` storage policy so reviewers do not infer a storage-integrity downgrade.

## Implementation Notes
- Current fixed-shape assumptions live in `src/DCoding.Data.DVault/StableHashDigest.cs`, `src/DCoding.Data.DVault/DefaultStableHashService.cs`, `tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs`, and `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt`; those are the primary evidence points for the required contract widening.
- Keep `IStableHashService.ComputeHash(string normalizedInput)` and the shared normalizer behavior intact unless a broader API change is strictly necessary; the main contract change is digest-shape flexibility, not a new normalization pipeline.
- Treat `DataVaultConventions.StableHashAlgorithmId = "sha256-v1"` as the default model and key hashing baseline and keep it separate from `PersistenceContentHashAlgorithm = "sha-256"`.
- If alternate algorithms are documented before built-in registrations ship, the docs and tests must distinguish approved contract ids from default runtime registration so `AddDVault()` is not read as auto-enabling SHA-1 or truncated digests.
- Any byte-access addition should avoid mutable aliasing; the byte representation and canonical hex representation must not be allowed to diverge.

## Open Questions
- none

## Follow-Up Questions
- If product wants out-of-box registrations beyond the caller-supplied service override path, which exact truncated SHA-256 sizes should ship first and which model or key surfaces may use them?
- If alternate algorithm ids later need persisted storage metadata outside the in-memory stable-hash surface, should that land as a separate storage and migration ticket rather than expanding this story?

## Risks
- Broadening `StableHashDigest` is a public API behavior change and may break callers or tests that currently assume every digest is 64 lowercase hex characters.
- Without explicit wording, teams could confuse stable model and key hashing with persisted `content_hash` integrity rules and accidentally weaken storage expectations.
- Allowing SHA-1 or truncated digests without prominent caveats could be misread as a security recommendation instead of a bounded non-adversarial identity trade-off.

## Split Recommendations
- If implementation effort grows beyond the base contract and API widening, split built-in `sha1-v1` or truncated-SHA-256 registrations and their full compatibility-vector coverage into one or more follow-up tickets after the `sha256-v1`-compatible digest-contract change lands.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Define the stable hashing contract beyond fixed 64-character SHA-256 hex. Cover AlgorithmId semantics, digest byte length, canonical hex representation, optional byte access, default sha256-v1 compatibility, opt-in sha1-v1 and truncated SHA-256 candidates, collision-risk guidance for non-adversarial Data Vault key hashing, compliance wording for SHA-1, and the explicit non-goal of supported automatic key migration.