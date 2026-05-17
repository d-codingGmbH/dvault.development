[gicket-bot] PO-critic review contract

Summary
- Refined roll-up story is bounded and internally consistent: open questions are cleared, the three-child split and relation graph match the persisted contract, and repository/docs/tests evidence aligns. Approve for dev, with the watchout that the current branch is a ticket-metadata handoff branch rather than a fresh implementation branch.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGJGDGMXHPT1VP0ASQ5HJ4/description.md:7-15 records PO handoff `ready_for_po_critic`, names the three child tickets, ties the story to generator/attribute/test/doc evidence, and description.md:51-52 records `## Open Questions` as `- none`.
- .gicket/relations/J4/KM/06F2PGJGDGMXHPT1VP0ASQ5HJ4--06F2PGJN1XCV8F7NWH567SQSKM--parentOf.json:3-5, .gicket/relations/J4/30/06F2PGJGDGMXHPT1VP0ASQ5HJ4--06F2PGJSXP18VKKV52QZA4NP30--parentOf.json:3-5, and .gicket/relations/J4/5C/06F2PGJGDGMXHPT1VP0ASQ5HJ4--06F2PGJYY6S97B4Z8044D34K5C--parentOf.json:3-6 show the current child graph; a relation search for the story/doc pair returned only the `parentOf` file, matching the contract claim that the former `blocks` edge is gone.
- src/DCoding.Data.DVault.Analyzers/DataVaultMappingSourceGenerator.cs:83-101 enforces exactly one mapping shape, :143-154 rejects repeated participant hub names with DMV1955, and :447-560 emits `IDataVaultHubMapper<TSource>`, `IDataVaultLinkMapper<TSource>`, and `IDataVaultSatelliteMapper<TSource>` implementations that return `DataVaultRegistry*SaveOperation` values.
- src/DCoding.Data.DVault/DataVaultHubMappingAttribute.cs, DataVaultLinkMappingAttribute.cs, DataVaultHubSatelliteMappingAttribute.cs, and the related binding attribute files in src/DCoding.Data.DVault expose the compile-time declaration surface the contract names; tests/DCoding.Data.DVault.Tests/Unit/DataVaultTypedMapperContractTests.cs:104-132 directly verifies exact names and orders on those attributes.
- tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultMappingSourceGeneratorTests.cs:10-79 covers deterministic hub/link/hub-parent-satellite generation and :82-170 covers DMV1950-DMV1955 failure cases; src/DCoding.Data.DVault.Analyzers/DataVaultMappingDiagnosticCatalog.cs:8-60 defines DMV1950 through DMV1955.
- src/DCoding.Data.DVault/IDataVaultHubMapper.cs:8-12, IDataVaultLinkMapper.cs:32-37, IDataVaultSatelliteMapper.cs:57-62, and DataVaultSaveServiceTypedExtensions.cs:86-178 keep `loadTimestamp` and `recordSource` outside mappers and pass them through explicit save-service APIs; tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedMapperSaveServiceSqliteTests.cs:67-164 exercises generated helpers through explicit save calls on SQLite.
- docs/releases/v0.12.0.md:22-27, 42-61, and 93-99; README.md:21-25 and 483-486; docs/production-adoption-checklist.md:7-10 and 38-40; and src/DCoding.Data.DVault.Analyzers/README.md:31-39 all align on the generated-mapper scope, DMV1950-DMV1955 diagnostics, analyzer-package placement, and preserved explicit save boundary.
- Branch-history evidence: `git rev-parse --short=12 HEAD` returned `63b3819c82e6`, matching the provided scratch source ref; `git diff --stat 63b3819c82e6ac435b005b4d6c298be8cfc9d271..HEAD` returned no file changes; `git diff --name-only develop..HEAD` listed only .gicket ticket metadata/comment/event files for 06F2PGJGDGMXHPT1VP0ASQ5HJ4; and `git log --oneline develop..HEAD` showed only handoff/lease commits `25df2a69a`, `64d3413fa`, `c5b895c49`, and `63b3819c8`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A runnable generator-based consumer example is still intentionally out of scope; the contract tracks that as a follow-up question instead of current-story acceptance scope.
- Generated support for link-parent satellites and repeated-participant or self-link mappings is intentionally excluded from the v1 slice and remains a follow-on edge-case bucket if product wants it later.

Risky assumptions
- Downstream roles need to treat this as a roll-up/verification story: relative to `develop`, the branch currently carries ticket metadata only, so fresh implementation work should not be inferred from this handoff branch alone.
- The broader typed-mapper contract surface includes link-parent satellite contracts in tests/DCoding.Data.DVault.Tests/Unit/DataVaultTypedMapperContractTests.cs:43-79, but the generated-helper acceptance scope is narrower and the generated SQLite integration test stays on hub/link/hub-parent satellite shapes.

AC / test suggestions
- If a follow-on ticket is created for link-parent satellite generation, give it its own AC and negative tests so it is not conflated with the existing shared `IDataVaultSatelliteMapper<TSource>` contract.
- If a runnable sample ticket is created, require explicit evidence that callers still own `loadTimestamp`, `recordSource`, `DbContext`, and `IDataVaultSaveService` orchestration.
- If repeated-participant or self-link support is revisited, require explicit participant-alias semantics plus negative tests for ambiguous same-hub links.

Implementation watchouts
- Keep the developer handoff on ticket state, not new scope: the current branch is metadata-only over `develop`, so downstream work should be validation/closure of the already materialized roll-up rather than re-implementation.
- Do not widen this story to link-parent satellites, repeated-participant links, or self-links; the contract already marks those as follow-on scope.

Non-blocking notes
- Incoming `blocks` relations remain on disk, but both upstream tickets are already `done`, so they are historical satisfied dependencies rather than current PO blockers.

Split recommendations
- No additional split is needed; the existing contract/implementation/documentation child split is already materialized and all three child tickets are done.
- If the team wants generated link-parent satellite support or repeated-participant/self-link support, create separate follow-on tickets rather than widening this story.
- If the team wants a runnable consumer sample for generated mappings, create a separate docs/examples ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment