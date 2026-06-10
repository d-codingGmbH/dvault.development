<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket to one bounded DB2 external-opt-in integration story: add DB2 save and read smoke coverage around the existing provider-neutral baseline, keep optimized-strategy and live-schema-reader work out of scope, and materialize no child tickets, relation updates, description updates, attachments, or planning documents.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- No bounded ticket writes were materialized in this refinement run: no child tickets, relation updates, description updates, attachments, or planning documents.
- The target branch head matches the supplied scratch source ref `a2317f2f84b07998327e06ba0b0846b8c334dabf`, so there is no partial implementation on the branch to ratify.
- `src/DCoding.Data.DVault.Db2` currently exposes `AddDVaultDb2()` plus DB2 provider-behavior/capability-profile wiring only; unlike PostgreSQL, SQL Server, MySQL, Oracle, and SQLite provider packages, it does not register a DB2-specific save strategy or PIT/bridge/latest read strategy.
- The current read-optimization baseline keeps optimized latest-satellite reads SQLite-only and optimized PIT/bridge reads limited to PostgreSQL, SQL Server, MySQL, and Oracle, so DB2 read coverage for this story should prove provider-neutral execution and diagnostics fallback rather than invent a DB2 optimized path.
- Unit coverage already marks `IBM.EntityFrameworkCore` live-schema reading as explicitly unsupported until a reader exists, so DB2 live-schema drift and reader parity are not part of this ticket.

### Scope In
- DB2 external opt-in integration test scaffolding in the integration test project, including conditional DB2 provider package wiring and connection-string-gated execution consistent with existing external providers.
- Smoke coverage that `AddDVaultDb2()` plus the real IBM EF Core provider can persist explicit hub, link, and satellite saves against a live DB2 database.
- DB2 current/latest and as-of latest-satellite reads plus PIT as-of and bridge traversal integration coverage on maintained test data, using the existing provider-neutral read boundary where no DB2 optimized strategy is registered.
- Diagnostic assertions or equivalent observable evidence that DB2 save and read execution remains on the documented provider-neutral fallback path when no DB2-specific strategy is available.

### Scope Out
- New DB2 provider-specific optimized save, latest-satellite, PIT, or bridge read strategies.
- DB2 live-schema reader or drift-reporting support.
- Making DB2 part of the default local validation lane or provisioning DB2 or Podman infrastructure inside the repository.
- DB2 benchmark or performance-claim rows, or release-posture expansion beyond the tested smoke and integration boundary.

## Acceptance Criteria
- The integration test project can opt into DB2 execution through a DB2-specific connection-string gate following the existing external-provider pattern, and DB2 tests skip cleanly when the gate is absent.
- `AddDVaultDb2()` is covered against a real DB2 database for representative explicit hub, link, and satellite saves, and persisted rows prove the expected hash key, load timestamp, record source, and payload behavior.
- Representative DB2 current/latest and as-of latest-satellite reads, PIT as-of reads, and bridge traversal reads succeed against maintained test data without requiring any new DB2-specific optimized read strategy.
- DB2 save and read diagnostics for the covered scenarios do not claim a nonexistent DB2 provider-specific strategy; they preserve the documented provider-neutral fallback posture where applicable.
- Provider discovery and category baselines are updated so DB2 smoke and integration test classes are explicitly categorized as external opt-in coverage and do not disturb required local SQLite coverage.

## Definition of Done
- Net8 and net10 integration test assets both build with the DB2 opt-in wiring in place, and default local test execution remains DB2-free when the connection-string gate is unset.
- The new DB2 tests pass when a developer supplies a live DB2 connection string and fail only on real regressions, not on missing external infrastructure.
- Existing unit and integration tests that codify provider-neutral fallback, provider discovery, and DB2 unsupported live-schema-reader behavior remain green.
- No new docs or tests imply that DB2 now has provider-specific optimized read or save strategies or live-schema-reader support.

## Implementation Notes
- Mirror the established external-provider pattern: add a DB2 integration configuration class and trait or category entry, keep the IBM provider package conditional on the DB2 connection-string environment variable, and keep execution opt-in only.
- The integration test project currently references MySQL, PostgreSQL, Oracle, SQL Server, and SQLite provider packages but not `DCoding.Data.DVault.Db2` or `IBM.EntityFrameworkCore`; this ticket should add the minimum DB2-specific project and package wiring needed for the new tests.
- `AddDVaultDb2()` already calls `AddDVault()` and registers `IBM.EntityFrameworkCore` to the `db2-v1` capability profile plus `db2-provider-v1` provider behavior; use that existing startup surface rather than adding a parallel DB2-only registration story.
- Because no DB2 provider-specific save or read strategy is registered today, save coverage should validate provider-neutral `IDataVaultSaveService` behavior and read coverage should validate provider-neutral latest-satellite, PIT, and bridge execution plus fallback diagnostics.
- Use the installed developer-managed DB2 or Podman path only as an external opt-in harness; do not add repo-owned container provisioning, secrets, or default CI requirements.

## Open Questions
- none

## Follow-Up Questions
- After DB2 integration coverage lands, should README and release-validation docs be aligned with the DB2 package line and test story, since README installation guidance includes `DCoding.Data.DVault.Db2` but the current `v0.33.0` release note omits DB2 from the documented package family and external-provider matrix?
- Should DB2 later get a dedicated live-schema reader or optimized save and read strategies once there is stable evidence that provider-neutral coverage is insufficient?

## Risks
- `IBM.EntityFrameworkCore` DDL, type-mapping, or transaction behavior may require DB2-specific fixture handling even though the runtime save and read path stays provider-neutral.
- The integration project must maintain conditional IBM provider package wiring for both `net8.0` and `net10.0`; missing one target would create a parity gap.
- Because DB2 coverage stays opt-in and externally provisioned, unattended default-local validation will only prove discovery and skip behavior unless a DB2 instance is explicitly supplied.

## Split Recommendations
- No split recommended; the visible branch state supports one bounded story covering DB2 opt-in test scaffolding plus representative save and read integration coverage.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add DB2 provider smoke and integration coverage for explicit hub/link/satellite saves plus current/as-of/PIT/bridge read paths where the existing provider-neutral or diagnostics-gated provider strategy supports DB2. Use the installed Podman/container path only for external opt-in tests, and preserve provider-neutral fallback for unsupported optimized shapes.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Added DB2 external opt-in integration coverage behind `DVAULT_TEST_DB2_CONNECTION_STRING`.
- Covered `AddDVaultDb2()` live DB2 hub, link, and satellite saves through provider-neutral fallback.
- Covered DB2 latest/current/as-of satellite reads, PIT as-of reads, and bridge traversal reads on maintained test data through provider-neutral fallback.
- Updated provider category discovery and the integration provider package matrix for net8.0 and net10.0 conditional IBM wiring.
- Repaired the build failure from the first developer snapshot by using the correct `IndexOverride` record constructor parameter name.

### Verification
- `dotnet build DVault.slnx --nologo` passed. NuGet vulnerability lookup emitted read-only HTTP-cache warnings, but the build completed with 0 errors.
- `dotnet test DVault.slnx --nologo --no-build` passed after the successful build: integration net8.0 210 total / 187 succeeded / 23 skipped; integration net10.0 223 total / 200 succeeded / 23 skipped; unit net8.0 449 total / 449 succeeded; unit net10.0 467 total / 467 succeeded.
- `bash tools/check-format.sh` passed.

### DB2 Opt-In Notes
- Default local validation remains DB2-free; the DB2 live smoke tests skipped cleanly when `DVAULT_TEST_DB2_CONNECTION_STRING` was absent.
- Set `DVAULT_TEST_DB2_CONNECTION_STRING` before restore/build/test so the conditional `IBM.EntityFrameworkCore` package reference and `DCoding.Data.DVault.Db2` project reference materialize.
- With a live DB2 database available, run `dotnet test tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj --filter "Provider=DB2" --nologo`.
<!-- gicket-bot:developer-delivery:v1:end -->