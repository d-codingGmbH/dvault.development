<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Republished the source-backed docs-only bridge refinement for PO-critic routing; scope remains unchanged and no split or planning write was needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Bridge tables remain an opt-in v0.5 deferred capability; this ticket documents the current deferred baseline rather than a concrete bridge runtime API.
- The authoritative inputs for this ticket are the current repository baseline and the cited planning and architecture documents already in context, not a future parent-only naming artifact.
- The documentation example is limited to exactly one conceptual many-to-many traversal scenario and must not invent bridge-specific APIs, annotations, EF metadata members, or generated table contracts not present on the branch.
- The existing parentOf relation from 06EZ0NTV4SVAKV98C418T8A3CC already captures the ticket tree, so no relation changes were needed.
- No child tickets, planning documents, or attachments were materialized in this refinement pass because the existing bounded contract already covered the work.

### Scope In
- Document bridge tables as an opt-in v0.5 deferred capability layered on the current hub, link, and satellite baseline.
- Explain that the visible repository baseline exposes no bridge-specific EF metadata translator output and no bridge-specific annotation contract today.
- Provide exactly one minimal many-to-many traversal scenario described in current repository vocabulary and clearly marked as a deferred bridge-use-case example, not as a source-backed API walkthrough.
- Reuse the current public vocabulary around AddDVault(), UseDataVault(), ApplyDataVaultMetadata(), and IDataVaultSaveService.

### Scope Out
- Implementing bridge modeling, EF metadata projection, save behavior, validation, or tests.
- Inventing bridge-specific APIs, annotations, EF metadata members, generated table names, or provider behaviors not proven on the branch.
- Hierarchy-depth semantics, recursive traversal behavior, provider-specific DDL or migrations, PIT interactions, and multi-active interactions beyond explicit deferred notes.
- Runnable sample applications or broad README expansion beyond the bounded bridge documentation task.

## Acceptance Criteria
- Documentation explains bridge tables as an opt-in v0.5 deferred capability rather than part of ordinary hub, link, and satellite setup.
- Documentation states that the visible repository baseline does not currently expose bridge-specific EF metadata translator output or bridge-specific annotation names.
- Documentation uses only current repository vocabulary and high-level bridge terminology already present in planning docs; it does not invent bridge-specific APIs, generated names, or table shapes.
- Documentation includes exactly one minimal many-to-many traversal scenario framed as a conceptual deferred-capability example rather than as proof of implemented bridge runtime behavior.
- Documentation explicitly marks hierarchy-specific behavior, provider-specific behavior, PIT implications, and multi-active implications as unsupported or deferred unless later tickets define them.

## Definition of Done
- A developer can implement the docs update from current repository evidence without waiting for parent story 06EZ0NTV4SVAKV98C418T8A3CC.
- The resulting page makes the deferred status and lack of current bridge runtime surface explicit while still giving one clear many-to-many scenario.
- Any later parent-driven bridge naming or shape details can be handled as a follow-up docs sync instead of blocking this ticket.

## Implementation Notes
- docs/plans/deferred-data-vault-capabilities.md already describes bridge tables as a deferred v0.5 capability and keeps them outside the ordinary hub, link, and satellite baseline.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs creates only hub, link, and satellite projections in CreateEntities and exposes no bridge projection path in the visible branch snapshot.
- src/DCoding.Data.DVault/DataVaultAnnotationNames.cs exposes provider-neutral annotation names for conventions, produced names, entity kind, metadata name, parent reference, ordinal, property role, technical column role, and provider metadata, but no bridge-specific annotation contract.
- docs/architecture/dvault-v1-explicit-save-service.md and README.md preserve the current public vocabulary around AddDVault(), UseDataVault(), ApplyDataVaultMetadata(), and IDataVaultSaveService.
- No ticket attachments or branch artifacts in the provided context publish a concrete bridge implementation surface, so the deliverable stays architecture-level and source-backed.

## Open Questions
- none

## Follow-Up Questions
- If parent story 06EZ0NTV4SVAKV98C418T8A3CC later publishes concrete bridge names or shapes, should a small follow-up docs sync mirror those terms?
- After this child is implemented, should README.md add a short cross-link to the bridge documentation page?
- If hierarchy-style traversal later needs a worked example, should that become a separate follow-up docs ticket rather than expanding this page?

## Risks
- A later bridge implementation ticket may introduce concrete naming or runtime semantics that require a small terminology sync in the docs.
- The docs could drift into speculative API design unless they stay anchored to the current source-backed deferred baseline.

## Split Recommendations
- No split is required for the current bounded docs-only task.
- If later work needs hierarchy-specific walkthroughs, runnable samples, or docs tied to implemented bridge APIs, create separate follow-up docs tickets.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: document bridge table support with a minimal example.

Acceptance Criteria:
- Documentation explains bridge use cases and the supported v0.5 baseline.
- The example is aligned with generated model behavior and existing naming conventions.
- Unsupported traversal patterns are called out rather than implied.