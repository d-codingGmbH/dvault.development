[gicket-bot] PO-critic review contract

Summary
- Delivery contract is clear, bounded, and aligned with current repo patterns; no blocking PO questions remain, so the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4RJP5KG02DF7AEMCQYGNVW/description.md contains a Delivery Contract with Open Questions = none, six acceptance-criteria bullets, full-rebuild-only scope, provider-neutral fallback default, and documentation follow-through split to 06FE4RKGASKV6F7DF0RD1WTAV4.
- .gicket/tickets/06FE4RJP5KG02DF7AEMCQYGNVW/comments/06FF061ZWDG0FWB93G8MS0T4DM.md records PO handoff decision ready_for_po_critic with the same bounded scope; the visible comment history for this ticket is bot refinement/handoff/lease workflow rather than unresolved human clarification.
- git log --oneline --decorate -n 8 on ticket/06FE4RJP5KG02DF7AEMCQYGNVW-task-prototype-postgresql-pit-rebuild-insert-sel shows 3902b3fd2f '[06FE4RJP5KG02DF7AEMCQYGNVW] handoff po->po-critic' and 53c9fab8fa lease-claim po-critic commits, confirming this is still at pre-development handoff.
- git diff --name-only develop...HEAD lists only .gicket/tickets/06FE4RJP5KG02DF7AEMCQYGNVW/* paths, so the branch currently carries ticket metadata/comments but no source or test implementation changes.
- src/DCoding.Data.DVault/DataVaultPitMaintenanceService.cs and src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs keep IDataVaultPitMaintenanceService as the explicit RebuildAsync/MaintainParentsAsync boundary backed by DefaultDataVaultPitMaintenanceService; src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs currently registers save/read/PIT-read/bridge-read strategies only, which matches the ticket's premise that a PIT maintenance selection seam is new internal work rather than a public API change.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs already covers multi-active tuple PIT rebuilds, late-arriving parent correction, and link-parent PIT rebuild/read behavior; tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs accepts Postgres PIT read gates for hub-parent, shared-driving-key multi-active, and link-parent PIT shapes; tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj plus PostgresIntegrationTestConfiguration.cs and NpgsqlProviderReflection.cs show an existing opt-in Npgsql harness via DVAULT_TEST_POSTGRES_CONNECTION_STRING.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Add explicit proof that AddDVaultPostgres on a non-Npgsql DbContext declines the provider path and stays on provider-neutral rebuild without partial execution.
- Add explicit proof that MaintainParentsAsync remains provider-neutral even when the PostgreSQL RebuildAsync candidate exists.
- Add explicit proof that a gated PostgreSQL rebuild replaces preexisting PIT rows and preserves result-count parity with the provider-neutral baseline.

Risky assumptions
- One PostgreSQL INSERT SELECT prototype can safely cover all three approved PIT baseline shapes without forcing a shape-specific follow-up ticket.
- The existing opt-in Postgres harness (DVAULT_TEST_POSTGRES_CONNECTION_STRING) will be available often enough to produce the required external integration proof.
- An internal provider-owned maintenance selection seam can be added without widening IDataVaultPitMaintenanceService or turning the ticket into a cross-provider platform effort.

AC / test suggestions
- Require unit coverage for every explicit gate-decline reason the prototype uses, including provider mismatch, unsupported PIT shape, and full-rebuild-only rejection of MaintainParentsAsync.
- Require an opt-in Npgsql integration test lane that compares PostgreSQL-provider rebuild output and DataVaultPitMaintenanceResult values against the provider-neutral baseline for ordinary, shared-driving-key multi-active, and link-parent PITs.
- Require tracing/diagnostics assertions that selected and declined PostgreSQL paths stay redacted and do not surface raw SQL, query plans, connection details, or request values.

Implementation watchouts
- Do not introduce a new public maintenance entrypoint or a shared cross-provider maintenance platform in this ticket.
- Keep MaintainParentsAsync and bridge maintenance on the provider-neutral path; this ticket is RebuildAsync-only.
- Keep PostgreSQL SQL text, quoting, connection handling, and exact provider-name matching inside DCoding.Data.DVault.Postgres.
- Preserve DefaultDataVaultPitMaintenanceService semantics for delete-and-rebuild behavior, DataVaultPitMaintenanceResult counts, late-arriving correction behavior, and redacted activity tracing.

Non-blocking notes
- git show --stat 3902b3fd2f174e74249591a71d497dff2628b981 touches .gicket/tickets/06FE4RJP5KG02DF7AEMCQYGNVW/description.md and ticket.json, so the PO summary line claiming no description write was applied is a provenance-note mismatch; it does not weaken the current delivery contract.
- The current ticket branch is metadata-only versus develop, which is expected for a pre-development PO handoff but means all repo proof promised by this ticket still needs to be created by the developer/tester path.

Split recommendations
- Keep the current split: 06FE4RJP5KG02DF7AEMCQYGNVW for the PostgreSQL prototype, 06FE4RJZ4PA0DZ3HXDSEG2BQMM for the SQL Server sibling, 06FE4RK80ZXGCZ62CMSAYP164W for bridge feasibility, and 06FE4RKGASKV6F7DF0RD1WTAV4 for documentation follow-through.
- If one supported PIT baseline shape proves materially different in SQL complexity or proof burden, open a shape-specific follow-up ticket instead of widening this prototype mid-stream.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment