[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB7HYG17X73GH0K535GYJH8/description.md is the authoritative contract and its Open Questions section says '- none'.
- git log --oneline --decorate -n 20 shows cc63997f [06EXB7J6HCA9QZ3DPP5Z03YGJ0] AUTO-INTEGRATION squash into develop and 6c12a391 [06EXB7JEF55Y007XK28DAD1E2R] AUTO-INTEGRATION squash into develop.
- git diff --name-only develop..HEAD -- . ':(exclude).gicket/**' returned no paths, so this story branch adds ticket metadata only and no product/test file delta beyond develop.
- src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs defines DataVaultProviderCapabilityProfile and DataVaultProviderCapabilityProfiles.Sqlite; src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs defaults its public Apply path to that SQLite profile; src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs keeps the zero-argument ApplyDataVaultMetadata entry point.
- tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs defines DVAULT_TEST_POSTGRES_CONNECTION_STRING and the explicit skip message; tests/DCoding.Data.DVault.Tests/Integration/PostgresDataVaultSchemaTests.cs skips when configuration is absent.
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj conditionally includes Npgsql.EntityFrameworkCore.PostgreSQL when $(DVAULT_TEST_POSTGRES_CONNECTION_STRING) is set, and README.md documents the opt-in Postgres command plus the no-Docker/no-checked-in-secrets boundary.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Readers could over-interpret the existing Postgres test/documentation surface as general runtime Postgres support unless they follow the contract's explicit scope-out.
- The stale incoming blocks relation from 06EXB7FF1J9NR2849WKDR8DKPG is being treated as workflow history rather than an active dependency.

AC / test suggestions
- Validation should explicitly cover both contract states: missing-env skip behavior and env-provided Postgres schema behavior.
- For the opt-in path, run dotnet test with DVAULT_TEST_POSTGRES_CONNECTION_STRING present from process start so the conditional Npgsql package can be restored.

Implementation watchouts
- Keep work on the existing internal seam in DataVaultProviderCapabilities.cs and DataVaultEfMetadataTranslator.cs; this story does not authorize a public provider-selection API.
- Preserve the zero-configuration SQLite default path in DataVaultModelBuilderExtensions.cs and do not make default validation depend on Postgres, Docker, or tracked secrets.
- Keep Postgres-specific behavior isolated to the existing integration-test and documentation surfaces.

Non-blocking notes
- This story is currently acting more like a contract/workflow aggregator over already-landed child work than a fresh implementation request.
- .gicket/tickets/06EXB7HYG17X73GH0K535GYJH8/comments/06EY25F26V7ZZDXNB2601ZWM64.md records the latest PO outcome as 'po-refinement-ready' and handoff to role 'po-critic'.

Split recommendations
- No further split recommended; the abstraction task and the Postgres opt-in test task are already separate, persisted, and done.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment