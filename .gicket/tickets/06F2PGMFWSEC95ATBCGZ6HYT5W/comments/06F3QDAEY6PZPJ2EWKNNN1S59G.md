[gicket-bot] PO refinement contract

Summary
- Refined the epic around the repository-visible v0.14.0 provider bulk-ingestion baseline and left no blocking PO questions.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already fixes the v1 baseline: explicit opt-in bulk ingestion is delivered through IDataVaultSaveService with DataVaultBulkSaveRequest, while single-request DataVaultSaveRequest remains supported.
- Registry-backed callers use DataVaultRegistryBulkSaveRequest to adapt shared metadata registries into the same ordered explicit bulk-save pipeline rather than a separate write surface.
- Provider-native bulk ingestion is a bounded optimization layer for PostgreSQL, SQL Server, MySQL, and Oracle only; provider-neutral fallback remains the guaranteed baseline when no strategy is selected or a strategy declines the batch.
- The current baseline keeps native-dispatch eligibility visible through diagnostics and documents bounded exclusions such as unsupported multi-active native batches.
- No child tickets, relation changes, or planning documents were materialized in this refinement pass.

Scope In
- Public ordered bulk-save entry points and supporting contracts on the existing IDataVaultSaveService write boundary.
- Registry-backed bulk-save adaptation for callers whose authoritative metadata source is a DataVaultMetadataModel or DataVaultMetadataRegistry.
- Deterministic provider-neutral bulk processing and fallback behavior for unsupported or declined native strategy cases.
- Provider-native bulk-ingestion strategy support, selection gates, and diagnostics for PostgreSQL, SQL Server, MySQL, and Oracle within the existing provider package family.
- Relevant unit, integration, external-provider proof, benchmark, README, architecture, and release-note updates tied to the bulk-ingestion behavior.

Scope Out
- New provider packages or native bulk-ingestion commitments beyond PostgreSQL, SQL Server, MySQL, and Oracle in this release line.
- Changing the public write boundary away from IDataVaultSaveService or removing the existing single-request save path.
- Native support for unsupported batch shapes that the documented eligibility gates deliberately decline, including multi-active satellite operations.
- Unrelated refactors to model-first, bridge, PIT, analyzer, or general package layout areas that do not directly support provider bulk ingestion.

Open questions
- none

Follow-up questions
- Should a later release expose provider-specific native-dispatch thresholds and eligibility gates as configurable policy rather than fixed documented defaults?
- Should a later follow-up ticket add native bulk-ingestion support for additional providers or keep provider-neutral fallback as the long-term answer outside the current four-provider set?
- Do we want a separate future ticket for richer benchmark publication and comparison artifacts beyond the current benchmark evidence documented in the release notes?

Risks
- External-provider proof depends on opt-in environment-specific connection strings, so native provider evidence can be skipped in default local or CI runs unless those lanes are explicitly configured.
- Benchmark and throughput claims are environment-sensitive; release notes should continue to tie performance evidence to recorded machine, provider, and execution context rather than promise universal gains.
- The epic spans core contracts, provider packages, diagnostics, tests, and documentation, so downstream implementation work should stay bounded to this release contract to avoid scope creep.

Split recommendations
- No mandatory split is required for PO-critic handoff because the repository already presents one coherent v0.14.0 epic baseline.
- If later execution tracking needs finer granularity, split along three bounded slices: core bulk-save contract and fallback behavior, provider-native strategy coverage and diagnostics, and documentation or benchmark evidence.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment