[gicket-bot] PO refinement contract

Summary
- Repository evidence already fixes the current boundary: all existing save paths, including provider-specific strategies, normalize and hash in .NET through the shared stable-hash services, so this ticket should document database-side hashing only as a future provider-gated, separately evidenced contract; no child tickets, planning documents, attachments, or relation writes were materialized in this refinement run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 baseline is already ratified by repository docs: docs/plans/stable-hashing-contract.md owns sha256-v1 stable-hash normalization and compatibility vectors, and docs/plans/dvault-v1-default-persistence-convention-policy.md owns the logical sha-256 content-hash tuple.
- Current provider-neutral and provider-optimized save paths all compute hub and link hashes through IStableHashNormalizer and IStableHashService; repository evidence does not show any provider package using database hash functions today.
- docs/plans/optional-advanced-configuration-hooks.md already reserves provider-accelerated hashing for a separate versioned contract rather than an implicit default-path change.
- No child tickets, description updates, attachments, or planning documents were materialized in this refinement run; the outgoing block to 06F5Q93H60W6X8FJ88PWTR6NG4 remains consistent with the current ticket graph.

Scope In
- Document the current compatibility boundary: canonical normalization and digest computation stay on the .NET side for hub and link hash-key generation, including provider-optimized save strategies.
- Define the minimum evidence gate for any future provider-side hashing path: deterministic parity with published vectors and canonicalization rules, explicit provider gating, decline or fallback behavior, and benchmark evidence under matched inputs.
- Cross-reference the existing stable-hashing, persistence-convention, and performance-evidence contracts as the required sources of truth for any future provider-side proposal.
- Explain that this ticket is documentation and governance work only and does not introduce runtime hashing behavior.

Scope Out
- Implementing database-side hashing in PostgreSQL, MySQL, SQL Server, Oracle, SQLite, or the provider-neutral fallback writer.
- Changing sha256-v1, sha-256, canonicalization identifiers, hash tuple semantics, or the current DI-resolved IStableHashService and IStableHashNormalizer boundary.
- Algorithm migration, simultaneous hash-version support, persisted hash backfill, or automatic satellite hash-diff generation.
- Broader release-note and README rollup already tracked by 06F5Q93H60W6X8FJ88PWTR6NG4.

Open questions
- none

Follow-up questions
- If a provider later pursues database-side hashing, should release-facing docs mention it only after one provider has cleared the full evidence gate, or can it remain a planning-only boundary until then?
- Should future provider-side hashing proposals require one provider-specific ticket per provider even when two providers share similar SQL capabilities, to keep compatibility and benchmark evidence independently auditable?

Risks
- If later documentation skips the shared manifest, vectors, or benchmark contract, teams could mistake provider-local hash behavior for proven compatibility.
- Downstream documentation rollup ticket 06F5Q93H60W6X8FJ88PWTR6NG4 remains blocked until this boundary text is landed.

Split recommendations
- If work ever moves beyond documentation, split it into one shared contract or governance ticket plus separate provider-specific evidence or implementation tickets; do not widen this ticket into multi-provider runtime hashing work.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment