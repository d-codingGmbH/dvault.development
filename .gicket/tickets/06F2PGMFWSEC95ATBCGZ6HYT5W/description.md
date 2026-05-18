<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the epic around the repository-visible v0.14.0 provider bulk-ingestion baseline and left no blocking PO questions.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The repository already fixes the v1 baseline: explicit opt-in bulk ingestion is delivered through IDataVaultSaveService with DataVaultBulkSaveRequest, while single-request DataVaultSaveRequest remains supported.
- Registry-backed callers use DataVaultRegistryBulkSaveRequest to adapt shared metadata registries into the same ordered explicit bulk-save pipeline rather than a separate write surface.
- Provider-native bulk ingestion is a bounded optimization layer for PostgreSQL, SQL Server, MySQL, and Oracle only; provider-neutral fallback remains the guaranteed baseline when no strategy is selected or a strategy declines the batch.
- The current baseline keeps native-dispatch eligibility visible through diagnostics and documents bounded exclusions such as unsupported multi-active native batches.
- No child tickets, relation changes, or planning documents were materialized in this refinement pass.

### Scope In
- Public ordered bulk-save entry points and supporting contracts on the existing IDataVaultSaveService write boundary.
- Registry-backed bulk-save adaptation for callers whose authoritative metadata source is a DataVaultMetadataModel or DataVaultMetadataRegistry.
- Deterministic provider-neutral bulk processing and fallback behavior for unsupported or declined native strategy cases.
- Provider-native bulk-ingestion strategy support, selection gates, and diagnostics for PostgreSQL, SQL Server, MySQL, and Oracle within the existing provider package family.
- Relevant unit, integration, external-provider proof, benchmark, README, architecture, and release-note updates tied to the bulk-ingestion behavior.

### Scope Out
- New provider packages or native bulk-ingestion commitments beyond PostgreSQL, SQL Server, MySQL, and Oracle in this release line.
- Changing the public write boundary away from IDataVaultSaveService or removing the existing single-request save path.
- Native support for unsupported batch shapes that the documented eligibility gates deliberately decline, including multi-active satellite operations.
- Unrelated refactors to model-first, bridge, PIT, analyzer, or general package layout areas that do not directly support provider bulk ingestion.

## Acceptance Criteria
- Consumers can submit ordered explicit bulk saves through IDataVaultSaveService using DataVaultBulkSaveRequest, and registry-backed callers can build the same ordered batch through DataVaultRegistryBulkSaveRequest.
- The default save pipeline preserves deterministic ordered processing and falls back to the provider-neutral writer whenever no registered provider strategy is eligible for the current DbContext and batch.
- Provider-native bulk dispatch is implemented and documented only for PostgreSQL, SQL Server, MySQL, and Oracle, with diagnostics-visible reasons for selection or fallback.
- Repository tests cover core bulk-save behavior, strategy selection and fallback, and the opt-in external-provider evidence lanes used to prove native provider behavior.
- README, release notes, and other relevant guidance document the public bulk-ingestion baseline, verification path, and intentionally deferred behavior.

## Definition of Done
- Core, provider, and test artifacts in the existing seven-package DVault family reflect the agreed bulk-ingestion contract and pass the relevant repository verification lanes.
- Provider-specific proof remains bounded to the documented opt-in external-provider test lanes driven by the existing DVAULT_TEST_* connection-string conventions.
- Public API surface, diagnostics messaging, benchmarks, examples, and release-note text are updated where the bulk-ingestion feature changes observable behavior.
- Any intentionally deferred capability or unsupported optimization case is explicitly documented instead of left ambiguous.

## Implementation Notes
- Use the existing v0.14.0 release-note contract as the ratified public baseline instead of reopening naming, provider set, or write-boundary decisions already visible in the repository.
- Repository evidence already shows the expected implementation slices: core save-service work in the main DVault package, provider strategy implementations in the PostgreSQL, SQL Server, MySQL, and Oracle packages, and corresponding test coverage in DataVaultSaveStrategySelectionTests, ExternalProviderBulkSaveAssertions, and provider-specific integration suites.
- SQLite remains part of the package family but is not a native bulk-ingestion target in this baseline; its supported path is the provider-neutral fallback.
- No persistent planning writes were made during this run.

## Open Questions
- none

## Follow-Up Questions
- Should a later release expose provider-specific native-dispatch thresholds and eligibility gates as configurable policy rather than fixed documented defaults?
- Should a later follow-up ticket add native bulk-ingestion support for additional providers or keep provider-neutral fallback as the long-term answer outside the current four-provider set?
- Do we want a separate future ticket for richer benchmark publication and comparison artifacts beyond the current benchmark evidence documented in the release notes?

## Risks
- External-provider proof depends on opt-in environment-specific connection strings, so native provider evidence can be skipped in default local or CI runs unless those lanes are explicitly configured.
- Benchmark and throughput claims are environment-sensitive; release notes should continue to tie performance evidence to recorded machine, provider, and execution context rather than promise universal gains.
- The epic spans core contracts, provider packages, diagnostics, tests, and documentation, so downstream implementation work should stay bounded to this release contract to avoid scope creep.

## Split Recommendations
- No mandatory split is required for PO-critic handoff because the repository already presents one coherent v0.14.0 epic baseline.
- If later execution tracking needs finer granularity, split along three bounded slices: core bulk-save contract and fallback behavior, provider-native strategy coverage and diagnostics, and documentation or benchmark evidence.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add explicit opt-in bulk ingestion paths for high-volume Data Vault writes.

## Scope
- Refine and complete the work for "Provider bulk ingestion" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.