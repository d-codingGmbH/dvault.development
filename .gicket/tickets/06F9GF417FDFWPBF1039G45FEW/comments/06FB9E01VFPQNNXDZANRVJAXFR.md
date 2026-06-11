[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence shows AddDVault(options => ...) is the established advanced-configuration surface, but DataVaultOptions currently has no hashing-specific option; this story should extend that existing surface instead of introducing a parallel configuration entrypoint.
- docs/plans/stable-hashing-contract.md and src/DCoding.Data.DVault/StableHashDigest.cs already ratify the bounded built-in algorithm vocabulary and digest shapes: sha256-v1, sha1-v1, sha256-128-v1, and sha256-160-v1. The selector should stay limited to that finite set.
- Optionless AddDVault() and DefaultStableHashService already preserve sha256-v1 as the zero-configuration baseline; this story adds explicit opt-in built-in registrations and must not change the default path.
- DataVaultConventions.StableHashAlgorithmId is the existing public conventions surface that reports the stable-hash algorithm id, so an explicit built-in selection must align that value with the resolved IStableHashService and produced StableHashDigest values.
- The ticket description already contains the authoritative refinement contract; no child tickets, attachments, or planning documents were materialized in this pass.

Scope In
- Add a focused DataVaultOptions hashing-selection surface on AddDVault(options => ...) for the approved built-in stable-hash ids sha256-v1, sha1-v1, sha256-128-v1, and sha256-160-v1.
- Register deterministic built-in IStableHashService implementations for the approved ids while preserving sha256-v1 as the optionless default registration.
- Propagate the selected built-in algorithm id through the resolved IStableHashService, produced StableHashDigest values, and the public DataVaultConventions.StableHashAlgorithmId surface.
- Fail fast when the built-in selection surface receives an unsupported algorithm id instead of silently falling back or interpreting arbitrary custom ids.
- Add deterministic tests for the default path and each approved opt-in algorithm, including digest length, canonical lowercase hex, truncation behavior, and no-auto-enable registration behavior.
- Preserve the existing separation between stable model/key hashing and the persistence content_hash policy value sha-256.

Scope Out
- Reopening the already-completed algorithm-aware StableHashDigest validation contract from done task 06F9GF3TRG65G8MTMG7DH4PREC.
- Diagnostics, explain output, and support-bundle exposure of the selected algorithm, which remain follow-up work on ticket 06F9GF46KZYRKR1EGEPR3TV824.
- Release-note, README, and adoption-guidance work tracked separately by ticket 06F9GF4CRMXKEY2QT97W0S3GTR.
- Hash-key storage-profile, schema-shape, EF annotation, provider-capability, or migration-compatibility changes tracked separately by ticket 06F9GF5FV54DGWY9GA8ZEZWM5R.
- Automatic rehashing, backfill, parallel-key storage, dual-write compatibility lanes, or automatic migration of persisted hub keys, link keys, or hash diffs when callers opt into a different algorithm.
- An open-ended built-in selector for arbitrary caller-supplied algorithm ids; unsupported or custom algorithms remain caller-owned through direct IStableHashService registration.

Open questions
- none

Follow-up questions
- After the bounded built-in selector lands, does product want a first-class DataVaultOptions surface for caller-supplied custom IStableHashService registrations, or is direct dependency-injection override sufficient outside the approved built-in ids?
- Once the separate hash-key storage-profile contract lands, should non-default algorithm selection gain storage-compatibility diagnostics or gates before callers use shorter digests with persisted hub or link keys?

Risks
- Adding a public DataVaultOptions hashing selector and conventions alignment changes registration precedence behavior, so tests must explicitly lock down how the new selector interacts with the existing raw-DI override path.
- Opting into shorter digests before the separate storage-profile and diagnostics tickets land could create adopter confusion if ticket and API text do not keep those boundaries explicit.
- sha1-v1 and truncated SHA-256 remain bounded non-adversarial identity trade-offs and must not be framed as security, password-hashing, or compliance defaults.
- The stale incoming blocks relation is queued for owner-branch replay rather than already applied on this branch, so relation visibility may lag until replay completes even though the refinement contract is settled.

Split recommendations
- No child-ticket split is needed. Diagnostics/support-bundle exposure, documentation guidance, and storage-profile compatibility already exist as separate follow-up tickets.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 3
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment