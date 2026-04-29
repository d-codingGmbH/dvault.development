[gicket-bot] PO refinement contract

Summary
- Refinement cannot return to PO-critic as ready because the required prerequisite ticket/relation materialization is still unsatisfied. The attempted prerequisite setup ticket creation was blocked by local trust policy, and repository evidence still shows no source project, no test project, and no implemented AddDVault/UseDataVault API.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - This smoke-test ticket must stay out of dev. It is not being rescoped to include the larger setup work; the contract should continue to require the public startup API implementation, solution/library project, DVault test project, and standard test command before this ticket is developed.
- critic-item-2: `cannot_answer` - Concrete dependency relations are still not satisfied. A prerequisite ticket for the combined startup API implementation and DVault test scaffold was attempted, but gicket-create-ticket was denied by the local trust policy before a ticket id existed, so no blocking relation could be added. No existing verified prerequisite implementation/test-scaffold ticket is available in the provided context to link instead.
- critic-item-3: `answered` - The critic finding stands: this ticket is not ready for developer handoff now. Development is blocked until the public startup API and runnable test scaffold exist or a separate prerequisite ticket/relation can be materialized.
- critic-item-4: `answered` - The compatibility blocker remains verified. The only local AddDVault/UseDataVault material is the planning document; there is no repository source implementation or project containing those public members for this smoke test to target.

Clarifications
- Do not advance this ticket to developer handoff from the current repository baseline.
- This ticket remains a smoke-test task only; it should not absorb solution creation, library project creation, public API implementation, or repository-wide test scaffold setup.
- A prerequisite setup ticket was attempted for the missing public startup API implementation and DVault test scaffold, but creation failed with BOT-LOCAL-TOOL-TRUST-BLOCKED, so no new ticket id or dependency relation was persisted.
- The API-shape planning ticket 06EXB6ZC4M7Q55PXTFBVWP34S0 is useful context for expected AddDVault and UseDataVault names, but it is not itself the implementation/test-scaffold prerequisite requested by PO-critic.

Scope In
- After prerequisites exist, add a smoke test that defines a small consuming DbContext and verifies the public minimal startup path succeeds with default DVault configuration.
- Exercise the public AddDVault and UseDataVault startup surface or an approved equivalent from later implementation planning.
- Keep the smoke test self-contained without external databases, network services, or machine-specific infrastructure.

Scope Out
- Creating the production AddDVault or UseDataVault API implementation from scratch.
- Creating the repository solution skeleton, main library project, package metadata, or general build configuration.
- Creating the DVault test-suite scaffold or deciding the repository-wide test command as hidden scope within this ticket.
- Provider-matrix coverage, broad configuration regression testing, and external database integration tests.

Open questions
- Which persisted ticket should own the prerequisite public AddDVault/UseDataVault implementation plus solution/library/test scaffold, or should the blocked setup-ticket creation be retried by an actor/tool policy that can create and relate that ticket?

Follow-up questions
- After the first smoke test lands, decide separately whether to add provider-specific startup coverage or broader configuration regression tests.

Risks
- The ticket will continue to fail PO-critic review until a concrete prerequisite ticket exists and is linked with a blocking/dependency relation.
- Implementing this ticket before the public API and test scaffold exist would turn it into implicit architecture and repository setup work.
- A smoke test that asserts internal registration or EF convention details instead of observable minimal startup success may become brittle.

Split recommendations
- Create or identify a separate prerequisite setup/API implementation ticket for the solution/library project, AddDVault/UseDataVault implementation, DVault test project, and standard test command, then add a blocking relation from that prerequisite to this smoke-test ticket.
- No split is needed for the smoke-test body itself once the prerequisite implementation and scaffold exist.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment