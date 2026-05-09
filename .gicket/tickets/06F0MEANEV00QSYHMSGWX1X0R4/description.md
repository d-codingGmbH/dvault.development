<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the parent registry story to limit code-first compatibility to the existing internal EF translation path, explicitly scope both public point-in-time lookup families, and keep the live three-child split unchanged with no planning writes.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- DataVaultMetadataRegistry remains the v1 registry baseline; this story should not reopen a second registry abstraction.
- Satellite lookup remains parent-scoped; exact-name lookup covers hubs, links, bridges, legacy point-in-time tables through TryGetPointInTimeTable, and PIT metadata through TryGetPit.
- CLR-type lookup remains opt-in and only succeeds where one explicit, unambiguous DataVaultMetadataClrMapping exists.
- The default registration path remains optionless AddDVault(). Registry-backed projection is the additive path where callers register a DataVaultMetadataModel or prebuilt DataVaultMetadataRegistry during service setup and opt contexts in with UseDataVaultMetadata().
- Existing public code-first declarations stay in scope only through their current EF model-building path; this story does not add a public code-first-to-DataVaultMetadataModel or code-first-to-registry export API.
- Live parentOf relations to 06F0MEAXT99V0P115P0WEJD4P0, 06F0MEB634X6CTBZ00W108G3FG, and 06F0MEBFTW8FY5T7PY5HJ5JXJ4 remain unchanged; no child tickets, relations, attachments, or planning documents were created in this pass.

### Scope In
- Immutable DataVaultMetadataRegistry creation and deterministic lookup over hubs, links, satellites, bridges, DataVaultPointInTimeMetadata, DataVaultPitMetadata, and provider capability profiles.
- DI and EF integration that lets AddDVault(...) register one authoritative DataVaultMetadataModel or prebuilt DataVaultMetadataRegistry and lets UseDataVaultMetadata() consume it with explicit context-level overrides.
- Reuse of the existing provider-neutral DataVaultMetadataModel translation pipeline across metadata-first registration and the existing public code-first EF model path.
- Actionable validation and diagnostics for duplicate logical names, missing metadata dependencies, conflicting metadata sources, and ambiguous or absent CLR-based lookups.

### Scope Out
- A new public code-first export or registration API that produces DataVaultMetadataModel or DataVaultMetadataRegistry outside the current EF model-building path.
- Model-first file import-export, external serialization formats, or repository-to-registry tooling.
- Runtime mutation of registry contents after service-provider build.
- New provider-specific SQL, save-service semantics, read-service behavior, PointInTimeTables or PIT refresh behavior, or bridge maintenance behavior beyond consuming the registry as authoritative metadata.

## Acceptance Criteria
- Callers can register one authoritative DVault metadata source during service setup by supplying a DataVaultMetadataModel or DataVaultMetadataRegistry and consume it consistently across schema projection, save/read workflows, diagnostics, and examples.
- The registry exposes immutable deterministic lookup for hubs, links, bridges, DataVaultPointInTimeMetadata through exact-name lookup and TryGetPointInTimeTable, DataVaultPitMetadata through exact-name lookup and TryGetPit, plus parent-scoped satellite lookup by exact parent reference and logical name; CLR-type lookup works only where one explicit mapping exists.
- Registry-backed projection reuses the existing provider-neutral DataVaultMetadataModel translation pipeline. The existing public code-first EF model path remains compatible because it already normalizes internally into that pipeline during model building, and this story does not add a separate public code-first export or registry-registration path.
- If the same EF model receives conflicting DVault metadata sources, or if logical-name or CLR lookup is missing or ambiguous, failure is immediate and actionable rather than silent or order-dependent.
- Bridges, legacy point-in-time tables, and PIT metadata are representable in the registry without making their runtime population, refresh, or maintenance behavior part of this story.

## Definition of Done
- Public API and examples show the registry-backed path through AddDVault(...) and UseDataVaultMetadata(...) without regressing the optionless AddDVault() baseline and without introducing a new public code-first export or registry-registration API.
- Automated coverage proves deterministic registry contents and diagnostics for duplicate names, missing dependencies, parent-scoped satellite lookup, CLR lookup conflicts, metadata-source conflicts, and exact-name point-in-time lookup through TryGetPointInTimeTable and TryGetPit.
- Registry-backed consumers continue to reuse the existing provider-neutral metadata translation pipeline instead of introducing a second interpretation path for schema projection or runtime services.
- The parent ticket contract remains aligned with the live child structure and related tickets; no extra relation cleanup or planning-document write is required for this refinement pass.

## Implementation Notes
- DataVaultMetadataRegistry already exposes immutable collections plus public TryGetPointInTimeTable and TryGetPit lookup families; the parent contract should name both families explicitly instead of using generic point-in-time wording.
- DataVaultCodeFirstModelBuilder has an internal constructor and internal BuildMetadataModel(), while the public code-first extensions translate during EF model building; use that as evidence to keep code-first compatibility scoped to internal normalization, not a new public export surface.
- README already documents the intended v1 registry wiring: services.AddDVault(options => options.UseMetadataModel(...)), app-level consumption through UseDataVaultMetadata(), and explicit DataVaultMetadataModel or DataVaultMetadataRegistry context overrides.
- Completed child tickets 06F0ME9PM8KXH3VP59TQR0ETA8 and 06F0MEA1FF743S14XQW02H4A3W remain historical evidence that code-first is additive to the shared translation pipeline, while 06F0MEB634X6CTBZ00W108G3FG covers the public registration path and 06F0MEBFTW8FY5T7PY5HJ5JXJ4 covers downstream registry consumers.
- Broader schema-parity regression breadth remains intentionally separated onto 06F0MEAD1BAA5QEVM3F9QJA38G.

## Open Questions
- none

## Follow-Up Questions
- If the team later wants app-startup registration directly from code-first declarations, should that be a separate public export or registration ticket rather than expanding this parent beyond the current EF model-building path?
- After registry and model-first work settles, should the older PointInTimeTables naming be publicly deprecated in favor of Pits, or should both lookup families remain first-class long-term?

## Risks
- If app-level registry defaults and explicit context overrides are not conflict-checked consistently, different workflows can project different metadata from the same DbContext model.
- If CLR lookup falls back to first-match or registration-order behavior, the registry loses the deterministic semantics this story is meant to centralize.
- Because both PointInTimeTables and Pits are publicly exposed, docs, examples, and tests must keep the two lookup families explicit or consumers may assume one supersedes the other.
- Because bridges and both point-in-time families are representable, downstream consumers may over-assume runtime support unless docs and diagnostics keep the deferred-capability boundary explicit.

## Split Recommendations
- Keep the existing parentOf split to 06F0MEAXT99V0P115P0WEJD4P0, 06F0MEB634X6CTBZ00W108G3FG, and 06F0MEBFTW8FY5T7PY5HJ5JXJ4; current evidence does not justify new child tickets or relation changes.
- Keep broader code-first parity breadth on 06F0MEAD1BAA5QEVM3F9QJA38G rather than folding that regression matrix back into this parent.
- If public code-first export or registration is desired later, split it into a dedicated follow-up instead of expanding this story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Provide a reusable model registry so schema projection, save services, read services, diagnostics, and examples can use one authoritative Data Vault model rather than repeated metadata construction.

## Scope In

- Immutable registry API for hubs, links, satellites, PIT, bridges, and future model-first imports.
- Service registration through AddDVault and DbContext/model integration.
- Lookup by logical name and by CLR type where available.

## Scope Out

- Model-first file import/export.
- Runtime mutation of the registry after service-provider build.

## Acceptance Criteria

- Users can register or obtain a Data Vault model once and reuse it in save/read workflows.
- The registry can represent both existing metadata-first declarations and new Code-First declarations.
- Ambiguous or missing lookups fail with actionable diagnostics.