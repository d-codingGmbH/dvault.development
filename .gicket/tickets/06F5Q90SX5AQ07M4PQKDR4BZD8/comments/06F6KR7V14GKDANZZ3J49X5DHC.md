[gicket-bot] PO refinement contract

Summary
- The current delivery contract already resolves the PO-critic ambiguity: link-parent PIT support is runtime-path-only, public model-first `dvault.model.v1` PIT declarations/import-export/diagnostics remain out of scope, and the documentation boundary is explicit. No additional child-ticket, relation, attachment, planning-document, or description writes were required in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now explicitly keeps model-first `dvault.model.v1` PIT declarations, import/export, and diagnostics out of scope; this story only expands link-parent PIT support on the existing `DataVaultPitMetadata` / registry-backed runtime path.
- critic-item-2: `answered` - The scope-out and documentation language now explicitly say link-parent PIT support in this story applies only to the existing `DataVaultPitMetadata` / registry-backed runtime path, while model-first `dvault.model.v1` PIT artifacts remain hub-parent-only.
- critic-item-3: `answered` - Not applicable because model-first PIT support is explicitly out of scope; the contract resolves the decision by exclusion rather than adding model-first public-surface acceptance criteria.
- critic-item-4: `answered` - The developer handoff boundary is now explicit: link-parent PIT support is limited to the existing runtime `DataVaultPitMetadata` / registry-backed path, and public model-first PIT declarations remain hub-parent-only.
- critic-item-5: `answered` - Documentation work is now bounded so README, production guidance, deferred-capability text, and release-note language must describe link-parent PIT support only for the `DataVaultPitMetadata` / registry-backed runtime path and must not imply broader `dvault.model.v1` support.

Clarifications
- The repository already supports link-parent satellite modeling and `DataVaultMetadataRegistry` accepts `DataVaultPitMetadata` whose parent is a `Link`, so this story extends an existing runtime path rather than introducing a new PIT metadata type or declaration path.
- Preserve the existing PIT row/read contract names `ParentHashKey`, `LoadTimestamp`, and ordered `<Satellite>LoadTimestamp` snapshot columns; for link-parent PITs `ParentHashKey` carries the link hash key.
- Link-parent PIT support is bounded to one declared link parent plus ordered unique non-multi-active satellites attached to that same link; hub-attached, mixed-parent, bridge-driven, and multi-active PIT shapes remain unsupported here.
- PIT reads stay on the explicit-metadata `DataVaultPitAsOfReadRequest` / `ReadPitRowsAsync(...)` / `ReadPitAsync(...)` boundary; this story does not add registry-backed PIT read requests.
- Model-first `dvault.model.v1` PIT artifacts remain hub-parent-only for this ticket: the public JSON shape, import/export, and drift/diagnostic surfaces do not gain link-parent PIT support here.
- The live incoming `blocks` relation from done ticket `06F5Q90KC6JGQPSP285XQYSPK8` is historical sequencing context rather than an active scope blocker; no relation cleanup was applied in this pass.
- No child-ticket, relation, attachment, planning-document, or further description writes were applied in this pass because the current delivery contract already reflects the required model-first scope decision.

Scope In
- Extend PIT EF metadata translation to project one link-parent `DataVaultPitMetadata` that already reaches the existing EF/registry runtime path, with deterministic PIT table metadata and snapshot columns for attached link-parent satellites.
- Extend explicit and registry-backed PIT maintenance so rebuild and targeted parent maintenance accept the supported link-parent PIT shape and recompute history for explicit link hash keys.
- Extend provider-neutral PIT-backed reads and required diagnostics so explicit `DataVaultPitAsOfReadRequest` callers can read maintained link-parent PIT rows without changing hub-parent PIT behavior or projection semantics.
- Add unit, SQLite integration, public contract snapshot, and documentation coverage for the supported link-parent runtime baseline on the existing `DataVaultPitMetadata` / registry-backed path.

Scope Out
- Multi-active PIT semantics, driving-key PIT row generation, or link-parent PITs that reference multi-active satellites.
- PITs that mix hub-parent and link-parent satellites, traverse bridges, or introduce a new PIT metadata/declaration surface.
- Model-first `dvault.model.v1` PIT declaration changes, including JSON import/export, drift/diagnostic, or other artifact-contract updates required to express link-parent PIT parents or link-parent satellite membership.
- Registry-backed PIT read request surfaces, automatic PIT refresh, background scheduling, `SaveChanges` hooks, or PIT/bridge orchestration.
- Provider-specific link-parent PIT read optimization, physical tuning promises, or broader benchmark/evidence work already deferred to downstream diagnostics tickets.

Open questions
- none

Follow-up questions
- Should a later ticket extend SQLite or other provider-specific PIT read strategies to accept the new link-parent baseline instead of declining to provider-neutral fallback?
- After link-parent PIT support lands, should PIT reads remain explicit-metadata-only, or is a separate registry-backed PIT as-of read request worth planning?
- Should a separate future ticket extend `dvault.model.v1` PIT declarations/import/export/drift diagnostics from the current hub-parent artifact contract to link-parent PIT artifacts?

Risks
- README, production-adoption guidance, deferred-capabilities planning text, and existing release notes currently describe link-parent PITs as unsupported; partial doc updates would create public contract drift.
- Because this story intentionally broadens the runtime `DataVaultPitMetadata` path without broadening the current model-first PIT artifact contract, incomplete docs could imply `dvault.model.v1` link-parent PIT support that import/export/diagnostics still do not provide.
- The current codebase has separate hub-only guards in PIT translation, maintenance validation, read validation, and strategy diagnostics, so updating only one path would leave inconsistent behavior or regress hub-parent compatibility.
- Downstream diagnostics/benchmark work already depends on this story, so incomplete link-parent validation or missing regression coverage would delay later PIT evidence tickets.

Split recommendations
- No additional split is required for the runtime story. If product direction later requires model-first link-parent PIT artifacts, plan that as a separate additive ticket across `dvault.model.v1` JSON, import/export, and drift/diagnostic surfaces.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment