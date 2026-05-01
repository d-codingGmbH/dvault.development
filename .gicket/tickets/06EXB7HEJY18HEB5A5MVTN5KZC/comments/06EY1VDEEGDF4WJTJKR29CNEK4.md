[gicket-bot] PO refinement contract

Summary
- Refined the ticket around the existing explicit save service, current SQLite EF baseline, and deterministic hash-key reuse for idempotent hub and link writes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This ticket applies to the existing explicit IDataVaultSaveService write path and does not introduce SaveChanges interception or a new public write API.
- The visible v1 baseline is the current SQLite EF Core provider profile with deterministic hash generation through IStableHashNormalizer and IStableHashService.
- Current model metadata already provides the relevant uniqueness baseline: hubs use a hash-key primary key plus a unique business-key index, and links use a hash-key primary key.
- Idempotent behavior for this ticket means a repeated hub or link write reuses the existing row instead of inserting a duplicate; duplicate detection is based on the existing hash-key shape, not on load timestamp or record source values.

Scope In
- Adjust hub persistence so repeated writes of the same business-key values reuse the existing row through the current explicit save service.
- Adjust link persistence so repeated writes of the same participant hash-key combination reuse the existing row through the current explicit save service.
- Preserve deterministic hash-key computation, default naming, and current EF metadata conventions while implementing reuse behavior.
- Add or update automated tests on the current SQLite baseline to prove repeated hub and link writes keep row counts stable and return deterministic saved-record summaries.
- Document the v1 concurrency assumptions and limitations for duplicate writes within the current provider-capability baseline.

Scope Out
- Satellite persistence or satellite idempotency behavior.
- New SaveChanges interceptors, alternate write APIs, or advanced configuration hooks.
- Provider-specific upsert, merge, or bulk-write features beyond the current SQLite-oriented baseline.
- Broader Data Vault capabilities already deferred from MVP, including PIT tables, bridge tables, and multi-active satellites.

Open questions
- none

Follow-up questions
- If later providers need stronger simultaneous duplicate-write guarantees than the SQLite baseline, should DVault add an explicit provider-capability contract for retry or upsert semantics?
- When satellite save behavior is introduced, should a separate ticket define whether satellites need their own idempotent persistence contract distinct from hubs and links?

Risks
- If duplicate detection does not align exactly with the existing stable hashing contract, repeated writes could still surface primary-key or unique-constraint failures instead of reusing rows.
- Because the current provider capability profile declares no concurrency support, documentation must avoid overstating the guarantee beyond the tested SQLite baseline and documented uniqueness-constraint assumptions.
- Tests that only repeat writes within a single DbContext could miss real persisted duplicate behavior across service invocations.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment