[gicket-bot] PO refinement contract

Summary
- Refined the delivery contract to enumerate the v1 override surface and set an explicit boundary with sibling ticket 06EXB75NX7Z0DY7X0BD0YFZECM. No child tickets, relations, attachments, or planning documents are needed; the item is ready for PO-critic.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The v1 override surface is the parent story's concrete name families: hub table names, link table names, satellite table names, Data Vault technical column names, index names, and constraint names. Broader future variants are deferred; this ticket owns override points for those families, not additional naming strategies.
- critic-item-2: `answered` - Sibling ticket 06EXB75NX7Z0DY7X0BD0YFZECM owns the concrete default table/column naming rules and examples. This ticket owns the public override abstraction, options hook, built-in default-policy plumbing, and tests proving default and custom policy paths. The default implementation here must delegate to or preserve the sibling-owned default rules once available, and must not independently finalize casing, pluralization, reserved-word, or technical-column naming semantics beyond providing a deterministic placeholder/default path needed to exercise the hook.
- critic-item-3: `answered` - Acceptance criteria now specify that a custom policy must affect produced hub table names, link table names, satellite table names, technical column names, index names, and constraint names when those names are produced by the modeling flow. Default behavior is sufficient when callers can use the modeling API with no naming policy configured, all produced names come from the built-in/default policy path, and the public options surface remains optional.
- critic-item-4: `answered` - The under-specified terms 'modeling names' and 'relevant modeled names' are replaced by an explicit v1 target list: hub table names, link table names, satellite table names, Data Vault technical column names, index names, and constraint names. The public abstraction may expose this as one interface or equivalent public contract, but it must cover these families as override targets.
- critic-item-5: `answered` - The default-policy requirement is narrowed to plumbing and fallback behavior, not ownership of final default naming semantics. Ticket 06EXB75NX7Z0DY7X0BD0YFZECM remains the source of truth for default table/column naming rules and examples; this task must avoid duplicating those decisions and should integrate with that sibling's rules when both slices are implemented.

Clarifications
- The v1 naming-policy override surface covers hub table names, link table names, satellite table names, Data Vault technical column names, index names, and constraint names.
- This ticket owns the public override abstraction and options hook in the modeling configuration surface; it does not own the detailed default naming convention decisions for casing, singular/plural handling, reserved words, or examples.
- Sibling ticket 06EXB75NX7Z0DY7X0BD0YFZECM owns the concrete default table and column naming policy. This ticket's default path must preserve or delegate to that sibling-owned behavior when available.
- The current repository branch has no visible source or test roots, so implementation should follow the first owning modeling project and test layout introduced by the foundation/modeling work.
- No child tickets, relation writes, attachments, or planning documents were created; the contract change is sufficient to address the critic findings.

Scope In
- Define a public naming policy interface or equivalent public abstraction that consumers can implement.
- Expose an optional naming policy hook through the modeling options/configuration surface, aligned with DataVaultModelOptions from the extension-shape planning document.
- Support override points for hub table names, link table names, satellite table names, Data Vault technical column names, index names, and constraint names.
- Provide default-policy plumbing so callers who do not configure naming still get deterministic built-in names without user action.
- Add tests covering both the no-custom-policy path and a custom-policy path affecting the enumerated v1 name families that the modeling flow produces.

Scope Out
- Changing ticket workflow, labels, or runtime handoff metadata.
- Finalizing default naming convention semantics owned by sibling ticket 06EXB75NX7Z0DY7X0BD0YFZECM, including casing, singular/plural handling, reserved-word handling, and detailed examples.
- Multiple built-in naming policy variants beyond the single default path required for v1.
- Retrofitting downstream packages or integrations that do not yet exist in the visible repository surface.
- Creating unrelated source, solution, or test structure outside the owning project layout established by the foundation/modeling implementation work.

Open questions
- none

Follow-up questions
- After sibling ticket 06EXB75NX7Z0DY7X0BD0YFZECM is refined or implemented, confirm whether this ticket should explicitly depend on that work before development sequencing, or whether the two can be implemented together under the parent story.
- After the first modeling API is in place, decide whether additional built-in naming conventions are worth adding as separate policies.
- Once documentation structure exists, consider adding user-facing examples for custom naming policies.

Risks
- Public API shape may be hard to change later, so the initial naming policy contract should stay minimal while still covering the six v1 name families.
- If this ticket and sibling 06EXB75NX7Z0DY7X0BD0YFZECM are developed independently, teams must coordinate to avoid conflicting default naming semantics.

Split recommendations
- No split recommended for this ticket; interface, options hook, default plumbing, and tests remain one cohesive implementation slice. The sibling default-policy ticket is the separate boundary for detailed default naming rules.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment