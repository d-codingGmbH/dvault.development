<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this task to the provider-neutral fallback bulk-save substrate behind the repository-visible bulk SPI; repository and local ticket evidence resolved the naming and split boundaries, and no planning writes were needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already fixes the v1 bulk SPI baseline: IDataVaultSaveService exposes SaveAsync(DbContext, DataVaultBulkSaveRequest), registry adapters expose DataVaultRegistryBulkSaveRequest, and README.md documents ordered bulk saves for prepared source batches.
- This ticket owns the provider-neutral implementation behind that already-visible bulk SPI, not a redesign or rename of the public bulk request types.
- The fallback path is the AddDVault core save-service path in src/DCoding.Data.DVault/DataVaultSaveService.cs: it resolves load timestamp and record source once per request, preserves caller order across the batch, evaluates provider strategies first, and executes the built-in EF writer when no strategy accepts the batch.
- The existing blocks relations from this ticket to 06F2PGNGVQ3TZZWSABAK5SNFK4 and 06F2PGNT7DF4DVNKYWDFZC8DEM remain consistent: provider-native bulk strategies and provider bulk integration coverage are downstream of the provider-neutral fallback baseline.
- The done epic relation from 06F2PGK4QJ0YGXK5479W83Z2J0 is historical release-ordering context already ratified in that epic's closure contract and does not reopen completed v0.13 work.
- No persistent planning action was materialized in this pass: no child ticket was created, no relation was added or removed, no attachment was added, and no planning document was written.

### Scope In
- Provider-neutral execution of ordered bulk save batches through the existing DataVaultBulkSaveRequest and DataVaultRegistryBulkSaveRequest SPI in src/DCoding.Data.DVault.
- Fallback semantics that keep one explicit save contract across providers: if no registered provider strategy CanSave gate accepts the current DbContext plus ordered request batch, the core EF-backed writer persists the batch.
- Batch-order preservation and per-request load-timestamp and record-source resolution across mixed hub, link, and satellite operations.
- Fallback satellite handling that carries in-memory latest HashDiff state across the ordered bulk batch and suppresses duplicate latest-state writes when a later request replays the same state.
- Automated unit and integration coverage that proves the provider-neutral fallback baseline independently of provider-specific optimized strategies.

### Scope Out
- Provider-native bulk SQL implementations and provider-specific eligibility thresholds; those stay in 06F2PGNGVQ3TZZWSABAK5SNFK4.
- Container-backed or externally configured provider integration runs; those stay in 06F2PGNT7DF4DVNKYWDFZC8DEM.
- Benchmark harness work and comparative performance reporting; that stays in 06F2PGNZBRNCQ1SV2KKP6F3BA8.
- Broad release-note packaging and adoption-document closure beyond the already-visible core fallback bulk description; that stays in 06F2PGP2B2RZGGK3CVKK5WRRP8.
- Any implicit SaveChanges interception model or non-explicit persistence mode.

## Acceptance Criteria
- The task ratifies the existing public bulk SPI names instead of reopening them: IDataVaultSaveService.SaveAsync(DbContext, DataVaultBulkSaveRequest), DataVaultBulkSaveRequest, and DataVaultRegistryBulkSaveRequest remain the v1 bulk request surfaces.
- The provider-neutral AddDVault save path can persist an ordered bulk request without any provider-specific save strategy registration, using the same explicit save contract as single-request saves.
- Within one ordered bulk batch, hub and link operations preserve caller batch order, and satellite writes are evaluated against the full ordered batch so duplicate latest-state HashDiff replays do not produce extra rows.
- The fallback path resolves load timestamp and record source once per request before strategy dispatch and makes the resolved batch available to any compatible provider strategy through DataVaultProviderSaveStrategyContext.
- Automated coverage proves the provider-neutral fallback baseline for ordered bulk saves and covers key batch semantics such as request-order preservation, latest-state HashDiff behavior, and strategy-versus-fallback selection.
- If public-facing fallback bulk behavior changes from the already-visible README baseline, only the relevant core API and fallback documentation is updated here while broader v0.14 release-note packaging remains with 06F2PGP2B2RZGGK3CVKK5WRRP8.

## Definition of Done
- Core save-service code in src/DCoding.Data.DVault persists ordered DataVaultBulkSaveRequest batches through the built-in fallback writer when no optimized strategy accepts the batch.
- The fallback implementation continues to share one explicit contract with registry-backed bulk requests and with provider-strategy dispatch rather than introducing a parallel persistence pipeline.
- Relevant unit and integration tests in tests/DCoding.Data.DVault.Tests continue to prove AddDVault-only ordered bulk execution without provider-specific registration and to cover the intended latest-state batch semantics.
- Ticket text and downstream relations remain aligned with the current split: fallback baseline here, native strategies/provider integration/benchmarks/documentation in sibling tickets.

## Implementation Notes
- Use src/DCoding.Data.DVault/DataVaultSaveService.cs as the authoritative fallback implementation surface; SaveAsync(DbContext, DataVaultBulkSaveRequest) already delegates to SaveRequestsAsync over request.Requests.
- Keep the strategy-selection contract unchanged: registered IDataVaultProviderSaveStrategy instances are ordered by descending Priority, CanSave is evaluated against the ordered request batch, and fallback runs only when no strategy accepts.
- Preserve the resolved-request context shape already exposed by DataVaultProviderSaveStrategyContext.ResolvedRequests so provider-specific bulk strategies can reuse the same pre-resolved load timestamp and record-source data.
- Match the visible fallback regression baseline in tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs and tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs, which already cover hook resolution, ordered batch saves, and bulk satellite latest-HashDiff chronology.
- Repository evidence already carries the same ordered-batch eligibility model into request-bound diagnostics in src/DCoding.Data.DVault/DataVaultDiagnostics.cs; keep fallback bulk behavior aligned with that shared gate evaluation model.
- No persistent planning action was materialized in this pass.

## Open Questions
- none

## Follow-Up Questions
- Benchmark ticket 06F2PGNZBRNCQ1SV2KKP6F3BA8 should decide whether release guidance needs a measured crossover point between the provider-neutral fallback path and provider-native bulk strategies.
- Documentation ticket 06F2PGP2B2RZGGK3CVKK5WRRP8 should decide how much of the current batch-order and latest-HashDiff fallback behavior belongs in README examples versus architecture-only guidance.

## Risks
- Because the public bulk SPI and README description are already visible on branch, widening this ticket into API renaming or contract redesign would create churn across existing tests, diagnostics, typed helpers, and docs.
- If the fallback baseline and native strategy tickets diverge on batch-order or latest-state semantics, provider-specific bulk behavior can drift away from the documented core correctness contract.
- Provider strategies with provider-name or minimum-batch gates rely on this fallback path remaining the correctness baseline when CanSave declines; regressions here would surface as cross-provider data correctness issues, not only as performance loss.

## Split Recommendations
- No additional split is recommended; current relations already separate the provider-neutral fallback substrate, provider-native strategies, provider integration coverage, benchmarking, and documentation.
- If later work wants streaming or non-materialized ingestion beyond caller-supplied ordered request batches, create a follow-on story instead of widening this ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Provide a provider-neutral fallback implementation before native strategies.

## Scope
- Refine and complete the work for "Implement fallback bulk ingestion path" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.