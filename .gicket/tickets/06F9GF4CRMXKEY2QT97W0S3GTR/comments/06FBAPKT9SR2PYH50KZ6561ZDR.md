[gicket-bot] PO refinement contract

Summary
- Refined this as a bounded v0.35.0 documentation task around the already-established stable-hash contract; no ticket or planning writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the bounded built-in algorithm set: `sha256-v1` is the default, with explicit opt-in `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1` in `src/DCoding.Data.DVault/BuiltInStableHashService.cs`.
- `AddDVault()` keeps `sha256-v1` as the default registration and does not enable non-default ids automatically; that behavior is already covered by `README.md`, `docs/plans/stable-hashing-contract.md`, and `tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs`.
- `docs/releases/v0.22.0.md` already established the stable-hash governance baseline, so this ticket is a v0.35.0 guidance refresh rather than a new algorithm-selection decision.
- No bounded child-ticket, relation, description, attachment, or planning-document writes were applied during this refinement pass.

Scope In
- README, release-note, and compatibility-guidance updates for planned release `v0.35.0` and aligned package outputs `8.35.0`/`10.35.0`.
- Default-selection guidance explaining why `sha256-v1` remains the safe compatibility baseline for persisted hub keys, link keys, hash diffs, and related stable hashes.
- Explicit opt-in guidance for shorter digests and `sha1-v1`, including non-adversarial use framing, collision-risk tradeoffs, and security/compliance caveats.
- Documentation of the existing no-automatic-migration posture and the repository proof surfaces for algorithm choice, digest shape, determinism, and validation behavior.

Scope Out
- Changing the runtime default algorithm, stable-hash normalizer, built-in algorithm set, or provider-side hashing behavior.
- Implementing automatic rehash, backfill, migration tooling, or any persisted-hash repair workflow.
- Adding new cryptographic or compliance features, password-hashing guidance, or security-control claims beyond bounded caveats.
- Introducing new benchmark, collision-simulation, or provider-specific diagnostics features beyond documenting the existing evidence surfaces.

Open questions
- none

Follow-up questions
- Should a later ticket add quantitative key-volume sizing examples or collision-budget tables for teams choosing between `sha256-128-v1` and `sha256-160-v1`?
- Once blocking ticket `06F9GF46KZYRKR1EGEPR3TV824` resolves, does any newly landed implementation evidence need to be cross-linked into the final v0.35.0 doc set?
- Should a future governance ticket formally deprecate `sha1-v1`, or is the current non-default-with-caveats posture sufficient for now?

Risks
- The ticket currently has a live incoming `blocks` relation from `06F9GF46KZYRKR1EGEPR3TV824`; if that dependency changes the shipped algorithm surface or diagnostics wording, this documentation slice will need a last sync pass before closure.
- The main delivery risk is documentation drift across `README.md`, the stable hashing contract, and the new `v0.35.0` release note. The exact ids, digest lengths, and no-automatic-migration posture must stay aligned.
- Overstating `sha1-v1` or truncated digests as security or compliance features would conflict with the contract and create avoidable adoption risk.

Split recommendations
- No split recommended; the remaining work is one bounded documentation slice anchored to existing contract, code, and test evidence.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment