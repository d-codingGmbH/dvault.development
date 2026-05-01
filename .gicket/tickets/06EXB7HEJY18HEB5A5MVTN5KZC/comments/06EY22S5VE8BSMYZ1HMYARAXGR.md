[gicket-bot] PO-critic review contract

Summary
- Delivery contract is now source-backed, scoped to the existing save-service API, and specific enough for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- git -C /mnt/c/Projects/DVault rev-parse HEAD returned 4e0d303c0af43b3546fdf13bb4e7a79643c72085, matching the claimed scratch-source-ref.
- .gicket/tickets/06EXB7HEJY18HEB5A5MVTN5KZC/description.md:35-42 defines hub/link reuse semantics, RowsWritten = 0 for fully reused repeats, separate-DbContext test coverage, and the no-concurrency-signal documentation requirement; :60-61 records Open Questions as none.
- src/DCoding.Data.DVault/DataVaultSaveService.cs:18-21 exposes IDataVaultSaveService.SaveAsync(DbContext, DataVaultSaveRequest, CancellationToken); :166-187 exposes DataVaultSaveResult.RowsWritten and SavedRecords; :193-230 exposes DataVaultSavedRecord.Kind, MetadataName, TableName, and HashKey.
- src/DCoding.Data.DVault/DataVaultSaveService.cs:247-266 and :269-337 show the current implementation still stages hub/link rows and returns SaveChangesAsync row count, matching the contract statement that idempotent reuse is new behavior to implement.
- tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:11-72 currently covers first-save insert behavior only, so the ticket is correctly scoped to add repeated-save coverage rather than relying on existing proof.
- tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs:27-53 proves the current SQLite baseline: hub business-key indexes are unique while the link participant relationship index is non-unique.
- src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs:58-62, 214-217, and 233-249 define the SQLite profile with DataVaultProviderConcurrencySupport.NoneInV1Unsupported and deterministic failure for requested concurrency signals; tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs:7-20 asserts the same baseline.
- .gicket/tickets/06EXB7HEJY18HEB5A5MVTN5KZC/comments/06EY21MX7F8Z9JKNHX4DK2BFAR.md:10-13 records that the prior PO-critic blockers about inferred public API/type claims were answered in the persisted refinement contract.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not explicitly pin mixed-request behavior where some operations reuse existing rows and others insert new rows in the same SaveAsync call; it hardens the fully reused repeat case only.

Risky assumptions
- Reuse lookup still depends on the implementation matching the existing stable-hash normalization and field ordering exactly for both hub business keys and link participant keys; the ticket already captures this risk in description.md:67-71.

AC / test suggestions
- Add one SQLite integration test that mixes reused and newly inserted operations in the same request so RowsWritten semantics are pinned beyond the all-reused case.
- Add a repeat-save test that changes both LoadTimestamp and RecordSource on the second invocation and verifies the originally persisted lineage values remain unchanged for both hub and link rows across a new DbContext.

Implementation watchouts
- Duplicate detection for links must be anchored to the existing deterministic link hash flow, not to the non-unique participant relationship index.
- DataVaultSaveResult.RowsWritten must reflect newly inserted rows only when reuse occurs, not requested-operation count.
- Reuse must preserve SavedRecords ordering exactly as the current traversal does: hub operations in request order, then link operations.

Non-blocking notes
- none

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment