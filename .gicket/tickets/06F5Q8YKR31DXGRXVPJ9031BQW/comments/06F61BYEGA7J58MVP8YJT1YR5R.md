[gicket-bot] PO refinement contract

Summary
- Refinement keeps this ticket scoped to an internal provider-staging SPI and transaction contract behind the existing `IDataVaultSaveService` boundary. No child-ticket, relation, description, attachment, or planning-document writes were materialized because the referenced architecture/release docs and the live relation graph already provide a bounded baseline for PO handoff.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current public baseline is already fixed by the referenced docs: `IDataVaultSaveService` remains the caller-visible write boundary, request ordering stays explicit, and callers continue to own `DbContext`, transaction, and cancellation semantics.
- This story is an internal architecture-contract story for staged provider bulk ingestion. It should define an internal SPI and behavior contract for downstream implementation tickets without introducing a new public save API or unstable provider-facing consumer surface.
- Relation facts were already clear from the bounded ticket reads: this ticket is a child of `06F5Q8YBVRS2EZVMJK5EATV9AR`, is currently blocked by `06F5Q8Y3WW9FFV7HA289VHCEAM`, and blocks five downstream tickets; no relation cleanup was needed in this refinement pass.
- Comment facts were already clear from the bounded ticket reads: there were no human comments to incorporate, only bot claim/lease comments.

Scope In
- Define the internal staging lifecycle for one staged bulk-ingestion attempt, including stage creation, population, execution, cleanup, failure, and cancellation exit paths.
- Define the normalized internal row handoff from the explicit save pipeline into provider implementations for hub, link, and satellite rows while preserving existing ordering and metadata rules.
- Define caller-owned transaction participation, dirty-context preconditions, concurrency stance, and diagnostics gates for unsupported providers and unsupported request shapes.
- Define how oversized batches and provider/schema limitations are classified and rejected or declined before provider-specific staging work proceeds.

Scope Out
- Adding a new public consumer-facing staged bulk API beyond the existing `IDataVaultSaveService` boundary.
- Implementing provider-specific temp-table/staging optimizations for individual provider packages.
- Changing unrelated explicit-save, bridge-maintenance, read-service, or EF design-time workflow contracts.
- Promising provider-native retry, merge, or cross-provider concurrency semantics beyond the bounded v1 internal contract.

Open questions
- none

Follow-up questions
- After this internal SPI lands, should release notes publish a concise provider-coverage matrix for staged bulk ingestion support versus fallback/unsupported behavior?
- Do we want a follow-up ticket to standardize telemetry/event names and counters for staging-object creation, cleanup, and oversized-batch rejection?
- If a specific provider later needs materially different cleanup or concurrency behavior, should that be isolated in provider-specific follow-up tickets rather than broadening this shared contract?

Risks
- Late churn in lifecycle or transaction rules will cascade into the five downstream tickets already blocked by this story.
- If oversized-batch and schema-limitation gates are under-specified, provider packages may diverge in when they reject equivalent request shapes.
- If the implementation leaks provider-specific staging abstractions into public namespaces, the project could accidentally take on a long-term public API support burden.

Split recommendations
- No additional split is recommended in this refinement pass; the live relation graph already shows this story serving as the contract/architecture blocker for five downstream tickets.
- If later evidence shows one provider needs materially different staging cleanup or transaction semantics, create a provider-specific follow-up ticket instead of widening this shared contract story.

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