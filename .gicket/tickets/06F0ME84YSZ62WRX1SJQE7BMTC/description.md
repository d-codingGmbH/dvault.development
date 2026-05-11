<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the epic against the existing v0.6.0 planning, README, and release context. The v1 baseline is bounded: additive EF Code-First metadata, registry-backed shared metadata usage, explicit save/read services, diagnostics, and examples, with no PO-blocking questions remaining.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v0.6.0 Code-First baseline is hub declarations, hub-parent satellites, multi-active driving keys via DrivingKey(...), and ordered hub links through ApplyDataVaultMetadata(vault => ...).
- The public builder family lives in DCoding.Data.DVault and uses DataVaultCodeFirst* naming to avoid collision with the existing metadata-first modeling builders.
- Code-First projection remains additive and flows through DataVaultMetadataModel into the existing provider-aware ApplyDataVaultMetadata path.
- The explicit save boundary is retained: callers use IDataVaultSaveService with load timestamp and record source supplied at the request boundary; SaveChanges interception remains out of scope.
- Typed read helpers are for latest/as-of satellite projections using caller-owned projector delegates; raw satellite row reads remain the advanced escape hatch.
- Registry-backed metadata remains the documented path for shared metadata and examples; v0.6.0 does not expose a public Code-First-to-registry conversion API.

### Scope In
- Epic coordination for fluent EF Code-First metadata covering hubs, hub-parent satellites, multi-active driving keys, and ordered hub links.
- Reusable metadata registry usage for shared schema projection, save/read service configuration, examples, and diagnostics.
- Typed explicit save/read usability improvements that keep load timestamp, record source, and Data Vault write boundaries visible.
- Validation and explain output for metadata models, registries, Code-First declarations, and configured DbContexts.
- README and runnable quickstart examples demonstrating the v0.6.0 happy path.
- Source-compatible preservation of v0.5 metadata-first APIs and existing explicit service behavior.

### Scope Out
- SaveChanges interception or hidden Data Vault writes.
- Model-first JSON/YAML import or export specifications.
- Full PIT-backed read APIs, bridge traversal read helpers, PIT row maintenance, or bridge row maintenance.
- Provider-specific read optimizations beyond compatibility with existing metadata and raw read surfaces.
- Public Code-First-to-registry conversion as an authoritative registry source.
- Fluent link-parent satellite declarations for v0.6.0; metadata-first remains the available path for that shape.
- Hub logical-name overrides in the v1 Code-First surface; applications needing alternate logical hub names can use metadata-first declarations.

## Acceptance Criteria
- A small .NET/EF Core domain model can declare Data Vault hubs, hub-parent satellites, multi-active driving keys, and ordered hub links with the documented fluent Code-First API.
- The Code-First API projects to the existing provider-aware schema conventions without requiring callers to recreate equivalent metadata objects across schema, save, and read paths for the happy path.
- Business keys, payload fields, driving keys, and link participants preserve caller declaration order where order affects generated metadata.
- Unsupported selector or link shapes produce actionable validation errors that point callers toward supported repeated single-member declarations or metadata-first alternatives.
- Explicit save helpers preserve visible load timestamp and record source inputs and do not hide writes behind DbContext.SaveChanges interception.
- Typed latest/as-of satellite read helpers support caller-owned projection delegates while keeping raw row-level reads available.
- Diagnostics and explain output cover metadata-first, registry-backed, Code-First, and configured DbContext scenarios sufficiently for users to understand the projected model.
- README and quickstarts show the recommended v0.6.0 path and identify bounded limitations and compatibility paths.
- Existing v0.5 metadata-first APIs remain source-compatible.

## Definition of Done
- All child stories needed for the bounded v0.6.0 Code-First usability flow are either completed or explicitly documented as out of scope/follow-up.
- Public docs and examples align with the implemented API surface and do not advertise unsupported Code-First shapes as available.
- Tests or validation evidence cover hub, hub-parent satellite, driving-key, link, registry, typed read, diagnostics, and compatibility paths at the level appropriate to each child story.
- Release notes document compatibility, known limitations, and the explicit persistence boundary.
- No implemented path requires SaveChanges interception, model-first specs, PIT/bridge runtime reads, or a Code-First-to-registry bridge to satisfy the v0.6.0 happy path.

## Implementation Notes
- Use docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md as the authoritative fluent Code-First contract for child implementation alignment.
- Keep DataVaultCodeFirstModelBuilder and related public builders in DCoding.Data.DVault beside UseDataVault() and ApplyDataVaultMetadata().
- Project Code-First declarations into DataVaultMetadataModel and reuse existing provider-aware schema projection rather than creating a separate schema path.
- Treat repeated BusinessKey(...), Payload(...), DrivingKey(...), and Participant<TEntity>() calls as canonical declaration-order inputs.
- Keep typed reads delegate-based for v0.6.0; do not add reflection-driven DTO binding under this epic.
- Keep registry-backed metadata as the shared model source for examples and application-wide reuse until a future Code-First-to-registry bridge is explicitly scoped.

## Open Questions
- none

## Follow-Up Questions
- Should a later release add a public Code-First-to-registry bridge for teams that want one fluent declaration to become the authoritative shared metadata source?
- Should fluent link-parent satellite declarations be added after the hub-parent v1 surface has enough adoption evidence?
- Should model-first JSON/YAML specs planned for v0.7.0 share validation and diagnostics infrastructure with the Code-First metadata path?
- Should future read work prioritize PIT-backed reads, bridge traversal reads, or provider-specific read optimizations first?
- Should a later convenience API wrap explicit saves without weakening the visible load timestamp and record source boundary?

## Risks
- Because this is an epic spanning API, persistence, reads, diagnostics, and examples, child stories must stay aligned to the same bounded v0.6.0 contract to avoid documentation/API drift.
- Users may infer Code-First declarations are an authoritative registry source unless documentation continues to distinguish Code-First projection from registry-backed metadata.
- Typed read helper ergonomics must remain narrow enough to preserve explicit projection control and avoid implying a broader model-first read contract.

## Split Recommendations
- Keep this ticket as the umbrella epic and route implementation through bounded child stories rather than expanding the epic into direct implementation scope.
- If additional work is discovered, split by product surface: fluent API projection, registry integration, explicit save/read helpers, diagnostics/explain output, and examples/docs.
- Do not add new subtickets for v0.6.0 limitations already documented as future work unless a separate release planning decision promotes one of them into current scope.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Make ordinary DVault usage feel natural in a .NET/EF Core application by adding a fluent Code-First modeling surface, a reusable model registry, typed explicit save/read helpers, diagnostics, and examples.

## Scope In

- Fluent EF Code-First API for hubs, links, satellites, and ordinary satellite read/write workflows.
- Central registry for schema projection, save services, read services, examples, and diagnostics.
- Typed explicit save/read helpers that keep load timestamp and record source visible.
- Validation/explain output and starter examples.

## Scope Out

- SaveChanges interception or hidden writes.
- Model-first JSON/YAML specifications; those are planned for v0.7.0.
- Full PIT/bridge runtime read models beyond compatibility with current metadata.

## Acceptance Criteria

- A small domain model can be configured with a concise fluent API and projected to existing schema conventions.
- Users do not need to recreate equivalent metadata objects in schema, save, and read code for the happy path.
- Typed helpers preserve explicit Data Vault write boundaries.
- Existing v0.5 APIs remain source-compatible.