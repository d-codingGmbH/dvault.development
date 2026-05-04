[gicket-bot] PO refinement contract

Summary
- Refined the ticket to lock the live MySQL smoke path to Pomelo.EntityFrameworkCore.MySql, require UseMySql plus ServerVersion.AutoDetect(connectionString) bootstrap, and mirror the existing Postgres conditional restore and skip behavior; the existing parentOf relation from story 06EZ0NBPWEWAP264B4XP36CXC8 remains unchanged and no child tickets or planning documents were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The durable contract now requires the live MySQL integration path to use the EF Core provider package Pomelo.EntityFrameworkCore.MySql in the non-packable integration test project. The shipped src/DCoding.Data.DVault.MySql package stays compatibility-only and does not gain a runtime EF Core provider dependency from this ticket.
- critic-item-2: `answered` - The MySQL DbContext setup contract is: take the normalized DVAULT_TEST_MYSQL_CONNECTION_STRING value, build DbContextOptionsBuilder through the chosen provider's UseMySql extension, and bootstrap server-version handling through ServerVersion.AutoDetect(connectionString) or the provider's direct equivalent inside a reflection helper rather than through a hard-coded server-version constant. The live smoke test then uses that MySQL-backed DbContext together with AddDVaultMySql() and IDataVaultSaveService for one insert-only proof.
- critic-item-3: `answered` - Yes. The MySQL path must mirror the Postgres conditional package-restore and missing-provider skip behavior. The integration project should add a conditional PackageReference for Pomelo.EntityFrameworkCore.MySql behind $(DVAULT_TEST_MYSQL_CONNECTION_STRING) != ''. If the env var is absent or blank, the live MySQL path skips through the configuration helper. If the env var is set but the provider assembly is still unavailable, the MySQL reflection helper must Assert.Skip with restore guidance parallel to the existing Npgsql message so restore, build, and test stay automation-stable.
- critic-item-4: `answered` - Resolved by making the contract explicit: the live MySQL DbContext is test-only wiring based on Pomelo.EntityFrameworkCore.MySql and UseMySql with provider-managed server-version detection, while AddDVaultMySql() remains the compatibility-only DVault registration surface. This removes the implementation ambiguity called out by PO-critic without expanding the ticket into provider-specific optimized runtime work.
- critic-item-5: `answered` - Resolved by explicitly requiring MySQL to mirror the Postgres conditional-restore and missing-provider skip behavior whenever DVAULT_TEST_MYSQL_CONNECTION_STRING is used. The ticket now treats that restore-and-skip rule as part of the acceptance contract rather than as an implied implementation detail.

Clarifications
- The live MySQL integration path is locked to the test-only EF Core provider package Pomelo.EntityFrameworkCore.MySql; this ticket does not add that dependency to src/DCoding.Data.DVault.MySql.
- The MySQL DbContext bootstrap contract is UseMySql with the connection string from DVAULT_TEST_MYSQL_CONNECTION_STRING and server-version detection through ServerVersion.AutoDetect(connectionString) or the provider's direct equivalent inside a reflection helper.
- MySQL must mirror the existing Postgres opt-in model end-to-end: configuration helper, conditional package restore, missing-provider Assert.Skip guidance, one live external integration test, and default-smoke discovery tests.
- MySQL remains a compatibility-only provider surface in v1: AddDVaultMySql() continues to call AddDVault() and this ticket does not require a MySQL-specific IDataVaultProviderSaveStrategy or DataVaultProviderCapabilityProfiles entry.
- The existing parentOf relation from story 06EZ0NBPWEWAP264B4XP36CXC8 remains the only relation context for this task; no new child tickets, relations, or planning documents were needed in this PO pass.

Scope In
- Add MySQL opt-in integration-test configuration and skip behavior in tests/DCoding.Data.DVault.Tests/Integration using the existing provider-category conventions.
- Add a conditional Pomelo.EntityFrameworkCore.MySql package reference plus a reflection-based MySQL provider bootstrap helper that only activates when DVAULT_TEST_MYSQL_CONNECTION_STRING is configured.
- Add one ProviderIntegration.ExternalOptIn / Provider=MySQL live smoke test that uses AddDVaultMySql(), a MySQL-backed DbContext, and IDataVaultSaveService to prove at least one insert-only explicit save scenario against a real MySQL database.
- Update provider discovery expectations and README guidance so local and CI users know how to enable the MySQL opt-in path and what restore/build/test contract it follows.

Scope Out
- Any MySQL-specific optimized IDataVaultProviderSaveStrategy, provider capability profile, upsert path, or concurrency work in shipped runtime code.
- Any runtime-package dependency from src/DCoding.Data.DVault.MySql to the EF Core MySQL provider; the provider dependency is test-only for this ticket.
- Repository-managed Docker or MySQL provisioning, checked-in secrets, or a new CI workflow file.
- Broad MySQL parity coverage such as schema snapshot matrices, link or satellite save expansion, performance benchmarking, or MariaDB-specific compatibility commitments beyond the chosen smoke baseline.

Open questions
- none

Follow-up questions
- Should a later ticket add richer MySQL coverage such as link or satellite saves, reuse-path assertions, or schema-translation checks once the single live smoke path is stable?
- Should future automation add a repository-managed MySQL CI service job, or should MySQL remain documentation-only external opt-in coverage for now?
- Should a later provider ticket explicitly define whether MariaDB variants are in scope under the same Pomelo-based test contract, or keep v1 limited to a generic MySQL-compatible server reached through the supplied connection string?

Risks
- The live MySQL smoke still depends on an external developer or CI database, so server-version or dialect drift should be contained by keeping the v1 proof to one narrow insert-only scenario and provider-managed version autodetection.
- Because provider restore is conditional, the MySQL env var must be present for restore, build, and test when the live path is selected; otherwise the provider assembly can be unavailable at execution time and the test will skip instead of proving the path.
- The contract now standardizes on Pomelo.EntityFrameworkCore.MySql, so README and test guidance must stay aligned if the repository later chooses a different EF Core MySQL provider in a separate ticket.

Split recommendations
- If the work expands beyond one compatibility-path smoke test, split MySQL-specific optimized save behavior or capability-profile work into a separate provider ticket.
- If the team wants containerized provisioning or always-on CI execution for MySQL, split that automation from this ticket's test-contract and documentation scope.
- If cross-engine behavior such as MariaDB validation becomes necessary, split that compatibility matrix from this ticket's single-provider smoke baseline.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment