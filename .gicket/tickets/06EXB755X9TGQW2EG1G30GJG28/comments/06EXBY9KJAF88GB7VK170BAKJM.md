[gicket-bot] PO refinement contract

Summary
- Refined the technical metadata column contracts ticket with bounded v1 defaults, implementation scope, acceptance criteria, and no blocking PO questions.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The ticket is limited to defining reusable contracts for Data Vault technical metadata columns: hash keys, hash diffs, load timestamps, and record source.
- V1 should use one shared contract model in the visible DVault codebase and cover the current source/test layout: src/DVault and tests/DVault.Tests.
- Default column names must be conventional but overrideable per column role; the developer may choose exact API/member names as long as the exposed behavior matches the contract.

Scope In
- Define consistent technical metadata column contracts for hash key, hash diff, load timestamp, and record source roles.
- Document or encode each role's default name, semantic purpose, requiredness expectations, and whether the name can be overridden.
- Ensure the contracts can be applied consistently across vault structures such as hubs, links, and satellites without duplicating incompatible definitions.
- Add focused tests or equivalent verification in tests/DVault.Tests for default names, override behavior, and consistent role representation.

Scope Out
- Generating physical database DDL or migration scripts.
- Implementing complete hub, link, satellite, or PIT/bridge modeling behavior beyond what is needed to consume the metadata contracts.
- Choosing organization-wide naming variants beyond the v1 defaults needed for this ticket.
- Changing workflow columns, ticket metadata, or automation labels.

Open questions
- none

Follow-up questions
- Should future tickets support additional technical metadata roles such as end date, current flag, deleted flag, tenant id, or run id?
- Should a later convention-policy ticket formalize exact physical casing for SQL targets once target database support is introduced?
- Should downstream modeling tickets expose structure-specific aliases for hash keys, such as hub or link key naming patterns?

Risks
- The current repository snapshot does not expose an existing convention-policy implementation, so this ticket should avoid overfitting to database-specific casing or DDL details.
- If later vault-structure tickets define incompatible metadata assumptions, they may need to adapt to this shared contract baseline rather than create parallel definitions.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment