[gicket-bot] PO refinement contract

Summary
- Refined the story around the existing `sha256-v1` baseline: widen the stable-hash digest contract from fixed 64-character SHA-256 hex to an algorithm-aware variable-digest contract, keep current relation state unchanged, and do not reopen persistence content-hash policy.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the zero-config baseline: `docs/plans/stable-hashing-contract.md`, `DefaultStableHashService`, `StableHashServiceTests`, and `DataVaultConventions.StableHashAlgorithmId` all ratify `sha256-v1` as the default stable-hash algorithm id.
- The present public `StableHashDigest` contract still hard-codes exactly 64 lowercase hexadecimal characters, so this story should explicitly remove the fixed-SHA-256 assumption from the digest contract while preserving `sha256-v1` compatibility.
- Persistence content hashing is a separate policy surface with its own fixed `sha-256` and 64-character lowercase-hex rules; this story should not weaken or broaden `content_hash` storage semantics.
- No child tickets, relation mutations, description updates, attachments, or planning documents were materialized during this refinement because the scope is already bounded as a single story.

Scope In
- Define stable `AlgorithmId` semantics so an algorithm id covers the digest algorithm, any truncation policy, and the compatibility version for identical normalized input.
- Redefine the stable-hash digest contract so canonical lowercase hex is the required serialized form and digest length is validated per algorithm instead of globally fixed at 64 characters.
- Preserve `sha256-v1` as the default out-of-box algorithm, current vectors, UTF-8/no-BOM behavior, and `AddDVault()` zero-configuration registration path.
- Document explicit, non-default candidate ids for `sha1-v1` and truncated SHA-256 variants, including their opt-in status and deterministic compatibility expectations.
- Document collision-risk and compliance guidance for non-adversarial Data Vault key hashing, plus the explicit non-goal of automatic key migration when algorithms change.
- Allow optional digest-byte access only if it is read-only and guaranteed to round-trip with the canonical hex value.

Scope Out
- Automatic rehashing, backfill, or migration of persisted hub keys, link keys, hash diffs, or other stored values when callers change algorithms.
- Changes to the persistence `content_hash` tuple contract or `DataVaultConventions.PersistenceContentHashAlgorithm`, which remain fixed to the separate storage policy.
- Provider-side SQL hashing, provider-specific canonicalization, or silent replacement of the shared .NET normalizer and service semantics.
- Any claim that SHA-1 or truncated digests are a security control, password-hashing policy, or adversarial-collision defense.
- An open-ended algorithm plug-in matrix beyond the bounded ids and candidates approved by this story.

Open questions
- none

Follow-up questions
- If product wants out-of-box registrations beyond the caller-supplied service override path, which exact truncated SHA-256 sizes should ship first and which model or key surfaces may use them?
- If alternate algorithm ids later need persisted storage metadata outside the in-memory stable-hash surface, should that land as a separate storage and migration ticket rather than expanding this story?

Risks
- Broadening `StableHashDigest` is a public API behavior change and may break callers or tests that currently assume every digest is 64 lowercase hex characters.
- Without explicit wording, teams could confuse stable model and key hashing with persisted `content_hash` integrity rules and accidentally weaken storage expectations.
- Allowing SHA-1 or truncated digests without prominent caveats could be misread as a security recommendation instead of a bounded non-adversarial identity trade-off.

Split recommendations
- If implementation effort grows beyond the base contract and API widening, split built-in `sha1-v1` or truncated-SHA-256 registrations and their full compatibility-vector coverage into one or more follow-up tickets after the `sha256-v1`-compatible digest-contract change lands.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment