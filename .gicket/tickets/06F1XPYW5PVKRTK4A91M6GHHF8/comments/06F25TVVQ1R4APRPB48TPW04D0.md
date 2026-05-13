[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F1XPYW5PVKRTK4A91M6GHHF8/description.md: Delivery Contract states PO handoff decision ready_for_po_critic and Open Questions is none.
- Ticket comments include 06F25SFDMP5AT0M0BB1F2DPBS8.md with the PO refinement contract and 06F25SH7Y4Z0C38T2AZT16BMSW.md reporting outcome po-refinement-ready.
- git diff --name-status develop...HEAD shows changes only under .gicket/tickets/06F1XPYW5PVKRTK4A91M6GHHF8; no src or tests implementation changes are present on the review branch.
- tests/DCoding.Data.DVault.Tests/Integration and tests/DCoding.Data.DVault.Tests/Unit contain existing DVault test projects; their csproj files target net10.0 and reference Microsoft.EntityFrameworkCore.Sqlite plus the DVault source projects.
- src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs exposes public ApplyDataVaultMetadata overloads; src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs exposes public UseDataVaultMetadata overloads.
- src/DCoding.Data.DVault/DataVaultAnnotationNames.cs defines DVault annotation names including ProducedName, EntityKind, MetadataName, ParentReferenceKind, ParentReferenceName, Ordinal, and PropertyRole.
- src/DCoding.Data.DVault/IDataVaultReadService.cs exposes public ReadLatestSatelliteRowsAsync and ReadPitRowsAsync; src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs exposes public ReadLatestSatelliteAsync typed projection helper.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs already verifies registry metadata projection through UseDataVaultMetadata with SQLite.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedSatelliteReadServiceSqliteTests.cs already seeds deterministic rows and asserts exact latest/as-of projection values through IDataVaultReadService.
- tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs shows existing direct EF reads from generated DVault shared-type entity sets such as HubOrder, LinkOrderProduct, and SatOrderProductFulfillment.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The developer must choose a read path that is actually expressible through EF Core compiled query APIs; the contract permits this by requiring an already-supported deterministic read/query surface and forbidding new query APIs.
- The compiled model proof should use a checked-in deterministic test fixture path, not EF CLI generated artifacts, because the contract explicitly scopes out design-time compiled-model generation.

AC / test suggestions
- Name or structure the compiled model test so failures identify metadata availability versus annotation value mismatch.
- Name or structure the compiled query test so failures identify query execution versus returned data shape mismatch.
- Use a small deterministic hub/link/satellite fixture already represented in the current tests and assert concrete returned row/projection values, not only absence of exceptions.

Implementation watchouts
- Keep the test in the existing Unit or Integration test project conventions instead of creating a new harness.
- Avoid adding Microsoft.EntityFrameworkCore.Design or EF CLI dependencies to src/DCoding.Data.DVault.
- If SQLite is selected, keep it as the existing local relational baseline and avoid provider-matrix claims.
- Do not expand this task into benchmark work, provider-specific implementation, or broad query-shape coverage.

Non-blocking notes
- Parent story 06F1XPYA9MD0T9C4651ND8KX0W is still todo/needs-po, but it is a parentOf relation rather than an unresolved blocks relation against this refined task.
- No build or test command was run because this PO-critic review was limited to read-only ticket and repository inspection.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment