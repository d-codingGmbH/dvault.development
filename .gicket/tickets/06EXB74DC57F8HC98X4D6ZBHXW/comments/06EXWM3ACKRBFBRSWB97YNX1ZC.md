[gicket-bot] PO refinement contract

Summary
- Refined the epic contract to address the PO-critic formatting-gate blocker. The contract now treats tools/check-format.sh as a known broken repository prerequisite and does not require developers on this modeling epic to satisfy that gate until the script_repo_root defect is restored by a separate tooling fix.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The ticket-level contract is updated by replacing the unconditional formatting-gate DoD with an explicit prerequisite: tools/check-format.sh is currently blocked by the repository defect where script_repo_root is undefined, so this epic requires dotnet test for the touched modeling/hashing scope plus normal formatting policy adherence, while executable formatting-gate enforcement remains dependent on restoring tools/check-format.sh in a separate tooling/governance fix.
- critic-item-2: `answered` - The contract now acknowledges that the formatting gate is non-actionable while tools/check-format.sh fails before checking governed files because script_repo_root is undefined. The executable validation path for this epic is dotnet test through the repository solution for touched modeling and hashing scope where the local net10.0 SDK is available; formatting remains governed by docs/formatting.md, .editorconfig, and .gitattributes, but the shell gate itself is a separate blocker until repaired.

Clarifications
- This epic remains a planning parent for provider-neutral Data Vault 2.x modeling core work, not a request to deliver all modeling features in one implementation ticket.
- Existing parentOf relations already split the epic into three child tickets: 06EXB74NRVRX18GD33CH1C12SW, 06EXB75DX3YAJFMJ6TNHVPAWYG, and 06EXB765S2X2MR2K18ZBV8RC38.
- The v1 concept baseline is hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources, as documented in docs/architecture/mvp-data-vault-concepts.md.
- The current repository baseline is .NET 10 with main library source under src/DCoding.Data.DVault and tests under tests/DCoding.Data.DVault.Tests.
- The repository already contains modeling and hashing foundations including DCoding.Data.DVault.Modeling, DataVaultMetadata, DataVaultModel, DataVaultConventions, technical metadata column contracts, DefaultStableHashService, DefaultStableHashNormalizer, and deterministic unit coverage.
- tools/check-format.sh is currently a broken repository prerequisite because it references script_repo_root without defining it; this ticket records that blocker instead of requiring developers to satisfy a non-executable gate.

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
- Repairing tools/check-format.sh is outside this modeling epic except as an explicitly recorded prerequisite for restoring the repository-level formatting gate.

Open questions
- none

Follow-up questions
- When the related child tickets are resumed, confirm which of the three existing children owns each remaining slice: metadata shape, deterministic model behavior, and any future EF-facing integration boundary.
- After the MVP modeling core is accepted, decide whether PIT, bridge, multi-active satellite, and provider-optimization capabilities should become separate epics or remain deferred planning notes.
- When advanced hook implementation begins, decide whether the configuration surface is immediately stable public API or experimental for the first implementation pass.
- Provider ecosystem prioritization remains a future planning decision after provider-neutral modeling contracts are accepted.
- Create or route a separate tooling/governance ticket to repair tools/check-format.sh so it defines the script repository root before invoking git -C and restores the canonical formatting gate from shared implementation standards.

Risks
- The epic scope is broad; delivery should continue through bounded child tickets rather than reopening this parent as one implementation unit.
- Future provider adapter work could accidentally reinterpret logical names, required metadata fields, or hash semantics unless it explicitly references the provider-neutral contracts.
- Changing stable hash normalization, algorithm identifiers, or default naming semantics after persistence exists would require compatibility planning.
- Until tools/check-format.sh is fixed, repository-level formatting enforcement cannot be fully automated even though formatting policy remains normative.

Split recommendations
- Do not create additional child tickets during this PO pass; the epic already has three persisted parentOf children and current evidence supports PO-critic review after the formatting-gate prerequisite is explicitly recorded.
- If later review finds a child still too large, split by bounded responsibility: modeling metadata contracts, deterministic hashing/model identity, and provider-facing integration/adaptation boundaries.
- Treat restoration of tools/check-format.sh as a separate tooling/governance follow-up rather than a child split of this modeling-core epic.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 6
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment