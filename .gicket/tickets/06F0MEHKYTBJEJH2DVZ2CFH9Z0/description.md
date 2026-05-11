<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined ticket 06F0MEHKYTBJEJH2DVZ2CFH9Z0 as a bounded provider-neutral bridge traversal read implementation. Repository evidence shows bridge metadata and generated schema are already established; no child tickets, relation changes, attachments, or planning documents were created.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The current ticket remains correctly related under parent story 06F0MEGPPETJD4ZDEN5ESGR7JW and blocks downstream benchmark ticket 06F0MEJ0NE80R7CNS982S3PKVR plus docs/release ticket 06F0MEJPGG7JBFEXD693BHY07W.
- Repository evidence fixes the v1 bridge baseline: many-to-many and hierarchy bridge metadata, provider-neutral EF shared-type bridge tables, endpoint hash-key columns, and hierarchy TraversalDepth.
- Existing bridge schema tests already prove BridgeCustomerOrder and BridgeSalesRegionHierarchy table shapes, keys, indexes, and absence of EF foreign keys; this ticket implements reads over that baseline.
- Only bot claim and lease comments were found for this ticket; no human PO-scope comment or attachment required further clarification.

### Scope In
- Implement provider-neutral read helpers over generated bridge tables in the core DVault read-model surface.
- Support many-to-many traversal rows using the bridge declaration, ordered endpoint hash-key columns, endpoint direction, and requested endpoint hash keys.
- Support bounded hierarchy traversal rows using ancestor hash key, descendant hash key, and TraversalDepth semantics from the bridge metadata baseline.
- Return deterministic empty results for empty bridge tables and for valid requests whose endpoint hash keys have no matching bridge rows.
- Fail deterministically with clear diagnostics for missing bridge metadata, missing generated bridge entity/table/properties, unsupported bridge kind, unsupported projection features, malformed endpoint bindings, and unsupported depth requests.
- Add focused unit and integration coverage for many-to-many reads, hierarchy reads, empty bridges, missing endpoints, unsupported shapes, and existing bridge schema compatibility.

### Scope Out
- Provider-specific SQL, provider-specific query tuning, and provider-specific read strategy dispatch.
- PIT-backed read implementation or PIT/bridge composition.
- Bridge row population, closure maintenance, save-service changes, migrations, EF foreign keys, or navigations.
- Full recursive graph engine behavior, unbounded traversal, arbitrary path finding, path payload columns, effectivity windows, or relationship graph metadata.
- Code-First bridge declaration expansion, model-first import/export work, or release documentation updates beyond test/API evidence needed for this implementation.

## Acceptance Criteria
- Many-to-many read requests over a declared bridge return only matching rows with deterministic ordering and expose both endpoint hash keys using the bridge metadata column order.
- Hierarchy read requests return ancestor hash key, descendant hash key, and TraversalDepth, and honor bounded depth constraints without implying unbounded recursion.
- Empty bridge tables and valid requests with no matching endpoint rows return empty results rather than errors.
- Unsupported or inconsistent metadata/model shapes produce diagnostics that include the bridge name and the unsupported kind, feature, endpoint, table, property, or depth condition.
- Implementation uses provider-neutral EF querying over generated shared-type bridge tables and does not rely on EF relationships, navigations, provider-specific SQL, or provider package behavior.
- Existing bridge translation and SQLite schema tests continue to pass, and new tests cover empty bridges, missing endpoints, many-to-many traversal, hierarchy depth handling, and unsupported shapes.
- Any public request/response additions align with the existing IDataVaultReadService and caller-owned projection style; public API snapshots are updated if the surface changes.

## Definition of Done
- Bridge traversal read helpers are implemented in the core DCoding.Data.DVault package with deterministic request validation and result ordering.
- Tests cover the accepted baseline and regression paths in the existing unit and integration test roots.
- Public API snapshots, XML documentation, and diagnostics expectations are updated when public types or messages are added.
- No provider package optimization, PIT behavior, bridge maintenance, or graph-engine behavior is introduced as part of this ticket.
- A developer can run the relevant DVault test projects and see existing bridge schema coverage plus new bridge read coverage pass.

## Implementation Notes
- Use DataVaultMetadataModel.Bridges or registry-backed bridge lookup as the authoritative bridge source; do not infer bridge semantics from EF foreign keys or navigations.
- Match the existing translator baseline: table name Bridge plus normalized bridge name, many-to-many endpoint hash-key columns, hierarchy Ancestor/Descendant hash-key columns, and the integer TraversalDepth property with BridgeDepth role/logical kind.
- The core assembly can use existing internal bridge endpoint metadata; avoid broad public exposure of endpoint internals unless required by the public read contract.
- Follow the existing satellite read-service pattern: DbContext shared-type dictionary queries, AsNoTracking, deterministic batching/filtering where useful, and caller-owned typed projection rather than reflection DTO binding.
- Treat metadata/model mismatch as a diagnostic failure; treat valid no-row reads as empty results.
- Unsupported projection features already fail translation with the baseline message that only endpoint hash-key columns and hierarchy TraversalDepth are supported; mirror that boundary in read diagnostics.

## Open Questions
- none

## Follow-Up Questions
- After the baseline implementation lands, decide in provider-specific read-strategy work whether bridge traversal should receive provider-optimized query paths.
- After row/request behavior proves stable, consider typed projection convenience overloads for bridge traversal results.
- Plan bridge row maintenance or closure refresh behavior separately if future stories need automated population rather than manual/generated rows.
- Future architecture can evaluate unbounded recursive traversal, path payloads, effectivity windows, and graph-query composition as separate advanced capabilities.

## Risks
- A DbContext configured with different metadata than the request could otherwise look like an empty result; this must be diagnosed clearly.
- Ambiguous hierarchy depth semantics could produce misleading partial graph answers, so unsupported or unbounded depth requests must fail instead of being approximated.
- Provider-neutral EF shared-type dictionary queries may be slower than provider-specific SQL; that is acceptable for this baseline and should be measured by downstream benchmark work.
- Expanding endpoint metadata visibility unnecessarily would enlarge the public API beyond the implemented read baseline.

## Split Recommendations
- No split is recommended. The contract design is already completed by 06F0MEHDFYCVK42FFY77FXHXBR, while benchmarks and documentation are already represented as downstream blocked tickets.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Implement the documented provider-neutral bridge traversal read baseline for many-to-many and bounded hierarchy bridge rows.

## Scope In

- Reads over generated bridge tables.
- Deterministic handling of traversal depth and endpoint hash-key columns.
- Tests for empty bridges, missing endpoints, and unsupported shapes.

## Scope Out

- Provider-specific optimization.
- PIT read implementation.

## Acceptance Criteria

- The implementation is correct before provider-specific tuning is attempted.
- Diagnostics clearly identify unsupported bridge metadata combinations.
- Existing bridge schema tests continue to pass.