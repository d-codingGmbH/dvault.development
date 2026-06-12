<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Epic scope is bounded by the stable-hashing contract, current built-in hash service baseline, and five persisted child tickets; no additional PO-level blockers remain before PO-critic review.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already ratifies the bounded v1 stable-hash algorithm set: sha256-v1 as the default, with explicit opt-in alternatives sha1-v1, sha256-128-v1, and sha256-160-v1.
- The epic remains a tracking parent. Persisted relations show five parentOf children: 06F9GF3MZHKQQ6D4SAQ0AMTKJR, 06F9GF3TRG65G8MTMG7DH4PREC, 06F9GF417FDFWPBF1039G45FEW, 06F9GF46KZYRKR1EGEPR3TV824, and 06F9GF4CRMXKEY2QT97W0S3GTR.
- This follow-up metadata repair removed stale relation references after the child tickets completed; docs/plans/stable-hashing-contract.md and the existing ticket relations remain the authoritative planning surface.

### Scope In
- Make stable hash algorithm selection a first-class DVault capability while keeping sha256-v1 as the zero-configuration default.
- Support the bounded v1 non-default algorithm ids already documented in the repository for deliberate caller opt-in.
- Carry stable-hash algorithm identity and deterministic behavior through the service, EF Core/modeling, diagnostics, and documentation surfaces covered by this epic's child tickets.
- Preserve deterministic normalization, canonical lowercase hexadecimal digest behavior, and provider-neutral semantics across repeated runs and machines.

### Scope Out
- Automatic migration, rehashing, backfill, or reconciliation of previously persisted hub keys, link keys, hash diffs, or other stored hashes.
- Key rotation, secret management, cryptographic compliance positioning, or any security-specific hashing policy.
- Changing the persistence content_hash storage contract away from its fixed SHA-256 storage-integrity semantics.
- Database-side hashing, provider-specific orchestration, or making non-default algorithms implicit in AddDVault().

## Acceptance Criteria
- The epic's delivered work keeps sha256-v1 as the default registered compatibility baseline and exposes non-default stable hash algorithms only through explicit caller opt-in.
- The v1 supported algorithm identifiers remain bounded to the repository-defined set sha256-v1, sha1-v1, sha256-128-v1, and sha256-160-v1, with AlgorithmId remaining authoritative for digest semantics and validation.
- Delivered child work preserves deterministic UTF-8 normalized-input hashing, canonical lowercase hexadecimal digest text, and failure-before-hash behavior for null, invalid, or unsupported inputs.
- EF Core/modeling and public registration surfaces treat stable hash algorithm choice as a replaceable service boundary without introducing provider-dependent or platform-dependent behavior.
- Epic completion does not change default persistence content_hash behavior and does not introduce automatic persisted-hash migration or automatic database-side hashing.

## Definition of Done
- All persisted child tickets under this epic are refined against the stable hashing contract and completed or handed off without reopening the default algorithm or candidate-id baseline.
- Repository documentation and implementation/tests together demonstrate deterministic behavior for the default and approved non-default stable hash ids in the scope assigned to the child tickets.
- The epic can be closed once the five parentOf children are delivered and no current relation gate prevents epic completion.

## Implementation Notes
- Use docs/plans/stable-hashing-contract.md as the authoritative v1 contract for algorithm ids, normalization rules, digest serialization, and compatibility boundaries.
- Current repository code already exposes the bounded built-in algorithm baseline in src/DCoding.Data.DVault/BuiltInStableHashService.cs; downstream work should align to those ids instead of reopening naming or adding extra v1 algorithms.
- Keep stable hash algorithm selection separate from the persistence content_hash tuple; the contract explicitly keeps content_hash_algorithm/content_hash on the fixed SHA-256 storage policy.
- Preserve the convention-first posture described in docs/plans/optional-advanced-configuration-hooks.md: ordinary AddDVault() usage stays zero-configuration, and any alternate hash algorithm path remains additive and explicit.

## Open Questions
- none

## Follow-Up Questions
- After v1 delivery, should DVault publish separate operator guidance for callers that intentionally replace the stable hash service after hashes have already been persisted?

## Risks
- No current relation gate prevents epic completion; the remaining risk is documentation drift if future stable-hash work expands beyond the bounded v1 algorithm set.
- Shorter non-default digests reduce key width at the cost of a weaker collision profile; documentation and diagnostics need to keep those algorithms framed as non-default deterministic identity trade-offs, not security controls.
- If later work expands beyond the bounded v1 algorithm set, compatibility and migration pressure will grow because persisted stable-hash values are caller-owned once stored.

## Split Recommendations
- No additional split is required in this turn; the epic is already decomposed through five persisted parentOf child tickets and should continue as a tracking parent.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Make DVault stable hash algorithms first-class without changing the library focus. Keep sha256-v1 as the default, add explicit opt-in alternatives for smaller hash keys where appropriate, preserve deterministic EF Core behavior, and avoid automatic migration, key rotation, platform orchestration, or database-side hashing as a default path.