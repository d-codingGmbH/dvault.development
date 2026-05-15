[gicket-bot] PO refinement contract

Summary
- Refined ticket 06F1XQ1VWEX0WPAXE78FHSWJ8G against repository and ticket evidence. Existing child task 06F1XQ25KK4VY4MYJSDG9V4BZM is done and provides the bounded PostgreSQL provider-container fixture sample; no new child tickets, planning documents, attachments, or relation changes were created in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- SQLite remains the no-container local baseline. PostgreSQL is the first external provider fixture; SQL Server, MySQL, and Oracle fixture expansion is future work unless separate tickets are created.
- The fixture boundary is opt-in local developer setup: document Podman/Docker commands and hand off DVAULT_TEST_POSTGRES_CONNECTION_STRING to the existing quickstart and external integration tests. Do not make DVault start containers by default.
- Existing provider environment variables are DVAULT_TEST_POSTGRES_CONNECTION_STRING, DVAULT_TEST_SQLSERVER_CONNECTION_STRING, DVAULT_TEST_MYSQL_CONNECTION_STRING, and DVAULT_TEST_ORACLE_CONNECTION_STRING. This story should not introduce parallel names for the same connection-string handoff.
- The repository already uses conditional provider package restore for external integration tests and explicit skip messages when local configuration is missing.

Scope In
- Ratify the checked-in PostgreSQL fixture sample and documentation as the first provider container fixture pattern.
- Document exact local commands, image/tag, port, database, user, local password handling, cleanup, and required schema/table privileges for the PostgreSQL fixture.
- Keep examples/README.md linked to the PostgreSQL fixture and quickstart so one connection string can exercise both the example and opt-in tests.
- Show the repo-root Postgres test command using Category=ProviderIntegration.ExternalOptIn, Provider=Postgres, and -p:DVAULT_TEST_POSTGRES_CONNECTION_STRING=Configured.
- Keep missing connection string, missing runtime/image, unreachable database, wrong credentials, and insufficient privileges explicit through skip or failure diagnostics.
- Preserve a reusable lifecycle pattern for later provider fixtures: start runtime, configure connection string, run targeted validation, inspect output, and clean up.

Scope Out
- No full provider fixture matrix in this story.
- No mandatory Docker, Podman, or external database dependency for default dotnet test execution.
- No checked-in secrets, machine-specific connection strings, or bundled database images.
- No benchmark container orchestration or provider-performance certification expansion.
- No CI provider matrix expansion unless a later ticket explicitly configures it.
- No requirement to introduce a reusable DotNet.Testcontainers abstraction in this story; a documented local fixture is acceptable for the first provider baseline.

Open questions
- none

Follow-up questions
- After the PostgreSQL fixture baseline, should MySQL, SQL Server, or Oracle be the next provider fixture sample?
- Should a later ticket add reusable Testcontainers-based .NET helper code, or keep provider startup as documented local Podman/Docker commands?
- Should future provider fixtures be consolidated into one local-provider guide after at least two external providers have samples?
- Should CI add any opt-in provider container lane later, separate from default test execution?

Risks
- The story title can invite full provider-matrix scope; keep this pass bounded to the done PostgreSQL first-provider fixture and reusable pattern.
- Podman and Docker networking differ across hosts, so the sample must keep hostname and port overrides visible.
- Conditional provider restore can fail if the documented MSBuild marker property is omitted during opt-in test runs.
- Hardcoded ports can collide with local services; documentation should keep alternate host-port mapping explicit.

Split recommendations
- No new split is recommended now. The existing done child 06F1XQ25KK4VY4MYJSDG9V4BZM covers the first provider fixture sample.
- If the product later requires a full external-provider fixture matrix, split MySQL, SQL Server, and Oracle into separate provider-specific tickets because images, licensing, authentication, and privilege setup differ.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 6
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment