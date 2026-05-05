[gicket-bot] PO-critic review contract

Summary
- Persisted story contract is closed and concrete, and current repository, branch-history, and ticket/comment evidence all support developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NBPWEWAP264B4XP36CXC8/description.md persists the delivery contract with 'PO Handoff' set to 'ready_for_po_critic' and '## Open Questions' set to 'none'.
- git log --oneline -n 20 shows 045e71e7 [06EZ0NBX79YQ0J5A9ECJG955TC] AUTO-INTEGRATION squash into develop and c19376a1 [06EZ0NC3VNZ5FP9XDYVX9DHW1G] AUTO-INTEGRATION squash into develop; git diff --name-only develop...HEAD only changes .gicket/tickets/06EZ0NBPWEWAP264B4XP36CXC8/*, so the story branch is carrying ticket-state updates rather than hidden implementation.
- src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:15-22 registers the Pomelo provider-name mapping to DataVaultProviderCapabilityProfiles.MySql and wires MySqlDataVaultSaveStrategy through AddDVaultMySql().
- src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs:19-56 gates the optimized path to clean Pomelo.EntityFrameworkCore.MySql contexts, uses INSERT IGNORE for unique-row writes, and src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs:263-333 filters satellite writes by latest hash diff.
- tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs:12-113 and tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs:141-198 cover registration, Pomelo-only profile selection, and non-Pomelo fallback behavior.
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:15-20 conditionally restores Pomelo.EntityFrameworkCore.MySql when DVAULT_TEST_MYSQL_CONNECTION_STRING is set, and tests/DCoding.Data.DVault.Tests/Integration/MySqlExplicitDataVaultSaveServiceTests.cs:21-95 provides the opt-in live MySQL smoke path.
- README.md:183-272 and docs/architecture/dvault-v1-explicit-save-service.md:41-64 document ProviderIntegration.ExternalOptIn via DVAULT_TEST_MYSQL_CONNECTION_STRING, Pomelo-only MySQL support, provider-neutral fallback, and SQLite-only benchmark posture.
- tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs:45-57, tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.MySql.approved.txt, and tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs:18-29 and 179-193 show that MySQL package API snapshot and package-verification coverage are part of the bounded validation surface.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The current opt-in live MySQL smoke path proves an explicit hub save, but it does not yet demonstrate live link or satellite scenarios.
- The story intentionally excludes direct support examples for MySql.EntityFrameworkCore or MariaDB-specific compatibility; those providers remain fallback-only.
- There is no MySQL-specific benchmark or CI-managed MySQL execution example; benchmark coverage remains the shared SQLite runner.

Risky assumptions
- Exact provider-name matching on Pomelo.EntityFrameworkCore.MySql remains stable enough to keep capability selection and optimized dispatch correct over future package updates.
- The opt-in external MySQL smoke lane is representative enough even though real MySQL environment differences can still surface after merge.
- SQLite benchmark evidence remains an acceptable proxy until a separate MySQL performance-validation ticket exists.

AC / test suggestions
- If Product wants stronger release confidence later, open a follow-up ticket for opt-in live MySQL link and satellite smoke assertions.
- If broader provider coverage matters, create a separate ticket for MySql.EntityFrameworkCore or MariaDB compatibility instead of widening this story.
- If performance needs a provider-specific gate later, create a dedicated MySQL benchmark or CI ticket rather than extending this story.

Implementation watchouts
- Preserve provider-neutral fallback whenever the active provider is not Pomelo or the DbContext already has Added, Modified, or Deleted tracked entries.
- Keep MySQL-specific SQL confined to src/DCoding.Data.DVault.MySql; the core package should remain provider-selection and fallback orchestration only.
- The live MySQL lane depends on DVAULT_TEST_MYSQL_CONNECTION_STRING being present during restore, build, and test so the conditional Pomelo package reference is available.

Non-blocking notes
- Child ticket 06EZ0NBX79YQ0J5A9ECJG955TC had an earlier tester return-to-dev, but the later tester handoff comment 06EZAY093W7F862AA785WKMFYG supersedes it with verified completion at commit de4a13f4cc95.
- The story branch diff against develop is ticket-metadata only, which aligns with the story now serving as a verified parent contract over already-integrated child work.

Split recommendations
- No further split recommended; the story is already materialized through parentOf children 06EZ0NBX79YQ0J5A9ECJG955TC and 06EZ0NC3VNZ5FP9XDYVX9DHW1G, and both are done.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment