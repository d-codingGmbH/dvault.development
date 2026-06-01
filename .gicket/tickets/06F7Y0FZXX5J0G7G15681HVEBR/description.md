<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story against the existing ticket, comment, relation, and repository baseline: the v2 contract should formalize the current request-bound read diagnostics/read-shape surface as bounded redacted explain output. No child-ticket, relation, description, attachment, or planning-document writes were needed for this refinement pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The repository baseline already exposes the target surface through IDataVaultReadDiagnosticsService.Analyze(...) returning DataVaultDiagnosticsResult with ReadStrategy and additive ReadShape; this ticket should ratify that surface instead of inventing a parallel API.
- The contract should reuse the existing closed vocabularies already visible in code: DataVaultReadShapeKind = LatestSatellite, PitAsOf, Bridge; satellite semantics/modes = Current, AsOf, Traversal; read-strategy status = NotEvaluated, ProviderStrategySelected, ProviderNeutralFallback.
- Read-plan explain output is deterministic diagnostic data only: translated table and column identity, deterministic row-selection and ordering rules, provider strategy status, selected strategy name when available, finite fallback causes, and expected key/index baselines.
- The redaction boundary stays value-free: no raw request keys, raw hash-key values, as-of or timestamp values, SQL text, query plans, credentials, connection strings, provider error text, exception text, or other secret-bearing dumps.
- Non-applicable optional fields should be omitted rather than populated with placeholders or sentinel text, and finite fallback lists must stay machine-readable rather than collapsing into free-form prose.

### Scope In
- Define the public v2 contract for request-bound read explain diagnostics covering latest/current and as-of satellite reads, PIT as-of reads, and bridge traversal reads.
- Define the provider section of the payload and explicitly reuse the current read-strategy status, selected-strategy, and finite fallback-cause vocabularies.
- Define the per-shape payload members for satellite, PIT, and bridge diagnostics, including translated entity identity, filter columns, projected column groups, deterministic selection or ordering rules, and expected key or index baselines.
- Define the redaction and omission rules for support-bundle and diagnostics serialization so consumers know which facts are present and which secret-bearing values are intentionally excluded.
- State that this contract is explainability and tuning guidance over existing diagnostics surfaces, not a new execution API.

### Scope Out
- Any new query planner, LINQ provider, or alternate runtime query-execution surface.
- Raw SQL capture, provider query-plan export, provider physical-plan inspection, or credential-bearing diagnostics.
- Automatic index creation, automatic index recommendations, or provider-specific physical-design promises.
- New typed PIT or bridge helper generation, source-generator scope expansion, or DTO projection contracts.
- Changes to PIT or bridge maintenance behavior, strategy dispatch algorithms, or provider optimization implementation beyond documenting the current explain surface.

## Acceptance Criteria
- The refined contract names IDataVaultReadDiagnosticsService.Analyze(...) and DataVaultDiagnosticsResult.ReadShape as the authoritative request-bound surface and states that support-bundle export serializes the same bounded data under readShape.
- The contract preserves the existing closed vocabularies: DataVaultReadShapeKind values LatestSatellite, PitAsOf, and Bridge; read-strategy status values NotEvaluated, ProviderStrategySelected, and ProviderNeutralFallback; and the finite DataVaultReadStrategyFallbackCauseKind set already used by diagnostics.
- Latest/current and as-of satellite diagnostics are specified to include translated satellite identity, parent reference, filter columns, Current vs AsOf semantics, series-selection rule, cutoff rule, deterministic ordering, projected column groups, and expected index baseline.
- PIT diagnostics are specified to include translated PIT identity, parent reference, referenced satellite snapshot bindings, filter columns, PIT row-identity columns, PIT row-selection rule, snapshot lookup behavior, no-latest-fallback behavior, maintained-PIT prerequisite, projected column groups, referenced-satellite lookup count, and expected index baseline.
- Bridge diagnostics are specified to include bridge kind, translated bridge identity, endpoint descriptors, selected filter endpoint, endpoint filter, optional maximum-depth predicate, deterministic ordering, supported endpoint rules, projected column groups, and expected traversal index baseline.
- Provider facts are limited to provider name, capability and behavior profile names plus defaulting flags, selected strategy name when present, read-strategy status, and finite fallback causes; non-applicable optional fields are omitted rather than filled with sentinels.
- The contract explicitly forbids raw request keys, raw hash-key values, as-of or timestamp values, SQL text, query plans, credentials, connection strings, provider error text, exception text, and other secret-bearing output.
- The contract explicitly says this surface is diagnostics and tuning guidance, not a raw-SQL advisor, automatic-index advisor, or provider-specific physical-plan promise.

## Definition of Done
- An authoritative ticket handoff or public-facing contract document enumerates the reused vocabularies, per-shape payload members, redaction rules, omission rules, and non-goals.
- Any implementation or documentation updates keep IDataVaultReadDiagnosticsService, DataVaultDiagnosticsResult.ReadShape, and support-bundle serialization aligned with the documented contract instead of introducing a second competing shape.
- Automated coverage proves representative satellite, PIT, and bridge read-shape payloads plus provider-selected and provider-neutral fallback exposure remain serialized as documented and do not leak supplied request-key values.
- Release or guidance text that references read-plan or read-shape explainability is updated to keep the public message aligned with the bounded redacted diagnostics surface and to avoid raw-SQL or query-plan promises.

## Implementation Notes
- DefaultDataVaultDiagnosticsService.CreateReadShapeDiagnostics(...) already builds the baseline surface; refinement should treat that implementation as the shape to formalize rather than redesigning the contract from scratch.
- The current satellite payload type already carries Semantics, Satellite, ParentReference, FilterColumns, SeriesSelectionRule, CutoffRule, DeterministicOrdering, ExpectedIndexBaseline, and additive ProjectedColumns.
- The current PIT payload type already carries Pit, ParentReference, ReferencedSatellites, FilterColumns, PitRowSelectionRule, SnapshotLookupBehavior, NoLatestFallbackBehavior, MaintainedPitPrerequisite, ExpectedIndexBaseline, and additive ProjectedColumns, RowIdentityColumns, and ReferencedSatelliteLookupCount.
- The current bridge payload type already carries BridgeKind, Bridge, Endpoints, FilterEndpoint, EndpointFilter, optional DepthPredicate, DeterministicOrdering, SupportedEndpointRules, ExpectedTraversalIndexBaseline, and additive ProjectedColumns.
- Existing tests already cover the intended boundary: support-bundle export includes readShape while omitting a supplied secret parent hash key, and PIT plus bridge diagnostics tests assert both provider-selected and provider-neutral fallback states.
- Reuse existing finite fallback and gate vocabularies such as UnsupportedSatelliteParent, MultiActiveSatelliteUnsupported, UnsupportedPitShape, UnsupportedBridgeShape, and NoProviderSpecificStrategyRegistered instead of introducing free-form fallback text.
- When the contract needs user-facing terminology for request semantics, align it with the existing Current, AsOf, and Traversal vocabulary already reused by activity tracing.
- No persistent ticket or planning writes were applied during this refinement pass; the handoff is based on current repository and ticket evidence only.

## Open Questions
- none

## Follow-Up Questions
- Should the eventual public artifact be a dedicated architecture or planning document, or is an expanded authoritative ticket handoff sufficient once implementation lands?
- After the contract ships, should release guidance include a small reviewed readShape JSON example so consumers can map the documented members to support-bundle output without reading tests?
- If a future story wants raw SQL or execution-plan evidence, should that remain a separate consumer-owned capture workflow instead of extending this redacted diagnostics contract?

## Risks
- Several rule members such as SeriesSelectionRule, PitRowSelectionRule, SnapshotLookupBehavior, and SupportedEndpointRules are literal explanatory strings; the contract should describe their meaning and bounded purpose without over-promising exact prose stability unless the team wants wording to become part of the serialized compatibility surface.
- ExpectedIndexBaseline reflects translated metadata baselines, not observed provider execution plans; unclear wording could cause consumers to infer unsupported physical-plan guarantees.
- Current repository evidence shows strong SQLite and provider-neutral fallback coverage; if the public contract starts making stronger cross-provider wording guarantees, additional provider-specific verification may be needed.

## Split Recommendations
- No split recommended for this ticket. The current repository already provides a bounded baseline, and future raw-SQL capture, automatic-index advisory behavior, or broader generated-helper work should stay in separate additive tickets.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Define the v2 read-plan explanation contract before implementation.

# Scope In
- Specify bounded output for latest/current/as-of satellite, PIT, and bridge reads.
- Include provider strategy, fallback cause, translated shape facts, expected key/index access paths, and omission rules.
- Preserve redaction for raw hash keys, request keys, timestamps, SQL text, query plans, credentials, and provider errors.

# Acceptance Criteria
- Contract reuses existing diagnostics vocabularies where possible.
- It states that read-plan explainability is diagnostic output, not a new query planner or LINQ provider.