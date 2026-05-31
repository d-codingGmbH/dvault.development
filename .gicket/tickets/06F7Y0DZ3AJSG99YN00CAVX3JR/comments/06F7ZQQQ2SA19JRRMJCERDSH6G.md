[gicket-bot] PO-critic review contract

Summary
- Ticket is ready for developer handoff: the delivery contract is resolved, the existing async chunked-save and typed mapper baselines are directly evidenced in repo sources, and the remaining risks are implementation watchouts rather than PO blockers.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted ticket contract at `.gicket/tickets/06F7Y0DZ3AJSG99YN00CAVX3JR/description.md` is in `ready_for_po_critic` state and its `## Open Questions` section says `- none`; the contract already spells out scope, acceptance criteria, definition of done, implementation notes, risks, and follow-up questions.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs` directly defines `IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, CancellationToken)` and implements it by delegating to `SaveChunkedRequestsAsync(...)` with sequential `await foreach` chunk processing, which matches the execution boundary this story is supposed to build on.
- `src/DCoding.Data.DVault/DataVaultSaveServiceTypedExtensions.cs` already provides typed helper baselines for single-item and `IEnumerable<TSource>` saves (`SaveHubAsync`, `SaveHubsAsync`, `SaveLinkAsync`, `SaveLinksAsync`, `SaveOrdinaryHubSatelliteAsync`, `SaveOrdinaryHubSatellitesAsync`) and enforces ordinary hub-parent satellite-only support through `RequireOrdinaryHubParentSatellite(...)`.
- `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt` shows the current public API baseline already contains the typed helper overloads (lines <redacted>) and the separate async chunked save overload on `IDataVaultSaveService` (line 1617), so the requested async typed convenience layer is additive to existing public surfaces rather than a new save-service contract.
- `docs/architecture/dvault-v1-streaming-explicit-save-contract.md` and `docs/architecture/dvault-v1-typed-row-mapper-contract.md` directly confirm the authoritative compatibility rules: async chunk processing must stay ordered, single-pass, non-background, and non-buffering, while typed mappers keep `loadTimestamp` and `recordSource` explicit and are limited to the existing hub, link, and ordinary hub-parent satellite contract shapes.
- `docs/releases/v0.12.0.md` documents source-generated mapper helpers on the same explicit save boundary, which matches this ticket's acceptance criterion about generated typed mapper compatibility where the current helper surface already supports it.
- Branch-history evidence shows this is still a pre-development handoff: `git log --oneline --grep='06F7Y0DZ3AJSG99YN00CAVX3JR|06F7Y0CN1804HZW03J4XQ8XEJR'` returned only PO/PO-critic workflow commits for this ticket (`77d1fee34443`, `4bb66c90edeb`, `44c01c489969`) plus the earlier landed async-source contract integration commit `ea67fe2c5`, and `git diff --name-only develop...ticket/06F7Y0DZ3AJSG99YN00CAVX3JR-story-add-typed-async-chunk-mapper-helpers-for-e` listed only `.gicket/tickets/06F7Y0DZ3AJSG99YN00CAVX3JR/*` metadata files.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No blocking gap remains, but the contract does not give a concrete worked example for the final under-filled chunk after several full chunks; that behavior should be pinned in tests.
- No blocking gap remains, but the contract does not show an explicit example of an async source that yields zero effective requests after mapping or no-op filtering; the DoD already expects empty or no-op behavior to be proven.

Risky assumptions
- The ticket assumes the new async helper surface will live on the existing helper extension lanes instead of introducing a new abstraction; that is strongly suggested by the implementation notes but not frozen to exact method names.
- The ticket assumes `caller-owned bounded chunk sizing or equivalent visible chunk-boundary input` can be satisfied by a visible request-count boundary or an equally explicit alternative, leaving final API-shape choice to implementation.

AC / test suggestions
- Add focused unit coverage that a mapper or request-factory failure at item `N` stops later chunk production and preserves the current typed-helper exception-context pattern with source type and bounded position context.
- Add explicit async-helper tests for empty async source, empty chunk, and final partial chunk behavior so the no-op and ordering contract is executable instead of only prose.
- Add integration coverage that generated hub, link, and ordinary hub-parent satellite mappers succeed through the async helper path in deterministic saved-record order.

Implementation watchouts
- Do not pre-buffer the full `IAsyncEnumerable<TSource>`; adapt into bounded `DataVaultSaveChunk` values and delegate to the existing `IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, ...)` boundary.
- Preserve caller-visible `loadTimestamp`, `recordSource`, cancellation, transaction ownership, and chunk-boundary control; the contract explicitly rules out hidden defaults and background continuation.
- Keep the existing typed-helper failure-wrapping and ordinary hub-parent satellite guard behavior from `DataVaultSaveServiceTypedExtensions` so async typed helpers do not silently broaden the supported shape.

Non-blocking notes
- A separate todo follow-up task exists at `.gicket/tickets/06F7Y0F650KM61BQXMEQPZ86DR/ticket.json` for v0.24.0 async streaming and EF safety documentation, so documentation follow-up has already been split rather than left implicit.

Split recommendations
- Keep the core implementation story focused on `IAsyncEnumerable<TSource>` to `DataVaultSaveRequest` mapping and typed helper convenience; leave any later convenience API for async sources that already yield registry-backed requests to a separate follow-up story, as the ticket's own follow-up questions already suggest.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment