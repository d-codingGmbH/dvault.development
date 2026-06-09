[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F9G8EE7ZA666MW8YEB2QP8BW/description.md contains ## Open Questions with none and PO handoff ready_for_po_critic.
- git log --graph -n 8 shows develop already contains AUTO-INTEGRATION squashes for child work, including commits 49e567502, a1a2a7aa7, eb925a227, 65d5f7cc4, and e529c3a56.
- git diff --name-only develop..HEAD lists only .gicket ticket files for 06F9G8EE7ZA666MW8YEB2QP8BW, and git log develop..HEAD is only four workflow commits (4eeea2fe5, 5e3327400, 936e1dfe5, ae8bfc0f2), so the epic branch adds ticket metadata only on top of develop.
- Repository source matches the contract boundary: src/DCoding.Data.DVault/DCoding.Data.DVault.csproj and provider projects under src/DCoding.Data.DVault.{MySql,Oracle,Postgres,Sqlite,SqlServer}/ target net8.0;net10.0, while src/DCoding.Data/DCoding.Data.csproj remains non-packable net10.0.
- tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs and tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj pin the documented matrix, including MySql.EntityFrameworkCore 10.0.7 for both targets and the DVAULT_TEST_* external-provider opt-in gates.
- README.md, docs/manual-nuget-publication.md, docs/plans/shared-implementation-standards.md, and docs/releases/v0.33.0.md all separate consumer lines 8.33.0/net8.0 and 10.33.0/net10.0, reject consumer-facing 0.33.0, and keep analyzer guidance local with PrivateAssets=all; tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs enforces the same rules.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Developer handoff is still appropriate even though the owner branch is metadata-only above develop, because the intended implementation baseline for this epic is the already integrated child-ticket work on develop.
- The live blocks relation from 06F9G8EE7ZA666MW8YEB2QP8BW to 06F9G8GS08VNH0DT09Q4PC2HRC is treated as downstream workflow cleanup, not unfinished epic scope.

AC / test suggestions
- Keep closure evidence anchored to the same surfaces already named in the contract: README.md, docs/manual-nuget-publication.md, docs/plans/shared-implementation-standards.md, docs/releases/v0.33.0.md, EfCoreProviderVersionMatrixTests.cs, and PackageVerifier.cs.
- If a later closure reviewer wants one explicit check, verify the documented separation between Category=ProviderIntegration.RequiredLocal and Category=ProviderIntegration.ExternalOptIn still matches the repository baseline.

Implementation watchouts
- Do not treat the epic owner branch as the implementation branch; develop..HEAD contains only ticket metadata, while the implementation evidence landed through child-ticket AUTO-INTEGRATION commits on develop.
- Keep the analyzer boundary explicit: src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj stays net10.0, and consumer compatibility relies on package-line guidance plus local analyzer assets rather than analyzer-project multitargeting.
- Keep src/DCoding.Data/DCoding.Data.csproj out of any consumer compatibility or publication claim; it remains the non-packable net10.0 source-root anchor.

Non-blocking notes
- The live blocks relation file .gicket/relations/BW/RC/06F9G8EE7ZA666MW8YEB2QP8BW--06F9G8GS08VNH0DT09Q4PC2HRC--blocks.json still exists, but the PO run report already treated that as a queued follow-up rather than a refinement blocker.

Split recommendations
- No further split recommended; the existing child decomposition already covers the epic scope through 06F9G8EQJGBRSWE96VE028HJYW, 06F9GF2Z4Y7A91ZHG4NW1YTNMC, 06F9G8EXXFJJ1SWWQXC2N9P2X8, 06F9G8F4RQ0T7RV82M3H2H3FVG, 06F9G8FBQTAPXXS1Y4NR5QKVG8, and 06F9G8FJMZ3AY43YG06W2V4T8G.
- Keep 06F9G8GS08VNH0DT09Q4PC2HRC as downstream follow-on work rather than expanding this epic.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment