[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket has a clear override-policy intent and no contract Open Questions, but the handoff is not yet bounded against the parent/sibling naming-policy scope.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted .gicket/tickets/06EXB75XTWD7FTRAFE5GNDCS5R/description.md has Delivery Contract Scope In for a public naming policy abstraction, optional custom policy hook, default policy, and default/custom tests; its ## Open Questions section says '- none'.
- Read-only root inspection with find . -maxdepth 2 -type f showed only .gicket/.gicket-bot metadata and obj artifacts at the repository top level; no source or test roots were visible in that bounded check.
- git show --stat --oneline b688398078b0 shows the PO handoff changed this ticket's description, comments, events, and ticket.json, with 195 insertions and 7 deletions.
- Relation file .gicket/relations/YG/5R/06EXB75DX3YAJFMJ6TNHVPAWYG--06EXB75XTWD7FTRAFE5GNDCS5R--parentOf.json states parentOf from parent story 06EXB75DX3YAJFMJ6TNHVPAWYG to this task.
- Parent story .gicket/tickets/06EXB75DX3YAJFMJ6TNHVPAWYG/description.md scopes deterministic naming for hubs, links, satellites, technical columns, indexes, and constraints, with AC that users can override naming policy when needed.
- Relation scan for 06EXB75XTWD7FTRAFE5GNDCS5R and 06EXB75NX7Z0DY7X0BD0YFZECM showed only their parentOf relations from 06EXB75DX3YAJFMJ6TNHVPAWYG, with no blocks/dependency relation between the sibling default-policy task and this override-points task.

Blocking findings
- The contract says the public abstraction must cover 'modeling names' and 'relevant modeled names' but does not enumerate the v1 override targets. The parent story does enumerate hubs, links, satellites, technical columns, indexes, and constraints, so the child task is under-specified for a public API handoff.
- The contract includes providing a default policy and default-path tests, but sibling ticket 06EXB75NX7Z0DY7X0BD0YFZECM separately owns default table/column naming policy and is still needs-po. Without a dependency or boundary, developers could implement conflicting or duplicate default behavior.

Required PO actions
- Update this ticket's delivery contract to either include the parent story's concrete name families as the v1 override surface or explicitly scope which subset is in and which are deferred.
- Clarify the relationship with sibling ticket 06EXB75NX7Z0DY7X0BD0YFZECM: add a dependency/boundary in the contract or adjust scope so this task does not make unstated decisions about default naming conventions owned elsewhere.
- Revise acceptance criteria so a developer can tell exactly which produced names must be affected by a custom policy and what default behavior is sufficient for this task.

Open issues ledger
- critic-item-1 [required-po-action] Update this ticket's delivery contract to either include the parent story's concrete name families as the v1 override surface or explicitly scope which subset is in and which are deferred.
- critic-item-2 [required-po-action] Clarify the relationship with sibling ticket 06EXB75NX7Z0DY7X0BD0YFZECM: add a dependency/boundary in the contract or adjust scope so this task does not make unstated decisions about default naming conventions owned elsewhere.
- critic-item-3 [required-po-action] Revise acceptance criteria so a developer can tell exactly which produced names must be affected by a custom policy and what default behavior is sufficient for this task.
- critic-item-4 [blocking-finding] The contract says the public abstraction must cover 'modeling names' and 'relevant modeled names' but does not enumerate the v1 override targets. The parent story does enumerate hubs, links, satellites, technical columns, indexes, and constraints, so the child task is under-specified for a public API handoff.
- critic-item-5 [blocking-finding] The contract includes providing a default policy and default-path tests, but sibling ticket 06EXB75NX7Z0DY7X0BD0YFZECM separately owns default table/column naming policy and is still needs-po. Without a dependency or boundary, developers could implement conflicting or duplicate default behavior.

Missing examples / edge cases
- No persisted example identifies a concrete default produced name or a concrete custom-policy-produced name.
- No edge case states whether override coverage must include hubs, links, satellites, technical columns, indexes, and constraints, or only the first modeling flow introduced by this task.
- No ticket-level expectation covers overlap with sibling edge cases: singular/plural inputs, casing, reserved words, and technical columns.

Risky assumptions
- Assuming 'modeling names' means the same set of name families listed in the parent story.
- Assuming this task can define a default policy before the sibling default-policy ticket is refined.
- Assuming the first modeling API shape can be safely chosen by the developer without PO specifying the minimum public override surface.

AC / test suggestions
- Add an AC that names the v1 override targets, or states that the custom policy must affect every naming output produced by the first modeling flow.
- Add an AC clarifying whether detailed default naming rules are in this task or delegated to 06EXB75NX7Z0DY7X0BD0YFZECM.
- Add test guidance that the custom-policy test must assert a changed produced name for at least one named in-scope modeling output, not just construction of an options object.

Implementation watchouts
- Public naming API shape is hard to change later; the PO contract should bound the minimum override surface before dev starts.
- Repository inspection found no established source/test layout, so dev should not infer compatibility with an existing public API.
- Keep multiple built-in naming variants out of scope unless PO explicitly changes this ticket.

Non-blocking notes
- The persisted contract has no unresolved Open Questions.
- Ticket comments read through gicket-read-ticket-comments returned 9 comments, all observed as bot claim/refinement/handoff/lease history rather than human objection threads.
- No split is needed solely for interface, options hook, default path, and tests once the sibling/default-policy boundary is clarified.

Split recommendations
- Do not split this task further; clarify dependency/scope with sibling 06EXB75NX7Z0DY7X0BD0YFZECM before handoff.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment