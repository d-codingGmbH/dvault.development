[gicket-bot] PO refinement contract

Summary
- Refined the ticket against local repository evidence. PostgreSQL is the v1 first provider-container fixture sample, aligned to the existing Postgres quickstart, opt-in integration tests, and DVAULT_TEST_POSTGRES_CONNECTION_STRING. No child tickets, planning documents, attachments, or relation changes were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Use PostgreSQL for the first provider container fixture sample. Repository evidence already has a PostgreSQL quickstart, Postgres external opt-in integration tests, AddDVaultPostgres coverage, and README commands for DVAULT_TEST_POSTGRES_CONNECTION_STRING.
- SQLite remains the required local no-container baseline; it is not the target for this container fixture sample.
- The sample should provide developer-managed Podman/Docker setup guidance and hand off a normal connection string to existing DVault tests/examples. It must not make container startup mandatory for default test runs or CI.
- The visible container image baseline is docker.io/postgres:18 from repository release documentation; if implementation pins an image, start from that checked-in baseline and keep the tag explicit in the sample.
- Current ticket comments are bot claim/lease metadata only and do not alter product scope.
- Live relation state was verified: incoming parentOf from 06F1XQ1VWEX0WPAXE78FHSWJ8G, incoming blocks from done tickets 06F1XQ03MADSPQD0AJN6R50D44 and 06F1XPX99KQRB09GRQG50Z75FM. The done-source blocks are historical/non-blocking under the refinement rules and were left unchanged.

Scope In
- Add a runnable PostgreSQL provider container fixture sample for local development.
- Expose a DVAULT_TEST_POSTGRES_CONNECTION_STRING value compatible with PostgresIntegrationTestConfiguration and the PostgreSQL quickstart.
- Document Podman and Docker expectations, including image/tag, port, database, user, password placeholder, cleanup, and required schema/table privileges.
- Show the existing repo-root command for running Postgres external opt-in tests with the configured connection string and MSBuild marker property.
- Keep missing runtime, missing image, missing connection string, or unreachable database outcomes explicit and developer-readable.
- Shape the sample so later MySQL, SQL Server, and Oracle fixtures can reuse the same documentation pattern.

Scope Out
- No full provider matrix in this task.
- No mandatory CI container startup or default test-suite dependency on Docker or Podman.
- No checked-in secrets, machine-specific connection strings, or bundled database images.
- No replacement of existing SQLite required-local coverage.
- No broad Testcontainers helper framework for every provider; that remains parent/helper-story scope.
- No benchmark container orchestration or provider-performance certification expansion.

Open questions
- none

Follow-up questions
- After the PostgreSQL sample lands, should the parent helper story prioritize MySQL, SQL Server, or Oracle as the next provider fixture?
- Should a later task convert the docs-only sample into reusable Testcontainers-based test helpers, or keep provider startup as documented local commands?
- Should future provider fixture samples share one consolidated local-provider guide once at least two external providers have working samples?

Risks
- Container guidance can drift from README if it implies DVault provisions databases by default; documentation must preserve the opt-in boundary.
- Hardcoded ports or credentials can conflict with developer machines; the sample should make overrides and cleanup clear.
- Podman and Docker networking differ on some hosts, so the sample should call out connection-string adjustment rather than hiding runtime-specific assumptions.

Split recommendations
- No new split is recommended. This task is already the bounded first-provider sample under parent story 06F1XQ1VWEX0WPAXE78FHSWJ8G, while the full provider matrix remains out of scope.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment