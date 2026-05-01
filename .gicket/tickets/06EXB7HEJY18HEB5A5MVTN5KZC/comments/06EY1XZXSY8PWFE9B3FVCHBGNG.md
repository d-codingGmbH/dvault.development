[gicket-bot] PO refinement contract

Summary
- Amended the ticket contract to preserve first-persisted hub and link lineage metadata on reuse and to define IDataVaultSaveService reuse results on the existing public API.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now states the same rule for both hubs and links: when a repeated save resolves to an existing row, the service reuses that row and preserves the row's first persisted LoadTimestamp and RecordSource instead of updating those lineage fields.
- critic-item-2: `answered` - The contract now defines caller-visible reuse on the existing result shape: DataVaultSaveResult.RowsWritten counts only rows newly inserted by the invocation, so reused rows contribute 0, and SavedRecords still contains one deterministic DataVaultSavedRecord per requested hub or link operation with the resolved persisted kind, metadata name, table name, and hash key whether the row was inserted or reused.
- critic-item-3: `answered` - The delivery contract now closes the lineage-metadata gap by forbidding mutation of previously persisted LoadTimestamp and RecordSource during duplicate reuse for both hubs and links; repeated saves may resolve to the existing row, but they must not overwrite its stored lineage metadata.
- critic-item-4: `answered` - The delivery contract now defines second-invocation behavior on the public result types: an all-reuse repeated save returns RowsWritten = 0, and when the same operations are supplied in the same order, SavedRecords returns the same deterministic per-operation summaries on the repeated invocation as on the first invocation.

Clarifications
- This ticket keeps the existing explicit IDataVaultSaveService boundary and the current public DataVaultSaveResult and DataVaultSavedRecord shape; reuse semantics are clarified on the existing API rather than through SaveChanges interception or new result members.
- For both hubs and links, a repeated save that resolves to an existing row must preserve that row's first persisted LoadTimestamp and RecordSource; the duplicate request must not overwrite existing lineage metadata.
- Duplicate detection for hubs and links remains driven by the existing deterministic hash-key inputs, so different request load timestamp or record source values do not create another hub or link row.
- Caller-visible reuse is expressed through the existing result contract: RowsWritten counts only new rows inserted during the invocation, and SavedRecords still returns deterministic per-operation summaries for every requested hub or link operation whether the row was inserted or reused.
- SavedRecords ordering remains deterministic by following the existing traversal shape: hub operations in request order, then link operations in request order.

Scope In
- Adjust hub persistence so repeated writes of the same business-key values reuse the existing hub row through the current explicit save service without mutating previously persisted LoadTimestamp or RecordSource.
- Adjust link persistence so repeated writes of the same participant hash-key combination reuse the existing link row through the current explicit save service without mutating previously persisted LoadTimestamp or RecordSource.
- Define caller-visible reuse semantics on the existing IDataVaultSaveService result contract, including RowsWritten behavior and deterministic SavedRecords content and order.
- Preserve deterministic hash-key computation, default naming, and current EF metadata conventions while implementing reuse behavior.
- Add or update automated tests on the current SQLite baseline to prove repeated hub and link writes keep row counts stable, preserve first-persisted lineage metadata, and return deterministic save results across separate invocations.
- Document the v1 concurrency assumptions and limitations for duplicate writes within the current provider-capability baseline.

Scope Out
- Satellite persistence or satellite idempotency behavior.
- New SaveChanges interceptors, alternate write APIs, or advanced configuration hooks.
- Provider-specific upsert, merge, or bulk-write features beyond the current SQLite-oriented baseline.
- Broader Data Vault capabilities already deferred from MVP, including PIT tables, bridge tables, and multi-active satellites.
- Adding new inserted-versus-reused flags or other new public result members to DataVaultSaveResult or DataVaultSavedRecord.

Open questions
- none

Follow-up questions
- If later providers need stronger simultaneous duplicate-write guarantees than the SQLite baseline, should DVault add an explicit provider-capability contract for retry or upsert semantics?
- When satellite save behavior is introduced, should a separate ticket define whether satellites need their own idempotent persistence contract distinct from hubs and links?

Risks
- If duplicate detection or resolved-row lookup does not align exactly with the existing stable hashing contract, repeated writes could still surface primary-key or unique-constraint failures instead of reusing rows.
- Because the current provider capability profile declares no concurrency support, documentation must avoid overstating the guarantee beyond the tested SQLite baseline and documented uniqueness-constraint assumptions.
- Tests that assert only row counts, or only repeat writes within a single DbContext, could miss regressions in preserved lineage metadata or RowsWritten and SavedRecords behavior across separate service invocations.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 8

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment