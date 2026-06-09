[gicket-bot] PO refinement contract

Summary
- Refined the multitarget story against the completed compatibility contract: the repository is still net10.0-only, this story should retarget the core and provider package projects plus the relevant runtime-facing tests to net8.0/net10.0, and the existing blocks edge from done ticket 06F9G8EQJGBRSWE96VE028HJYW is historical relation residue rather than an active scope blocker.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Ticket 06F9G8EQJGBRSWE96VE028HJYW is done and its delivery contract plus docs/plans/shared-implementation-standards.md already fix the dual package-line architecture: unchanged package IDs, 8.33.0 for net8.0 and EF Core 8, 10.33.0 for net10.0 and EF Core 10, and no mixed-line resolved target.
- Current repository evidence is still single-target net10.0 across the packable runtime/provider csproj files and the Shared, Unit, Modeling, Integration, and Analyzers test projects, so this story is an additive retargeting of existing project files rather than a new compatibility-policy decision.
- The packable implementation surface for this story is DCoding.Data.DVault plus DCoding.Data.DVault.MySql, DCoding.Data.DVault.Oracle, DCoding.Data.DVault.Postgres, DCoding.Data.DVault.Sqlite, and DCoding.Data.DVault.SqlServer; DCoding.Data.DVault.Analyzers remains coordinated tooling whose asset boundary must be preserved, not reopened as a runtime/provider compatibility decision.
- Relevant test retargeting is limited to the runtime/provider-facing test projects that build or execute against those packages; analyzer-only tests stay out unless build compatibility forces a narrow adjustment.
- The live relation graph still contains a blocks edge from done ticket 06F9G8EQJGBRSWE96VE028HJYW into this story; treat that edge as historical workflow residue, not as an active blocker on the PO contract.

Scope In
- Retarget the packable runtime and provider projects to support net8.0 and net10.0 from the same source tree while preserving the existing package IDs.
- Retarget the relevant runtime/provider-facing test projects so the solution can build and validate both compatibility lines without turning opt-in external-provider execution into a default requirement.
- Add the target-framework-conditioned package reference or shared MSBuild version-selection logic needed so each resolved target uses exactly one intended EF/provider line.
- Update the project-level pack inputs needed so separate 8.33.0 and 10.33.0 package-line artifacts can be produced without inventing a consumer-facing 0.33.0 package version.
- Preserve current package metadata, README/analyzer asset expectations, and provider-to-core dependency shape for the selected package line.

Scope Out
- Changing the dual-line compatibility policy, package IDs, or provider-version matrix already ratified by 06F9G8EQJGBRSWE96VE028HJYW and docs/plans/shared-implementation-standards.md.
- Adding the exhaustive provider matrix assertions owned by 06F9G8F4RQ0T7RV82M3H2H3FVG.
- Updating package verifier behavior, CI guidance, or manual release flow beyond what this story must expose in project and pack inputs; that remains in 06F9G8FBQTAPXXS1Y4NR5QKVG8.
- Updating README or release-note compatibility messaging outside the minimum metadata preservation needed for pack outputs; that remains in 06F9G8FJMZ3AY43YG06W2V4T8G.
- Adding runtime features, provider behavior changes, container or database provisioning, or publication automation.

Open questions
- none

Follow-up questions
- Should a later build-policy task standardize a named line-selection property or artifact-directory convention for separate 8.33.0 and 10.33.0 pack runs, rather than leaving that as local build implementation detail?
- After this story lands, should analyzer-host compatibility be revisited explicitly for non-net10 build environments, or is preserving the current tooling-only analyzer boundary sufficient for v0.33?

Risks
- The repository currently hard-codes net10.0 across runtime, provider, test, verifier, and documentation surfaces, so partial conversion can leave one compatibility line apparently supported in project files but broken in pack or downstream validation.
- Separate 8.33.0 and 10.33.0 outputs keep the existing package IDs, which minimizes naming churn but raises the chance of local pack confusion if the selected line and artifact destination are not explicit during implementation.
- Combining target-framework conditions with the existing opt-in external-provider test switches can accidentally create mixed or under-tested restore graphs if the MSBuild conditions are not composed carefully.
- Because verifier, manual-release, and documentation tasks are deliberately split out, this story can be code-complete while the broader release lane still looks inconsistent until sibling tickets land.

Split recommendations
- No additional split is recommended. The epic is already decomposed into the completed compatibility-policy work plus separate multitargeting, provider-matrix test, verifier and CI, and documentation tickets.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment