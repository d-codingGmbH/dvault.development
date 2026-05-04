[gicket-bot] PO refinement contract

Summary
- Refined this implementation task against current repo and ticket evidence; no new child tickets or planning docs were materialized because story 06EZ0N9TJSXFXH0YZRA3QN2S14 already has sibling task 06EZ0NA7CWDYJ7ZS3K5GM0187M for opt-in PostgreSQL integration coverage.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence shows src/DCoding.Data.DVault.Postgres currently contains only AddDVaultPostgres() calling services.AddDVault() and no PostgreSQL-specific save strategy implementation.
- This ticket is already a child of story 06EZ0N9TJSXFXH0YZRA3QN2S14 (Optimize PostgreSQL provider save strategy).
- Sibling task 06EZ0NA7CWDYJ7ZS3K5GM0187M already owns opt-in live PostgreSQL integration coverage, so this ticket should stay focused on strategy implementation, registration, local tests, and documentation.
- Current repository docs and tests still describe PostgreSQL as compatibility-only fallback, including README.md, docs/architecture/dvault-v1-explicit-save-service.md, and tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs; those statements become part of this ticket's update surface.

Scope In
- Implement a PostgreSQL-specific IDataVaultProviderSaveStrategy in src/DCoding.Data.DVault.Postgres and register it from AddDVaultPostgres().
- Use PostgreSQL-suitable set-based hub, link, and satellite write behavior behind the existing explicit save-service and provider-strategy contracts.
- Preserve provider-neutral fallback behavior by declining unsupported contexts or request shapes instead of changing the caller API.
- Update local tests and repository documentation that currently assert PostgreSQL is fallback-only.

Scope Out
- Opt-in live PostgreSQL integration tests and environment-gated execution belong to sibling task 06EZ0NA7CWDYJ7ZS3K5GM0187M.
- Benchmark harness expansion or PostgreSQL benchmark evidence is not required for this task's implementation contract.
- Changes to the public IDataVaultSaveService request/response API or provider-neutral capability contracts are out of scope.
- Provider-specific optimization work for SQLite, SQL Server, Oracle, or MySQL is out of scope.

Open questions
- none

Follow-up questions
- Story 06EZ0N9TJSXFXH0YZRA3QN2S14 still mentions benchmark evidence, but the current repository benchmark harness is SQLite-only; decide separately whether that story-level benchmark requirement should become its own follow-up ticket instead of expanding this implementation task.

Risks
- If the PostgreSQL strategy does not preserve the existing bulk and chronological satellite hash-diff behavior from the fallback and SQLite paths, repeated or out-of-order batches can regress silently.
- If README and architecture guidance are not updated with the code change, the repository will continue to advertise PostgreSQL as fallback-only and mislead downstream implementers.
- Live PostgreSQL execution semantics are intentionally not proven by this ticket alone; that risk is mitigated only when sibling ticket 06EZ0NA7CWDYJ7ZS3K5GM0187M lands.

Split recommendations
- No additional split is needed for this task; repository ticket data already splits opt-in PostgreSQL integration coverage into sibling ticket 06EZ0NA7CWDYJ7ZS3K5GM0187M.
- If story-level benchmark evidence remains required for 06EZ0N9TJSXFXH0YZRA3QN2S14, track it as a separate follow-up task rather than expanding this implementation ticket or the integration-coverage sibling.

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