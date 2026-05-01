[gicket-bot] PO-critic review contract

Summary
- Repository and dependency evidence support the ticket scope, but the contract still leaves duplicate-reuse behavior under-specified for persisted lineage metadata and SaveAsync results, so it should return to PO for one more clarification pass.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- git -C /mnt/c/Projects/DVault rev-parse returned branch ticket/06EXB7HEJY18HEB5A5MVTN5KZC-task-persist-hubs-and-links-idempotently at HEAD 89bdc4bdea00072c9f5acb70af9829d146746f88, matching the scratch-source ref for this review.
- .gicket/tickets/06EXB7HEJY18HEB5A5MVTN5KZC/description.md:7-9 records PO handoff decision ready_for_po_critic, and :50-51 records Open Questions as none.
- src/DCoding.Data.DVault/DataVaultSaveService.cs:10-21 exposes the existing IDataVaultSaveService public boundary, and src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:21-23 registers IStableHashService, IStableHashNormalizer, and IDataVaultSaveService in AddDVault().
- src/DCoding.Data.DVault/DataVaultSaveService.cs:247-266,297,334 shows the current DefaultDataVaultSaveService always stages hub/link rows with Add() before SaveChangesAsync, so idempotent reuse is not implemented yet but the implementation surface is concrete.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:75-89 defines hub uniqueness as a hash-key primary key plus a unique business-key index, and :138-152 defines link uniqueness as a hash-key primary key with a non-unique participant relationship index.
- src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs:233-249 defines the sqlite-v1 provider profile with NoneInV1Unsupported SQL-function and concurrency support, matching the contract's constrained provider baseline.
- tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:11-72 already proves one-pass SQLite hub/link persistence through IDataVaultSaveService, and tests/DCoding.Data.DVault.Tests contains existing Unit, Integration, and Shared test projects for the repeated-write coverage this ticket asks for.
- Branch history for the save-service baseline shows commit aa10cb23 ([06EXB7H6KV753KM125XN3VDRTM] AUTO-INTEGRATION squash into develop), and .gicket/tickets/06EXB7H6KV753KM125XN3VDRTM/ticket.json marks that prerequisite ticket done; follow-up comments on this ticket (06EY1EQPMJDCA4HR9Q3N5WWB4M.md, 06EY1FWYHGAZ3DHRBSK7DEE0X0.md, 06EY1NP7YMCC7141BECEB741NW.md, 06EY1R9D2VYZAG4QZWZNJ3J81G.md) show completed PO, PO-critic, dev, and test flows for that dependency.

Blocking findings
- The contract never states what should happen to an existing hub or link row's LoadTimestamp and RecordSource when a duplicate write is reused. Current persistence stores those fields for both hubs and links in src/DCoding.Data.DVault/DataVaultSaveService.cs:287-291 and :324-328, and the broader model docs treat them as lineage metadata in docs/architecture/mvp-data-vault-concepts.md:21-28 and :32-39, but .gicket/tickets/06EXB7HEJY18HEB5A5MVTN5KZC/description.md:31-35 only constrains row reuse and row counts. A developer can satisfy the current ACs while still mutating previously persisted lineage metadata.
- The public reuse result is still under-specified. DataVaultSaveResult and DataVaultSavedRecord are public in src/DCoding.Data.DVault/DataVaultSaveService.cs:166-230, but the ticket only says repeated writes should return deterministic saved-record summaries (.gicket/tickets/06EXB7HEJY18HEB5A5MVTN5KZC/description.md:21 and :34) without defining the expected RowsWritten value on a reused row or exactly what deterministic SavedRecords behavior callers must observe on the second invocation.

Required PO actions
- Amend the delivery contract to state whether a repeated hub/link save preserves the first persisted LoadTimestamp and RecordSource or updates the existing row, and make the same rule explicit for both hubs and links.
- Amend the delivery contract to define the caller-visible IDataVaultSaveService result on reuse: expected RowsWritten behavior and what SavedRecords must contain on a repeated save.

Open issues ledger
- critic-item-1 [required-po-action] Amend the delivery contract to state whether a repeated hub/link save preserves the first persisted LoadTimestamp and RecordSource or updates the existing row, and make the same rule explicit for both hubs and links.
- critic-item-2 [required-po-action] Amend the delivery contract to define the caller-visible IDataVaultSaveService result on reuse: expected RowsWritten behavior and what SavedRecords must contain on a repeated save.
- critic-item-3 [blocking-finding] The contract never states what should happen to an existing hub or link row's LoadTimestamp and RecordSource when a duplicate write is reused. Current persistence stores those fields for both hubs and links in src/DCoding.Data.DVault/DataVaultSaveService.cs:287-291 and :324-328, and the broader model docs treat them as lineage metadata in docs/architecture/mvp-data-vault-concepts.md:21-28 and :32-39, but .gicket/tickets/06EXB7HEJY18HEB5A5MVTN5KZC/description.md:31-35 only constrains row reuse and row counts. A developer can satisfy the current ACs while still mutating previously persisted lineage metadata.
- critic-item-4 [blocking-finding] The public reuse result is still under-specified. DataVaultSaveResult and DataVaultSavedRecord are public in src/DCoding.Data.DVault/DataVaultSaveService.cs:166-230, but the ticket only says repeated writes should return deterministic saved-record summaries (.gicket/tickets/06EXB7HEJY18HEB5A5MVTN5KZC/description.md:21 and :34) without defining the expected RowsWritten value on a reused row or exactly what deterministic SavedRecords behavior callers must observe on the second invocation.

Missing examples / edge cases
- A repeated hub save in a fresh DbContext with the same business key but a different load timestamp and record source, asserting both row count and the intended lineage-metadata behavior.
- A repeated link save in a fresh DbContext with the same participant hash-key combination but different load metadata, again asserting both row count and the intended lineage-metadata behavior.
- An explicit note on whether duplicate hub/link operations inside a single DataVaultSaveRequest are in scope for this ticket or intentionally deferred.

Risky assumptions
- Assuming reuse means do-not-update semantics for existing hub/link lineage metadata, even though the ticket never states that rule explicitly.
- Assuming deterministic saved-record summaries mean the second call returns the same hash keys and a zero-new-rows result, even though the public SaveAsync result contract is not pinned in the ticket.
- Assuming same-request duplicate operations are outside scope because the contract emphasizes repeated save invocations and persisted scenarios (.gicket/tickets/06EXB7HEJY18HEB5A5MVTN5KZC/description.md:48 and :60).

AC / test suggestions
- Add one acceptance-criteria example that a second save with the same business key or participant hash-key inputs but different load metadata still yields a single persisted row and the explicitly chosen lineage-metadata behavior.
- Add one acceptance-criteria example that the repeated call returns the same SavedRecords hash keys and a defined RowsWritten value.
- Keep at least one repeated-write test that reopens a new DbContext between the first and second save, because the ticket already identifies single-tracker-only coverage as insufficient.

Implementation watchouts
- DefaultDataVaultSaveService currently uses Add() for every hub and link row before SaveChangesAsync (src/DCoding.Data.DVault/DataVaultSaveService.cs:256-265,297,334), so idempotency will need explicit lookup/reuse behavior rather than the current insert-only flow.
- Hub uniqueness and link uniqueness are not symmetric: hubs have a unique business-key index plus hash-key PK, while links only have the link hash-key PK and a non-unique participant relationship index (src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:75-89 and :138-152).
- The sqlite-v1 provider profile declares no SQL-function or concurrency support (src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs:233-249), so the documentation must keep simultaneous-writer guarantees narrow.

Non-blocking notes
- The stable-hashing prerequisite is grounded in source and completed prior work: docs/plans/stable-hashing-contract.md:15-22 defines the public hashing contract, and .gicket/tickets/06EXB765S2X2MR2K18ZBV8RC38/ticket.json marks the hash-service story done.

Split recommendations
- Keep stronger multi-writer guarantees and any same-request duplicate batching rules as follow-up scope unless the PO explicitly wants them pulled into this ticket.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment