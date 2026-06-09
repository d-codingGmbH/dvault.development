[gicket-bot] PO-critic review contract

Summary
- Authoritative delivery contract is specific, aligned with current repository evidence, and has no unresolved PO questions; ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- git -C /mnt/c/Projects/DVault diff --name-only develop...HEAD returned only .gicket/tickets/06F9G8EQJGBRSWE96VE028HJYW/** paths, so this branch is still a ticket-contract change with no product-code drift.
- .gicket/tickets/06F9G8EQJGBRSWE96VE028HJYW/description.md defines the dual-line contract, required provider pins, analyzer boundary, conditional-reference boundary, and an Open Questions section of 'none'.
- src/DCoding.Data.DVault/DCoding.Data.DVault.csproj plus the six provider/analyzer csproj files under src/ expose the seven existing PackageId values and still target net10.0, matching the contract's stated pre-v0.33 baseline.
- docs/plans/shared-implementation-standards.md states 'Current .NET projects target net10.0', which matches the contract's baseline clarification.
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj currently uses opt-in provider PackageReference conditions and pins Microsoft.EntityFrameworkCore.Sqlite 10.0.8, MySql.EntityFrameworkCore 10.0.7, Npgsql.EntityFrameworkCore.PostgreSQL 10.0.1, Oracle.EntityFrameworkCore <redacted>, and Microsoft.EntityFrameworkCore.SqlServer 10.0.8; this directly evidences both the current opt-in pattern and the stated 10.0.1 -> 10.0.2 downstream alignment point.
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj references src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj as OutputItemType='Analyzer' ReferenceOutputAssembly='false' PrivateAssets='all'; src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj also sets SuppressDependenciesWhenPacking=true and packs analyzer assets only, matching the local analyzer-asset boundary.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No worked example yet shows a consumer package set for net8.0 and a separate consumer package set for net10.0 resolving exactly one compatibility line each; this is acceptable for handoff but should be covered by verifier/docs follow-up work.
- The contract deliberately leaves the concrete line-selection property or artifact-directory convention as follow-up build-policy work rather than defining an example in this story.

Risky assumptions
- MySql.EntityFrameworkCore 10.0.7 remains an intentional exception for both targets and downstream docs/verifiers will make clear that this does not mean mixed 8.x/10.x dependency resolution is allowed.

AC / test suggestions
- Downstream verification should include one net8.0 dependency graph and one net10.0 dependency graph and fail any graph that resolves both 8.x and 10.x EF/provider packages together.
- Documentation acceptance should include at least one consumer-facing install example that distinguishes planning release v0.33.0 from NuGet package versions 8.33.0 and 10.33.0.
- Verifier/test acceptance should explicitly check the net10 PostgreSQL pin moves from the current 10.0.1 repository baseline to the required 10.0.2 contract value.

Implementation watchouts
- Current repository baseline is net10.0-only across src/**/*.csproj, tests/**/*.csproj, and docs/plans/shared-implementation-standards.md; downstream implementation must update every surfaced baseline consistently.
- Integration tests already use DVAULT_TEST_*_CONNECTION_STRING-gated provider PackageReference entries; downstream work should preserve that opt-in shape and add target-framework-specific version selection instead of unconditional mixed references.
- Analyzer behavior must stay build-time-local and non-transitive; the existing Analyzer project reference posture is the source evidence to preserve.

Non-blocking notes
- README.md still advertises 0.32.0 installation commands, which matches the upstream policy ticket's historical baseline and is expected until the separate v0.33 documentation ticket lands.

Split recommendations
- No additional split recommended; the existing epic decomposition already separates version-line policy, compatibility contract, multitargeting, verifier/CI, provider-matrix testing, and documentation work.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment