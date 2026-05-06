<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket to make the multi-active driving key an explicit, payload-name-based contract that preserves current satellite, parent-hash-key, and hash-diff semantics and leaves no PO blockers.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Multi-active satellites stay opt-in; ordinary satellites keep the current parent-hash-key plus load-timestamp baseline and do not require a driving key.
- The driving key is the additional discriminator that allows multiple concurrently active satellite rows for the same parent; it supplements the parent hash key instead of replacing it.
- A driving-key definition applies to both hub-parent and link-parent satellites because the current satellite metadata model already supports both parent kinds.
- Driving-key members are referenced by the provider-neutral satellite payload names already used by DataVaultSatelliteMetadata and DataVaultSatelliteSaveOperation, not by produced physical column names.
- The parent hash key is implicit in the uniqueness partition and must not be repeated as a driving-key member.
- Technical metadata such as HashDiff, LoadTimestamp, and RecordSource, plus other run-variant metadata, are invalid driving-key members.
- Multi-active support does not introduce a new satellite hash-key algorithm; existing parent hub/link hash-key behavior remains unchanged, and hash diff stays the deterministic payload change detector.

### Scope In
- Define the logical driving-key contract for multi-active satellites and its relation to the existing parent-hash-key satellite baseline.
- Define the structural validation rules for valid driving-key definitions, including deterministic member resolution and canonical ordering.
- Define how the driving key interacts with hash diff so downstream persistence can evaluate unchanged duplicates and changed rows within the correct logical partition.
- Provide enough contract clarity for the existing persistence and docs/tests sibling tasks to proceed without inventing placeholder public API names.

### Scope Out
- Provider-specific DDL, physical index layouts, migration behavior, or simultaneous multi-writer guarantees.
- Implementing multi-active persistence behavior itself; that remains in ticket 06EZ0NW61GFJN90PSB5N934G2G.
- Writing the user-facing docs and completing test coverage; that remains in ticket 06EZ0NWCA6NEZH8VBJNGW4FVHG.
- Inventing placeholder public type names, method names, or compatibility commitments before a real implementation export requires snapshot review.

## Acceptance Criteria
- The ticket contract states that a multi-active satellite is opt-in and that ordinary satellites keep the current default behavior unchanged.
- The contract states that concurrently active rows are distinguished by parent hash key plus an explicit driving key, where the driving key is a non-empty set of distinct declared payload fields resolved by provider-neutral payload name.
- Validation rejects missing or structurally invalid driving-key definitions, including duplicate members, unknown payload members, produced physical column names, parent hash key, technical metadata members, and other metadata-derived or run-variant members that are unstable by contract.
- The contract states that parent hash-key computation remains unchanged and that hash diff remains the deterministic digest of the full satellite payload state rather than a replacement for the driving key.
- The contract gives downstream persistence work the logical partition rule: unchanged duplicate suppression and changed row insertion are evaluated within each parent-hash-key-plus-driving-key partition, preserving insert-only history semantics.

## Definition of Done
- The ticket text leaves no blocking PO-level questions about what a driving key is, what it may reference, or how it differs from parent hash key and hash diff.
- Downstream persistence and docs/test tickets can implement against one bounded contract without reopening multi-active identity, validation, or determinism decisions.
- The refined contract keeps public API naming optional until the owning implementation change introduces a real export subject to the existing snapshot guardrail.
- Non-goals and unsupported assumptions are explicit so reviewers do not infer provider-specific schema or concurrency promises from this contract ticket.

## Implementation Notes
- Build the contract on the current satellite baseline: DataVaultSatelliteMetadata already models one parent reference, declared payload names, and the technical metadata roles HashDiff, LoadTimestamp, and RecordSource.
- Use the same provider-neutral payload-name namespace already used by DataVaultSatelliteSaveOperation payload values; do not define driving keys in terms of translated physical column names.
- Treat the driving key logically as a distinct field set, but store or compare its canonical representation in deterministic payload declaration order so repeated model builds yield the same contract shape.
- When a deterministic tuple or digest representation is needed, reuse the existing stable-hash normalization rules rather than introducing provider-specific ordering or formatting rules.
- Keep hash diff scoped to full payload change detection and exclude timestamps, record source, produced table or column names, and other non-payload metadata from the hash input, consistent with the stable hashing contract.
- Downstream persistence should mirror current ordinary satellite behavior inside each logical partition: only the latest hash diff suppresses an unchanged repeat, and a previously seen hash diff may reappear later as a new history row after an intervening change.
- Apply the same contract to hub-parent and link-parent satellites; no separate hub-only v1 restriction is needed from current repository evidence.

## Open Questions
- none

## Follow-Up Questions
- If stronger semantic validation is later needed, should DVault add explicit payload roles or annotations for stable concurrent-row identity instead of trying to infer stability from names?
- When first-pass multi-active implementation lands, should any new API remain internal until the owning story produces a real public export that updates the snapshot guardrail?
- If a provider later needs stronger same-partition concurrency enforcement than the SQLite-oriented baseline, should that be handled in a separate provider-capability ticket rather than expanding this provider-neutral contract?

## Risks
- If implementation allows volatile descriptive fields or metadata-derived values into the driving key, unchanged suppression can degrade into insert-every-time behavior.
- If downstream work computes hash diff from only driving-key members instead of the full payload, non-key payload changes inside one concurrent row partition can be missed.
- If reviewers read this contract as a promise of provider-specific uniqueness indexes or multi-writer conflict handling, downstream delivery can overstate guarantees that the current provider-neutral baseline does not make.

## Split Recommendations
- No additional split is needed. Keep this ticket as the contract-definition slice, keep persistence behavior in 06EZ0NW61GFJN90PSB5N934G2G, and keep docs/tests in 06EZ0NWCA6NEZH8VBJNGW4FVHG.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: define the driving-key contract for multi-active satellites.

Acceptance Criteria:
- The contract identifies which fields distinguish concurrently active satellite rows for the same parent key.
- Validation rejects missing or unstable driving-key definitions.
- Hash key and hash diff behavior remains deterministic and documented.