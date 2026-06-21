[gicket-bot] PO refinement contract

Summary
- Refined the ticket into a bounded adopter-documentation task anchored on the checked-in hash-key storage contract, current binary-first adoption guidance, and SQLite-scoped footprint evidence; no child-ticket, relation, attachment, or planning-document writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Ticket comments and closure-evidence context add no extra scope: there are no human comments and no closure evidence amendments to account for.
- Repository evidence fixes the v1 baseline: logical hash keys stay canonical lowercase hexadecimal strings while physical storage may be HexString or explicit opt-in Binary, so the guide should document migration and adoption around that boundary rather than redefine the storage contract.
- The guide should treat the support-bundle and translated metadata or live-schema facts as the authoritative preflight baseline for storage-profile, algorithm-id, digest-length, store-type, value-format, and conversion-behavior compatibility checks.
- Built-in stable-hash examples should stay bounded to the visible v1 algorithm ids sha256-v1, sha1-v1, sha256-128-v1, and sha256-160-v1, including the documented same-width incompatibility case.
- Current relation context was verified and left unchanged: this ticket still blocks 06FE4R0TBG8JP5WA2SHXKH438M and remains blocked and related by 06FE4R089MT3BYRCVH7Q4EX6CG.

Scope In
- Document an adopter-owned path for moving existing persisted hash-key storage from hex-string physical columns to binary physical columns.
- Document preflight validation inputs, compatibility checks, execution sequencing, rollback expectations, and provider caveats for that change.
- Document how binary-first guidance for new schemas or projects differs from reviewed migration planning for existing persisted databases.

Scope Out
- Automatic migration execution, backfill, dual-write, repair, reconcile, or rehash tooling.
- Changing caller-facing hash-key value types away from canonical lowercase hexadecimal strings.
- Provider-side SQL hashing or broader stable-hash governance changes beyond documenting the current bounded baseline.

Open questions
- none

Follow-up questions
- Should a later follow-up add provider-specific migration examples or evidence bundles for PostgreSQL or SQL Server once equivalent checked-in benchmarks or validation artifacts exist?
- Should release notes or package-compatibility docs add an explicit link to the new guide after it lands, beyond the adoption and checklist cross-links needed for this ticket?

Risks
- Current quantified footprint evidence is SQLite-only, so overly broad provider performance or storage claims would create documentation drift.
- If downstream implementation tickets change the exact support-bundle or validation surface names, this guide will need a final terminology pass before release.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment