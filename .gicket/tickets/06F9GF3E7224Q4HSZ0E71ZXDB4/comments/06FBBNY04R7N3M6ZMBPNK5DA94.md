[gicket-bot] PO refinement contract

Summary
- Epic scope is bounded by the stable-hashing contract, current built-in hash service baseline, and six persisted child tickets; no additional PO-level blockers remain before PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already ratifies the bounded v1 stable-hash algorithm set: sha256-v1 as the default, with explicit opt-in alternatives sha1-v1, sha256-128-v1, and sha256-160-v1.
- The epic remains a tracking parent. Persisted relations show six parentOf children: 06F9GF3MZHKQQ6D4SAQ0AMTKJR, 06F9GF3TRG65G8MTMG7DH4PREC, 06F9GF417FDFWPBF1039G45FEW, 06F9GF46KZYRKR1EGEPR3TV824, 06F9GF4CRMXKEY2QT97W0S3GTR, and 06F9GF5FV54DGWY9GA8ZEZWM5R.
- No new description update, relation cleanup, attachment write, or planning-document write was required in this turn because docs/plans/stable-hashing-contract.md and the existing ticket relations already provide the authoritative planning surface.

Scope In
- Make stable hash algorithm selection a first-class DVault capability while keeping sha256-v1 as the zero-configuration default.
- Support the bounded v1 non-default algorithm ids already documented in the repository for deliberate caller opt-in.
- Carry stable-hash algorithm identity and deterministic behavior through the service, EF Core/modeling, diagnostics, and documentation surfaces covered by this epic's child tickets.
- Preserve deterministic normalization, canonical lowercase hexadecimal digest behavior, and provider-neutral semantics across repeated runs and machines.

Scope Out
- Automatic migration, rehashing, backfill, or reconciliation of previously persisted hub keys, link keys, hash diffs, or other stored hashes.
- Key rotation, secret management, cryptographic compliance positioning, or any security-specific hashing policy.
- Changing the persistence content_hash storage contract away from its fixed SHA-256 storage-integrity semantics.
- Database-side hashing, provider-specific orchestration, or making non-default algorithms implicit in AddDVault().

Open questions
- none

Follow-up questions
- After v1 delivery, should DVault publish separate operator guidance for callers that intentionally replace the stable hash service after hashes have already been persisted?

Risks
- Epic completion is still operationally gated by child ticket 06F9GF4CRMXKEY2QT97W0S3GTR, which currently has a persisted blocks relation against this epic.
- Shorter non-default digests reduce key width at the cost of a weaker collision profile; documentation and diagnostics need to keep those algorithms framed as non-default deterministic identity trade-offs, not security controls.
- If later work expands beyond the bounded v1 algorithm set, compatibility and migration pressure will grow because persisted stable-hash values are caller-owned once stored.

Split recommendations
- No additional split is required in this turn; the epic is already decomposed through six persisted parentOf child tickets and should continue as a tracking parent.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment