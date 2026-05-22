[gicket-bot] PO-critic review contract

Summary
- Ready for dev: the contract is bounded, `## Open Questions` is `none`, repository evidence confirms the relevant DVault boundaries, and branch history shows this is still a metadata-only pre-dev handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F492ARW2N6SNYJH15RHMZEN8/description.md is the authoritative contract, sets `PO Handoff` to `ready_for_po_critic`, and has `## Open Questions` -> `- none`.
- .gicket/relations/QM/N8/06F492A3MPSGP3KXDNZECN01QM--06F492ARW2N6SNYJH15RHMZEN8--parentOf.json and .gicket/relations/N8/VM/06F492ARW2N6SNYJH15RHMZEN8--06F492BNDPWS9P4EDSV0W7G6VM--blocks.json confirm the epic parent and downstream docs blocker relations referenced by the contract.
- src/DCoding.Data.DVault.Analyzers/README.md plus src/DCoding.Data.DVault.Analyzers/CodeFirstDiagnosticCatalog.cs and DataVaultMappingDiagnosticCatalog.cs show the current analyzer inventory is DMV1901, DMV1902, and DMV1950-DMV1955.
- tests/DCoding.Data.DVault.Tests/Analyzers currently contains DataVaultCodeFirstAnalyzerTests.cs and DataVaultMappingSourceGeneratorTests.cs, so the analyzer test surface already exists in-repo.
- src/DCoding.Data.DVault/DataVaultSaveService.cs, src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs, src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs, README.md, and docs/architecture/dvault-ef-compiled-compatibility.md directly evidence the explicit `IDataVaultSaveService` write boundary, opt-in `UseDataVaultSaveChangesMetadataInterceptor(...)`, `UseDataVaultMetadata(...)`/`ApplyDataVaultMetadata(...)`, and safe shared-type read patterns over `Set<Dictionary<string, object>>(...)`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A concrete fire/non-fire pair for `DbSet` exposure versus the documented shared-type read pattern would further tighten the first rule boundary.
- A concrete unsafe direct-write example versus the allowed opt-in `UseDataVaultSaveChangesMetadataInterceptor(...)` lane would help anchor generated-row tracking edge cases.
- A minimal `statically obvious missing metadata registration` example would clarify what counts as unambiguous local source evidence without expanding into whole-app DI inference.

Risky assumptions
- Implementation can identify generated DVault tables and technical metadata from stable symbol/annotation surfaces instead of brittle produced-name heuristics alone.
- `Obviously unsafe direct generated-table write` can be detected locally with low false-positive risk and without sliding into the runtime guard space already split to ticket `06F492AYE4A3PKA2D20DDPQ37C`.
- Safe shared-type read patterns must stay exempt across ordinary LINQ, `AsNoTracking()`, and compiled-query call sites, not just one documented example.

AC / test suggestions
- Assert non-find coverage for the safe patterns documented in README.md and docs/architecture/dvault-ef-compiled-compatibility.md, including `AsNoTracking()` and `EF.CompileQuery` over `Set<Dictionary<string, object>>(...)`.
- Assert non-find coverage around the explicit `IDataVaultSaveService` path and the opt-in `UseDataVaultSaveChangesMetadataInterceptor(...)` lane called out in the contract.
- Assert that each new DMV diagnostic ships stable ID/message/remediation text and that only mechanical fixes receive code-fix coverage.

Implementation watchouts
- Keep the analyzer slice local and high-confidence: the contract excludes runtime blocking, preflight/drift/query-shape diagnostics, and cross-project DI inference.
- Any new EF-misuse IDs must fit the existing analyzer catalog/README/test conventions without colliding with shipped diagnostics DMV1901/1902 and DMV1950-DMV1955.
- Because `develop..HEAD` is metadata-only, implementation still starts from the current analyzer baseline; both analyzer logic and analyzer tests remain to be added.

Non-blocking notes
- The current ticket state (`todo`, no assignees, `critic-needed`) is normal for this PO-critic gate and does not indicate a refinement gap.
- The sibling-ticket split is coherent: runtime guard, query-shape, preflight, drift, and documentation work are already separated, so this story can stay focused on compile-time analyzer misuse detection.

Split recommendations
- No split recommended at PO gate; the current sibling tickets already isolate runtime guard, query-shape, preflight, drift, and docs work from this analyzer slice.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment