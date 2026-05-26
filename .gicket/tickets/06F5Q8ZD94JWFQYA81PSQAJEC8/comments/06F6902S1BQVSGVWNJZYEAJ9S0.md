[gicket-bot] PO refinement contract

Summary
- Refined as an additive AddDVaultPostgres optimization story: stage large eligible PostgreSQL explicit save batches with native transfer while preserving existing save semantics, fallback behavior, and opt-in Postgres evidence lanes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This story is an internal optimization of the existing AddDVaultPostgres()/PostgresDataVaultSaveStrategy boundary, not a new public IDataVaultSaveService API.
- The current repository baseline already has a PostgreSQL optimized path for clean Npgsql contexts using set-based insert/reuse plus latest-state satellite checks; this story adds a staged bulk branch for larger eligible batches instead of reopening that baseline.
- The proof surfaces already exist: Postgres opt-in integration coverage is gated by DVAULT_TEST_POSTGRES_CONNECTION_STRING, and the existing provider-native-bulk-ingestion benchmark row for dvault-adddvaultpostgres-optimized is the benchmark evidence target.
- The core repository already exposes staged-provider bulk diagnostics vocabulary, so PostgreSQL staged-path eligibility, decline, fallback, or cleanup reporting should reuse that existing bounded taxonomy instead of inventing a new one.

Scope In
- Add a PostgreSQL staged bulk branch inside the existing Postgres provider strategy for large eligible explicit save batches on clean Npgsql-backed DbContext instances.
- Create and populate PostgreSQL transient staging structures, use COPY or another demonstrably provider-native bulk transfer into the stage, and apply staged rows to hub, link, and ordinary satellite targets.
- Preserve current latest-state satellite hash-diff filtering, idempotent hub/link reuse behavior, saved-record ordering, and caller-visible row-count semantics.
- Extend PostgreSQL-gated integration coverage and the existing provider-native benchmark evidence so the staged path is exercised when Postgres is configured.
- Update current documentation and benchmark execution-detail wording needed to describe the landed PostgreSQL optimized boundary accurately.

Scope Out
- Any new public save-service overloads or changes to DataVaultSaveRequest, DataVaultBulkSaveRequest, or DataVaultChunkedSaveRequest.
- Provider-native chunk execution or any change to the current chunked-save public contract documented for v0.19.0.
- New benchmark artifact schemas or new benchmark scenario families; the existing provider-native-bulk-ingestion PostgreSQL baseline remains the evidence surface.
- Cross-provider optimization redesign for SQLite, SQL Server, MySQL, or Oracle beyond small shared refactors strictly required to support the PostgreSQL implementation.
- Database provisioning, CI secret management, Docker setup, or always-on live Postgres infrastructure.

Open questions
- none

Follow-up questions
- Which upcoming coordinated release note should become the first public claim set that advertises PostgreSQL staged bulk as a documented provider optimization beyond the historical v0.19.0 baseline?

Risks
- A PostgreSQL staged path may need provider-specific runtime hooks not used by the current Postgres package path, which increases implementation complexity and can create packaging or dependency tradeoffs if not kept bounded.
- Temporary staging cleanup and transaction participation are the main regression areas; failures there could leave transient artifacts or cause unintended fallback unless explicitly covered by tests.
- The current PostgreSQL optimized path already has a non-staged set-based implementation, so applying staging too broadly can regress small or medium batches if eligibility remains poorly tuned.
- PostgreSQL performance and live integration proof are external-opt-in, so benchmark and integration evidence can remain skipped on machines without DVAULT_TEST_POSTGRES_CONNECTION_STRING; the skipped-row contract must stay visible instead of silently dropping evidence.

Split recommendations
- If provider-specific runtime dependency or packaging-policy work grows beyond the PostgreSQL write path itself, split that dependency-policy decision from the staged-bulk behavior and evidence work.
- If broader multi-provider staged-bulk diagnostics alignment becomes necessary, keep this story focused on PostgreSQL behavior and proof and move cross-provider diagnostics symmetry into a separate follow-up ticket.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment