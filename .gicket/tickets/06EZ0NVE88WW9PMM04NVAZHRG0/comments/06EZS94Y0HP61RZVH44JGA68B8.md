[gicket-bot] PO refinement contract

Summary
- Reset the child contract to a blocked-on-parent docs task: parent story 06EZ0NTV4SVAKV98C418T8A3CC must define the authoritative bridge surface before this ticket can return to PO-critic, and no planning artifacts or relations were changed in this pass.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now makes the refresh step explicit: after parent story 06EZ0NTV4SVAKV98C418T8A3CC is refined, this child must be re-checked and updated to match any concrete bridge names or shape details before it returns to PO-critic.
- critic-item-2: `answered` - This child is not being sent forward. The handoff is reset to needs_po_clarification because parent story 06EZ0NTV4SVAKV98C418T8A3CC remains the prerequisite source of truth for bridge naming and shape, so developer handoff would be premature.
- critic-item-3: `answered` - The repository still does not expose a source-backed bridge surface. Bridge capability is only defined at architecture level, bridge ownership is delegated to parent story 06EZ0NTV4SVAKV98C418T8A3CC, current EF translation only emits hub, link, and satellite entities, DataVaultAnnotationNames defines no bridge annotations, and there is no attached bridge artifact on the parent or child ticket. The child contract now explicitly forbids guessing bridge names or shapes before parent refinement.

Clarifications
- The authoritative bridge source for this child is the refined contract of parent story 06EZ0NTV4SVAKV98C418T8A3CC; the architecture decision record is only the deferred-capability baseline.
- This child remains documentation-only and stays blocked until the parent contract defines concrete bridge naming and shape details.
- The only worked example in scope is one minimal many-to-many traversal scenario; hierarchy-style traversal remains deferred unless a later ticket scopes it separately.

Scope In
- Document bridge tables as an opt-in v0.5 deferred capability layered on the existing hub, link, and satellite baseline.
- Record the guardrail that the docs must consume the parent bridge contract as the source of bridge names, annotations, types, and table shape.
- Provide exactly one minimal many-to-many traversal example after the parent contract makes the bridge surface concrete.
- Reuse current repository vocabulary around AddDVault(), UseDataVault(), ApplyDataVaultMetadata(), and IDataVaultSaveService.

Scope Out
- Implementing bridge modeling, EF metadata projection, save behavior, validation, or tests.
- Inventing bridge API, annotation, metadata, table, or example names before parent story 06EZ0NTV4SVAKV98C418T8A3CC defines them.
- Hierarchy-depth semantics, recursive traversal behavior, provider-specific DDL or migrations, PIT interactions, and multi-active interactions beyond explicit deferred notes.
- Runnable sample applications or broad README expansion beyond the bounded bridge documentation task.

Open questions
- Parent story 06EZ0NTV4SVAKV98C418T8A3CC still needs PO refinement to publish the authoritative bridge surface naming and shape that this documentation child must follow.

Follow-up questions
- After the parent bridge contract is refined and this child is implemented, should README.md add a short cross-link to the bridge documentation page?
- If hierarchy-style traversal later needs a worked example, should that become a separate follow-up docs ticket rather than expanding this many-to-many-focused child?

Risks
- Parent story 06EZ0NTV4SVAKV98C418T8A3CC may introduce bridge names or metadata shape details that require one more sync pass on this child before implementation begins.
- If documentation work starts before the parent contract exists, the example will drift into guessed bridge terms that the repository does not currently prove.

Split recommendations
- No split is recommended while the child remains a bounded documentation task blocked on the parent bridge contract.
- If later scope needs a hierarchy-specific walkthrough or multiple worked examples, create a separate follow-up docs ticket after the parent bridge surface is defined.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment