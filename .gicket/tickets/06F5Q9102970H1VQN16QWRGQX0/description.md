<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined multi-active PIT support into a bounded shared-driving-key expansion over the current hub-parent PIT baseline; no persistent planning writes were applied.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence shows current PIT translation, maintenance, read pipelines, README, and release guidance all reject multi-active PITs, so this story is a real contract expansion rather than a naming cleanup.
- The bounded v1 default is still hub-parent PIT only, but it now allows one shared canonical driving-key set across all referenced multi-active hub-parent satellites; ordinary hub-parent satellites may participate as parent-wide snapshots in the same PIT.
- A multi-active tuple contributes PIT history only after that tuple first becomes visible; from that point onward the existing distinct-timestamp and carry-forward PIT rule is applied per `(parentHashKey, drivingKeyTuple)` without collapsing tuple series.
- Live relation state was left unchanged in this run: the ticket is a child of epic `06F5Q90CSKMGK3NZZ25XTW6W4C`, still has an incoming `blocks` relation from done story `06F5Q90KC6JGQPSP285XQYSPK8`, and still blocks diagnostics story `06F5Q91DR1555RSBQT7KDST684`.
- No child tickets, relation cleanup, description updates, attachments, or planning documents were materialized in this refinement run.

### Scope In
- Expand `DataVaultPitMetadata` support for hub-parent PITs that reference one or more multi-active hub-parent satellites sharing the same canonical driving-key names and order.
- Project PIT driving-key columns, expanded PIT primary-key and index shape, and tuple-aware snapshot-reference columns without regressing ordinary PIT tables.
- Support rebuild, targeted parent maintenance, and PIT-backed as-of reads for multi-active tuple history while preserving the current explicit caller-owned maintenance workflow.
- Add deterministic validation, typed projection exposure, explain and diagnostic updates, public API snapshot changes, and automated coverage for supported and rejected multi-active PIT shapes.

### Scope Out
- Link-parent PITs.
- Multi-active PITs that would require more than one driving-key family, cross-product tuple expansion, or automatic reconciliation of incompatible driving-key sets.
- Automatic, scheduled, background, or `SaveChanges`-triggered PIT maintenance.
- Provider-specific PIT maintenance or PIT read optimization.
- New tuple-filter request parameters or broader artifact and governance changes beyond what existing PIT metadata can already express.

## Acceptance Criteria
- When a PIT references multi-active hub-parent satellites that all resolve to the same canonical driving-key names and order, the generated PIT entity includes those driving-key columns between `ParentHashKey` and `LoadTimestamp`, and the PIT primary-key and baseline traversal index expand to `(ParentHashKey, <DrivingKey...>, LoadTimestamp)`.
- Rebuild and `MaintainParentsAsync(...)` compute PIT history per `(parentHashKey, drivingKeyTuple)` using the current distinct-timestamp and carry-forward rule: tuple-qualified multi-active rows participate only in their own tuple series, ordinary satellites remain parent-wide, and no tuple series is collapsed into another.
- PIT-backed reads keep the existing parent-hash-key request surface but return every visible tuple row for the requested parents at the `asOf` cutoff; each read record and typed PIT projection exposes the canonical driving-key values so same-parent results remain unambiguous.
- Before translation, maintenance, or read execution, deterministic failures identify unsupported or ambiguous shapes such as link-parent PITs, duplicate satellite references, multi-active references with incompatible driving-key sets or order, reference metadata that contradicts the resolved satellite metadata, and any shape that would require cross-product tuple semantics.
- Unit tests, SQLite integration tests, diagnostics and explain coverage, public API snapshot updates, and documentation updates prove both the preserved ordinary PIT baseline and the new multi-active tuple baseline.

## Definition of Done
- Public PIT read and maintenance surfaces, typed PIT projection helpers, and approval snapshots are updated additively without regressing ordinary PIT callers.
- README, PIT maintenance and PIT read guidance, production-adoption documentation, and active release notes no longer describe multi-active PITs as unsupported for the bounded shared-driving-key baseline while preserving explicit exclusions for link-parent PITs, automatic orchestration, and provider-specific optimization.
- SQLite integration coverage demonstrates tuple-aware rebuild, tuple-aware targeted parent maintenance, mixed ordinary-plus-multi-active PIT behavior, and deterministic rejection of incompatible multi-active shapes.
- Explain and diagnostic outputs describe tuple-aware PIT row identity, filters, and projected columns consistently with the implemented maintenance and read behavior.

## Implementation Notes
- Current touch points are `DataVaultEfMetadataTranslator`, `DataVaultPitMaintenanceShapeValidator`, `DefaultDataVaultPitMaintenanceService`, `DataVaultPitReadPipeline`, `DataVaultPitReadRecord`, `DataVaultPitSatelliteSnapshot`, `DataVaultPitProjectionRow`, and `DataVaultDiagnostics`.
- Use `DataVaultSatelliteMetadata.DrivingKeyNames` as the authoritative tuple key set; `DataVaultPitSatelliteReferenceMetadata.IsMultiActive` must not introduce a second naming scheme and should fail deterministically when it contradicts the resolved satellite metadata.
- Keep `DataVaultPitAsOfReadRequest` and `DataVaultPitParentMaintenanceRequest` parent-hash-key based; the bounded v1 change expands returned PIT row identity rather than adding tuple-filter request parameters.
- Typed PIT projection should treat canonical driving-key names as additional PIT-row exact names beside `ParentHashKey` and `LoadTimestamp`, and reject collisions with reserved PIT technical names.
- Ordinary PIT semantics remain the regression baseline: PITs that reference only non-multi-active satellites must keep their current table shape, maintenance behavior, read behavior, and diagnostics.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket add explicit driving-key-tuple filters to `DataVaultPitAsOfReadRequest` for large parent fan-out cases, or is parent-only filtering sufficient beyond this bounded v1 baseline?
- If model-first governance needs explicit multi-active PIT examples or artifact-level tuple-shape diagnostics beyond satellite-driven inference, should that be handled in a separate documentation or schema ticket?
- If teams later need multi-active PITs spanning incompatible driving-key families or cross-product tuple semantics, that should be handled in a separate contract ticket rather than broadening this story.

## Risks
- Supporting multi-active PITs is not just maintenance work: current translation, read records, typed projection helpers, diagnostics, and published guidance all assume at most one visible PIT row per parent hash key.
- The live ticket graph still contains an incoming `blocks` relation from done story `06F5Q90KC6JGQPSP285XQYSPK8`; because no relation cleanup was applied in this run, automation that trusts raw relation state may still treat it as a blocker.
- Tuple-aware PIT maintenance and read paths will increase row counts and in-memory grouping pressure for parents with high driving-key fan-out until a separate optimization ticket changes the current provider-neutral approach.

## Split Recommendations
- No additional split is recommended if this story is bounded to one shared canonical driving-key set across referenced multi-active satellites and keeps tuple filters, model-first follow-ons, and provider-specific optimization out of scope.
- If the release also needs explicit tuple-filter read requests or broader artifact-schema changes, split those into follow-up tickets instead of enlarging this story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Add PIT support for explicitly modeled multi-active satellite state.

Acceptance criteria:
- Defines deterministic PIT row semantics for driving-key tuples and parent hash keys.
- Supports rebuild and targeted parent maintenance without collapsing distinct driving-key states.
- Extends diagnostics for unsupported or ambiguous multi-active PIT shapes.