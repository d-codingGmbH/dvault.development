[gicket-bot] PO refinement contract

Summary
- Refined the epic as an already-split tracking parent: the existing PIT/bridge boundary docs and live relations bound v1 scope, no new PO blockers were found, and no persistent planning writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the v1 boundary: PIT and bridge tables stay explicit, metadata-driven read models with caller-owned maintenance and no implicit scheduling or SaveChanges orchestration.
- This epic is already materially split through persisted parentOf relations to 06F5Q90KC6JGQPSP285XQYSPK8, 06F5Q90SX5AQ07M4PQKDR4BZD8, 06F5Q9102970H1VQN16QWRGQX0, 06F5Q916BXE2N372SWMH1X776G, 06F5Q91DR1555RSBQT7KDST684, and 06F5Q91M0PM17RP43ZQRPBDXP0.
- No description update, relation change, attachment, child-ticket creation, or planning-document write was materialized in this pass because docs/architecture/dvault-v1-pit-bridge-boundary.md and the referenced PIT plan docs already provide the authoritative refinement baseline.

Scope In
- Explicit PIT maintenance for one DataVaultPitMetadata declaration at a time, including full rebuild and bounded parent-targeted maintenance within the documented v1 boundary.
- PIT-backed as-of reads over explicit PIT metadata, including the documented hub-parent baseline plus the bounded multi-active and link-parent support already ratified by the architecture and plan documents.
- Explicit bridge completeness that belongs inside the library boundary: metadata-driven projection plus caller-invoked bridge maintenance and read-model behavior for supported many-to-many and hierarchy cases.
- Provider-neutral correctness, registry-backed maintenance resolution, and documentation/test coverage that keep SQLite as the only repository-proven optimized provider path.

Scope Out
- Background jobs, cron abstractions, dashboarding, deployment tooling, or any automatic PIT/bridge refresh triggered by reads, startup, or SaveChanges.
- Provider-specific PIT or bridge optimization beyond the current SQLite-proven path.
- Model-first link-parent PIT artifacts, registry-backed PIT as-of read requests, link-parent multi-active PITs, incompatible driving-key families, and cross-product tuple semantics.
- Broader orchestration or application-layer scheduling concerns that belong to consumers rather than the DVault EF Core library.

Open questions
- none

Follow-up questions
- When delivery approaches closure, should any provider-specific PIT/bridge optimization beyond SQLite be queued as separate backlog tickets rather than absorbed into this epic?
- If consumers later need PIT/bridge refresh orchestration, should that be planned as a separate application-layer integration epic outside the DVault library boundary?

Risks
- This epic currently has a live incoming blocks relation from 06F5Q90718D21DN1N1Q2AP7YEM.
- The bounded v1 stance on multi-active and link-parent PIT behavior can be destabilized if delivery tries to absorb cross-product tuple semantics or other deferred variants under this epic.
- Bridge completion can drift if delete-aware maintenance, advanced hierarchy semantics, or broader traversal features are implicitly added instead of being tracked as separate deferred work.

Split recommendations
- No additional split is recommended now; the epic already has six persisted child tickets linked by parentOf relations.
- If new asks emerge for provider-specific optimization or orchestration, create separate follow-up tickets instead of broadening this epic's v1 boundary.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment