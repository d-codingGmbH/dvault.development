[gicket-bot] PO-critic review contract

Summary
- Ticket is sufficiently refined for a developer to produce the architecture-level hook plan without runtime implementation.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB6QX6JJX9H7CZT3YAXSAD4/description.md contains the durable Delivery Contract with PO Handoff decision ready_for_po_critic and Open Questions set to none.
- The Delivery Contract scope includes naming, hashing, record source resolution, timestamp sourcing/formatting, and provider behavior, with explicit scope-out for production implementation and premature method/file naming.
- .gicket/tickets/06EXB6QX6JJX9H7CZT3YAXSAD4/comments/06EXCJ800VW7QM293333RJ1WNG.md records the PO refinement contract, planned ticket updates, Open questions: none, and split recommendations: none.
- git ls-tree --name-only HEAD lists only .gicket-bot and .gicket at repository root, supporting the contract's statement that no source/test layout constrains a concrete API yet.
- .gicket/milestones/06EXB6F6Z8HMH2BQKDY1ZKQCPC.json names the milestone Foundation and architecture with description 'Project setup, shared standards, and core architecture decisions.'

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The contract relies on a developer creating an architecture-level plan/document rather than runtime code; this is acceptable because implementation is explicitly out of scope, but the dev handoff should preserve that boundary.

AC / test suggestions
- Keep acceptance focused on a documented plan with default behavior for each hook category and no required user configuration for the default path.

Implementation watchouts
- Do not bind the plan to class names, method names, parameter names, or source file locations until the source/test layout exists.
- Make provider behavior a generic bounded extension point and leave provider-specific option matrices to later tickets.

Non-blocking notes
- The local worktree shows many modified .gicket files in git status/diff stat, apparently metadata or line-ending churn; the ticket's persisted contract content remains readable and coherent for this review.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment