[gicket-bot] PO refinement contract

Summary
- Refined the story against the existing ticket, comment, relation, and repository baseline: the v2 contract should formalize the current request-bound read diagnostics/read-shape surface as bounded redacted explain output. No child-ticket, relation, description, attachment, or planning-document writes were needed for this refinement pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository baseline already exposes the target surface through IDataVaultReadDiagnosticsService.Analyze(...) returning DataVaultDiagnosticsResult with ReadStrategy and additive ReadShape; this ticket should ratify that surface instead of inventing a parallel API.
- The contract should reuse the existing closed vocabularies already visible in code: DataVaultReadShapeKind = LatestSatellite, PitAsOf, Bridge; satellite semantics/modes = Current, AsOf, Traversal; read-strategy status = NotEvaluated, ProviderStrategySelected, ProviderNeutralFallback.
- Read-plan explain output is deterministic diagnostic data only: translated table and column identity, deterministic row-selection and ordering rules, provider strategy status, selected strategy name when available, finite fallback causes, and expected key/index baselines.
- The redaction boundary stays value-free: no raw request keys, raw hash-key values, as-of or timestamp values, SQL text, query plans, credentials, connection strings, provider error text, exception text, or other secret-bearing dumps.
- Non-applicable optional fields should be omitted rather than populated with placeholders or sentinel text, and finite fallback lists must stay machine-readable rather than collapsing into free-form prose.

Scope In
- Define the public v2 contract for request-bound read explain diagnostics covering latest/current and as-of satellite reads, PIT as-of reads, and bridge traversal reads.
- Define the provider section of the payload and explicitly reuse the current read-strategy status, selected-strategy, and finite fallback-cause vocabularies.
- Define the per-shape payload members for satellite, PIT, and bridge diagnostics, including translated entity identity, filter columns, projected column groups, deterministic selection or ordering rules, and expected key or index baselines.
- Define the redaction and omission rules for support-bundle and diagnostics serialization so consumers know which facts are present and which secret-bearing values are intentionally excluded.
- State that this contract is explainability and tuning guidance over existing diagnostics surfaces, not a new execution API.

Scope Out
- Any new query planner, LINQ provider, or alternate runtime query-execution surface.
- Raw SQL capture, provider query-plan export, provider physical-plan inspection, or credential-bearing diagnostics.
- Automatic index creation, automatic index recommendations, or provider-specific physical-design promises.
- New typed PIT or bridge helper generation, source-generator scope expansion, or DTO projection contracts.
- Changes to PIT or bridge maintenance behavior, strategy dispatch algorithms, or provider optimization implementation beyond documenting the current explain surface.

Open questions
- none

Follow-up questions
- Should the eventual public artifact be a dedicated architecture or planning document, or is an expanded authoritative ticket handoff sufficient once implementation lands?
- After the contract ships, should release guidance include a small reviewed readShape JSON example so consumers can map the documented members to support-bundle output without reading tests?
- If a future story wants raw SQL or execution-plan evidence, should that remain a separate consumer-owned capture workflow instead of extending this redacted diagnostics contract?

Risks
- Several rule members such as SeriesSelectionRule, PitRowSelectionRule, SnapshotLookupBehavior, and SupportedEndpointRules are literal explanatory strings; the contract should describe their meaning and bounded purpose without over-promising exact prose stability unless the team wants wording to become part of the serialized compatibility surface.
- ExpectedIndexBaseline reflects translated metadata baselines, not observed provider execution plans; unclear wording could cause consumers to infer unsupported physical-plan guarantees.
- Current repository evidence shows strong SQLite and provider-neutral fallback coverage; if the public contract starts making stronger cross-provider wording guarantees, additional provider-specific verification may be needed.

Split recommendations
- No split recommended for this ticket. The current repository already provides a bounded baseline, and future raw-SQL capture, automatic-index advisory behavior, or broader generated-helper work should stay in separate additive tickets.

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 4
- implementation-notes items: 8

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment