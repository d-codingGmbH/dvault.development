<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement closes the PO-critic gaps by defining the per-entry documentation contract and by making the v1 seed set explicit as the 18 importer/projection diagnostics DMV1001-DMV1801, leaving no blocking PO questions.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- V1 seed scope is exactly the currently emitted model-artifact importer/projection diagnostics from DataVaultModelArtifactParser and DataVaultModelImportResult: DMV1001, DMV1002, DMV1101, DMV1102, DMV1103, DMV1201, DMV1202, DMV1203, DMV1301, DMV1302, DMV1303, DMV1401, DMV1501, DMV1502, DMV1601, DMV1602, DMV1701, and DMV1801.
- Each catalog entry documentation contract lives on the catalog definition object in the core package, not only in XML docs, comments, or a separate ad hoc list.
- Required per-entry documentation fields are a short summary/title, an explanation of when the diagnostic is raised, and remediation guidance, alongside the stable identity fields code, severity, and category.
- JSON pointer and logical source path remain per-emitted-diagnostic context and must be preserved by regression tests rather than stored as static catalog-entry documentation.
- No child tickets, relation changes, or planning documents were materialized in this refinement pass.

### Scope In
- Add a core-package diagnostic definition model and a central deterministic diagnostic catalog.
- Seed the catalog with the exact 18 model-artifact importer/projection diagnostics listed in this contract.
- Refactor the model-artifact importer/projection diagnostic path to resolve those seeded definitions from the catalog instead of scattering the stable metadata inline.
- Add focused tests for exact seed-set completeness, id uniqueness, required documentation fields, and unchanged importer/projection observable behavior.

### Scope Out
- Diagnostics outside the parser/import-result path, including diagnostics-service analysis, code-first validation, and provider save/read strategy diagnostics.
- Analyzer package, CLI surface, or consumer-facing published diagnostic catalog artifact.
- Broad migration of all repository diagnostic emitters in this ticket.

## Acceptance Criteria
- A core-package diagnostic definition model and central catalog exist and expose deterministic discovery of the v1 catalog in ascending code order.
- The v1 catalog contains exactly these seeded diagnostics and no others: DMV1001, DMV1002, DMV1101, DMV1102, DMV1103, DMV1201, DMV1202, DMV1203, DMV1301, DMV1302, DMV1303, DMV1401, DMV1501, DMV1502, DMV1601, DMV1602, DMV1701, and DMV1801.
- Every catalog entry stores its stable identity metadata code, severity, and category plus its required documentation fields summary/title, explanation, and remediation on the definition itself.
- Focused unit tests fail if any v1 catalog entry is missing or blank required documentation fields, if any code is duplicated, if any seeded entry's severity/category drift from the current shipped importer/projection baseline, or if the discovered seed set differs from the approved 18-code list.
- The wired importer/projection path resolves those diagnostics through the catalog without changing the currently observed codes, categories, JSON pointers, or logical source paths asserted by existing tests.

## Definition of Done
- All 18 seeded definitions are implemented in the core-package catalog and discoverable deterministically.
- The parser/importer projection path uses catalog-backed definitions end to end for the approved 18-code seed set.
- New catalog tests cover exact seed-set completeness, unique ids, and the required per-entry documentation contract.
- Existing importer/projection regression tests continue to pass for representative DMV1002 and DMV1801 behavior around category, JSON pointer, and logical source path.
- No unrelated diagnostics families are migrated in this ticket.

## Implementation Notes
- Use DataVaultModelArtifactParser.cs and DataVaultModelImportResult.cs as the authoritative v1 seed sources, including the current code/category/severity baseline and the full 18-code scope.
- The catalog owns stable definition metadata and the required documentation fields; JSON pointer and logical source path remain emitted-instance data.
- Keep all existing diagnostic codes and categories stable; this ticket centralizes definitions, it does not rename the shipped diagnostics.
- Make catalog discovery deterministic by exposing the seed set in the same fixed ascending code order listed in this contract.
- Diagnostics outside those two source files should remain follow-up work rather than being pulled into this first catalog slice.

## Open Questions
- none

## Follow-Up Questions
- After this 18-code importer/projection slice lands, should a follow-up ticket migrate diagnostics-service analysis, code-first validation, and provider strategy diagnostics into the same catalog?
- Once the internal catalog shape is proven, should a separate documentation ticket publish the supported diagnostic list for external consumers?

## Risks
- Centralizing 18 existing emitters may touch multiple call sites, so regression coverage must protect stable observed behavior.
- If later tickets extend the catalog without preserving the same per-entry documentation tests, diagnostic documentation quality could drift.

## Split Recommendations
- No split is recommended; the 18-code importer/projection seed set is already the smallest coherent first slice visible in the repository.
- If future migration expands beyond this seed set, create separate follow-up tickets by diagnostic family instead of enlarging this ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Implement the catalog and focused tests for stable DVault diagnostic definitions.

## Scope In

- Add a diagnostic definition model.
- Test id uniqueness, severity/category validity, and documentation coverage.
- Wire one existing validation path through the catalog.

## Scope Out

- No analyzer package.
- No large validator rewrite.

## Acceptance Criteria

- Catalog definitions are deterministic and centrally discoverable.
- Focused unit tests cover the catalog rules.

## Implementation Notes

- Keep the first slice small and reusable.

## Open Questions

- none