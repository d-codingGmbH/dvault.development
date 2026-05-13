<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Delivery contract refined and ready for PO-critic review.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The canonical comparison target is the existing dvault.model.v1 artifact contract already fixed by docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md; this story does not define a new snapshot or schema dialect.
- Completed prerequisite stories 06F1XPS7KGKBP5SVMQPJC49J2G and 06F1XPVPKVGYKCV04PY98TSS78 remain the authoritative diagnostic-code and design-time workflow baselines; their preserved PO handoff text is historical context only.
- Repository evidence already fixes the design-time boundary: ModelSnapshot comparison stays metadata-only, while live schema comparison is optional evidence rather than migration generation or repair.
- No new child tickets, attachments, or planning documents were materialized in this pass.

### Scope In
- Deliver one coherent drift-comparison story across canonical dvault.model.v1 artifacts, EF ModelSnapshot metadata, and optional live database schema metadata.
- Keep ModelSnapshot comparison provider-neutral and based on the existing DVault EF annotation surface, with deterministic ordering and stable drift diagnostics.
- Keep live schema comparison provider-neutral at the contract level, with SQLite as the required v1 supported lane and explicit unsupported or unavailable results for non-implemented or unavailable providers.
- Document when ModelSnapshot comparison is sufficient and when live schema comparison is needed for additional physical-schema evidence.
- Preserve coverage for DVault table kinds already in the visible v0.7.0 baseline, including hubs, links, satellites, PITs, and bridges.

### Scope Out
- No migration generation, migration-operations diff engine, or automatic repair.
- No repo-owned EF CLI shim, design-time services package, or expansion beyond the established consumer-owned design-time factory boundary.
- No requirement that every provider package ship first-class live-schema readers in this story.
- No provider-specific DDL diff engine, foreign-key graph diffing, or arbitrary non-DVault catalog comparison.
- No dvault.model.v1 contract expansion beyond the current documented artifact baseline.

## Acceptance Criteria
- EF ModelSnapshot comparison produces deterministic provider-neutral drift results for hubs, links, satellites, PITs, and bridges using the current DVault EF metadata surface.
- Optional live schema comparison produces deterministic results for the supported v1 physical surface of DVault-owned tables, ordered columns, named primary-key constraints, and secondary indexes.
- Missing, renamed, incompatible, or unsupported metadata is reported with stable machine-readable diagnostics instead of being silently ignored.
- When live schema comparison is not implemented for a provider or the requested environment is unavailable, the result is a clear unsupported or unavailable outcome rather than a false pass.
- Repository documentation explains the evidence boundary: ModelSnapshot comparison validates projected EF metadata, while live schema comparison is used when physical database evidence is required.

## Definition of Done
- The parent story contract remains bounded to the two existing implementation children: 06F1XPWNAWWMDBRK315S66P7AM for ModelSnapshot comparison and 06F1XPWYZTWE9E46GNPFB8F804 for live-schema comparison, with no additional split required for this slice.
- Both comparison lanes reuse the existing DVault drift-report and diagnostic-code conventions instead of creating parallel report formats.
- Documentation and public guidance match the implemented support boundary, including SQLite-first live-schema evidence and explicit non-support or opt-in status for other providers.
- The parent story no longer carries stale blocks relations to completed prerequisite stories.

## Implementation Notes
- Use dvault.model.v1 as the single authoritative artifact baseline and keep the existing DataVaultModelDriftReporter.Compare drift-report conventions as the shared comparison and reporting boundary.
- Keep ModelSnapshot extraction on the existing provider-neutral EF annotation surface, especially EntityKind, MetadataName, ProducedName, Ordinal, ParentReferenceKind, ParentReferenceName, PropertyRole, ProviderLogicalPropertyKind, MetadataSourceKind, and MetadataSourceFingerprint.
- Keep live-schema comparison bounded to the physical surface the repository currently emits: DVault-owned tables, ordered columns, named primary keys, and secondary indexes, with SQLite as the required local proof lane.
- Preserve deterministic ordering across both lanes so the same underlying metadata does not yield different drift ordering or diagnostic sequencing.

## Open Questions
- none

## Follow-Up Questions
- After this story ships, should the design-time preflight workflow expose one first-class command or report format that can invoke artifact-only, ModelSnapshot, and live-schema comparisons through a shared operator entrypoint?
- Which provider should be the next live-schema implementation after the SQLite-first baseline: Postgres, SQL Server, Oracle, or MySQL?
- Should the repository later publish one consolidated public guide covering when to use artifact review, ModelSnapshot comparison, and live-schema comparison together?

## Risks
- If the ModelSnapshot and live-schema lanes normalize names, ordinals, or ordering differently, users may see false drift between logically equivalent metadata.
- Documentation may over-promise support if it implies broad multi-provider live-schema coverage instead of the SQLite-first baseline and explicit unsupported or unavailable results.
- If future dvault.model.v1 fields exceed what the current EF projection surface exposes, snapshot comparison must keep surfacing explicit unsupported gaps rather than implying full coverage.

## Split Recommendations
- No further split recommended. The bounded implementation split already exists as child tickets 06F1XPWNAWWMDBRK315S66P7AM and 06F1XPWYZTWE9E46GNPFB8F804, and both are already done.
- If broader provider-by-provider live-schema support is needed later, track each provider expansion in separate follow-up tickets rather than widening this parent story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Extend v0.7.0 drift tooling to compare governed DVault artifacts against EF ModelSnapshot metadata and optional live database schema metadata.

## Scope In

- Add an EF ModelSnapshot comparison adapter.
- Add a provider-neutral live schema reader abstraction with provider hooks where useful.
- Report drift with stable diagnostic codes and deterministic ordering.
- Document when ModelSnapshot comparison is sufficient and when live schema comparison is needed.

## Scope Out

- No automatic migration generation.
- No destructive database repair.
- No full provider-specific SQL diff engine.

## Acceptance Criteria

- ModelSnapshot comparison reports no drift for matching metadata.
- Missing/renamed/incompatible items include locations.
- Live schema comparison is optional and skipped clearly when unavailable.
- Docs state provider evidence boundaries.

## Implementation Notes

- Reuse v0.7.0 model artifact and drift report concepts.

## Open Questions

- none