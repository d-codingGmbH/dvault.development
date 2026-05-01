[gicket-bot] PO refinement contract

Summary
- Refined the story as an already-materialized two-task split: child ticket 06EXB7J6HCA9QZ3DPP5Z03YGJ0 covers the provider capability abstraction, child ticket 06EXB7JEF55Y007XK28DAD1E2R covers the optional Postgres test switch, and repository evidence already confirms the SQLite-default/no-Docker boundary.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Relation context is already materialized: parent epic 06EXB7F6WNWSJJV14EXTPSFDRG links to this story, and this story already has parentOf child tickets 06EXB7J6HCA9QZ3DPP5Z03YGJ0 ('Task: Define provider capability abstraction') and 06EXB7JEF55Y007XK28DAD1E2R ('Task: Add optional Postgres integration test switch'), both currently done.
- Repository evidence shows the provider-readiness seam already lives in src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs, with ApplyDataVaultMetadata() preserving the zero-configuration default path.
- Repository evidence also shows the local Postgres opt-in contract is environment-driven through DVAULT_TEST_POSTGRES_CONNECTION_STRING, with skip messaging in tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs, a Postgres schema test in tests/DCoding.Data.DVault.Tests/Integration/PostgresDataVaultSchemaTests.cs, and matching README documentation.
- No new child tickets, relations, attachments, or planning documents were created in this refinement pass because the split and its ticket context are already persisted.

Scope In
- Centralize provider-aware mapping decisions behind the existing capability-profile abstraction used by the DVault EF metadata translation path.
- Preserve Sqlite as the default zero-configuration provider/test baseline for normal dotnet test execution.
- Support local opt-in Postgres integration/schema tests when DVAULT_TEST_POSTGRES_CONNECTION_STRING is supplied.
- Document the local Postgres opt-in contract and explicit no-Docker/no-provisioning boundary.

Scope Out
- Provisioning Docker, containers, databases, or checked-in machine-specific secrets.
- Making Postgres mandatory for default build, test, or CI validation.
- Public runtime provider-selection APIs or general production Postgres provider support.
- Broader provider-specific optimizations, migrations, or non-Sqlite capability matrices beyond the current readiness/test boundary.

Open questions
- none

Follow-up questions
- After workflow catch-up, should the stale incoming blocks relation from done ticket 06EXB7FF1J9NR2849WKDR8DKPG be cleaned up or re-pointed through a separate relation-hygiene pass?
- When a later story introduces real non-Sqlite runtime behavior, should provider selection stay internal first or become a public configuration surface immediately?
- Should CI-hosted Postgres coverage remain deferred until a concrete runtime Postgres capability/profile ticket is scheduled?

Risks
- Because repository tests already mention Postgres, readers may over-interpret this story as full runtime Postgres support unless the documentation keeps the readiness-only boundary explicit.
- If future provider work bypasses the centralized capability-profile seam, provider checks can fragment again across translator or test code.
- Conditional Postgres test enablement depends on documentation and skip messaging staying aligned with the environment-variable contract.

Split recommendations
- No additional split recommended; the story is already bounded by existing child tickets 06EXB7J6HCA9QZ3DPP5Z03YGJ0 and 06EXB7JEF55Y007XK28DAD1E2R, which cover the abstraction work and the local Postgres test opt-in separately.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment