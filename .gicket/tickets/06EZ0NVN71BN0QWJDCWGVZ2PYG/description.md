<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the parent multi-active satellite story around the existing three-child split, ratified the shared driving-key contract and current repository baseline, and found no blocking PO questions.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The story is already split through persisted child relations into 06EZ0NVX3RYPTFZKYCYEH9HB8W for the driving-key contract, 06EZ0NW61GFJN90PSB5N934G2G for persistence behavior, and 06EZ0NWCA6NEZH8VBJNGW4FVHG for the remaining documentation and example slice; no new child tickets or relation edits were needed in this PO pass.
- The authoritative contract for opt-in modeling, save-shape, validation, canonical ordering, and uniqueness is the existing planning artifact docs/plans/multi-active-satellite-driving-key-contract-06EZ0NVX3R-06EZ0NW61G.md, which the persistence child already treats as normative.
- Repository evidence already ratifies the v1 baseline instead of leaving it open: DataVaultPropertyRole includes DrivingKey, the EF translator inserts driving-key columns immediately after the parent hash key for opt-in satellites, and ordinary satellites continue to expose no driving keys by default.
- Current repository behavior also bounds the story by rejecting PIT references to multi-active satellites, so multi-active PIT semantics are not part of this story and should stay deferred.

### Scope In
- Deliver the multi-active satellite capability as an opt-in extension to existing satellite modeling rather than a default behavioral change.
- Carry one consistent contract from modeling through explicit save operations, schema translation, uniqueness partitioning, and insert-only history behavior.
- Support deterministic driving-key declaration order, name-based value matching, and unchanged replay suppression within each parent-hash-key-plus-driving-key series.
- Provide user-facing documentation and a minimal example that explain supported multi-active usage and the bounded v1 limitations.

### Scope Out
- PIT support for multi-active satellites, including PIT metadata, translation, or snapshot semantics.
- Provider-specific optimized parity, provider-native upsert behavior, and multi-writer conflict guarantees beyond the provider-neutral fallback baseline.
- Same-series same-load-timestamp changed-row conflict semantics, which remain follow-up work.
- Unrelated deferred capability families such as bridge behavior expansion, SaveChanges interception, or broader advanced-hook redesign.

## Acceptance Criteria
- The story ratifies the existing split and shared artifact as the authoritative source for multi-active satellite behavior, so child implementation work does not reopen public contract decisions.
- Multi-active satellites are opt-in through declared driving keys, while ordinary satellites keep current behavior unchanged and expose empty driving-key collections and value sets.
- Validation rejects empty, duplicate, overlapping, missing, extra, or null driving-key names or values, and supplied driving-key values are matched by logical name then reordered into canonical declaration order.
- For opt-in multi-active satellites, projected schema stores driving-key columns immediately after the parent hash-key column and expands the satellite primary key and latest-state partition to parent hash key plus the canonical ordered driving-key tuple plus load timestamp.
- Persistence suppresses unchanged replays only within one parent-hash-key-plus-driving-key partition, inserts a new row when the latest hash diff changes in that partition, and allows same-parent same-load-timestamp rows to coexist when their driving-key tuples differ.
- Documentation and proof coverage include a minimal multi-active satellite example plus the supported-pattern and limitation notes needed to keep v1 expectations bounded.

## Definition of Done
- The parent story, its child tickets, and the shared planning artifact describe one non-conflicting multi-active contract with no remaining PO-level ambiguity about opt-in shape, validation, ordering, or persistence semantics.
- Required implementation and test work covers modeling, save-surface validation, schema translation, unchanged replay suppression, changed-row insertion, and deterministic coexistence across different driving-key tuples without regressing ordinary satellites.
- The minimal documentation example and limitation notes are present and consistent with the repository baseline, including the absence of multi-active PIT support and provider-specific concurrency promises.
- No additional planning materialization is required for this refinement pass beyond the already-existing child tickets and shared contract artifact.

## Implementation Notes
- Normative sources for this story are the completed contract child 06EZ0NVX3RYPTFZKYCYEH9HB8W, the refined persistence child 06EZ0NW61GFJN90PSB5N934G2G, and the shared artifact at docs/plans/multi-active-satellite-driving-key-contract-06EZ0NVX3R-06EZ0NW61G.md.
- Repository code already exposes the bounded translator baseline needed for refinement: DataVaultEfMetadataTranslator projects driving-key columns between parent hash key and HashDiff and uses parentHashKey plus ordered driving keys plus loadTimestamp as the satellite key shape for opt-in multi-active satellites.
- DataVaultPropertyRole.DrivingKey is already part of the provider-neutral metadata vocabulary, so downstream work should treat driving-key columns as an additive satellite role rather than inventing a parallel concept.
- The PIT translator currently rejects multi-active satellite references, which should be documented as a deliberate v1 limitation rather than reopened as a blocker for this story.
- This pass did not create or modify child tickets, relations, attachments, or planning documents because the necessary split and the shared contract artifact were already present and sufficient.

## Open Questions
- none

## Follow-Up Questions
- After the provider-neutral path is complete, should provider-specific optimized save strategies implement native multi-active partitioning or continue declining those batches until separate parity tickets land?
- Should same-series same-load-timestamp changed-row conflict behavior be specified in a dedicated follow-up ticket before any provider claims stronger concurrency semantics?
- When PIT work is resumed, what explicit contract should govern PIT treatment of multi-active satellites instead of the current hard rejection baseline?

## Risks
- If the documentation and example slice drifts from the shared driving-key artifact, the parent story can appear complete while child deliverables encode conflicting caller-visible behavior.
- If any implementation path treats hash diff as part of driving-key identity or includes non-payload metadata in hash diff computation, unchanged suppression and insert-only history semantics can break.
- If optimized provider strategies stop declining multi-active batches before they honor the same partitioning rules as the provider-neutral writer, behavior can diverge by provider.
- If reviewers infer PIT support or multi-writer guarantees from the multi-active story title alone, the ticket can accumulate scope that the current repository baseline explicitly excludes.

## Split Recommendations
- No further split is needed. Keep 06EZ0NVX3RYPTFZKYCYEH9HB8W as the completed contract slice, keep 06EZ0NW61GFJN90PSB5N934G2G as the persistence slice, and keep 06EZ0NWCA6NEZH8VBJNGW4FVHG as the documentation and example slice for the remaining story deliverables.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: add baseline support for multi-active satellites where multiple active records can exist for the same business key at the same time.

Scope:
- Define a driving-key contract for multi-active satellites.
- Persist multi-active satellite records with deterministic uniqueness and insert-only history semantics.
- Document supported patterns and limitations.

Acceptance Criteria:
- Multi-active satellites are opt-in and do not alter normal satellite behavior by default.
- Tests cover duplicate prevention, changed record insertion, unchanged record suppression, and driving-key validation.
- Documentation includes a minimal multi-active satellite example.