[gicket-bot] PO-critic review contract

Summary
- The four child tickets are fully evidenced as done and landed on develop, but this parent ticket is now a closure-only roll-up with no residual developer slice, so approving it for dev would contradict its persisted contract and current routing must be corrected by PO.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/relations/TW/0G/06F8KZM6KFZ3WC5MY5NC12B0TW--06F8KZMRXRHRKHV56Y96M4S90G--parentOf.json, /TW/RG/06F8KZM6KFZ3WC5MY5NC12B0TW--06F8KZN2BBPB3XFFXEXGX4N4RG--parentOf.json, /TW/VC/06F8KZM6KFZ3WC5MY5NC12B0TW--06F8KZNBGB8FPW6TK5A8SAJMVC--parentOf.json, and /TW/68/06F8KZM6KFZ3WC5MY5NC12B0TW--06F8KZNNS76TD9Z7ESB173FZ68--parentOf.json persist exactly four child links from the epic.
- git show --name-only ef35f304c, d23b0e481, fa1f7a1f1, and 826b80b9f matches the epic contract: ef35f304c adds docs/plans/provider-identifier-ddl-guardrail-contract.md; d23b0e481 includes src/DCoding.Data.DVault/DataVaultProviderIdentifierPreflight.cs and tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderIdentifierPreflightTests.cs; fa1f7a1f1 includes src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs and src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs; 826b80b9f includes README.md, docs/model-first-governance.md, docs/production-adoption-checklist.md, and docs/releases/v0.29.0.md.
- git branch --contains on ef35f304c, d23b0e481, fa1f7a1f1, and 826b80b9f showed both 'develop' and 'ticket/06F8KZM6KFZ3WC5MY5NC12B0TW-epic-provider-naming-and-ddl-guardrails', so all four cited child integration commits are already landed on develop.
- git diff --name-only develop..HEAD -- README.md docs src tests returned no paths, while git diff --name-only develop..HEAD -- .gicket returned only .gicket files; this epic branch currently carries ticket metadata changes but no remaining product/code/doc delta.
- .gicket/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/description.md says the epic is a closure-only roll-up, says no residual developer-owned work remains and no dev handoff is required, and shows '## Open Questions' followed by '- none'.
- The latest PO refinement comment .gicket/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/comments/06F98Q355HWYS8T8HY4W93A568.md says the epic must stay on the PO-owned closure/completion path rather than moving to dev and that approving this epic for dev would contradict the persisted contract.

Blocking findings
- Child coverage is sufficient, but the parent ticket is explicitly closure-only with no residual developer slice. Under the allowed outcome enum, approve_for_dev would misroute the ticket to dev and contradict both the persisted delivery contract and the latest PO comment.

Required PO actions
- Keep the closure evidence anchored to the four done child tickets and landed develop commits ef35f304c, d23b0e481, fa1f7a1f1, and 826b80b9f when performing the final closure/completion cleanup.

Open issues ledger
- critic-item-1 [required-po-action] Keep the closure evidence anchored to the four done child tickets and landed develop commits ef35f304c, d23b0e481, fa1f7a1f1, and 826b80b9f when performing the final closure/completion cleanup.
- critic-item-2 [blocking-finding] Child coverage is sufficient, but the parent ticket is explicitly closure-only with no residual developer slice. Under the allowed outcome enum, approve_for_dev would misroute the ticket to dev and contradict both the persisted delivery contract and the latest PO comment.

Missing examples / edge cases
- No additional developer-handoff examples are missing; the remaining gap is workflow/routing for a closure-only parent.

Risky assumptions
- This review assumes the lingering .gicket/relations/68/TW/06F8KZNNS76TD9Z7ESB173FZ68--06F8KZM6KFZ3WC5MY5NC12B0TW--blocks.json relation is historical housekeeping, because .gicket/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/comments/06F98QE2D09YXATYSWFS4BKFWR.md dropped that follow-up as obsolete and parent ticket.json currently has isBlocked false.
- This review assumes the workflow can represent a PO-side closure disposition after PO updates the ticket; the current runtime path po-critic.on-success=dev does not match the ticket's closure-only contract.

AC / test suggestions
- Use the final closure note to cite the same direct evidence that passed this audit: the four done child ticket ids, the four landed commits, and the empty develop..HEAD diff for README.md, docs, src, and tests.

Implementation watchouts
- Do not send this epic to dev for new work; develop already contains the child deliverables and this branch has no product diff outside .gicket metadata.
- Do not reopen this parent for provider expansion or consumer-facing physical naming override work; create new follow-up tickets or epics instead.

Non-blocking notes
- All four child descriptions show '## Open Questions' followed by '- none'.

Split recommendations
- No new split is needed for this epic.
- Any future provider-expansion or physical-naming-override work should be tracked on new follow-up tickets or epics, not reopened under 06F8KZM6KFZ3WC5MY5NC12B0TW.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment