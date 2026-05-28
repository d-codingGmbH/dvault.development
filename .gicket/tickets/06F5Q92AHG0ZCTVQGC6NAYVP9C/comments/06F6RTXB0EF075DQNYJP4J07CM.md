[gicket-bot] PO-critic review contract

Summary
- Persisted delivery contract is specific, has no open questions, and repository evidence confirms the existing runtime, metadata, and analyzer-test baseline needed for a clean pre-development handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F5Q92AHG0ZCTVQGC6NAYVP9C/description.md contains the persisted Delivery Contract with `## Open Questions` -> `- none`, 5 acceptance-criteria bullets, and 4 definition-of-done bullets.
- .gicket/tickets/06F5Q92AHG0ZCTVQGC6NAYVP9C/comments/06F6RKCH2Y5D10TXJ1VVB2G674.md records PO handoff `ready_for_po_critic` and repeats the same satellite-only scope and diagnostics boundary.
- git log --oneline on branch ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite shows the handoff commit `bc8250d72` and current HEAD `bec85c92a`; git diff --name-only cbbe81d4e..bec85c92a changes only .gicket/tickets/06F5Q92AHG0ZCTVQGC6NAYVP9C/**, so no src/docs/tests implementation work is expected yet on this pre-dev branch.
- docs/plans/typed-read-model-generator-contract.md names ticket 06F5Q92AHG0ZCTVQGC6NAYVP9C as the satellite slice, fixes the generated naming/method contract (`{SatelliteProducedName}ReadModel`, `{SatelliteProducedName}ReadExtensions`, `Read...CurrentAsync`, `Read...LatestAsync`, `Read...AsOfAsync`), and reserves diagnostics DMV1960-DMV1969.
- src/DCoding.Data.DVault/IDataVaultReadService.cs, DataVaultLatestSatelliteReadRequest.cs, DataVaultReadServiceCurrentSatelliteExtensions.cs, DataVaultReadServiceRegistryExtensions.cs, DataVaultReadServiceTypedProjectionExtensions.cs, and DataVaultSatelliteProjectionRow.cs directly show the existing latest/current/as-of satellite read and typed-projection surface the story is supposed to wrap.
- src/DCoding.Data.DVault/DataVaultAnnotationNames.cs defines ProducedName, MetadataName, ParentReferenceKind, ParentReferenceName, Ordinal, PropertyRole, ProviderLogicalPropertyKind, MetadataSourceKind, and MetadataSourceFingerprint annotations that the delivery contract explicitly depends on.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs and src/DCoding.Data.DVault/DataVaultCodeFirstLinkBuilder.cs directly show hub-parent or link-parent satellite metadata plus multi-active driving-key support, matching the ticket scope.
- src/DCoding.Data.DVault.Analyzers/DataVaultMappingSourceGenerator.cs, src/DCoding.Data.DVault.Analyzers/DataVaultMappingDiagnosticCatalog.cs, tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultMappingSourceGeneratorTests.cs, and tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultCodeFirstAnalyzerTests.cs show the existing analyzer/source-generator package and Roslyn test harness named in the Definition of Done.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The optional stable direct EF projection path stays inside the compiled-model and compiled-query compatibility boundary referenced by the contract and is not used to justify provider-specific SQL or runtime-shaped queries.
- One authoritative metadata source per generated scope can be resolved consistently enough to preserve produced names, source fingerprint, parent reference data, ordinals, CLR types, and nullability across metadata-first, model-first, and code-first inputs.

AC / test suggestions
- Keep dev verification explicitly tied to representative hub-parent, link-parent, and deterministic multi-active satellite cases plus negative DMV196x coverage for stale fingerprints, unsupported shapes, naming collisions, and nullability fallback.
- If later clarification is needed, prefer concrete generated-name and row-binding examples rather than widening scope or adding new runtime behavior.

Implementation watchouts
- Current and Latest are contract-equivalent convenience names over the same no-asOf latest-satellite request path; generated naming must not imply different semantics.
- Link-parent satellite helpers are in scope, but PIT and bridge helpers/diagnostics stay in sibling ticket 06F5Q92R02HB7FCE1AWKXPTMRW.
- The contract requires diagnostics instead of unstable helper emission when authoritative metadata, fingerprints, produced bindings, or normalized public names cannot be resolved deterministically.

Non-blocking notes
- The ticket title mentions latest/as-of, but the persisted contract intentionally includes `Current` as an equivalent convenience wrapper and defines that behavior explicitly.

Split recommendations
- No further split recommended; the persisted contract already isolates satellite generation from the PIT/bridge slice and the repository baseline supports that separation.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment