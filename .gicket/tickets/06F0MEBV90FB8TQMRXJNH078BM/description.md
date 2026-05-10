<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story to the bounded v1 surface already implied by the architecture notes and current branch: thin typed mapper contracts, explicit save-service convenience helpers, and typed latest/as-of satellite projections over the existing read service. No blocking PO questions remain.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The ticket keeps DVault's explicit boundary intact. Typed helpers reduce raw name/value list assembly, but callers still choose when to save and still pass load timestamp and record source explicitly.
- The common hub plus satellite flow is satisfied by composing a typed hub save and a typed ordinary hub-parent satellite save; this story does not require a single aggregate-save API or hidden orchestration layer.
- Typed row mappers are thin one-source-to-one-operation adapters keyed by exact logical metadata names. Canonical metadata ordering and required-name validation stay with the existing registry and save/read pipelines.
- V1 typed link mappers are limited to link shapes whose participant hub metadata names are unique; repeated same-hub or self-link typed mappings remain out of scope for this contract.
- Typed save convenience helpers are intentionally narrower than the satellite mapper contract: they cover only ordinary hub-parent satellite writes. Link-parent and multi-active satellite writes continue through the existing explicit registry save requests in v1.
- Typed read projections are in scope for latest and as-of satellite reads, including ordinary, link-parent, and multi-active satellite rows, using explicit parent hash keys.

### Scope In
- Public core-package typed mapper contracts for hub, link, and satellite source-to-operation mapping.
- Typed single-row and bulk save helper extensions over IDataVaultSaveService for hubs, links, and ordinary hub-parent satellites, with explicit load timestamp and record source parameters.
- Typed latest/as-of satellite read projection helpers over IDataVaultReadService for explicit-metadata requests and registry-backed requests keyed by parent hash keys.
- Caller-owned DTO projection from satellite rows, including access to technical fields, payload values, and driving-key values within the current latest/as-of read baseline.

### Scope Out
- Automatic SaveChanges interception or any hidden persistence trigger.
- Implicit or ambient load timestamp or record source population that removes those inputs from the call site.
- A full LINQ provider, broad query abstraction, PIT or bridge helpers, or other read shapes beyond latest/as-of satellite reads by explicit parent hash keys.
- Typed convenience save helpers for link-parent satellites or multi-active satellites in v1.
- New mapper discovery or registration infrastructure, source generators, or a separate public validator layer.

## Acceptance Criteria
- A caller can implement IDataVaultHubMapper<TSource>, IDataVaultLinkMapper<TSource>, and IDataVaultSatelliteMapper<TSource> as thin one-source-to-one-operation adapters that use exact logical metadata names and leave load timestamp and record source outside the mapper contract.
- IDataVaultSaveService exposes typed helper APIs for single-row and caller-ordered bulk hub, link, and ordinary hub-parent satellite saves. Each helper accepts source data, mapper, load timestamp, and record source, assembles registry-backed requests, and executes through the existing explicit save-service pipeline.
- A caller can complete the common hub then ordinary hub-parent satellite flow without hand-building raw name/value lists, while still observing the explicit boundary between the hub save result and the subsequent satellite save.
- Typed save helper failures return deterministic diagnostics that identify the logical target, stable source type, and batch index when relevant, while missing required metadata-owned names continue to fail in the existing save-plan validation boundary.
- IDataVaultReadService exposes typed latest/as-of satellite projection helpers that accept either explicit satellite metadata or a registry-backed parent-plus-satellite request and project each selected row into a caller-owned DTO.
- Typed read projections cover ordinary, link-parent, and multi-active satellite rows within the current latest/as-of baseline and expose technical fields, payload values, and driving-key values with deterministic invalid, missing, or null value failures.

## Definition of Done
- The typed mapper contracts, typed save helpers, typed latest/as-of read requests, and typed read projection helpers are published from DCoding.Data.DVault as public provider-neutral APIs without requiring a new service type or options model.
- XML or API documentation and public API snapshot coverage describe the explicit boundary, including visible load timestamp and record source inputs and the v1 same-hub link limitation.
- Unit tests cover mapper contract behavior, request assembly, batch ordering, validation ownership, and diagnostic wrapping for typed save helpers.
- SQLite integration coverage proves the common hub plus ordinary satellite save flow and typed latest/as-of satellite projections, including link-parent reads, multi-active reads, and load-timestamp normalization across supported storage modes.

## Implementation Notes
- Use extension methods on the existing IDataVaultSaveService and IDataVaultReadService surfaces rather than introducing a parallel service abstraction.
- Typed save helpers should assemble DataVaultRegistrySaveRequest or DataVaultRegistryBulkSaveRequest and then delegate to the existing explicit save pipeline so hashing, reuse detection, provider strategy dispatch, and missing-name validation remain centralized.
- Keep load timestamp and record source as explicit method parameters. Normalize and validate them through the existing save pipeline rather than inventing mapper-local policy objects.
- The ordinary satellite save helpers should explicitly reject link-parent and driving-key satellite shapes with actionable diagnostics, because v1 convenience save scope is only the ordinary hub-parent case.
- Typed read helpers should stay on the narrow latest/as-of path by parent hash key and project through the existing satellite projection row and accessor surface, including reserved technical names for ParentHashKey, HashDiff, LoadTimestamp, and RecordSource.
- Registry-backed read adapters should resolve the authoritative metadata registry from the DbContext once and then delegate into the same explicit typed projection pipeline used by explicit-metadata requests.

## Open Questions
- none

## Follow-Up Questions
- Should a later story add typed convenience save helpers for link-parent and multi-active satellites, or is the raw registry request path sufficient for those shapes?
- If typed mapper adoption grows, do we want a separate DI or discovery story for mapper registration patterns, or should mapper selection remain entirely caller-owned?
- Do later read-model stories need broader query composition, PIT-backed projections, or bridge traversal helpers beyond the current explicit parent-hash latest/as-of baseline?

## Risks
- Because IDataVaultSatelliteMapper<TSource> supports link-parent and multi-active outputs while typed save convenience helpers do not, examples and docs must make that boundary explicit to avoid caller confusion.
- Typed read projection reserves the technical names ParentHashKey, HashDiff, LoadTimestamp, and RecordSource; metadata that reuses those names as payload or driving-key names will fail fast and must stay outside this v1 surface.
- Consumers may expect a broader typed query layer once projections exist, but this story intentionally remains limited to explicit latest/as-of satellite reads by parent hash key.

## Split Recommendations
- If the team wants one-call orchestration that chains hub save results into satellite writes, capture that as a separate convenience-layer story rather than expanding this ticket beyond the existing explicit save boundary.
- If typed save support is needed for link-parent or multi-active satellites, split that into a follow-on story with its own acceptance tests and diagnostics instead of widening this v1 ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Provide typed helper APIs that map domain objects or DTOs to explicit DVault save/read operations without hiding load timestamp, record source, or Data Vault write boundaries.

## Scope In

- Typed hub/link/satellite mapper contracts.
- Explicit save helpers over the existing save-service pipeline.
- Typed latest/as-of satellite read projections over the existing read service.

## Scope Out

- Automatic SaveChanges interception.
- Full LINQ provider or broad query abstraction.

## Acceptance Criteria

- Users can save a common hub plus satellite flow without assembling raw name/value lists by hand.
- Load timestamp and record source remain visible parameters or explicit policies.
- Typed read helpers project latest/as-of satellite rows into stable DTOs.