[gicket-bot] PO-critic review contract

Summary
- Prior PO-critic blockers are addressed in the persisted contract; the ticket is specific enough for developer handoff, with remaining risk in implementation and test execution rather than PO ambiguity.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- gicket-read-ticket returned ticket 06EZ0NBX79YQ0J5A9ECJG955TC revision 06EZ5XJYT9HNFB690JJ4V26ZTW with Open Questions = none and acceptance criteria that now explicitly name Pomelo.EntityFrameworkCore.MySql, the existing ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel) path, MySQL-local IDataVaultProviderSaveStrategy registration, and fallback coverage.
- git -C /mnt/c/Projects/DVault log --oneline --decorate -n 8 ticket/06EZ0NBX79YQ0J5A9ECJG955TC-task-implement-mysql-provider-capability-profile shows 1bdab08b handoff po-critic->po and d8baa649 handoff po->po-critic, confirming a resolved review loop on the same branch.
- src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs currently exposes only AddDVaultMySql() and only calls services.AddDVault();, matching the ticket clarification that MySQL is compatibility-only today.
- src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs exposes only the public ApplyDataVaultMetadata(this ModelBuilder, DataVaultMetadataModel) entry point, and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs currently hard-codes DataVaultProviderCapabilityProfiles.Sqlite.
- src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs currently exposes only public DataVaultProviderCapabilityProfiles.Sqlite and seven DataVaultLogicalPropertyKind values, which directly anchors the MySQL capability-profile completeness requirement to an existing public seam.
- tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs currently expects AddDVaultMySql() to register no IDataVaultProviderSaveStrategy, README.md and docs/architecture/dvault-v1-explicit-save-service.md still describe MySQL as fallback-only, and tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs shows the external opt-in pattern the ticket references for any optional live MySQL tests.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No concrete example is written for a non-Pomelo EF Core MySQL provider declining CanSave; this is not blocking because the acceptance criteria already require fallback outside the named Pomelo baseline.
- No concrete example is written for a dirty DbContext or unsafe ordered batch causing the MySQL optimized path to decline; this is not blocking because the implementation notes already bind MySQL to the existing ProviderSqlExecutionContract safety shape.

Risky assumptions
- The contract assumes the existing ApplyDataVaultMetadata(...) public surface can select a MySQL profile after Pomelo configuration and AddDVaultMySql() registration without introducing a new required public hook, even though the current translator is hard-wired to SQLite.
- The contract assumes Pomelo-only compatibility can be detected cleanly enough to keep unsupported EF Core MySQL providers out of scope without broad provider-name heuristics.
- The contract assumes unit, snapshot, registration, dispatch, and fallback coverage are sufficient to validate MySQL native type choices and UTC load-timestamp behavior without required live MySQL execution.

AC / test suggestions
- Add direct tests for the MySQL capability profile across all seven current DataVaultLogicalPropertyKind values and update API snapshots if a new public profile member is added on DataVaultProviderCapabilityProfiles.
- Flip MySQL registration coverage from expectProviderStrategy: false to true and add dispatch tests that prove Pomelo-only acceptance plus fallback for non-Pomelo and dirty-context cases.
- Add metadata-translation assertions that the existing ApplyDataVaultMetadata(...) path emits MySQL provider annotations and native store types when the Pomelo baseline is active.
- If optional live MySQL tests are later added, mirror the PostgresIntegrationTestConfiguration external opt-in skip pattern instead of making MySQL a required local prerequisite.

Implementation watchouts
- DefaultDataVaultSaveService in src/DCoding.Data.DVault/DataVaultSaveService.cs sorts provider strategies by descending Priority and stops at the first CanSave match, so MySQL gating must not steal unsupported or fallback scenarios.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs is the current hard-coded SQLite seam, so MySQL activation needs additive behavior rather than a silent public-contract change.
- All MySQL-specific SQL needs to stay inside src/DCoding.Data.DVault.MySql, and docs/quality/one-member-per-file.md still applies to new public or protected types in that package.

Non-blocking notes
- README.md and docs/architecture/dvault-v1-explicit-save-service.md still describe MySQL as compatibility-only today, which matches the current codebase and is already covered by the ticket's documentation Definition of Done.

Split recommendations
- No split recommended. The Pomelo baseline decision, existing public activation contract, MySQL-local optimized writer boundary, and bounded test coverage still form one coherent delivery seam.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment