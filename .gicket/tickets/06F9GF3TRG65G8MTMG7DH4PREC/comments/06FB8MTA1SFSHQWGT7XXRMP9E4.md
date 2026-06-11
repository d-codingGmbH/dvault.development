[gicket-bot] PO refinement contract

Summary
- Refinement ratified algorithm-aware StableHashDigest validation around a bounded v1 algorithm set, preserved sha256-v1 compatibility, applied the current ticket description update, and queued cleanup of one stale historical blocks relation.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Applied a ticket description update on 06F9GF3TRG65G8MTMG7DH4PREC that makes docs/plans/stable-hashing-contract.md the authoritative boundary for this task: sha256-v1 stays the default 32-byte/64-lowercase-hex digest, sha1-v1 is 20 bytes/40 hex, sha256-128-v1 is 16 bytes/32 hex, sha256-160-v1 is 20 bytes/40 hex, and unknown caller-supplied algorithm ids are accepted only for whole-byte lowercase hex.
- Repository evidence in src/DCoding.Data.DVault/StableHashDigest.cs and tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs already matches that bounded direction: DigestByteLength is derived from Value length, known ids enforce algorithm-specific lengths, custom ids are lower-hex whole-byte only, and published sha256-v1 vectors remain fixed.
- No child tickets, attachments, or new planning documents were materialized for this refinement pass.
- Queued removal of stale relation 06F9GF3MZHKQQ6D4SAQ0AMTKJR--06F9GF3TRG65G8MTMG7DH4PREC--blocks as outbox mutation-ee8323dd972bfc8a so the historical story no longer blocks this task once replay runs on the source ticket owner branch.

Scope In
- Relax StableHashDigest and directly related validation logic so digest shape is algorithm-aware instead of universally fixed to 64 lowercase hex characters.
- Preserve the public StableHashDigest metadata surface needed by callers: AlgorithmId, Value, and DigestByteLength.
- Keep the default IStableHashService/AddDVault behavior compatible with sha256-v1 and its published test vectors.
- Add or maintain regression tests for valid canonical lowercase hex, invalid lengths, invalid non-lowercase or non-whole-byte values, and existing sha256-v1 vectors.

Scope Out
- Changing the default stable hash algorithm away from sha256-v1.
- Automatically registering non-default algorithm ids in AddDVault().
- Changing DataVaultConventions persistence content-hash policy or content_hash storage semantics.
- Automatic migration, rehashing, or backfill of persisted hub keys, link keys, hash diffs, or other stored stable-hash values.

Open questions
- none

Follow-up questions
- If product later wants non-default stable-hash ids to be used for persisted hub/link keys rather than only supported by the digest boundary, should that be handled in a separate compatibility ticket covering storage, migration, and provider/integration expectations?
- Should a future API or documentation ticket expose the bounded known-algorithm digest-length table more directly for consumers that want pre-validation guidance before constructing StableHashDigest?

Risks
- Other repository areas and downstream consumers still commonly assume default 64-character hash keys; this task intentionally preserves sha256-v1 as the default, so broader algorithm-substitution compatibility remains separate work.
- Accepting unknown custom algorithm ids is deliberate but caller-owned, so documentation and reviews must avoid implying those ids are a DVault cryptographic approval or compliance policy.
- The stale blocks relation cleanup is queued for replay on another ticket owner branch, so relation views may temporarily continue to show the historical block until outbox mutation-ee8323dd972bfc8a replays.

Split recommendations
- No child-ticket split is needed for this task; the current scope is already bounded to StableHashDigest validation behavior, preserved sha256-v1 compatibility, and regression coverage.

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