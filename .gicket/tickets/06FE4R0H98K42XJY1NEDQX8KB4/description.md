<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket into a bounded adopter-documentation task anchored on the checked-in hash-key storage contract, current binary-first adoption guidance, and SQLite-scoped footprint evidence; no child-ticket, relation, attachment, or planning-document writes were needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Ticket comments and closure-evidence context add no extra scope: there are no human comments and no closure evidence amendments to account for.
- Repository evidence fixes the v1 baseline: logical hash keys stay canonical lowercase hexadecimal strings while physical storage may be HexString or explicit opt-in Binary, so the guide should document migration and adoption around that boundary rather than redefine the storage contract.
- The guide should treat the support-bundle and translated metadata or live-schema facts as the authoritative preflight baseline for storage-profile, algorithm-id, digest-length, store-type, value-format, and conversion-behavior compatibility checks.
- Built-in stable-hash examples should stay bounded to the visible v1 algorithm ids sha256-v1, sha1-v1, sha256-128-v1, and sha256-160-v1, including the documented same-width incompatibility case.
- Current relation context was verified and left unchanged: this ticket still blocks 06FE4R0TBG8JP5WA2SHXKH438M and remains blocked and related by 06FE4R089MT3BYRCVH7Q4EX6CG.

### Scope In
- Document an adopter-owned path for moving existing persisted hash-key storage from hex-string physical columns to binary physical columns.
- Document preflight validation inputs, compatibility checks, execution sequencing, rollback expectations, and provider caveats for that change.
- Document how binary-first guidance for new schemas or projects differs from reviewed migration planning for existing persisted databases.

### Scope Out
- Automatic migration execution, backfill, dual-write, repair, reconcile, or rehash tooling.
- Changing caller-facing hash-key value types away from canonical lowercase hexadecimal strings.
- Provider-side SQL hashing or broader stable-hash governance changes beyond documenting the current bounded baseline.

## Acceptance Criteria
- A checked-in guide explains that Binary is an explicit opt-in physical storage profile, HexString remains the compatible default, and public, request, and diagnostic hash-key values stay lowercase hexadecimal strings.
- The guide defines a preflight checklist that uses the support-bundle or equivalent translated metadata baseline to compare storage profile, stable-hash algorithm id, digest byte length, provider store type, provider value format, and conversion behavior before any migration step.
- The guide defines a caller-owned execution and rollback sequence for moving existing persisted data from hex to binary storage, including fail-closed handling when persisted compatibility facts drift or when algorithm changes are mixed into the same change.
- The guide states provider caveats using only checked-in evidence: the built-in provider profile baseline is finite, SQLite footprint evidence is the current quantified storage example, and broader provider-specific savings or performance claims are not promised.
- The guide is discoverable from the current adoption documentation path through cross-links from existing checked-in entry points.

## Definition of Done
- The migration and validation guide is checked into the docs set with terminology consistent with the hash-key storage contract and current getting-started or adoption docs.
- The guide includes concrete validation and rollback checkpoints a consumer can follow before, during, and after cutover without implying automatic DVault migration support.
- Relevant existing documentation entry points are updated or linked so the guide is discoverable from the current adoption path.
- Any examples, tables, or caveats in the guide stay aligned with the visible v1 algorithms, storage profiles, and SQLite evidence bundle.

## Implementation Notes
- Anchor terminology and compatibility facts on docs/plans/hash-key-storage-profile-contract.md: HexString default, Binary explicit opt-in, logical boundary stays lowercase hex text, and persisted compatibility changes fail closed.
- Reuse the current checked-in adoption language from docs/getting-started.md and docs/production-adoption-checklist.md: binary-first is for new schemas or new projects and does not automatically migrate existing persisted storage.
- Use the visible BuiltInStableHashService algorithm ids and the contract's digest-size table when explaining why equal column widths do not prove compatibility.
- Treat hash-key-footprint.md as SQLite-local evidence only; if the guide mentions storage savings or performance shape, keep those claims explicitly scoped to that bundle.
- Preserve the provider-caveat posture from the contract, including that not every provider has the same live-schema evidence surface; do not promise provider-specific automation that is not already documented.
- A reasonable implementation shape is a dedicated adopter guide under docs plus cross-links from the existing adoption and checklist entry points rather than burying the migration detail only in release notes.

## Open Questions
- none

## Follow-Up Questions
- Should a later follow-up add provider-specific migration examples or evidence bundles for PostgreSQL or SQL Server once equivalent checked-in benchmarks or validation artifacts exist?
- Should release notes or package-compatibility docs add an explicit link to the new guide after it lands, beyond the adoption and checklist cross-links needed for this ticket?

## Risks
- Current quantified footprint evidence is SQLite-only, so overly broad provider performance or storage claims would create documentation drift.
- If downstream implementation tickets change the exact support-bundle or validation surface names, this guide will need a final terminology pass before release.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: document how consumers can plan, validate, and execute their own hex-to-binary hash storage adoption. Acceptance: guidance covers rollback, compatibility checks, and provider caveats without automatic migration execution.