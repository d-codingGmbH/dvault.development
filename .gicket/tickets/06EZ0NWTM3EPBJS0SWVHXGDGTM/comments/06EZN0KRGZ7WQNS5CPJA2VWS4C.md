[gicket-bot] PO-critic review contract

Summary
- Persisted contract is bounded, matches the current explicit save-service baseline, and has no unresolved open questions; it is ready for developer handoff with a few batch/fallback parity edge cases to carry into tests.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EZ0NWTM3EPBJS0SWVHXGDGTM/description.md:7-17,52-53` records `ready_for_po_critic` and `## Open Questions` = `- none`.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs:12-35` defines the explicit `IDataVaultSaveService` boundary, and `:41-106` shows `DataVaultSaveRequest` already carries one `LoadTimestamp`, one `RecordSource`, plus hub/link/satellite operations.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs:399-446` shows fallback dispatch first evaluates registered `IDataVaultProviderSaveStrategy` implementations and otherwise uses one provider-neutral writer; `:648-699` applies request-level timestamp/record-source values to satellite rows too.
- `src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs:39-81` shows provider strategies currently receive batched `Requests` plus hashing dependencies through the public `DataVaultProviderSaveStrategyContext`.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:10-24` keeps `AddDVault()` optionless while registering default DI services, matching the ticket's zero-config constraint.
- `docs/plans/optional-advanced-configuration-hooks.md:15-25,87-132` already documents optional hook categories and requires invalid record-source/timestamp behavior to fail clearly.
- `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:510-567` and `src/DCoding.Data.DVault/DataVaultSaveService.cs:612-646` prove there is already an ordered `DataVaultBulkSaveRequest` baseline that hook resolution must preserve.
- `src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs:72-83,120-121,530-538` shows Oracle has its own optimized-batch gate and persists load timestamps as formatted UTC text, supporting the contract's drift/round-trip risk callouts.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Explicitly cover `DataVaultBulkSaveRequest`: the repo already supports ordered multi-request batches, but the contract never states in one place that hooks resolve once per `DataVaultSaveRequest` inside a bulk batch, not once per whole batch.
- Spell out invalid-hook behavior for a mixed bulk batch: tests should prove whether one bad resolved timestamp/record source fails the whole save before persistence rather than allowing partial writes.
- Include an edge case where an optimized provider strategy declines and the same provider package falls back to the provider-neutral writer, to prove hook semantics do not change across strategy-selection boundaries.

Risky assumptions
- The ticket assumes the concrete 'advanced-configuration surface' can be chosen during implementation; current repo patterns are mixed (`DataVaultModelOptions` for naming, DI defaults in `AddDVault()` for hashing/save service), so the API shape still needs an implementation decision.
- `DataVaultProviderSaveStrategyContext` is public and currently carries only `DbContext`, `Requests`, `IStableHashService`, and `IStableHashNormalizer` (`src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs:39-81`); centralizing resolved hook values may require a public-contract adjustment, not just internal wiring.
- The architecture note still uses hub/link-only wording (`docs/architecture/dvault-v1-explicit-save-service.md:8,16-23`) while current source and README already include satellite behavior, so developers need to follow the source baseline rather than that older phrasing.

AC / test suggestions
- Add one acceptance/test sentence that `DataVaultBulkSaveRequest` preserves per-request resolution: two requests in one batch can produce different effective timestamps/record sources, but each request still uses one shared resolved pair across its own hub/link/satellite operations.
- Add a parity test matrix for default path vs selected provider strategy vs strategy-declined fallback, especially Oracle satellite batches and non-Pomelo `AddDVaultMySql()` fallback scenarios already evidenced by the repository.
- Add a failure-mode assertion that invalid hook output causes a clear error with no silent default fallback and no persisted rows for that save invocation.

Implementation watchouts
- Do not let provider strategies re-resolve hook values independently; the current source duplicates `request.LoadTimestamp`/`request.RecordSource` handling across fallback and provider-specific writers.
- Oracle formats timestamps to ISO text inside the strategy, while the fallback writer relies on model annotations and `ApplyModelValueFormats`; parity needs to hold across both paths.
- Satellite handling is batch-aware and chronology-sensitive in the current fallback writer, so repeated hook evaluation per satellite row could subtly change latest-hash-diff behavior.

Non-blocking notes
- The legacy draft at the bottom of `description.md` still mentions configuration 'per model or save operation where appropriate', but the authoritative contract above it explicitly narrows v1 to request-level resolution.

Split recommendations
- Keep provider-specific option objects, native precision controls, or adapter-only timestamp behavior in `06EZ0NX282R80VF5VBKS6ARFZC`, consistent with the current contract.
- Keep broader end-user docs/examples and failure-mode narratives in `06EZ0NX9SVP7MSB1R4PJ50EHGW`, not in this implementation ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment