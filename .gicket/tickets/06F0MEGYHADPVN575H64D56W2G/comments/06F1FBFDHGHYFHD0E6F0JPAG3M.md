[gicket-bot] PO refinement contract

Summary
- Refined the ticket to a bounded PIT-backed as-of read contract that extends the existing `IDataVaultReadService` projector pattern and the documented `DataVaultPitMetadata` baseline; no new split, attachment, planning document, or relation change was needed for this refinement pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 public read boundary should stay on `IDataVaultReadService`; the PIT-backed contract should add a PIT request/raw-record pair that follows the existing latest/as-of projector pattern instead of creating a separate reflection-based read stack.
- The contract should be anchored to `DataVaultPitMetadata` for one hub plus its ordered non-multi-active hub-attached satellites; it should not reopen link-based PITs, bridge traversal reads, or PIT over multi-active satellites.
- The older `DataVaultPointInTimeMetadata` and `DataVaultModelBuilder.PointInTime(...)` surface remains historical and out of scope for this ticket; v1 naming and examples should use the newer `Pit` vocabulary.
- Timestamp handling stays logical and provider-neutral: callers supply an `asOf` instant as `DateTimeOffset`, and provider timestamp storage modes remain an implementation detail behind the existing capability-profile pipeline.
- Missing PIT rows for requested parent hash keys should yield no projected record for those parents, and an existing PIT row with an absent satellite snapshot should surface that satellite segment as absent rather than silently falling back to non-PIT latest/as-of reads.

Scope In
- Document the PIT-backed as-of request/response contract for reading one declared PIT by parent hash key set and `asOf` instant.
- Define the raw PIT read-record shape needed for typed projector delegates, including PIT load timestamp and per-satellite snapshot access in declared order.
- Specify behavior for multiple satellites, missing PIT rows, and missing per-satellite snapshot values inside an otherwise matched PIT row.
- Specify deterministic diagnostics for unsupported multi-active satellite, bridge, or other out-of-baseline PIT declarations.
- Provide worked examples and fixture expectations that align with current DVault naming and read-service conventions.

Scope Out
- PIT row population, refresh scheduling, late-arriving reconciliation, and any other PIT maintenance behavior.
- Provider-specific SQL, indexing, query optimization, or storage-tuning work.
- Bridge traversal helpers, link-based PIT parents, and PIT over multi-active satellites.
- Renaming, reconciling, or deprecating the older `PointInTime` modeling surface.
- Reflection-based DTO binding or a second public read service just for PIT.

Open questions
- none

Follow-up questions
- After the contract is implemented, should a higher-level convenience helper be added for common named read-model cases, or is the projector-only baseline sufficient for v1?
- Once PIT-backed reads exist, does the roadmap want a separate follow-up to unify or retire the older `PointInTime` modeling vocabulary?
- Do downstream docs want runnable quickstart coverage for PIT-backed reads in addition to the contract examples, or can that wait until PIT row maintenance exists?

Risks
- The main scope-creep risk is pulling legacy `PointInTime` naming, PIT maintenance, or provider-specific optimization into this ticket; any of those would turn a bounded contract task into multi-ticket design work.
- If the raw PIT read-record shape does not make missing satellite snapshot state explicit, downstream typed projectors may implement inconsistent null-or-absence behavior across satellites.
- The live upstream `blocks` relation means a later change to PIT metadata rules could still force this contract to be revised, even though the current repository documents are strong enough for PO refinement now.

Split recommendations
- No new split is recommended from current evidence; keep this ticket as the bounded public-contract-and-examples decision and let the already-related downstream work consume the finalized contract.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment