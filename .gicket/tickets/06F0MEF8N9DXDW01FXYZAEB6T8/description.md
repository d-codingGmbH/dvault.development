<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story around the existing dvault.model.v1 governance baseline, deterministic export, manual drift reporting, and docs updates.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 export target is the existing canonical JSON-first dvault.model.v1 artifact contract documented in docs/model-first-governance.md and docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md.
- The v1 default naming policy is the repository default policy with naming.policy set to default; alternate naming policies are future advanced configuration work and do not block this ticket.
- Drift comparison is manual tooling/library functionality for review evidence and does not include database migration execution, release publishing, or CI automation.
- The comparison baseline is the expected dvault.model.v1 artifact versus generated/current EF metadata and produced table metadata visible through DVault annotations, produced names, declaration ordering, and provider-neutral roles.

### Scope In
- Add deterministic export from DVault Code-First declarations and registry-backed metadata into canonical dvault.model.v1 JSON artifacts.
- Preserve the existing v1 artifact envelope, declaration categories, strict schemaVersion value, default values, stable declaration ordering, and unknown-field behavior when exporting.
- Add drift tooling that compares an expected dvault.model.v1 model against generated/current EF/table metadata for provider-neutral Data Vault structures.
- Report added, removed, renamed, and incompatible drift across relevant tables, columns, indexes, constraints, entity kinds, metadata names, parent/participant references, ordering, and provider-neutral property roles where that metadata is available.
- Update model governance documentation so teams can export artifacts, run drift comparison manually, and use the report as pre-release review evidence without release credentials.

### Scope Out
- Executing or generating database migrations.
- CI publishing automation or release-gate wiring.
- Direct YAML ingestion, YAML fixture contracts, or a core YAML dependency.
- Provider-specific DDL diffing beyond the provider-neutral EF/table metadata available in the current branch.
- Changing the dvault.model.v1 schema contract or introducing v2 artifact compatibility.
- Advanced custom naming, hashing, timestamp, record-source, or provider hook implementation beyond honoring the existing v1 defaults.

## Acceptance Criteria
- Export from Code-First and registry-backed metadata produces valid dvault.model.v1 JSON using the documented top-level categories hubs, links, satellites, pits, and bridges.
- Export is deterministic across repeated runs for the same model: declaration order, object shape, default values, and JSON field ordering are stable.
- Export uses schemaVersion exactly equal to dvault.model.v1 and emits or resolves naming.policy and loadTimestampStorage according to the documented v1 defaults and supported tokens.
- Exported artifacts can be consumed by the existing model-first import/validation path without lossy changes for supported v1 concepts.
- Drift comparison accepts an expected dvault.model.v1 artifact and generated/current EF metadata as inputs and returns a structured report suitable for manual review.
- Drift reports identify added, removed, renamed, and incompatible table, column, index, and constraint elements where those elements are represented in the available provider-neutral metadata.
- Drift reports include enough location detail to identify the affected declaration, produced table/entity, produced column/property, metadata role, and expected versus actual values.
- Drift comparison preserves stable ordering in report output so equivalent inputs produce equivalent reports.
- Manual usage is documented and does not require release credentials, package publishing credentials, or database migration execution.
- Tests cover deterministic export, empty/default artifact export, representative hub/link/satellite/PIT/bridge export, no-drift comparison, and representative drift categories.

## Definition of Done
- Public or internal APIs added for export and drift comparison follow existing DVault naming, namespace, and source layout conventions under src/DCoding.Data.DVault.
- Focused tests are added under tests/DCoding.Data.DVault.Tests for exporter determinism, v1 contract compatibility, drift classification, and report ordering.
- Documentation in the model-first governance workflow describes the export and drift steps, expected review artifacts, and manual pre-release usage.
- The implementation builds and the relevant DVault test suite passes in the target branch.
- Error and report messages use precise artifact/metadata locations and avoid requiring provider-specific database access for the manual workflow.

## Implementation Notes
- Use the existing dvault.model.v1 schema contract as authoritative: schemaVersion is required and exact, declaration arrays default to empty, naming.policy defaults to default, and loadTimestampStorage defaults to provider-default.
- Favor structured metadata and EF annotations such as DataVaultAnnotationNames.EntityKind, MetadataName, ProducedName, ParentReferenceKind, ParentReferenceName, Ordinal, PropertyRole, TechnicalColumnRole, ProviderProfile, ProviderStorageType, ProviderValueFormat, MetadataSourceKind, and MetadataSourceFingerprint rather than ad hoc name parsing when comparing generated metadata.
- Treat declaration array order as part of the governed model and preserve it in both export and drift evidence.
- The v1 drift report should be provider-neutral first; provider-specific storage differences should only be reported when they are already represented through DVault provider metadata annotations.
- Renames can be reported when a stable metadata identity maps to a different produced name; otherwise added plus removed entries are acceptable evidence for unmatched items.
- Keep command-line or manual tooling optional and lightweight if added; the core deliverable is reusable export and drift functionality plus documentation.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket wire drift comparison into CI or repository release gates once the manual workflow is proven?
- Should future schema versions add provider-specific DDL drift or migration-plan generation beyond provider-neutral metadata comparison?
- Should future advanced configuration tickets expose custom naming/hash/timestamp policies in exported model artifacts once those hooks exist?

## Risks
- Rename detection may be limited when metadata lacks a stable identity that survives produced-name changes; report unmatched items as added and removed rather than guessing.
- Provider-specific EF metadata can vary by provider, so this story should keep v1 drift semantics grounded in DVault-owned provider-neutral annotations and documented logical metadata.
- PIT and bridge support depends on the current branch's available metadata surfaces; tests should pin the supported v1 shapes and report unsupported comparison gaps explicitly.

## Split Recommendations
- If implementation size grows, split into exporter implementation, drift report implementation, and documentation/examples as separate delivery slices while keeping this story's v1 contract unchanged.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Let teams export configured models and compare expected model artifacts against generated/current Data Vault metadata so governance issues are visible before release.

## Scope In

- Export from registry/Code-First model to model-first artifact.
- Drift report comparing expected model versus generated EF/table metadata.
- Documentation for model governance workflow.

## Scope Out

- Database migration execution.
- CI publishing automation.

## Acceptance Criteria

- Exported artifacts are deterministic.
- Drift reports identify added, removed, renamed, or incompatible table/column/index/constraint elements.
- The workflow can be used manually without release credentials.