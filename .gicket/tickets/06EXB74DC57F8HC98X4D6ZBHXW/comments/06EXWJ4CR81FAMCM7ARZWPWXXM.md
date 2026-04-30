[gicket-bot] PO refinement contract

Summary
- Refined epic 06EXB74DC57F8HC98X4D6ZBHXW using verified ticket state, existing child relations, repository layout, modeling source evidence, tests, and referenced planning contracts; no new child tickets or planning documents were created in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This epic is a planning parent for provider-neutral Data Vault 2.x modeling core work, not a request to deliver all modeling features in one implementation ticket.
- Existing parentOf relations already split the epic into three child tickets: 06EXB74NRVRX18GD33CH1C12SW, 06EXB75DX3YAJFMJ6TNHVPAWYG, and 06EXB765S2X2MR2K18ZBV8RC38.
- The v1 concept baseline is hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources, as documented in docs/architecture/mvp-data-vault-concepts.md.
- The current repository baseline is .NET 10 with main library source under src/DCoding.Data.DVault and tests under tests/DCoding.Data.DVault.Tests.
- The repository already contains modeling and hashing foundations including DCoding.Data.DVault.Modeling, DataVaultMetadata, DataVaultModel, DataVaultConventions, technical metadata column contracts, DefaultStableHashService, DefaultStableHashNormalizer, and deterministic unit coverage.

Scope In
- Provider-neutral metadata contracts for Data Vault hubs, links, satellites, technical metadata columns, hash keys, hash diffs, load timestamps, and record sources.
- Deterministic model behavior for naming, concept classification, metadata column requiredness, stable hashing, and ordered/culture-independent normalization.
- Convention-first defaults that work without provider-specific EF behavior or mandatory advanced configuration.
- Tests that prove deterministic behavior across repeated runs, field ordering, culture-sensitive values, and invalid metadata or unsupported hashing inputs.
- Alignment with shared implementation standards, default naming policy, stable hashing contract, optional advanced hook boundaries, and MVP Data Vault concept documentation.

Scope Out
- Provider-specific DDL, EF provider adapters, migrations, physical schema generation, and dialect-specific behavior.
- PIT table generation, bridge table generation, multi-active satellites, provider-specific optimizations, and other deferred Data Vault capabilities.
- Runtime loading automation, ingest orchestration, source catalog integration, and validation tooling beyond core modeling contracts.
- Security hashing, encryption, signatures, MACs, password hashing, key management, or secret rotation.
- Concrete public API names for future advanced configuration hooks unless a child ticket explicitly owns that implementation surface.

Open questions
- none

Follow-up questions
- When the related child tickets are resumed, confirm which of the three existing children owns each remaining slice: metadata shape, deterministic model behavior, and any future EF-facing integration boundary.
- After the MVP modeling core is accepted, decide whether PIT, bridge, multi-active satellite, and provider-optimization capabilities should become separate epics or remain deferred planning notes.
- When advanced hook implementation begins, decide whether the configuration surface is immediately stable public API or experimental for the first implementation pass.
- Provider ecosystem prioritization remains a future planning decision after provider-neutral modeling contracts are accepted.

Risks
- The epic scope is broad; delivery should continue through bounded child tickets rather than reopening this parent as one implementation unit.
- Future provider adapter work could accidentally reinterpret logical names, required metadata fields, or hash semantics unless it explicitly references the provider-neutral contracts.
- Changing stable hash normalization, algorithm identifiers, or default naming semantics after persistence exists would require compatibility planning.

Split recommendations
- Do not create additional child tickets during this PO pass; the epic already has three persisted parentOf children and current evidence supports PO-critic review.
- If later review finds a child still too large, split by bounded responsibility: modeling metadata contracts, deterministic hashing/model identity, and provider-facing integration/adaptation boundaries.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment