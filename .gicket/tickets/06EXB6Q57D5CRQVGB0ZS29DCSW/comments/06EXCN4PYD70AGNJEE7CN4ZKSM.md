[gicket-bot] PO refinement contract

Summary
- Refined the deferred Data Vault capabilities ticket using ticket, comment, relation, attachment, and repository context. The ticket is bounded as architecture documentation only, with no product-code implementation and no child-ticket materialization required in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This ticket documents non-MVP Data Vault capabilities only; PIT table generation, bridge table generation, multi-active satellites, and provider-specific optimizations must be visible as deferred work and explicitly excluded from the MVP package.
- The current ticket already has a parentOf incoming relation from 06EXB6PNA0VA1XTR85B6X3T7ZG, so it sits under an existing parent planning item rather than needing a new split relation.
- No ticket attachments are currently present. The referenced repository documents and branch snapshot are sufficient planning context for this refinement pass.

Scope In
- Create or update architecture planning documentation that names PIT tables, bridge tables, multi-active satellites, and provider-specific optimizations as future capabilities.
- Describe each deferred capability at a product-planning level, including why it is not part of the MVP release.
- Leave clear extension space for future epics or stories without defining implementation commitments for the first package.

Scope Out
- Implementing PIT, bridge, multi-active satellite, or provider-specific generation behavior.
- Changing product code, tests, package structure, or runtime behavior.
- Creating the future capability epics now unless a later PO pass decides to materialize them separately.

Open questions
- none

Follow-up questions
- When implementation planning starts, decide whether PIT, bridge, multi-active satellite, and provider optimization work should become separate epics or smaller capability stories.
- Later provider work should decide which database engines need first-class optimization support beyond the Sqlite-oriented MVP baseline.
- Future Data Vault scope should decide how much automation is expected for each deferred capability versus documentation-only guidance.

Risks
- If the deferred-capabilities document uses overly specific implementation language, downstream work may treat non-MVP features as committed MVP scope.
- If future hooks are too vague, later epics may need additional PO refinement before development can start.

Split recommendations
- No immediate split is recommended. Keep this ticket as one documentation task, and consider separate future epics for PIT generation, bridge generation, multi-active satellites, and provider-specific optimizations after the MVP documentation baseline lands.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment