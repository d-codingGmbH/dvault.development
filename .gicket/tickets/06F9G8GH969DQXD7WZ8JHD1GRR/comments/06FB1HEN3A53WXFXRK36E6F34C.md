[gicket-bot] tracking-epic-closure-v1

Summary
- Closed tracking-only epic '06F9G8GH969DQXD7WZ8JHD1GRR' because all parentOf child tickets are done and no parent-owned implementation slice remains.
- PO-critic closure audit approved that the completed child set satisfies the parent tracking-only epic.

Evidence
- parent ticket: `06F9G8GH969DQXD7WZ8JHD1GRR`
- parentOf child `06F9G8GS08VNH0DT09Q4PC2HRC` status `done`
- parentOf child `06F9G8GZ384VKA7RVF039WKX1M` status `done`
- parentOf child `06F9G8H5HE1CJHQXGC2C2YK7P8` status `done`
- parentOf child `06F9G8HBXS7Y42J7XFSQKZ2AZ8` status `done`
- parentOf child `06F9G8HJJDJH4KF9VK6TZ8B1Z0` status `done`
- parentOf child `06F9G8HRZ72XP5Z7FNWM6MBMQC` status `done`

PO-critic audit evidence
- .gicket/tickets/06F9G8GH969DQXD7WZ8JHD1GRR/description.md marks the epic as an already-split tracking parent and its `## Open Questions` section is `- none`.
- The persisted parent-child graph contains six `parentOf` edges from epic `06F9G8GH969DQXD7WZ8JHD1GRR`: `.gicket/relations/RR/RC/06F9G8GH969DQXD7WZ8JHD1GRR--06F9G8GS08VNH0DT09Q4PC2HRC--parentOf.json`, `.gicket/relations/RR/1M/06F9G8GH969DQXD7WZ8JHD1GRR--06F9G8GZ384VKA7RVF039WKX1M--parentOf.json`, `.gicket/relations/RR/P8/06F9G8GH969DQXD7WZ8JHD1GRR--06F9G8H5HE1CJHQXGC2C2YK7P8--parentOf.json`, `.gicket/relations/RR/Z8/06F9G8GH969DQXD7WZ8JHD1GRR--06F9G8HBXS7Y42J7XFSQKZ2AZ8--parentOf.json`, `.gicket/relations/RR/Z0/06F9G8GH969DQXD7WZ8JHD1GRR--06F9G8HJJDJH4KF9VK6TZ8B1Z0--parentOf.json`, and `.gicket/relations/RR/QC/06F9G8GH969DQXD7WZ8JHD1GRR--06F9G8HRZ72XP5Z7FNWM6MBMQC--parentOf.json`.
- `src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj` exists, targets `net8.0;net10.0`, and pins `IBM.EntityFrameworkCore` `8.0.0.400` and `10.0.0.100`.
- `src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs` exposes `AddDVaultDb2()` and registers `IBM.EntityFrameworkCore` against `DataVaultProviderCapabilityProfiles.Db2` before adding DB2 behavior.
- `src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs` maps `IBM.EntityFrameworkCore` to `DataVaultProviderCapabilityProfiles.Db2`, and `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs` defines `db2-v1` with `MaximumIdentifierLength = 128`, `AllowsIndexesCoveredByPrimaryKey = false`, and `AppendToKey` included-index handling.
- `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs` routes `IBM.EntityFrameworkCore` to `Db2UnsupportedReader`, and `tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractOutcomeTests.cs` asserts DB2 returns `UnsupportedProvider` until a dedicated reader exists.
- `tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs` covers representative DB2 hub/link/satellite saves plus latest/as-of/PIT/bridge reads, and `tests/DCoding.Data.DVault.Tests/Integration/Db2IntegrationTestConfiguration.cs` keeps that lane opt-in behind `DVAULT_TEST_DB2_CONNECTION_STRING`.
- `tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs` and `tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs` verify the DB2 package/version matrix and eight-package verification expectations.
- `docs/releases/v0.34.0.md`, `README.md`, `docs/production-adoption-checklist.md`, and `docs/manual-nuget-publication.md` all contain the v0.34.0 DB2 documentation baseline.
- `git diff --name-only develop..HEAD -- ':(exclude).gicket/**'` returned no files and `git diff --name-status ccb345e2d45978aa7dc2aac5b214c838b1eff6f8..HEAD` returned no output, so this epic branch is tracker-only and relies on already-landed repository state.

PO-critic non-blocking notes
- `git log --oneline --decorate --max-count=5 -- .gicket/tickets/06F9G8GH969DQXD7WZ8JHD1GRR .gicket/relations/QC/RR/06F9G8HRZ72XP5Z7FNWM6MBMQC--06F9G8GH969DQXD7WZ8JHD1GRR--blocks.json` showed head `ccb345e2d`, `c231253f4`, and `d7f042d2f` as PO/PO-critic lease and handoff commits, which is consistent with a tracker-only epic review rather than pending source implementation on this branch.

PO-critic closure watchouts
- The current DB2 baseline is intentionally bounded: `AddDVaultDb2()` plus `db2-v1` capability/profile wiring, provider-neutral save/read fallback, opt-in external smoke tests, and explicit DB2 live-schema unsupported handling.
- Any follow-on work that adds DB2 provider-native strategies, provisioning, CI infrastructure, or a DB2 live-schema reader would be new scope beyond this tracking epic's accepted boundary.

<!-- gicket-semantic-idempotency-key: bot-closure:06f9g8gh969dqxd7wz8jhd1grr:tracking-epic:done:done -->