[gicket-bot] PO refinement contract

Summary
- Refined ticket 06F0MEHKYTBJEJH2DVZ2CFH9Z0 as a bounded provider-neutral bridge traversal read implementation. Repository evidence shows bridge metadata and generated schema are already established; no child tickets, relation changes, attachments, or planning documents were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current ticket remains correctly related under parent story 06F0MEGPPETJD4ZDEN5ESGR7JW and blocks downstream benchmark ticket 06F0MEJ0NE80R7CNS982S3PKVR plus docs/release ticket 06F0MEJPGG7JBFEXD693BHY07W.
- Repository evidence fixes the v1 bridge baseline: many-to-many and hierarchy bridge metadata, provider-neutral EF shared-type bridge tables, endpoint hash-key columns, and hierarchy TraversalDepth.
- Existing bridge schema tests already prove BridgeCustomerOrder and BridgeSalesRegionHierarchy table shapes, keys, indexes, and absence of EF foreign keys; this ticket implements reads over that baseline.
- Only bot claim and lease comments were found for this ticket; no human PO-scope comment or attachment required further clarification.

Scope In
- Implement provider-neutral read helpers over generated bridge tables in the core DVault read-model surface.
- Support many-to-many traversal rows using the bridge declaration, ordered endpoint hash-key columns, endpoint direction, and requested endpoint hash keys.
- Support bounded hierarchy traversal rows using ancestor hash key, descendant hash key, and TraversalDepth semantics from the bridge metadata baseline.
- Return deterministic empty results for empty bridge tables and for valid requests whose endpoint hash keys have no matching bridge rows.
- Fail deterministically with clear diagnostics for missing bridge metadata, missing generated bridge entity/table/properties, unsupported bridge kind, unsupported projection features, malformed endpoint bindings, and unsupported depth requests.
- Add focused unit and integration coverage for many-to-many reads, hierarchy reads, empty bridges, missing endpoints, unsupported shapes, and existing bridge schema compatibility.

Scope Out
- Provider-specific SQL, provider-specific query tuning, and provider-specific read strategy dispatch.
- PIT-backed read implementation or PIT/bridge composition.
- Bridge row population, closure maintenance, save-service changes, migrations, EF foreign keys, or navigations.
- Full recursive graph engine behavior, unbounded traversal, arbitrary path finding, path payload columns, effectivity windows, or relationship graph metadata.
- Code-First bridge declaration expansion, model-first import/export work, or release documentation updates beyond test/API evidence needed for this implementation.

Open questions
- none

Follow-up questions
- After the baseline implementation lands, decide in provider-specific read-strategy work whether bridge traversal should receive provider-optimized query paths.
- After row/request behavior proves stable, consider typed projection convenience overloads for bridge traversal results.
- Plan bridge row maintenance or closure refresh behavior separately if future stories need automated population rather than manual/generated rows.
- Future architecture can evaluate unbounded recursive traversal, path payloads, effectivity windows, and graph-query composition as separate advanced capabilities.

Risks
- A DbContext configured with different metadata than the request could otherwise look like an empty result; this must be diagnosed clearly.
- Ambiguous hierarchy depth semantics could produce misleading partial graph answers, so unsupported or unbounded depth requests must fail instead of being approximated.
- Provider-neutral EF shared-type dictionary queries may be slower than provider-specific SQL; that is acceptable for this baseline and should be measured by downstream benchmark work.
- Expanding endpoint metadata visibility unnecessarily would enlarge the public API beyond the implemented read baseline.

Split recommendations
- No split is recommended. The contract design is already completed by 06F0MEHDFYCVK42FFY77FXHXBR, while benchmarks and documentation are already represented as downstream blocked tickets.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment