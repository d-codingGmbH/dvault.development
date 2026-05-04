[gicket-bot] PO-critic review contract

Summary
- Return to PO: the task still depends on an unbounded PostgreSQL optimized-path dependency and does not define how tests will prove optimized execution instead of fallback.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NA7CWDYJ7ZS3K5GM0187M/description.md:14-24 and 56-62 says this task targets a PostgreSQL optimized path owned by a parent story, scopes strategy implementation out, and acknowledges the tests may not go green if that path is unavailable.
- .gicket/tickets/06EZ0NA7CWDYJ7ZS3K5GM0187M/description.md:49-50 records `## Open Questions` as `- none`.
- src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:14-19 shows `AddDVaultPostgres()` currently only calls `services.AddDVault()`.
- tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:42-53 explicitly expects `AddDVaultPostgres()` to register no `IDataVaultProviderSaveStrategy`, while `AddDVaultSqlite()` does register one.
- docs/architecture/dvault-v1-explicit-save-service.md:39-50 marks PostgreSQL as a v0.5 compatibility baseline with no provider-specific save strategy required and only `ProviderIntegration.ExternalOptIn` validation.
- tests/DCoding.Data.DVault.Tests/Integration/PostgresDataVaultSchemaTests.cs:8-40, tests/DCoding.Data.DVault.Tests/Shared/ProviderTestCategories.cs:7-12, and tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:16-18 show the existing opt-in Postgres harness already has the `ProviderIntegration.ExternalOptIn` trait, skip-on-missing-config behavior, per-run schema isolation, and a conditional Npgsql package reference.
- `git show --stat d39451c8e0fd` shows the PO->PO-critic handoff changed only `.gicket/tickets/06EZ0NA7CWDYJ7ZS3K5GM0187M` ticket metadata/comments/description, and `git rev-parse HEAD` shows current HEAD `57ef924ecec2d666734444dee0272eecf0cbb42a` is the po-critic claim commit.

Blocking findings
- The ticket requires green behavior tests for a PostgreSQL optimized save path, but the current repository baseline is still compatibility-only and the contract explicitly scopes strategy implementation out. Without an explicit dependency/sequence rule to the parent optimization work, this task is not independently developer-ready.
- The contract does not define what observable proves the PostgreSQL optimized strategy ran instead of the provider-neutral fallback. Existing repo evidence shows fallback and optimized paths share the same caller contract, so persisted rows or `RowsWritten` alone do not prove optimized-path execution.

Required PO actions
- Name and link the exact parent PostgreSQL optimization ticket in this task, and state whether this task is blocked on that ticket, must ship in the same delivery unit, or should be resequenced after it.
- Refine acceptance criteria / DoD so the tests must directly prove optimized-path selection rather than only persisted behavior.
- Resolve the scope overlap between this task and story `06EZ0N9TJSXFXH0YZRA3QN2S14`, which already claims opt-in integration coverage for the PostgreSQL optimized path.

Open issues ledger
- critic-item-1 [required-po-action] Name and link the exact parent PostgreSQL optimization ticket in this task, and state whether this task is blocked on that ticket, must ship in the same delivery unit, or should be resequenced after it.
- critic-item-2 [required-po-action] Refine acceptance criteria / DoD so the tests must directly prove optimized-path selection rather than only persisted behavior.
- critic-item-3 [required-po-action] Resolve the scope overlap between this task and story `06EZ0N9TJSXFXH0YZRA3QN2S14`, which already claims opt-in integration coverage for the PostgreSQL optimized path.
- critic-item-4 [blocking-finding] The ticket requires green behavior tests for a PostgreSQL optimized save path, but the current repository baseline is still compatibility-only and the contract explicitly scopes strategy implementation out. Without an explicit dependency/sequence rule to the parent optimization work, this task is not independently developer-ready.
- critic-item-5 [blocking-finding] The contract does not define what observable proves the PostgreSQL optimized strategy ran instead of the provider-neutral fallback. Existing repo evidence shows fallback and optimized paths share the same caller contract, so persisted rows or `RowsWritten` alone do not prove optimized-path execution.

Missing examples / edge cases
- The contract does not say what should happen when PostgreSQL is configured and Npgsql is available, but no PostgreSQL optimized strategy is actually registered yet: skip, fail, or block on the parent story.
- The contract does not say whether proving optimized-path use requires a dedicated observable like strategy registration / no tracked fallback entries, or whether some other signal is expected.
- The contract mentions flaky-environment risk but does not make per-run isolation expectations explicit for the new save-behavior tests, even though the existing Postgres schema test uses random schema isolation.

Risky assumptions
- Assumes `minimal test-only wiring` can exercise `AddDVaultPostgres()` optimized behavior even though `AddDVaultPostgres()` currently only adds the core fallback services.
- Assumes the optimized PostgreSQL path will exist in the same workstream even though the contract itself says the tests may not become green otherwise.
- Assumes persisted-row and `RowsWritten` assertions alone can distinguish optimized execution from fallback execution.

AC / test suggestions
- Add an acceptance criterion that the Postgres suite must prove a PostgreSQL-specific optimized strategy was selected, not just that rows were persisted.
- Add the parent ticket ID and dependency rule directly into the delivery contract so handoff sequencing is explicit.
- If this remains a separate task, add an explicit expectation for the configured-Postgres-but-no-optimized-strategy case.

Implementation watchouts
- Existing harness pieces already in repo are the intended boundary: `PostgresIntegrationTestConfiguration`, `NpgsqlProviderReflection`, `ProviderIntegration.ExternalOptIn`, and the per-run schema create/drop pattern in `PostgresDataVaultSchemaTests`.
- The integration test project currently keeps Npgsql conditional and does not yet reference the Postgres provider project; any future wiring must preserve the default no-Postgres local run baseline.
- The SQLite behavior baseline for unchanged vs changed satellite hash diffs is already concrete in `ExplicitDataVaultSaveServiceSqliteTests`; PostgreSQL coverage should mirror those semantics rather than invent provider-specific ones.

Non-blocking notes
- `## Open Questions` is explicitly `- none`, so the return is not caused by unresolved Open Questions.
- Existing repository evidence already supports the opt-in harness portion of the contract; the blocker is dependency/proof clarity for the optimized path.

Split recommendations
- Preferred: keep this as a child task only if it is explicitly linked and sequenced behind the PostgreSQL optimization story.
- Alternative: fold this save-behavior coverage back into story `06EZ0N9TJSXFXH0YZRA3QN2S14`, because that story already includes opt-in PostgreSQL integration coverage in its own scope.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment