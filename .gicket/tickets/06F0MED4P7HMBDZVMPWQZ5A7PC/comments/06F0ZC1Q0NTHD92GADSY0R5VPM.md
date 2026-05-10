[gicket-bot] PO refinement contract

Summary
- Refined the diagnostics contract to make save-strategy reporting request-bound and deterministic: actual dispatch is evaluated only against `DbContext` plus `DataVaultSaveRequest`/`DataVaultBulkSaveRequest`, validation-only calls return strategy status `not evaluated`, and tests must cover dirty-context, multi-active, unknown-provider, and SQL Server/MySQL/Oracle batch-threshold fallbacks. No child tickets, relation changes, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Actual save-strategy reporting is defined against the same explicit save boundary as `IDataVaultSaveService`: the caller supplies the target `DbContext` plus either one `DataVaultSaveRequest` or one ordered `DataVaultBulkSaveRequest`. Diagnostics must evaluate the exact ordered request batch against the registered `IDataVaultProviderSaveStrategy.CanSave(DbContext, IReadOnlyList<DataVaultSaveRequest>)` gates and report the selected provider strategy or provider-neutral fallback for that concrete input; provider-name-only availability is not sufficient. If a registry-backed convenience overload is offered, it must resolve to the same explicit request batch before evaluation.
- critic-item-2: `answered` - Acceptance and test coverage now need to pin the currently material fallback causes and thresholds explicitly: dirty `DbContext` state, multi-active satellite operations, unknown or unregistered provider names, SQL Server optimized dispatch only when total operations are at least 50 and satellite operations are at most 500, and MySQL/Oracle optimized dispatch only when total operations are at least 50.
- critic-item-3: `answered` - Validation-only or explain-only calls without a save request must not invent an actual dispatch result. They still return validation, explain, capability-profile, and provider-behavior-profile data, but the strategy-evaluation section returns `not evaluated` until the caller supplies a `DbContext` plus explicit save request or batch.
- critic-item-4: `answered` - The earlier `actual save-strategy dispatch result` language is now grounded in the dispatcher implementation: `SaveRequestsAsync` resolves the ordered request batch, calls `CanSave(dbContext, requests)` on each registered strategy, and selects the first compatible strategy by descending priority before falling back to the provider-neutral writer. Diagnostics must report against that same concrete input and selection flow.
- critic-item-5: `answered` - The DoD and acceptance contract now require reason-specific strategy-selection assertions instead of provider-name-only reporting. Tests must prove dirty tracked changes, multi-active satellite rejection, unknown or unregistered provider fallback, SQL Server total/satellite threshold rejection, and MySQL/Oracle minimum-batch rejection, so a provider-name-only implementation no longer satisfies the ticket.

Clarifications
- Actual save-strategy diagnostics are defined only for the explicit save boundary: `DbContext` plus one `DataVaultSaveRequest` or one ordered `DataVaultBulkSaveRequest`; provider-name-only availability is not sufficient for the `actual dispatch` surface.
- If a registry-backed convenience overload is exposed, it must first resolve to the same explicit request batch used by `IDataVaultSaveService` before strategy evaluation runs.
- Validation-only or explain-only calls without a save request still return validation, explain, capability-profile, and provider-behavior-profile data, but the strategy section returns `not evaluated` instead of an inferred actual dispatch result.
- Explain output must continue to distinguish capability-profile selection, provider-behavior-profile selection, and save-strategy evaluation as separate surfaces.
- Current material fallback causes that must be reported and tested are dirty tracked EF state, multi-active satellite operations, unknown or unregistered provider names, SQL Server optimized dispatch rejection below 50 total operations or above 500 satellite operations, and MySQL/Oracle optimized dispatch rejection below 50 total operations.
- Unknown or unregistered provider names still default capability-profile selection to `sqlite-v1` and provider-behavior selection to `provider-neutral-v1`; diagnostics must flag that default as a risky fallback instead of supported SQLite intent.
- No child tickets, relation writes, or planning documents were created in this refinement pass.

Scope In
- Add a public machine-readable diagnostics contract that validates current `DataVaultMetadataModel`, `DataVaultMetadataRegistry`, and code-first declarations before save or runtime execution.
- Add request-bound save-strategy diagnostics that evaluate the same dispatcher inputs and ordering used by `IDataVaultSaveService` for explicit single-request and bulk-request saves.
- Return a deterministic `not evaluated` strategy status for validation-only or explain-only calls that do not provide a save request batch.
- Report deterministic fallback-cause categories for dirty `DbContext` state, multi-active satellite operations, unknown or unregistered provider names, provider/profile mapping gaps, and current SQL Server/MySQL/Oracle optimized-batch thresholds.
- Keep explain output aligned with translator-owned table, column, key, index, constraint, provider-mapping, metadata-source, capability-profile, load-timestamp-storage, and provider-behavior surfaces.
- Add tests that assert stable structured payloads and deterministic ordering across the built-in provider profile set and load-timestamp storage variants.

Scope Out
- Provider-name-only strategy reporting that ignores request shape or `DbContext` state.
- Inventing representative save batches for validation-only diagnostics instead of returning `not evaluated`.
- Provider-specific save SQL, dispatch priorities, or optimization-behavior changes.
- CLI command implementation beyond keeping the structured diagnostics payload reusable for future tooling.
- Registry architecture redesign or new metadata-authoring surfaces outside the existing metadata-model, registry, and code-first APIs.
- Runnable example authoring and README or release-document updates, which remain on sibling tickets 06F0MEDBFZ25YA1M7RJ71Z7ZCM and 06F0MEDJC732GDD77H60R259P0.

Open questions
- none

Follow-up questions
- After this API lands, should a future CLI wrapper expose the same structured diagnostics payload directly, or should CLI-specific shaping stay outside the core library?
- Once sibling docs and examples tickets land, should user-facing docs explicitly call out that unknown EF provider names default capability selection to `sqlite-v1` while strategy evaluation may still return provider-neutral fallback or `not evaluated`?

Risks
- If fallback-reason reporting duplicates provider `CanSave` gates instead of sharing extracted helpers, diagnostics can drift from actual runtime dispatch behavior.
- The SQL Server/MySQL/Oracle threshold numbers are part of the current v0.5 behavior baseline; future provider-optimization changes will need diagnostics tests and documentation updated in lockstep.
- Unknown-provider capability fallback to `sqlite-v1` remains a risky default and will mislead callers unless diagnostics keeps surfacing it as a warning state rather than supported configuration.

Split recommendations
- No new split is recommended; the current evidence supports a bounded contract refinement without materializing child tickets, and this ticket remains the diagnostics child under story 06F0MECWYMPQ4R0KWV1R637RT0 that blocks docs ticket 06F0MEDJC732GDD77H60R259P0.
- Keep the completed registry ticket 06F0MEAXT99V0P115P0WEJD4P0 as upstream context only and do not reopen registry redesign work inside this diagnostics task.

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment