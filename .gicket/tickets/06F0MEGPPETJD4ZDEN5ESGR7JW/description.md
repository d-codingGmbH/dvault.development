<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the ticket, comment/relation state, child-ticket delivery, current source/tests, and release-note context; rewrote the parent story as a completion/consistency umbrella, made hierarchy bridge depth explicitly required and bounded, and clarified that consumer-facing release-note follow-up is already owned by downstream docs ticket 06F0MEJPGG7JBFEXD693BHY07W.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The ticket comment history currently contains bot-authored workflow, prior PO refinement, and PO-critic artifacts only; no human scope-change comment was found during this pass.
- No ticket-local attachment files were found; the referenced repository documents and current source/test layout were sufficient planning context for this refinement.
- Parent/child structure remains unchanged: epic 06F0MEDTB8496GYVM9K42F9VPG is parentOf this story, and this story is parentOf 06F0MEGYHADPVN575H64D56W2G, 06F0MEH660Y5QTNR5P8JPS2QXC, 06F0MEHDFYCVK42FFY77FXHXBR, and 06F0MEHKYTBJEJH2DVZ2CFH9Z0.
- This parent story should now be treated as umbrella/completion scope over already-delivered PIT and bridge read-helper work, not as a request for new parent-branch implementation beyond the existing done child tickets.
- No child tickets, relation updates, planning documents, or attachments were created during this PO pass.

### Scope In
- Story-level completion and contract consistency for the already-delivered provider-neutral PIT and bridge read-helper baseline across source, tests, public API snapshots, and child-ticket outcomes.
- Provider-neutral PIT as-of reads for one DataVaultPitMetadata declaration with one hub parent, ordered ordinary hub-attached satellites, a raw row API, and caller-owned typed projection helper behavior.
- Provider-neutral bridge raw-row and typed-projection helpers for many-to-many From/To traversal and hierarchy Ancestor/Descendant traversal over generated bridge shared-type tables.
- Deterministic validation and diagnostics for unsupported PIT or bridge metadata shapes, malformed generated EF shared-type metadata, and bounded hierarchy traversal request validation.
- Repository-level confirmation that remaining consumer-facing docs/release and benchmark follow-up already exists as downstream work instead of unresolved parent-story implementation scope.

### Scope Out
- New parent-story implementation work beyond the four existing done child tickets.
- Provider-specific read optimization, SQL tuning, or provider-specific read-strategy expansion beyond already-tracked downstream work.
- PIT refresh, PIT maintenance orchestration, bridge row maintenance, closure computation, or any automatic population of maintenance tables.
- Unbounded hierarchy traversal, arbitrary graph/path querying, path payload columns, effectivity windows, EF relationships, or foreign-key/navigation behavior.
- Consumer-facing release-note or README edits themselves; those remain downstream documentation work rather than parent-story implementation scope.

## Acceptance Criteria
- PIT request validation requires non-null DataVaultPitMetadata, deduplicates parent hash keys with ordinal comparison, rejects null/empty/whitespace keys, and normalizes AsOf to UTC DateTimeOffset.
- PIT raw reads return one record per requested parent hash key only when a generated PIT row exists with LoadTimestamp visible at or before AsOf; missing PIT rows produce no placeholder record and do not fall back to latest non-PIT satellite reads.
- Bridge many-to-many requests support From or To endpoint filtering and reject maximumDepth, while hierarchy bridge requests support Ancestor or Descendant endpoint filtering and require a non-negative bounded maximumDepth.
- Bridge raw reads return endpoint hash keys in generated bridge column order and include TraversalDepth only for hierarchy rows; typed projection helpers use exact generated endpoint-column names and exact TraversalDepth access where applicable.
- Unsupported PIT or bridge metadata/model shapes fail before partial data is returned, with diagnostics that name the affected metadata and unsupported condition.
- Repository evidence for this parent story includes public PIT and bridge read surfaces, unit coverage, SQLite integration coverage, and public API snapshot entries already present on branch; remaining docs/release and benchmark follow-up stays with existing downstream tickets 06F0MEJPGG7JBFEXD693BHY07W and 06F0MEJ0NE80R7CNS982S3PKVR.

## Definition of Done
- The four existing parentOf child tickets 06F0MEGYHADPVN575H64D56W2G, 06F0MEH660Y5QTNR5P8JPS2QXC, 06F0MEHDFYCVK42FFY77FXHXBR, and 06F0MEHKYTBJEJH2DVZ2CFH9Z0 remain done and continue to match the parent-story contract.
- The parent story contract matches the current source/test behavior and is routed as completion/consistency scope back through PO-critic rather than to dev for fresh parent-branch implementation work.
- Public PIT and bridge read surfaces, tests, and diagnostics continue to reflect bounded provider-neutral behavior and do not imply PIT/bridge maintenance ownership or unbounded hierarchy semantics.
- Release-note/changelog ownership for consumer-facing wording is explicitly downstream to 06F0MEJPGG7JBFEXD693BHY07W, and benchmark/performance follow-up remains explicitly downstream to 06F0MEJ0NE80R7CNS982S3PKVR.
- No blocking PO clarification remains on ticket scope, hierarchy depth semantics, parent-story workflow posture, or release-note ownership.

## Implementation Notes
- Current source evidence shows PIT raw reads on IDataVaultReadService via ReadPitRowsAsync(...) plus typed PIT projections through DataVaultReadServicePitExtensions.ReadPitAsync(...).
- Current source evidence shows bridge raw-row and typed-projection helpers through DataVaultReadServiceBridgeExtensions, DataVaultBridgeReadRequest, DataVaultBridgeReadRecord, and DataVaultBridgeProjectionRow.
- DataVaultBridgeReadRequest and DataVaultBridgeReadServiceTests are the authoritative branch evidence for the bounded hierarchy rule: hierarchy requests require maximumDepth, many-to-many requests do not allow it, and negative depth is rejected.
- docs/releases/v0.6.0.md still reflects a pre-delivery limitation snapshot; treat that file as downstream documentation consistency work rather than as evidence that this parent story still needs new implementation scope.
- No relation cleanup, child-ticket creation, planning-document write, or attachment binding was performed during this PO pass.

## Open Questions
- none

## Follow-Up Questions
- When docs/release ticket 06F0MEJPGG7JBFEXD693BHY07W resumes, should the stale v0.6.0 limitation text be amended directly, or should the next release notes supersede it as the authoritative consumer-facing update?
- When benchmark ticket 06F0MEJ0NE80R7CNS982S3PKVR resumes, which provider-specific read workloads still need measurement now that PIT and bridge helper surfaces are already present on branch?
- After documentation catches up, should README or quickstart examples explicitly show the bounded hierarchy maximumDepth requirement so consumers do not assume unbounded traversal?

## Risks
- Consumers may still read docs/releases/v0.6.0.md and conclude PIT-backed reads and bridge helpers are absent until downstream docs/release work updates the consumer-facing notes.
- Hierarchy bridge reads depend on precomputed rows and a required bounded maximumDepth; they do not imply arbitrary recursive traversal or automatic closure maintenance.
- Consumers may expect PIT or bridge helpers to populate PIT/bridge maintenance tables; the read-only boundary must stay explicit in diagnostics and follow-up documentation.

## Split Recommendations
- No further split is recommended. The parent story is already decomposed into four done child tickets, and the remaining docs/release and benchmark work already exists as downstream tickets rather than missing child scope.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Turn the deferred PIT and bridge metadata baseline into practical provider-neutral read helpers for common as-of snapshot and traversal scenarios.

## Scope In

- PIT-backed as-of read API contract and baseline implementation.
- Bridge traversal query helper contract and baseline implementation.
- Correctness tests over generated tables and existing metadata.

## Scope Out

- Provider-specific query tuning.
- Full graph query engine or unbounded recursive hierarchy semantics.

## Acceptance Criteria

- PIT read helpers return source-backed as-of rows for configured satellites.
- Bridge helper covers documented many-to-many and bounded hierarchy traversal baseline.
- Unsupported cases fail with clear diagnostics instead of returning incomplete data.