<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the epic contract to address the PO-critic formatting-gate blocker. The contract now treats tools/check-format.sh as a known broken repository prerequisite and does not require developers on this modeling epic to satisfy that gate until the script_repo_root defect is restored by a separate tooling fix.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This epic remains a planning parent for provider-neutral Data Vault 2.x modeling core work, not a request to deliver all modeling features in one implementation ticket.
- Existing parentOf relations already split the epic into three child tickets: 06EXB74NRVRX18GD33CH1C12SW, 06EXB75DX3YAJFMJ6TNHVPAWYG, and 06EXB765S2X2MR2K18ZBV8RC38.
- The v1 concept baseline is hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources, as documented in docs/architecture/mvp-data-vault-concepts.md.
- The current repository baseline is .NET 10 with main library source under src/DCoding.Data.DVault and tests under tests/DCoding.Data.DVault.Tests.
- The repository already contains modeling and hashing foundations including DCoding.Data.DVault.Modeling, DataVaultMetadata, DataVaultModel, DataVaultConventions, technical metadata column contracts, DefaultStableHashService, DefaultStableHashNormalizer, and deterministic unit coverage.
- tools/check-format.sh is currently a broken repository prerequisite because it references script_repo_root without defining it; this ticket records that blocker instead of requiring developers to satisfy a non-executable gate.

### Scope In
- Provider-neutral metadata contracts for Data Vault hubs, links, satellites, technical metadata columns, hash keys, hash diffs, load timestamps, and record sources.
- Deterministic model behavior for naming, concept classification, metadata column requiredness, stable hashing, and ordered/culture-independent normalization.
- Convention-first defaults that work without provider-specific EF behavior or mandatory advanced configuration.
- Tests that prove deterministic behavior across repeated runs, field ordering, culture-sensitive values, and invalid metadata or unsupported hashing inputs.
- Alignment with shared implementation standards, default naming policy, stable hashing contract, optional advanced hook boundaries, and MVP Data Vault concept documentation.

### Scope Out
- Provider-specific DDL, EF provider adapters, migrations, physical schema generation, and dialect-specific behavior.
- PIT table generation, bridge table generation, multi-active satellites, provider-specific optimizations, and other deferred Data Vault capabilities.
- Runtime loading automation, ingest orchestration, source catalog integration, and validation tooling beyond core modeling contracts.
- Security hashing, encryption, signatures, MACs, password hashing, key management, or secret rotation.
- Concrete public API names for future advanced configuration hooks unless a child ticket explicitly owns that implementation surface.
- Repairing tools/check-format.sh is outside this modeling epic except as an explicitly recorded prerequisite for restoring the repository-level formatting gate.

## Acceptance Criteria
- Core modeling contracts represent hubs, links, satellites, metadata columns, hashing roles, load timestamps, record sources, and historization semantics without requiring a specific EF provider.
- The v1 default path remains convention-first and optionless for ordinary use, with advanced naming, hashing, record-source, timestamp, and provider behavior treated as optional extension boundaries.
- Hashing behavior follows the stable hashing contract: sha256-v1 by default, UTF-8 without BOM, lowercase hexadecimal digest values, invariant normalization, deterministic field ordering, and clear failures for null, invalid, or unsupported inputs.
- Modeling behavior follows the MVP concept baseline: hubs store business identity, links store relationships, satellites store descriptive history, and every vault record carries load timestamp and record source metadata.
- Tests cover deterministic naming, metadata contracts, concept classification, stable hash vectors, culture independence, duplicate/invalid metadata handling, and provider-neutral behavior without relying on provider-specific persistence features.

## Definition of Done
- Implementation and tests remain in the established src/DCoding.Data.DVault and tests/DCoding.Data.DVault.Tests layout and follow shared implementation standards.
- Relevant source-of-truth documents are referenced or followed instead of duplicating policy text: shared implementation standards, MVP Data Vault concepts, default naming policy, stable hashing contract, v1 persistence conventions, and optional advanced hook plan.
- dotnet test through the repository solution succeeds for the touched modeling and hashing scope where the local environment supports the configured net10.0 SDK.
- Changed governed text files follow docs/formatting.md, .editorconfig, and .gitattributes formatting rules; the executable bash tools/check-format.sh gate is a known blocked prerequisite until the script_repo_root defect is fixed outside this modeling epic.
- Once tools/check-format.sh is restored, the non-mutating formatting gate must be run for changed governed text files before treating the repository-level formatting requirement as fully validated.
- No provider-specific behavior, advanced Data Vault capability, or runtime configuration commitment is introduced unless covered by a dedicated child ticket or planning contract.

## Implementation Notes
- Use DCoding.Data.DVault.Modeling as the owning namespace and folder for modeling-core contracts unless a child ticket explicitly owns a namespace change.
- Use the existing DataVaultConventions, DataVaultMetadata, DataVaultModel, DataVaultModelBuilder, DefaultDataVaultNamingPolicy, DefaultNamingPolicy, TechnicalMetadataColumnContract, TechnicalMetadataColumnRole, and TechnicalMetadataColumnRequiredness shapes as repository evidence for the v1 baseline.
- Keep stable hashing reusable through IStableHashService, IStableHashNormalizer, StableHashDigest, DefaultStableHashService, and DefaultStableHashNormalizer; model-specific code should deliberately map fields before hashing rather than serialize arbitrary objects.
- Record source and load timestamp are required lineage metadata for hub, link, and satellite records; logical timestamps should be UTC and provider-neutral, with timestamps excluded from content hashes unless a later payload contract explicitly includes them.
- The formatting-gate blocker is specifically in tools/check-format.sh: with set -u active, the script references script_repo_root before assignment. Developers on this epic should not be asked to prove this broken gate passes until the tooling defect is fixed.

## Open Questions
- none

## Follow-Up Questions
- When the related child tickets are resumed, confirm which of the three existing children owns each remaining slice: metadata shape, deterministic model behavior, and any future EF-facing integration boundary.
- After the MVP modeling core is accepted, decide whether PIT, bridge, multi-active satellite, and provider-optimization capabilities should become separate epics or remain deferred planning notes.
- When advanced hook implementation begins, decide whether the configuration surface is immediately stable public API or experimental for the first implementation pass.
- Provider ecosystem prioritization remains a future planning decision after provider-neutral modeling contracts are accepted.
- Create or route a separate tooling/governance ticket to repair tools/check-format.sh so it defines the script repository root before invoking git -C and restores the canonical formatting gate from shared implementation standards.

## Risks
- The epic scope is broad; delivery should continue through bounded child tickets rather than reopening this parent as one implementation unit.
- Future provider adapter work could accidentally reinterpret logical names, required metadata fields, or hash semantics unless it explicitly references the provider-neutral contracts.
- Changing stable hash normalization, algorithm identifiers, or default naming semantics after persistence exists would require compatibility planning.
- Until tools/check-format.sh is fixed, repository-level formatting enforcement cannot be fully automated even though formatting policy remains normative.

## Split Recommendations
- Do not create additional child tickets during this PO pass; the epic already has three persisted parentOf children and current evidence supports PO-critic review after the formatting-gate prerequisite is explicitly recorded.
- If later review finds a child still too large, split by bounded responsibility: modeling metadata contracts, deterministic hashing/model identity, and provider-facing integration/adaptation boundaries.
- Treat restoration of tools/check-format.sh as a separate tooling/governance follow-up rather than a child split of this modeling-core epic.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Implement provider-neutral Data Vault metadata and behavior contracts.

## Scope
- Represent hubs, links, satellites, metadata columns, hashing, and historization semantics.

## Acceptance Criteria
- Core concepts are usable without depending on a specific EF provider.
- Modeling tests cover deterministic behavior.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.