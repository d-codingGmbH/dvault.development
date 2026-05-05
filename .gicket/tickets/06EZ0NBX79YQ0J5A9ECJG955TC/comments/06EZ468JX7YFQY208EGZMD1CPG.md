[gicket-bot] PO refinement contract

Summary
- Refined 06EZ0NBX79YQ0J5A9ECJG955TC against the existing provider-optimization architecture: MySQL is currently compatibility-only through `AddDVaultMySql()`, no child tickets or planning documents were materialized, and the ticket is ready for PO critic with bounded scope around MySQL capability-profile wiring plus a MySQL-local optimized writer that falls back cleanly.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence shows `src/DCoding.Data.DVault.MySql` currently contains only `AddDVaultMySql()` and registers no `IDataVaultProviderSaveStrategy`; this ticket upgrades that package from compatibility-only registration to a MySQL-specific capability-profile and optimized-writer implementation.
- The shared boundaries are already fixed by `docs/architecture/dvault-v1-explicit-save-service.md`: core owns `DataVaultProviderCapabilityProfile`, `IDataVaultProviderSaveStrategy`, deterministic dispatch, and provider-neutral fallback, while MySQL-specific SQL must stay inside `src/DCoding.Data.DVault.MySql`.
- The current logical-property baseline is already finite in `DataVaultLogicalPropertyKind`; this ticket should map the existing kinds for MySQL and keep SQL-function and concurrency declarations at the current `NoneInV1Unsupported` baseline unless a narrowly justified additive change is required by proven MySQL behavior.
- The ticket remains a child of 06EZ0NBPWEWAP264B4XP36CXC8; no new child tickets, relations, attachments, or planning documents were created during this refinement pass.

Scope In
- Add a MySQL provider capability profile under the shared provider contract and wire the MySQL path to use it for provider-aware model translation.
- Add MySQL optimized save-strategy registration and implementation inside `src/DCoding.Data.DVault.MySql` using the existing `IDataVaultProviderSaveStrategy` boundary.
- Add automated coverage for MySQL registration, capability-profile completeness, optimized-path selection, and provider-neutral fallback behavior.
- Update affected docs, XML comments, and API/snapshot expectations if the MySQL provider baseline changes visible behavior or public surface.

Scope Out
- New logical property kinds, new concurrency signals, merge/upsert semantics, retry semantics, or broader multi-writer conflict handling.
- Benchmark parity with SQLite or required local MySQL integration infrastructure.
- Optimized writers for PostgreSQL, SQL Server, or Oracle.
- Changing the explicit caller contract around `IDataVaultSaveService`, `DataVaultSaveRequest`, or `SaveChanges` interception.
- A broad provider-neutral redesign beyond the minimum additive hook needed for provider-specific capability-profile selection.

Open questions
- none

Follow-up questions
- After MySQL lands, should the same provider-capability selection hook be rolled into the remaining compatibility-only provider packages before they gain optimized writers?
- Should MySQL later gain benchmark coverage or a required local integration baseline, or remain external opt-in like the current Postgres provider integration pattern?
- If downstream users need multiple EF Core MySQL provider implementations verified explicitly, should that be scheduled as a separate follow-up ticket instead of widening this task?

Risks
- MySQL native type and precision choices must preserve the existing explicit UTC load-timestamp contract; a poor mapping can create round-trip or ordering regressions.
- If no external MySQL integration coverage is added, unit-tested strategy code can still miss runtime dialect or provider-detection differences.
- Provider-capability selection currently defaults to SQLite in shared translation code, so widening that path for MySQL creates a regression risk unless the change stays strictly additive.

Split recommendations
- No split is recommended from current evidence; capability-profile wiring, MySQL-local optimized writer behavior, and the associated coverage all sit on the same architectural seam and remain a single bounded provider task.

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