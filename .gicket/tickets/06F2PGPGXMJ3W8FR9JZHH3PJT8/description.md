<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Resolved the remaining PO-critic gap by defining shortest-path hierarchy depth semantics for bridge maintenance, requiring incremental maintenance to lower stored depth when a newly ingested shorter path appears, and promoting that rule into acceptance and test expectations. No child tickets, relation changes, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The story remains under epic 06F2PGP7HM8F39K3J0H5JHB3B4 and release v0.15.0, and it still blocks 06F2PGPKXWRFXNPFA1JR0X67XC and 06F2PGPXVAYRBC94RQ7X5V4DVG until a stable bridge-maintenance baseline ships.
- The existing blocks relation from done epic 06F2PGMFWSEC95ATBCGZ6HYT5W remains historical release ordering only and is not an active PO blocker.
- Current repository evidence remains read-only for bridges: README.md, docs/releases/v0.7.0.md, and docs/production-adoption-checklist.md all describe bridge reads as consuming already materialized tables without maintaining them.
- Hierarchy bridge maintenance persists exactly one row per distinct ancestor/descendant pair, with TraversalDepth equal to the shortest positive recursive-link path currently materialized for that pair. Direct edges persist as depth 1, equal-depth or longer alternate paths do not create duplicate rows, and the contract still does not add implicit self rows.
- Incremental hierarchy maintenance is defined for source-link ingestion adds. If later ingestion creates a shorter alternate path, the existing row must be updated to the new minimum TraversalDepth; otherwise the existing row remains unchanged. Full rebuild remains the authoritative path when callers need removals or increased depths after destructive topology changes.
- No child tickets, relation changes, or planning documents were materialized during this refinement pass.

### Scope In
- Add an explicit provider-neutral bridge-maintenance service in the core DVault package, additive beside IDataVaultSaveService and IDataVaultReadService rather than hidden behind EF tracking, bridge reads, or SaveChanges interception.
- Cover the shipped bridge metadata baseline only: many-to-many bridges and hierarchy bridges declared through DataVaultBridgeMetadata and projected through the existing bridge tables/entities.
- Define both full rebuild behavior and incremental maintenance behavior for one bridge at a time so callers can either recompute a bridge from persisted source-link rows or maintain newly affected bridge rows after source-link ingestion.
- Support both explicit metadata requests and registry-backed resolution so callers using UseDataVaultMetadata() can maintain a bridge by logical name without re-declaring metadata.
- Keep existing bridge read APIs compatible; the maintenance story materializes rows those APIs can consume and does not redesign bridge query projection ergonomics.
- For many-to-many bridges, recompute or maintain exactly one row per distinct endpoint pair required by the bridge metadata.
- For hierarchy bridges, recompute or maintain exactly one row per distinct ancestor/descendant pair with shortest positive TraversalDepth semantics and direct-edge depth 1 as the v1 default.

### Scope Out
- PIT maintenance behavior; that remains sibling story 06F2PGPBRFT48JG57SV57N9TVW.
- Provider-specific bridge or PIT read optimization; that remains sibling story 06F2PGPRGN0EVGD6RY5KY9M56W.
- Broader current and as-of query API redesign; that remains blocked story 06F2PGPKXWRFXNPFA1JR0X67XC.
- Advanced bridge projection features already deferred in repository evidence, including effectivity windows, path payload columns, closure-maintenance state columns, generated relationship graphs, PIT interactions, and multi-active interactions.
- Automatic scheduler or trigger behavior, background orchestration, or implicit maintenance during ordinary reads or saves.
- Provider-specific bulk SQL, physical tuning, or benchmark claims beyond the provider-neutral baseline.
- Multi-bridge batch orchestration; keep the v1 maintenance contract bounded to one bridge per request.
- Delete-aware or topology-shrinking incremental hierarchy maintenance that would need row removal or increased TraversalDepth without using the full rebuild path.

## Acceptance Criteria
- A new explicit public bridge-maintenance surface is added to DCoding.Data.DVault and registered through the normal AddDVault startup path, with naming and request patterns consistent with the existing explicit save and read services.
- Full rebuild over a many-to-many bridge recomputes the bridge table from persisted source-link rows and leaves exactly one row per distinct endpoint pair required by the bridge metadata.
- Full rebuild over a hierarchy bridge recomputes ancestor/descendant closure rows from persisted recursive link rows, persists exactly one row per distinct ancestor/descendant pair, stores positive integer TraversalDepth values equal to the minimum hop count across all currently materialized paths for that pair, treats direct edges as depth 1, and does not introduce effectivity or path-payload semantics.
- Incremental bridge maintenance can add missing bridge rows for newly relevant source-link data without requiring a full rebuild. For hierarchy bridges, when later source-link ingestion creates a shorter alternate path for an existing pair, maintenance updates the persisted TraversalDepth to that shorter minimum; equal or longer alternate paths do not change the stored row.
- Repeated rebuild or incremental execution over the same additive source state is idempotent, and rebuild and incremental maintenance converge to identical bridge contents for the same persisted source-link state.
- Registry-backed callers can invoke bridge maintenance against the authoritative metadata registry by bridge name, with deterministic failure when the bridge metadata is missing or unsupported.
- Existing bridge read APIs continue to work against maintained tables without API regression, and public API snapshot coverage is updated for any new public maintenance types.
- Tests cover many-to-many and hierarchy rebuild and incremental flows, duplicate suppression, shortest-depth selection when multiple hierarchy paths reach the same pair, shorter-path updates, equal-or-longer-path no-ops, registry-backed resolution, and at least one SQLite integration path that proves bridge rows no longer require manual seeding by application code alone.
- README and the v0.15.0 release-note delta are updated to replace the current read-only bridge limitation with the new explicit caller-invoked maintenance baseline while documenting the minimum-hop TraversalDepth rule for hierarchy bridges.

## Definition of Done
- Core package code, DI registration, and public API snapshots are updated for the bridge-maintenance surface.
- Unit and SQLite integration tests pass for both bridge kinds and both maintenance modes, including duplicate-path shortest-depth coverage and shorter-path incremental update coverage for hierarchy bridges.
- Repository documentation reflects the new explicit bridge-maintenance baseline, documents minimum-hop TraversalDepth semantics for hierarchy bridges, and no longer implies that bridge population is only manual once the service exists.
- The implementation leaves sibling PIT maintenance, query-API follow-up, provider-specific optimization, and broader adopter documentation scopes untouched except for required compatibility or handoff notes.

## Implementation Notes
- Follow the existing explicit-service architecture: a dedicated bridge-maintenance service with request and result types is the bounded default, while registry-backed adapters should mirror the save and read pattern already used by DataVaultSaveServiceRegistryExtensions and DataVaultReadServiceRegistryExtensions.
- Use the existing bridge metadata and table baseline as authoritative input: many-to-many bridges project only endpoint hash keys, hierarchy bridges project ancestor and descendant hash keys plus TraversalDepth, and unsupported ProjectionFeatures remain out of scope for this story.
- Because the hierarchy bridge projection stores only one keyed row per ancestor/descendant pair and downstream reads filter on persisted TraversalDepth through maximumDepth, maintenance must treat the shortest positive path length as the authoritative stored depth for that pair.
- Incremental maintenance is addition-oriented after source-link ingestion: insert missing pairs, lower stored TraversalDepth when a shorter path appears, ignore equal or longer alternate paths, and require callers to use full rebuild when deletions or topology shrinkage would require row removal or increased depth.
- Current bridge read tests manually insert rows into BridgeCustomerOrder and BridgeSalesRegionHierarchy; add maintenance-specific tests rather than weakening those existing read-contract tests.
- Bridge maintenance should read from persisted source-link state and write bridge tables only; it must not change hub, link, or satellite save semantics, hash calculation, or the existing explicit IDataVaultSaveService boundary.
- Keep the baseline provider-neutral. If implementation internally uses deletes and reinserts for rebuild, the externally visible contract is simply that the maintained bridge contents match current persisted source-link state after the call completes.
- The blocked query-API and documentation tickets should be able to consume this story's shipped surface without reopening bridge semantics, so document minimum-hop TraversalDepth, direct-edge depth 1, no implicit self-row contract, and caller-invoked maintenance as the v1 defaults.

## Open Questions
- none

## Follow-Up Questions
- After the provider-neutral baseline ships, does the release need a separate follow-on for provider-specific bridge-maintenance performance paths, or is the existing read-optimization story sufficient for the first adopter wave?
- Should broader adopter guidance document a recommended loader orchestration pattern between IDataVaultSaveService and bridge maintenance for batch link ingestion, or is the minimal README and release-note delta enough for v0.15.0?
- If adopters later need delete-aware incremental hierarchy closure maintenance instead of the documented full-rebuild fallback, should that ship as a separate follow-up ticket after the v0.15.0 baseline lands?

## Risks
- Hierarchy bridge maintenance can expand quickly on large recursive link sets, so this story should stay correctness-first and provider-neutral before any specialization work.
- Because current bridge tables do not carry effectivity, path payload, or closure-state columns and persist only one TraversalDepth per ancestor/descendant pair, the implementation must avoid inventing advanced semantics beyond the minimum-hop closure rule defined here.
- Incremental shortest-path updates must stay idempotent and converge with full rebuild; otherwise the blocked query-API and documentation follow-ons would inherit unstable maximumDepth semantics.

## Split Recommendations
- No split recommended; sibling tickets already isolate PIT maintenance, query API work, provider-aware optimization, and v0.15.0 documentation.
- If delete-aware incremental hierarchy maintenance is later required beyond the rebuild fallback, track it as a separate follow-up rather than widening this story's v1 baseline.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add explicit bridge rebuild and incremental maintenance contracts.

## Scope
- Refine and complete the work for "Add bridge maintenance service" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.