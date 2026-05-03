[gicket-bot] PO refinement contract

Summary
- Refinement ratified the existing SQLite-local and Postgres-configured test baseline, bounded SQL Server/Oracle/MySQL to smoke coverage in v1, and left no blocking PO questions.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already separates tests into existing Unit and Integration areas inside tests/DCoding.Data.DVault.Tests; this ticket refines execution categories within that layout instead of introducing a new test-project split.
- Visible integration coverage is SQLite-heavy with a Postgres configuration/schema path already present; no existing SQL Server, Oracle, or MySQL external integration fixture is visible in the branch snapshot, so those providers are bounded to smoke coverage in v1 unless equivalent configuration already appears during implementation.
- SQLite remains required local integration coverage and must distinguish the core AddDVault() fallback path from the optimized AddDVaultSqlite() registration path so the expected provider-specific behavior is explicit.
- This ticket already blocks 06EXB82RW6PV2NFG088G6BPFHC, so the default-versus-opt-in category contract defined here should be treated as the source of truth for downstream test and pipeline work.

Scope In
- Category and filter the existing SQLite integration tests as required local coverage in the current integration test project.
- Add an opt-in category boundary and actionable skip behavior for external-provider integration tests that depend on live database configuration.
- Provide default-run smoke coverage for provider packages that should not require live database access, including registration/package checks for providers without external configuration scaffolding.
- Update test discovery or runner expectations so default repository validation executes required SQLite integration and provider smoke coverage without selecting unconfigured external database tests.

Scope Out
- Creating new SQL Server, Oracle, or MySQL live database fixtures, credentials, containers, or CI infrastructure in this ticket.
- Provider-specific performance, tuning, or optimization test work.
- Production API or runtime behavior changes beyond what is needed to support test categorization and coverage clarity.
- Reorganizing the repository into new test assemblies when category/filter work within the existing layout is sufficient.

Open questions
- none

Follow-up questions
- When SQL Server, Oracle, or MySQL gain real external fixtures, should each provider follow the same opt-in configuration contract established here or introduce provider-specific configuration shapes?
- After categories are stable, should opt-in external-provider runs remain manual/on-demand only, or should later CI work add scheduled jobs for configured environments?

Risks
- If runner filters and test categories drift apart, default runs may either miss required SQLite coverage or accidentally include external-provider tests.
- If skip messages are inconsistent across providers, missing external configuration can look like silent test avoidance instead of an intentional opt-in boundary.

Split recommendations
- No new split is recommended; current repository evidence keeps this ticket bounded to category/filtering work inside the existing test layout.
- Existing blocking follow-up work already covers adjacent slices such as unit-test category work (06EXB80FPE3REH11RQ1YR6BW1G) and opt-in Postgres switch work (06EXB7JEF55Y007XK28DAD1E2R), so this ticket should stay focused on the shared provider-integration category contract.

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