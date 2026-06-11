<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this as a bounded v0.35.0 documentation task around the already-established stable-hash contract; no ticket or planning writes were needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already fixes the bounded built-in algorithm set: `sha256-v1` is the default, with explicit opt-in `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1` in `src/DCoding.Data.DVault/BuiltInStableHashService.cs`.
- `AddDVault()` keeps `sha256-v1` as the default registration and does not enable non-default ids automatically; that behavior is already covered by `README.md`, `docs/plans/stable-hashing-contract.md`, and `tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs`.
- `docs/releases/v0.22.0.md` already established the stable-hash governance baseline, so this ticket is a v0.35.0 guidance refresh rather than a new algorithm-selection decision.
- No bounded child-ticket, relation, description, attachment, or planning-document writes were applied during this refinement pass.

### Scope In
- README, release-note, and compatibility-guidance updates for planned release `v0.35.0` and aligned package outputs `8.35.0`/`10.35.0`.
- Default-selection guidance explaining why `sha256-v1` remains the safe compatibility baseline for persisted hub keys, link keys, hash diffs, and related stable hashes.
- Explicit opt-in guidance for shorter digests and `sha1-v1`, including non-adversarial use framing, collision-risk tradeoffs, and security/compliance caveats.
- Documentation of the existing no-automatic-migration posture and the repository proof surfaces for algorithm choice, digest shape, determinism, and validation behavior.

### Scope Out
- Changing the runtime default algorithm, stable-hash normalizer, built-in algorithm set, or provider-side hashing behavior.
- Implementing automatic rehash, backfill, migration tooling, or any persisted-hash repair workflow.
- Adding new cryptographic or compliance features, password-hashing guidance, or security-control claims beyond bounded caveats.
- Introducing new benchmark, collision-simulation, or provider-specific diagnostics features beyond documenting the existing evidence surfaces.

## Acceptance Criteria
- Updated docs state that `AddDVault()` defaults to `sha256-v1`, with SHA-256 over UTF-8 normalized input bytes without a byte order mark and 64 lowercase hexadecimal characters.
- Updated docs enumerate `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1` as explicit opt-in built-ins only, and explain when shorter digests may be reasonable for non-adversarial Data Vault key hashing.
- Updated docs explicitly warn that `sha1-v1` is not a security or compliance control and that algorithm or truncation changes after persistence are caller-owned compatibility work with no automatic rehash, backfill, or migration.
- The `v0.35.0` release-note and compatibility wording carry forward the existing stable-hash contract instead of implying new runtime hashing behavior.
- The final documentation points readers to `docs/plans/stable-hashing-contract.md` and `tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs` as the proof surfaces for vectors, digest shape, determinism, explicit selection, and truncated-digest behavior.

## Definition of Done
- README and release-note or compatibility guidance are internally consistent with `docs/plans/stable-hashing-contract.md`, `BuiltInStableHashService.cs`, and `StableHashDigest.cs`.
- The exact built-in ids and digest lengths match repository code and tests: `sha256-v1` 64 hex, `sha1-v1` 40 hex, `sha256-128-v1` 32 hex, and `sha256-160-v1` 40 hex.
- The documentation uses existing diagnostics and proof surfaces only: `AlgorithmId`, algorithm-aware digest validation, published vectors, and stable-hash unit tests.
- The completed doc set preserves the current boundary that provider packages may optimize transport but must not silently replace the shared stable-hash contract.

## Implementation Notes
- Start from the existing `README.md` 'Govern stable hashes' section and `docs/releases/v0.22.0.md` 'Stable Hash Governance' section; extend that language rather than inventing a new narrative.
- `tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs` already covers published SHA-256 vectors, repeated deterministic hashing, UTF-8-without-BOM behavior, explicit built-in selection, and truncated-SHA-256 leading-byte behavior; cite those tests as the primary proof surface.
- `docs/plans/stable-hashing-contract.md` already states that non-default ids are not auto-registered, SHA-1 and truncated SHA-256 are non-adversarial identity trade-offs only, and post-persistence algorithm changes are caller-owned compatibility work; the v0.35.0 docs should summarize those points consistently.
- For diagnostics wording, stay within existing repository surfaces such as `StableHashDigest.AlgorithmId`, `StableHashDigest.DigestByteLength`, and algorithm-aware validation; do not promise new runtime migration, compliance, or observability features.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket add quantitative key-volume sizing examples or collision-budget tables for teams choosing between `sha256-128-v1` and `sha256-160-v1`?
- Once blocking ticket `06F9GF46KZYRKR1EGEPR3TV824` resolves, does any newly landed implementation evidence need to be cross-linked into the final v0.35.0 doc set?
- Should a future governance ticket formally deprecate `sha1-v1`, or is the current non-default-with-caveats posture sufficient for now?

## Risks
- The ticket currently has a live incoming `blocks` relation from `06F9GF46KZYRKR1EGEPR3TV824`; if that dependency changes the shipped algorithm surface or diagnostics wording, this documentation slice will need a last sync pass before closure.
- The main delivery risk is documentation drift across `README.md`, the stable hashing contract, and the new `v0.35.0` release note. The exact ids, digest lengths, and no-automatic-migration posture must stay aligned.
- Overstating `sha1-v1` or truncated digests as security or compliance features would conflict with the contract and create avoidable adoption risk.

## Split Recommendations
- No split recommended; the remaining work is one bounded documentation slice anchored to existing contract, code, and test evidence.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Update hashing docs, README guidance, release notes, and compatibility notes for planning release v0.35.0 and package outputs 8.35.0/10.35.0. Document why sha256-v1 remains the default, when shorter opt-in algorithms may be reasonable for Data Vault key hashing, SHA-1 security/compliance caveats, collision-risk framing, no supported automatic migration, and how tests/diagnostics prove the selected algorithm.