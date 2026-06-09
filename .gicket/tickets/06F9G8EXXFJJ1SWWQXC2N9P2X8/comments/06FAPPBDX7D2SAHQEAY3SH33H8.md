[gicket-bot] PO refinement contract

Summary
- Resolved the PO-critic ambiguity by pinning the exact dual-target project set and by explicitly allowing net8 exclusions for the net10-only benchmark and package-verifier helper slices.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The exact dual-target set for this story is src/DCoding.Data.DVault, src/DCoding.Data.DVault.MySql, src/DCoding.Data.DVault.Oracle, src/DCoding.Data.DVault.Postgres, src/DCoding.Data.DVault.Sqlite, src/DCoding.Data.DVault.SqlServer, plus tests/DCoding.Data.DVault.Tests.Shared, tests/DCoding.Data.DVault.Tests.Unit, and tests/DCoding.Data.DVault.Tests.Integration. The standalone tests/DCoding.Data.DVault.Tests.Modeling project is not required to multi-target in this story, although its linked source files must still compile when Unit builds both targets.
- critic-item-2: `answered` - The net10-only helper projects stay out of scope as standalone conversion work. benchmarks/DCoding.Data.DVault.Benchmarks and tools/DCoding.Data.DVault.PackageVerification may remain net10-only, and this story explicitly allows target-conditioned ProjectReference and source-file inclusion or exclusion so the net8 path does not depend on those helpers while current net10 benchmark and verifier coverage stays intact.
- critic-item-3: `answered` - The required net8 boundary is the dual-target runtime/provider packages plus the dual-target portions of Shared, Unit, and Integration. Net8 does not need a standalone build of benchmarks/DCoding.Data.DVault.Benchmarks or tools/DCoding.Data.DVault.PackageVerification, and it may exclude benchmark-dependent integration coverage, its matching discovery assertions, and PackageVerifierTests through target conditions as long as the remaining runtime/provider-facing coverage still builds against net8.
- critic-item-4: `answered` - The helper-project dependency gap is closed by contract rather than by hidden scope growth: Integration currently reaches benchmarks/DCoding.Data.DVault.Benchmarks and Unit currently reaches tools/DCoding.Data.DVault.PackageVerification, but those helper slices are now explicitly outside the required net8 path. Shared, Unit, and Integration remain in scope only for the runtime/provider-facing coverage that can build after those helper-specific exclusions.
- critic-item-5: `answered` - The Shared, Unit, and Integration projects are in scope for the dual-target path. The separate Modeling project is not a required standalone multitarget project for this story, although the Modeling sources linked into Unit must remain compatible with both targets. That removes the prior ambiguity about whether all four test-project buckets had to multi-target.

Clarifications
- The required dual-target implementation set is exactly the six packable runtime/provider projects plus tests/DCoding.Data.DVault.Tests.Shared, tests/DCoding.Data.DVault.Tests.Unit, and tests/DCoding.Data.DVault.Tests.Integration.
- tests/DCoding.Data.DVault.Tests.Modeling is not a required standalone multitarget project for this story; its source files still need to compile where Unit links them, but the separate Modeling csproj may stay net10-only unless a narrow build-support change is unavoidable.
- benchmarks/DCoding.Data.DVault.Benchmarks and tools/DCoding.Data.DVault.PackageVerification stay out of scope as standalone net8 conversion work for this ticket.
- The allowed net8 boundary is the runtime/provider package line plus the Shared, Unit, and Integration coverage that does not require the benchmark or package-verifier helpers; target-conditioned exclusions are allowed for those helper-dependent slices.
- src/DCoding.Data, src/DCoding.Data.DVault.Analyzers, and tests/DCoding.Data.DVault.Tests/Analyzers remain outside the mandatory dual-target set for this story unless a narrow non-behavioral build fix is required.
- The live blocks relation from done ticket 06F9G8EQJGBRSWE96VE028HJYW remains historical relation residue and does not block this story.

Scope In
- Dual-targeting src/DCoding.Data.DVault and the five packable provider projects for net8.0 and net10.0.
- Dual-targeting tests/DCoding.Data.DVault.Tests.Shared, tests/DCoding.Data.DVault.Tests.Unit, and tests/DCoding.Data.DVault.Tests.Integration to the extent required for runtime/provider build and test coverage.
- Adding target-conditioned package and project selection so each resolved target uses the intended EF/provider line without mixed restores.
- Adding target-conditioned exclusions for benchmark-dependent and package-verifier-dependent test slices when those helpers would otherwise force the net8 path to depend on net10-only projects.
- Preserving existing package IDs, provider-to-core dependency shape, pack inputs, analyzer asset boundary, and opt-in external-provider execution switches.

Scope Out
- Standalone net8 conversion of benchmarks/DCoding.Data.DVault.Benchmarks.
- Standalone net8 conversion of tools/DCoding.Data.DVault.PackageVerification and the broader verifier or CI rewiring already owned by 06F9G8FBQTAPXXS1Y4NR5QKVG8.
- Standalone net8 conversion of tests/DCoding.Data.DVault.Tests.Modeling, src/DCoding.Data.DVault.Analyzers, or tests/DCoding.Data.DVault.Tests/Analyzers unless a narrow build-only adjustment is unavoidable.
- New runtime behavior, provider matrix assertions owned by 06F9G8F4RQ0T7RV82M3H2H3FVG, documentation or release-automation scope, or any new package family split.

Open questions
- none

Follow-up questions
- Should a later build-policy task standardize a named line-selection property or artifact-directory convention for separate 8.33.0 and 10.33.0 pack runs?
- After this story lands, should benchmarks/DCoding.Data.DVault.Benchmarks or tools/DCoding.Data.DVault.PackageVerification themselves gain explicit net8 support, or is keeping them net10-only under later verifier or performance work sufficient?
- After this story lands, should analyzer-host compatibility for non-net10 development environments be revisited explicitly, or is preserving the tooling-only analyzer boundary still sufficient for v0.33?

Risks
- Conditionally excluding benchmark and package-verifier slices on net8 can drift if future tests add new helper-project dependencies without matching target conditions.
- Because Unit and Integration each mix required runtime/provider coverage with excluded helper slices, incorrect MSBuild conditions can silently reduce net8 coverage or break existing net10 coverage.
- Sibling ticket 06F9G8FBQTAPXXS1Y4NR5QKVG8 still owns verifier and CI rewiring, so repository-wide dual-line validation remains operationally incomplete until that task lands.

Split recommendations
- No additional split is required. The helper-project boundary is now explicit, and the remaining verifier follow-up already belongs to 06F9G8FBQTAPXXS1Y4NR5QKVG8 rather than to a new child ticket.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment