<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Revised the ticket contract to keep the public export surface on existing DataVaultMetadataRegistry/DataVaultMetadataModel inputs and to make legacy PointInTimeTables a deterministic rejection case, so the ticket can return to PO-critic without creating child tickets or planning documents.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Public exporter inputs are existing DataVaultMetadataRegistry and optionally DataVaultMetadataModel instances; raw ApplyDataVaultMetadata(vault => ...), Action<DataVaultCodeFirstModelBuilder>, and direct ModelBuilder export entry points remain out of scope.
- Code-First-originated coverage remains in scope only as proof that metadata produced by the current internal Code-First projection path or equivalent package-owned materialization can be exported once it exists as DataVaultMetadataModel/DataVaultMetadataRegistry.
- Legacy PointInTimeTables are not serializable to dvault.model.v1; non-empty PointInTimeTables must trigger deterministic export failure instead of silent omission or lossy adaptation.
- No child tickets, relation updates, attachments, or planning documents were materialized in this refinement pass.

### Scope In
- Public export of an existing DataVaultMetadataRegistry to strict JSON dvault.model.v1 from DCoding.Data.DVault.
- Optional parallel export of an existing DataVaultMetadataModel when it can follow the same canonical registry/model traversal and artifact contract.
- Deterministic serialization of hubs, links, satellites, pits, bridges, naming.policy default, and loadTimestampStorage tokens already representable in dvault.model.v1.
- Code-First-originated metadata coverage after the metadata has already been materialized into DataVaultMetadataModel/DataVaultMetadataRegistry by existing package paths.
- Deterministic rejection and documentation for non-empty legacy PointInTimeTables inputs.

### Scope Out
- A new public Code-First-to-registry or Code-First-to-model bridge/export entry point from raw fluent declarations.
- Export overloads that accept raw Action<DataVaultCodeFirstModelBuilder>, ModelBuilder, or fluent builder syntax as the public artifact source.
- Silent omission or automatic adaptation of legacy PointInTimeTables into dvault.model.v1 pits.
- Provider-specific DDL, schema extraction, drift tooling, YAML export, or code generation from fluent builder syntax.
- Export of runtime-only save/read state, provider dispatch behavior, or other non-artifact runtime details.

## Acceptance Criteria
- A caller can export an existing DataVaultMetadataRegistry to a strict JSON dvault.model.v1 artifact through a public API in DCoding.Data.DVault; if a DataVaultMetadataModel overload is provided, it emits the same contract and ordering semantics.
- The public API contract does not promise direct export from raw Code-First declarations; Code-First-originated coverage is satisfied by exporting metadata after it has already been materialized into DataVaultMetadataModel/DataVaultMetadataRegistry.
- The exporter emits schemaVersion as dvault.model.v1 and serializes only fields defined by docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md, with stable property order, stable declaration order, and stable formatting across repeated runs.
- The exporter preserves supported hubs, links, satellites, pits, bridges, naming policy, and loadTimestampStorage choices present in the source model/registry.
- If the source model/registry contains any legacy PointInTimeTables entries, export fails deterministically with caller-visible diagnostics that name the unsupported legacy surface instead of silently omitting or adapting it.
- Representative successful exports round-trip through DataVaultModelArtifactImporter.ImportJson without diagnostics for the supported shape, and tests cover both successful Pits export and legacy PointInTimeTables rejection.
- Public XML/docs state that the artifact is provider-neutral and that raw Code-First declarations and legacy PointInTimeTables are not public dvault.model.v1 export inputs for this ticket.

## Definition of Done
- Exporter API and implementation ship in DCoding.Data.DVault without adding product-code dependencies outside the existing JSON/model-first boundary.
- Tests cover deterministic registry export, optional DataVaultMetadataModel overload behavior, successful Code-First-produced metadata export after materialization, successful Pits export, and rejection of legacy PointInTimeTables.
- Public XML docs and any touched model-first docs explicitly distinguish supported public inputs from raw Code-First declarations and explain the PointInTimeTables rejection behavior.
- Implementation preserves canonical registry/model declaration order and stable serialization behavior for repeated exports of the same supported input.
- Existing relevant tests continue to pass.

## Implementation Notes
- Center the public API on DataVaultMetadataRegistry and optionally DataVaultMetadataModel; do not add a public overload that accepts raw fluent Code-First declarations, Action<DataVaultCodeFirstModelBuilder>, or ModelBuilder.
- Use the same canonical ordered metadata surfaces already exposed by DataVaultMetadataRegistry/DataVaultMetadataModel for hubs, links, satellites, pits, bridges, naming, and load timestamp storage.
- Detect non-empty PointInTimeTables before serialization and return deterministic failure that explicitly names the legacy surface and the lack of a dvault.model.v1 pointInTimeTables contract.
- Keep successful round-trip coverage aligned to the existing importer boundary and add explicit tests that prove rejection is deterministic for legacy PointInTimeTables.

## Open Questions
- none

## Follow-Up Questions
- A later ticket can decide whether to add a public Code-First-to-model/registry bridge for callers who want direct export from fluent declarations.
- A later model-first ticket can decide whether legacy PointInTimeTables deserves a migration helper or additive adapter to pits.
- A later tooling ticket can decide whether the exporter should also surface through CLI or build integration.

## Risks
- Existing callers that still populate legacy PointInTimeTables will receive export failures until they migrate to Pits, so diagnostics and docs must be explicit.
- Test-only/internal Code-First coverage must not be documented in a way that implies a new public raw Code-First export API.
- Serializer configuration must still be fixed by tests so rejection handling does not mask ordering or formatting regressions on successful exports.

## Split Recommendations
- No split recommended; the ticket is bounded once the public input surface is limited to existing model/registry objects and PointInTimeTables are defined as a deterministic rejection case.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Export the configured registry/model into a deterministic model-first artifact for review and version control.

## Scope In

- Export from Code-First/registry representation.
- Stable ordering and formatting.
- Round-trip compatibility tests where in scope.

## Scope Out

- Drift reporting.
- Database schema extraction.

## Acceptance Criteria

- Export output is deterministic across runs.
- Export preserves supported logical names, keys, payloads, driving keys, PIT, bridge, naming, and timestamp storage choices.
- Unsupported runtime-only details are omitted or documented clearly.