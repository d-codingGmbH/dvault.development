[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NADTKZP9J1YCVNMDH60WC/description.md sets PO Handoff to ready_for_po_critic and Open Questions to none.
- src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs defines IDataVaultProviderSaveStrategy and DataVaultProviderSaveStrategyContext, and src/DCoding.Data.DVault/DataVaultSaveService.cs sorts registered strategies by descending Priority and falls back when no strategy CanSave accepts the request batch.
- src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs calls AddDVault() and registers SqlServerDataVaultSaveStrategy as IDataVaultProviderSaveStrategy; src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultSaveStrategy.cs gates on Microsoft.EntityFrameworkCore.SqlServer with no pending tracked changes, filters latest satellite hash diffs, and chunks by SqlServerMaxCommandParameterCount = 2000.
- tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs covers SQL Server registration, clean-context gating, unique-insert SQL shape, latest-hash-diff satellite logic, and saved-record ordering.
- tests/DCoding.Data.DVault.Tests/Integration/SqlServerIntegrationTestConfiguration.cs, tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj, and tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs make live SQL Server coverage opt-in via DVAULT_TEST_SQLSERVER_CONNECTION_STRING and cover representative hub, link, and satellite saves.
- README.md documents the Optional Local SQL Server Integration Tests command using DVAULT_TEST_SQLSERVER_CONNECTION_STRING and says database provisioning is external to DVault; docs/architecture/dvault-v1-explicit-save-service.md documents the SQL Server strategy, provider-neutral fallback, and opt-in validation lane.
- ticket.json for 06EZ0N8HW9PZAFKMM5WQD564VR and 06EZ0N9AM9AJ3AB8DQ6Y1JBS28 shows the referenced blocking tickets are done, and ticket.json for 06EZ0NAMGKJ63WCXAK1J7B08TR and 06EZ0NAWNDDEP32P497E39MQXR shows both materialized child tickets are done.
- git log --decorate --oneline on ticket/06EZ0NADTKZP9J1YCVNMDH60WC-story-optimize-sql-server-provider-save-strategy shows the PO handoff commit e13cffa8 and current PO-critic claim b9a68bb8; git diff --name-only develop..ticket/06EZ0NADTKZP9J1YCVNMDH60WC-story-optimize-sql-server-provider-save-strategy lists only .gicket/tickets/06EZ0NADTKZP9J1YCVNMDH60WC files.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- none

AC / test suggestions
- Evaluate acceptance against the concrete repo surfaces already named in the contract: unit SQL-shape and gating coverage in tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs, opt-in configuration in tests/DCoding.Data.DVault.Tests/Integration/SqlServerIntegrationTestConfiguration.cs and Integration/DCoding.Data.DVault.Tests.Integration.csproj, and live hub/link/satellite smoke coverage in tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs.

Implementation watchouts
- Do not reopen the scoped-out DataVaultProviderCapabilityProfiles.SqlServer path; repo-wide search found no DataVaultProviderCapabilityProfiles.SqlServer, and src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs currently exposes Sqlite and Oracle capability profiles only.
- Preserve the provider-neutral fallback boundary already embodied by src/DCoding.Data.DVault/DataVaultSaveService.cs and the clean-context gate in src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultSaveStrategy.cs.

Non-blocking notes
- The persisted contract is anchored to concrete existing repo surfaces rather than open-ended new design work, which is appropriate for dev handoff.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment