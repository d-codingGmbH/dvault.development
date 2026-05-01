<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story as an already-materialized two-task split: child ticket 06EXB7J6HCA9QZ3DPP5Z03YGJ0 covers the provider capability abstraction, child ticket 06EXB7JEF55Y007XK28DAD1E2R covers the optional Postgres test switch, and repository evidence already confirms the SQLite-default/no-Docker boundary.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Relation context is already materialized: parent epic 06EXB7F6WNWSJJV14EXTPSFDRG links to this story, and this story already has parentOf child tickets 06EXB7J6HCA9QZ3DPP5Z03YGJ0 ('Task: Define provider capability abstraction') and 06EXB7JEF55Y007XK28DAD1E2R ('Task: Add optional Postgres integration test switch'), both currently done.
- Repository evidence shows the provider-readiness seam already lives in src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs, with ApplyDataVaultMetadata() preserving the zero-configuration default path.
- Repository evidence also shows the local Postgres opt-in contract is environment-driven through DVAULT_TEST_POSTGRES_CONNECTION_STRING, with skip messaging in tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs, a Postgres schema test in tests/DCoding.Data.DVault.Tests/Integration/PostgresDataVaultSchemaTests.cs, and matching README documentation.
- No new child tickets, relations, attachments, or planning documents were created in this refinement pass because the split and its ticket context are already persisted.

### Scope In
- Centralize provider-aware mapping decisions behind the existing capability-profile abstraction used by the DVault EF metadata translation path.
- Preserve Sqlite as the default zero-configuration provider/test baseline for normal dotnet test execution.
- Support local opt-in Postgres integration/schema tests when DVAULT_TEST_POSTGRES_CONNECTION_STRING is supplied.
- Document the local Postgres opt-in contract and explicit no-Docker/no-provisioning boundary.

### Scope Out
- Provisioning Docker, containers, databases, or checked-in machine-specific secrets.
- Making Postgres mandatory for default build, test, or CI validation.
- Public runtime provider-selection APIs or general production Postgres provider support.
- Broader provider-specific optimizations, migrations, or non-Sqlite capability matrices beyond the current readiness/test boundary.

## Acceptance Criteria
- Provider-aware logical-to-native mapping decisions for the current DVault EF translation path are centralized behind one capability-profile abstraction instead of scattered provider checks.
- The default ApplyDataVaultMetadata() path continues to use the existing Sqlite-first baseline so ordinary repository validation does not require Postgres.
- Postgres-specific integration tests are skipped when DVAULT_TEST_POSTGRES_CONNECTION_STRING is absent, and the skip message explains the local opt-in contract and that Docker/database provisioning are external.
- When DVAULT_TEST_POSTGRES_CONNECTION_STRING is present, a developer can opt into the Postgres schema/integration tests without changing product code or committing secrets.
- README and test-surface documentation describe the Postgres opt-in contract and keep SQLite as the default test path.

## Definition of Done
- The acceptance criteria are satisfied across the existing source and test layout in src/DCoding.Data.DVault/ and tests/DCoding.Data.DVault.Tests/.
- Documentation and tests follow docs/plans/shared-implementation-standards.md and the repository formatting gate.
- No product-code path or repository-tracked configuration makes Postgres, Docker, or machine-specific setup mandatory for the default validation flow.
- The story remains bounded to provider readiness and local test opt-in, without expanding into general runtime provider support.

## Implementation Notes
- The approved split is already materialized by child ticket 06EXB7J6HCA9QZ3DPP5Z03YGJ0 for the provider capability abstraction and child ticket 06EXB7JEF55Y007XK28DAD1E2R for the optional Postgres test switch; both relations already exist on this story and both child tickets are done.
- Use src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs as the concrete implementation seam for provider readiness, while keeping src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs on its current zero-argument default path.
- Keep Postgres-specific behavior isolated to the existing integration-test surfaces, especially tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs, PostgresDataVaultSchemaTests.cs, and the conditional Npgsql.EntityFrameworkCore.PostgreSQL package reference in tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj.
- README.md already defines the expected environment-variable opt-in contract; implementation should keep repository-tracked secrets and automated database provisioning out of scope.
- The incoming blocks relation from done story 06EXB7FF1J9NR2849WKDR8DKPG reflects existing workflow history and does not introduce a PO refinement blocker for this ticket.

## Open Questions
- none

## Follow-Up Questions
- After workflow catch-up, should the stale incoming blocks relation from done ticket 06EXB7FF1J9NR2849WKDR8DKPG be cleaned up or re-pointed through a separate relation-hygiene pass?
- When a later story introduces real non-Sqlite runtime behavior, should provider selection stay internal first or become a public configuration surface immediately?
- Should CI-hosted Postgres coverage remain deferred until a concrete runtime Postgres capability/profile ticket is scheduled?

## Risks
- Because repository tests already mention Postgres, readers may over-interpret this story as full runtime Postgres support unless the documentation keeps the readiness-only boundary explicit.
- If future provider work bypasses the centralized capability-profile seam, provider checks can fragment again across translator or test code.
- Conditional Postgres test enablement depends on documentation and skip messaging staying aligned with the environment-variable contract.

## Split Recommendations
- No additional split recommended; the story is already bounded by existing child tickets 06EXB7J6HCA9QZ3DPP5Z03YGJ0 and 06EXB7JEF55Y007XK28DAD1E2R, which cover the abstraction work and the local Postgres test opt-in separately.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Prepare provider abstractions and optional Postgres tests while keeping Docker setup outside implementation scope.

## Scope
- Design provider capability abstraction.
- Support local Postgres tests when environment configuration is present.

## Acceptance Criteria
- Sqlite remains the default test path.
- Postgres tests are skipped unless explicitly enabled.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.