<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement keeps this ticket scoped to an internal provider-staging SPI and transaction contract behind the existing `IDataVaultSaveService` boundary. No child-ticket, relation, description, attachment, or planning-document writes were materialized because the referenced architecture/release docs and the live relation graph already provide a bounded baseline for PO handoff.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The current public baseline is already fixed by the referenced docs: `IDataVaultSaveService` remains the caller-visible write boundary, request ordering stays explicit, and callers continue to own `DbContext`, transaction, and cancellation semantics.
- This story is an internal architecture-contract story for staged provider bulk ingestion. It should define an internal SPI and behavior contract for downstream implementation tickets without introducing a new public save API or unstable provider-facing consumer surface.
- Relation facts were already clear from the bounded ticket reads: this ticket is a child of `06F5Q8YBVRS2EZVMJK5EATV9AR`, is currently blocked by `06F5Q8Y3WW9FFV7HA289VHCEAM`, and blocks five downstream tickets; no relation cleanup was needed in this refinement pass.
- Comment facts were already clear from the bounded ticket reads: there were no human comments to incorporate, only bot claim/lease comments.

### Scope In
- Define the internal staging lifecycle for one staged bulk-ingestion attempt, including stage creation, population, execution, cleanup, failure, and cancellation exit paths.
- Define the normalized internal row handoff from the explicit save pipeline into provider implementations for hub, link, and satellite rows while preserving existing ordering and metadata rules.
- Define caller-owned transaction participation, dirty-context preconditions, concurrency stance, and diagnostics gates for unsupported providers and unsupported request shapes.
- Define how oversized batches and provider/schema limitations are classified and rejected or declined before provider-specific staging work proceeds.

### Scope Out
- Adding a new public consumer-facing staged bulk API beyond the existing `IDataVaultSaveService` boundary.
- Implementing provider-specific temp-table/staging optimizations for individual provider packages.
- Changing unrelated explicit-save, bridge-maintenance, read-service, or EF design-time workflow contracts.
- Promising provider-native retry, merge, or cross-provider concurrency semantics beyond the bounded v1 internal contract.

## Acceptance Criteria
- An authoritative v1 contract defines the staging lifecycle for one provider-staged bulk attempt, including deterministic setup, execution, cleanup, cancellation handling, and behavior when stage setup or provider execution fails.
- The contract defines the internal handoff shape for normalized hub, link, and satellite work items and preserves existing caller-visible ordering, load-timestamp, and record-source rules without exposing unstable staging types as public API.
- The contract states that the caller continues to own the `DbContext`, current or ambient transaction, and cancellation token, and that provider staging participates in that boundary without creating, committing, rolling back, or suppressing transactions on the caller's behalf.
- The contract defines diagnostics and gates for unsupported providers, dirty EF contexts, multi-active shapes, oversized batches, and provider/schema limitations, including when the pipeline must fail fast before staging starts.
- The contract identifies the v1 concurrency stance for staged ingestion and the cleanup guarantees for successful, failed, and canceled attempts.
- The contract explicitly records that this SPI is internal implementation surface area and does not widen the settled public explicit-save contract.

## Definition of Done
- The ticket description or an authoritative attached/planning artifact contains the v1 provider-staging SPI and transaction contract and references the current explicit-save baseline documents.
- Downstream blocked tickets can rely on the contract without reopening public API shape, transaction ownership, ordering, or cancellation semantics.
- Diagnostics expectations are concrete enough to drive provider-neutral and provider-capability tests for supported versus unsupported shapes.
- Non-goals are documented: no new public staging API, no provider-specific rollout promises, and no change to caller-owned transaction/cancellation behavior.

## Implementation Notes
- Use `docs/architecture/dvault-v1-explicit-save-service.md` as baseline authority for the visible write boundary and v1 caller-owned transaction semantics.
- Use `docs/architecture/dvault-v1-streaming-explicit-save-contract.md` as baseline authority for ordered execution, explicit metadata flow, cancellation behavior, and bounded continuation semantics across chunked work.
- Use `docs/releases/v0.5.0.md` and `docs/releases/v0.19.0.md` as compatibility baselines: provider-specific save strategies already exist, staged provider bulk ingestion is additive future work, and this ticket should define internal contract shape rather than reset public compatibility.
- Anchor unsupported-shape diagnostics to provider capability/profile concepts so providers can consistently reject dirty contexts, multi-active shapes, oversized batches, and schema limitations without leaking provider-specific contracts into public namespaces.
- No bounded planning writes were applied during this refinement pass: no child tickets were created, no relations were changed, no ticket description update was queued, and no planning document or attachment was materialized.

## Open Questions
- none

## Follow-Up Questions
- After this internal SPI lands, should release notes publish a concise provider-coverage matrix for staged bulk ingestion support versus fallback/unsupported behavior?
- Do we want a follow-up ticket to standardize telemetry/event names and counters for staging-object creation, cleanup, and oversized-batch rejection?
- If a specific provider later needs materially different cleanup or concurrency behavior, should that be isolated in provider-specific follow-up tickets rather than broadening this shared contract?

## Risks
- Late churn in lifecycle or transaction rules will cascade into the five downstream tickets already blocked by this story.
- If oversized-batch and schema-limitation gates are under-specified, provider packages may diverge in when they reject equivalent request shapes.
- If the implementation leaks provider-specific staging abstractions into public namespaces, the project could accidentally take on a long-term public API support burden.

## Split Recommendations
- No additional split is recommended in this refinement pass; the live relation graph already shows this story serving as the contract/architecture blocker for five downstream tickets.
- If later evidence shows one provider needs materially different staging cleanup or transaction semantics, create a provider-specific follow-up ticket instead of widening this shared contract story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Define the internal provider staging contract for staged bulk ingestion.

Acceptance criteria:
- Specifies staging lifecycle, naming, transaction ownership, cleanup, cancellation, and concurrency behavior.
- Defines normalized hub/link/satellite row handoff to provider implementations without unstable public APIs.
- Adds diagnostics gates for unsupported providers, dirty contexts, multi-active shapes, oversized batches, and schema limitations.