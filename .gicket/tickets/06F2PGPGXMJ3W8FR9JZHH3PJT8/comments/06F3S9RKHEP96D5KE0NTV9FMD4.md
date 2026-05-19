[gicket-bot] PO refinement contract

Summary
- Resolved the remaining PO-critic gap by defining shortest-path hierarchy depth semantics for bridge maintenance, requiring incremental maintenance to lower stored depth when a newly ingested shorter path appears, and promoting that rule into acceptance and test expectations. No child tickets, relation changes, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Hierarchy maintenance persists exactly one row per distinct ancestor/descendant pair because the projected hierarchy bridge key is the pair itself. When multiple recursive paths produce the same pair, the persisted TraversalDepth is the minimum positive hop count across all currently materialized paths. Direct edges persist as depth 1, and alternate paths of equal or greater length do not create extra rows.
- critic-item-2: `answered` - Incremental hierarchy maintenance is addition-oriented after source-link ingestion. If newly ingested recursive-link data creates a shorter path for an already materialized ancestor/descendant pair, maintenance must update the stored TraversalDepth downward to that new minimum. If the new path is equal or longer, the existing row remains unchanged. If callers remove links or otherwise need rows removed or depths increased, full rebuild is the authoritative recovery path for this story.
- critic-item-3: `answered` - The contract now promotes the shortest-path rule into acceptance and test expectations: hierarchy rebuild must persist one row per pair with the minimum positive TraversalDepth, incremental maintenance must lower an existing row when a shorter path appears, and automated coverage must prove duplicate-path suppression, shorter-path updates, equal-or-longer no-ops, idempotence, registry-backed resolution, and SQLite integration.
- critic-item-4: `answered` - Resolved by defining the authoritative hierarchy rule as shortest positive path depth per persisted ancestor/descendant pair. That removes the prior ambiguity between shortest-path, first-seen, unsupported-duplicate, or other semantics for the single-row bridge shape.
- critic-item-5: `answered` - Resolved by requiring incremental maintenance to reduce stored TraversalDepth when later ingestion discovers a shorter alternate path, while leaving equal or longer alternates unchanged and using full rebuild for destructive topology changes. That makes idempotence and maximumDepth semantics stable enough for the blocked query API and documentation follow-ons.

Clarifications
- The story remains under epic 06F2PGP7HM8F39K3J0H5JHB3B4 and release v0.15.0, and it still blocks 06F2PGPKXWRFXNPFA1JR0X67XC and 06F2PGPXVAYRBC94RQ7X5V4DVG until a stable bridge-maintenance baseline ships.
- The existing blocks relation from done epic 06F2PGMFWSEC95ATBCGZ6HYT5W remains historical release ordering only and is not an active PO blocker.
- Current repository evidence remains read-only for bridges: README.md, docs/releases/v0.7.0.md, and docs/production-adoption-checklist.md all describe bridge reads as consuming already materialized tables without maintaining them.
- Hierarchy bridge maintenance persists exactly one row per distinct ancestor/descendant pair, with TraversalDepth equal to the shortest positive recursive-link path currently materialized for that pair. Direct edges persist as depth 1, equal-depth or longer alternate paths do not create duplicate rows, and the contract still does not add implicit self rows.
- Incremental hierarchy maintenance is defined for source-link ingestion adds. If later ingestion creates a shorter alternate path, the existing row must be updated to the new minimum TraversalDepth; otherwise the existing row remains unchanged. Full rebuild remains the authoritative path when callers need removals or increased depths after destructive topology changes.
- No child tickets, relation changes, or planning documents were materialized during this refinement pass.

Scope In
- Add an explicit provider-neutral bridge-maintenance service in the core DVault package, additive beside IDataVaultSaveService and IDataVaultReadService rather than hidden behind EF tracking, bridge reads, or SaveChanges interception.
- Cover the shipped bridge metadata baseline only: many-to-many bridges and hierarchy bridges declared through DataVaultBridgeMetadata and projected through the existing bridge tables/entities.
- Define both full rebuild behavior and incremental maintenance behavior for one bridge at a time so callers can either recompute a bridge from persisted source-link rows or maintain newly affected bridge rows after source-link ingestion.
- Support both explicit metadata requests and registry-backed resolution so callers using UseDataVaultMetadata() can maintain a bridge by logical name without re-declaring metadata.
- Keep existing bridge read APIs compatible; the maintenance story materializes rows those APIs can consume and does not redesign bridge query projection ergonomics.
- For many-to-many bridges, recompute or maintain exactly one row per distinct endpoint pair required by the bridge metadata.
- For hierarchy bridges, recompute or maintain exactly one row per distinct ancestor/descendant pair with shortest positive TraversalDepth semantics and direct-edge depth 1 as the v1 default.

Scope Out
- PIT maintenance behavior; that remains sibling story 06F2PGPBRFT48JG57SV57N9TVW.
- Provider-specific bridge or PIT read optimization; that remains sibling story 06F2PGPRGN0EVGD6RY5KY9M56W.
- Broader current and as-of query API redesign; that remains blocked story 06F2PGPKXWRFXNPFA1JR0X67XC.
- Advanced bridge projection features already deferred in repository evidence, including effectivity windows, path payload columns, closure-maintenance state columns, generated relationship graphs, PIT interactions, and multi-active interactions.
- Automatic scheduler or trigger behavior, background orchestration, or implicit maintenance during ordinary reads or saves.
- Provider-specific bulk SQL, physical tuning, or benchmark claims beyond the provider-neutral baseline.
- Multi-bridge batch orchestration; keep the v1 maintenance contract bounded to one bridge per request.
- Delete-aware or topology-shrinking incremental hierarchy maintenance that would need row removal or increased TraversalDepth without using the full rebuild path.

Open questions
- none

Follow-up questions
- After the provider-neutral baseline ships, does the release need a separate follow-on for provider-specific bridge-maintenance performance paths, or is the existing read-optimization story sufficient for the first adopter wave?
- Should broader adopter guidance document a recommended loader orchestration pattern between IDataVaultSaveService and bridge maintenance for batch link ingestion, or is the minimal README and release-note delta enough for v0.15.0?
- If adopters later need delete-aware incremental hierarchy closure maintenance instead of the documented full-rebuild fallback, should that ship as a separate follow-up ticket after the v0.15.0 baseline lands?

Risks
- Hierarchy bridge maintenance can expand quickly on large recursive link sets, so this story should stay correctness-first and provider-neutral before any specialization work.
- Because current bridge tables do not carry effectivity, path payload, or closure-state columns and persist only one TraversalDepth per ancestor/descendant pair, the implementation must avoid inventing advanced semantics beyond the minimum-hop closure rule defined here.
- Incremental shortest-path updates must stay idempotent and converge with full rebuild; otherwise the blocked query-API and documentation follow-ons would inherit unstable maximumDepth semantics.

Split recommendations
- No split recommended; sibling tickets already isolate PIT maintenance, query API work, provider-aware optimization, and v0.15.0 documentation.
- If delete-aware incremental hierarchy maintenance is later required beyond the rebuild fallback, track it as a separate follow-up rather than widening this story's v1 baseline.

Persisted contract coverage
- acceptance-criteria items: 9
- definition-of-done items: 4
- implementation-notes items: 8

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment