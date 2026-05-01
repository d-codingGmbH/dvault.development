<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined 06EXB7JEF55Y007XK28DAD1E2R into a bounded local-only Postgres test opt-in task; no child tickets or planning documents were created, no recent human comments added extra scope, and the ticket remains a child of 06EXB7HYG17X73GH0K535GYJH8, blocked by 06EXB7J6HCA9QZ3DPP5Z03YGJ0, and blocking 06EXB80QQHAYH61RY4X3T1E8S0.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- V1 should use developer-supplied environment variables as the default opt-in contract for local Postgres integration tests; supporting an equivalent local test-settings path is optional only if the existing test harness already prefers it.
- The default repository validation path must remain usable without a Postgres instance, Docker, or checked-in machine-specific configuration.
- This ticket only adds test gating and documentation; it does not change the current SQLite-first provider baseline visible in DataVaultEfMetadataTranslator and DataVaultProviderCapabilityProfiles.Sqlite.

### Scope In
- Bounded opt-in for Postgres-backed integration tests within the existing DVault test layout.
- Clear skip behavior and diagnostics when required Postgres configuration is absent.
- Repository documentation for the local configuration contract needed to run the Postgres tests.

### Scope Out
- Docker Compose, containers, or other automated Postgres provisioning.
- Making Postgres mandatory for default dotnet test or CI validation.
- General runtime Postgres provider implementation, public provider-selection APIs, or replacing the current SQLite default behavior.

## Acceptance Criteria
- When required Postgres configuration is absent, Postgres-specific integration tests are skipped instead of failing, and the skip message clearly explains that local Postgres configuration is missing.
- When the documented configuration is present, a developer can opt into the Postgres integration tests without editing product code or repository-tracked secrets.
- Documentation names the local opt-in contract and states that Docker or database provisioning is external to DVault.
- Normal dotnet test execution on an unconfigured machine does not require Postgres.

## Definition of Done
- Relevant tests are added or updated inside the existing test roots to cover both configured and unconfigured behavior.
- Documentation is added or updated in the repository and follows the shared implementation standards and formatting gate.
- The repository's default provider behavior remains unchanged outside the explicit Postgres test opt-in path.

## Implementation Notes
- Use the existing repository layout from README.md and DVault.slnx; keep this work inside the current test surfaces rather than creating a new validation flow.
- Prefer environment-driven local configuration for v1 so secrets and machine-specific connection values stay out of checked-in files.
- Because the visible provider baseline is still SQLite, keep any Postgres-specific wiring isolated to test discovery, setup, and documentation.
- No new child tickets, relations, attachments, or planning documents were materialized in this run.

## Open Questions
- none

## Follow-Up Questions
- Should later provider work add CI-hosted Postgres coverage once a production Postgres capability profile exists?
- Should future multi-provider testing tickets standardize one shared naming convention for provider opt-in environment variables?

## Risks
- If the skip contract is vague, developers may read skipped tests as accidental missing coverage rather than intentional local opt-in behavior.
- Later tickets could over-interpret this test switch as proof of runtime Postgres support unless the documentation keeps that boundary explicit.

## Split Recommendations
- No split recommended; the ticket is already bounded to local opt-in, skip behavior, and documentation.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Allow local Postgres tests when a developer provides connection configuration.

## Scope
- Use environment variables or test settings to opt in.
- Do not implement Docker provisioning as part of this task.

## Acceptance Criteria
- Tests skip clearly when Postgres is not configured.
- Documentation states that local Docker setup is external.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.