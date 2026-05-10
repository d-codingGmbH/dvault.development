<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the diagnostics contract to make save-strategy reporting request-bound and deterministic: actual dispatch is evaluated only against `DbContext` plus `DataVaultSaveRequest`/`DataVaultBulkSaveRequest`, validation-only calls return strategy status `not evaluated`, and tests must cover dirty-context, multi-active, unknown-provider, and SQL Server/MySQL/Oracle batch-threshold fallbacks. No child tickets, relation changes, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Actual save-strategy diagnostics are defined only for the explicit save boundary: `DbContext` plus one `DataVaultSaveRequest` or one ordered `DataVaultBulkSaveRequest`; provider-name-only availability is not sufficient for the `actual dispatch` surface.
- If a registry-backed convenience overload is exposed, it must first resolve to the same explicit request batch used by `IDataVaultSaveService` before strategy evaluation runs.
- Validation-only or explain-only calls without a save request still return validation, explain, capability-profile, and provider-behavior-profile data, but the strategy section returns `not evaluated` instead of an inferred actual dispatch result.
- Explain output must continue to distinguish capability-profile selection, provider-behavior-profile selection, and save-strategy evaluation as separate surfaces.
- Current material fallback causes that must be reported and tested are dirty tracked EF state, multi-active satellite operations, unknown or unregistered provider names, SQL Server optimized dispatch rejection below 50 total operations or above 500 satellite operations, and MySQL/Oracle optimized dispatch rejection below 50 total operations.
- Unknown or unregistered provider names still default capability-profile selection to `sqlite-v1` and provider-behavior selection to `provider-neutral-v1`; diagnostics must flag that default as a risky fallback instead of supported SQLite intent.
- No child tickets, relation writes, or planning documents were created in this refinement pass.

### Scope In
- Add a public machine-readable diagnostics contract that validates current `DataVaultMetadataModel`, `DataVaultMetadataRegistry`, and code-first declarations before save or runtime execution.
- Add request-bound save-strategy diagnostics that evaluate the same dispatcher inputs and ordering used by `IDataVaultSaveService` for explicit single-request and bulk-request saves.
- Return a deterministic `not evaluated` strategy status for validation-only or explain-only calls that do not provide a save request batch.
- Report deterministic fallback-cause categories for dirty `DbContext` state, multi-active satellite operations, unknown or unregistered provider names, provider/profile mapping gaps, and current SQL Server/MySQL/Oracle optimized-batch thresholds.
- Keep explain output aligned with translator-owned table, column, key, index, constraint, provider-mapping, metadata-source, capability-profile, load-timestamp-storage, and provider-behavior surfaces.
- Add tests that assert stable structured payloads and deterministic ordering across the built-in provider profile set and load-timestamp storage variants.

### Scope Out
- Provider-name-only strategy reporting that ignores request shape or `DbContext` state.
- Inventing representative save batches for validation-only diagnostics instead of returning `not evaluated`.
- Provider-specific save SQL, dispatch priorities, or optimization-behavior changes.
- CLI command implementation beyond keeping the structured diagnostics payload reusable for future tooling.
- Registry architecture redesign or new metadata-authoring surfaces outside the existing metadata-model, registry, and code-first APIs.
- Runnable example authoring and README or release-document updates, which remain on sibling tickets 06F0MEDBFZ25YA1M7RJ71Z7ZCM and 06F0MEDJC732GDD77H60R259P0.

## Acceptance Criteria
- A caller can obtain one stable structured diagnostics result composed of serializable DTO data from the current metadata-first, registry-backed, or code-first configuration paths without executing a save.
- When a caller supplies a `DbContext` plus one `DataVaultSaveRequest` or ordered `DataVaultBulkSaveRequest`, diagnostics evaluate the same strategy-ordering and compatibility gates as `IDataVaultSaveService` and report the selected provider strategy or provider-neutral fallback for that exact input.
- When a caller does not supply a save request batch, diagnostics still return validation, explain, capability-profile, and provider-behavior-profile data, but the save-strategy section returns `not evaluated` instead of inventing representative dispatch.
- Strategy-evaluation output explicitly classifies current material fallback causes: dirty tracked EF state, multi-active satellite operations, unknown or unregistered provider names, SQL Server optimized dispatch requiring at least 50 total operations and at most 500 satellite operations, and MySQL/Oracle optimized dispatch requiring at least 50 total operations.
- Explain results enumerate each generated entity with deterministic order and include table kind and name, source metadata name, ordered properties with role and provider mapping metadata, primary key, projected indexes and constraints, selected capability profile, effective load-timestamp storage shape, and selected provider-behavior profile.
- When capability selection defaulted because the EF provider name was unknown or unregistered, or when a provider profile omits a required logical mapping, the diagnostics result reports that condition explicitly instead of silently presenting a normal supported configuration.
- A concise human-readable rendering can be produced from the structured result, and automated tests assert the structured payload rather than brittle whole-string formatting.
- Built-in coverage includes the current visible provider baseline `sqlite-v1`, `postgres-v1`, `sqlserver-v1`, `oracle-v1`, and `mysql-pomelo-v1`, plus `WithLoadTimestampStorage` variants used by the existing translator and tests.

## Definition of Done
- Public API placement follows current DVault package and layout conventions and remains additive to existing registry, translator, save-service, and provider-behavior surfaces.
- Automated tests cover metadata-first, registry-backed, and code-first validation and explain flows, plus request-bound strategy evaluation for explicit single-request and bulk-request saves and the `not evaluated` validation-only path.
- Automated tests explicitly assert dirty-context fallback, multi-active satellite fallback, unknown or unregistered provider fallback, SQL Server total/satellite threshold rejection, and MySQL/Oracle minimum-batch rejection.
- The implementation reuses the authoritative current translation, provider-capability selection, provider-behavior selection, and strategy-dispatch logic rather than creating a second independent naming or provider-resolution path.
- The task completes without adding a CLI command, changing provider optimization behavior, or absorbing the sibling examples or docs scope.

## Implementation Notes
- Model actual-dispatch explanation around an additive evaluator that takes `DbContext` plus explicit save request input; if registry-backed convenience is exposed, resolve through the existing registry adapters first and then evaluate the resolved explicit batch.
- Reuse current dispatch semantics from `DataVaultSaveService`: strategies are evaluated in descending `Priority`, dependency-injection registration order breaks equal-priority ties, the first compatible strategy wins, and the provider-neutral writer remains the fallback when every strategy declines.
- Use the current provider `CanSave` gates as the authoritative compatibility baseline: provider-name match, clean change tracker, no multi-active satellite operations, SQL Server total/satellite thresholds, and MySQL/Oracle minimum total-operation thresholds.
- Keep validation-only diagnostics request-free and emit `not evaluated` for the strategy section instead of guessing an `actual dispatch` result.
- Surface current capability and provider-behavior fallbacks (`sqlite-v1` and `provider-neutral-v1`) as risky or defaulted states when no registered provider mapping applies.
- Because `IDataVaultProviderSaveStrategy.CanSave` returns only `bool` today, fallback-cause reporting should come from a shared gate helper or thin evaluator extraction that preserves current dispatch semantics rather than a separate hand-maintained rule table.

## Open Questions
- none

## Follow-Up Questions
- After this API lands, should a future CLI wrapper expose the same structured diagnostics payload directly, or should CLI-specific shaping stay outside the core library?
- Once sibling docs and examples tickets land, should user-facing docs explicitly call out that unknown EF provider names default capability selection to `sqlite-v1` while strategy evaluation may still return provider-neutral fallback or `not evaluated`?

## Risks
- If fallback-reason reporting duplicates provider `CanSave` gates instead of sharing extracted helpers, diagnostics can drift from actual runtime dispatch behavior.
- The SQL Server/MySQL/Oracle threshold numbers are part of the current v0.5 behavior baseline; future provider-optimization changes will need diagnostics tests and documentation updated in lockstep.
- Unknown-provider capability fallback to `sqlite-v1` remains a risky default and will mislead callers unless diagnostics keeps surfacing it as a warning state rather than supported configuration.

## Split Recommendations
- No new split is recommended; the current evidence supports a bounded contract refinement without materializing child tickets, and this ticket remains the diagnostics child under story 06F0MECWYMPQ4R0KWV1R637RT0 that blocks docs ticket 06F0MEDJC732GDD77H60R259P0.
- Keep the completed registry ticket 06F0MEAXT99V0P115P0WEJD4P0 as upstream context only and do not reopen registry redesign work inside this diagnostics task.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Add deterministic diagnostics that explain the configured Data Vault model and report invalid or risky configurations before runtime writes fail.

## Scope In

- Validation for duplicate logical names, missing parents, unsupported provider/profile combinations, and ambiguous typed mappings.
- Explain output for tables, columns, indexes, constraints, timestamp storage, provider profile, and strategy selection.
- Machine-readable output suitable for tests and future CLI tooling.

## Scope Out

- Full CLI command implementation unless needed for examples.
- Provider-specific optimization changes.

## Acceptance Criteria

- Diagnostics can be asserted in tests without brittle formatting.
- Human-readable output is concise enough for README/examples.
- Strategy fallback reasons are visible when provider-specific optimization is not selected.