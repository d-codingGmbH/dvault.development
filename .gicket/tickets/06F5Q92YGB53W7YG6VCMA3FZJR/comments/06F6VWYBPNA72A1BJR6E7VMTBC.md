[gicket-bot] PO refinement contract

Summary
- Verified live ticket revision 06F6VVSCJQTP7624A6PAKQG14R already re-baselines this story to a residual diagnostic-only typed-read follow-up: docs ownership stays on 06F5Q93H60W6X8FJ88PWTR6NG4, zero new typed-read code fixes is accepted, and the ticket is ready to return to PO-critic.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The explicit residual deliverable is a diagnostic-only follow-up: add deterministic non-satellite typed-read outcomes for unsupported PIT, bridge, dynamic-query-required, model-first-out-of-contract, and helper-skipped shapes so they surface DMV1963, DMV1964, DMV1967, DMV1968, and DMV1969 instead of silently dropping out. Satellite generation remains completed in 06F5Q92AHG0ZCTVQGC6NAYVP9C, PIT/bridge helper generation remains completed in 06F5Q92R02HB7FCE1AWKXPTMRW, and documentation remains separate in 06F5Q93H60W6X8FJ88PWTR6NG4.
- critic-item-2: `answered` - Documentation ownership is now explicit and removed from this story. README, analyzer README, model-first guidance, checklist, and release-note rollup stay on docs task 06F5Q93H60W6X8FJ88PWTR6NG4, while this ticket owns only the residual diagnostic follow-up that the docs task still depends on.
- critic-item-3: `answered` - Zero new typed-read code fixes is acceptable for this ticket. The residual deliverable is diagnostics only, and the contract now explicitly rejects code fixes for DMV1963, DMV1964, DMV1967, DMV1968, and DMV1969 because those cases do not have one deterministic single local edit.
- critic-item-4: `answered` - The ticket now states a distinct remaining developer-owned deliverable: implement the missing DMV1963, DMV1964, DMV1967, DMV1968, and DMV1969 residual diagnostic paths and tests on top of the already-landed satellite baseline, without reopening the completed child implementation slices.
- critic-item-5: `answered` - Documentation ownership ambiguity is resolved by stripping docs acceptance criteria from this story and leaving the typed-read documentation rollup on 06F5Q93H60W6X8FJ88PWTR6NG4. The current ticket keeps only the residual diagnostic work that still blocks that docs task.
- critic-item-6: `answered` - The residual code-fix decision is explicit: this ticket ships zero new typed-read code fixes. Its remaining scope is limited to diagnostic emission and regression tests for the unsupported or skipped non-satellite paths.

Clarifications
- 06F5Q92AHG0ZCTVQGC6NAYVP9C and 06F5Q92R02HB7FCE1AWKXPTMRW are completed landed evidence and are not reopened by this story.
- Documentation ownership remains on 06F5Q93H60W6X8FJ88PWTR6NG4, and the current outgoing blocks relation to that docs task remains consistent with the split.
- No new child ticket, relation edit, attachment, planning document, or description write was required in this turn because the live persisted description already reflects the re-baselined contract.

Scope In
- Add explicit DMV1963, DMV1964, DMV1967, DMV1968, and DMV1969 outcomes for unsupported or skipped non-satellite typed-read shapes.
- Turn silent authoritative support-bundle skip paths into deterministic diagnostics with no helper emission for the affected shape.
- Add analyzer or generator tests for each residual diagnostic path while preserving the landed satellite baseline.

Scope Out
- Satellite helper generation already landed in 06F5Q92AHG0ZCTVQGC6NAYVP9C.
- PIT or bridge helper generation, runtime API expansion, and provider-specific SQL remain out of scope.
- README, analyzer README, model-first, checklist, and release-note updates belong to 06F5Q93H60W6X8FJ88PWTR6NG4.
- New typed-read code fixes are out of scope for this ticket.

Open questions
- none

Follow-up questions
- Should a later compatibility ticket deprecate the legacy DVaultReadModelMetadataSourceFingerprint property after the residual diagnostic surface is complete?
- After this diagnostic-only follow-up lands, should docs task 06F5Q93H60W6X8FJ88PWTR6NG4 present DMV1963 through DMV1969 as one consolidated typed-read section or keep satellite and unsupported-shape guidance separate?

Risks
- If unsupported PIT, bridge, dynamic, model-first, or helper-skipped cases continue to drop out silently, consumers and the docs task cannot distinguish not implemented from misconfigured.
- Expanding this follow-up into runtime helper generation or speculative code fixes would reopen already-completed implementation slices.
- Documentation rollup remains blocked on this residual diagnostic surface until the current story lands.

Split recommendations
- No further split is recommended. The residual diagnostic-only follow-up is smaller than the completed generator slices and separate from the documentation rollup.
- Keep the documentation rollup on 06F5Q93H60W6X8FJ88PWTR6NG4 rather than reabsorbing docs scope into this story.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment