[gicket-bot] PO refinement contract

Summary
- Foundation evidence is now present on the branch, so the prior PO blocker is resolved. DVault.slnx, src/DVault, and tests/DVault.Tests are visible in current repository evidence; the metadata abstraction task may continue to PO-critic with the foundation dependency treated as satisfied. The direct blocks relation was attempted again but remains denied by trust policy, so no persisted relation was created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- critic-item-1: `answered` - The previous instruction to keep the ticket in PO refinement was conditional on missing foundation evidence. Current branch evidence now shows DVault.slnx in project-files, src/DVault in src-roots, and tests/DVault.Tests in test-roots, so the condition is satisfied and the ticket no longer needs to remain blocked for missing foundation structure.
- critic-item-2: `answered` - The ticket contract is refreshed against concrete repository evidence: DVault.slnx exists, src/DVault is the v1 production owning root, and tests/DVault.Tests is the v1 unit-test owning root. The developer should implement the metadata abstractions in src/DVault and add focused tests under tests/DVault.Tests.
- critic-item-3: `answered` - A direct persisted blocks relation from foundation ticket 06EXB6XBV95E08R2W9ZQ1PRDPM to this metadata task was attempted, but the local trust policy denied the gicket relation add command with BOT-LOCAL-TOOL-TRUST-BLOCKED. Because the foundation paths now exist, the missing relation is no longer a development blocker for this ticket.
- critic-item-4: `answered` - The blocking finding is closed because the ticket's missing-foundation premise is outdated. Required foundation paths are present in repository evidence, and the absence of a persisted blocks relation no longer needs to enforce sequencing after foundation completion.
- critic-item-5: `answered` - The substantive contract should no longer say the ticket must not enter development before foundation completion. Foundation completion is now evidenced on this branch, so the handoff can proceed to PO-critic using the refreshed contract.

Clarifications
- The prior open question about missing foundation repository evidence is answered: DVault.slnx, src/DVault, and tests/DVault.Tests are present in current branch evidence.
- Use src/DVault as the v1 production owning root and tests/DVault.Tests as the v1 unit-test owning root for this ticket.
- The metadata abstraction scope remains valid for v1; no product-scope split is needed.
- The attempted direct blocks relation from foundation ticket 06EXB6XBV95E08R2W9ZQ1PRDPM to this metadata task failed under trust policy with BOT-LOCAL-TOOL-TRUST-BLOCKED, and no persisted relation was created.
- Because the foundation structure now exists, the missing persisted blocks relation is recorded as a sequencing-history limitation rather than a current blocker.

Scope In
- Define metadata abstractions for hubs, links, and satellites in the DVault library under src/DVault.
- Provide enough documented public or protected members for tests to create and inspect hub, link, and satellite metadata.
- Represent minimum required relationships: hubs have identifying metadata, links connect two or more hub-like endpoints, and satellites are associated with a parent hub or link and descriptive metadata.
- Add focused unit tests under tests/DVault.Tests for valid construction and obvious invalid inputs.

Scope Out
- Creating DVault.slnx, csproj files, src/DVault, tests/DVault.Tests, or other foundation scaffolding.
- Database schema generation, migrations, SQL rendering, physical Data Vault deployment behavior, persistence, serialization, configuration loading, or runtime discovery.
- Advanced Data Vault variants such as effectivity satellites, multi-active satellites, PIT tables, bridge tables, or business vault constructs.
- Adding or enforcing ticket-relation policy beyond the already attempted trust-blocked blocks relation.

Open questions
- none

Follow-up questions
- Later tickets can decide whether to add specialized Data Vault constructs such as effectivity satellites, multi-active satellites, PIT tables, bridge tables, or business vault metadata.
- Later tickets can define serialization/configuration formats, hash computation rules, and stricter naming convention enforcement beyond basic missing-input validation.
- If ticket-relation trust policy later permits it, a relation can still be added for historical board clarity, but it is not required for this ticket to proceed now that foundation evidence is present.

Risks
- The direct blocks relation remains absent because trust policy denied the relation write, so board-level dependency history may be less explicit than the refreshed contract.
- The ticket intentionally defines only a minimal v1 metadata surface, so future Data Vault variants may require additive model changes.

Split recommendations
- No split is needed for the metadata abstraction scope; proceed as one focused modeling task now that the foundation structure exists.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 9

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment