<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Ratified the branch-visible v1 bulk save contract around the existing explicit request and strategy surfaces, confirmed the current child/sibling split is sufficient, and made no persistent planning changes.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already fixes the v1 bulk contract: IDataVaultSaveService exposes SaveAsync(DbContext, DataVaultBulkSaveRequest), DataVaultSaveServiceRegistryExtensions expose DataVaultRegistryBulkSaveRequest, IDataVaultDiagnosticsService has bulk-request Analyze overloads, and the public API snapshot already carries those surfaces.
- This story ratifies that explicit bulk SPI and its shared semantics; it does not introduce a second public bulk-insert API, an implicit SaveChanges ingestion mode, or provider-name-specific branching in the core contract.
- IDataVaultProviderSaveStrategy together with DataVaultProviderSaveStrategyContext is the public provider-native extensibility boundary for bulk persistence; provider-specific SQL implementations stay in provider packages.
- The current split is already justified by live local evidence: child 06F2PGN4GPQCGC5WHZQBGP4SD0 is done for provider-neutral fallback bulk execution, blocked siblings 06F2PGNGVQ3TZZWSABAK5SNFK4, 06F2PGNT7DF4DVNKYWDFZC8DEM, and 06F2PGP2B2RZGGK3CVKK5WRRP8 own native strategies, provider integration coverage, and documentation, and benchmark story 06F2PGNZBRNCQ1SV2KKP6F3BA8 remains separate under the same epic.
- The incoming blocks relation from done epic 06F2PGK4QJ0YGXK5479W83Z2J0 is historical release-ordering context and does not reopen v0.13 work.
- No persistent planning action was materialized in this pass: no child ticket was created, no relation was added or removed, no attachment was added, and no planning document was written.

### Scope In
- Ratify the explicit public bulk save request surfaces: DataVaultBulkSaveRequest on IDataVaultSaveService and DataVaultRegistryBulkSaveRequest as the registry-backed adapter boundary.
- Define ordered-batch semantics for explicit bulk saves: caller-supplied request order is preserved, load timestamp and record source remain explicit per request, and the whole ordered batch is the unit of strategy evaluation.
- Define the provider-native bulk SPI boundary through IDataVaultProviderSaveStrategy, strategy Priority ordering, dependency-injection registration-order tie-breaks, and DataVaultProviderSaveStrategyContext.
- Keep provider-neutral fallback semantics as the correctness baseline when no strategy accepts, including in-batch satellite latest-state HashDiff suppression and chronological state handling.
- Keep diagnostics and typed bulk helper surfaces aligned with the same explicit bulk contract rather than creating parallel ingestion paths.

### Scope Out
- Provider-native SQL implementations, provider-specific CanSave thresholds, and package-specific optimization details; those stay in 06F2PGNGVQ3TZZWSABAK5SNFK4.
- Container-backed or externally configured provider bulk integration coverage; that stays in 06F2PGNT7DF4DVNKYWDFZC8DEM.
- Benchmark harness work and comparative performance claims; that stays in 06F2PGNZBRNCQ1SV2KKP6F3BA8.
- Broader v0.14 release-note packaging and adoption-document closure beyond the contract-specific bulk SPI wording; that stays in 06F2PGP2B2RZGGK3CVKK5WRRP8.
- Implicit SaveChanges-based ingestion, streaming or non-materialized ingestion, and any non-explicit persistence mode.

## Acceptance Criteria
- The story ratifies the existing public bulk SPI instead of reopening it: IDataVaultSaveService.SaveAsync(DbContext, DataVaultBulkSaveRequest), DataVaultBulkSaveRequest, and DataVaultRegistryBulkSaveRequest remain the v1 bulk request surfaces.
- Ordered bulk saves are defined as caller-ordered batches for both explicit and registry-backed paths, and typed mapper bulk helpers continue to adapt into that same registry-backed batch contract rather than defining a separate ingestion API.
- Provider-native bulk extensibility is defined by IDataVaultProviderSaveStrategy: strategies are evaluated by descending Priority with dependency-injection registration order as the tie-break, receive the whole ordered batch, and fall back to the provider-neutral writer when none accepts.
- The bulk strategy context carries pre-resolved per-request load timestamp and record source data through DataVaultProviderSaveStrategyContext.ResolvedRequests so provider-native implementations and diagnostics share one baseline.
- The provider-neutral fallback baseline preserves hub/link request order and satellite correctness across the ordered batch, including duplicate latest-state HashDiff suppression and chronological replay handling.
- README, request-bound diagnostics, and public API snapshot coverage stay aligned with the ratified bulk SPI while broader documentation packaging remains with 06F2PGP2B2RZGGK3CVKK5WRRP8.

## Definition of Done
- Ticket text explicitly captures the bulk SPI boundary and the current child/sibling ownership split.
- Branch-visible source, docs, diagnostics, and public API snapshot continue to expose one explicit bulk contract rather than a second bulk-insert SPI or hidden implicit ingestion path.
- Existing downstream relations remain aligned with the refined split: 06F2PGN4GPQCGC5WHZQBGP4SD0 for fallback, 06F2PGNGVQ3TZZWSABAK5SNFK4 for native strategies, 06F2PGNT7DF4DVNKYWDFZC8DEM for provider integration coverage, 06F2PGNZBRNCQ1SV2KKP6F3BA8 for benchmarks, and 06F2PGP2B2RZGGK3CVKK5WRRP8 for docs.
- No additional child ticket, relation edit, attachment, or planning document is required for this refinement pass.

## Implementation Notes
- Use src/DCoding.Data.DVault/DataVaultSaveService.cs as the authoritative contract surface: IDataVaultSaveService exposes single-request and DataVaultBulkSaveRequest overloads, and DataVaultSaveServiceRegistryExtensions adapt DataVaultRegistryBulkSaveRequest into the same bulk pipeline.
- Use src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs as the authoritative provider-native extension boundary: CanSave(DbContext, IReadOnlyList<DataVaultSaveRequest>), Priority, SaveAsync(DataVaultProviderSaveStrategyContext), and ResolvedRequests define the shared batch-dispatch contract.
- Keep request-bound diagnostics aligned with the same batch contract via IDataVaultDiagnosticsService.Analyze(DbContext, DataVaultBulkSaveRequest) and Analyze(DbContext, DataVaultRegistryBulkSaveRequest).
- Keep DataVaultSaveServiceTypedExtensions bulk helper methods as adapters into DataVaultRegistryBulkSaveRequest rather than allowing a parallel typed bulk ingestion surface to emerge.
- Preserve the already-visible repository wording that ratifies the current boundary, especially README bulk-save guidance and docs/releases/v0.9.0.md on existing provider save-strategy bulk extensibility.
- Treat 06F2PGN4GPQCGC5WHZQBGP4SD0 as the completed provider-neutral fallback substrate and leave native strategies, provider integration coverage, benchmarks, and broader docs to their existing sibling tickets.

## Open Questions
- none

## Follow-Up Questions
- Should benchmark ticket 06F2PGNZBRNCQ1SV2KKP6F3BA8 establish a documented crossover point, if any, between the provider-neutral fallback path and provider-native strategies before stronger performance guidance is published?
- If later work needs streaming or non-materialized ingestion instead of caller-supplied ordered batches, should that be a separate follow-on story rather than widening DataVaultBulkSaveRequest?
- Once provider-native strategy work settles, does docs ticket 06F2PGP2B2RZGGK3CVKK5WRRP8 want a consumer-facing example of DataVaultRegistryBulkSaveRequest or typed bulk helper usage beyond the current README baseline?

## Risks
- Because the bulk SPI is already branch-visible in source, README, diagnostics, and the public API snapshot, renaming or widening it now would create unnecessary churn across code, tests, and documentation.
- If provider-native strategy tickets diverge from ordered-batch or ResolvedRequests semantics, bulk behavior can drift across providers even when public APIs still match.
- Performance messaging can outrun evidence unless benchmark ticket 06F2PGNZBRNCQ1SV2KKP6F3BA8 finishes before stronger comparative claims are added.

## Split Recommendations
- No additional split is recommended; the current child/sibling ticket graph already isolates fallback substrate, native strategies, provider integration coverage, benchmarks, and documentation.
- If future work wants streaming ingestion, transport adapters, or queue-specific batching, create a separate follow-on story instead of widening this contract ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Define the public and internal contracts for batched DVault writes.

## Scope
- Refine and complete the work for "Define explicit bulk ingestion SPI" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.