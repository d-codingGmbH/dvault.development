<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Amended the contract to ratify `Pomelo.EntityFrameworkCore.MySql` as the single v1 EF Core MySQL baseline, require MySQL activation through the existing `ApplyDataVaultMetadata(...)` call path after `AddDVaultMySql()`, and mark live MySQL SQL contract tests out of scope for completion; no child tickets, relations, attachments, or planning documents were created in this pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence still shows `src/DCoding.Data.DVault.MySql` contains only `AddDVaultMySql()` and no provider-specific save strategy; this ticket upgrades that package from compatibility-only registration to Pomelo-targeted capability-profile and optimized-writer support.
- `Pomelo.EntityFrameworkCore.MySql` is the single supported external EF Core MySQL provider baseline for this ticket. Other EF Core MySQL providers remain out of scope and must not be treated as compatible by the v1 MySQL path.
- The caller-visible model-building contract stays on the existing `ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel)` API. After the app configures the Pomelo provider and calls `AddDVaultMySql()`, that same call path must emit the MySQL capability profile automatically rather than requiring a new public MySQL-specific model-builder hook.
- Live MySQL SQL contract tests are not required for ticket completion; unit, snapshot, registration, dispatch, and fallback coverage is sufficient. If optional live MySQL tests are added, they must use the repository's external opt-in pattern and skip cleanly when configuration is absent.
- The ticket remains a child of `06EZ0NBPWEWAP264B4XP36CXC8`; no new child tickets, relations, attachments, or planning documents were created during this refinement pass.

### Scope In
- Name `Pomelo.EntityFrameworkCore.MySql` as the v1 compatible EF Core MySQL provider baseline for MySQL capability-profile selection and optimized save-strategy dispatch.
- Make the existing `ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel)` caller path pick up the MySQL capability profile automatically when the application is configured for Pomelo and calls `AddDVaultMySql()`.
- Register a MySQL `IDataVaultProviderSaveStrategy` inside `src/DCoding.Data.DVault.MySql` and keep all MySQL-specific SQL inside that provider project.
- Add automated registration, capability-profile completeness, optimized-path selection, and provider-neutral fallback coverage for the bounded Pomelo baseline.

### Scope Out
- Support for `MySql.EntityFrameworkCore` or any other EF Core MySQL provider beyond `Pomelo.EntityFrameworkCore.MySql`.
- Required live MySQL database contract tests, Docker/database provisioning, or any mandatory local MySQL prerequisite for ticket completion.
- New logical property kinds, new concurrency signals, merge/upsert semantics, retry semantics, or broader multi-writer conflict handling.
- A new required public model-building hook or overload for MySQL activation.
- Optimized writers for PostgreSQL, SQL Server, or Oracle, or a broad provider-neutral redesign beyond the minimum additive wiring needed to preserve the current caller experience.

## Acceptance Criteria
- With `Pomelo.EntityFrameworkCore.MySql` configured and `AddDVaultMySql()` registered, the existing `ApplyDataVaultMetadata(ModelBuilder, DataVaultMetadataModel)` call path uses a MySQL capability profile instead of the current SQLite-only default without requiring callers to switch to a new public model-building hook.
- The MySQL capability profile declares mappings for every current `DataVaultLogicalPropertyKind` and preserves the existing annotation pattern for provider profile name, logical property kind, native store type, and value format.
- `AddDVaultMySql()` registers a MySQL `IDataVaultProviderSaveStrategy` in `src/DCoding.Data.DVault.MySql`, and core dispatch selects it only when the current `DbContext`, ordered request batch, and active EF Core provider are compatible with the Pomelo baseline.
- All MySQL-specific SQL required by the optimized path lives in the MySQL provider project; the core package does not embed MySQL SQL text or execute MySQL-specific branches to perform the optimized write.
- When the active provider is not the supported Pomelo baseline or the request/context shape is otherwise unsafe, `CanSave` declines and the existing provider-neutral fallback writer persists the request without changing the public save contract.
- Ticket completion requires automated unit, snapshot, registration, capability-profile completeness, dispatch, and fallback coverage; live MySQL SQL contract tests are optional and not required for this ticket.

## Definition of Done
- Core and MySQL implementation changes follow the existing repository layout, package boundaries, and one-member-per-file policy.
- Affected unit, snapshot, package-verification, and integration tests for the bounded Pomelo baseline are updated and passing; no required local MySQL database prerequisite is introduced.
- Documentation and comments that currently describe MySQL as compatibility-only are updated where the implemented behavior changes that statement, including the named Pomelo baseline and the preserved `ApplyDataVaultMetadata(...)` caller experience.
- No MySQL-specific SQL or provider-specific persistence behavior is introduced outside `src/DCoding.Data.DVault.MySql`; any optional live MySQL tests skip cleanly when their external opt-in configuration is absent.

## Implementation Notes
- Reuse the existing dispatcher semantics in `DefaultDataVaultSaveService`: strategies are evaluated by descending `Priority`, equal priorities keep DI registration order, and the core fallback remains the terminal path when no strategy accepts.
- Keep the MySQL optimized writer boundary shaped like the existing SQLite strategy and shared `ProviderSqlExecutionContract`: parameterized SQL, participation in the current transaction, cancellation propagation, and `CanSave` rejection when pending tracked EF changes make the optimized path unsafe.
- Do not broaden provider detection beyond the single Pomelo baseline in this ticket; unsupported EF Core MySQL providers should remain follow-up work rather than implicit compatibility.
- Preserve the existing public caller surface on `ApplyDataVaultMetadata(...)`; internal or provider-package wiring is acceptable if needed to make that existing call path choose the MySQL capability profile after `AddDVaultMySql()` and Pomelo provider configuration.
- If developers opportunistically add live MySQL SQL contract coverage, it must mirror the repository's external opt-in provider-test pattern and remain non-blocking for ticket completion.

## Open Questions
- none

## Follow-Up Questions
- Should a separate follow-up ticket add explicit support for additional EF Core MySQL providers such as `MySql.EntityFrameworkCore`?
- Should optional live MySQL SQL contract tests be added later with a dedicated opt-in environment-variable contract mirroring the existing Postgres pattern?
- After MySQL lands, should the same provider-capability selection hook be rolled into the remaining compatibility-only provider packages before they gain optimized writers?

## Risks
- Because live MySQL SQL contract tests are not required here, unit-tested strategy code can still miss runtime Pomelo/MySQL dialect or provider-detection differences until optional follow-up validation is added.
- MySQL native type and precision choices must preserve the existing explicit UTC load-timestamp contract; a poor mapping can create round-trip or ordering regressions.
- Provider-capability selection currently defaults to SQLite in shared translation code, so widening that path for automatic MySQL activation carries regression risk unless the change stays strictly additive and well covered.

## Split Recommendations
- No split is recommended from current evidence; the Pomelo baseline decision, preserved caller activation contract, MySQL-local optimized writer behavior, and bounded automated coverage remain one architectural seam.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: add MySQL provider capability registration and optimized writer boundary implementation.

Acceptance Criteria:
- MySQL capabilities are registered through the shared provider contract.
- MySQL-specific SQL stays inside the MySQL provider project.
- Unsupported write cases route to fallback behavior.