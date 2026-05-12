[gicket-bot] PO-critic review contract

Summary
- Return to PO: the hierarchy-depth acceptance criterion conflicts with the implemented and tested bridge request shape, and the parent story workflow is stale because all child deliverables are already done and merged into develop.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEGPPETJD4ZDEN5ESGR7JW/description.md:62-63 shows ## Open Questions = none.
- .gicket/tickets/06F0MEGPPETJD4ZDEN5ESGR7JW/description.md:40 says hierarchy bridge requests have optional maximum-depth filtering.
- src/DCoding.Data.DVault/DataVaultBridgeReadRequest.cs:123-140 requires maximumDepth for hierarchy bridges, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultBridgeReadServiceTests.cs:24-42 asserts missing depth is invalid.
- git diff --name-only develop...HEAD lists only .gicket/tickets/06F0MEGPPETJD4ZDEN5ESGR7JW/* metadata files; no PIT/bridge source, test, or docs files differ from develop.
- git log --oneline develop -- relevant PIT/bridge source and test files returns 0db0450fa [06F0MEH660Y5QTNR5P8JPS2QXC] AUTO-INTEGRATION squash into develop and 95cbdef44 [06F0MEHKYTBJEJH2DVZ2CFH9Z0] AUTO-INTEGRATION squash into develop, so the feature code is already on develop.
- Public surface and coverage are present in src/DCoding.Data.DVault/IDataVaultReadService.cs:21-31, src/DCoding.Data.DVault/DataVaultReadServiceBridgeExtensions.cs:17-64, tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitReadServiceSqliteTests.cs:18-163, and tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs:13-165.

Blocking findings
- The delivery contract is internally inconsistent for hierarchy bridge depth. Acceptance criterion .gicket/tickets/06F0MEGPPETJD4ZDEN5ESGR7JW/description.md:40 makes maximum depth sound optional, but the actual public request type requires a bounded maximumDepth for hierarchy bridges (src/DCoding.Data.DVault/DataVaultBridgeReadRequest.cs:123-140) and the unit tests enforce that behavior (tests/DCoding.Data.DVault.Tests/Unit/DataVaultBridgeReadServiceTests.cs:24-42).

Required PO actions
- Rewrite the hierarchy bridge acceptance criterion so it explicitly matches intended behavior: either require non-negative bounded maximumDepth for hierarchy requests or change the code/tests in a separate implementation ticket if optional depth was the true requirement.
- Reconcile the parent story workflow with observed delivery state. If this is an umbrella or completion story, update status and routing expectations instead of sending it to dev for new implementation work.
- Clarify release-note or changelog ownership if this story still owns scope-consistency work; docs/releases/v0.6.0.md:46-47 still describes PIT-backed reads and bridge helpers as not delivered.

Open issues ledger
- critic-item-1 [required-po-action] Rewrite the hierarchy bridge acceptance criterion so it explicitly matches intended behavior: either require non-negative bounded maximumDepth for hierarchy requests or change the code/tests in a separate implementation ticket if optional depth was the true requirement.
- critic-item-2 [required-po-action] Reconcile the parent story workflow with observed delivery state. If this is an umbrella or completion story, update status and routing expectations instead of sending it to dev for new implementation work.
- critic-item-3 [required-po-action] Clarify release-note or changelog ownership if this story still owns scope-consistency work; docs/releases/v0.6.0.md:46-47 still describes PIT-backed reads and bridge helpers as not delivered.
- critic-item-4 [blocking-finding] The delivery contract is internally inconsistent for hierarchy bridge depth. Acceptance criterion .gicket/tickets/06F0MEGPPETJD4ZDEN5ESGR7JW/description.md:40 makes maximum depth sound optional, but the actual public request type requires a bounded maximumDepth for hierarchy bridges (src/DCoding.Data.DVault/DataVaultBridgeReadRequest.cs:123-140) and the unit tests enforce that behavior (tests/DCoding.Data.DVault.Tests/Unit/DataVaultBridgeReadServiceTests.cs:24-42).

Missing examples / edge cases
- A concrete accepted example of a hierarchy bridge request without maximumDepth, or explicit rejection text if unbounded hierarchy reads are out of scope.

Risky assumptions
- Assuming docs/releases/v0.6.0.md is historical release context only; if it is still a live acceptance reference for this story, it conflicts with the current source and test state.
- Assuming there is no remaining developer-owned work outside the four done child tickets; the repository and branch history do not show any new code delta on this parent story branch.

AC / test suggestions
- Change AC wording from optional maximum-depth filtering to explicit bounded-depth behavior for hierarchy bridges, because that is what the public request type and tests currently prove.
- Keep a regression test that distinguishes allowed many-to-many no-depth requests from required hierarchy depth requests.
- If the parent story remains active, add a ticket-level acceptance criterion for the expected workflow or state once all child tickets are done.

Implementation watchouts
- Bridge reads are exposed through DataVaultReadServiceBridgeExtensions.cs, not as a new IDataVaultReadService interface member; docs should not imply the PIT and bridge surfaces land on the exact same API boundary.
- Hierarchy traversal remains source-backed and depends on precomputed bridge rows plus TraversalDepth; it is not an unbounded recursive query engine.

Non-blocking notes
- The persisted contract has no unresolved open questions (.gicket/tickets/06F0MEGPPETJD4ZDEN5ESGR7JW/description.md:62-63).
- Public API snapshot evidence exists in tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt, and SQLite integration coverage exists for both PIT and bridge read helpers.

Split recommendations
- No additional split recommended; the story is already decomposed into four child tickets, and status or routing cleanup is the remaining need.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment