[gicket-bot] PO-critic review contract

Summary
- Approved for developer handoff: the contract is bounded, has no unresolved open questions, and matches the repository's current SQLite-first default and existing test layout, with Postgres kept explicitly test-only and opt-in.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB7JEF55Y007XK28DAD1E2R/description.md` contains a delivery contract with `## Open Questions` set to `- none`, acceptance criteria for skip behavior/documentation/default `dotnet test`, and scope-out text excluding public provider-selection APIs and runtime Postgres support.
- `git rev-parse ticket/06EXB7JEF55Y007XK28DAD1E2R-task-add-optional-postgres-integration-test-swit 14f5b073269236b1038469a1947b676fd4f9af92` returned the same SHA for the branch head and scratch-source-ref; `git log -n 6` shows only PO/PO-critic handoff and lease-claim commits on this branch after `develop`.
- `git diff --name-only develop..ticket/06EXB7JEF55Y007XK28DAD1E2R-task-add-optional-postgres-integration-test-swit -- . ':(exclude).gicket/**'` returned no files, so the current review branch changes ticket metadata/workflow state only, not repository source or tests.
- `README.md`, `DVault.slnx`, and `tests/DCoding.Data.DVault/README.md` confirm the intended layout: executable tests live under `tests/DCoding.Data.DVault.Tests/` with Unit, Integration, and Shared projects, while default local validation still includes plain `dotnet test` from the repo root.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` still defaults its provider path to `DataVaultProviderCapabilityProfiles.Sqlite`, and `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` exposes only `ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel)` with no public provider-selection parameter.
- `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs` currently exposes one built-in profile, `DataVaultProviderCapabilityProfiles.Sqlite`.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` references `Microsoft.EntityFrameworkCore.Sqlite`, and the current shared/integration test helpers are SQLite-specific (`tests/DCoding.Data.DVault.Tests/Shared/SqliteTestDatabase.cs`, `Integration/SqliteDataVaultSchemaTests.cs`, `Integration/SqliteProviderCapabilityProfileTests.cs`).
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs` already uses reflection to invoke the translator's internal 3-parameter `Apply` overload with a custom `DataVaultProviderCapabilityProfile`, which is an existing test-only seam for non-default provider-profile validation without changing the public API.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Partial configuration: some required Postgres environment variables are present but the full opt-in contract is incomplete.
- Malformed configuration: invalid connection string or invalid individual environment-variable values.
- Configured-but-unreachable database: credentials/network/database name are present but the database cannot be reached.
- Isolation expectations for local Postgres runs: database naming, cleanup, and parallel-test collision handling are not spelled out in the ticket.

Risky assumptions
- The implementation can add meaningful Postgres-backed tests without widening the public API, even though the public `ApplyDataVaultMetadata` path remains SQLite-default and the built-in profiles currently stop at `Sqlite`.
- Developers will choose a clear environment-variable naming contract during implementation; the ticket intentionally fixes the mechanism class (`environment variables`) but not the exact variable names or minimum key set.
- Documentation will make it unmistakable that this ticket adds local optional test coverage only and does not imply supported runtime Postgres provider behavior.

AC / test suggestions
- Add at least one deterministic test around discovery/gating and skip-message text that passes on machines without Postgres, so the default validation path proves the opt-in contract without a live database.
- Treat partial or malformed configuration as part of the contract: either skip with a deterministic diagnostic or fail only after an explicit fully-configured opt-in path is attempted.
- In documentation, show the exact environment-variable names and one example opt-in invocation, while explicitly restating that Docker/provisioning is external and default `dotnet test` remains unchanged.

Implementation watchouts
- Do not broaden `DataVaultModelBuilderExtensions.ApplyDataVaultMetadata` or change `DataVaultEfMetadataTranslator`'s default SQLite behavior just to make the Postgres tests possible.
- Keep any Postgres-specific package references, provider profiles, setup helpers, and diagnostics isolated to test code and repository documentation; avoid implying a production-ready provider surface.
- Ensure skip behavior happens before absent-configuration runs degrade into connection/setup failures on unconfigured machines.
- Do not introduce repository-tracked secrets, machine-specific config files, or a CI/default dependency on Postgres.

Non-blocking notes
- The persisted blocker relation to `06EXB7J6HCA9QZ3DPP5Z03YGJ0` is no longer an active refinement concern because that upstream ticket is already `done` and integrated to `develop`.
- The current PO/PO-critic branch delta is ticket metadata only; no repository code has been staged yet for this ticket.
- The existing test surface is strongly SQLite-shaped, so some test-only scaffolding is expected even though the contract correctly keeps product behavior unchanged.

Split recommendations
- No split recommended; the contract remains bounded to local opt-in test gating, clear skip diagnostics, documentation, and preserving the default no-Postgres validation path.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment