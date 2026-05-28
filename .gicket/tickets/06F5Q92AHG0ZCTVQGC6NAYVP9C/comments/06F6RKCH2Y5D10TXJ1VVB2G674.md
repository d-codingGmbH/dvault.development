[gicket-bot] PO refinement contract

Summary
- Refined the satellite-only typed read-model generator story against the existing generator contract and current read-service/analyzer baselines; no split or relation changes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This ticket is the satellite-only implementation slice of `docs/plans/typed-read-model-generator-contract.md`; PIT and bridge generation stay in sibling ticket `06F5Q92R02HB7FCE1AWKXPTMRW`.
- Repository evidence already fixes the runtime baseline: `IDataVaultReadService`, `DataVaultLatestSatelliteReadRequest`, `DataVaultReadServiceCurrentSatelliteExtensions`, and `DataVaultSatelliteProjectionRow` define current/latest/as-of satellite semantics, so this story generates compile-time wrappers over that surface instead of adding a new read engine.
- Repository evidence also fixes the generator host: `DCoding.Data.DVault.Analyzers` already carries `DataVaultMappingSourceGenerator`, so the typed satellite generator belongs in that analyzer/source-generator package and should use the satellite-relevant portion of the reserved `DMV1960`-`DMV1969` range.
- No child tickets, relation edits, description edits, attachments, or planning documents were materialized because the existing contract document already bounds this story sufficiently for PO handoff.

Scope In
- Normalize one authoritative metadata source for satellite generation from the supported metadata-first, model-first, or code-first inputs, preserving source kind/fingerprint, produced names, metadata names, parent reference data, property roles, provider logical/value metadata, ordinals, CLR types, and nullability needed by generated helpers.
- Generate satellite-specific `{SatelliteProducedName}ReadModel` and `{SatelliteProducedName}ReadExtensions` types in `{RootNamespace}.DVault.GeneratedReadModels` with `Read...CurrentAsync`, `Read...LatestAsync`, and `Read...AsOfAsync` helpers over `IDataVaultReadService`.
- Support hub-parent, link-parent, and deterministic multi-active satellites within the documented v1 provider-neutral string payload/driving-key boundary.
- Emit bounded diagnostics for unresolved authoritative metadata, stale fingerprints, unsupported satellite shapes, deterministic name collisions, dynamic/provider-specific query requirements, and conservative payload-nullability fallback.

Scope Out
- Generated PIT or bridge helpers, PIT/bridge descriptor validation, and PIT/bridge diagnostics; those remain in `06F5Q92R02HB7FCE1AWKXPTMRW`.
- Any new runtime read engine, arbitrary caller-defined request compilation, provider-specific SQL generation, or automatic PIT/bridge maintenance.
- Runtime-selected payload subsets, caller-supplied filter predicates, ordering customization, unbounded traversal, or non-string payload/driving-key type support beyond the documented v1 boundary.
- Changes to `dvault.model.v1` governance beyond consuming already-supported model-first inputs for satellite helper generation.

Open questions
- none

Follow-up questions
- After the satellite slice lands, decide whether public README and release-note examples should show generated satellite read models immediately or wait until sibling PIT/bridge generation ticket `06F5Q92R02HB7FCE1AWKXPTMRW` lands so the typed-read story can be presented as one coordinated feature.
- A later contract pass can decide whether v2 should widen generated payload and driving-key CLR type support beyond the current provider-neutral string boundary.
- Once both child tickets land, confirm whether the analyzer package README should present the full `DMV1960`-`DMV1969` catalog in one consolidated typed-read generator section.

Risks
- The contract intentionally bounds generated satellite properties to shapes that can be proven from authoritative metadata; consumers with provider-specific or non-string payload mappings will receive diagnostics or skipped generation rather than helpers.
- Metadata-source ambiguity or source-fingerprint drift will hard-fail generation by design, so teams using multiple declaration paths must keep one authoritative source visible per generated scope.
- If any supported shape uses the optional stable direct EF projection path, implementation must stay inside the repository's compiled-model and compiled-query compatibility boundary to avoid subtle provider regressions.

Split recommendations
- No further split is recommended: the repository already isolates PIT and bridge generation into `06F5Q92R02HB7FCE1AWKXPTMRW`, and the remaining satellite generator and analyzer slice is bounded enough for one implementation story.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment