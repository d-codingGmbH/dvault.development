[gicket-bot] PO refinement contract

Summary
- Refined the task to a provider-package-only SQL Server optimized save implementation behind the existing strategy boundary, with fallback preservation and repeatable live smoke coverage kept in the sibling SQL Server coverage ticket.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This task owns the SQL Server optimized writer implementation itself; the separate repeatable opt-in SQL Server smoke suite already has its own child ticket 06EZ0NAWNDDEP32P497E39MQXR.
- The optimized path must remain behind IDataVaultProviderSaveStrategy and AddDVaultSqlServer and must not add SQL Server SQL or provider-name branching to src/DCoding.Data.DVault.
- The strategy should accept only compatible SQL Server DbContext instances with a clean change tracker and supported request/model shapes; all other cases must fall back through the existing provider-neutral save service.
- No configurable batch-size threshold or required SQL Server feature is mandated here; any parameterized set-based insert-only approach is acceptable if it removes fallback-style per-row unique-row existence probes for the optimized path.
- Satellite handling must preserve the current insert-only semantics already visible in DefaultDataVaultSaveService and SqliteDataVaultSaveStrategy: compare the latest known hash diff per parent across the ordered batch and insert only when the hash diff changes.

Scope In
- Implement a SQL Server-specific IDataVaultProviderSaveStrategy inside src/DCoding.Data.DVault.SqlServer.
- Register the strategy from AddDVaultSqlServer while preserving the existing AddDVault baseline.
- Use SQL Server-appropriate set-based SQL for hub and link reuse detection and insert-only writes on the optimized path.
- Use batch-oriented latest-hash-diff lookup and insert filtering for satellite rows without changing caller-visible save ordering or row-count semantics.
- Add or update the minimal non-live tests needed to prove registration, compatibility gating, and fallback preservation for the new SQL Server strategy.

Scope Out
- The repeatable opt-in SQL Server smoke suite and configuration contract described by ticket 06EZ0NAWNDDEP32P497E39MQXR.
- Always-on live SQL Server test infrastructure, CI secrets, containers, or local provisioning automation.
- Changes to IDataVaultSaveService, stable hashing, naming policy, or provider-neutral EF metadata translation beyond what is strictly required to plug into the existing save-strategy boundary.
- Provider-specific SQL or provider-name switches in src/DCoding.Data.DVault.
- Optimized strategy work for PostgreSQL, Oracle, or MySQL.

Open questions
- none

Follow-up questions
- Should the follow-on SQL Server smoke ticket adopt a Postgres-like environment contract, for example a DVAULT_TEST_SQLSERVER_CONNECTION_STRING-style opt-in variable?
- Once the SQL Server optimized path lands, should docs/architecture/dvault-v1-explicit-save-service.md be updated in the same delivery stream to move SQL Server out of the current compatibility-only matrix row?
- If PostgreSQL, Oracle, and MySQL later gain optimized writers, should the repository add a shared provider SQL execution contract helper analogous to the current SQLite-focused contract coverage?

Risks
- An implementation that still loops over rows and executes per-row existence checks behind raw SQL would satisfy wiring but miss the actual performance objective.
- SQL Server-specific SQL can drift into update or upsert semantics that break the explicit insert-only contract for hub, link, or satellite history.
- Overly broad CanSave gating could route dirty contexts or unsupported model shapes into the optimized path and bypass the known-safe fallback.
- Because this ticket does not own the repeatable live SQL Server smoke suite, SQL text that looks correct in isolation may not be exercised against a real SQL Server instance until the follow-on coverage work lands.

Split recommendations
- Keep repeatable opt-in SQL Server smoke/live validation in ticket 06EZ0NAWNDDEP32P497E39MQXR so this ticket stays focused on provider-package implementation and fallback-safe strategy wiring.
- If documentation or validation work expands beyond brief expectation updates, keep that work with the parent SQL Server optimization story rather than enlarging this implementation ticket.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment