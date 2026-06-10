[gicket-bot] PO-critic review contract

Summary
- Delivery contract is actionable for a docs-only developer handoff: the ticket has no open questions, the authoritative DB2 package/version sources are pinned, and repository evidence already shows both the stale documentation baseline and the DB2 support limits that the docs must preserve.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj` sets `PackageId` to `DCoding.Data.DVault.Db2`, targets `net8.0;net10.0`, and pins `IBM.EntityFrameworkCore` `8.0.0.400` / `10.0.0.100`.
- `README.md` already documents `DCoding.Data.DVault.Db2` in the `8.34.0` and `10.34.0` install lines, but `docs/production-adoption-checklist.md` still treats `v0.33.0`, `8.33.0`, and `10.33.0` as current and lists only the seven-package family without `DCoding.Data.DVault.Db2`.
- `docs/releases` contains `v0.10.0.md` through `v0.33.0.md`; there is no `docs/releases/v0.34.0.md` yet.
- `README.md` says `Category=ProviderIntegration.ExternalOptIn` currently covers only Postgres, SQL Server, Oracle, and MySQL, while `tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs`, `tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs`, and `tests/DCoding.Data.DVault.Tests/Integration/Db2IntegrationTestConfiguration.cs` already define DB2 opt-in coverage via `Provider=DB2` and `DVAULT_TEST_DB2_CONNECTION_STRING`.
- `src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs` exposes `AddDVaultDb2()`, `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs` defines the `db2-v1` capability profile, and `tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs` exercises representative DB2 save/read behavior.
- `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs` registers `IBM.EntityFrameworkCore` as an explicit unsupported live-schema reader, and `tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractOutcomeTests.cs` asserts `UnsupportedProvider` for DB2.
- `git diff --name-only d4466d320...HEAD` lists only `.gicket/tickets/06F9G8HRZ72XP5Z7FNWM6MBMQC/...` files, so the ticket branch has only PO/critic ticket-metadata commits so far and no repository docs/code edits.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Spell out the non-secret conditional-restore marker `-p:DVAULT_TEST_DB2_CONNECTION_STRING=Configured` alongside the real environment variable, because `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` conditions both the IBM provider package and the DB2 project reference on that property.
- Call out the DB2 live-schema caveat explicitly so docs do not imply parity with the Postgres/SQL Server/Oracle/MySQL live-schema guidance already described elsewhere in `README.md` and `docs/releases/v0.33.0.md`.
- Clarify which documentation surface should carry the external DB2 fixture walkthrough, because `examples/` currently contains only SQLite/Postgres runnable guidance and a Postgres container-fixture README.

Risky assumptions
- Assuming DB2 support docs can stay high-level without naming the current hard limit that live-schema drift reading is explicitly unsupported for `IBM.EntityFrameworkCore`.
- Assuming a generic developer-managed container/Podman note is sufficient even though the repository currently has no checked-in DB2 fixture README or approved DB2 image/tag baseline.
- Assuming repository-wide consistency does not require touching `docs/manual-nuget-publication.md`, which still says the coordinated family is seven packages on the `8.33.0` / `10.33.0` baseline while `README.md` already describes eight `8.34.0` and eight `10.34.0` packages.

AC / test suggestions
- Require the docs to name `DVAULT_TEST_DB2_CONNECTION_STRING`, the `ProviderIntegration.ExternalOptIn` category, and the `Provider=DB2` filter so the DB2 opt-in path matches the existing test contract.
- Require the DB2 provider-support text to stay within source-backed claims: `AddDVaultDb2()`, `IBM.EntityFrameworkCore`, `db2-v1`, 128-character identifier handling, ISO-8601 text timestamp storage by default, provider-neutral save diagnostics, and provider-neutral read fallback.
- Require the v0.34.0 release note to distinguish the planning label `v0.34.0` from consumer package versions `8.34.0` and `10.34.0`, matching the current README install guidance.

Implementation watchouts
- Do not imply default build/test or package restore now requires DB2; the integration project restores IBM provider assets and the DB2 provider project only when `DVAULT_TEST_DB2_CONNECTION_STRING` is non-empty.
- Do not document DB2 as having built-in live-schema drift support or provider-native optimization guarantees; current repository evidence only supports provider registration/capability mapping plus opt-in smoke coverage with provider-neutral save/read paths.
- Do not preserve the stale README wording that external opt-in coverage is only Postgres/SQL Server/Oracle/MySQL once DB2 docs are updated.

Non-blocking notes
- The retrieved ticket comment history is automation/handoff only; no human discussion introduced new unresolved scope.
- The branch history (`95f6ddde9`, `d78ad28ee`, `7f95015b7`) is orchestration-only and consistent with a pre-development PO gate.
- No split is needed just because the branch currently contains only ticket metadata; that is expected before developer implementation.

Split recommendations
- No split recommended; the remaining work is still one coordinated documentation pass across README-adjacent surfaces plus the new v0.34.0 release-note baseline.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment