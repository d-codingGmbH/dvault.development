[gicket-bot] PO refinement contract

Summary
- Refined Oracle validation to a Postgres-style external opt-in smoke path with `DVAULT_TEST_ORACLE_CONNECTION_STRING`, one representative hub-save check, and documentation, while keeping optimized-writer work out of scope.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Oracle should follow the repository's existing external-provider test pattern: one developer-managed connection-string environment variable, skipped-by-default live tests, and `ProviderIntegration.ExternalOptIn` for the configured database path.
- The v1 configuration contract for this ticket is `DVAULT_TEST_ORACLE_CONNECTION_STRING`; missing or blank values mean the live Oracle smoke path is not configured.
- `AddDVaultOracle()` currently exposes the Oracle compatibility baseline by delegating to `AddDVault()`, so this ticket's live smoke must verify observable behavior through the public `IDataVaultSaveService` contract rather than depend on the absence or presence of a provider-specific strategy.
- Existing default Oracle provider-registration smoke coverage remains part of normal local validation; the new live insert-path validation is opt-in and must not make Oracle a default developer prerequisite.
- The bounded v1 save scenario for this ticket is one representative insert-only hub save; broader link, satellite, reuse, concurrency, or performance scenarios belong to separate Oracle tickets.

Scope In
- Add Oracle external-test configuration plumbing in the integration test project, following the current Postgres-style opt-in pattern.
- Add default-run configuration-contract coverage that proves missing Oracle configuration is treated as a documented skip condition instead of a noisy failure.
- Add one opt-in live Oracle smoke test that starts DVault through `AddDVaultOracle()` and persists a single representative hub row through `IDataVaultSaveService`.
- Document the Oracle opt-in command shape, environment variable name, and external database prerequisite for maintainers.
- Keep Oracle test category and provider trait discovery aligned with the repository's existing provider smoke and external-integration conventions.

Scope Out
- Implementing an Oracle provider capability profile or optimized save strategy.
- Adding multi-scenario Oracle coverage for links, satellites, reuse/idempotency, concurrency, or benchmarks.
- Adding checked-in secrets, Docker images, CI infrastructure, or repository-managed Oracle provisioning.
- Making the packable `src/DCoding.Data.DVault.Oracle` project depend directly on a concrete Oracle EF Core provider package.
- Expanding shared provider contracts beyond what is needed for the Oracle configuration path and one live smoke save.

Open questions
- none

Follow-up questions
- If Oracle optimized-writer work lands later, should the live Oracle smoke suite gain one explicit strategy-selection assertion or remain purely public-contract smoke coverage?
- Should SQL Server, Oracle, and MySQL all standardize on the same `DVAULT_TEST_<PROVIDER>_CONNECTION_STRING` naming pattern in contributor-facing documentation?
- Is there later value in an optional nightly or release-time external-provider smoke run once maintainers have stable Oracle access?

Risks
- The first Oracle opt-in harness may expose provider-package acquisition, target-framework compatibility, or local setup friction because the repository currently has no Oracle external-fixture baseline.
- Oracle object-creation and cleanup behavior may be more brittle than the existing SQLite and Postgres paths if the configured user lacks the expected privileges.
- If the live smoke test overfits current fallback internals instead of observable save behavior, it will conflict with later Oracle optimized-writer work.

Split recommendations
- No split recommended; provider capability registration and optimized writer work already belong to sibling Oracle tickets, and this ticket stays bounded to opt-in validation configuration, one live save smoke, and documentation.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment