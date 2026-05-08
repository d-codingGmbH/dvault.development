[gicket-bot] PO-critic review contract

Summary
- Return to PO: the epic's bridge-closure story is not yet contract-consistent with its own child evidence, so this tracking-only closure umbrella is not ready for developer handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- The same epic contract says the bridge hierarchy-validation gap is already closed by commit `47bef894a` and its Definition of Done requires parent contract language, child-delivery evidence, and repository evidence to no longer conflict about that bridge baseline.
- gicket-read-ticket-comments: ticket 06EZ0NTV4SVAKV98C418T8A3CC later records dev commit `9a5d5de0980b` to tighten `DataVaultMetadataModel.ValidateHierarchyBridge` plus negative tests, tester verification of 6/6 acceptance criteria, and an integrator `ACCEPT` decision for automatic integration.
- repository-read-text: `docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md` requires hierarchy validation to reject any source link that is not a two-participant self-link over one hub type.
- repository-read-text: `docs/plans/deferred-data-vault-capabilities.md` keeps bridge support opt-in and provider-neutral, with row population and maintenance deferred, which is consistent with a tracking-only epic and inconsistent with reopening parent-owned implementation scope.

Blocking findings
- The parent Definition of Done is not met yet: the epic says the bridge hierarchy-validation gap is already closed, but the authoritative child ticket 06EZ0NTV4SVAKV98C418T8A3CC still persists a `ready_for_dev`/remaining-gap contract. Parent contract, child contract, and later child history are still in conflict.
- The epic still leaves PO-level ambiguity about what closes now versus what needed later bridge work, because the bridge child's durable ticket contract was not refreshed after the later dev/test/integrator evidence. Future reviewers can still read the child as unfinished from the persisted contract alone.

Required PO actions
- Refresh ticket 06EZ0NTV4SVAKV98C418T8A3CC so its persisted delivery contract and handoff state match the post-integration reality, or explicitly reopen that child if the remaining-gap wording is still intended to govern.
- Update epic 06EZ0NS59T2SW9976HHSGP2GF0 to cite the corrected bridge-child state and one concrete closure reference, instead of relying on a child ticket whose persisted contract still says more developer work is required.
- Keep this as a ticket-contract alignment pass only; do not expand the epic into new parent-owned implementation scope.

Open issues ledger
- critic-item-1 [required-po-action] Refresh ticket 06EZ0NTV4SVAKV98C418T8A3CC so its persisted delivery contract and handoff state match the post-integration reality, or explicitly reopen that child if the remaining-gap wording is still intended to govern.
- critic-item-2 [required-po-action] Update epic 06EZ0NS59T2SW9976HHSGP2GF0 to cite the corrected bridge-child state and one concrete closure reference, instead of relying on a child ticket whose persisted contract still says more developer work is required.
- critic-item-3 [required-po-action] Keep this as a ticket-contract alignment pass only; do not expand the epic into new parent-owned implementation scope.
- critic-item-4 [blocking-finding] The parent Definition of Done is not met yet: the epic says the bridge hierarchy-validation gap is already closed, but the authoritative child ticket 06EZ0NTV4SVAKV98C418T8A3CC still persists a `ready_for_dev`/remaining-gap contract. Parent contract, child contract, and later child history are still in conflict.
- critic-item-5 [blocking-finding] The epic still leaves PO-level ambiguity about what closes now versus what needed later bridge work, because the bridge child's durable ticket contract was not refreshed after the later dev/test/integrator evidence. Future reviewers can still read the child as unfinished from the persisted contract alone.

Missing examples / edge cases
- The epic does not yet provide one durable closure breadcrumb that reconciles the stale bridge child contract, the later bridge child comments, and the cited closure commit into a single reviewer-friendly trail.
- There is no parent-level trace list that maps each child ticket to its exact closing repository/comment evidence, so closure still depends on reconstructing scattered records.

Risky assumptions
- Assuming commit `47bef894a` is obviously the same closure event as the bridge child's later verified/integrated history around `9a5d5de0980b` without a direct cited bridge-history link in the ticket contract.
- Assuming future closure reviewers will read late bot comments before trusting the child ticket's delivery contract.

AC / test suggestions
- Add an epic-level acceptance note that the bridge child's persisted contract/handoff must be closure-aligned with the accepted integrated fix before the epic can pass PO-critic.
- Add one explicit closure-evidence bullet mapping each child ticket to the repository, doc, or comment artifact used for epic closure, especially for the bridge hierarchy-validation fix.

Implementation watchouts
- If the stale bridge child contract remains, automation or human reviewers may route the epic back into developer work even though later comments show the bridge fix was implemented and integrated.
- Relying on comment history alone for reconciliation is fragile because the persisted child contract is easier to find and currently says the opposite.

Non-blocking notes
- The epic itself passes the specific `## Open Questions` gate because its persisted contract says `none`.
- The repository planning docs reviewed here are consistent with a tracking-only umbrella over opt-in, additive deferred capabilities rather than new parent-owned implementation.

Split recommendations
- No new split is needed; return this to PO for contract/state alignment on existing ticket 06EZ0NTV4SVAKV98C418T8A3CC and then rerun PO-critic on the epic.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment