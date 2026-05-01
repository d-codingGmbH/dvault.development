[gicket-bot] PO refinement contract

Summary
- Restated the ticket against source-visible DVault save-service and SQLite metadata, removed unsupported inferred API language, and kept the work on the existing save-service/test surface with no split or planning artifact write.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Replaced inference with source-backed contract language: the branch visibly contains IDataVaultSaveService.SaveAsync(DbContext, DataVaultSaveRequest, CancellationToken), DataVaultSaveResult.RowsWritten, DataVaultSaveResult.SavedRecords, and DataVaultSavedRecord.Kind/MetadataName/TableName/HashKey. The contract now limits reuse reporting to those visible members and does not assume a hidden reuse-specific API.
- critic-item-2: `answered` - The contract no longer infers an unseen public type. It anchors the work to the existing public types already present in the branch: IDataVaultSaveService, DataVaultSaveRequest, DataVaultSaveResult, and DataVaultSavedRecord. The new work is the idempotent behavior inside the existing save path, not a claim that a reuse API already exists.
- critic-item-3: `answered` - The unsupported claim was restated as an implementation change on the existing service: DefaultDataVaultSaveService currently stages hub and link inserts and returns the SaveChangesAsync count; this ticket is to add lookup/reuse behavior while keeping the existing result members, not to rely on an already-implemented reuse contract or new public result members.

Clarifications
- This ticket stays on the existing public IDataVaultSaveService.SaveAsync(DbContext, DataVaultSaveRequest, CancellationToken) boundary and the currently visible DataVaultSaveResult/DataVaultSavedRecord members; no new public reuse flag, alternate write API, or SaveChanges interceptor is required by this contract.
- Current branch evidence shows DataVaultSaveResult exposes RowsWritten plus SavedRecords, and DataVaultSavedRecord exposes only Kind, MetadataName, TableName, and HashKey; caller-visible reuse semantics must therefore be expressed through those existing members.
- Current branch evidence also shows DefaultDataVaultSaveService still unconditionally stages hub and link rows before SaveChangesAsync, so idempotent reuse is new behavior to implement in this ticket rather than an already-delivered API guarantee.
- The translated SQLite baseline remains the source-backed schema baseline: hubs and links keep hash-key primary keys, hubs also keep a unique business-key index, and link participant columns currently have a non-unique relationship index.
- SavedRecords ordering is source-backed by the current SaveAsync traversal: hub operations are appended in request order before link operations, and reuse behavior must preserve that same order.
- No child ticket, relation write, attachment write, or planning document was materialized because the ticket remains bounded on the existing save-service surface.

Scope In
- Make repeated hub saves reuse the existing hub row through IDataVaultSaveService without mutating the first persisted LoadTimestamp or RecordSource.
- Make repeated link saves reuse the existing link row through IDataVaultSaveService without mutating the first persisted LoadTimestamp or RecordSource.
- Define reuse semantics only on the existing DataVaultSaveResult and DataVaultSavedRecord members already present in the branch.
- Keep the existing stable-hash services, naming policy, DI registration path, and ApplyDataVaultMetadata() SQLite model baseline.
- Add or update SQLite integration tests in the current DVault test layout to prove idempotent hub/link behavior across separate save invocations and contexts.
- Document the visible v1 concurrency baseline from DataVaultProviderCapabilityProfiles.Sqlite.

Scope Out
- Satellite persistence or satellite idempotency behavior.
- New public result members, inserted-versus-reused flags, or a second write API.
- SaveChanges interceptors or other alternate persistence entry points.
- Provider-specific upsert, merge, retry, or bulk-write abstractions beyond the current SQLite-focused baseline.
- Advanced configuration hooks or naming/hash/provider customization work already deferred elsewhere.
- Broader deferred Data Vault capabilities such as PIT tables, bridge tables, and multi-active satellites.

Open questions
- none

Follow-up questions
- If later providers need stronger simultaneous duplicate-write guarantees than DataVaultProviderCapabilityProfiles.Sqlite, should a separate capability ticket introduce retry or upsert semantics?
- When satellite save behavior is scheduled, should its idempotency contract be defined separately from hub/link reuse?

Risks
- If the reuse lookup does not exactly match the current stable-hash normalization and field ordering, repeated writes may still miss existing rows and hit primary-key failures.
- Because the current implementation reports raw SaveChangesAsync row count, the developer must separate insert counting from requested operation counting to keep RowsWritten correct for reused rows.
- Because current link metadata only shows a non-unique participant relationship index, assuming a participant-combination uniqueness constraint would overstate what the branch actually provides.
- Tests limited to one DbContext could miss regressions in persisted lineage preservation or duplicate detection across separate invocations.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment