<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the epic as a closure-ready umbrella over four already-materialized delivery tracks: model-first import, model export/drift tooling, PIT/bridge read helpers, and provider-aware read optimization follow-up. Current branch docs and source already establish the bounded v0.7 model-first and advanced read-model baseline, so no new child tickets, relation changes, attachments, or planning documents were needed in this pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The ticket snapshot shows no recent comments, so this pass ratifies branch evidence and existing child-ticket outcomes rather than responding to a new human scope change.
- The model-first baseline is already fixed on branch as governed JSON-first dvault.model.v1 with exact schemaVersion matching, canonical hubs/links/satellites/pits/bridges categories, strict unknown-field rejection, declaration-order preservation, naming.policy default = default, and loadTimestampStorage default = provider-default.
- The advanced read-model baseline is already bounded on branch to implemented latest/as-of satellite reads, provider-neutral PIT-backed as-of reads, provider-neutral bridge reads, and provider-aware optimization hooks rather than PIT refresh, bridge maintenance, unbounded graph traversal, or blanket provider-specific SQL behavior.
- Already-created child tickets cover the epic split: 06F0MEE0NC2009J73PP0ATE6YW for model-first import, 06F0MEF8N9DXDW01FXYZAEB6T8 for model export and drift tooling, 06F0MEGPPETJD4ZDEN5ESGR7JW for PIT and bridge read/query helpers, and 06F0MEHSH6S31ZE4K0Q3EKR784 for provider-aware read optimization follow-up; all four are already done.
- docs/releases/v0.7.0.md is the current branch release baseline for this epic; docs/releases/v0.6.0.md remains historical context and should not reopen already-delivered model-first, PIT, or bridge scope.
- No new child tickets, relation updates, attachments, or planning documents were materialized during this refinement pass.

### Scope In
- Epic-level closure and contract consistency across the existing child deliveries for model-first import, export/drift, PIT/bridge reads, and provider-aware optimization.
- Governed dvault.model.v1 import, canonical export, drift comparison, and projection into the existing registry and EF metadata path used by DVault.
- Provider-neutral PIT as-of reads and bridge traversal read helpers backed by implemented metadata, raw-row contracts, typed projector helpers, and bounded diagnostics.
- Benchmark-informed provider-aware read-strategy hooks where optimized paths remain additive to provider-neutral fallback behavior.

### Scope Out
- Replacing the existing Code-First or metadata-first declaration paths.
- Direct YAML ingestion, YAML-specific parser semantics, or a core YAML dependency.
- PIT refresh or maintenance orchestration, bridge row maintenance or closure generation, unbounded graph traversal, or broad ORM-style abstraction over Data Vault semantics.
- Provider-specific optimization for every provider and every read shape in this epic.
- Automatic database, container, or benchmark-environment provisioning.

## Acceptance Criteria
- A governed dvault.model.v1 artifact can be imported, validated, exported canonically, drift-compared, and projected into the same registry and EF metadata surfaces used by existing DVault flows.
- Model-first import/export/drift behavior preserves the documented v1 contract: exact schemaVersion matching, canonical declaration categories, strict unknown-field rejection, stable declaration ordering, and documented default values.
- PIT-backed reads remain source-backed by implemented DataVaultPitMetadata behavior, including bounded request validation, raw-row reads, and caller-owned typed projection helpers.
- Bridge reads remain source-backed by implemented generated bridge metadata, exact endpoint-column access, bounded hierarchy depth handling, and deterministic diagnostics for unsupported shapes.
- Provider-aware read optimization remains benchmark-driven and additive: registered strategies are evaluated ahead of provider-neutral fallback without changing caller-facing provider-neutral read-service contracts.
- Epic completion is satisfied by the existing child-ticket set and current branch evidence rather than by new parent-ticket implementation work.

## Definition of Done
- The existing child tickets for import, export/drift, PIT/bridge reads, and provider-aware optimization remain done and continue to match the refined epic contract.
- Repository documentation and source remain aligned on the v0.7 model-first governance baseline and the bounded advanced read-model baseline.
- Source, test, and public API evidence from the delivered child work remain consistent with this refined epic contract.
- The epic contract does not imply PIT/bridge maintenance ownership, direct YAML ingestion, or provider-specific optimization beyond the bounded surfaces already evidenced on branch.
- No blocking PO clarification remains for epic scope, split shape, or baseline architecture.

## Implementation Notes
- Use docs/model-first-governance.md and docs/releases/v0.7.0.md as the current branch policy and release baseline for the epic; treat docs/releases/v0.6.0.md as historical context only.
- Treat 06F0MEE0NC2009J73PP0ATE6YW as the authoritative delivery owner for dvault.model.v1 import and projection semantics.
- Treat 06F0MEF8N9DXDW01FXYZAEB6T8 as the authoritative delivery owner for canonical export and drift reporting.
- Treat 06F0MEGPPETJD4ZDEN5ESGR7JW as the authoritative delivery owner for PIT and bridge read helpers, including the bounded hierarchy maximumDepth rule and exact generated bridge-column access surfaces reflected in current source.
- Treat 06F0MEHSH6S31ZE4K0Q3EKR784 as the authoritative delivery owner for provider-aware read-strategy hooks and benchmark-backed optimization follow-up.
- Current branch source evidence includes DataVaultAnnotationNames bridge and PIT-related roles, DataVaultBridgeReadPipeline, DataVaultBridgeReadRecord, DataVaultBridgeProjectionRow, and the PIT contract plan in docs/plans/06F0MEGYHADPVN575H64D56W2G-pit-backed-as-of-read-api-contract.md.
- No new planning artifact was written because the epic split is already materialized and this pass was refinement-only.

## Open Questions
- none

## Follow-Up Questions
- Should a later README or quickstart update make the bounded hierarchy maximumDepth requirement more explicit for bridge-read consumers?
- Should later provider-specific work optimize PIT or bridge reads beyond the current benchmark-backed latest/as-of satellite optimization baseline?
- Should later release or CI work surface drift reports and benchmark artifacts more explicitly once the manual governance workflow has settled?

## Risks
- Historical docs such as docs/releases/v0.6.0.md can still be misread as current capability posture unless reviewers anchor on docs/releases/v0.7.0.md and the refined child-ticket contracts.
- Consumers may overread the advanced read-model baseline as including PIT refresh, bridge maintenance, or unbounded traversal unless downstream docs continue to keep those boundaries explicit.
- Provider-aware optimization claims should stay tied to benchmarked, branch-visible evidence so the epic is not interpreted as blanket optimization coverage for every provider or read shape.

## Split Recommendations
- No further split is recommended. The epic is already decomposed into four done child tickets covering model-first import, export/drift tooling, PIT/bridge read helpers, and provider-aware optimization follow-up.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Add durable model-first specifications and advanced read-model helpers after v0.6 establishes a friendlier Code-First and registry baseline.

## Scope In

- Versioned model-first schema and import/export path.
- Validation and drift reports for model governance.
- PIT-backed as-of reads and bridge traversal helper baseline.
- Benchmark-driven provider-aware read optimization hooks.

## Scope Out

- Replacing the v0.6 Code-First path.
- Hiding Data Vault semantics behind a broad ORM abstraction.
- Automatic database/container provisioning.

## Acceptance Criteria

- A model-first artifact can be validated and projected into the same registry/EF metadata surface used by Code-First.
- Read helpers are source-backed by implemented PIT/bridge metadata behavior.
- Provider read optimization work is benchmarked and gated by correctness tests.