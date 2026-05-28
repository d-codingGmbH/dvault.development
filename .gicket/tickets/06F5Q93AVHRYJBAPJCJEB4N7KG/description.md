<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository evidence already fixes the current boundary: all existing save paths, including provider-specific strategies, normalize and hash in .NET through the shared stable-hash services, so this ticket should document database-side hashing only as a future provider-gated, separately evidenced contract; no child tickets, planning documents, attachments, or relation writes were materialized in this refinement run.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 baseline is already ratified by repository docs: docs/plans/stable-hashing-contract.md owns sha256-v1 stable-hash normalization and compatibility vectors, and docs/plans/dvault-v1-default-persistence-convention-policy.md owns the logical sha-256 content-hash tuple.
- Current provider-neutral and provider-optimized save paths all compute hub and link hashes through IStableHashNormalizer and IStableHashService; repository evidence does not show any provider package using database hash functions today.
- docs/plans/optional-advanced-configuration-hooks.md already reserves provider-accelerated hashing for a separate versioned contract rather than an implicit default-path change.
- No child tickets, description updates, attachments, or planning documents were materialized in this refinement run; the outgoing block to 06F5Q93H60W6X8FJ88PWTR6NG4 remains consistent with the current ticket graph.

### Scope In
- Document the current compatibility boundary: canonical normalization and digest computation stay on the .NET side for hub and link hash-key generation, including provider-optimized save strategies.
- Define the minimum evidence gate for any future provider-side hashing path: deterministic parity with published vectors and canonicalization rules, explicit provider gating, decline or fallback behavior, and benchmark evidence under matched inputs.
- Cross-reference the existing stable-hashing, persistence-convention, and performance-evidence contracts as the required sources of truth for any future provider-side proposal.
- Explain that this ticket is documentation and governance work only and does not introduce runtime hashing behavior.

### Scope Out
- Implementing database-side hashing in PostgreSQL, MySQL, SQL Server, Oracle, SQLite, or the provider-neutral fallback writer.
- Changing sha256-v1, sha-256, canonicalization identifiers, hash tuple semantics, or the current DI-resolved IStableHashService and IStableHashNormalizer boundary.
- Algorithm migration, simultaneous hash-version support, persisted hash backfill, or automatic satellite hash-diff generation.
- Broader release-note and README rollup already tracked by 06F5Q93H60W6X8FJ88PWTR6NG4.

## Acceptance Criteria
- The documentation states that current DVault compatibility is defined by .NET-side canonical normalization and hashing, and it explains that this remains true for both the provider-neutral writer and today's provider-optimized save strategies.
- The documentation identifies the mandatory source-of-truth contracts for any future provider-side hashing work: docs/plans/stable-hashing-contract.md, docs/plans/dvault-v1-default-persistence-convention-policy.md, and docs/plans/performance-evidence-benchmark-artifact-contract.md.
- The documentation defines the minimum admission evidence before any provider may offer database-side hashing: provider-specific deterministic equivalence tests against published vectors and canonicalization rules, explicit opt-in or provider-gated selection with safe decline or fallback semantics, and benchmark artifacts collected under matched run inputs.
- The documentation explicitly says this ticket does not add runtime database-side hashing behavior or make provider-specific hashing the default path.

## Definition of Done
- An authoritative repository document or focused update on the ticket branch records the future database-side hashing boundary and keeps .NET-side hashing as the current default contract.
- The text makes clear that a future provider-side path may only preserve existing semantics, never silently replace them, and must use a separate documented contract and evidence gate before release claims are made.
- The deliverable reuses the published stable-hash vectors and the shared benchmark artifact contract instead of inventing ticket-specific compatibility or performance formats.
- The resulting contract leaves no PO-level ambiguity about what evidence, fallback posture, and non-goals apply before downstream documentation ticket 06F5Q93H60W6X8FJ88PWTR6NG4 consumes this boundary.

## Implementation Notes
- Repository evidence fixes the defaults in code: DataVaultConventions.Default exposes sha256-v1 for stable hashes and sha-256 for logical persistence content hashes.
- IDataVaultProviderBehavior already states that provider behavior must not change DVault hashing semantics.
- docs/architecture/dvault-v1-explicit-save-service.md says provider save strategies receive the shared stable-hash service and normalizer; current provider packages compute hashes through those services instead of provider SQL hash functions.
- docs/plans/optional-advanced-configuration-hooks.md already says provider-accelerated hashing requires a separate versioned contract; this ticket should expand that boundary rather than reopen the baseline.
- Use 06F5Q934MSKVCQAHPCWEM29CZW as landed context for the canonicalization manifest and compatibility vectors, not as a blocker; that related ticket is already done.
- The benchmark evidence contract already defines the reusable benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json artifact set and matched-input rule for future provider-side performance claims.

## Open Questions
- none

## Follow-Up Questions
- If a provider later pursues database-side hashing, should release-facing docs mention it only after one provider has cleared the full evidence gate, or can it remain a planning-only boundary until then?
- Should future provider-side hashing proposals require one provider-specific ticket per provider even when two providers share similar SQL capabilities, to keep compatibility and benchmark evidence independently auditable?

## Risks
- If later documentation skips the shared manifest, vectors, or benchmark contract, teams could mistake provider-local hash behavior for proven compatibility.
- Downstream documentation rollup ticket 06F5Q93H60W6X8FJ88PWTR6NG4 remains blocked until this boundary text is landed.

## Split Recommendations
- If work ever moves beyond documentation, split it into one shared contract or governance ticket plus separate provider-specific evidence or implementation tickets; do not widen this ticket into multi-provider runtime hashing work.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Evaluate database-side hashing as a future provider-gated escape hatch without implementing it as a default path.

Acceptance criteria:
- Documents why .NET-side canonical hashing remains the default compatibility contract.
- Defines evidence required before any provider may offer database-side hashing.
- Avoids adding runtime database-side hashing behavior in this ticket.