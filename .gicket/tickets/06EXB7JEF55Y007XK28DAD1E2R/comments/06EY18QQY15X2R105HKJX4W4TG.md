[gicket-bot] PO refinement contract

Summary
- Refined 06EXB7JEF55Y007XK28DAD1E2R into a bounded local-only Postgres test opt-in task; no child tickets or planning documents were created, no recent human comments added extra scope, and the ticket remains a child of 06EXB7HYG17X73GH0K535GYJH8, blocked by 06EXB7J6HCA9QZ3DPP5Z03YGJ0, and blocking 06EXB80QQHAYH61RY4X3T1E8S0.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- V1 should use developer-supplied environment variables as the default opt-in contract for local Postgres integration tests; supporting an equivalent local test-settings path is optional only if the existing test harness already prefers it.
- The default repository validation path must remain usable without a Postgres instance, Docker, or checked-in machine-specific configuration.
- This ticket only adds test gating and documentation; it does not change the current SQLite-first provider baseline visible in DataVaultEfMetadataTranslator and DataVaultProviderCapabilityProfiles.Sqlite.

Scope In
- Bounded opt-in for Postgres-backed integration tests within the existing DVault test layout.
- Clear skip behavior and diagnostics when required Postgres configuration is absent.
- Repository documentation for the local configuration contract needed to run the Postgres tests.

Scope Out
- Docker Compose, containers, or other automated Postgres provisioning.
- Making Postgres mandatory for default dotnet test or CI validation.
- General runtime Postgres provider implementation, public provider-selection APIs, or replacing the current SQLite default behavior.

Open questions
- none

Follow-up questions
- Should later provider work add CI-hosted Postgres coverage once a production Postgres capability profile exists?
- Should future multi-provider testing tickets standardize one shared naming convention for provider opt-in environment variables?

Risks
- If the skip contract is vague, developers may read skipped tests as accidental missing coverage rather than intentional local opt-in behavior.
- Later tickets could over-interpret this test switch as proof of runtime Postgres support unless the documentation keeps that boundary explicit.

Split recommendations
- No split recommended; the ticket is already bounded to local opt-in, skip behavior, and documentation.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment