[gicket-bot] PO refinement contract

Summary
- Reset the epic to a contract-alignment-only blocker state: bridge child 06EZ0NTV4SVAKV98C418T8A3CC has post-integration closure evidence, but its persisted delivery contract still says `ready_for_dev`, so the epic cannot return to PO-critic until that child is refreshed or explicitly reopened.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `cannot_answer` - Confirmed the mismatch. Child 06EZ0NTV4SVAKV98C418T8A3CC is `done`, but its persisted delivery contract still says `ready_for_dev` and describes a remaining hierarchy-validation gap. Later child history shows tester verification and integrator acceptance for branch `ticket/06EZ0NTV4SVAKV98C418T8A3CC-story-add-bridge-table-modeling-and-generation` at commit `9a5d5de0980b`. This run has no bounded tool to rewrite an existing ticket description or reopen that child, so the epic is being held in PO until that child contract is refreshed or explicitly reopened.
- critic-item-2: `answered` - The epic contract is updated to stop treating bridge closure as already settled in parent prose. It now says epic closure remains blocked on refreshing child 06EZ0NTV4SVAKV98C418T8A3CC and cites the child integrator `ACCEPT` decision plus verified commit `9a5d5de0980b` as the concrete closure evidence that the corrected child state must reflect.
- critic-item-3: `answered` - Kept this pass at ticket-contract alignment only. No new implementation scope, bridge feature scope, child implementation tickets, relation edits, attachments, or planning documents were added.
- critic-item-4: `answered` - Confirmed the blocking finding. The parent ticket still says the bridge hierarchy-validation gap is already closed and leaves `Open Questions` as none, while the authoritative child ticket still persists a `ready_for_dev` remaining-gap contract. The epic now treats that conflict as unresolved and blocking.
- critic-item-5: `answered` - Confirmed the ambiguity. Later child history shows the hierarchy-validation fix was developed, tested, and accepted, but the durable child contract still reads as unfinished. The epic now distinguishes current post-integration evidence from durable closure and requires child-ticket contract alignment before epic closure.

Clarifications
- This epic remains a tracking-only closure umbrella with no parent-owned implementation, test, or documentation slice.
- The existing child split remains architecture 06EZ0NSBM3GD7DY11Y4PZMXD28, PIT 06EZ0NSXY2Y1JZ8SSCX177C770, bridge 06EZ0NTV4SVAKV98C418T8A3CC, multi-active satellites 06EZ0NVN71BN0QWJDCWGVZ2PYG, and advanced hooks 06EZ0NWKC9ZME5BSCJFSQEQ02R.
- Current bridge workflow evidence indicates the narrow hierarchy-validation gap was implemented, tested, and accepted on child 06EZ0NTV4SVAKV98C418T8A3CC, but the authoritative child contract still persists pre-integration `ready_for_dev` wording.
- docs/plans/deferred-data-vault-capabilities.md remains the umbrella decision record, with docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md and docs/plans/optional-advanced-configuration-hooks.md as bounded supporting contracts.
- No new child tickets, relation mutations, attachments, or planning documents were materialized in this pass because the blocker is stale contract text on an existing child and no bounded existing-ticket update surface was provided.

Scope In
- Track and ratify the combined provider-neutral deferred-capability baseline already delivered or bounded in the five existing child tickets.
- Preserve epic-level guardrails that deferred capabilities remain opt-in, additive, compatibility-reviewed where public surface changes occur, and non-disruptive to the established default DVault path.
- Align the epic's closure language with durable child-ticket state, especially the bridge child contract mismatch.

Scope Out
- Any new parent-owned product-code, repository-test, or repository-documentation implementation slice.
- Reopening bridge runtime maintenance, bridge row population, PIT refresh or population, link-based PIT, multi-active PIT semantics, provider-specific optimization, or unbounded Data Vault pattern variants.
- Changing the established `AddDVault()`, `UseDataVault()`, `ApplyDataVaultMetadata()`, explicit `IDataVaultSaveService`, deterministic convention, or SQLite-default baseline without separate follow-up work.
- Creating replacement implementation tickets for already integrated bridge validation work instead of fixing the stale child contract.

Open questions
- none

Follow-up questions
- After child-contract alignment, should README or docs-index discoverability link directly to the umbrella deferred-capability decision record and the supporting bridge and hooks contracts?
- If later work reopens PIT refresh, bridge runtime maintenance, or multi-active PIT behavior, should each remain a narrow follow-up ticket rather than expanding this closure umbrella?

Risks
- If the epic is reviewed again before child 06EZ0NTV4SVAKV98C418T8A3CC is refreshed, future reviewers can still infer unfinished bridge work from the stale child contract.
- Later deferred-capability work could erode opt-in or deterministic guardrails if epic closure prose is read as blanket approval instead of bounded child-owned delivery.

Split recommendations
- No additional implementation split is recommended; the existing five-child structure remains the correct decomposition.
- If the runtime can route a narrow PO pass against child 06EZ0NTV4SVAKV98C418T8A3CC, use that pass to refresh the child contract instead of creating new bridge implementation work or expanding this epic.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment