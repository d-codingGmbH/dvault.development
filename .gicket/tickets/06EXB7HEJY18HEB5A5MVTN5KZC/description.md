<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Restated the ticket against source-visible DVault save-service and SQLite metadata, removed unsupported inferred API language, and kept the work on the existing save-service/test surface with no split or planning artifact write.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket stays on the existing public IDataVaultSaveService.SaveAsync(DbContext, DataVaultSaveRequest, CancellationToken) boundary and the currently visible DataVaultSaveResult/DataVaultSavedRecord members; no new public reuse flag, alternate write API, or SaveChanges interceptor is required by this contract.
- Current branch evidence shows DataVaultSaveResult exposes RowsWritten plus SavedRecords, and DataVaultSavedRecord exposes only Kind, MetadataName, TableName, and HashKey; caller-visible reuse semantics must therefore be expressed through those existing members.
- Current branch evidence also shows DefaultDataVaultSaveService still unconditionally stages hub and link rows before SaveChangesAsync, so idempotent reuse is new behavior to implement in this ticket rather than an already-delivered API guarantee.
- The translated SQLite baseline remains the source-backed schema baseline: hubs and links keep hash-key primary keys, hubs also keep a unique business-key index, and link participant columns currently have a non-unique relationship index.
- SavedRecords ordering is source-backed by the current SaveAsync traversal: hub operations are appended in request order before link operations, and reuse behavior must preserve that same order.
- No child ticket, relation write, attachment write, or planning document was materialized because the ticket remains bounded on the existing save-service surface.

### Scope In
- Make repeated hub saves reuse the existing hub row through IDataVaultSaveService without mutating the first persisted LoadTimestamp or RecordSource.
- Make repeated link saves reuse the existing link row through IDataVaultSaveService without mutating the first persisted LoadTimestamp or RecordSource.
- Define reuse semantics only on the existing DataVaultSaveResult and DataVaultSavedRecord members already present in the branch.
- Keep the existing stable-hash services, naming policy, DI registration path, and ApplyDataVaultMetadata() SQLite model baseline.
- Add or update SQLite integration tests in the current DVault test layout to prove idempotent hub/link behavior across separate save invocations and contexts.
- Document the visible v1 concurrency baseline from DataVaultProviderCapabilityProfiles.Sqlite.

### Scope Out
- Satellite persistence or satellite idempotency behavior.
- New public result members, inserted-versus-reused flags, or a second write API.
- SaveChanges interceptors or other alternate persistence entry points.
- Provider-specific upsert, merge, retry, or bulk-write abstractions beyond the current SQLite-focused baseline.
- Advanced configuration hooks or naming/hash/provider customization work already deferred elsewhere.
- Broader deferred Data Vault capabilities such as PIT tables, bridge tables, and multi-active satellites.

## Acceptance Criteria
- A second save of the same hub business-key values through IDataVaultSaveService.SaveAsync(...) reuses the existing hub row, keeps hub row count stable, and preserves the first persisted LoadTimestamp and RecordSource.
- A second save of the same link participant hash-key values through IDataVaultSaveService.SaveAsync(...) reuses the existing link row, keeps link row count stable, and preserves the first persisted LoadTimestamp and RecordSource.
- Hub and link duplicate detection continues to derive from the existing deterministic hash flow already used by DefaultDataVaultSaveService; different request lineage values alone do not create a new hub or link row.
- For a repeated request whose rows are fully reused, DataVaultSaveResult.RowsWritten is 0 and SavedRecords still returns one deterministic DataVaultSavedRecord per requested operation, in the same hub-then-link request order, with the same Kind, MetadataName, TableName, and HashKey values as the first save.
- The implementation keeps the current explicit save-service API surface and does not require new public result members or a second write entry point.
- Automated SQLite tests prove the behavior across separate save invocations and separate DbContext lifetimes, not only within one change tracker.
- Concurrency documentation states that the current provider baseline exposes no concurrency-signal support and that broader provider-neutral multi-writer guarantees are out of scope.

## Definition of Done
- The acceptance criteria are covered by automated tests in the existing tests/DCoding.Data.DVault.Tests layout.
- The implementation stays inside DefaultDataVaultSaveService and the existing AddDVault() registration path instead of adding a new public persistence surface.
- Tests prove a later repeated save with different request LoadTimestamp and RecordSource values does not overwrite persisted lineage metadata.
- Tests prove fully reused second saves return RowsWritten = 0 and preserve deterministic SavedRecords content and order.
- Any added documentation matches the visible SQLite provider baseline in DataVaultProviderCapabilityProfiles.Sqlite and does not claim unsupported concurrency signals or provider-specific upsert semantics.

## Implementation Notes
- Refine src/DCoding.Data.DVault/DataVaultSaveService.cs; the current implementation at DefaultDataVaultSaveService.SaveAsync/AddHub/AddLink only computes hashes, stages new shared-type rows, and returns SaveChangesAsync row count.
- Keep the existing public surface visible in source: IDataVaultSaveService, DataVaultSaveRequest, DataVaultSaveResult, and DataVaultSavedRecord.
- Use the existing IStableHashNormalizer and IStableHashService flow for reuse keys: hub hashes from business-key fields, link hashes from participant hub hash keys.
- Perform reuse lookup against the translated table and technical column names that the current naming policy produces.
- Do not infer an existing unique participant-combination constraint for links; current model metadata shows a non-unique relationship index on participant columns plus a primary key on the computed link hash key.
- Extend tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs or equivalent current integration coverage to assert repeated save behavior across separate service/context invocations.
- Document the current concurrency limitation instead of abstracting retries, merges, or provider-neutral upsert APIs.

## Open Questions
- none

## Follow-Up Questions
- If later providers need stronger simultaneous duplicate-write guarantees than DataVaultProviderCapabilityProfiles.Sqlite, should a separate capability ticket introduce retry or upsert semantics?
- When satellite save behavior is scheduled, should its idempotency contract be defined separately from hub/link reuse?

## Risks
- If the reuse lookup does not exactly match the current stable-hash normalization and field ordering, repeated writes may still miss existing rows and hit primary-key failures.
- Because the current implementation reports raw SaveChangesAsync row count, the developer must separate insert counting from requested operation counting to keep RowsWritten correct for reused rows.
- Because current link metadata only shows a non-unique participant relationship index, assuming a participant-combination uniqueness constraint would overstate what the branch actually provides.
- Tests limited to one DbContext could miss regressions in persisted lineage preservation or duplicate detection across separate invocations.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Ensure repeated writes reuse existing hub and link rows.

## Scope
- Use hash keys and uniqueness constraints to avoid duplicates.

## Acceptance Criteria
- Repeated write tests keep row counts stable.
- Concurrency assumptions are documented.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.