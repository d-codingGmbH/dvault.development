<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story to the remaining benchmark-validity gap: keep the existing harness and artifacts, but require provider-native bulk rows to use provider-eligible write shapes and prove selected strategies before docs reuse the timings.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current ticket comments are only the bot claim and lease comments, the ticket has no persisted attachments, and no child ticket, relation change, attachment, or planning document was created in this pass.
- Repository evidence already fixes the v1 benchmark baseline: the existing benchmark project and benchmark README keep classic EF as the required local SQLite comparison lane, while optional external providers are DVault fallback versus provider-specific rows with deterministic skipped-row handling.
- The current external-provider matrix is not yet safe to treat as native-proof everywhere: MySQL, Oracle, and SQL Server optimized write rows can silently fall back on undersized batches, SQL Server also declines batches with more than 500 satellite operations, and only SQLite currently exposes a provider-specific read strategy.
- Existing relations remain coherent without cleanup: parent epic 06F2PGMFWSEC95ATBCGZ6HYT5W still owns this story, done blockers 06F2PGK4QJ0YGXK5479W83Z2J0 and 06F2PGNGVQ3TZZWSABAK5SNFK4 are historical ordering context, and docs task 06F2PGP2B2RZGGK3CVKK5WRRP8 remains the downstream documentation owner.

### Scope In
- Tighten the bulk-write benchmark matrix so any row presented as provider-native optimized bulk execution uses a provider-eligible request shape and proves the named strategy was selected.
- Preserve the existing required-local SQLite write comparisons for classic EF, AddDVault fallback, and AddDVaultSqlite optimized execution.
- Align optional external-provider native benchmark shapes with the current live bulk-test and strategy-gate baseline so native-versus-fallback timings stay comparable.
- Keep the benchmark artifact contract in scope: benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json remain the documentation-ready evidence surface.

### Scope Out
- New provider save-strategy algorithms, provider gate-threshold changes, or fallback semantic changes already owned by the completed implementation tickets.
- Live external-provider integration test implementation, which stays with done task 06F2PGNT7DF4DVNKYWDFZC8DEM.
- Read-model benchmark expansion or non-SQLite provider-specific read claims; this story stays on bulk-ingestion write evidence.
- Broader README, release-note, or adoption-document packaging beyond any narrow benchmark guidance needed to keep the evidence boundary accurate.

## Acceptance Criteria
- The required-local SQLite matrix continues to compare classic EF, AddDVault fallback, and AddDVaultSqlite optimized write paths for the shipped write scenarios.
- Any benchmark row labeled as provider-specific optimized bulk write must verify, through diagnostics or an equivalent explicit assertion, that the named provider strategy executed instead of the provider-neutral fallback writer.
- External-provider native write scenarios use request shapes that satisfy the current gates: clean DbContext, no multi-active satellites, SQL Server at least 50 total operations and at most 500 satellite operations, MySQL and Oracle at least 50 total operations, and a matching fallback comparison row on the same provider and request shape.
- Rows that intentionally remain fallback baseline or skipped are labeled as such in the benchmark artifacts, and non-SQLite read rows are not treated as provider-specific optimized evidence in this story.
- benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json continue to capture scenario, provider, execution status, skip reason, timing data, provider discovery state, and machine context together.
- Benchmark integration tests cover the strategy-selection proof and any adjusted matrix or artifact behavior needed by this story.

## Definition of Done
- Benchmark claims of provider-native bulk behavior can no longer silently time provider-neutral fallback execution.
- External-provider benchmark rows are comparable to the existing live bulk integration proof rather than using request shapes that the native strategy gates decline.
- The ticket stays bounded to write-path benchmark evidence, while broader documentation packaging remains with 06F2PGP2B2RZGGK3CVKK5WRRP8.
- No PO-blocking open questions remain before the ticket advances to PO-critic.

## Implementation Notes
- BenchmarkRunner.cs currently creates external-provider rows for customer-profile-history, customer-profile-bulk-insert-only, customer-profile-bulk-history, and order-product-fulfillment-history without diagnostics-based proof that the optimized strategy actually ran.
- ExternalProviderBulkSaveAssertions.cs already provides a bounded provider-eligible mixed hub, link, and satellite batch and asserts selected strategy names; reuse or mirror that baseline for native-provider benchmark rows instead of inventing a second shape.
- CustomerProfileBulkScenarios.ChangeHeavy is 100 customers times 10 profile states, so it exceeds SQL Server's current native limit of 500 satellite operations and should not be treated as native SQL Server benchmark evidence.
- Only SQLite registers a provider-specific read strategy through SqliteDataVaultReadStrategy.cs, so non-SQLite read rows should stay outside native bulk claims in this ticket.
- Current branch head 66d72e4a9 contains only ticket metadata work for this story; no child ticket was created, no relation was added or removed, no attachment was added, and no planning document was written during refinement.

## Open Questions
- none

## Follow-Up Questions
- Should docs task 06F2PGP2B2RZGGK3CVKK5WRRP8 publish any crossover guidance only from completed benchmark-summary artifacts so the provider and machine context stays attached to copied timings?
- If a future release adds non-SQLite provider read strategies or wants a broader scale-matrix publication, should that be tracked in a separate follow-on benchmark ticket instead of widening this bulk-ingestion story?

## Risks
- Without strategy-selection proof, benchmark output can mislabel fallback timings as provider-native results and create false performance claims.
- External-provider timings remain environment-sensitive because they depend on developer-managed databases and conditional provider dependencies, so downstream docs must preserve skip status and run context.
- Cross-provider comparisons will drift if benchmark request shapes stop matching the bounded native-strategy eligibility proven by the live bulk integration tests.

## Split Recommendations
- No additional split is recommended; the current graph already separates fallback implementation, native strategy implementation, provider integration coverage, benchmarks, and documentation.
- If future work needs read-strategy benchmarking or a materially broader benchmark matrix, open a fresh follow-on ticket instead of widening this story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Measure bulk ingestion against the classic and existing DVault save paths.

## Scope
- Refine and complete the work for "Benchmark fallback and native bulk ingestion" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.