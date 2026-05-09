[gicket-bot] PO refinement contract

Summary
- Refined the parent registry story to limit code-first compatibility to the existing internal EF translation path, explicitly scope both public point-in-time lookup families, and keep the live three-child split unchanged with no planning writes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now limits code-first compatibility to the existing public EF model path: public code-first APIs remain compatible because they already normalize internally into the DataVaultMetadataModel translation pipeline during model building; this story does not require a new public export or registry-registration API from code-first declarations.
- critic-item-2: `answered` - The contract now names both in-scope public lookup families explicitly: legacy point-in-time tables use DataVaultPointInTimeMetadata and TryGetPointInTimeTable, and PIT projections use DataVaultPitMetadata and TryGetPit.
- critic-item-3: `answered` - Acceptance Criteria and Definition of Done were rechecked and rewritten so dev/test only infer the planned public API surface: AddDVault(...), UseDataVaultMetadata(...), the existing registry lookup APIs, and compatibility with the existing public code-first EF translation path, not a new public code-first export or registry-registration API.
- critic-item-4: `answered` - Resolved the blocking finding by removing the claim that callers can publicly feed the registry directly from code-first declarations. Current public evidence only supports internal normalization during EF translation, so the parent contract now reflects that narrower behavior.
- critic-item-5: `answered` - Resolved the blocking finding by replacing generic point-in-time wording with the concrete public registry families already exposed on DataVaultMetadataRegistry: PointInTimeTables and TryGetPointInTimeTable, plus Pits and TryGetPit.

Clarifications
- DataVaultMetadataRegistry remains the v1 registry baseline; this story should not reopen a second registry abstraction.
- Satellite lookup remains parent-scoped; exact-name lookup covers hubs, links, bridges, legacy point-in-time tables through TryGetPointInTimeTable, and PIT metadata through TryGetPit.
- CLR-type lookup remains opt-in and only succeeds where one explicit, unambiguous DataVaultMetadataClrMapping exists.
- The default registration path remains optionless AddDVault(). Registry-backed projection is the additive path where callers register a DataVaultMetadataModel or prebuilt DataVaultMetadataRegistry during service setup and opt contexts in with UseDataVaultMetadata().
- Existing public code-first declarations stay in scope only through their current EF model-building path; this story does not add a public code-first-to-DataVaultMetadataModel or code-first-to-registry export API.
- Live parentOf relations to 06F0MEAXT99V0P115P0WEJD4P0, 06F0MEB634X6CTBZ00W108G3FG, and 06F0MEBFTW8FY5T7PY5HJ5JXJ4 remain unchanged; no child tickets, relations, attachments, or planning documents were created in this pass.

Scope In
- Immutable DataVaultMetadataRegistry creation and deterministic lookup over hubs, links, satellites, bridges, DataVaultPointInTimeMetadata, DataVaultPitMetadata, and provider capability profiles.
- DI and EF integration that lets AddDVault(...) register one authoritative DataVaultMetadataModel or prebuilt DataVaultMetadataRegistry and lets UseDataVaultMetadata() consume it with explicit context-level overrides.
- Reuse of the existing provider-neutral DataVaultMetadataModel translation pipeline across metadata-first registration and the existing public code-first EF model path.
- Actionable validation and diagnostics for duplicate logical names, missing metadata dependencies, conflicting metadata sources, and ambiguous or absent CLR-based lookups.

Scope Out
- A new public code-first export or registration API that produces DataVaultMetadataModel or DataVaultMetadataRegistry outside the current EF model-building path.
- Model-first file import-export, external serialization formats, or repository-to-registry tooling.
- Runtime mutation of registry contents after service-provider build.
- New provider-specific SQL, save-service semantics, read-service behavior, PointInTimeTables or PIT refresh behavior, or bridge maintenance behavior beyond consuming the registry as authoritative metadata.

Open questions
- none

Follow-up questions
- If the team later wants app-startup registration directly from code-first declarations, should that be a separate public export or registration ticket rather than expanding this parent beyond the current EF model-building path?
- After registry and model-first work settles, should the older PointInTimeTables naming be publicly deprecated in favor of Pits, or should both lookup families remain first-class long-term?

Risks
- If app-level registry defaults and explicit context overrides are not conflict-checked consistently, different workflows can project different metadata from the same DbContext model.
- If CLR lookup falls back to first-match or registration-order behavior, the registry loses the deterministic semantics this story is meant to centralize.
- Because both PointInTimeTables and Pits are publicly exposed, docs, examples, and tests must keep the two lookup families explicit or consumers may assume one supersedes the other.
- Because bridges and both point-in-time families are representable, downstream consumers may over-assume runtime support unless docs and diagnostics keep the deferred-capability boundary explicit.

Split recommendations
- Keep the existing parentOf split to 06F0MEAXT99V0P115P0WEJD4P0, 06F0MEB634X6CTBZ00W108G3FG, and 06F0MEBFTW8FY5T7PY5HJ5JXJ4; current evidence does not justify new child tickets or relation changes.
- Keep broader code-first parity breadth on 06F0MEAD1BAA5QEVM3F9QJA38G rather than folding that regression matrix back into this parent.
- If public code-first export or registration is desired later, split it into a dedicated follow-up instead of expanding this story.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment