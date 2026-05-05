[gicket-bot] PO-critic review contract

Summary
- Ticket scope is strong, but it is not ready for developer handoff until Product names the concrete EF Core MySQL provider baseline and the expected model-translation activation contract.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NBX79YQ0J5A9ECJG955TC/description.md sets PO handoff to ready_for_po_critic and ## Open Questions to none.
- src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs currently only calls services.AddDVault(); and tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs asserts AddDVaultMySql() registers no IDataVaultProviderSaveStrategy.
- src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs exposes only ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel), which calls src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs; that translator hard-codes DataVaultProviderCapabilityProfiles.Sqlite.
- src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs defines 7 current DataVaultLogicalPropertyKind values and only ships the Sqlite profile; tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs currently expects sqlite-v1 and TEXT provider annotations.
- docs/architecture/dvault-v1-explicit-save-service.md assigns provider-specific SQL to provider packages and names the existing external opt-in pattern only for Postgres; tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs uses DVAULT_TEST_POSTGRES_CONNECTION_STRING and PostgresDataVaultSchemaTests.cs skips when it is missing.
- Search rg -n 'Pomelo|MySqlConnector|MySql.EntityFrameworkCore|EntityFrameworkCore.MySql|DVAULT_TEST_MYSQL' src tests docs README.md returned no matches, and a filename search for MySql/MySQL under tests only found tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.MySql.approved.txt.

Blocking findings
- The contract never names the EF Core MySQL provider baseline the optimized writer must accept. That is material because IDataVaultProviderSaveStrategy.CanSave compatibility, SQL dialect details, and any optional live SQL contract coverage all depend on a concrete provider, but the repository currently defines no MySQL provider package, provider-name constant, or opt-in test contract.
- The contract does not pin the caller experience for provider-capability selection. The only public model-translation entry point is ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel), and it currently hard-wires the SQLite profile, so Product should decide whether MySQL activation must work with existing model-building calls only or whether a new explicit model-building hook is acceptable.

Required PO actions
- Amend the delivery contract to name the supported EF Core MySQL provider baseline for this ticket: one specific provider-name/package or an explicit list of provider names that the MySQL strategy must treat as compatible.
- Clarify the allowed activation contract for MySQL model translation: either existing ApplyDataVaultMetadata(...) calls must pick up MySQL automatically after AddDVaultMySql(), or a caller-visible additive model-building hook/overload is explicitly allowed.
- If live MySQL SQL contract tests are in scope, define the external opt-in contract alongside the provider choice. If they are out of scope, state that unit/dispatch coverage alone is acceptable for this ticket.

Open issues ledger
- critic-item-1 [required-po-action] Amend the delivery contract to name the supported EF Core MySQL provider baseline for this ticket: one specific provider-name/package or an explicit list of provider names that the MySQL strategy must treat as compatible.
- critic-item-2 [required-po-action] Clarify the allowed activation contract for MySQL model translation: either existing ApplyDataVaultMetadata(...) calls must pick up MySQL automatically after AddDVaultMySql(), or a caller-visible additive model-building hook/overload is explicitly allowed.
- critic-item-3 [required-po-action] If live MySQL SQL contract tests are in scope, define the external opt-in contract alongside the provider choice. If they are out of scope, state that unit/dispatch coverage alone is acceptable for this ticket.
- critic-item-4 [blocking-finding] The contract never names the EF Core MySQL provider baseline the optimized writer must accept. That is material because IDataVaultProviderSaveStrategy.CanSave compatibility, SQL dialect details, and any optional live SQL contract coverage all depend on a concrete provider, but the repository currently defines no MySQL provider package, provider-name constant, or opt-in test contract.
- critic-item-5 [blocking-finding] The contract does not pin the caller experience for provider-capability selection. The only public model-translation entry point is ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel), and it currently hard-wires the SQLite profile, so Product should decide whether MySQL activation must work with existing model-building calls only or whether a new explicit model-building hook is acceptable.

Missing examples / edge cases
- No example states what CanSave should do when the application uses a different EF Core MySQL provider implementation than the chosen baseline.
- No example shows how MySQL callers are expected to activate the MySQL capability profile during OnModelCreating if internal DI-only activation proves insufficient.

Risky assumptions
- Assuming all EF Core MySQL providers expose interchangeable provider names and SQL behavior would be unsafe; the repository currently provides no direct MySQL baseline.
- Assuming AddDVaultMySql() can switch model translation away from the SQLite default without any caller-visible model-building change is risky given the current public API surface.

AC / test suggestions
- Add an acceptance-criteria sentence that names the provider compatibility rule, for example that the MySQL strategy accepts only the specified provider name(s) and declines all others so provider-neutral fallback remains deterministic.
- If external MySQL validation is desired, mirror the existing Postgres external-opt-in pattern explicitly by naming the environment variable and stating that it is not required local validation.

Implementation watchouts
- src/DCoding.Data.DVault/DataVaultSaveService.cs sorts strategies by descending Priority and falls back when none accept; the MySQL contract needs to preserve that dispatch behavior.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs currently lock the default annotation path to sqlite-v1, so any provider-aware hook must remain strictly additive for non-MySQL callers.

Non-blocking notes
- The persisted contract is otherwise well-bounded: scope in/out is explicit, the architecture document already fixes provider-package ownership, and ## Open Questions is resolved to none.
- git log --oneline -- src/DCoding.Data.DVault.MySql shows only d81b6e885 and bf6170381, which is consistent with the ticket's current claim that MySQL is still a compatibility-only surface.

Split recommendations
- No split recommended after clarification; capability-profile wiring, provider detection, optimized writer behavior, and associated coverage still fit one provider-scoped task.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment