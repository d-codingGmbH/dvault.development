[gicket-bot] PO refinement contract

Summary
- Refined the parent multi-active satellite story around the existing three-child split, ratified the shared driving-key contract and current repository baseline, and found no blocking PO questions.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The story is already split through persisted child relations into 06EZ0NVX3RYPTFZKYCYEH9HB8W for the driving-key contract, 06EZ0NW61GFJN90PSB5N934G2G for persistence behavior, and 06EZ0NWCA6NEZH8VBJNGW4FVHG for the remaining documentation and example slice; no new child tickets or relation edits were needed in this PO pass.
- The authoritative contract for opt-in modeling, save-shape, validation, canonical ordering, and uniqueness is the existing planning artifact docs/plans/multi-active-satellite-driving-key-contract-06EZ0NVX3R-06EZ0NW61G.md, which the persistence child already treats as normative.
- Repository evidence already ratifies the v1 baseline instead of leaving it open: DataVaultPropertyRole includes DrivingKey, the EF translator inserts driving-key columns immediately after the parent hash key for opt-in satellites, and ordinary satellites continue to expose no driving keys by default.
- Current repository behavior also bounds the story by rejecting PIT references to multi-active satellites, so multi-active PIT semantics are not part of this story and should stay deferred.

Scope In
- Deliver the multi-active satellite capability as an opt-in extension to existing satellite modeling rather than a default behavioral change.
- Carry one consistent contract from modeling through explicit save operations, schema translation, uniqueness partitioning, and insert-only history behavior.
- Support deterministic driving-key declaration order, name-based value matching, and unchanged replay suppression within each parent-hash-key-plus-driving-key series.
- Provide user-facing documentation and a minimal example that explain supported multi-active usage and the bounded v1 limitations.

Scope Out
- PIT support for multi-active satellites, including PIT metadata, translation, or snapshot semantics.
- Provider-specific optimized parity, provider-native upsert behavior, and multi-writer conflict guarantees beyond the provider-neutral fallback baseline.
- Same-series same-load-timestamp changed-row conflict semantics, which remain follow-up work.
- Unrelated deferred capability families such as bridge behavior expansion, SaveChanges interception, or broader advanced-hook redesign.

Open questions
- none

Follow-up questions
- After the provider-neutral path is complete, should provider-specific optimized save strategies implement native multi-active partitioning or continue declining those batches until separate parity tickets land?
- Should same-series same-load-timestamp changed-row conflict behavior be specified in a dedicated follow-up ticket before any provider claims stronger concurrency semantics?
- When PIT work is resumed, what explicit contract should govern PIT treatment of multi-active satellites instead of the current hard rejection baseline?

Risks
- If the documentation and example slice drifts from the shared driving-key artifact, the parent story can appear complete while child deliverables encode conflicting caller-visible behavior.
- If any implementation path treats hash diff as part of driving-key identity or includes non-payload metadata in hash diff computation, unchanged suppression and insert-only history semantics can break.
- If optimized provider strategies stop declining multi-active batches before they honor the same partitioning rules as the provider-neutral writer, behavior can diverge by provider.
- If reviewers infer PIT support or multi-writer guarantees from the multi-active story title alone, the ticket can accumulate scope that the current repository baseline explicitly excludes.

Split recommendations
- No further split is needed. Keep 06EZ0NVX3RYPTFZKYCYEH9HB8W as the completed contract slice, keep 06EZ0NW61GFJN90PSB5N934G2G as the persistence slice, and keep 06EZ0NWCA6NEZH8VBJNGW4FVHG as the documentation and example slice for the remaining story deliverables.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment