[gicket-bot] PO refinement contract

Summary
- Ratified the branch-visible v1 bulk save contract around the existing explicit request and strategy surfaces, confirmed the current child/sibling split is sufficient, and made no persistent planning changes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the v1 bulk contract: IDataVaultSaveService exposes SaveAsync(DbContext, DataVaultBulkSaveRequest), DataVaultSaveServiceRegistryExtensions expose DataVaultRegistryBulkSaveRequest, IDataVaultDiagnosticsService has bulk-request Analyze overloads, and the public API snapshot already carries those surfaces.
- This story ratifies that explicit bulk SPI and its shared semantics; it does not introduce a second public bulk-insert API, an implicit SaveChanges ingestion mode, or provider-name-specific branching in the core contract.
- IDataVaultProviderSaveStrategy together with DataVaultProviderSaveStrategyContext is the public provider-native extensibility boundary for bulk persistence; provider-specific SQL implementations stay in provider packages.
- The current split is already justified by live local evidence: child 06F2PGN4GPQCGC5WHZQBGP4SD0 is done for provider-neutral fallback bulk execution, blocked siblings 06F2PGNGVQ3TZZWSABAK5SNFK4, 06F2PGNT7DF4DVNKYWDFZC8DEM, and 06F2PGP2B2RZGGK3CVKK5WRRP8 own native strategies, provider integration coverage, and documentation, and benchmark story 06F2PGNZBRNCQ1SV2KKP6F3BA8 remains separate under the same epic.
- The incoming blocks relation from done epic 06F2PGK4QJ0YGXK5479W83Z2J0 is historical release-ordering context and does not reopen v0.13 work.
- No persistent planning action was materialized in this pass: no child ticket was created, no relation was added or removed, no attachment was added, and no planning document was written.

Scope In
- Ratify the explicit public bulk save request surfaces: DataVaultBulkSaveRequest on IDataVaultSaveService and DataVaultRegistryBulkSaveRequest as the registry-backed adapter boundary.
- Define ordered-batch semantics for explicit bulk saves: caller-supplied request order is preserved, load timestamp and record source remain explicit per request, and the whole ordered batch is the unit of strategy evaluation.
- Define the provider-native bulk SPI boundary through IDataVaultProviderSaveStrategy, strategy Priority ordering, dependency-injection registration-order tie-breaks, and DataVaultProviderSaveStrategyContext.
- Keep provider-neutral fallback semantics as the correctness baseline when no strategy accepts, including in-batch satellite latest-state HashDiff suppression and chronological state handling.
- Keep diagnostics and typed bulk helper surfaces aligned with the same explicit bulk contract rather than creating parallel ingestion paths.

Scope Out
- Provider-native SQL implementations, provider-specific CanSave thresholds, and package-specific optimization details; those stay in 06F2PGNGVQ3TZZWSABAK5SNFK4.
- Container-backed or externally configured provider bulk integration coverage; that stays in 06F2PGNT7DF4DVNKYWDFZC8DEM.
- Benchmark harness work and comparative performance claims; that stays in 06F2PGNZBRNCQ1SV2KKP6F3BA8.
- Broader v0.14 release-note packaging and adoption-document closure beyond the contract-specific bulk SPI wording; that stays in 06F2PGP2B2RZGGK3CVKK5WRRP8.
- Implicit SaveChanges-based ingestion, streaming or non-materialized ingestion, and any non-explicit persistence mode.

Open questions
- none

Follow-up questions
- Should benchmark ticket 06F2PGNZBRNCQ1SV2KKP6F3BA8 establish a documented crossover point, if any, between the provider-neutral fallback path and provider-native strategies before stronger performance guidance is published?
- If later work needs streaming or non-materialized ingestion instead of caller-supplied ordered batches, should that be a separate follow-on story rather than widening DataVaultBulkSaveRequest?
- Once provider-native strategy work settles, does docs ticket 06F2PGP2B2RZGGK3CVKK5WRRP8 want a consumer-facing example of DataVaultRegistryBulkSaveRequest or typed bulk helper usage beyond the current README baseline?

Risks
- Because the bulk SPI is already branch-visible in source, README, diagnostics, and the public API snapshot, renaming or widening it now would create unnecessary churn across code, tests, and documentation.
- If provider-native strategy tickets diverge from ordered-batch or ResolvedRequests semantics, bulk behavior can drift across providers even when public APIs still match.
- Performance messaging can outrun evidence unless benchmark ticket 06F2PGNZBRNCQ1SV2KKP6F3BA8 finishes before stronger comparative claims are added.

Split recommendations
- No additional split is recommended; the current child/sibling ticket graph already isolates fallback substrate, native strategies, provider integration coverage, benchmarks, and documentation.
- If future work wants streaming ingestion, transport adapters, or queue-specific batching, create a separate follow-on story instead of widening this contract ticket.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment