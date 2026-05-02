[gicket-bot] PO-critic review contract

Summary
- Approved for dev: the persisted contract is bounded, open questions are resolved, upstream blockers are done, and the repository already contains the DVault primitives and example surface this ticket depends on.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB7SY3J6160R9Q35CFN6Q1W/description.md and .gicket/tickets/06EXB7SY3J6160R9Q35CFN6Q1W/comments/06EYJX6ENWTRKWYCWMPH83XDRR.md both record the PO handoff decision ready_for_po_critic, the bounded scope, and 'Open questions - none'.
- git -C /mnt/c/Projects/DVault log --oneline --decorate -n 5 ticket/06EXB7SY3J6160R9Q35CFN6Q1W-task-implement-dvault-link-and-satellite-version shows 91c8a5ce '[06EXB7SY3J6160R9Q35CFN6Q1W] handoff po->po-critic' followed by b6366ad6 '[06EXB7SY3J6160R9Q35CFN6Q1W] lease claim po-critic'.
- src/DCoding.Data.DVault/DataVaultSaveService.cs exposes IDataVaultSaveService, DataVaultSaveRequest, DataVaultLinkSaveOperation, and DataVaultSatelliteSaveOperation; AddSatelliteRowIfChangedAsync compares the latest persisted HashDiff for a parent before inserting.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs creates link entities and satellites; tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs asserts a link-parent satellite named SatCustomerOrderState with columns CustomerOrderHashKey | HashDiff | LoadTimestamp | RecordSource | StateCode.
- tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs already proves multi-version satellite history behavior by persisting three SatCustomerContact rows for one CustomerHashKey across changed and returned hash diffs.
- tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs already asserts SQLite schema/table/index shape for HubOrder, LinkCustomerOrder, and SatCustomerOrderState, and tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs is the existing order/product narrative surface the ticket plans to extend.
- find /mnt/c/Projects/DVault/examples -maxdepth 2 -print returned only /mnt/c/Projects/DVault/examples and /mnt/c/Projects/DVault/examples/.gitkeep, matching the contract's scope-out of a runnable examples project.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The contract leaves the exact relationship-context payload open, so the developer still has to choose a human-readable order/product attribute change that makes link history obvious.
- The contract assumes the developer will keep participant ordering and naming deterministic so the visible link and satellite table names stay aligned with the current naming-policy conventions.

AC / test suggestions
- Keep acceptance evidence explicit that the same link parent hash key produces at least two satellite rows with different HashDiff and LoadTimestamp values.
- Keep schema evidence explicit about both the link table and its satellite table rather than relying only on a broad snapshot diff.

Implementation watchouts
- DataVaultSaveService.cs only suppresses a satellite insert when the latest row for the same parent has the same HashDiff, so the example must drive a changed payload/hash diff for the same order-product relationship.
- examples/ is still empty except for .gitkeep, so work should remain in the existing integration-test and documentation surfaces.
- NormalEfOrderProductSqliteTests.cs already provides the compact order/product narrative; expanding into a more abstract or wider scenario would fight the documentation-friendly acceptance criterion.

Non-blocking notes
- No human comment thread introduced new unresolved scope questions after the PO refinement contract; the later comments are workflow handoff and lease records.

Split recommendations
- No split recommended; the work is bounded to one order/product scenario and the repository already contains the generic save-service, link/satellite translation, and schema-test primitives it depends on.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment