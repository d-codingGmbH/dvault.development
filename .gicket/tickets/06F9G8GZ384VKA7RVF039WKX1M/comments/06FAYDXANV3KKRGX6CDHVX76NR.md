[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F9G8GZ384VKA7RVF039WKX1M/description.md:16-21 scopes this ticket to the DB2 package, AddDVaultDb2(), explicit provider-name/profile wiring, and target-framework-pinned IBM.EntityFrameworkCore references; :50-51 sets `## Open Questions` to `none`.
- .gicket/tickets/06F9G8GS08VNH0DT09Q4PC2HRC/ticket.json:7 shows the prerequisite contract ticket is `done`, and its description.md:89-102 defines the exact downstream contract this story relies on: DVault package lines `8.34.0` / `10.34.0`, IBM.EntityFrameworkCore `8.0.0.400` / `10.0.0.100`, provider identifier `IBM.EntityFrameworkCore`, and explicit no-SQLite-fallback registration.
- DVault.slnx:11-18 plus `find src -maxdepth 1 -mindepth 1 -type d -name 'DCoding.Data.DVault*'` show the current repo has `DCoding.Data.DVault`, analyzers, and five provider packages (`MySql`, `Oracle`, `Postgres`, `Sqlite`, `SqlServer`), with no `src/DCoding.Data.DVault.Db2` directory yet.
- src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs:11-19, src/DCoding.Data.DVault/DataVaultModelArtifactImporter.cs:35-44, src/DCoding.Data.DVault/DataVaultModelArtifactExporter.cs:28-34, and src/DCoding.Data.DVault/DataVaultDiagnostics.cs:<redacted> currently enumerate only SQLite/Postgres/SqlServer/Oracle/MySql, matching the ticket's explicit DB2-wiring scope.
- `git diff --name-only 1c6ae83bcd43e21f93fc42135416e3d0de31d5df..HEAD` touches only `.gicket/tickets/06F9G8GZ384VKA7RVF039WKX1M/*`, so the branch evidence is ticket-refinement-only; there is no premature product-code diff to reinterpret.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Assumes IBM's provider continues to expose `DbContext.Database.ProviderName == IBM.EntityFrameworkCore` exactly as recorded in 06F9G8GS08VNH0DT09Q4PC2HRC/description.md:98-102; that contract itself says any alias change needs a new ticket update.
- Assumes the planned family bump to `8.34.0` / `10.34.0` lands coherently with the separate verifier/documentation tickets, because tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs:11-12,23-76 still encode the current `8.33.0` / `10.33.0` seven-package baseline.

AC / test suggestions
- Keep explicit acceptance/test coverage on every finite provider list named by the ticket: `DataVaultProviderCapabilityProfileSelection`, `DataVaultModelArtifactImporter.CreateProviderCapabilityProfiles(...)`, `DataVaultModelArtifactExporter`, and `KnownProviderNames`.
- Add package-matrix coverage that proves `IBM.EntityFrameworkCore` is pinned to `8.0.0.400` only for `net8.0` and `10.0.0.100` only for `net10.0`, with no mixed EF Core line references.
- Add a negative selection/diagnostics check that DB2 does not silently fall through to the SQLite fallback path when the DB2 package is intended to provide explicit support.

Implementation watchouts
- `DataVaultProviderCapabilityProfileSelection.Select(...)` currently falls back to `DataVaultProviderCapabilityProfiles.Sqlite` for unknown provider names (src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs:43-56), so partial DB2 wiring would create misleading support claims.
- The clearest local startup pattern is `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:18-25`, where provider-name registration happens before `services.AddDVault()`; the DB2 package contract expects the same ordering.
- Package verification currently lists only seven packages and the `8.33.0` / `10.33.0` lines (tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs:11-12,23-76), so the separate verifier ticket must stay aligned with this story.

Non-blocking notes
- `test -e src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj` returned exit status `1`; for this pre-development PO gate that absence is expected implementation work, not a ticket-quality blocker.
- No additional split is emerging from branch evidence; the current ticket already delegates schema/guardrails, integration, verifier, and documentation work to sibling tickets rather than widening scope.

Split recommendations
- No further split recommended. The current contract and epic relation set already separate package work (`06F9G8GZ384VKA7RVF039WKX1M`) from schema/guardrails (`06F9G8H5HE1CJHQXGC2C2YK7P8`), integration (`06F9G8HBXS7Y42J7XFSQKZ2AZ8`), package verification (`06F9G8HJJDJH4KF9VK6TZ8B1Z0`), documentation (`06F9G8HRZ72XP5Z7FNWM6MBMQC`), and the completed contract baseline (`06F9G8GS08VNH0DT09Q4PC2HRC`).

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment