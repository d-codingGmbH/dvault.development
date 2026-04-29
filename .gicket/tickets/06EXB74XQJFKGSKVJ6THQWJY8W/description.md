<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Foundation evidence is now present on the branch, so the prior PO blocker is resolved. DVault.slnx, src/DVault, and tests/DVault.Tests are visible in current repository evidence; the metadata abstraction task may continue to PO-critic with the foundation dependency treated as satisfied. The direct blocks relation was attempted again but remains denied by trust policy, so no persisted relation was created.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The prior open question about missing foundation repository evidence is answered: DVault.slnx, src/DVault, and tests/DVault.Tests are present in current branch evidence.
- Use src/DVault as the v1 production owning root and tests/DVault.Tests as the v1 unit-test owning root for this ticket.
- The metadata abstraction scope remains valid for v1; no product-scope split is needed.
- The attempted direct blocks relation from foundation ticket 06EXB6XBV95E08R2W9ZQ1PRDPM to this metadata task failed under trust policy with BOT-LOCAL-TOOL-TRUST-BLOCKED, and no persisted relation was created.
- Because the foundation structure now exists, the missing persisted blocks relation is recorded as a sequencing-history limitation rather than a current blocker.

### Scope In
- Define metadata abstractions for hubs, links, and satellites in the DVault library under src/DVault.
- Provide enough documented public or protected members for tests to create and inspect hub, link, and satellite metadata.
- Represent minimum required relationships: hubs have identifying metadata, links connect two or more hub-like endpoints, and satellites are associated with a parent hub or link and descriptive metadata.
- Add focused unit tests under tests/DVault.Tests for valid construction and obvious invalid inputs.

### Scope Out
- Creating DVault.slnx, csproj files, src/DVault, tests/DVault.Tests, or other foundation scaffolding.
- Database schema generation, migrations, SQL rendering, physical Data Vault deployment behavior, persistence, serialization, configuration loading, or runtime discovery.
- Advanced Data Vault variants such as effectivity satellites, multi-active satellites, PIT tables, bridge tables, or business vault constructs.
- Adding or enforcing ticket-relation policy beyond the already attempted trust-blocked blocks relation.

## Acceptance Criteria
- Tests can construct valid hub metadata and assert required identifying properties are retained.
- Tests can construct valid link metadata with at least two related endpoints and assert relationships are retained.
- Tests can construct valid satellite metadata associated with a parent hub or link and assert required properties are retained.
- Creating metadata with null, empty, or whitespace required names fails with a clear argument or validation exception.
- Creating link metadata without the minimum required endpoints fails validation.
- Creating satellite metadata without a required parent relationship fails validation.
- Public or protected members introduced for the abstractions include XML documentation where applicable.

## Definition of Done
- The implementation compiles in the existing DVault solution or project structure.
- Relevant unit tests are added under tests/DVault.Tests and pass.
- The public modeling API is intentionally small, documented, and consistent with established repository naming and layout conventions.
- Validation behavior is deterministic and covered by tests for the missing-input cases in the acceptance criteria.
- No out-of-scope persistence, generation, database runtime behavior, or project scaffolding is introduced.

## Implementation Notes
- Foundation structure is now present: DVault.slnx is listed in project-files, src/DVault is listed in src-roots, and tests/DVault.Tests is listed in test-roots.
- Target production implementation to src/DVault and tests to tests/DVault.Tests.
- Use the current repository namespace conventions visible in existing tests, including DVault and DVault.Modeling, unless nearby source files establish a more specific convention for the new metadata types.
- Prefer simple immutable or validation-on-construction metadata types unless existing source conventions indicate another pattern.
- Keep the abstractions at the metadata/modeling layer and avoid coupling them to a database provider or storage format.
- Use .NET argument validation conventions that match the codebase, such as ArgumentException or ArgumentNullException for invalid required inputs.
- Use the MVP concept boundary from docs/architecture/mvp-data-vault-concepts.md: hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources are the current concept vocabulary, while hash algorithms and normalization remain future work.
- Respect docs/plans/deferred-data-vault-capabilities.md by keeping PIT tables, bridge tables, multi-active satellites, and provider-specific optimizations out of this ticket.
- A persisted blocks relation was attempted from foundation skeleton ticket 06EXB6XBV95E08R2W9ZQ1PRDPM to this ticket but failed with BOT-LOCAL-TOOL-TRUST-BLOCKED; do not block implementation solely on that relation now that foundation evidence exists.

## Open Questions
- none

## Follow-Up Questions
- Later tickets can decide whether to add specialized Data Vault constructs such as effectivity satellites, multi-active satellites, PIT tables, bridge tables, or business vault metadata.
- Later tickets can define serialization/configuration formats, hash computation rules, and stricter naming convention enforcement beyond basic missing-input validation.
- If ticket-relation trust policy later permits it, a relation can still be added for historical board clarity, but it is not required for this ticket to proceed now that foundation evidence is present.

## Risks
- The direct blocks relation remains absent because trust policy denied the relation write, so board-level dependency history may be less explicit than the refreshed contract.
- The ticket intentionally defines only a minimal v1 metadata surface, so future Data Vault variants may require additive model changes.

## Split Recommendations
- No split is needed for the metadata abstraction scope; proceed as one focused modeling task now that the foundation structure exists.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Create the core object model for Data Vault structures.

## Scope
- Define one documented public or protected member per file where applicable.

## Acceptance Criteria
- Hub, link, and satellite metadata can be created in tests.
- The model validates obvious missing inputs.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.