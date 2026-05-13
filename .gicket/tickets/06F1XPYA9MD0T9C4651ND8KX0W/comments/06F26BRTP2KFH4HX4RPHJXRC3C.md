[gicket-bot] PO-critic review contract

Summary
- Ticket 06F1XPYA9MD0T9C4651ND8KX0W has a persisted, bounded delivery contract with no unresolved open questions. The scope is clear enough for developer handoff: use existing SQLite/local integration coverage for compiled EF paths, add or keep the compatibility tests, document supported patterns and limitations, and avoid unsupported provider/tooling/performance expansion.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F1XPYA9MD0T9C4651ND8KX0W/description.md contains the gicket-bot human refinement contract with PO Handoff decision ready_for_po_critic and ## Open Questions set to none.
- Current branch is ticket/06F1XPYA9MD0T9C4651ND8KX0W-story-prove-compiled-model-and-compiled-query-co at a1ced89561b2e619b19a4d4e488709331193157a; git log shows PO handoff commits df603294e7f1, 5d13a3c4a, and po-critic claim a1ced8956 after develop e859f5c46.
- git diff --name-status develop...HEAD shows only .gicket ticket/comment/event metadata changes for this story; no src, tests, docs, or benchmark implementation changes are currently pending on the story branch.
- Child task comment 06F268XKEV3CJZQZJK7Z1SYQ18.md records tester verification of 5/5 acceptance criteria and 4/4 DoD for compiled compatibility tests, and comment 06F2690N69GE8DAS0XY79PMWD0.md records integrator decision ACCEPT.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs directly uses EF.CompileQuery, IModelRuntimeInitializer.Initialize, and UseModel, and asserts MetadataSourceKind, EntityKind, MetadataName, ProducedName, PropertyRole, and TechnicalColumnRole annotations.
- tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs includes DataVaultCompiledCompatibilitySqliteTests in RequiredLocalSqliteCoverageTypes; DVault.slnx includes tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj.
- src/DCoding.Data.DVault/DataVaultAnnotationNames.cs defines the annotation names and DataVaultPropertyRole referenced by the contract; src/DCoding.Data.DVault/TechnicalMetadataColumnRole.cs and src/DCoding.Data.DVault/Modeling/DataVaultModel.cs define the technical role and table kind types used by the tests.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The developer should treat the already-integrated child test coverage as reusable evidence, but the story still explicitly requires user-facing documentation or release-note coverage before completion.
- The contract intentionally limits compatibility claims to EF-owned runtime-model UseModel usage and stable direct EF compiled query shapes; dynamic IDataVaultReadService requests and caller-owned projector delegates are not promised compiled-query surfaces.

AC / test suggestions
- Record the exact focused test command/results in the developer handoff or implementation notes, because the DoD requires run evidence.
- When documenting unsupported shapes, name the dynamic request/projector limitation explicitly so the compiled query proof is not overread as a broad read-service guarantee.

Implementation watchouts
- Do not add DVault-owned EF design-time services, dotnet ef commands, provider-specific compiled model generators, or provider-specific compiled query optimizations under this story.
- Docs search found compiled compatibility coverage in tests but not matching user-facing docs yet; developer should update README, docs, or release notes to explain the supported boundary and avoid benchmark claims unless stable artifacts are cited.
- The story branch currently has only .gicket metadata changes relative to develop, so any implementation work should be scoped to remaining documentation/evidence gaps and should not duplicate the done child task unnecessarily.

Non-blocking notes
- Existing repository tests provide direct source evidence that the named public DVault annotation/type surfaces exist and are used in compiled compatibility coverage.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment