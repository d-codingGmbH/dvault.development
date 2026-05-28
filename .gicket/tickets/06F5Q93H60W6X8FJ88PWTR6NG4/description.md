<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Restated the docs-only v0.22.0 contract against visible generator, support-bundle, public-API-snapshot, stable-hash, and validation evidence; no child tickets, relation changes, attachments, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The authoritative typed read-model evidence is the analyzer README, DataVaultTypedReadModelSourceGenerator.cs, and DataVaultTypedReadModelSourceGeneratorTests.cs; the contract should cite those files rather than inferred analyzer-package API or generator snapshot surfaces.
- The current generator is opt-in via DVaultGenerateTypedReadModels=true, accepts exactly one authoritative dvault.support-bundle.v1 additional file, and enforces metadata-source fingerprint drift through the visible generator property surface.
- The generated-helper boundary is satellite-only: hub-parent, link-parent, and deterministic multi-active satellites emit Read...CurrentAsync, Read...LatestAsync, and Read...AsOfAsync; PIT and bridge shapes remain diagnostic or runtime-service territory and do not emit typed helpers.
- dvault.model.v1 and dvault.support-bundle.v1 are workflow artifact contracts, not checked-in repo-root files on this branch; the branch snapshot shows both root files missing, so docs must describe consumer-owned artifact production and import rather than a repository baseline file.
- The public API approval surface that can be linked today is limited to docs/quality/api-surface-snapshots.md, tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs, and the six approved files for DCoding.Data.DVault, Sqlite, Postgres, SqlServer, Oracle, and MySql.
- No bounded writes were applied in this refinement run; no child tickets, relation updates, attachments, or planning documents were needed.

### Scope In
- Update README with source-backed typed satellite helper wording, support-bundle workflow, fingerprint governance, stable-hash references, and dynamic and compiled-query alternatives.
- Update the analyzer README to match visible source-generator behavior and DMV1960 through DMV1969 outcomes without implying raw model parsing or unsupported helper shapes.
- Update model-first governance and production checklist docs to describe the consumer-owned dvault.model.v1 to projected metadata to dvault.support-bundle.v1 workflow and manual artifact boundary.
- Add docs/releases/v0.22.0.md as a new docs-only release note that links only branch-visible evidence surfaces.
- Align all targeted docs to a v0.22.0 documentation baseline without implying package publication or automatic approval generation.

### Scope Out
- No runtime, analyzer, or source-generator code changes.
- No new public types, public APIs, or API snapshot infrastructure.
- No analyzer-package public API snapshot claim unless such a snapshot is added in a separate ticket.
- No dedicated generator approval-snapshot files or snapshot harness in this ticket.
- No PIT or bridge typed helper implementation or docs that imply shipped typed helpers for those shapes.
- No standalone DVault CLI, automatic support-bundle routing, or automatic package-publication claims.

## Acceptance Criteria
- README documents opt-in typed read-model generation with DVaultGenerateTypedReadModels=true, the consumer-owned support-bundle command and artifact workflow, and the existing alternatives of dynamic IDataVaultReadService requests and consumer-owned compiled EF queries.
- src/DCoding.Data.DVault.Analyzers/README.md states the visible generator boundary exactly: one authoritative dvault.support-bundle.v1 additional file, satellite-only current, latest, and as-of helpers, fingerprint drift handling, and DMV1960 through DMV1969 outcomes for missing, stale, unsupported, collision, nullability-fallback, and skipped-helper cases.
- docs/model-first-governance.md and docs/production-adoption-checklist.md route readers through the reviewed dvault.model.v1 artifact, projected EF and DVault metadata, and consumer-invoked dvault.support-bundle.v1 export flow without implying repo-checked baseline files or a standalone CLI.
- docs/releases/v0.22.0.md is created and links only existing evidence surfaces: docs/quality/api-surface-snapshots.md, tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs, tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/*.approved.txt, tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs, docs/plans/stable-hashing-contract.md, tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs, docs/architecture/dvault-ef-compiled-compatibility.md, and README local validation commands.
- Across the targeted docs, the wording consistently states that the current generator does not parse raw dvault.model.v1 additional files directly, does not emit PIT or bridge helpers, does not generate provider-specific SQL or dynamic-request compilation, and does not rely on non-existent analyzer or generator approval snapshots.

## Definition of Done
- The targeted docs and new docs/releases/v0.22.0.md resolve to existing repository paths, commands, and evidence surfaces that are visible in the current branch.
- The docs tell one consistent v0.22.0 story for support-bundle-driven typed satellite helpers and sha256-v1 compatibility without reintroducing unsourced API or snapshot claims.
- Public API references are limited to the committed core and provider snapshot surface that exists today.
- Generator evidence references are limited to the source-generator implementation and test surface that exists today.
- The docs keep package publication, support-bundle transport, and approval-snapshot generation explicitly manual and consumer-owned.

## Implementation Notes
- Use README.md as the consumer-facing hub because it already shows DVaultGenerateTypedReadModels=true, the consumer support-bundle command, IDataVaultReadService dynamic read APIs, compiled-query guidance, and README local validation commands.
- Use src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs, src/DCoding.Data.DVault.Analyzers/README.md, and tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs as the authoritative typed-helper evidence. The visible source defines build_property.DVaultGenerateTypedReadModels, build_property.DVaultTypedReadModelMetadataSourceFingerprint, and dvault.support-bundle.v1, and the tests prove satellite helper emission plus DMV1960, DMV1961, DMV1963, DMV1964, DMV1966, DMV1967, DMV1968, and DMV1969 behavior.
- Use docs/quality/api-surface-snapshots.md, tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs, and the six tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/*.approved.txt files for public API snapshot references. Do not add analyzer-package or generator approval-snapshot wording unless new evidence is added in a separate ticket.
- Use docs/plans/stable-hashing-contract.md and tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs for the sha256-v1 compatibility story and published vector evidence.
- Use docs/architecture/dvault-ef-compiled-compatibility.md beside README typed-read guidance when documenting consumer-owned compiled EF query alternatives outside the generated satellite-helper boundary.
- The current branch has no repo-root dvault.model.v1, no repo-root dvault.support-bundle.v1, and no docs/releases/v0.22.0.md; treat those as workflow and release-note outputs to be documented or created by the ticket, not as pre-existing committed baseline files.

## Open Questions
- none

## Follow-Up Questions
- If the team later adds dedicated analyzer-package API approval or generator approval-snapshot infrastructure, should that ship as a separate quality or evidence ticket rather than be implied by docs text?
- When PIT or bridge typed-helper implementation actually lands, should a later ticket widen the public docs and release-note evidence beyond the current satellite-only boundary?

## Risks
- The main PO risk is reintroducing unsupported evidence claims, especially analyzer-package public API snapshots or dedicated generator approval snapshots that are not present in the branch.
- Docs can become misleading if they imply repo-checked dvault.model.v1 or dvault.support-bundle.v1 baseline files instead of the visible consumer-owned artifact workflow.
- The generator boundary can be overstated if PIT or bridge helper emission, provider-specific SQL generation, or dynamic-request compilation is described as current behavior.
- If the new v0.22.0 note cites commands or evidence files outside the current repo surfaces, the docs will drift from the actual validation and approval baseline.

## Split Recommendations
- If the team wants new analyzer or generator snapshot infrastructure, split that into a separate quality or evidence ticket.
- If release documentation needs PIT or bridge typed-helper coverage later, split that follow-up to the ticket that ships the actual implementation.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Update docs for typed read model generation and hash governance.

Acceptance criteria:
- README, analyzer README, model-first docs, production checklist, and release notes describe generated read helpers and hash compatibility guidance.
- Links public API snapshots, generator snapshots, compatibility vectors, and validation commands.
- Keeps dynamic read requests and consumer-owned compiled queries documented as alternatives.