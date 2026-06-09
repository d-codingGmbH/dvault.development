[gicket-bot] PO-critic review contract

Summary
- Approved for dev: the persisted PO contract now resolves the earlier helper-project and test-scope ambiguity, leaves no open PO questions, and the remaining concerns are implementation watchouts rather than PO blockers.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F9G8EXXFJJ1SWWQXC2N9P2X8/description.md persists PO Handoff = ready_for_po_critic, Open Questions = none, and clarifications naming exactly six packable runtime/provider projects plus tests/DCoding.Data.DVault.Tests/Shared, Unit, and Integration, while keeping tests/DCoding.Data.DVault.Tests/Modeling, benchmarks/DCoding.Data.DVault.Benchmarks, and tools/DCoding.Data.DVault.PackageVerification out of standalone net8 scope.
- git diff 0a801a750..f9476318d -- .gicket/tickets/06F9G8EXXFJJ1SWWQXC2N9P2X8/description.md shows the PO refinement added the exact test/build project set and the explicit net8 exclusion language for benchmark-dependent integration and package-verifier unit slices after the earlier return.
- Earlier PO-critic comment .gicket/tickets/06F9G8EXXFJJ1SWWQXC2N9P2X8/comments/06FAPKX9YD5FPMCTGBFT79548M.md recorded five blocking critic items; latest PO refinement comment .gicket/tickets/06F9G8EXXFJJ1SWWQXC2N9P2X8/comments/06FAPPBDX7D2SAHQEAY3SH33H8.md marks critic-item-1 through critic-item-5 as answered.
- Direct repository reads confirm the helper boundaries the contract now calls out: tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj project-references ../../../benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj; ProviderIntegrationCategoryDiscoveryTests.cs includes typeof(BenchmarkScenarioExecutionTests) in RequiredLocalSqliteCoverageTypes; tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj project-references ../../../tools/DCoding.Data.DVault.PackageVerification/DCoding.Data.DVault.PackageVerification.csproj and links ../Modeling/*.cs; tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs exists.
- find tests/DCoding.Data.DVault.Tests -maxdepth 2 -type f -name '*.csproj' returns Shared, Unit, Integration, Modeling, and Analyzers project files, so the updated contract matches the current repository structure rather than the stale seed snapshot.
- Branch history shows the latest substantive PO handoff commit is f9476318d281cd50dbdbdd22a75c2d16ba7c4eff; git diff --name-only 49e567502..f2eaf1c0f -- src tests benchmarks tools returned no files, so this branch remains pre-development and the current review is ticket-quality only.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not include a worked example of the exact net8-only exclusion set beyond the named benchmark discovery and PackageVerifierTests slices, although the current implementation notes are specific enough for developer handoff.
- The ticket leaves artifact-directory or build-property naming for separate 8.33.0 and 10.33.0 pack runs as a follow-up question rather than an acceptance example.

Risky assumptions
- Future helper-dependent Unit and Integration tests will receive matching target conditions so the explicit net8 boundary does not drift over time.
- Sibling task 06F9G8FBQTAPXXS1Y4NR5QKVG8 can complete verifier and CI rewiring later without reopening this ticket's now-explicit project-set decision.

AC / test suggestions
- Validate acceptance against the persisted boundary as written: net8 needs the dual-target package line plus the Shared/Unit/Integration path after conditioned exclusions, while net10 must keep current benchmark and package-verifier coverage intact where it exists today.
- Keep downstream acceptance evidence tied to the ratified 8.33.0 and 10.33.0 contract from 06F9G8EQJGBRSWE96VE028HJYW, especially the required Npgsql.EntityFrameworkCore.PostgreSQL 10.0.2 net10 line.

Implementation watchouts
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj still project-references the net10-only benchmark helper and currently pins Npgsql.EntityFrameworkCore.PostgreSQL 10.0.1; the story contract expects the helper exclusion to be conditioned for net8 and the net10 line to end at 10.0.2.
- tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj still project-references tools/DCoding.Data.DVault.PackageVerification and links ../Modeling/*.cs, so the approved scope depends on keeping verifier-only net8 exclusions separate from modeling-source compatibility.
- tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs hard-codes BenchmarkScenarioExecutionTests in the required SQLite coverage list, so benchmark net10-only exclusions must keep those discovery expectations aligned.

Non-blocking notes
- Current .gicket/tickets/06F9G8EXXFJJ1SWWQXC2N9P2X8/ticket.json still carries blocked/dev and blocked/test from the earlier blocking cycle alongside critic-needed; that looks like workflow residue rather than a current PO ambiguity.

Split recommendations
- No additional split is required; the helper-project boundary is now explicit in this ticket contract, and verifier/CI follow-up already belongs to 06F9G8FBQTAPXXS1Y4NR5QKVG8.
- Keep provider version matrix assertions in 06F9G8F4RQ0T7RV82M3H2H3FVG; this story no longer needs a further child ticket just to resolve project-set ambiguity.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment