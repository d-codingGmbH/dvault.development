<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Resolved the PO-critic ambiguity by pinning the exact dual-target project set and by explicitly allowing net8 exclusions for the net10-only benchmark and package-verifier helper slices.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The required dual-target implementation set is exactly the six packable runtime/provider projects plus tests/DCoding.Data.DVault.Tests.Shared, tests/DCoding.Data.DVault.Tests.Unit, and tests/DCoding.Data.DVault.Tests.Integration.
- tests/DCoding.Data.DVault.Tests.Modeling is not a required standalone multitarget project for this story; its source files still need to compile where Unit links them, but the separate Modeling csproj may stay net10-only unless a narrow build-support change is unavoidable.
- benchmarks/DCoding.Data.DVault.Benchmarks and tools/DCoding.Data.DVault.PackageVerification stay out of scope as standalone net8 conversion work for this ticket.
- The allowed net8 boundary is the runtime/provider package line plus the Shared, Unit, and Integration coverage that does not require the benchmark or package-verifier helpers; target-conditioned exclusions are allowed for those helper-dependent slices.
- src/DCoding.Data, src/DCoding.Data.DVault.Analyzers, and tests/DCoding.Data.DVault.Tests/Analyzers remain outside the mandatory dual-target set for this story unless a narrow non-behavioral build fix is required.
- The live blocks relation from done ticket 06F9G8EQJGBRSWE96VE028HJYW remains historical relation residue and does not block this story.

### Scope In
- Dual-targeting src/DCoding.Data.DVault and the five packable provider projects for net8.0 and net10.0.
- Dual-targeting tests/DCoding.Data.DVault.Tests.Shared, tests/DCoding.Data.DVault.Tests.Unit, and tests/DCoding.Data.DVault.Tests.Integration to the extent required for runtime/provider build and test coverage.
- Adding target-conditioned package and project selection so each resolved target uses the intended EF/provider line without mixed restores.
- Adding target-conditioned exclusions for benchmark-dependent and package-verifier-dependent test slices when those helpers would otherwise force the net8 path to depend on net10-only projects.
- Preserving existing package IDs, provider-to-core dependency shape, pack inputs, analyzer asset boundary, and opt-in external-provider execution switches.

### Scope Out
- Standalone net8 conversion of benchmarks/DCoding.Data.DVault.Benchmarks.
- Standalone net8 conversion of tools/DCoding.Data.DVault.PackageVerification and the broader verifier or CI rewiring already owned by 06F9G8FBQTAPXXS1Y4NR5QKVG8.
- Standalone net8 conversion of tests/DCoding.Data.DVault.Tests.Modeling, src/DCoding.Data.DVault.Analyzers, or tests/DCoding.Data.DVault.Tests/Analyzers unless a narrow build-only adjustment is unavoidable.
- New runtime behavior, provider matrix assertions owned by 06F9G8F4RQ0T7RV82M3H2H3FVG, documentation or release-automation scope, or any new package family split.

## Acceptance Criteria
- src/DCoding.Data.DVault/DCoding.Data.DVault.csproj and the five packable provider csproj files target both net8.0 and net10.0, while src/DCoding.Data/DCoding.Data.csproj remains the non-packable source-root anchor.
- tests/DCoding.Data.DVault.Tests/Shared/DCoding.Data.DVault.Tests.Shared.csproj, tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj, and tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj provide the required dual-target runtime/provider validation path for this story.
- tests/DCoding.Data.DVault.Tests/Modeling/DCoding.Data.DVault.Tests.Modeling.csproj, src/DCoding.Data.DVault.Analyzers, tests/DCoding.Data.DVault.Tests/Analyzers, benchmarks/DCoding.Data.DVault.Benchmarks, and tools/DCoding.Data.DVault.PackageVerification are not required standalone net8 conversion targets for this story.
- The net8 path may use target-conditioned ProjectReference and Compile conditions so benchmark-dependent integration coverage, its corresponding discovery assertions, and package-verifier unit coverage stay net10-only, while the remaining runtime/provider-facing Unit and Integration coverage builds under both target frameworks.
- For net8.0, the resolved dependency graph matches the shared 8.33 contract; for net10.0, the resolved dependency graph stays on the 10.33 contract, including Npgsql.EntityFrameworkCore.PostgreSQL 10.0.2 on the net10 line where used.
- Conditional PackageReference logic remains limited to target-framework selection plus the existing opt-in external-provider switches, and no required build, test, or pack target restores both 8.x and 10.x EF/provider lines together.
- Project-level pack inputs still support separate 8.33.0 and 10.33.0 artifact lines with unchanged package IDs and no consumer-facing 0.33.0 package version.

## Definition of Done
- The contract explicitly names the required multitarget project set and the allowed helper-project exclusions, with no remaining PO ambiguity around Shared, Unit, Modeling, Integration, benchmarks, or package verification.
- Developers can build the dual-target runtime/provider package line and the required Shared, Unit, and Integration validation path for both target frameworks without mixed-line restores.
- Net10 benchmark and package-verifier coverage remains intact where it exists today, but those helper projects do not become hidden mandatory net8 scope for this story.
- Sibling tickets for provider matrix tests, verifier and CI guidance, and documentation can proceed without reopening the project-set decision resolved here.

## Implementation Notes
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj currently references benchmarks/DCoding.Data.DVault.Benchmarks, and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs is the visible source that consumes that helper project.
- tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs currently includes BenchmarkScenarioExecutionTests in the required SQLite coverage list, so any benchmark net10-only exclusion must condition its discovery expectations as well as the project reference.
- tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj currently references tools/DCoding.Data.DVault.PackageVerification, and tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs is the visible source that consumes that helper project.
- tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj links ../Modeling/*.cs into Unit, so modeling source compatibility still matters for net8 Unit builds even though the standalone Modeling project is not a required separate dual-target project.
- Preserve the existing external-provider opt-in PackageReference conditions in Integration and compose target-framework conditions on top of them rather than replacing them with unconditional mixed references.

## Open Questions
- none

## Follow-Up Questions
- Should a later build-policy task standardize a named line-selection property or artifact-directory convention for separate 8.33.0 and 10.33.0 pack runs?
- After this story lands, should benchmarks/DCoding.Data.DVault.Benchmarks or tools/DCoding.Data.DVault.PackageVerification themselves gain explicit net8 support, or is keeping them net10-only under later verifier or performance work sufficient?
- After this story lands, should analyzer-host compatibility for non-net10 development environments be revisited explicitly, or is preserving the tooling-only analyzer boundary still sufficient for v0.33?

## Risks
- Conditionally excluding benchmark and package-verifier slices on net8 can drift if future tests add new helper-project dependencies without matching target conditions.
- Because Unit and Integration each mix required runtime/provider coverage with excluded helper slices, incorrect MSBuild conditions can silently reduce net8 coverage or break existing net10 coverage.
- Sibling ticket 06F9G8FBQTAPXXS1Y4NR5QKVG8 still owns verifier and CI rewiring, so repository-wide dual-line validation remains operationally incomplete until that task lands.

## Split Recommendations
- No additional split is required. The helper-project boundary is now explicit, and the remaining verifier follow-up already belongs to 06F9G8FBQTAPXXS1Y4NR5QKVG8 rather than to a new child ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Retarget the packable DVault runtime/provider projects and relevant test projects to support net8.0 and net10.0 in parallel while producing package artifacts whose major version matches the dependency line: 8.33.0 for net8.0/EF Core 8 and 10.33.0 for net10.0/EF Core 10. Add conditional PackageReference or central-package-version handling so each target framework resolves the intended EF provider versions without mixing incompatible assemblies. Preserve current net10.0 behavior, package metadata, and analyzer/source-generator asset expectations.