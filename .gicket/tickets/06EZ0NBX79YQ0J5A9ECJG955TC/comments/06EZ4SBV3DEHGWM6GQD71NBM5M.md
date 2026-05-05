[gicket-bot] PO refinement contract

Summary
- Amended the contract to ratify `Pomelo.EntityFrameworkCore.MySql` as the single v1 EF Core MySQL baseline, require MySQL activation through the existing `ApplyDataVaultMetadata(...)` call path after `AddDVaultMySql()`, and mark live MySQL SQL contract tests out of scope for completion; no child tickets, relations, attachments, or planning documents were created in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract is amended to treat `Pomelo.EntityFrameworkCore.MySql` as the single supported EF Core MySQL provider baseline for this ticket. The MySQL capability profile and optimized writer only need to claim compatibility for that provider; other EF Core MySQL providers stay out of scope and must not be treated as compatible in v1.
- critic-item-2: `answered` - The caller-visible activation contract stays on the existing `ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel)` API. After the application configures the Pomelo provider and calls `AddDVaultMySql()`, that same model-building call path must select the MySQL capability profile automatically; a new required public MySQL model-building hook or overload is not allowed for this ticket.
- critic-item-3: `answered` - Live MySQL SQL contract tests are out of scope for completion of this ticket. Required evidence is unit, snapshot, registration, capability-profile completeness, dispatch, and fallback coverage; any optional live MySQL SQL tests must remain external opt-in and skip when configuration is absent.
- critic-item-4: `answered` - The blocking provider-baseline gap is resolved by naming `Pomelo.EntityFrameworkCore.MySql` as the single v1 compatible EF Core MySQL provider for `IDataVaultProviderSaveStrategy.CanSave`, MySQL capability-profile activation, and any optional future live contract testing in this area. Other EF Core MySQL providers are explicitly out of scope for this ticket.
- critic-item-5: `answered` - The blocking activation-contract gap is resolved by requiring MySQL activation to work through the existing `ApplyDataVaultMetadata(...)` caller experience after `AddDVaultMySql()` and Pomelo provider configuration. Internal or provider-package wiring may be added if needed, but a new required caller-visible MySQL-specific hook is not part of this ticket.

Clarifications
- Repository evidence still shows `src/DCoding.Data.DVault.MySql` contains only `AddDVaultMySql()` and no provider-specific save strategy; this ticket upgrades that package from compatibility-only registration to Pomelo-targeted capability-profile and optimized-writer support.
- `Pomelo.EntityFrameworkCore.MySql` is the single supported external EF Core MySQL provider baseline for this ticket. Other EF Core MySQL providers remain out of scope and must not be treated as compatible by the v1 MySQL path.
- The caller-visible model-building contract stays on the existing `ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel)` API. After the app configures the Pomelo provider and calls `AddDVaultMySql()`, that same call path must emit the MySQL capability profile automatically rather than requiring a new public MySQL-specific model-builder hook.
- Live MySQL SQL contract tests are not required for ticket completion; unit, snapshot, registration, dispatch, and fallback coverage is sufficient. If optional live MySQL tests are added, they must use the repository's external opt-in pattern and skip cleanly when configuration is absent.
- The ticket remains a child of `06EZ0NBPWEWAP264B4XP36CXC8`; no new child tickets, relations, attachments, or planning documents were created during this refinement pass.

Scope In
- Name `Pomelo.EntityFrameworkCore.MySql` as the v1 compatible EF Core MySQL provider baseline for MySQL capability-profile selection and optimized save-strategy dispatch.
- Make the existing `ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel)` caller path pick up the MySQL capability profile automatically when the application is configured for Pomelo and calls `AddDVaultMySql()`.
- Register a MySQL `IDataVaultProviderSaveStrategy` inside `src/DCoding.Data.DVault.MySql` and keep all MySQL-specific SQL inside that provider project.
- Add automated registration, capability-profile completeness, optimized-path selection, and provider-neutral fallback coverage for the bounded Pomelo baseline.

Scope Out
- Support for `MySql.EntityFrameworkCore` or any other EF Core MySQL provider beyond `Pomelo.EntityFrameworkCore.MySql`.
- Required live MySQL database contract tests, Docker/database provisioning, or any mandatory local MySQL prerequisite for ticket completion.
- New logical property kinds, new concurrency signals, merge/upsert semantics, retry semantics, or broader multi-writer conflict handling.
- A new required public model-building hook or overload for MySQL activation.
- Optimized writers for PostgreSQL, SQL Server, or Oracle, or a broad provider-neutral redesign beyond the minimum additive wiring needed to preserve the current caller experience.

Open questions
- none

Follow-up questions
- Should a separate follow-up ticket add explicit support for additional EF Core MySQL providers such as `MySql.EntityFrameworkCore`?
- Should optional live MySQL SQL contract tests be added later with a dedicated opt-in environment-variable contract mirroring the existing Postgres pattern?
- After MySQL lands, should the same provider-capability selection hook be rolled into the remaining compatibility-only provider packages before they gain optimized writers?

Risks
- Because live MySQL SQL contract tests are not required here, unit-tested strategy code can still miss runtime Pomelo/MySQL dialect or provider-detection differences until optional follow-up validation is added.
- MySQL native type and precision choices must preserve the existing explicit UTC load-timestamp contract; a poor mapping can create round-trip or ordering regressions.
- Provider-capability selection currently defaults to SQLite in shared translation code, so widening that path for automatic MySQL activation carries regression risk unless the change stays strictly additive and well covered.

Split recommendations
- No split is recommended from current evidence; the Pomelo baseline decision, preserved caller activation contract, MySQL-local optimized writer behavior, and bounded automated coverage remain one architectural seam.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment