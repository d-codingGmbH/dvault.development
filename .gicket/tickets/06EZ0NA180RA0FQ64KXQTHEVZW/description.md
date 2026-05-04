<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this implementation task against current repo and ticket evidence; no new child tickets or planning docs were materialized because story 06EZ0N9TJSXFXH0YZRA3QN2S14 already has sibling task 06EZ0NA7CWDYJ7ZS3K5GM0187M for opt-in PostgreSQL integration coverage.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence shows src/DCoding.Data.DVault.Postgres currently contains only AddDVaultPostgres() calling services.AddDVault() and no PostgreSQL-specific save strategy implementation.
- This ticket is already a child of story 06EZ0N9TJSXFXH0YZRA3QN2S14 (Optimize PostgreSQL provider save strategy).
- Sibling task 06EZ0NA7CWDYJ7ZS3K5GM0187M already owns opt-in live PostgreSQL integration coverage, so this ticket should stay focused on strategy implementation, registration, local tests, and documentation.
- Current repository docs and tests still describe PostgreSQL as compatibility-only fallback, including README.md, docs/architecture/dvault-v1-explicit-save-service.md, and tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs; those statements become part of this ticket's update surface.

### Scope In
- Implement a PostgreSQL-specific IDataVaultProviderSaveStrategy in src/DCoding.Data.DVault.Postgres and register it from AddDVaultPostgres().
- Use PostgreSQL-suitable set-based hub, link, and satellite write behavior behind the existing explicit save-service and provider-strategy contracts.
- Preserve provider-neutral fallback behavior by declining unsupported contexts or request shapes instead of changing the caller API.
- Update local tests and repository documentation that currently assert PostgreSQL is fallback-only.

### Scope Out
- Opt-in live PostgreSQL integration tests and environment-gated execution belong to sibling task 06EZ0NA7CWDYJ7ZS3K5GM0187M.
- Benchmark harness expansion or PostgreSQL benchmark evidence is not required for this task's implementation contract.
- Changes to the public IDataVaultSaveService request/response API or provider-neutral capability contracts are out of scope.
- Provider-specific optimization work for SQLite, SQL Server, Oracle, or MySQL is out of scope.

## Acceptance Criteria
- AddDVaultPostgres() registers a PostgreSQL-specific IDataVaultProviderSaveStrategy while continuing to provide the existing IDataVaultSaveService fallback path.
- For compatible PostgreSQL/Npgsql DbContext instances with no pending tracked EF changes, hub and link saves use set-based PostgreSQL insert/reuse semantics so repeated requests do not create duplicate rows and RowsWritten counts only inserted rows.
- Satellite saves use PostgreSQL-suitable set-based latest-state checks by parent hash key and hash diff so unchanged payload replays insert no duplicate row while changed payloads append new insert-only history rows.
- When the PostgreSQL strategy cannot safely handle the current context or request batch, it declines through CanSave and the provider-neutral save service handles the request without provider-specific surprises.
- Local repository tests and documentation are updated to reflect PostgreSQL optimized registration, while live PostgreSQL execution verification remains explicitly split to ticket 06EZ0NA7CWDYJ7ZS3K5GM0187M.

## Definition of Done
- src/DCoding.Data.DVault.Postgres contains the optimized strategy implementation and AddDVaultPostgres() no longer behaves as a compatibility-only registration surface.
- Local test coverage is updated for the changed Postgres registration and dispatch expectations, and the default local test suite remains runnable without PostgreSQL installed.
- Repository documentation that currently says PostgreSQL falls back until a future writer exists is updated consistently across README and architecture notes.
- The implementation preserves the existing explicit save-service boundary and does not require new caller-facing APIs or workflow metadata decisions.

## Implementation Notes
- Follow the existing provider boundary in src/DCoding.Data.DVault: keep the strategy behind IDataVaultProviderSaveStrategy and reuse DataVaultProviderSaveStrategyContext, naming policy, stable hash normalizer, and stable hash service rather than introducing a PostgreSQL-only request shape.
- Mirror the SQLite strategy guardrails: CanSave should accept only clean PostgreSQL/Npgsql contexts and decline contexts with pending tracked changes so the provider-neutral fallback remains valid.
- Use parameterized raw ADO.NET/DbConnection commands that participate in the current EF transaction and observe cancellation, rather than relying on SaveChanges interception or string-concatenated SQL.
- Preserve the existing satellite history semantics already exercised by the SQLite baseline and fixed by docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md, even though live PostgreSQL verification is handled by sibling ticket 06EZ0NA7CWDYJ7ZS3K5GM0187M.
- Update the repository guidance that still says PostgreSQL is compatibility-only, especially README.md and docs/architecture/dvault-v1-explicit-save-service.md.

## Open Questions
- none

## Follow-Up Questions
- Story 06EZ0N9TJSXFXH0YZRA3QN2S14 still mentions benchmark evidence, but the current repository benchmark harness is SQLite-only; decide separately whether that story-level benchmark requirement should become its own follow-up ticket instead of expanding this implementation task.

## Risks
- If the PostgreSQL strategy does not preserve the existing bulk and chronological satellite hash-diff behavior from the fallback and SQLite paths, repeated or out-of-order batches can regress silently.
- If README and architecture guidance are not updated with the code change, the repository will continue to advertise PostgreSQL as fallback-only and mislead downstream implementers.
- Live PostgreSQL execution semantics are intentionally not proven by this ticket alone; that risk is mitigated only when sibling ticket 06EZ0NA7CWDYJ7ZS3K5GM0187M lands.

## Split Recommendations
- No additional split is needed for this task; repository ticket data already splits opt-in PostgreSQL integration coverage into sibling ticket 06EZ0NA7CWDYJ7ZS3K5GM0187M.
- If story-level benchmark evidence remains required for 06EZ0N9TJSXFXH0YZRA3QN2S14, track it as a separate follow-up task rather than expanding this implementation ticket or the integration-coverage sibling.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: implement the PostgreSQL optimized write strategy behind the provider capability contract.

Acceptance Criteria:
- Hub, link, and satellite write paths use set-based operations suitable for PostgreSQL.
- The strategy handles unchanged satellites without duplicate inserts and changed satellites with insert-only history semantics.
- Unsupported cases fall back instead of throwing provider-specific surprises.