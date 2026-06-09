[gicket-bot] PO-critic review contract

Summary
- The contract is close, but it leaves the exact net8-compatible test/build project set unresolved where in-scope tests currently depend on net10-only helper projects.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F9G8EXXFJJ1SWWQXC2N9P2X8/description.md shows `Open Questions` = `none`, says relevant runtime/provider-facing tests must build against both target frameworks, and separately says verifier rewiring stays in sibling task `06F9G8FBQTAPXXS1Y4NR5QKVG8`.
- All packable runtime/provider projects are still single-target `net10.0`: `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj`, `src/DCoding.Data.DVault.MySql/DCoding.Data.DVault.MySql.csproj`, `src/DCoding.Data.DVault.Oracle/DCoding.Data.DVault.Oracle.csproj`, `src/DCoding.Data.DVault.Postgres/DCoding.Data.DVault.Postgres.csproj`, `src/DCoding.Data.DVault.Sqlite/DCoding.Data.DVault.Sqlite.csproj`, and `src/DCoding.Data.DVault.SqlServer/DCoding.Data.DVault.SqlServer.csproj`.
- All visible test projects are still single-target `net10.0`: `tests/DCoding.Data.DVault.Tests/Shared/DCoding.Data.DVault.Tests.Shared.csproj`, `tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj`, `tests/DCoding.Data.DVault.Tests/Modeling/DCoding.Data.DVault.Tests.Modeling.csproj`, `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj`, and `tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj`.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` project-references `../../../benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj`, and that benchmark project currently targets `net10.0` and still pins `Npgsql.EntityFrameworkCore.PostgreSQL` `10.0.1`.
- `tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj` project-references `../../../tools/DCoding.Data.DVault.PackageVerification/DCoding.Data.DVault.PackageVerification.csproj`, and that verifier project currently targets `net10.0`.
- Branch history shows no implementation changes yet: `git rev-parse HEAD` returned `b42ad3488d3cad6bc2b164c7442f8085121e4e2e`, matching the supplied `scratch-source-ref`, and `git diff --name-only b42ad3488d3cad6bc2b164c7442f8085121e4e2e..HEAD` returned no files.

Blocking findings
- The delivery contract requires dual-target builds for the relevant runtime/provider-facing tests but does not resolve the supporting project set needed to make that true. The in-scope tests currently depend on net10-only helper projects: `benchmarks/DCoding.Data.DVault.Benchmarks` from Integration and `tools/DCoding.Data.DVault.PackageVerification` from Unit, while verifier rewiring is explicitly left to sibling task `06F9G8FBQTAPXXS1Y4NR5QKVG8` and benchmark handling is not mentioned.
- Because the contract references the Shared/Unit/Modeling/Integration baseline but does not explicitly say whether `Shared`, `Unit`, and `Modeling` must multi-target, stay net10-only, or use conditioned references, the promised net8 build/test boundary is still ambiguous at PO level.

Required PO actions
- Amend the contract to enumerate the exact test/build project set that must support the net8/net10 path, including whether `Shared`, `Unit`, `Modeling`, and `Integration` are all in scope or only a narrower subset is required.
- Decide how the net10-only helper projects referenced by those tests are handled: either include `benchmarks/DCoding.Data.DVault.Benchmarks` and-or `tools/DCoding.Data.DVault.PackageVerification` in scope, or explicitly allow conditioned project references or exclusions so the net8 build path remains valid without reopening sibling-ticket scope by surprise.
- If helper projects stay out of scope, add acceptance language that defines the allowed net8 build/test boundary precisely enough that developers do not have to infer it from current project references.

Open issues ledger
- critic-item-1 [required-po-action] Amend the contract to enumerate the exact test/build project set that must support the net8/net10 path, including whether `Shared`, `Unit`, `Modeling`, and `Integration` are all in scope or only a narrower subset is required.
- critic-item-2 [required-po-action] Decide how the net10-only helper projects referenced by those tests are handled: either include `benchmarks/DCoding.Data.DVault.Benchmarks` and-or `tools/DCoding.Data.DVault.PackageVerification` in scope, or explicitly allow conditioned project references or exclusions so the net8 build path remains valid without reopening sibling-ticket scope by surprise.
- critic-item-3 [required-po-action] If helper projects stay out of scope, add acceptance language that defines the allowed net8 build/test boundary precisely enough that developers do not have to infer it from current project references.
- critic-item-4 [blocking-finding] The delivery contract requires dual-target builds for the relevant runtime/provider-facing tests but does not resolve the supporting project set needed to make that true. The in-scope tests currently depend on net10-only helper projects: `benchmarks/DCoding.Data.DVault.Benchmarks` from Integration and `tools/DCoding.Data.DVault.PackageVerification` from Unit, while verifier rewiring is explicitly left to sibling task `06F9G8FBQTAPXXS1Y4NR5QKVG8` and benchmark handling is not mentioned.
- critic-item-5 [blocking-finding] Because the contract references the Shared/Unit/Modeling/Integration baseline but does not explicitly say whether `Shared`, `Unit`, and `Modeling` must multi-target, stay net10-only, or use conditioned references, the promised net8 build/test boundary is still ambiguous at PO level.

Missing examples / edge cases
- No explicit example covers the `net8.0` Integration test path when `tests/DCoding.Data.DVault.Tests.Integration.csproj` references `benchmarks/DCoding.Data.DVault.Benchmarks`, which is currently `net10.0`-only.
- No explicit example covers the `net8.0` Unit test path when `tests/DCoding.Data.DVault.Tests.Unit.csproj` references `tools/DCoding.Data.DVault.PackageVerification`, which is currently `net10.0`-only.
- The contract says analyzer-only tests stay out, but it does not state whether any narrow analyzer-related build accommodation is allowed for `tests/DCoding.Data.DVault.Tests.Integration.csproj`, which consumes the analyzer project as an analyzer asset.

Risky assumptions
- Assuming developers can infer the helper-project scope from repository references without a PO decision risks divergent implementations of the net8 test boundary.
- Assuming the sibling verifier task can remain separate while Unit tests are still part of the required dual-target build path may prove false once project-reference compatibility is enforced.
- Assuming the benchmark project can stay net10-only while Integration tests are required to build on net8 may prove false unless the contract explicitly permits conditioned exclusion.

AC / test suggestions
- Add one acceptance criterion that names the exact dual-targeted test and helper projects, or the exact allowed exclusions and conditions for net8 builds.
- Add one acceptance criterion that the net8 build path does not depend on a `net10.0`-only project reference from Unit or Integration.
- If benchmark participation is intended, add it explicitly and align its provider package pins to the same matrix rules already called out for Integration.

Implementation watchouts
- Current repository evidence still has `Npgsql.EntityFrameworkCore.PostgreSQL` `10.0.1` in both `tests/DCoding.Data.DVault.Tests.Integration.csproj` and `benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj`, while the contract requires `10.0.2` for the net10 line.
- The existing external-provider opt-in conditions in `tests/DCoding.Data.DVault.Tests.Integration.csproj` need to compose with target-framework selection without restoring mixed 8.x and 10.x graphs.
- Keep the analyzer boundary as build-time only; the current Integration test project already consumes the analyzer project as an analyzer asset rather than a runtime dependency.

Non-blocking notes
- The persisted contract has no unresolved open-questions section items.
- The completed compatibility ticket `06F9G8EQJGBRSWE96VE028HJYW` is correctly treated as historical relation residue rather than an active blocker.
- No repository implementation work has landed on this branch yet; this review is evaluating ticket readiness only.

Split recommendations
- No mandatory split is required if PO clarifies the helper-project boundary in this ticket.
- If PO wants to keep `benchmarks/DCoding.Data.DVault.Benchmarks` and `tools/DCoding.Data.DVault.PackageVerification` fully out of scope, consider a small follow-up ticket or an explicit acceptance carve-out so the developer handoff boundary is unambiguous.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment