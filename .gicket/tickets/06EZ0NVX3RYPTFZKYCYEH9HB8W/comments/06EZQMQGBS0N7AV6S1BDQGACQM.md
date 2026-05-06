[gicket-bot] PO refinement contract

Summary
- Refined the ticket to make the multi-active driving key an explicit, payload-name-based contract that preserves current satellite, parent-hash-key, and hash-diff semantics and leaves no PO blockers.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Multi-active satellites stay opt-in; ordinary satellites keep the current parent-hash-key plus load-timestamp baseline and do not require a driving key.
- The driving key is the additional discriminator that allows multiple concurrently active satellite rows for the same parent; it supplements the parent hash key instead of replacing it.
- A driving-key definition applies to both hub-parent and link-parent satellites because the current satellite metadata model already supports both parent kinds.
- Driving-key members are referenced by the provider-neutral satellite payload names already used by DataVaultSatelliteMetadata and DataVaultSatelliteSaveOperation, not by produced physical column names.
- The parent hash key is implicit in the uniqueness partition and must not be repeated as a driving-key member.
- Technical metadata such as HashDiff, LoadTimestamp, and RecordSource, plus other run-variant metadata, are invalid driving-key members.
- Multi-active support does not introduce a new satellite hash-key algorithm; existing parent hub/link hash-key behavior remains unchanged, and hash diff stays the deterministic payload change detector.

Scope In
- Define the logical driving-key contract for multi-active satellites and its relation to the existing parent-hash-key satellite baseline.
- Define the structural validation rules for valid driving-key definitions, including deterministic member resolution and canonical ordering.
- Define how the driving key interacts with hash diff so downstream persistence can evaluate unchanged duplicates and changed rows within the correct logical partition.
- Provide enough contract clarity for the existing persistence and docs/tests sibling tasks to proceed without inventing placeholder public API names.

Scope Out
- Provider-specific DDL, physical index layouts, migration behavior, or simultaneous multi-writer guarantees.
- Implementing multi-active persistence behavior itself; that remains in ticket 06EZ0NW61GFJN90PSB5N934G2G.
- Writing the user-facing docs and completing test coverage; that remains in ticket 06EZ0NWCA6NEZH8VBJNGW4FVHG.
- Inventing placeholder public type names, method names, or compatibility commitments before a real implementation export requires snapshot review.

Open questions
- none

Follow-up questions
- If stronger semantic validation is later needed, should DVault add explicit payload roles or annotations for stable concurrent-row identity instead of trying to infer stability from names?
- When first-pass multi-active implementation lands, should any new API remain internal until the owning story produces a real public export that updates the snapshot guardrail?
- If a provider later needs stronger same-partition concurrency enforcement than the SQLite-oriented baseline, should that be handled in a separate provider-capability ticket rather than expanding this provider-neutral contract?

Risks
- If implementation allows volatile descriptive fields or metadata-derived values into the driving key, unchanged suppression can degrade into insert-every-time behavior.
- If downstream work computes hash diff from only driving-key members instead of the full payload, non-key payload changes inside one concurrent row partition can be missed.
- If reviewers read this contract as a promise of provider-specific uniqueness indexes or multi-writer conflict handling, downstream delivery can overstate guarantees that the current provider-neutral baseline does not make.

Split recommendations
- No additional split is needed. Keep this ticket as the contract-definition slice, keep persistence behavior in 06EZ0NW61GFJN90PSB5N934G2G, and keep docs/tests in 06EZ0NWCA6NEZH8VBJNGW4FVHG.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment