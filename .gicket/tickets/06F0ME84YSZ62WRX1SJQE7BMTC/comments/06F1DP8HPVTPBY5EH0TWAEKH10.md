[gicket-bot] PO refinement contract

Summary
- Refined the epic against the existing v0.6.0 planning, README, and release context. The v1 baseline is bounded: additive EF Code-First metadata, registry-backed shared metadata usage, explicit save/read services, diagnostics, and examples, with no PO-blocking questions remaining.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v0.6.0 Code-First baseline is hub declarations, hub-parent satellites, multi-active driving keys via DrivingKey(...), and ordered hub links through ApplyDataVaultMetadata(vault => ...).
- The public builder family lives in DCoding.Data.DVault and uses DataVaultCodeFirst* naming to avoid collision with the existing metadata-first modeling builders.
- Code-First projection remains additive and flows through DataVaultMetadataModel into the existing provider-aware ApplyDataVaultMetadata path.
- The explicit save boundary is retained: callers use IDataVaultSaveService with load timestamp and record source supplied at the request boundary; SaveChanges interception remains out of scope.
- Typed read helpers are for latest/as-of satellite projections using caller-owned projector delegates; raw satellite row reads remain the advanced escape hatch.
- Registry-backed metadata remains the documented path for shared metadata and examples; v0.6.0 does not expose a public Code-First-to-registry conversion API.

Scope In
- Epic coordination for fluent EF Code-First metadata covering hubs, hub-parent satellites, multi-active driving keys, and ordered hub links.
- Reusable metadata registry usage for shared schema projection, save/read service configuration, examples, and diagnostics.
- Typed explicit save/read usability improvements that keep load timestamp, record source, and Data Vault write boundaries visible.
- Validation and explain output for metadata models, registries, Code-First declarations, and configured DbContexts.
- README and runnable quickstart examples demonstrating the v0.6.0 happy path.
- Source-compatible preservation of v0.5 metadata-first APIs and existing explicit service behavior.

Scope Out
- SaveChanges interception or hidden Data Vault writes.
- Model-first JSON/YAML import or export specifications.
- Full PIT-backed read APIs, bridge traversal read helpers, PIT row maintenance, or bridge row maintenance.
- Provider-specific read optimizations beyond compatibility with existing metadata and raw read surfaces.
- Public Code-First-to-registry conversion as an authoritative registry source.
- Fluent link-parent satellite declarations for v0.6.0; metadata-first remains the available path for that shape.
- Hub logical-name overrides in the v1 Code-First surface; applications needing alternate logical hub names can use metadata-first declarations.

Open questions
- none

Follow-up questions
- Should a later release add a public Code-First-to-registry bridge for teams that want one fluent declaration to become the authoritative shared metadata source?
- Should fluent link-parent satellite declarations be added after the hub-parent v1 surface has enough adoption evidence?
- Should model-first JSON/YAML specs planned for v0.7.0 share validation and diagnostics infrastructure with the Code-First metadata path?
- Should future read work prioritize PIT-backed reads, bridge traversal reads, or provider-specific read optimizations first?
- Should a later convenience API wrap explicit saves without weakening the visible load timestamp and record source boundary?

Risks
- Because this is an epic spanning API, persistence, reads, diagnostics, and examples, child stories must stay aligned to the same bounded v0.6.0 contract to avoid documentation/API drift.
- Users may infer Code-First declarations are an authoritative registry source unless documentation continues to distinguish Code-First projection from registry-backed metadata.
- Typed read helper ergonomics must remain narrow enough to preserve explicit projection control and avoid implying a broader model-first read contract.

Split recommendations
- Keep this ticket as the umbrella epic and route implementation through bounded child stories rather than expanding the epic into direct implementation scope.
- If additional work is discovered, split by product surface: fluent API projection, registry integration, explicit save/read helpers, diagnostics/explain output, and examples/docs.
- Do not add new subtickets for v0.6.0 limitations already documented as future work unless a separate release planning decision promotes one of them into current scope.

Persisted contract coverage
- acceptance-criteria items: 9
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment