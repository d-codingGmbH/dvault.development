[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7HEJY18HEB5A5MVTN5KZC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7HEJY18HEB5A5MVTN5KZC`.
- Optimistic claim succeeded (`expectedRevision=06EY1VF3AC0BM6BY11CP8A3E5C`, `currentRevision=06EY1VJN0ADGKSKVSYJ7447JT8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7HEJY18HEB5A5MVTN5KZC-task-persist-hubs-and-links-idempotently' from source '89bdc4bdea00072c9f5acb70af9829d146746f88'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7HEJY18HEB5A5MVTN5KZC-task-persist-hubs-and-links-idempotently` as `3b6246787762`.

Open questions / Risiken
- Blocking finding: The contract never states what should happen to an existing hub or link row's LoadTimestamp and RecordSource when a duplicate write is reused. Current persistence stores those fields for both hubs and links in src/DCoding.Data.DVault/DataVaultSaveService.cs:2...
- Blocking finding: The public reuse result is still under-specified. DataVaultSaveResult and DataVaultSavedRecord are public in src/DCoding.Data.DVault/DataVaultSaveService.cs:166-230, but the ticket only says repeated writes should return deterministic saved-record summaries (...
- Required PO action: Amend the delivery contract to state whether a repeated hub/link save preserves the first persisted LoadTimestamp and RecordSource or updates the existing row, and make the same rule explicit for both hubs and links.
- Required PO action: Amend the delivery contract to define the caller-visible IDataVaultSaveService result on reuse: expected RowsWritten behavior and what SavedRecords must contain on a repeated save.
- Risky assumption: Assuming reuse means do-not-update semantics for existing hub/link lineage metadata, even though the ticket never states that rule explicitly.
- Risky assumption: Assuming deterministic saved-record summaries mean the second call returns the same hash keys and a zero-new-rows result, even though the public SaveAsync result contract is not pinned in the ticket.
- Risky assumption: Assuming same-request duplicate operations are outside scope because the contract emphasizes repeated save invocations and persisted scenarios (.gicket/tickets/06EXB7HEJY18HEB5A5MVTN5KZC/description.md:48 and :60).
- Split recommendation: Keep stronger multi-writer guarantees and any same-request duplicate batching rules as follow-up scope unless the PO explicitly wants them pulled into this ticket.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8518`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `466d46bacd394ca49eb3acea28979a19`
- completed-at-utc: `<redacted>-01T00:32:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7HEJY18HEB5A5MVTN5KZC/runs/20260501T003255356Z-466d46bacd394ca49eb3acea28979a19.json`