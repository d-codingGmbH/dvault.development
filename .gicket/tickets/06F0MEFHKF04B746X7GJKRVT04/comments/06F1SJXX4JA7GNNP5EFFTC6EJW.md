[gicket-bot] PO refinement contract

Summary
- Revised the ticket contract to keep the public export surface on existing DataVaultMetadataRegistry/DataVaultMetadataModel inputs and to make legacy PointInTimeTables a deterministic rejection case, so the ticket can return to PO-critic without creating child tickets or planning documents.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Public caller scope is limited to exporting already-materialized DataVaultMetadataRegistry and, if implemented, DataVaultMetadataModel instances. This ticket does not add a public bridge or export entry point from raw Code-First declarations, fluent builder delegates, or ModelBuilder state.
- critic-item-2: `answered` - dvault.model.v1 export is defined only for Pits/pits. If the source DataVaultMetadataModel or DataVaultMetadataRegistry contains any legacy PointInTimeTables entries, export must fail with deterministic diagnostics that name the unsupported legacy surface; this ticket does not silently omit or adapt PointInTimeTables.
- critic-item-3: `answered` - Acceptance criteria and definition of done are updated accordingly: the public API promise is limited to existing model/registry inputs, Code-First coverage is internal/test-path only after metadata materialization, and PointInTimeTables are a deterministic rejection case with documentation and tests.
- critic-item-4: `answered` - The blocking Code-First gap is resolved by explicitly keeping raw Code-First-to-registry export out of scope for this ticket. Code-First-originated support means exporting metadata only after it has already been projected into DataVaultMetadataModel or DataVaultMetadataRegistry by existing internal or package-owned paths; no new public bridge is promised here.
- critic-item-5: `answered` - The blocking PIT surface gap is resolved by choosing deterministic rejection for legacy PointInTimeTables and using only Pits/pits for successful dvault.model.v1 export. This avoids silent data loss and keeps the exporter aligned with the published schema contract.

Clarifications
- Public exporter inputs are existing DataVaultMetadataRegistry and optionally DataVaultMetadataModel instances; raw ApplyDataVaultMetadata(vault => ...), Action<DataVaultCodeFirstModelBuilder>, and direct ModelBuilder export entry points remain out of scope.
- Code-First-originated coverage remains in scope only as proof that metadata produced by the current internal Code-First projection path or equivalent package-owned materialization can be exported once it exists as DataVaultMetadataModel/DataVaultMetadataRegistry.
- Legacy PointInTimeTables are not serializable to dvault.model.v1; non-empty PointInTimeTables must trigger deterministic export failure instead of silent omission or lossy adaptation.
- No child tickets, relation updates, attachments, or planning documents were materialized in this refinement pass.

Scope In
- Public export of an existing DataVaultMetadataRegistry to strict JSON dvault.model.v1 from DCoding.Data.DVault.
- Optional parallel export of an existing DataVaultMetadataModel when it can follow the same canonical registry/model traversal and artifact contract.
- Deterministic serialization of hubs, links, satellites, pits, bridges, naming.policy default, and loadTimestampStorage tokens already representable in dvault.model.v1.
- Code-First-originated metadata coverage after the metadata has already been materialized into DataVaultMetadataModel/DataVaultMetadataRegistry by existing package paths.
- Deterministic rejection and documentation for non-empty legacy PointInTimeTables inputs.

Scope Out
- A new public Code-First-to-registry or Code-First-to-model bridge/export entry point from raw fluent declarations.
- Export overloads that accept raw Action<DataVaultCodeFirstModelBuilder>, ModelBuilder, or fluent builder syntax as the public artifact source.
- Silent omission or automatic adaptation of legacy PointInTimeTables into dvault.model.v1 pits.
- Provider-specific DDL, schema extraction, drift tooling, YAML export, or code generation from fluent builder syntax.
- Export of runtime-only save/read state, provider dispatch behavior, or other non-artifact runtime details.

Open questions
- none

Follow-up questions
- A later ticket can decide whether to add a public Code-First-to-model/registry bridge for callers who want direct export from fluent declarations.
- A later model-first ticket can decide whether legacy PointInTimeTables deserves a migration helper or additive adapter to pits.
- A later tooling ticket can decide whether the exporter should also surface through CLI or build integration.

Risks
- Existing callers that still populate legacy PointInTimeTables will receive export failures until they migrate to Pits, so diagnostics and docs must be explicit.
- Test-only/internal Code-First coverage must not be documented in a way that implies a new public raw Code-First export API.
- Serializer configuration must still be fixed by tests so rejection handling does not mask ordering or formatting regressions on successful exports.

Split recommendations
- No split recommended; the ticket is bounded once the public input surface is limited to existing model/registry objects and PointInTimeTables are defined as a deterministic rejection case.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment